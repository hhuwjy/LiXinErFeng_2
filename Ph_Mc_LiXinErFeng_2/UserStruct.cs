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



            return ArrayV;

        }
        #endregion


        #region stringStruct
        public struct stringStruct
        {
            public string str;            
        }
        public TypeStruct getTypeStruct_stringStruct(stringStruct StructValue)
        {
            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.StringValue = StructValue.str;
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
            public bool  xCellMemClear; //电芯记忆清除信号
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


        #region OneSecPointNameStruct_IEC（1000ms数据）
        public struct OneSecPointNameStruct_IEC
        {
            public int iDataCount; //点位数量
            public stringStruct[] stringArrData;  
        }
        public TypeStruct getTypeStruct_OneSecPointNameStruct_IEC(OneSecPointNameStruct_IEC StructValue)
        {

            TypeStruct structV = new TypeStruct();

            ObjectType v0 = new ObjectType();
            v0.Int16Value = StructValue.iDataCount;
            v0.TypeCode = CoreType.CtInt16;
            structV.StructElements.Add(v0);

            ObjectType v1 = new ObjectType();
            v1.ArrayValue = getTypeArrray(StructValue.stringArrData);
            v1.TypeCode = CoreType.CtArray;
            structV.StructElements.Add(v1);

            return structV;
        }

        #endregion



        #region 从Excel中抽象出来的数据类型

        //设备信息（47个）
        public struct DeviceInfoConSturct_MC
        {
            public string varType;  //数据类型
            public string varName;  //标签名
            public string stationName;//工位名
            public int varOffset;  //地址偏移量
            public int stationNumber;  //工位对应序号
        }

        //加工工位（3个）
        public struct StationInfoStruct_MC
        {
            public string stationName;  //工位名    
            public string varType;  //变量类型
            public string varName;  //变量名
            public string varAnnotation;  //描述
            public int varOffset;  //地址偏移量
            public int varMagnification;     // 倍率
        }

        //1000ms数据
        public struct OneSecInfoStruct_MC
        {
            public string varType;  //变量类型
            public string varName;  //变量名
            public string varAnnotation;  //描述
            public int varOffset;  //地址偏移量

        }

        public struct OneSecAlarmStruct_MC
        {
            public string varType;  //变量类型
            public string varName;  //变量名
            public string varAnnotation;  //描述
            public double varOffset;  //地址偏移量

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

        #endregion


        #region 写入Excel的采集值 

        public struct AllDataReadfromMC
        {
            public bool[] DeviceInfoValue;          // 设备信息表
            public float[] Station1A1BInfoValue;        // 加工工位1A1B
            public float[] Station2A2BInfoValue;        // 加工工位2A2B
            public short[] ProductionDataValue;     // 生产统计 
            public ushort[] LifeManagementValue;     // 寿命管理
            public bool[] FunctionEnableValue;      // 功能开关
            public bool[] OEEInfo1Value;            //OEE(1)
            public bool[] OEEInfo2Value;            //OEE(2)


            public AllDataReadfromMC()
            {
                DeviceInfoValue = new bool[17];
                Station1A1BInfoValue = new float[11];
                Station2A2BInfoValue = new float[11];
                ProductionDataValue = new short[4];
                LifeManagementValue = new ushort[30];
                FunctionEnableValue = new bool[35];  // 以最大的35为界
                OEEInfo1Value = new bool[3];
                OEEInfo2Value = new bool[12];
            }
        }



        #endregion


    }
}
