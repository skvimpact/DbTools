using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbTools
{
    public class Selector
    {
        public string Script { get; set; }
        public DbParameter[] Parameters { get; set; }
        public CommandType CommandType { get; set; }
    }
}
