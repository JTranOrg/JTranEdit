
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace JTranEdit
{
    /****************************************************************************/
    /****************************************************************************/
    public class JsonFormatter
    {
        /****************************************************************************/
        public JsonFormatter()
        {
        }

        public int Indent { get; set; } = 4;

        /****************************************************************************/
        public string Format(ExpandoObject obj)
        {            
            var sb = new StringBuilder();

            Format(obj, sb, true, 0);

            return sb.ToString();
        }

        /****************************************************************************/
        private void AppendLine(StringBuilder sb, string text, int indent)
        {
            sb.Append("".PadLeft(indent * this.Indent));
            sb.AppendLine(text);
        }
        
        /****************************************************************************/
        private void Append(StringBuilder sb, string text, int indent)
        {
            sb.Append("".PadLeft(indent * this.Indent));
            sb.Append(text);
        }
        
        /****************************************************************************/
        private void Format(ExpandoObject obj, StringBuilder sb, bool last, int indent)
        {
            if(obj == null)
                return;

            AppendLine(sb, "{", indent);

            var dict     = (obj as IDictionary<string, object>).Where( kv=> !kv.Key.StartsWith("_jtran_") );
            var index    = 0;
            var numItems = dict.Count();

            foreach(var kv in dict)
            {
                var comma = (index == numItems-1) ? "" : ",";

                Append(sb, kv.Key + ":", indent + 1);

                Format(kv.Value, sb, comma, indent + 1);

                ++index;
            }

            AppendLine(sb, "}" + (last ? "" : ","), indent);
        }

        /****************************************************************************/
        private void Format(object value, StringBuilder sb, string comma, int indent)
        {
            if(value == null)
            {
                AppendLine(sb, " null" + comma, indent);
            }
            else if(value is IList list)
            {
                AppendLine(sb, "", indent);
                AppendLine(sb, "[", indent);

                var childIndex = 0;
                var numChildItems = list.Count;

                foreach(var child in list)
                {
                    var childComma = (++childIndex == numChildItems) ? "" : ",";

                    Format(child, sb, childComma, indent+1);
                }

                AppendLine(sb, "]" + comma, indent);
            }
            else if(value is ExpandoObject expando)
            {
                AppendLine(sb, "", indent);
                Format(expando, sb, comma == "", indent);                   
            }
            else
            {                       
                if(value is bool)
                    sb.AppendLine(" " + value.ToString().ToLower() + comma);
                else if(value is long)
                    sb.AppendLine(" " + value.ToString() + comma);
                else if(bool.TryParse(value.ToString(), out bool bval))
                    sb.AppendLine(" " + bval.ToString().ToLower() + comma);
                else if(value is DateTime dtVal)
                    sb.AppendLine(" \"" + dtVal.ToString("o") + "\"" + comma);
                else
                    sb.AppendLine(" \"" + value.ToString() + "\"" + comma);
            }
        }
    }
}
