using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Editor
{
    /// <summary>
    /// Provides functionality for converting json and text
    /// </summary>
    public class Linter
    {

        public Linter()
        {
        }

        /// <summary>
        /// Get Json from text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public JsonObject? ParseText(string text)
        {
            try
            {
                var cleanText = CleanText(text);

                var bytes = Encoding.UTF8.GetBytes(cleanText);

                return JsonSerializer.Deserialize<JsonObject>(bytes);
            }
            catch (Exception)
            {
                //Not json, return null
            }
            return null;
        }

        /// <summary>
        /// Remove unwanted characters from input text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string CleanText(string text)
        {
            //TODO: Add regex for unwanted chars or possibly remove bytes of chars
            text.Replace("//", "");
            text = text.ReplaceLineEndings("");

            return text;
        }

        /// <summary>
        /// Parses basic System Types from Json data values
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Type ParseType(JsonNode value)
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
                catch (Exception ex) { } //Todo: better handling

                try
                {

                    if (JsonObject.Parse(value.ToString()).GetType() == typeof(JsonObject))
                    {
                        return typeof(JsonObject);
                    }

                }
                catch (Exception ex)
                {
                    //Todo: better handling
                }
            }


            return typeof(string);
        }

        /// <summary>
        /// Determines if it's just a property or
        /// if more processing is needed for a nested json object
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsJustProperty(Type type)
        {
            if (type == typeof(JsonArray) || type == typeof(JsonObject))
                return false;
            return true;
        }

        /// <summary>
        /// Converts a .Net type name to a C# type name. 
        /// It will remove the "System." namespace, if present
        /// </summary>
        /// <param name="dotNetTypeName"></param>
        /// <param name="isNull"></param>
        /// <returns></returns>
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