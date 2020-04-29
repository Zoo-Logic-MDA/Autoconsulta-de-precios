using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;



namespace Autoconsulta_de_precios
{
    class GestionJson
    {
        public string DesarmarUnJson<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            //string jsonString = Encoding.BigEndianUnicode.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public DateTime ConvertJsonStringToDateTime(string jsonTime)
        {
            if (!string.IsNullOrEmpty(jsonTime) && jsonTime.IndexOf("Date") > -1)
            {
                string milis = jsonTime.Substring(jsonTime.IndexOf("(") + 1);
                string sign = milis.IndexOf("+") > -1 ? "+" : "-";
                string hours = milis.Substring(milis.IndexOf(sign));
                milis = milis.Substring(0, milis.IndexOf(sign));
                hours = hours.Substring(0, hours.IndexOf(")"));
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Convert.ToInt64(milis)).AddHours(Convert.ToInt64(hours) / 100);
            }

            return DateTime.Now;
        }

        public T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        public string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            //string jsonString = Encoding.BigEndianUnicode.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public string ConvertDateStringToJsonDate(DateTime dt )
        {
            string result = string.Empty;
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("/Date({0}+0800)/", ts.TotalMilliseconds);
            return result;
        }
    
    }
}
