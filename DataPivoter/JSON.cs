
namespace DataPivoter
{


    /// <summary>
    ///   A helper class for generating object graphs in 
    ///   JavaScript notation. Supports pretty printing.
    /// </summary>
    public class JsonHelper
    {





        public static string Serialize(object target)
        {
#if DEBUG
            return Serialize(target, true);
#else
			return Serialize(target, false);
#endif
        }


        public static T Deserialize<T>(string strValue)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(strValue);
        }


        public static System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>> Deserialize(string strValue)
        {
            Newtonsoft.Json.Linq.JArray tV = (Newtonsoft.Json.Linq.JArray)Deserialize<object>(strValue);
            System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>> tR = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>();

            for (int tC = 0; tC <= tV.Count - 1; tC++)
            {
                try
                {

                    string tS = tV[tC].ToString();
                    tS = tS.Replace("[]", "null");

                    tR.Add(Deserialize<System.Collections.Generic.Dictionary<string, string>>(tS));
                }
                catch (System.Exception tE)
                {
                    System.Console.WriteLine(tE);
                    //tR.Add(Deserialize(Of Dictionary(Of String, String))(tV.Item(tC).ToString()))
                }

            } // Next tC

            return tR;
        } // End Function Deserialize 


        //Cynosura.Base.JsonHelper.SerializeUnpretty(target)
        public static string SerializeUnpretty(object target)
        {
            return SerializeUnpretty(target, null);
        }


        public static string SerializeUnpretty(object target, string strCallback)
        {
            return Serialize(target, false, null);
        }


        //Cynosura.Base.JsonHelper.SerializePretty(target)
        public static string SerializePretty(object target)
        {
            return SerializePretty(target, null);
        }


        public static string SerializePretty(object target, string strCallback)
        {
            return Serialize(target, true, strCallback);
        }


        public static string Serialize(object target, bool prettyPrint)
        {
            return Serialize(target, prettyPrint, null);
        }


        public static string Serialize(object target, bool prettyPrint, string strCallback)
        {
            string strResult = null;

            // http://james.newtonking.com/archive/2009/10/23/efficient-json-with-json-net-reducing-serialized-json-size.aspx
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore };
            if (!prettyPrint)
            {
                settings.Formatting = Newtonsoft.Json.Formatting.None;
            }
            else
            {
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            }


            settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            settings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;

            //context.Response.Write(strCallback + " && " + strCallback + "(")

            if (string.IsNullOrEmpty(strCallback))
            {
                strResult = Newtonsoft.Json.JsonConvert.SerializeObject(target, settings);
                // JSONP
            }
            else
            {
                // https://github.com/visionmedia/express/pull/1374
                //strResult = strCallback + " && " + strCallback + "(" + Newtonsoft.Json.JsonConvert.SerializeObject(target, settings) + "); " + System.Environment.NewLine
                //typeof bla1 != "undefined" ? alert(bla1(3)) : alert("foo undefined");
                strResult = "typeof " + strCallback + " != 'undefined' ? " + strCallback + "(" + Newtonsoft.Json.JsonConvert.SerializeObject(target, settings) + ") : alert('Callback-Funktion \"" + strCallback + "\" undefiniert...'); " + System.Environment.NewLine;
            }

            settings = null;
            return strResult;



            //Dim sbSerialized As New StringBuilder()
            //Dim js As New JavaScriptSerializer()

            //js.Serialize(target, sbSerialized)

            //If prettyPrint Then
            //    Dim prettyPrintedResult As New StringBuilder()
            //    prettyPrintedResult.EnsureCapacity(sbSerialized.Length)

            //    Dim pp As New JsonPrettyPrinter()
            //    pp.PrettyPrint(sbSerialized, prettyPrintedResult)

            //    Return prettyPrintedResult.ToString()
            //Else
            //    Return sbSerialized.ToString()
            //End If

        } // End Function Serialize 


        public static System.Collections.Generic.Dictionary<string, object> NvcToDictionary(System.Collections.Specialized.NameValueCollection nvc, bool handleMultipleValuesPerKey = true)
        {
            System.Collections.Generic.Dictionary<string, object> result = new System.Collections.Generic.Dictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                if (handleMultipleValuesPerKey)
                {
                    string[] values = nvc.GetValues(key);
                    if ((values.Length == 1))
                    {
                        result.Add(key, values[0]);
                    }
                    else
                    {
                        result.Add(key, values);
                    }
                }
                else
                {
                    result.Add(key, nvc[key]);
                }
            }
            return result;
        }


        public static System.Collections.Generic.Dictionary<string, object> sessionToDictionary(System.Web.SessionState.HttpSessionState nvc)
        {
            System.Collections.Generic.Dictionary<string, object> result = new System.Collections.Generic.Dictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                result.Add(key, nvc[key]);
            }
            return result;
        }


    }


    // https://stackoverflow.com/questions/4580397/json-formatter-in-c
    public class JsonFormatter
    {
        #region class members
        const string Space = " ";
        const int DefaultIndent = 0;
        const string Indent = Space + Space + Space + Space;
        static readonly string NewLine = System.Environment.NewLine;
        #endregion

        private enum JsonContextType
        {
            Object, Array
        }

        static void BuildIndents(int indents, System.Text.StringBuilder output)
        {
            indents += DefaultIndent;
            for (; indents > 0; indents--)
                output.Append(Indent);
        }


        bool inDoubleString = false;
        bool inSingleString = false;
        bool inVariableAssignment = false;
        char prevChar = '\0';

        System.Collections.Generic.Stack<JsonContextType> context = new System.Collections.Generic.Stack<JsonContextType>();

        bool InString()
        {
            return inDoubleString || inSingleString;
        }

        public string PrettyPrint(string input)
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder(input.Length * 2);
            char c;

            for (int i = 0; i < input.Length; i++)
            {
                c = input[i];

                switch (c)
                {
                    case '{':
                        if (!InString())
                        {
                            if (inVariableAssignment || (context.Count > 0 && context.Peek() != JsonContextType.Array))
                            {
                                output.Append(NewLine);
                                BuildIndents(context.Count, output);
                            }
                            output.Append(c);
                            context.Push(JsonContextType.Object);
                            output.Append(NewLine);
                            BuildIndents(context.Count, output);
                        }
                        else
                            output.Append(c);

                        break;

                    case '}':
                        if (!InString())
                        {
                            output.Append(NewLine);
                            context.Pop();
                            BuildIndents(context.Count, output);
                            output.Append(c);
                        }
                        else
                            output.Append(c);

                        break;

                    case '[':
                        output.Append(c);

                        if (!InString())
                            context.Push(JsonContextType.Array);

                        break;

                    case ']':
                        if (!InString())
                        {
                            output.Append(c);
                            context.Pop();
                        }
                        else
                            output.Append(c);

                        break;

                    case '=':
                        output.Append(c);
                        break;

                    case ',':
                        output.Append(c);

                        if (!InString() && context.Peek() != JsonContextType.Array)
                        {
                            BuildIndents(context.Count, output);
                            output.Append(NewLine);
                            BuildIndents(context.Count, output);
                            inVariableAssignment = false;
                        }

                        break;

                    case '\'':
                        if (!inDoubleString && prevChar != '\\')
                            inSingleString = !inSingleString;

                        output.Append(c);
                        break;

                    case ':':
                        if (!InString())
                        {
                            inVariableAssignment = true;
                            output.Append(Space);
                            output.Append(c);
                            output.Append(Space);
                        }
                        else
                            output.Append(c);

                        break;

                    case '"':
                        if (!inSingleString && prevChar != '\\')
                            inDoubleString = !inDoubleString;

                        output.Append(c);
                        break;
                    case ' ':
                        if (InString())
                            output.Append(c);
                        break;

                    default:
                        output.Append(c);
                        break;
                }
                prevChar = c;
            }

            return output.ToString();
        }
    }


}
