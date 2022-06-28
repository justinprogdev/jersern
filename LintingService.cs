using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Editor
{
    internal class LintingService
    {

        public LintingService()
        {
        }

        internal JsonObject? ParseText(string text)
        {
            try
            {
                var cleanText = CleanText(text);
                
               
                var bytes = Encoding.UTF8.GetBytes(cleanText);
             
                return JsonSerializer.Deserialize<JsonObject>(bytes);
            }
            catch (Exception)
            {

            }
            return null;
        }

        private string CleanText(string text)
        {
            //while (text.Contains("//"))
            //{
            //    var startIndex = text.IndexOf('/');
            //    var endIndex = text.IndexOf('\n', startIndex);
            //    text = text.Replace(text.Substring(startIndex, endIndex - startIndex), "");

            //}
            text.Replace("//", " ");
            text = text.ReplaceLineEndings("");
            //while (text.Contains("\\"))
            //{
            //    var startIndex2 = text.IndexOf('\\');
            //    var endIndex2 = text.IndexOf('\n', startIndex2);
            //    text = text.Replace(text.Substring(startIndex2, endIndex2 - startIndex2), "");
            //}
            Console.WriteLine(text);
            return text;
        }

        internal Type ParseType(JsonNode value)
        {
            if (value == null)
            {
                return typeof(string);
            }
            else if (int.TryParse(value.ToString(), out var type1))
            {
                return typeof(int);
            }
            else if (float.TryParse(value.ToString(), out var type2))
            {
                return type2.GetType();
            }
            else if (decimal.TryParse(value.ToString(), out var type3))
            {
                return type3.GetType();
            }
            else if (double.TryParse(value.ToString(), out var type4))
            {
                return type4.GetType();
            }
            else
            {
                try
                {

                    if (JsonArray.Parse(value.ToString()).GetType() == typeof(JsonArray))
                    {
                        return typeof(JsonArray);
                    }

                }
                catch (Exception ex) { }

                try
                {

                    if (JsonObject.Parse(value.ToString()).GetType() == typeof(JsonObject))
                    {
                        return typeof(JsonObject);
                    }

                }
                catch (Exception ex)
                {

                }
            }


            return typeof(string);
        }

        /// <summary>
        /// Determines if it's just a property or
        /// a if more processing is needed
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal bool IsJustProperty(Type type)
        {
            if (type == typeof(JsonArray) || type == typeof(JsonObject))
                return false;
            return true;
        }

        /// <summary>Converts a .Net type name to a C# type name. It will remove the "System." namespace, if present,</summary>
        public string GetCSharpType(string dotNetTypeName, bool isNull = false)
        {
            string cstype = "";
            string nullable = isNull ? "?" : "";
            string prefix = "System.";
            string typeName = dotNetTypeName.StartsWith(prefix) ? dotNetTypeName.Remove(0, prefix.Length) : dotNetTypeName;

            switch (typeName)
            {
                case "Boolean": cstype = "bool"; break;
                case "Byte": cstype = "byte"; break;
                case "SByte": cstype = "sbyte"; break;
                case "Char": cstype = "char"; break;
                case "Decimal": cstype = "decimal"; break;
                case "Double": cstype = "double"; break;
                case "Single": cstype = "float"; break;
                case "Int32": cstype = "int"; break;
                case "UInt32": cstype = "uint"; break;
                case "Int64": cstype = "long"; break;
                case "UInt64": cstype = "ulong"; break;
                case "Object": cstype = "object"; break;
                case "Int16": cstype = "short"; break;
                case "UInt16": cstype = "ushort"; break;
                case "String": cstype = "string"; break;

                default: cstype = typeName; break; // do nothing
            }
            return $"{cstype}{nullable}";

        }
    }
}