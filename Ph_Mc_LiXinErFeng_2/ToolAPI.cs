using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Ph_Mc_LiXinErFeng
{
    public class ToolAPI
    {
        #region Convert Float Array To Ascii

        public StringBuilder ConvertFloatToAscii(float value)
        {
            StringBuilder asciiString = new StringBuilder(512);

             
            if (value >0 && value <= 255)  //value不会是0 if (value >= 0 && value <= 255)  
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)value };
                asciiString.Append(asciiEncoding.GetString(byteArray));
            }
            else if (value == 0)
            {
                asciiString.Append("");

            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }


            return asciiString;
        }



        public StringBuilder ConvertFloatArrayToAscii(float[] value, int startIndex, int endIndex)
        {
            StringBuilder asciiString = new StringBuilder(512);
            for (int i = startIndex; i < (endIndex + 1); i++)
            {
                asciiString.Append(ConvertFloatToAscii(value[i]));
            }
            asciiString.Append(",");
            return asciiString;
        }

        public StringBuilder ConvertFloatArrayToAscii(float[] value)
        {
            StringBuilder asciiString = new StringBuilder(512);
            foreach (float f in value)
            {
                if (f != 0)
                {
                    asciiString.Append(ConvertFloatToAscii(f));
                }
            }
            return asciiString;
        }

        #endregion



        #region 读取PLC的IP地址

        public List<string> ReadPLCIpAddress(string ObjectAddress)
        {
            List<string> plcIpAddresses = new List<string>();

            if (System.IO.File.Exists(ObjectAddress))
            {
                JObject json = JObject.Parse(System.IO.File.ReadAllText(ObjectAddress, System.Text.Encoding.UTF8));
                foreach (var property in json.Properties())
                {
                    plcIpAddresses.Add(property.Value.ToString());                 
                }
            }

            return plcIpAddresses;
        }
        #endregion


        #region  匹配 [100..200]这种string
        public static int[] ExtractRange(string input)
        {
            int[] index = new int[2];    
            string pattern = @"\[(\d+)\.\.(\d+)\]";
            Match match = Regex.Match(input, pattern);
            
            if (!match.Success)
            {
                throw new ArgumentException("Input string does not contain a valid range in the format [start..end].");
            }

            index[0] = int.Parse(match.Groups[1].Value);
            index[1] = int.Parse(match.Groups[2].Value);  

            return index;
            
        }
        #endregion



    }
}
