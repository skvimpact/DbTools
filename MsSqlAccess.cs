using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbTools
{
    public class MsSqlAccess : DataBaseAccess<SqlConnection, SqlCommand>//, IDataBaseAccess
    {
        public MsSqlAccess(string connectionString) : base(connectionString) { }
    }
}