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
        // 4795 4794 4785的设备信息 最后一个点位不连续
        public void ReadandSendDeviceInfo1(DeviceInfoConSturct_MC[] input, KeyenceMcNet mc, ref AllDataReadfromMC allDataReadfromMC, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
        {
            var ReadObject = input[0].varName;
            ushort length = (ushort)(input.Length - 1);
            var listWriteItem = new List<WriteItem>();

            var tempdata = new bool[input.Length];

            OperateResult<bool[]> ret = mc.ReadBool(ReadObject, length);
            OperateResult<bool> ret1 = mc.ReadBool(input[input.Length - 1].varName);

            if (ret.IsSuccess && ret1.IsSuccess)
            {
                Array.Copy(ret.Content, 0, tempdata, 0, ret.Content.Length);
                tempdata[ret.Content.Length] = ret1.Content;

                Array.Copy(tempdata, 0, allDataReadfromMC.DeviceInfoValue, 0, tempdata.Length);

                try
                {
                    listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary["DeviceInfo"], Arp.Type.Grpc.CoreType.CtArray, tempdata));
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

        public void ReadandSendDeviceInfo2(DeviceInfoConSturct_MC[] input, KeyenceMcNet mc, ref AllDataReadfromMC allDataReadfromMC, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
        {
            var ReadObject = input[0].varName;
            ushort length = (ushort)input.Length;
            var listWriteItem = new List<WriteItem>();

            OperateResult<bool[]> ret = mc.ReadBool(ReadObject, length);

            if (ret.IsSuccess)
            {
                Array.Copy(ret.Content, 0, allDataReadfromMC.DeviceInfoValue, 0, ret.Content.Length);

                try
                {
                    listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary["DeviceInfo"], Arp.Type.Grpc.CoreType.CtArray, ret.Content));
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

        //从大数组中取数，并发送工位数据
        public void SendStationData(StationInfoStruct_MC[] input, short[] EMArray, ref AllDataReadfromMC allDataReadfromMC, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
        {
          
            float[] senddata = new float[input.Length];
            string StationName_Now = CN2EN(input[0].stationName);
            var listWriteItem = new List<WriteItem>();


            for (int i = 0; i < input.Length; i++)
            {
                var index = input[i].varOffset - 5057; //硬编码，开始地址就是5057
                senddata[i] = (float)(EMArray[index] / Math.Pow(10, input[i].varMagnification));
            }

            //写入缓存区
            if (input[0].stationName == "加工工位(1A1B)")
            {
                Array.Copy(senddata, 0, allDataReadfromMC.Station1A1BInfoValue, 0, senddata.Length);

            }
            else
            {
                Array.Copy(senddata, 0, allDataReadfromMC.Station2A2BInfoValue, 0, senddata.Length);

            }


            try
            {
                listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary[StationName_Now], Arp.Type.Grpc.CoreType.CtArray, senddata));
                var writeItemsArray = listWriteItem.ToArray();
                var dataAccessServiceWriteRequest = grpcToolInstance.ServiceWriteRequestAddDatas(writeItemsArray);
                bool result = grpcToolInstance.WriteDataToDataAccessService(grpcDataAccessServiceClient, dataAccessServiceWriteRequest, new IDataAccessServiceWriteResponse(), options1);

            }
            catch (Exception e)
            {
                logNet.WriteError("[Grpc]", StationName_Now + " 数据发送失败：" + e);

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
        public void ReadandSendConOneSecData(OneSecInfoStruct_MC[] input, KeyenceMcNet mc, ref AllDataReadfromMC allDataReadfromMC, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
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

                    try
                    {
                        listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary[ReadObject], Arp.Type.Grpc.CoreType.CtArray, ret.Content));
                        var writeItemsArray = listWriteItem.ToArray();
                        var dataAccessServiceWriteRequest = grpcToolInstance.ServiceWriteRequestAddDatas(writeItemsArray);
                        bool result = grpcToolInstance.WriteDataToDataAccessService(grpcDataAccessServiceClient, dataAccessServiceWriteRequest, new IDataAccessServiceWriteResponse(), options1);

                    }
                    catch (Exception e)
                    {

                        logNet.WriteError("[Grpc]", ReadObject + " 数据发送失败：" + e);


                    }
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

                    try
                    {
                        listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary[ReadObject], Arp.Type.Grpc.CoreType.CtArray, ret.Content));
                        var writeItemsArray = listWriteItem.ToArray();
                        var dataAccessServiceWriteRequest = grpcToolInstance.ServiceWriteRequestAddDatas(writeItemsArray);
                        bool result = grpcToolInstance.WriteDataToDataAccessService(grpcDataAccessServiceClient, dataAccessServiceWriteRequest, new IDataAccessServiceWriteResponse(), options1);

                    }
                    catch (Exception e)
                    {
                        logNet.WriteError("[Grpc]", ReadObject + " 数据发送失败：" + e);

                    }
                }
                else
                {
                    logNet.WriteError("[MC]", ReadObject + " 数据读取失败");

                }
            }
        }

        //读取并发送 寿命管理数据  (需要根据地址筛选）
        public void ReadandSendDisOneSecData(OneSecInfoStruct_MC[] input, KeyenceMcNet mc, ref AllDataReadfromMC allDataReadfromMC, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
        {
            ushort[] senddata = new ushort[input.Length];
            var ReadObject = input[0].varName;
            ushort length = (ushort)(input[input.Length - 1].varOffset - input[0].varOffset + 1);
            var listWriteItem = new List<WriteItem>();

            OperateResult<ushort[]> ret = mc.ReadUInt16(ReadObject, length);

            if (ret.IsSuccess)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    var index = input[i].varOffset - input[0].varOffset;
                    senddata[i] = ret.Content[index];
                }

                Array.Copy(senddata, 0, allDataReadfromMC.LifeManagementValue, 0, senddata.Length);  //写入缓存区

                try
                {
                    listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary[ReadObject], Arp.Type.Grpc.CoreType.CtArray, senddata));
                    var writeItemsArray = listWriteItem.ToArray();
                    var dataAccessServiceWriteRequest = grpcToolInstance.ServiceWriteRequestAddDatas(writeItemsArray);
                    bool result = grpcToolInstance.WriteDataToDataAccessService(grpcDataAccessServiceClient, dataAccessServiceWriteRequest, new IDataAccessServiceWriteResponse(), options1);

                }
                catch (Exception e)
                {
                    logNet.WriteError("[Grpc]", ReadObject + " 数据发送失败：" + e);

                }
            }
            else
            {
                logNet.WriteError("[MC]", ReadObject + " 数据读取失败");

            }

        }

        //读取并发送报警数据
        public void ReadandSendAlarmData(OneSecAlarmStruct_MC[] input, KeyenceMcNet mc, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
        {
            var ReadObject = input[0].varName.Replace(".0", "");
            ushort length = 38; //硬编码 从DM8000-DM8037 
            bool[] senddata = new bool[length * 16];
            var listWriteItem = new List<WriteItem>();
            bool temp;

            OperateResult<byte[]> ret = mc.Read(ReadObject, length);

            if (ret.IsSuccess)
            {
                for (int i = 0; i < 38 * 16; i++)
                {
                    temp = mc.ByteTransform.TransBool(ret.Content, 0 + i);   // 每个bool 一个字节                 
                    senddata[i] = temp;
                }

                try
                {
                    listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary[ReadObject], Arp.Type.Grpc.CoreType.CtArray, senddata));
                    var writeItemsArray = listWriteItem.ToArray();
                    var dataAccessServiceWriteRequest = grpcToolInstance.ServiceWriteRequestAddDatas(writeItemsArray);
                    bool result = grpcToolInstance.WriteDataToDataAccessService(grpcDataAccessServiceClient, dataAccessServiceWriteRequest, new IDataAccessServiceWriteResponse(), options1);

                }
                catch (Exception e)
                {
                    logNet.WriteError("[Grpc]", ReadObject + " 数据发送失败：" + e);

                }
            }
            else
            {
                logNet.WriteError("[MC]", ReadObject + " 数据读取失败");
            }
        }

        #endregion


        #region 读取并发送点位名
        public void ReadandSendPointName(OneSecInfoStruct_MC[] InputStruct, OneSecPointNameStruct_IEC functionEnableNameStruct_IEC, int IEC_Array_Number, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
        {
            functionEnableNameStruct_IEC.iDataCount = InputStruct.Length;
            functionEnableNameStruct_IEC.stringArrData = new stringStruct[IEC_Array_Number];
            var listWriteItem = new List<WriteItem>();

            for (int i = 0; i < IEC_Array_Number; i++)
            {
                if (i < InputStruct.Length)
                {
                    functionEnableNameStruct_IEC.stringArrData[i].str = InputStruct[i].varAnnotation;
                }
                else
                {
                    functionEnableNameStruct_IEC.stringArrData[i].str = " ";
                }
            }
            try
            {
                listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary.GetValueOrDefault(InputStruct[0].varAnnotation), Arp.Type.Grpc.CoreType.CtStruct, functionEnableNameStruct_IEC));
                var writeItemsArray = listWriteItem.ToArray();
                var dataAccessServiceWriteRequest = grpcToolInstance.ServiceWriteRequestAddDatas(writeItemsArray);
                bool result = grpcToolInstance.WriteDataToDataAccessService(grpcDataAccessServiceClient, dataAccessServiceWriteRequest, new IDataAccessServiceWriteResponse(), options1);

            }
            catch (Exception e)
            {
                logNet.WriteError("[Grpc]", InputStruct[0].varAnnotation + " 点位名发送失败：" + e);
            }

        }

        public void ReadandSendPointName(OneSecAlarmStruct_MC[] InputStruct, OneSecPointNameStruct_IEC functionEnableNameStruct_IEC, int IEC_Array_Number, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
        {
            functionEnableNameStruct_IEC.iDataCount = InputStruct.Length;
            functionEnableNameStruct_IEC.stringArrData = new stringStruct[IEC_Array_Number];
            var listWriteItem = new List<WriteItem>();

            for (int i = 0; i < IEC_Array_Number; i++)
            {
                if (i < InputStruct.Length)
                {
                    functionEnableNameStruct_IEC.stringArrData[i].str = InputStruct[i].varAnnotation;
                }
                else
                {
                    functionEnableNameStruct_IEC.stringArrData[i].str = " ";
                }
            }
            try
            {
                listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary.GetValueOrDefault(InputStruct[0].varAnnotation), Arp.Type.Grpc.CoreType.CtStruct, functionEnableNameStruct_IEC));
                var writeItemsArray = listWriteItem.ToArray();
                var dataAccessServiceWriteRequest = grpcToolInstance.ServiceWriteRequestAddDatas(writeItemsArray);
                bool result = grpcToolInstance.WriteDataToDataAccessService(grpcDataAccessServiceClient, dataAccessServiceWriteRequest, new IDataAccessServiceWriteResponse(), options1);

            }
            catch (Exception e)
            {
                logNet.WriteError("[Grpc]", InputStruct[0].varAnnotation + " 点位名发送失败：" + e);
            }

        }

        public void ReadandSendPointName(StationInfoStruct_MC[] InputStruct, OneSecPointNameStruct_IEC functionEnableNameStruct_IEC, int IEC_Array_Number, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
        {
            functionEnableNameStruct_IEC.iDataCount = InputStruct.Length;
            functionEnableNameStruct_IEC.stringArrData = new stringStruct[IEC_Array_Number];
            var listWriteItem = new List<WriteItem>();

            for (int i = 0; i < IEC_Array_Number; i++)
            {
                if (i < InputStruct.Length)
                {
                    functionEnableNameStruct_IEC.stringArrData[i].str = InputStruct[i].varAnnotation;
                }
                else
                {
                    functionEnableNameStruct_IEC.stringArrData[i].str = " ";
                }
            }
            try
            {
                listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary.GetValueOrDefault(InputStruct[0].varAnnotation), Arp.Type.Grpc.CoreType.CtStruct, functionEnableNameStruct_IEC));
                var writeItemsArray = listWriteItem.ToArray();
                var dataAccessServiceWriteRequest = grpcToolInstance.ServiceWriteRequestAddDatas(writeItemsArray);
                bool result = grpcToolInstance.WriteDataToDataAccessService(grpcDataAccessServiceClient, dataAccessServiceWriteRequest, new IDataAccessServiceWriteResponse(), options1);

            }
            catch (Exception e)
            {
                logNet.WriteError("[Grpc]", InputStruct[0].varAnnotation + " 点位名发送失败：" + e);
            }

        }

        public void ReadandSendPointName(String[] InputString, OneSecPointNameStruct_IEC functionEnableNameStruct_IEC, int IEC_Array_Number, GrpcTool grpcToolInstance, Dictionary<string, string> nodeidDictionary, IDataAccessServiceClient grpcDataAccessServiceClient, CallOptions options1)
        {
            var listWriteItem = new List<WriteItem>();
            WriteItem[] writeItems = new WriteItem[] { };
            functionEnableNameStruct_IEC.iDataCount = InputString.Length;
            functionEnableNameStruct_IEC.stringArrData = new stringStruct[IEC_Array_Number];
            for (int i = 0; i < IEC_Array_Number; i++)
            {
                if (i < InputString.Length)
                {
                    functionEnableNameStruct_IEC.stringArrData[i].str = InputString[i];
                }
                else
                {
                    functionEnableNameStruct_IEC.stringArrData[i].str = " ";
                }
            }
            try
            {
                listWriteItem.Add(grpcToolInstance.CreatWriteItem(nodeidDictionary.GetValueOrDefault(InputString[0]), Arp.Type.Grpc.CoreType.CtStruct, functionEnableNameStruct_IEC));
                var writeItemsArray = listWriteItem.ToArray();
                var dataAccessServiceWriteRequest = grpcToolInstance.ServiceWriteRequestAddDatas(writeItemsArray);
                bool result = grpcToolInstance.WriteDataToDataAccessService(grpcDataAccessServiceClient, dataAccessServiceWriteRequest, new IDataAccessServiceWriteResponse(), options1);

            }
            catch (Exception e)
            {
                logNet.WriteError("[Grpc]", InputString[0] + " 点位名发送失败：" + e);
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

