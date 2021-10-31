using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbTools
{

    public abstract class DataBaseAccess<TConnection, TCommand>
        where TConnection : DbConnection, new()
        where TCommand : DbCommand, new()
    {
        public string ConnectionString { get; }

        protected TConnection _connection
        {
            get
            {
                var connection = new TConnection
                {
                    ConnectionString = ConnectionString
                };
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return connection;
            }
        }

        private TConnection _conn { get; }

        protected TConnection _c
        {
            get
            {
                if (_conn.State != ConnectionState.Open)
                    _conn.Open();
                return _conn;
            }
        }

        private TCommand _command(DbConnection connection, string commandText) =>
            new TCommand
            {
                Connection = connection,
                CommandText = commandText,
                CommandTimeout = 600
            };

        public DataBaseAccess(string connectionString)
        {
            ConnectionString = connectionString;
            ////!!!!
            _conn = new TConnection
            {
                ConnectionString = ConnectionString
            };
        }

        public DbDataReader GetDataReader(Selector selector)
        {
            DbDataReader result = default(DbDataReader);
            try
            {
                var command = _command(_c, selector.Script);
                if ((selector.Parameters?.Length ?? 0) > 0)
                    command.Parameters.AddRange(selector.Parameters);

                command.CommandType = selector.CommandType != 0 ? selector.CommandType : CommandType.Text;
                result = command.ExecuteReader(CommandBehavior.KeyInfo);// CloseConnection);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Failed to GetDataReader for " + selector.Script, ex, selector.Parameters);
                //Log.Error(ex, "Failed to GetDataReader");
                throw;
            }

            return result;
        }

        public int ExecuteNonQuery(string script)
        {
            int result = -1;
            try
            {
                var command = _command(_c, script);
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Failed to GetDataReader for " + script);
                //Log.Error(ex, "Failed to GetDataReader");
                throw;
            }
            return result;
        }
    }
}
