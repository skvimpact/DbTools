using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbTools
{
    public class ColumnSchema
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public int Size { get; set; }
        public bool AllowDBNull { get; set; }
        public bool IsFloat => CLRType == "double";

        public string CLRType =>
            DataType.Contains("Double") || DataType.Contains("Decimal") ? "double" :
            DataType.Contains("DateTime") ? "DateTime" :
            DataType.Contains("Int32") ? "int" :
            DataType.Contains("Int16") ? "Int16" :
            DataType.Contains("Byte[]") ? "byte[]" :
            DataType.Contains("Byte") ? "byte" :
            DataType.Contains("String") ? "string" : "unknown";

        public string CLRTypeNullable => string.Concat(CLRType, AllowDBNull && CLRType != "string" ? "?" : "");

        public string Default =>
            CLRType == "string" ? "\"\"" :
            CLRType == "DateTime" ? "NavDate.Blank" :
            CLRType == "bool" ? "false" : "0";
    }
}
