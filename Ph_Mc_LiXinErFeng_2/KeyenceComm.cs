using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication;
using HslCommunication.Profinet.Omron;
using System.Threading;
using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Collections;
using Grpc.Core;
using static Arp.Plc.Gds.Services.Grpc.IDataAccessService;
using Arp.Plc.Gds.Services.Grpc;
using Grpc.Net.Client;
using static Ph_Mc_LiXinErFeng.GrpcTool;
using System.Net.Sockets;
using System.Drawing;
using Opc.Ua;
using NPOI.SS.Formula.Functions;
using HslCommunication.LogNet;
using Microsoft.Extensions.Logging;
using static Ph_Mc_LiXinErFeng.UserStruct;
using static Ph_Mc_LiXinErFeng.Program;
using HslCommunication.Profinet.LSIS;

using MathNet.Numerics.LinearAlgebra.Factorization;
using HslCommunication.Profinet.Keyence;
using System.Reflection;
using NPOI.Util;
using Org.BouncyCastle.Ocsp;
using static NPOI.HSSF.Util.HSSFColor;





namespace Ph_Mc_LiXinErFeng
{

    class KeyenceComm
    {


        #region 读取并发送设备信息
        //
        // 4795 4794 的设备信息 最后一个点位不连续
        public void ReadandSendDeviceInfo1(DeviceInfoConSturct_MC[] input, KeyenceMcNet mc, ref AllDataReadfromMC allDataReadfromMC, ref UDT_StationListlnfo StationListlnfo, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
        {
            var ReadObject = input[0].varName;
            ushort length = (ushort)(input.Length - 1);
            var listWriteItem = new List<WriteItem>();

            var tempdata = new bool[input.Length];

            OperateResult<bool[]> ret = mc.ReadBool(ReadObject, length);  //读 MR8500 - MR8515 的数据
            OperateResult<bool> ret1 = mc.ReadBool(input[input.Length - 1].varName);  //读 MR8610 
            StationListlnfo.iDataCount = (short)input.Length;

            if (ret.IsSuccess && ret1.IsSuccess)
            {
                Array.Copy(ret.Content, 0, tempdata, 0, ret.Content.Length);

                for (int i = 0; i < input.Length - 1; i++)   //写入 MR8500 - MR8515 的数据
                {
                    StationListlnfo.arrDataPoint[input[i].stationNumber - 1].xCellMem = ret.Content[i];
                }

                tempdata[ret.Content.Length] = ret1.Content;
                StationListlnfo.arrDataPoint[ret.Content.Length].xCellMem = ret1.Content;  //写入 MR8610 

                Array.Copy(tempdata, 0, allDataReadfromMC.DeviceInfoValue, 0, tempdata.Length);  //写入暂存区（写入Excel的采集值）


                try
                {
                    listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary["StationListlnfo"], Arp.Type.Grpc.CoreType.CtStruct, StationListlnfo));
                    var writeItemsArray = listWriteItem.ToArray();
                    var dataAccessServiceWriteRequest = grpcToolInstance.ServiceWriteRequestAddDatas(writeItemsArray);
                    bool result = grpcToolInstance.WriteDataToDataAccessService(grpcDataAccessServiceClient, dataAccessServiceWriteRequest, new IDataAccessServiceWriteResponse(), options1);

                }
                catch (Exception e)
                {
                    logNet.WriteError("[Grpc]", " 设备信息数据发送失败：" + e);

                }
            }
            else
            {
                logNet.WriteError("[MC]", ReadObject + " 数据读取失败");

            }
        }

        // 4752 的设备信息 点位连续
        public void ReadandSendDeviceInfo2(DeviceInfoConSturct_MC[] input, KeyenceMcNet mc, ref AllDataReadfromMC allDataReadfromMC, ref UDT_StationListlnfo StationListlnfo, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
        {
            var ReadObject = input[0].varName;
            ushort length = (ushort)input.Length;
            var listWriteItem = new List<WriteItem>();
            StationListlnfo.iDataCount = (short)input.Length;

            OperateResult<bool[]> ret = mc.ReadBool(ReadObject, length);

            if (ret.IsSuccess)
            {
                Array.Copy(ret.Content, 0, allDataReadfromMC.DeviceInfoValue, 0, ret.Content.Length);  //写入暂存区（写入Excel的采集值）

                for (int i = 0; i < input.Length; i++)   //写入 MR8500 - MR8600 的数据
                {
                    StationListlnfo.arrDataPoint[input[i].stationNumber - 1].xCellMem = ret.Content[i];
                }

                try
                {
                    listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary["StationListlnfo"], Arp.Type.Grpc.CoreType.CtStruct, StationListlnfo));
                    var writeItemsArray = listWriteItem.ToArray();
                    var dataAccessServiceWriteRequest = grpcToolInstance.ServiceWriteRequestAddDatas(writeItemsArray);
                    bool result = grpcToolInstance.WriteDataToDataAccessService(grpcDataAccessServiceClient, dataAccessServiceWriteRequest, new IDataAccessServiceWriteResponse(), options1);

                }
                catch (Exception e)
                {
                    logNet.WriteError("[Grpc]", " 设备信息数据发送失败：" + e);

                }
            }
            else
            {
                logNet.WriteError("[MC]", ReadObject + " 数据读取失败");

            }
        }


        #endregion

        //从三个大数组中取数，并发送工位数据

        public void WriteStationData(StationInfoStruct_MC[] input, short[] EMArray, short[] DMArray, bool[] MRArray,  ref AllDataReadfromMC allDataReadfromMC, ref UDT_ProcessStationDataValue ProcessStationDataValue)
        {
            var index = 0;
            var j = 0;  // 加工工位采集值的索引
            double temp = 0;
            // 根据所属工位号，判断数组索引
            switch (input[0].stationName)
            {
                case "加工工位(1A)":
                    {
                        j = 0;

                        ProcessStationDataValue.arrDataPoint[j].iDataCount = (short)input.Length;

                        for (int i = 0; i < input.Length; i++)
                        {
                            if (input[i].varName.Substring(0, 2) == "EM")
                            {
                                index = input[i].varOffset - 5057;
                                temp = EMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();   // 写入 加工工位采集值结构体 
                                allDataReadfromMC.Station1AInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "DM")
                            {
                                index = input[i].varOffset - 9500;
                                temp = DMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station1AInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "MR")
                            {
                                index = CalculateIndex_H(6008, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = MRArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station1AInfoValue[i] = MRArray[index] ? "1" : "0";    // 写入数据暂存区（Excel）
                            }
                           
                        }

                    }
                    break;

                case "加工工位(1B)":
                    {
                        j = 1;

                        ProcessStationDataValue.arrDataPoint[j].iDataCount = (short)input.Length;

                        for (int i = 0; i < input.Length; i++)
                        {
                            if (input[i].varName.Substring(0, 2) == "EM")
                            {

                                index = input[i].varOffset - 5057;
                                temp = EMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();   // 写入 加工工位采集值结构体 
                                allDataReadfromMC.Station1BInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "DM")
                            {
                                index = input[i].varOffset - 9500;
                                temp = DMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station1BInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "MR")
                            {
                                index = CalculateIndex_H(6008, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = MRArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station1BInfoValue[i] = MRArray[index] ? "1" : "0";    // 写入数据暂存区（Excel）
                            }

                        }


                    }
                    break;

                case "加工工位(2A)":
                    {
                        j = 2;

                        ProcessStationDataValue.arrDataPoint[j].iDataCount = (short)input.Length;

                        for (int i = 0; i < input.Length; i++)
                        {
                            if (input[i].varName.Substring(0, 2) == "EM")
                            {
                                index = input[i].varOffset - 5057;
                                temp = EMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();   // 写入 加工工位采集值结构体 
                                allDataReadfromMC.Station2AInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "DM")
                            {
                                index = input[i].varOffset - 9500;
                                temp = DMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station2AInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "MR")
                            {
                                index = CalculateIndex_H(6008, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = MRArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station2AInfoValue[i] = MRArray[index] ? "1" : "0";    // 写入数据暂存区（Excel）
                            }

                          


                        }


                    }
                    break;

                case "加工工位(2B)":
                    {
                        j = 3;

                        ProcessStationDataValue.arrDataPoint[j].iDataCount = (short)input.Length;

                        for (int i = 0; i < input.Length; i++)
                        {
                            if (input[i].varName.Substring(0, 2) == "EM")
                            {

                                index = input[i].varOffset - 5057;
                                temp = EMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();   // 写入 加工工位采集值结构体 
                                allDataReadfromMC.Station2BInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）


                            }
                            else if (input[i].varName.Substring(0, 2) == "DM")
                            {
                                index = input[i].varOffset - 9500;
                                temp = DMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station2BInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "MR")
                            {
                                index = CalculateIndex_H(6008, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = MRArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station2BInfoValue[i] = MRArray[index] ? "1" : "0";    // 写入数据暂存区（Excel）
                            }

                           


                        }


                    }
                    break;

                default:
                    break;

            }
        }

        public void WriteStationData(StationInfoStruct_MC[] input, short[] EMArray, short[] DMArray, bool[] MRArray, bool[] RArray, ref AllDataReadfromMC allDataReadfromMC, ref UDT_ProcessStationDataValue ProcessStationDataValue)
        {
            var index = 0;
            var j = 0;  // 加工工位采集值的索引
            double temp = 0;
            // 根据所属工位号，判断数组索引
            switch (input[0].stationName)
            {
                case "加工工位(1A)":
                    {
                        j = 0;

                        ProcessStationDataValue.arrDataPoint[j].iDataCount = (short)input.Length;

                        for (int i = 0; i < input.Length; i++)
                        {
                            if (input[i].varName.Substring(0, 2) == "EM")
                            {
                                index = input[i].varOffset - 5057;
                                temp = EMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();   // 写入 加工工位采集值结构体 
                                allDataReadfromMC.Station1AInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "DM")
                            {
                                index = input[i].varOffset - 9500;
                                temp = DMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station1AInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "MR")
                            {
                                index = CalculateIndex_H(6008, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = MRArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station1AInfoValue[i] = MRArray[index].ToString();    // 写入数据暂存区（Excel）
                            }
                            else if (input[i].varName.Substring(0, 2) == "R7")  //设备4752独有的
                            {
                                index = CalculateIndex_H(79003, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = RArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station1AInfoValue[i] = RArray[index] ? "1" : "0";    // 写入数据暂存区（Excel）
                            }
                        }

                    }
                    break;

                case "加工工位(1B)":
                    {
                        j = 1;

                        ProcessStationDataValue.arrDataPoint[j].iDataCount = (short)input.Length;

                        for (int i = 0; i < input.Length; i++)
                        {
                            if (input[i].varName.Substring(0, 2) == "EM")
                            {

                                index = input[i].varOffset - 5057;
                                temp = EMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();   // 写入 加工工位采集值结构体 
                                allDataReadfromMC.Station1BInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "DM")
                            {
                                index = input[i].varOffset - 9500;
                                temp = DMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station1BInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "MR")
                            {
                                index = CalculateIndex_H(6008, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = MRArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station1BInfoValue[i] = MRArray[index] ? "1" : "0";    // 写入数据暂存区（Excel）
                            }

                            else if (input[i].varName.Substring(0, 2) == "R7")  //设备4752独有的
                            {
                                index = CalculateIndex_H(79003, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = RArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station1BInfoValue[i] = RArray[index] ? "1" : "0";    // 写入数据暂存区（Excel）
                            }
                        }


                    }
                    break;

                case "加工工位(2A)":
                    {
                        j = 2;

                        ProcessStationDataValue.arrDataPoint[j].iDataCount = (short)input.Length;

                        for (int i = 0; i < input.Length; i++)
                        {
                            if (input[i].varName.Substring(0, 2) == "EM")
                            {

                                index = input[i].varOffset - 5057;
                                temp = EMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();   // 写入 加工工位采集值结构体 
                                allDataReadfromMC.Station2AInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "DM")
                            {
                                index = input[i].varOffset - 9500;
                                temp = DMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station2AInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "MR")
                            {
                                index = CalculateIndex_H(6008, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = MRArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station2AInfoValue[i] = MRArray[index] ? "1" : "0";    // 写入数据暂存区（Excel）
                            }

                            else if (input[i].varName.Substring(0, 2) == "R7")  //设备4752独有的
                            {
                                index = CalculateIndex_H(79003, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = RArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station2AInfoValue[i] = RArray[index] ? "1" : "0";    // 写入数据暂存区（Excel）
                            }


                        }


                    }
                    break;

                case "加工工位(2B)":
                    {
                        j = 3;

                        ProcessStationDataValue.arrDataPoint[j].iDataCount = (short)input.Length;

                        for (int i = 0; i < input.Length; i++)
                        {
                            if (input[i].varName.Substring(0, 2) == "EM")
                            {

                                index = input[i].varOffset - 5057;
                                temp = EMArray[index] / Math.Pow(10, input[i].varMagnification);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = temp.ToString();   // 写入 加工工位采集值结构体 
                                allDataReadfromMC.Station2BInfoValue[i] = temp.ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "DM")
                            {
                                index = input[i].varOffset - 9500;
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = DMArray[index].ToString();    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station2BInfoValue[i] = DMArray[index].ToString();    // 写入数据暂存区（Excel）

                            }
                            else if (input[i].varName.Substring(0, 2) == "MR")
                            {
                                index = CalculateIndex_H(6008, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = MRArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station2BInfoValue[i] = MRArray[index] ? "1" : "0";    // 写入数据暂存区（Excel）
                            }

                            else if (input[i].varName.Substring(0, 2) == "R7")  //设备4752独有的
                            {
                                index = CalculateIndex_H(79003, input[i].varOffset);
                                ProcessStationDataValue.arrDataPoint[j].arrDataPoint[i].StringValue = RArray[index] ? "1" : "0";    // 写入 加工工位采集值结构体
                                allDataReadfromMC.Station2BInfoValue[i] = RArray[index] ? "1" : "0";    // 写入数据暂存区（Excel）
                            }


                        }


                    }
                    break;

                default:
                    break;

            }
        }


        #region 读1000ms的数据

        //读取OEE数据
        public bool[] ReadOEEData(OneSecInfoStruct_MC[] input, KeyenceMcNet mc)
        {
            var ReadObject = input[0].varName;
            ushort length = (ushort)input.Length;

            OperateResult<bool[]> ret = mc.ReadBool(ReadObject, length);

            if (ret.IsSuccess)
            {
                return ret.Content;
            }
            else
            {
                logNet.WriteError("[MC]", ReadObject + " 数据读取失败");

                return null;
            }
        }


        //读取并发送 功能开关、生产统计数据 
        public void ReadConOneSecData(OneSecInfoStruct_MC[] input, KeyenceMcNet mc, ref AllDataReadfromMC allDataReadfromMC, ref DeviceDataStruct_IEC DeviceDataStruct)
        {
            var ReadObject = input[0].varName;
            ushort length = (ushort)input.Length;
            var listWriteItem = new List<WriteItem>();

            if (input[0].varType == "BOOL")  //读功能开关
            {
                OperateResult<bool[]> ret = mc.ReadBool(ReadObject, length);

                if (ret.IsSuccess)
                {

                    Array.Copy(ret.Content, 0, allDataReadfromMC.FunctionEnableValue, 0, ret.Content.Length);  //写入缓存区

                    Array.Copy(ret.Content, 0, DeviceDataStruct.Value_FE, 0, input.Length);  //写入 DeviceDataStruct 结构体


                }
                else
                {

                    logNet.WriteError("[MC]", ReadObject + " 数据读取失败");

                }
            }
            else   //读生产统计
            {
                OperateResult<short[]> ret = mc.ReadInt16(ReadObject, length);

                if (ret.IsSuccess)
                {
                    Array.Copy(ret.Content, 0, allDataReadfromMC.ProductionDataValue, 0, ret.Content.Length);  //写入缓存区
                    Array.Copy(ret.Content, 0, DeviceDataStruct.Value_PD, 0, ret.Content.Length); //写入 DeviceDataStruct 结构体

                }
                else
                {
                    logNet.WriteError("[MC]", ReadObject + " 数据读取失败");

                }
            }
        }

        //读取并发送 寿命管理数据  (需要根据地址筛选）
        public void ReadDisOneSecData(OneSecInfoStruct_MC[] input, KeyenceMcNet mc, ref AllDataReadfromMC allDataReadfromMC, ref DeviceDataStruct_IEC DeviceDataStruct)
        {
            //uint[] senddata = new uint[input.Length];
            var ReadObject = input[0].varName;
            ushort length = (ushort)(input[input.Length - 1].varOffset - input[0].varOffset + 1);
            var listWriteItem = new List<WriteItem>();

            OperateResult<ushort[]> ret = mc.ReadUInt16(ReadObject, length);

            if (ret.IsSuccess)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    var index = input[i].varOffset - input[0].varOffset;
                    //senddata[i] = (uint)ret.Content[index];
                    allDataReadfromMC.LifeManagementValue[i] = ret.Content[index];  //写入缓存区
                    DeviceDataStruct.Value_LM[i] = ret.Content[index];//写入 DeviceDataStruct 结构体

                }

                //Array.Copy(senddata, 0, allDataReadfromMC.LifeManagementValue, 0, senddata.Length);  //写入缓存区
                //Array.Copy(senddata, 0, DeviceDataStruct.Value_LM, 0, senddata.Length); //写入 DeviceDataStruct 结构体

            }
            else
            {
                logNet.WriteError("[MC]", ReadObject + " 数据读取失败");

            }

        }

        //读取并发送报警数据
        public void ReadAlarmData(OneSecAlarmStruct_MC[] input, KeyenceMcNet mc, ref DeviceDataStruct_IEC DeviceDataStruct)
        {
            var ReadObject = input[0].varName.Replace(".0", "");
            ushort length = 38; //硬编码 从DM8000-DM8037 

            bool temp;

            OperateResult<byte[]> ret = mc.Read(ReadObject, length);

            if (ret.IsSuccess)
            {
                for (int i = 0; i < 38 * 16; i++)
                {
                    temp = mc.ByteTransform.TransBool(ret.Content, 0 + i);   // 每个bool 一个字节
                    DeviceDataStruct.Value_ALM[i] = temp;          //写入 DeviceDataStruct 结构体
                    //senddata[i] = temp;
                }


            }
            else
            {
                logNet.WriteError("[MC]", ReadObject + " 数据读取失败");
            }
        }

        #endregion


        #region 读取并发送点位名
        // 读取 功能开关、寿命管理、生产统计的点位名
        public void ReadPointName(OneSecInfoStruct_MC[] InputStruct, ref OneSecPointNameStruct_IEC OneSecNameStruct)
        {
            switch (InputStruct[0].varName)
            {
                case "MR8800":             // 功能开关
                    {
                        OneSecNameStruct.DataCount_FE = InputStruct.Length;
                        for (int i = 0; i < InputStruct.Length; i++)
                        {
                            OneSecNameStruct.Name_FE[i].StringValue = InputStruct[i].varAnnotation;
                        }
                    }
                    break;

                case "DM9400":  // 生产统计
                    {
                        OneSecNameStruct.DataCount_PD = InputStruct.Length;
                        for (int i = 0; i < InputStruct.Length; i++)
                        {
                            OneSecNameStruct.Name_PD[i].StringValue = InputStruct[i].varAnnotation;
                        }
                    }
                    break;

                case "DM9000":  // 寿命管理
                    {
                        OneSecNameStruct.DataCount_LM = InputStruct.Length;
                        for (int i = 0; i < InputStruct.Length; i++)
                        {
                            OneSecNameStruct.Name_LM[i].StringValue = InputStruct[i].varAnnotation;
                        }
                    }
                    break;

                default:
                    break;
            }

        }

        //读报警信息的点位名
        public void ReadPointName(OneSecAlarmStruct_MC[] InputStruct, ref OneSecPointNameStruct_IEC OneSecNameStruct)
        {
            OneSecNameStruct.DataCount_ALM = InputStruct.Length;
            for (int i = 0; i < InputStruct.Length; i++)
            {
                OneSecNameStruct.Name_ALM[i].StringValue = InputStruct[i].varAnnotation;
            }
        }

        //读OEE的点位名
        public void ReadPointName(List<OneSecInfoStruct_MC[]> OEEGroups, int stringnumber, ref OneSecPointNameStruct_IEC OneSecNameStruct)
        {
            var index = 0;
            OneSecNameStruct.DataCount_OEE = stringnumber;
            foreach (var OEEGroup in OEEGroups)
            {
                foreach (var OEE in OEEGroup)
                {
                    OneSecNameStruct.Name_OEE[index++].StringValue = OEE.varAnnotation;
                }
            }
        }

        //读加工工位点位名
        public void ReadPointName(List<StationInfoStruct_MC[]> StationDataStruct, ref ProcessStationNameStruct_IEC ProcessStationNameStruct)
        {
            ProcessStationNameStruct.StationCount = (short)StationDataStruct.Count;   //写入加工工位的个数

            var i = 0;  //工位数量的索引

            foreach (var StationData in StationDataStruct)
            {
                ProcessStationNameStruct.UnitStation[i].DataCount = (short)StationData.Length;   //每个加工工位的点位数量 （不超过16个点位）
                ProcessStationNameStruct.UnitStation[i].StationNO = (short)StationData[0].StationNumber;
                ProcessStationNameStruct.UnitStation[i].StationName = StationData[0].stationName;
                var j = 0;  //每个工位里采集值的索引
                foreach (var item in StationData)
                {
                    ProcessStationNameStruct.UnitStation[i].arrKey[j].StringValue = item.varAnnotation;
                    j++;
                }
                i++;
            }
        }
        #endregion



        //根据首地址和偏移地址，得出数据在所读数据中的索引， 起始地址为21偏移地址为 4101   数组索引为 （41-21）*16 +（01 -00 ）
        //适用于  LR MR R的软件元
        public int CalculateIndex_H(int startaddress, int offset)
        {
            // ep: 偏移地址 205401 拆分成 2054 和 01
            var start_x = startaddress % 100; //取前半部分
            var start_y = startaddress / 100; //取后半部分

            var offset_x = offset % 100; //取前半部分
            var offset_y = offset / 100; //取后半部分

            int index = (offset_y - start_y) * 16 + (offset_x - start_x);

            return index;
        }


        //XML标签转换 工位结构体数组的工位名是中文，为了方便XML与字典对应，需要转化为英文
        private string CN2EN(string NameCN)
        {
            string NameEN = "";

            switch (NameCN)
            {
                case "加工工位(1A1B)":
                    NameEN = "1A1B";
                    break;

                case "加工工位(2A2B)":
                    NameEN = "2A2B";
                    break;

                default:
                    break;

            }

            return NameEN;

        }




    }
}

