using System;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace DigitalFenceMonitor
{
    public class cmsAccount
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string autorition { get; set; }
    }

    public class cmsAreaSet
    {
        public int ID { get; set; }
        public string IPport { get; set; }
        public string AreaNum { get; set; }
        public string Status { get; set; }
        public string Describe { get; set; }
        public string Map { get; set; }
        public string DeviceType { get; set; }
    }

    public class cmsContent
    {
        public int ID { get; set; }
        public string NowDate { get; set; }
        public string NowTime { get; set; }
        public string StationIP { get; set; }
        public string StationName { get; set; }
        public string AreaNum { get; set; }
        public string AlertType { get; set; }
        public string Descripe { get; set; }
        public string OperationInfo { get; set; }
        public string Operator { get; set; }
        public string OperationResult { get; set; }
    }

    public class cmsStationSet
    {
        public int ID { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string StaName { get; set; }
        public string IPandPort { get; set; }
    }


    public class cmsLinkageAlert
    {
        public int ID { get; set; }
        public string LinkInfo { get; set; }
        public string IpPortNodes { get; set; }
    }


    public class cmsMap
    {
        public int ID { get; set; }
        public int PointX { get; set; }
        public int PointY { get; set; }
        public string MapInfo { get; set; }
    }


    public class cmsDayNightAlert
    {
        public int ID { get; set; }
        public string IpPortNum { get; set; }
        public string Status { get; set; }
    }

    public class cmsRegister
    {
        public int ID { get; set; }
        public int SpeedAdj { get; set; }
        public string HoyoSerial { get; set; }
        public string RegDate { get; set; }
        public string LastIp { get; set; }
        public string LastPort { get; set; }
        public string Master { get; set; }
    }


    public class cmsDataModel
    {
        private string path;

        public List<cmsAccount> Account = new List<cmsAccount>();
        public List<cmsAreaSet> AreaSet = new List<cmsAreaSet>();
        public List<cmsContent> Content = new List<cmsContent>();
        public List<cmsStationSet> StationSet = new List<cmsStationSet>();
        public List<cmsLinkageAlert> LinkageAlert = new List<cmsLinkageAlert>();
        public List<cmsMap> Map = new List<cmsMap>();
        public List<cmsDayNightAlert> DayNightAlert = new List<cmsDayNightAlert>();
        public cmsRegister cmsRegister = new cmsRegister();

        public void DataModelInit()
        {
            string JsonPath = "cmsModel.json";
            string userDocument = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\hoyoCMS";
            if (!Directory.Exists(@userDocument))
            {
                Directory.CreateDirectory(@userDocument);
            }
            string realPath = userDocument + "\\" + JsonPath;
            this.path = realPath;
        }

        public void write(string output)
        {
            FileStream fs = new FileStream(this.path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(formatJson(output));
            sw.Close();
            fs.Close();
        }

        private string formatJson(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }


        public string ReadJson()
        {
            string MapPath = this.path;
            try
            {
                // if (File.Exists(MapPath))
                FileStream fs = new FileStream(@MapPath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string readToEnd = sr.ReadToEnd();
                return readToEnd;
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

    }


    /// <summary>
    /// Json帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }



        public static DataTable ToDataTable(object o)
        {
            string json = JsonHelper.SerializeObject(o);

            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);

                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        //Columns
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        //Rows
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }

        public static bool IsRexMatched(string str, string regRaw)
        {
            Regex reg = new Regex(regRaw);
            MatchCollection result = reg.Matches(str);
            if (result.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
    }
}
