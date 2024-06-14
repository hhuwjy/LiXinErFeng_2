// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using Grpc.Net.Client;
using Arp.Device.Interface.Services.Grpc;
using Arp.Plc.Gds.Services.Grpc;
using Grpc.Core;
using Arp.Plc.Grpc;
using Google.Protobuf.Collections;
using System.Net;
using Opc.Ua.Export;
using Opc.Ua;
using Arp.Type.Grpc;
using Google.Protobuf;
using HslCommunication.LogNet;
using HslCommunication.Profinet.Omron;
using HslCommunication;
using System.ComponentModel;
using Google.Protobuf.WellKnownTypes;
using static System.Net.Mime.MediaTypeNames;
using static Ph_Mc_LiXinErFeng.UserStruct;
using static Arp.Plc.Gds.Services.Grpc.IDataAccessService;
using static Arp.Plc.Gds.Services.Grpc.ISubscriptionService;
using NPOI.Util;
using static Microsoft.IO.RecyclableMemoryStreamManager;


namespace Ph_Mc_LiXinErFeng
{
    public class GrpcTool
    {

        //CreateSubscription
        public bool CreatSubscribe(ISubscriptionService.ISubscriptionServiceClient GrpcSubscriptionServiceClient, ISubscriptionServiceCreateSubscriptionRequest createSubscriptionRequest, out ISubscriptionServiceCreateSubscriptionResponse createSubscriptionResponse, CallOptions options)
        {
            bool result = false;
            createSubscriptionResponse = new ISubscriptionServiceCreateSubscriptionResponse();
            try
            {
                createSubscriptionResponse = GrpcSubscriptionServiceClient.CreateSubscription(createSubscriptionRequest, options);
                if (createSubscriptionResponse.ReturnValue != 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        //Add Subscribe Variables
        public bool AddSubscribeVars(ISubscriptionService.ISubscriptionServiceClient GrpcSubscriptionServiceClient, ISubscriptionServiceCreateSubscriptionResponse createSubscriptionResponse, string filePath, CallOptions options)
        {
            bool result = true;
            // Add Variables to be subscribed
            try
            {
                uint unique_id = createSubscriptionResponse.ReturnValue;

                UANodeSet uaNodeSet = getNodeSet(filePath);
                if (uaNodeSet != null && uaNodeSet.Aliases != null)
                {
                    ISubscriptionServiceAddVariableRequest addVariableRequest = new ISubscriptionServiceAddVariableRequest();
                    ISubscriptionServiceAddVariableResponse addVariable_Response = new ISubscriptionServiceAddVariableResponse();
                    foreach (NodeIdAlias v in uaNodeSet.Aliases)
                    {
                        var nodeid = v.Value.Trim();
                        addVariableRequest.VariableName = nodeid;
                        addVariableRequest.SubscriptionId = unique_id;
                        addVariable_Response = GrpcSubscriptionServiceClient.AddVariable(addVariableRequest, options);
                        if (Convert.ToString(addVariable_Response.ReturnValue) != "DaeNone")
                        {
                            result = false;
                            Console.WriteLine("Add Variables:({0}) to be subscribed is failed", nodeid);
                        }
                        else
                        {
                            Console.WriteLine("Add Variables:({0}) to be subscribed is success", nodeid);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        // Subscribe 
        public bool Subscribe(ISubscriptionService.ISubscriptionServiceClient GrpcSubscriptionServiceClient, ISubscriptionServiceCreateSubscriptionResponse createSubscriptionResponse, CallOptions options)
        {
            bool result = false;
            try
            {
                var unique_id = createSubscriptionResponse.ReturnValue;

                //Service Request Subscribe
                ISubscriptionServiceSubscribeRequest subscribeRequest = new ISubscriptionServiceSubscribeRequest();
                ISubscriptionServiceSubscribeResponse subscribeResponse = new ISubscriptionServiceSubscribeResponse();
                subscribeRequest.SubscriptionId = createSubscriptionResponse.ReturnValue;
                subscribeRequest.SampleRate = 0;
                subscribeResponse = GrpcSubscriptionServiceClient.Subscribe(subscribeRequest, options);

                if (Convert.ToString(subscribeResponse.ReturnValue) == "DaeNone")
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        //Read Values from Subscribe
        public bool SubscribeReadValues(ISubscriptionService.ISubscriptionServiceClient GrpcSubscriptionServiceClient, ISubscriptionServiceCreateSubscriptionResponse createSubscriptionResponse, ISubscriptionServiceReadValuesRequest readValuesRequest, out ISubscriptionServiceReadValuesResponse readValuesResponse, CallOptions options)
        {
            bool result = false;
            readValuesResponse = new ISubscriptionServiceReadValuesResponse();
            try
            {
                // setup  readValuesRequest
                readValuesRequest.SubscriptionId = createSubscriptionResponse.ReturnValue;
                readValuesResponse = GrpcSubscriptionServiceClient.ReadValues(readValuesRequest, options);

                // get the ReturnValue from subscribe  
                if (Convert.ToString(readValuesResponse.ReturnValue) == "DaeNone")
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        //Read Variabls Infos from Subscribe
        public bool SubscribeReadInfos(ISubscriptionService.ISubscriptionServiceClient GrpcSubscriptionServiceClient, ISubscriptionServiceCreateSubscriptionResponse createSubscriptionResponse, ISubscriptionServiceGetVariableInfosRequest variableInfosRequest, out ISubscriptionServiceGetVariableInfosResponse variableInfosResponse, CallOptions options)
        {
            bool result = false;
            variableInfosResponse = new ISubscriptionServiceGetVariableInfosResponse();
            try
            {
                // setup  GetVariableInfosRequest
                variableInfosRequest.SubscriptionId = createSubscriptionResponse.ReturnValue;
                variableInfosResponse = GrpcSubscriptionServiceClient.GetVariableInfos(variableInfosRequest, options);

                // get the ReturnValue from subscribe  
                if (Convert.ToString(variableInfosResponse.ReturnValue) == "DaeNone")
                {
                    result = true;
                }


            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        UANodeSet getNodeSet(string filePath)
        {
            UANodeSet uaNodeSet = null;
            if (File.Exists(filePath))
            {
                FileStream file = File.Open(filePath, FileMode.Open);
                uaNodeSet = UANodeSet.Read(file);
            }
            return uaNodeSet;
        }

        PropertyInfo[] GetProperties<T>(T t)
        {
            PropertyInfo[] result = null;
            if (t == null)
            {
                return null;
            }
            result = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            return result;
        }



        //Add Data to IDataAccessServiceWriteRequest

        public void ServiceWriteRequestAddDatas(out IDataAccessServiceWriteRequest dataAccessServiceWriteRequest, WriteItem[] writeItems)
        {


            dataAccessServiceWriteRequest = new IDataAccessServiceWriteRequest();
            try
            {
                dataAccessServiceWriteRequest.Data.Clear();
                dataAccessServiceWriteRequest.Data.Add(writeItems);
            }
            catch (Exception e)
            {
                Console.WriteLine("ServiceWriteRequestAddDatas Erro: {0}", e);
            }
        }

        //Add Data to IDataAccessServiceWriteRequest
        public IDataAccessServiceWriteRequest ServiceWriteRequestAddDatas( WriteItem[] writeItems)
        {

            IDataAccessServiceWriteRequest dataAccessServiceWriteRequest = new IDataAccessServiceWriteRequest();
            try
            {
                dataAccessServiceWriteRequest.Data.Clear();
                dataAccessServiceWriteRequest.Data.Add(writeItems);
            }
            catch (Exception e)
            {
                Console.WriteLine("ServiceWriteRequestAddDatas Erro: {0}", e);
            }
            return dataAccessServiceWriteRequest;
        }



        //Write Data to DataAccessService 
        public bool WriteDataToDataAccessService(IDataAccessService.IDataAccessServiceClient grpcDataAccessServiceClient, IDataAccessServiceWriteRequest dataAccessServiceWriteRequest, IDataAccessServiceWriteResponse dataAccessServiceWriteResponse, CallOptions options)
        {
            bool result = true;
            try
            {
                dataAccessServiceWriteResponse = grpcDataAccessServiceClient.Write(dataAccessServiceWriteRequest, options);

                int count = dataAccessServiceWriteResponse.ReturnValue.Count;
                for (int i = 0; i < count; i++)
                {

                    //Console.WriteLine("dataAccessServiceWriteResponse.ReturnValue---{0}:   {1})", i,dataAccessServiceWriteResponse.ReturnValue[i]);

                    if (Convert.ToString(dataAccessServiceWriteResponse.ReturnValue[i]) != "DaeNone")
                    {
                        result = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("result = Exception:{0}",e.ToString());
                result = false;
            }
            return result;
        }


        public async Task<bool> WriteAsyncDataToDataAccessService(IDataAccessService.IDataAccessServiceClient grpcDataAccessServiceClient, IDataAccessServiceWriteRequest dataAccessServiceWriteRequest, IDataAccessServiceWriteResponse dataAccessServiceWriteResponse, CallOptions options)
        {
            bool result = true;
            try
            {
                dataAccessServiceWriteResponse = await grpcDataAccessServiceClient.WriteAsync(dataAccessServiceWriteRequest, options);

                //todotodo..........
                int count = dataAccessServiceWriteResponse.ReturnValue.Count;
                for (int i = 0; i < count; i++)
                {
                    if (Convert.ToString(dataAccessServiceWriteResponse.ReturnValue[i]) != "DaeNone")
                    {
                        result = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("result = Exception:{}");
                result = false;
            }
            return result;
        }



        //Convert from Arp.plc.Grpc.DataType to Arp.Type.Grpc.CoreType
        public Arp.Type.Grpc.CoreType getGrpcCoreType(DataType dataType)
        {
            Arp.Type.Grpc.CoreType result = new Arp.Type.Grpc.CoreType();
            switch ((int)dataType)
            {
                case 0:
                    result = Arp.Type.Grpc.CoreType.CtNone;
                    break;

                case 1:
                    result = Arp.Type.Grpc.CoreType.CtVoid;
                    break;

                case 3:
                    result = Arp.Type.Grpc.CoreType.CtBoolean;
                    break;

                case 6:
                    result = Arp.Type.Grpc.CoreType.CtChar;
                    break;

                case 5:
                    result = Arp.Type.Grpc.CoreType.CtInt8;
                    break;

                case 4:
                    result = Arp.Type.Grpc.CoreType.CtUint8;
                    break;

                case 9:
                    result = Arp.Type.Grpc.CoreType.CtInt16;
                    break;

                case 8:
                    result = Arp.Type.Grpc.CoreType.CtUint16;
                    break;

                case 11:
                    result = Arp.Type.Grpc.CoreType.CtInt32;
                    break;

                case 10:
                    result = Arp.Type.Grpc.CoreType.CtUint32;
                    break;

                case 13:
                    result = Arp.Type.Grpc.CoreType.CtInt64;
                    break;

                case 12:
                    result = Arp.Type.Grpc.CoreType.CtUint64;
                    break;

                case 14:
                    result = Arp.Type.Grpc.CoreType.CtReal32;
                    break;

                case 15:
                    result = Arp.Type.Grpc.CoreType.CtReal64;
                    break;

                case 45:
                    result = Arp.Type.Grpc.CoreType.CtString;
                    break;

                case 66:
                    result = Arp.Type.Grpc.CoreType.CtStruct;
                    break;

                case 1024:
                    result = Arp.Type.Grpc.CoreType.CtArray;
                    break;

                case 33:
                    result = Arp.Type.Grpc.CoreType.CtDateTime;
                    break;

                case 2048:
                    result = Arp.Type.Grpc.CoreType.CtEnum;
                    break;

                case 34:
                    result = Arp.Type.Grpc.CoreType.CtIecTime;
                    break;

                case 35:
                    result = Arp.Type.Grpc.CoreType.CtIecTime64;
                    break;

                case 36:
                    result = Arp.Type.Grpc.CoreType.CtIecDate;
                    break;

                case 37:
                    result = Arp.Type.Grpc.CoreType.CtIecDate64;
                    break;

                case 38:
                    result = Arp.Type.Grpc.CoreType.CtIecDateTime;
                    break;

                case 39:
                    result = Arp.Type.Grpc.CoreType.CtIecDateTime64;
                    break;

                case 40:
                    result = Arp.Type.Grpc.CoreType.CtIecTimeOfDay;
                    break;

                case 41:
                    result = Arp.Type.Grpc.CoreType.CtIecTimeOfDay64;
                    break;

                default:
                    result = Arp.Type.Grpc.CoreType.CtNone;
                    break;
            }
            return result;


        }

        public WriteItem CreatWriteItem(string portName, Arp.Type.Grpc.CoreType type, object value)
        {
            WriteItem writeItem = new WriteItem();
            writeItem.PortName = portName;
            writeItem.Value = new Arp.Type.Grpc.ObjectType();
            writeItem.Value.TypeCode = type;
            switch ((int)type)
            {
                case 2:
                    writeItem.Value.BoolValue = Convert.ToBoolean(value);
                    break;

                case 3:
                    writeItem.Value.CharValue = Convert.ToChar(value);
                    break;

                case 4:
                    writeItem.Value.Int8Value = Convert.ToInt16(value);
                    break;
                case 5:
                    writeItem.Value.Uint16Value = Convert.ToUInt16(value);
                    break;

                case 6:
                    writeItem.Value.Int16Value = Convert.ToInt16(value);
                    break;

                case 7:
                    writeItem.Value.Uint16Value = Convert.ToUInt16(value);
                    break;

                case 8:
                    writeItem.Value.Int32Value = Convert.ToInt32(value);
                    break;

                case 9:
                    writeItem.Value.Uint32Value = Convert.ToUInt32(value);
                    break;

                case 10:
                    writeItem.Value.Int64Value = Convert.ToInt64(value);
                    break;

                case 11:
                    writeItem.Value.UIntValue = Convert.ToUInt64(value);
                    break;

                case 12:
                    writeItem.Value.FloatValue = Convert.ToInt32(value);
                    break;

                case 13:
                    writeItem.Value.DoubleValue = Convert.ToDouble(value);
                    break;

                case 18:
                    writeItem.Value.StructValue = new UserStruct().getTypeStruct(value);
                    break;

                case 19:
                    string str = Convert.ToString(value);
                    if (str.Length > 511)
                    {
                        str = str.Substring(0, 511);
                    }
                    writeItem.Value.StringValue = str;
                    break;

                case 20:
                    writeItem.Value.ArrayValue = new UserStruct().getTypeArrray(value);
                    break;

                case 23:
                    writeItem.Value.Int64Value = Convert.ToInt64(value);
                    break;

                case 26:
                    writeItem.Value.StringValue = Convert.ToString(value);
                    break;

                case 30:
                    writeItem.Value.StringValue = Convert.ToString(value);
                    break;

                case 36:
                    writeItem.Value.StringValue = Convert.ToString(value);
                    break;

                case 41:
                    writeItem.Value.Int32Value = Convert.ToInt32(value);
                    break;

                case 42:
                    writeItem.Value.Int64Value = Convert.ToInt64(value);
                    break;

                case 43:
                    writeItem.Value.Int64Value = Convert.ToInt64(value);
                    break;

                case 44:
                    writeItem.Value.Int64Value = Convert.ToInt64(value);
                    break;

                case 45:
                    writeItem.Value.Int64Value = Convert.ToInt64(value);
                    break;

                case 46:
                    writeItem.Value.Int64Value = Convert.ToInt64(value);
                    break;

                case 47:
                    writeItem.Value.Int64Value = Convert.ToInt64(value);
                    break;

                case 48:
                    writeItem.Value.Int64Value = Convert.ToInt64(value);
                    break;

                default:
                    writeItem = null;
                    break;
            }
            return writeItem;

        }


        //get NodeIdDictionary from xml
        public Dictionary<string, string> getNodeIdDictionary(string filePath)
        {

            Dictionary<string, string> result = new Dictionary<string, string>();
            UANodeSet uaNodeSet = null;
            if (File.Exists(filePath))
            {
                FileStream file = File.Open(filePath, FileMode.Open);
                uaNodeSet = UANodeSet.Read(file);
            }

            if (uaNodeSet != null && uaNodeSet.Aliases != null)
            {
                foreach (NodeIdAlias v in uaNodeSet.Aliases)
                {
                    var nodeid = v.Value.Trim();
                    result.Add(v.Alias, nodeid);
                }
            }
            return result;

        }

        //get NodeId By Alias from NodeIdDictionary
        public string getNodeIdByAlias(Dictionary<string, string> NodeIdDictionary, String alias)
        {
            string result = "";

            if (NodeIdDictionary.ContainsKey(alias))
            {
                result = NodeIdDictionary[alias];
            }

            return result;
        }


        /// <summary>
        /// 得到属性值
        /// </summary>
        /// <param name="objclass">先进行dynamic objclass = assembly.CreateInstance(className)，得到的objclass</param>
        /// <param name="propertyname">属性名称</param>
        /// <returns>属性值，是object类型，使用时记得转换</returns>
        public object ReflectionGetPropertyValue(object objclass, string propertyname)
        {
            object result = null;

            PropertyInfo[] infos = GetProperties<object>(objclass);
            Console.WriteLine("infos:{0}", infos);
            try
            {
                foreach (PropertyInfo info in infos)
                {
                    if (info.Name == propertyname)
                    {
                        System.Console.WriteLine(info.GetValue(objclass, null));
                        result = info.GetValue(objclass, null);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                result = null;
            }

            return result;
        }

        public int getIndexByName(RepeatedField<global::Arp.Plc.Gds.Services.Grpc.VariableInfo> variableInfos, string name)
        {
            List<string> lisName = new List<string>();
            for (int i = 0; i < variableInfos.Count; i++)
            {
                Console.WriteLine("variableInfos[{0}].Name:{1}", i, variableInfos[i].Name);
                lisName.Add(variableInfos[i].Name);
            }

            Console.WriteLine("lisName.IndexOf(name):{0}", lisName.IndexOf(name));

            return lisName.IndexOf(name);
        }


        public class UnixDomainSocketConnectionFactory
        {
            private readonly EndPoint _endPoint;

            public UnixDomainSocketConnectionFactory(EndPoint endPoint)
            {
                _endPoint = endPoint;
            }

            public async ValueTask<Stream> ConnectAsync(SocketsHttpConnectionContext _,
                CancellationToken cancellationToken = default)
            {
                var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

                try
                {
                    await socket.ConnectAsync(_endPoint, cancellationToken).ConfigureAwait(false);
                    return new NetworkStream(socket, true);
                }
                catch
                {
                    socket.Dispose();
                    throw;
                }
            }
        }

        //Write Data to DataAccessService 
        public ObjectType ReadSingleDataToDataAccessService(IDataAccessService.IDataAccessServiceClient grpcDataAccessServiceClient, IDataAccessServiceReadSingleRequest dataAccessServiceReadSingleRequest, IDataAccessServiceReadSingleResponse dataAccessServiceReadSingleResponse, CallOptions options)
        {
            ObjectType result = null;
            try
            {
                dataAccessServiceReadSingleResponse = grpcDataAccessServiceClient.ReadSingle(dataAccessServiceReadSingleRequest, options);
                var readItem = dataAccessServiceReadSingleResponse.ReturnValue;

                if (readItem.Error.ToString() == "DaeNone")
                {
                    result = readItem.Value;
                }
                else
                {
                    Console.WriteLine("ReadSingleDataToDataAccessService fail:{0}", readItem.Error.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("result = Exception:{0}", e.ToString());
            }
            return result;
        }


    }


}


