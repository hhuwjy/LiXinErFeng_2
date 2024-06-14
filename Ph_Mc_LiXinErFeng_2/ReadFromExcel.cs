using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NPOI.SS.Formula;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using static Ph_Mc_LiXinErFeng.UserStruct;

namespace Ph_Mc_LiXinErFeng
{

    public class ReadExcel
    {
       
        public XSSFWorkbook connectExcel(string excelFilePath)
        {
            XSSFWorkbook xssWorkbook = null;

            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine(excelFilePath + ": 读取的文件不存在");
                return xssWorkbook;
            }

           

            try {
                using (FileStream stream = new FileStream(excelFilePath, FileMode.Open))
                {
                    stream.Position = 0;
                    xssWorkbook = new XSSFWorkbook(stream);
                    stream.Close();
                }
            }
            catch (Exception )
            {
                return xssWorkbook;
                throw;

            }


            return xssWorkbook;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public StationInfoStruct_MC[] ReadStationInfo_Excel(XSSFWorkbook xssWorkbook, string sheetName)
        {
            DataTable dtTable = new DataTable();
            List<string> rowList = new List<string>();
            
           
            //sheet = xssWorkbook.GetSheetAt(0);
            ISheet sheet = xssWorkbook.GetSheet(sheetName);
            if (sheet == null)
            {
                Console.WriteLine(sheetName+ "页不存在");
                return null;

            }


            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

     

            List<StationInfoStruct_MC> retList = new List<StationInfoStruct_MC>();


            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                {
                    dtTable.Columns.Add(cell.ToString());
                }
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                string str = Convert.ToString(row.GetCell(1));
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) continue;


                var v = new StationInfoStruct_MC();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        if (!string.IsNullOrEmpty(row.GetCell(j).ToString()) && !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                        {
                            v.stationName = Convert.ToString(sheetName);
                            if (j == getCellIndexByName(headerRow, "偏移地址"))
                            {
                                v.varName = Convert.ToString(row.GetCell(j));
                                if (!(string.IsNullOrEmpty(v.varName) || string.IsNullOrWhiteSpace(v.varName)))
                                {
                                    //Regex r = new Regex(@"(?i)(?<=\[)(.*)(?=\])");//中括号[]
                                    //var ms = r.Matches(v.varName);
                                    //if (ms.Count > 0)
                                    //v.varIndex = Convert.ToInt32(ms.ToArray()[0].Value);

                                    // 修改代码
                                    string temp = Convert.ToString(row.GetCell(j));
                                    v.varOffset = GetNumbersFromString(temp);
                                    //v.varOffset = Convert.ToInt32(row.GetCell(j));
                                   
                                }

                            }
                            else if (j == getCellIndexByName(headerRow, "点位名"))
                            {
                                v.varAnnotation = Convert.ToString(row.GetCell(j));

                            }
                            else if (j == getCellIndexByName(headerRow, "数据类型"))
                            {
                                v.varType = Convert.ToString(row.GetCell(j));

                            }
                            else if (j == getCellIndexByName(headerRow, "倍率"))
                            {
                                string temp = Convert.ToString(row.GetCell(j));
                                v.varMagnification = GetNumbersFromString(temp);
                            }
                        }
                    }
                }

                retList.Add(v);
            }

            return retList.ToArray(); ;
        }

        //从Excel中读取1秒的数据信息
        public OneSecInfoStruct_MC[] ReadOneSecInfo_Excel(XSSFWorkbook xssWorkbook, string sheetName,bool isHexadecimal)
        {
            DataTable dtTable = new DataTable();
            List<string> rowList = new List<string>();



            //sheet = xssWorkbook.GetSheetAt(0);
            ISheet sheet = xssWorkbook.GetSheet(sheetName);
            if (sheet == null)
            {
                Console.WriteLine(sheetName + "页不存在");
                return null;

            }


            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            List<OneSecInfoStruct_MC> retList = new List<OneSecInfoStruct_MC>();


            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                {
                    dtTable.Columns.Add(cell.ToString());
                }
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                string str = Convert.ToString(row.GetCell(1));
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) continue;


                var v = new OneSecInfoStruct_MC();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (j == getCellIndexByName(headerRow, "地址/标签"))
                    {
                        v.varName = Convert.ToString(row.GetCell(j).StringCellValue).Trim();
                    }
                    else if (j == getCellIndexByName(headerRow, "偏移地址"))
                    {
                        //v.varOffset = Convert.ToInt32(Convert.ToString(row.GetCell(j)),16);

                        // 修改代码
                        string temp = Convert.ToString(row.GetCell(j));
                        v.varOffset = GetNumbersFromString(temp);
                    }
                    else if (j == getCellIndexByName(headerRow, "点位名"))
                    {
                        v.varAnnotation = Convert.ToString(row.GetCell(j));

                    }
                    else if (j == getCellIndexByName(headerRow, "数据类型"))
                    {
                        v.varType = Convert.ToString(row.GetCell(j));
                    }
                }
                retList.Add(v);
            }

            return retList.ToArray();
        }

        public OneSecAlarmStruct_MC[] ReadOneSecAlarm_Excel(XSSFWorkbook xssWorkbook, string sheetName)
        {
            DataTable dtTable = new DataTable();
            List<string> rowList = new List<string>();



            //sheet = xssWorkbook.GetSheetAt(0);
            ISheet sheet = xssWorkbook.GetSheet(sheetName);
            if (sheet == null)
            {
                Console.WriteLine(sheetName + "页不存在");
                return null;

            }


            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            List<OneSecAlarmStruct_MC> retList = new List<OneSecAlarmStruct_MC>();


            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                {
                    dtTable.Columns.Add(cell.ToString());
                }
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                string str = Convert.ToString(row.GetCell(1));
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) continue;


                var v = new OneSecAlarmStruct_MC();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (j == getCellIndexByName(headerRow, "地址/标签"))
                    {
                        v.varName = Convert.ToString(row.GetCell(j).StringCellValue).Trim();
                    }
                    else if (j == getCellIndexByName(headerRow, "偏移地址"))
                    {
                        //v.varOffset = Convert.ToDouble(row.GetCell(j));

                        //修改代码
                        string temp = Convert.ToString(row.GetCell(j));                      
                        v.varOffset = Convert.ToDouble(temp);
                    }
                    else if (j == getCellIndexByName(headerRow, "点位名"))
                    {
                        v.varAnnotation = Convert.ToString(row.GetCell(j));

                    }
                    else if (j == getCellIndexByName(headerRow, "数据类型"))
                    {
                        v.varType = Convert.ToString(row.GetCell(j));
                    }
                }
                retList.Add(v);
            }

            return retList.ToArray();
        }


        //从Excel中读取DeviceInfo的数据信息 
        public DeviceInfoConSturct_MC[] ReadOneDeviceInfoConSturctInfo_Excel(XSSFWorkbook xssWorkbook, string sheetName, string columnName)
        {

            DataTable dtTable = new DataTable();
            List<string> rowList = new List<string>();


            ISheet sheet = xssWorkbook.GetSheet(sheetName);
            if (sheet == null)
            {
                Console.WriteLine(sheetName + "页不存在");
                return null;

            }


            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            List<DeviceInfoConSturct_MC> retList = new List<DeviceInfoConSturct_MC>();


            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                {
                    dtTable.Columns.Add(cell.ToString());
                }
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                int columnNumber = getCellIndexByName(headerRow, columnName);
                string str = Convert.ToString(row.GetCell(columnNumber));
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) continue;

                var v = new DeviceInfoConSturct_MC();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (j == getCellIndexByName(headerRow, "工位信号"))
                    {
                        v.stationNumber = Convert.ToInt32(row.GetCell(j).NumericCellValue);
                    }
                    else if (j == getCellIndexByName(headerRow, "工位名称"))
                    {
                        v.stationName = Convert.ToString(row.GetCell(j));
                    }
                    else if (j == (columnNumber))
                    {
                        string temp = Convert.ToString(row.GetCell(j));
                        //varName
                        v.varName = temp;

                        //varOffset
                        v.varOffset = GetNumbersFromString(temp); 

                        //varType
                        temp = Convert.ToString(headerRow.GetCell(j));
                        if (!(string.IsNullOrEmpty(temp) || string.IsNullOrWhiteSpace(temp)))
                        {
                            Regex r = new Regex(@"\((\w+)\)");
                            var ms = r.Matches(getNewString(temp));
                            if (ms.Count > 0)
                                v.varType = ms.ToArray()[0].Groups[1].Value;

                        }
                    }
                }
                retList.Add(v);
            }
            return retList.ToArray();
        }




       

        /// <summary>
        /// 文件是否被打开
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //public static bool IsFileOpen(string path)
        //{
        //    if (!File.Exists(path))
        //    {
        //        return false;
        //    }
        //    IntPtr vHandle = _lopen(path, OF_READWRITE | OF_SHARE_DENY_NONE);//windows Api上面有定义扩展方法
        //    if (vHandle == HFILE_ERROR)
        //    {
        //        return true;
        //    }
        //    CloseHandle(vHandle);
        //    return false;
        //}

        public static string getNewString(String Node)
        {
            String newNode = null;
            String allConvertNode = null;
            if (Node.Contains("（") && Node.Contains("）"))
            {
                newNode = Node.Replace("（", "(");
                allConvertNode = newNode.Replace("）", ")");
            }
            else if (!(Node.Contains("（")) && Node.Contains("）"))
            {
                allConvertNode = Node.Replace("）", ")");
            }
            else if (Node.Contains("（") && !(Node.Contains("）")))
            {
                newNode = Node.Replace("（", "(");
                allConvertNode = newNode;
            }
            else
            {
                allConvertNode = Node;
            }
            return allConvertNode;
        }


        //读取封装设备信息
        public DeviceInfoStruct_IEC[] ReadDeviceInfo_Excel(XSSFWorkbook xssWorkbook, string sheetName)
        {
            List<DeviceInfoStruct_IEC> deviceInfoStruct_IEC = new List<DeviceInfoStruct_IEC>();
            try
            {
                DataTable dtTable = new DataTable();
                List<string> rowList = new List<string>();
                ISheet sheet = xssWorkbook.GetSheet(sheetName.Trim());
                if (sheet == null)
                {
                    Console.WriteLine(sheetName + "页不存在");
                    return null;
                }

                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;


                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    {
                        dtTable.Columns.Add(cell.ToString());
                    }
                }
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    DeviceInfoStruct_IEC v = new DeviceInfoStruct_IEC();

                    string str = Convert.ToString(row.GetCell(0));
                    if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) continue;

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(j).ToString()) && !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                            {

                                if (j == 0)
                                {
                                    v.strDeviceName = string.IsNullOrEmpty(Convert.ToString(row.GetCell(j))) ? " " : Convert.ToString(row.GetCell(j));

                                }
                                else if (j == 1)
                                {
                                    v.strDeviceCode = string.IsNullOrEmpty(Convert.ToString(row.GetCell(j))) ? " " : Convert.ToString(row.GetCell(j));

                                }
                                else if (j == 2)
                                {
                                    v.strPLCType = string.IsNullOrEmpty(Convert.ToString(row.GetCell(j))) ? " " : Convert.ToString(row.GetCell(j));

                                }
                                else if (j == 3)
                                {
                                    v.strProtocol = string.IsNullOrEmpty(Convert.ToString(row.GetCell(j))) ? " " : Convert.ToString(row.GetCell(j));

                                }
                                else if (j == 4)
                                {
                                    v.strIPAddress = string.IsNullOrEmpty(Convert.ToString(row.GetCell(j))) ? " " : Convert.ToString(row.GetCell(j));

                                }
                                else if (j == 5)
                                {
                                    v.iPort = Convert.ToInt32 (row.GetCell(j).NumericCellValue);//这里超出int16的范围  

                                }
                                else if (j == 6)
                                {
                                    v.iStationCount = Convert.ToInt16(row.GetCell(j).NumericCellValue);

                                }
                            }
                        }
                    }
                    deviceInfoStruct_IEC.Add(v);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return deviceInfoStruct_IEC.ToArray(); ;


        }
         

        /// <summary>
        /// 根据首行单元格的值获取此单元格所在的列索引
        /// </summary>
        /// <param name="headerRow">首行</param>
        /// <param name="cellValue">单元格的值</param>
        /// <returns>-1：获取失败；正整数为单元格所在的列索引</returns>
        public int getCellIndexByName(IRow row, string cellValue)
        {

            int result = -1;

            int cellCount = row.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = row.GetCell(j);
                if (string.Equals(cell.StringCellValue.Trim(), cellValue))
                {
                    result = j;
                    return result;
                }
            }

            return result;
        }
         

        /// <summary>
        /// 从字符串中获取数字
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>字符串中的数据 -1：未获取到字符串中的数据</returns>
        public int GetNumbersFromString(string str)
        {
            int result = -1;
            string strNum = string.Empty;

            //取出字符串中所有的数字   
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
            { 
                strNum = Regex.Replace(str, "[a-z]", "", RegexOptions.IgnoreCase); 
            }


            if (string.IsNullOrEmpty(strNum) || string.IsNullOrWhiteSpace(strNum) || !StringIsNumbers(strNum))
            {
                result = -1;
            }
            else
            {
                result = Convert.ToInt32(strNum);
            }

            return result;
        }

        /// <summary>
        /// 判断字符串是否为纯数字字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>true：字符串是纯数字字符串 false：字符串不是纯数字字符串</returns>
        public bool StringIsNumbers(string strNum)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(strNum) && !string.IsNullOrWhiteSpace(strNum))
            {
                foreach (char chrTemp in strNum)
                {
                    if (!Char.IsNumber(chrTemp))
                    {
                        result = false;
                    }
                }
            }
            else
            {
                result = false;

            }
           

            return result;
        }





        /// <summary>
        /// 往Excel指定列写数据
        /// </summary>
        /// <param name="ExcelPath">excel文件路径</param>
        /// <param name="sheetname">Excel sheet名字</param>
        /// <param name="columnName">要写入列的名称（从0开始）</param>
        /// <param name="value">写入的数据（数组）</param>
        /// <returns></returns>
        public bool setExcelCellValue(String ExcelPath, String sheetname, string columnName, object value)
        {
            bool returnb = false;
            XSSFWorkbook wk = null;
            try
            {
                //读取Excell
                using (FileStream stream = new FileStream(ExcelPath, FileMode.Open))
                {
                    stream.Position = 0;
                    wk = new XSSFWorkbook(stream);
                    stream.Close();  //把xls文件读入workbook变量里，之后就可以关闭了
                }

                //写值到sheet
                ISheet sheet = wk.GetSheet(sheetname);
                IRow headerRow = sheet.GetRow(0);
                int column = getCellIndexByName(headerRow, columnName);

                if (value.GetType() == typeof(stringStruct[]))
                {
                    stringStruct[] values = (stringStruct[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (i < values.Length)
                        {
                            sheet.GetRow(i + 1).GetCell(column).SetCellValue(values[i].str);
                        }
                        else
                        {
                            sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                        }

                    }
                }
                if (value.GetType() == typeof(string[]))
                {
                    string[] values = (string[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue(values[i]);
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }

                        }


                    }
                }
                if (value.GetType() == typeof(StringBuilder[]))
                {
                    StringBuilder[] values = (StringBuilder[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                if (values[i] != null)
                                {
                                    sheet.GetRow(i + 1).GetCell(column).SetCellValue(values[i].ToString());
                                }
                                else
                                {
                                    sheet.GetRow(i + 1).GetCell(column).SetCellValue("");
                                }
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }

                        }


                    }
                }


                if (value.GetType() == typeof(bool[]))
                {
                    bool[] values = (bool[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue(Convert.ToString(values[i]));
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }

                        }

                    }
                }
                if (value.GetType() == typeof(float[]))
                {
                    float[] values = (float[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue(Convert.ToString(values[i]));
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }

                        }

                    }
                }
                if (value.GetType() == typeof(int[]))
                {
                    int[] values = (int[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue(Convert.ToString(values[i]));
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }

                        }

                    }
                }
                if (value.GetType() == typeof(Int16[]))
                {
                    Int16[] values = (Int16[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue(Convert.ToString(values[i]));
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }

                        }

                    }
                }

                if (value.GetType() == typeof(UInt16[]))
                {
                    UInt16[] values = (UInt16[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue(Convert.ToString(values[i]));
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }

                        }

                    }
                }

                if (value.GetType() == typeof(Int32[]))
                {
                    Int32[] values = (Int32[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue(Convert.ToString(values[i]));
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }

                        }

                    }
                }
                if (value.GetType() == typeof(Int64[]))
                {
                    Int64[] values = (Int64[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue(Convert.ToString(values[i]));
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }

                        }

                    }
                }
                if (value.GetType() == typeof(byte[]))
                {
                    byte[] values = (byte[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue(Convert.ToString(values[i]));
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }
                        }

                    }
                }
                if (value.GetType() == typeof(char[]))
                {
                    char[] values = (char[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue(Convert.ToString(values[i]));
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }


                        }

                    }
                }
                if (value.GetType() == typeof(double[]))
                {
                    double[] values = (double[])value;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(column) != null)
                        {
                            if (i < values.Length)
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue(Convert.ToString(values[i]));
                            }
                            else
                            {
                                sheet.GetRow(i + 1).GetCell(column).SetCellValue("ValueIsNull");
                            }

                        }

                    }
                }

                //写入Excell
                using (FileStream stream = File.Create(ExcelPath))
                {
                    wk.Write(stream);
                    stream.Close();
                }


                returnb = true;
            }
            catch (Exception)
            {
                returnb = false;
                throw;
            }

            return returnb;


        }







    }


    

}
