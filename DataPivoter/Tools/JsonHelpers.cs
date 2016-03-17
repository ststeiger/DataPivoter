
namespace DataPivoter
{


    public static class JsonHelpers
    {


        public static void AppendByteToFile(string fileName, byte[] byteData)
        {

            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Append, System.IO.FileAccess.Write))
            {
                fs.Write(byteData, 0, byteData.Length);
                fs.Flush();
                fs.Close();
            }

        }


        // DataPivoter.JsonHelpers.SerializeToFile("file", dt);
        public static void SerializeToFile(string fileName, object obj)
        {
            
            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
            {
                SerializeToStreamAndClose(obj, fs);
                fs.Close();
            }

        }


        public static void SerializeToStreamAndClose(object value, System.IO.Stream s)
        {
            SerializeToStreamAndClose(value, s, true);
        }

        public static void SerializeToStreamAndClose(object value, System.IO.Stream s, bool prettyPrint)
        {
            Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();
            // ser.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(s))
            {
                using (Newtonsoft.Json.JsonTextWriter jsonWriter = new Newtonsoft.Json.JsonTextWriter(writer))
                {
                    jsonWriter.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                    jsonWriter.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                    

                    if (prettyPrint)
                        jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;

                    ser.Serialize(jsonWriter, value);
                    jsonWriter.Flush();
                }

            }

            ser = null;
        }

        public static object DeserializeFromStreamAndClose<T>(System.IO.Stream stream)
        {
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

            using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
            {
                using (Newtonsoft.Json.JsonTextReader jsonTextReader = new Newtonsoft.Json.JsonTextReader(sr))
                {
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }


        public static void Serialize(object value, System.IO.Stream s)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(s);
            Newtonsoft.Json.JsonTextWriter jsonWriter = new Newtonsoft.Json.JsonTextWriter(writer);
            Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();
            ser.Serialize(jsonWriter, value);
            jsonWriter.Flush();
        }


        public static T Deserialize<T>(System.IO.Stream s)
        {
            Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();
            System.IO.StreamReader reader = new System.IO.StreamReader(s);
            Newtonsoft.Json.JsonTextReader jsonReader = new Newtonsoft.Json.JsonTextReader(reader);

            return ser.Deserialize<T>(jsonReader);
        }


        public static T DeserializeFromJsonStream<T>(System.IO.Stream stream)
        {
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            T data;
            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(stream))
            {
                data = (T)serializer.Deserialize(streamReader, typeof(T));
            }
            return data;
        }


        public static T DeserializeFromJsonString<T>(string json)
        {
            T data;
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(System.Text.Encoding.Default.GetBytes(json)))
            {
                data = DeserializeFromJsonStream<T>(stream);
            }
            return data;
        }


        public static T DeserializeFromJsonFile<T>(string fileName)
        {
            T data;
            using (System.IO.FileStream fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                data = DeserializeFromJsonStream<T>(fileStream);
            }
            return data;
        }


    }


}
