using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbTools
{
    public class Metadata
    {
        public string TableName { get; set; }
        public string Schema { get; set; }
        public List<ColumnSchema> Columns = new List<ColumnSchema>();//{ get; set; }
        public string ToModel()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            sb.AppendLine("namespace Model");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic class MyModel");
            sb.AppendLine("\t{");
            foreach (var i in Columns)
            {
                sb.AppendLine($"\t\t[Column(\"{i.Name}\")]");
                sb.AppendLine($"\t\tpublic {i.DataType} {Tools.Extract(i.Name)} {{ get; set; }}");
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
    public class Tools
    {
        public static string Extract(string value)
        {
            return value.Replace(" ", "").Replace("_", "");
        }
    }
}
