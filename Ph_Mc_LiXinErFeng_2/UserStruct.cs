using Arp.Type.Grpc;
using Google.Protobuf.WellKnownTypes;
using HslCommunication;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ph_Mc_LiXinErFeng
{
    public class UserStruct
    {

        #region getTypeStruct

        public TypeStruct getTypeStruct(object stru)
        {
            TypeStruct structV = new TypeStruct();


            if (stru.GetType() == typeof(UDT_StationInfo))
            {
                UDT_StationInfo StructValue = (UDT_StationInfo)stru;
                structV = new UserStruct().getTypeStruct_UDT_StationInfo(StructValue);
            }

            if (stru.GetType() == typeof(testStruct))
            {
                testStruct StructValue = (testStruct)stru;
                structV = new UserStruct().getTypeStruct_testStruct(StructValue);
            }

            if (stru.GetType() == typeof(OneSecPointNameStruct_IEC))
            {
                OneSecPointNameStruct_IEC StructValue = (OneSecPointNameStruct_IEC)stru;
                structV = new UserStruct().getTypeStruct_OneSecPointNameStruct_IEC(StructValue);
            }

            if (stru.GetType() == typeof(stringStruct))
            {
                stringStruct StructValue = (stringStruct)stru;
                structV = new UserStruct().getTypeStruct_stringStruct(StructValue);
            }

            if (stru.GetType() == typeof(DeviceInfoStruct_IEC))
            {
                DeviceInfoStruct_IEC StructValue = (DeviceInfoStruct_IEC)stru;
                structV = new UserStruct().getTypeStruct_DeviceInfoStruct_IEC(StructValue);
            }

            if (stru.GetType() == typeof(UDT_StationUnit))
            {
                UDT_StationUnit StructValue = (UDT_StationUnit)stru;
                structV = new UserStruct().getTypeStruct_UDT_StationUnit(StructValue);
            }

            if (stru.GetType() == typeof(UDT_StationListlnfo))
            {
                UDT_StationListlnfo StructValue = (UDT_StationListlnfo)stru;
                structV = new UserStruct().getTypeStruct_UDT_StationListlnfo(StructValue);
            }

            if (stru.GetType() == typeof(UnitStationNameStruct_IEC))
            {
                UnitStationNameStruct_IEC StructValue = (UnitStationNameStruct_IEC)stru;
                structV = new UserStruct().getTypeStruct_UnitStationNameStruct_IEC(StructValue);
            }

            if (stru.GetType() == typeof(ProcessStationNameStruct_IEC))
            {
                ProcessStationNameStruct_IEC StructValue = (ProcessStationNameStruct_IEC)stru;
                structV = new UserStruct().getTypeStruct_ProcessStationNameStruct_IEC(StructValue);
            }

            if (stru.GetType() == typeof(StationInfoStruct_IEC))
            {
                StationInfoStruct_IEC StructValue = (StationInfoStruct_IEC)stru;
                structV = new UserStruct().getTypeStruct_StationInfoStruct_IEC(StructValue);
            }

            if (stru.GetType() == typeof(UDT_DataValue))
            {
                UDT_DataValue StructValue = (UDT_DataValue)stru;
                structV = new UserStruct().getTypeStruct_UDT_DataValue(StructValue);
            }

            if (stru.GetType() == typeof(UDT_ProcessStationDataValue))
            {
                UDT_ProcessStationDataValue StructValue = (UDT_ProcessStationDataValue)stru;
                structV = new UserStruct().getTypeStruct_UDT_ProcessStationDataValue(StructValue);
            }

            if (stru.GetType() == typeof(DeviceDataStruct_IEC))
            {
                DeviceDataStruct_IEC StructValue = (DeviceDataStruct_IEC)stru;
                structV = new UserStruct().getTypeStruct_DeviceDataStruct_IEC(StructValue);
            }

            if (stru.GetType() == typeof(DeviceInfoStructList_IEC))
            {
                DeviceInfoStructList_IEC StructValue = (DeviceInfoStructList_IEC)stru;
                structV = new UserStruct().getTypeStruct_DeviceInfoStructList_IEC(StructValue);
            }

            /////  加
            return structV;

        }

        #endregion 


        #region getTypeArrray
        public TypeArray getTypeArrray(object Arr)
        {
            TypeArray ArrayV = new TypeArray();

            if (Arr.GetType() == typeof(float[]))
            {
                float[] floatArr = (float[])Arr;

                foreach (float f in floatArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.FloatValue = f;
                    objectType.TypeCode = CoreType.CtReal32;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(Int32[]))
            {
                Int32[] dintArr = (Int32[])Arr;

                foreach (Int32 f in dintArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.Int32Value = f;
                    objectType.TypeCode = CoreType.CtInt32;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(Int16[]))
            {
                Int16[] dintArr = (Int16[])Arr;

                foreach (Int16 f in dintArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.Int16Value = f;
                    objectType.TypeCode = CoreType.CtInt16;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(UInt16[]))
            {
                UInt16[] dintArr = (UInt16[])Arr;

                foreach (UInt16 f in dintArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.Uint16Value = f;
                    objectType.TypeCode = CoreType.CtUint16;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(UInt32[]))
            {
                UInt32[] dintArr = (UInt32[])Arr;

                foreach (UInt32 f in dintArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.Uint32Value = f;
                    objectType.TypeCode = CoreType.CtUint32;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(string[]))
            {
                string[] stringArr = (string[])Arr;

                foreach (string s in stringArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StringValue = s.PadRight(85, ' ');
                    objectType.TypeCode = CoreType.CtString;
                    ArrayV.ArrayElements.Add(objectType);
                }
            }

            if (Arr.GetType() == typeof(bool[]))
            {
                bool[] boolArr = (bool[])Arr;

                foreach (bool f in boolArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.BoolValue = f;
                    objectType.TypeCode = CoreType.CtBoolean;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(UDT_StationInfo[]))
            {
                UDT_StationInfo[] testStructArr = (UDT_StationInfo[])Arr;

                foreach (UDT_StationInfo f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_UDT_StationInfo(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(testStruct[]))
            {
                testStruct[] testStructArr = (testStruct[])Arr;

                foreach (testStruct f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_testStruct(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(stringStruct[]))
            {
                stringStruct[] testStructArr = (stringStruct[])Arr;

                foreach (stringStruct f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_stringStruct(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(DeviceInfoStruct_IEC[]))
            {
                DeviceInfoStruct_IEC[] testStructArr = (DeviceInfoStruct_IEC[])Arr;

                foreach (DeviceInfoStruct_IEC f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_DeviceInfoStruct_IEC(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(OneSecPointNameStruct_IEC[]))
            {
                OneSecPointNameStruct_IEC[] testStructArr = (OneSecPointNameStruct_IEC[])Arr;

                foreach (OneSecPointNameStruct_IEC f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_OneSecPointNameStruct_IEC(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(UDT_StationUnit[]))
            {
                UDT_StationUnit[] testStructArr = (UDT_StationUnit[])Arr;

                foreach (UDT_StationUnit f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_UDT_StationUnit(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(UDT_StationListlnfo[]))
            {
                UDT_StationListlnfo[] testStructArr = (UDT_StationListlnfo[])Arr;

                foreach (UDT_StationListlnfo f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_UDT_StationListlnfo(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(UnitStationNameStruct_IEC[]))
            {
                UnitStationNameStruct_IEC[] testStructArr = (UnitStationNameStruct_IEC[])Arr;

                foreach (UnitStationNameStruct_IEC f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_UnitStationNameStruct_IEC(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(ProcessStationNameStruct_IEC[]))
            {
                ProcessStationNameStruct_IEC[] testStructArr = (ProcessStationNameStruct_IEC[])Arr;

                foreach (ProcessStationNameStruct_IEC f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_ProcessStationNameStruct_IEC(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(StationInfoStruct_IEC[]))
            {
                StationInfoStruct_IEC[] testStructArr = (StationInfoStruct_IEC[])Arr;

                foreach (StationInfoStruct_IEC f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_StationInfoStruct_IEC(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(UDT_DataValue[]))
            {
                UDT_DataValue[] testStructArr = (UDT_DataValue[])Arr;

                foreach (UDT_DataValue f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_UDT_DataValue(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(UDT_ProcessStationDataValue[]))
            {
                UDT_ProcessStationDataValue[] testStructArr = (UDT_ProcessStationDataValue[])Arr;

                foreach (UDT_ProcessStationDataValue f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_UDT_ProcessStationDataValue(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }

            if (Arr.GetType() == typeof(DeviceDataStruct_IEC[]))
            {
                DeviceDataStruct_IEC[] testStructArr = (DeviceDataStruct_IEC[])Arr;

                foreach (DeviceDataStruct_IEC f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_DeviceDataStruct_IEC(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }
            if (Arr.GetType() == typeof(DeviceInfoStructList_IEC[]))
            {
                DeviceInfoStructList_IEC[] testStructArr = (DeviceInfoStructList_IEC[])Arr;

                foreach (DeviceInfoStructList_IEC f in testStructArr)
                {
                    ObjectType objectType = new ObjectType();
                    objectType.StructValue = getTypeStruct_DeviceInfoStructList_IEC(f);
                    objectType.TypeCode = CoreType.CtStruct;
                    ArrayV.ArrayElements.Add(objectType);
                }

            }



            return ArrayV;

        }
        #endregion


        #region stringStruct
        public struct stringStruct
        {
            public string StringValue;
        }
        public TypeStruct getTypeStruct_stringStruct(stringStruct StructValue)
        {
            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.StringValue = StructValue.StringValue;
            v0.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v0);

            return structV;
        }
        #endregion


        #region UDT_StationInfo
        //每个工位的信息
        public struct UDT_StationInfo
        {
            public bool xCellMem; //电芯记忆信号（当前工位有电芯，用于组合电芯条码做条码转移和参数绑定）
            public bool xCellMemClear; //电芯记忆清除信号
            public bool xStationBusy; //工位加工中信号（用于触发100ms参数采集，短于）
            public string strCellCode; //电芯条码
            public string strPoleEarCode; //极耳码
            public bool xIsProcessStation; //是否加工工位  
        }

        public TypeStruct getTypeStruct_UDT_StationInfo(UDT_StationInfo StructValue)
        {
            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.BoolValue = StructValue.xCellMem;
            v0.TypeCode = CoreType.CtBoolean;
            structV.StructElements.Add(v0);

            ObjectType v1 = new ObjectType();
            v1.BoolValue = StructValue.xCellMemClear;
            v1.TypeCode = CoreType.CtBoolean;
            structV.StructElements.Add(v1);

            ObjectType v2 = new ObjectType();
            v2.BoolValue = StructValue.xStationBusy;
            v2.TypeCode = CoreType.CtBoolean;
            structV.StructElements.Add(v2);

            ObjectType v3 = new ObjectType();
            v3.StringValue = StructValue.strCellCode;
            v3.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v3);

            ObjectType v4 = new ObjectType();
            v4.StringValue = StructValue.strPoleEarCode;
            v4.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v4);

            ObjectType v5 = new ObjectType();
            v5.BoolValue = StructValue.xIsProcessStation;
            v5.TypeCode = CoreType.CtBoolean;
            structV.StructElements.Add(v5);

            return structV;
        }
        #endregion


        #region testStruct
        public struct testStruct
        {
            public bool xCellMem; //电芯记忆信号（当前工位有电芯，用于组合电芯条码做条码转移和参数绑定）
            public bool xCellMemClear; //电芯记忆清除信号
            public bool xStationBusy; //工位加工中信号（用于触发100ms参数采集，短于）
            public string strCellCode; //电芯条码
            public string strPoleEarCode; //极耳码
            public bool xIsProcessStation; //是否加工工位  
            public float[] floatArr;
        }

        public TypeStruct getTypeStruct_testStruct(testStruct StructValue)
        {
            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.BoolValue = StructValue.xCellMem;
            v0.TypeCode = CoreType.CtBoolean;
            structV.StructElements.Add(v0);

            ObjectType v1 = new ObjectType();
            v1.BoolValue = StructValue.xCellMemClear;
            v1.TypeCode = CoreType.CtBoolean;
            structV.StructElements.Add(v1);

            ObjectType v2 = new ObjectType();
            v2.BoolValue = StructValue.xStationBusy;
            v2.TypeCode = CoreType.CtBoolean;
            structV.StructElements.Add(v2);

            ObjectType v3 = new ObjectType();
            v3.StringValue = StructValue.strCellCode;
            v3.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v3);

            ObjectType v4 = new ObjectType();
            v4.StringValue = StructValue.strPoleEarCode;
            v4.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v4);

            ObjectType v5 = new ObjectType();
            v5.BoolValue = StructValue.xIsProcessStation;
            v5.TypeCode = CoreType.CtBoolean;
            structV.StructElements.Add(v5);

            ObjectType v6 = new ObjectType();
            v6.ArrayValue = getTypeArrray(StructValue.floatArr);
            v6.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v6);

            return structV;
        }

        #endregion





        #region 从Excel中抽象出来的数据类型

        //设备信息（47个）
        public struct DeviceInfoConSturct_MC
        {
            public string varType;           // 数据类型
            public string varName;           // 标签名
            public string stationName;       // 工位名
            public int varOffset;            // 地址偏移量
            public int stationNumber;        // 工位对应序号
        }

        //加工工位（4个）
        public struct StationInfoStruct_MC
        {
            public string stationName;       // 工位名    
            public string varType;           // 变量类型
            public string varName;           // 变量名
            public string varAnnotation;     // 描述
            public int varOffset;            // 地址偏移量
            public int varMagnification;     // 倍率
            public int StationNumber;     //加工工位所属工位号
        }

        //1000ms数据
        public struct OneSecInfoStruct_MC
        {
            public string varType;           // 变量类型
            public string varName;           // 变量名
            public string varAnnotation;     // 描述
            public int varOffset;            // 地址偏移量

        }

        public struct OneSecAlarmStruct_MC
        {
            public string varType;          // 变量类型
            public string varName;          // 变量名
            public string varAnnotation;    // 描述
            public double varOffset;        // 地址偏移量

        }


        #endregion


        #region 设备总览信息（单sheet）
        public struct DeviceInfoStruct_IEC
        {
            public string strDeviceName; //设备名称
            public string strDeviceCode; //设备编号
            public int iStationCount; //工位数量
            public string strPLCType; //PLC型号
            public string strProtocol; //通讯协议
            public string strIPAddress; //IP
            public int iPort; //端口号
        }

        public TypeStruct getTypeStruct_DeviceInfoStruct_IEC(DeviceInfoStruct_IEC StructValue)
        {
            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.StringValue = Convert.ToString(StructValue.strDeviceName);
            v0.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v0);

            ObjectType v1 = new ObjectType();
            v1.StringValue = Convert.ToString(StructValue.strDeviceCode);
            v1.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v1);

            ObjectType v2 = new ObjectType();
            v2.Int16Value = StructValue.iStationCount;
            v2.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v2);

            ObjectType v3 = new ObjectType();
            v3.StringValue = Convert.ToString(StructValue.strPLCType);
            v3.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v3);

            ObjectType v4 = new ObjectType();
            v4.StringValue = Convert.ToString(StructValue.strProtocol);
            v4.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v4);

            ObjectType v5 = new ObjectType();
            v5.StringValue = Convert.ToString(StructValue.strIPAddress);
            v5.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v5);

            ObjectType v6 = new ObjectType();
            v6.Int32Value = StructValue.iPort;
            v6.TypeCode = CoreType.CtInt32;
            structV.StructElements.Add(v6);

            return structV;
        }

        public struct DeviceInfoStructList_IEC
        {
            public short iCount;    //设备信息总览表里的终端数量
            public DeviceInfoStruct_IEC[] arrDeviceInfo;

            public DeviceInfoStructList_IEC()
            {
                iCount = 0;
                arrDeviceInfo = new DeviceInfoStruct_IEC[30];

                for (int i = 0; i < arrDeviceInfo.Length; i++)
                {
                    arrDeviceInfo[i].strDeviceName = "";
                    arrDeviceInfo[i].strDeviceCode = "";
                    arrDeviceInfo[i].iStationCount = 0;
                    arrDeviceInfo[i].strPLCType = "";
                    arrDeviceInfo[i].strProtocol = "";
                    arrDeviceInfo[i].strIPAddress = "";
                    arrDeviceInfo[i].iPort = 0;

                }
            }

        }
        public TypeStruct getTypeStruct_DeviceInfoStructList_IEC(DeviceInfoStructList_IEC StructValue)
        {

            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.Int16Value = StructValue.iCount;
            v0.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v0);


            ObjectType v1 = new ObjectType();
            v1.ArrayValue = getTypeArrray(StructValue.arrDeviceInfo);
            v1.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v1);


            return structV;
        }


        #endregion


        #region 写入Excel的采集值 

        public struct AllDataReadfromMC
        {
            public bool[] DeviceInfoValue;          // 设备信息表
            public string[] Station1AInfoValue;        // 加工工位1A1B
            public string[] Station1BInfoValue;        // 加工工位1B
            public string[] Station2AInfoValue;        // 加工工位2A
            public string[] Station2BInfoValue;        // 加工工位2B
            public short[] ProductionDataValue;     // 生产统计 
            public uint[] LifeManagementValue;     // 寿命管理
            public bool[] FunctionEnableValue;      // 功能开关
            public bool[] OEEInfo1Value;            //OEE(1)
            public bool[] OEEInfo2Value;            //OEE(2)


            public AllDataReadfromMC()
            {
                DeviceInfoValue = new bool[17];
                Station1AInfoValue = new string[27];
                Station1BInfoValue = new string[27];
                Station2AInfoValue = new string[27];
                Station2BInfoValue = new string[27];
                ProductionDataValue = new short[4];
                LifeManagementValue = new uint[30];
                FunctionEnableValue = new bool[35];  // 以最大的35为界
                OEEInfo1Value = new bool[3];
                OEEInfo2Value = new bool[12];
            }
        }



        #endregion




        #region ProcessStationNameStruct_IEC 加工工位的点位名

        public struct UnitStationNameStruct_IEC
        {
            public short DataCount;           //点位数量 (每个工位里有多少个采集值）
            public short StationNO;           //所属工位号
            public string StationName;        // 加工工位名称
            public stringStruct[] arrKey;   // //每个加工工位最多16个点位

            public UnitStationNameStruct_IEC()
            {
                DataCount = 0;
                StationNO = 0;
                StationName = "";
                arrKey = new stringStruct[30];
                for (int i = 0; i < arrKey.Length; i++)
                {
                    arrKey[i].StringValue = " ";


                }


            }
        }

        public TypeStruct getTypeStruct_UnitStationNameStruct_IEC(UnitStationNameStruct_IEC StructValue)
        {

            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.Int16Value = StructValue.DataCount;
            v0.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v0);

            ObjectType v1 = new ObjectType();
            v1.Int16Value = StructValue.StationNO;
            v1.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v1);

            ObjectType v2 = new ObjectType();
            v2.StringValue = StructValue.StationName;
            v2.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v2);

            ObjectType v3 = new ObjectType();
            v3.ArrayValue = getTypeArrray(StructValue.arrKey);
            v3.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v3);


            return structV;
        }

        public struct ProcessStationNameStruct_IEC
        {
            public short StationCount;           //点位数量（一共有多少个加工工位）
            public UnitStationNameStruct_IEC[] UnitStation;

            public ProcessStationNameStruct_IEC()
            {
                StationCount = 0;
                UnitStation = new UnitStationNameStruct_IEC[10];
                for (int i = 0; i < UnitStation.Length; i++)
                {
                    UnitStation[i] = new UnitStationNameStruct_IEC();

                }
            }

        }

        public TypeStruct getTypeStruct_ProcessStationNameStruct_IEC(ProcessStationNameStruct_IEC StructValue)
        {

            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.Int16Value = StructValue.StationCount;
            v0.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v0);


            ObjectType v1 = new ObjectType();
            v1.ArrayValue = getTypeArrray(StructValue.UnitStation);
            v1.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v1);


            return structV;
        }

        #endregion


        #region OneSecPointNameStruct_IEC（1000ms 点位名）
        public struct OneSecPointNameStruct_IEC
        {
            public int DataCount_FE;            //功能开关点位数量
            public stringStruct[] Name_FE;      //功能开关点位名称

            public int DataCount_PD;            //生产统计点位数量
            public stringStruct[] Name_PD;      //生产统计点位名称

            public int DataCount_ALM;            //报警信号点位数量
            public stringStruct[] Name_ALM;      //报警信号点位名称

            public int DataCount_LM;             //寿命管理点位数量
            public stringStruct[] Name_LM;       //寿命管理点位名称

            public int DataCount_OEE;             //OEE点位点位数量
            public stringStruct[] Name_OEE;       //OEE点位点位名称 


            public OneSecPointNameStruct_IEC()
            {
                DataCount_FE = 0;
                DataCount_PD = 0;
                DataCount_ALM = 0;
                DataCount_LM = 0;
                DataCount_OEE = 0;

                Name_FE = new stringStruct[200];
                Name_PD = new stringStruct[20];
                Name_ALM = new stringStruct[2000];
                Name_LM = new stringStruct[36];
                Name_OEE = new stringStruct[20];

                for (int i = 0; i < Name_FE.Length; i++)
                {
                    Name_FE[i].StringValue = " ";
                }
                for (int i = 0; i < Name_PD.Length; i++)
                {
                    Name_PD[i].StringValue = " ";
                }
                for (int i = 0; i < Name_ALM.Length; i++)
                {
                    Name_ALM[i].StringValue = " ";
                }
                for (int i = 0; i < Name_LM.Length; i++)
                {
                    Name_LM[i].StringValue = " ";
                }
                for (int i = 0; i < Name_OEE.Length; i++)
                {
                    Name_OEE[i].StringValue = " ";
                }

            }
        }
        public TypeStruct getTypeStruct_OneSecPointNameStruct_IEC(OneSecPointNameStruct_IEC StructValue)
        {

            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.Int16Value = StructValue.DataCount_FE;
            v0.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v0);

            ObjectType v1 = new ObjectType();
            v1.ArrayValue = getTypeArrray(StructValue.Name_FE);
            v1.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v1);

            ObjectType v2 = new ObjectType();
            v2.Int16Value = StructValue.DataCount_PD;
            v2.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v2);

            ObjectType v3 = new ObjectType();
            v3.ArrayValue = getTypeArrray(StructValue.Name_PD);
            v3.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v3);

            ObjectType v4 = new ObjectType();
            v4.Int16Value = StructValue.DataCount_ALM;
            v4.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v4);

            ObjectType v5 = new ObjectType();
            v5.ArrayValue = getTypeArrray(StructValue.Name_ALM);
            v5.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v5);

            ObjectType v6 = new ObjectType();
            v6.Int16Value = StructValue.DataCount_LM;
            v6.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v6);

            ObjectType v7 = new ObjectType();
            v7.ArrayValue = getTypeArrray(StructValue.Name_LM);
            v7.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v7);

            ObjectType v8 = new ObjectType();
            v8.Int16Value = StructValue.DataCount_OEE;
            v8.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v8);

            ObjectType v9 = new ObjectType();
            v9.ArrayValue = getTypeArrray(StructValue.Name_OEE);
            v9.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v9);


            return structV;
        }

        #endregion


        #region StationInfoStruct_IEC 设备信息表中的信息

        public struct StationInfoStruct_IEC
        {
            public short StationCount;           //实际工位数量
            public short[] NextStationNO;      //后工位序号
            public short[] TempCodeNO;         //生成虚拟码

            public StationInfoStruct_IEC()
            {
                StationCount = 0;
                NextStationNO = new short[80];
                TempCodeNO = new short[80];
            }
        }
        public TypeStruct getTypeStruct_StationInfoStruct_IEC(StationInfoStruct_IEC StructValue)
        {

            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.Int16Value = StructValue.StationCount;
            v0.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v0);


            ObjectType v1 = new ObjectType();
            v1.ArrayValue = getTypeArrray(StructValue.NextStationNO);
            v1.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v1);


            ObjectType v2 = new ObjectType();
            v2.ArrayValue = getTypeArrray(StructValue.TempCodeNO);
            v2.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v2);

            return structV;
        }

        #endregion


        #region UDT_ProcessStationDataValue 加工工位采集值
        public struct UDT_DataValue
        {
            public short iDataCount; //点位数量
            public stringStruct[] arrDataPoint; //数组长度30


            public UDT_DataValue()
            {
                iDataCount = 0;
                arrDataPoint = new stringStruct[30];
                for (int i = 0; i < arrDataPoint.Length; i++)
                {
                    arrDataPoint[i].StringValue = " ";
                }

            }
        }
        public TypeStruct getTypeStruct_UDT_DataValue(UDT_DataValue StructValue)
        {
            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.Int16Value = StructValue.iDataCount;
            v0.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v0);

            ObjectType v1 = new ObjectType();
            v1.ArrayValue = getTypeArrray(StructValue.arrDataPoint);
            v1.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v1);

            return structV;
        }

        public struct UDT_ProcessStationDataValue
        {
            public short iDataCount; //点位数量
            public UDT_DataValue[] arrDataPoint; //数组长度10

            public UDT_ProcessStationDataValue()
            {
                iDataCount = 0;
                arrDataPoint = new UDT_DataValue[10];
                for (int i = 0; i < arrDataPoint.Length; i++)
                {
                    arrDataPoint[i] = new UDT_DataValue();
                }
            }
        }
        public TypeStruct getTypeStruct_UDT_ProcessStationDataValue(UDT_ProcessStationDataValue StructValue)
        {
            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.Int16Value = StructValue.iDataCount;
            v0.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v0);

            ObjectType v1 = new ObjectType();
            v1.ArrayValue = getTypeArrray(StructValue.arrDataPoint);
            v1.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v1);

            return structV;
        }

        #endregion


        #region 设备信息表的采集值 UDT_StationUnit
        //每个工位的信息
        public struct UDT_StationUnit
        {
            public int diProcessData;
            public bool xStationBusy; //工位加工中信号（用于触发100ms参数采集，短于）
            public bool xCellMem; //电芯记忆信号（当前工位有电芯，用于组合电芯条码做条码转移和参数绑定）
            public bool xCellMemClear; //电芯记忆清除信号
            public string strCellCode; //电芯条码
            public string strPoleEarCode; //极耳码


            public UDT_StationUnit()
            {
                diProcessData = 0;
                xStationBusy = false;
                xCellMem = false;
                xCellMemClear = false;
                strCellCode = "";
                strPoleEarCode = "";
            }
        }

        public TypeStruct getTypeStruct_UDT_StationUnit(UDT_StationUnit StructValue)
        {
            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.Int32Value = StructValue.diProcessData;
            v0.TypeCode = CoreType.CtInt32;
            structV.StructElements.Add(v0);

            ObjectType v1 = new ObjectType();
            v1.BoolValue = StructValue.xStationBusy;
            v1.TypeCode = CoreType.CtBoolean;
            structV.StructElements.Add(v1);

            ObjectType v2 = new ObjectType();
            v2.BoolValue = StructValue.xCellMem;
            v2.TypeCode = CoreType.CtBoolean;
            structV.StructElements.Add(v2);

            ObjectType v3 = new ObjectType();
            v3.BoolValue = StructValue.xCellMemClear;
            v3.TypeCode = CoreType.CtBoolean;
            structV.StructElements.Add(v3);

            ObjectType v4 = new ObjectType();
            v4.StringValue = StructValue.strCellCode;
            v4.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v4);

            ObjectType v5 = new ObjectType();
            v5.StringValue = StructValue.strPoleEarCode;
            v5.TypeCode = CoreType.CtString;
            structV.StructElements.Add(v5);



            return structV;
        }


        //每个工位的信息
        public struct UDT_StationListlnfo
        {
            public short iDataCount; //点位数量
            public UDT_StationUnit[] arrDataPoint; //数组长度80

            public UDT_StationListlnfo()
            {
                iDataCount = 0;
                arrDataPoint = new UDT_StationUnit[80];
                for (int i = 0; i < arrDataPoint.Length; i++)
                {
                    arrDataPoint[i] = new UDT_StationUnit();
                }
            }
        }

        public TypeStruct getTypeStruct_UDT_StationListlnfo(UDT_StationListlnfo StructValue)
        {
            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.Int16Value = StructValue.iDataCount;
            v0.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v0);

            ObjectType v1 = new ObjectType();
            v1.ArrayValue = getTypeArrray(StructValue.arrDataPoint);
            v1.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v1);

            return structV;
        }
        #endregion


        #region  设备数据采集值 1000ms数据

        public struct DeviceDataStruct_IEC
        {
            public bool[] Value_FE;
            public int[] Value_PD;
            public bool[] Value_ALM;
            public UInt32[] Value_LM;
            public bool[] Value_OEE;

            public DeviceDataStruct_IEC()
            {
                Value_FE = new bool[200];
                Value_PD = new int[20];
                Value_ALM = new bool[2000];
                Value_LM = new UInt32[36];
                Value_OEE = new bool[20];
            }

        }

        public TypeStruct getTypeStruct_DeviceDataStruct_IEC(DeviceDataStruct_IEC StructValue)
        {

            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.ArrayValue = getTypeArrray(StructValue.Value_FE);
            v0.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v0);

            ObjectType v1 = new ObjectType();
            v1.ArrayValue = getTypeArrray(StructValue.Value_PD);
            v1.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v1);

            ObjectType v2 = new ObjectType();
            v2.ArrayValue = getTypeArrray(StructValue.Value_ALM);
            v2.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v2);

            ObjectType v3 = new ObjectType();
            v3.ArrayValue = getTypeArrray(StructValue.Value_LM);
            v3.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v3);

            ObjectType v4 = new ObjectType();
            v4.ArrayValue = getTypeArrray(StructValue.Value_OEE);
            v4.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v4);


            return structV;
        }




        #endregion


    }
}
