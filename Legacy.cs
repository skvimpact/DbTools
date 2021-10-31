using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbTools
{
    class Legacy
    {
    }
    public class CompanySrv

    {
        /*
        private IOltpCompanyRepo repoOltp;

        private IDwhCompanyRepo repoDwh;

        private IDictionary<string, int?> items = new Dictionary<string, int?>();

        public CompanySrv(IOltpCompanyRepo repoOltp, IDwhCompanyRepo repoDwh)

        {

            this.repoOltp = repoOltp;

            this.repoDwh = repoDwh;

        }



        public void LoadDhw() =>

            repoDwh.AddRange(repoOltp.Companies.ToList().Except(repoDwh.Companies.ToList(), new CompanyComparer()));



        public void LoadCash() =>

            repoDwh.Companies.ToList().ForEach(c => items[c.Name] = c.Sk);



        public int? this[string nk]

        {

            get => nk == null ? null : items.TryGetValue(nk.Trim(), out int? sk) ? sk : null;

        }
        */
    }


    /*
    public class ColumnSchema

    {

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



        public string CLRTypeNullable => CLRType + (AllowDBNull && CLRType != "string" ? "?" : "");



        public string Default =>

            CLRType == "string" ? "\"\"" :

            CLRType == "DateTime" ? "NavDate.Blank" :

            CLRType == "bool" ? "false" : "0";

    }
    */
    /*
    public partial class Metadata

    {

        public string Class

        {

            get

            {

                StringBuilder sb = new StringBuilder();

                //sb.AppendLine($"    [Table(\"{TableName}\", Schema =\"{Schema}\")]");

                sb.AppendLine($"    public partial class {Model} : IModel");

                sb.AppendLine("    {");

                Column2Schema.

                    //OrderBy(i => i.Key).

                    ToList().

                    ForEach(i => {

                        sb.AppendLine($"        [Column(\"{i.Key}\")]");

                        sb.AppendLine($"        public {i.Value.CLRTypeNullable} {i.Key.ClearColumn()} {{ get; set; }}");

                    });

                sb.AppendLine("    }");

                return sb.ToString();

            }

        }



        public void ClassToFile()

        {

            Encoding MyEncoding = Encoding.GetEncoding("utf-8");

            StringBuilder sb = new StringBuilder();



            sb.AppendLine("using System;");

            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");

            sb.AppendLine($"namespace {String.Join(".", Path)}");

            sb.AppendLine("{");

            sb.AppendLine($"{Class}");

            sb.AppendLine("}");

            var tt = $"../{String.Join("/", Path)}/{Model}.cs";

            File.WriteAllText($"../{String.Join("/", Path)}/{Model}.cs", sb.ToString(), MyEncoding);

        }

    }
    */
    /*
    public partial class Metadata

    {

        public string Model { get; }

        public string[] Path { get; }

        public string ConnectionString { get; protected set; }

        public string TableName { get; set; }

        public string Schema { get; set; }

        public virtual IDictionary<string, ColumnSchema> Column2Schema { get; } = new Dictionary<string, ColumnSchema>();

        public Metadata(string model, string[] path, DbDataReader reader)

        {

            Model = model;

            Path = path;

            if (reader != null)

            {

                var schemaTable = reader.GetSchemaTable();

                //DataColumn dcCatalogName = schemaTable.Columns[schemaTable.Columns.IndexOf("BaseCatalogName")];

                DataColumn dcSchemaName = schemaTable.Columns[schemaTable.Columns.IndexOf("BaseSchemaName")];

                DataColumn dcTableName = schemaTable.Columns[schemaTable.Columns.IndexOf("BaseTableName")];

                DataColumn dcName = schemaTable.Columns[schemaTable.Columns.IndexOf("ColumnName")];

                DataColumn dcSize = schemaTable.Columns[schemaTable.Columns.IndexOf("ColumnSize")];//!!

                DataColumn dcType = schemaTable.Columns[schemaTable.Columns.IndexOf("DataType")];

                DataColumn dcNull = schemaTable.Columns[schemaTable.Columns.IndexOf("AllowDBNull")];



                bool fieldNull;

                string fieldType;

                string fieldName;



                //Catalog = schemaTable.Rows[0][dcCatalogName].ToString();

                TableName = schemaTable.Rows[0][dcTableName].ToString();

                Schema = schemaTable.Rows[0][dcSchemaName].ToString();



                foreach (DataRow field in schemaTable.Rows)

                {

                    //Catalog = field[dcCatalogName].ToString();

                    TableName = field[dcTableName].ToString();

                    Schema = field[dcSchemaName].ToString();



                    fieldNull = bool.Parse(field[dcNull].ToString());

                    fieldType = field[dcType].ToString();

                    fieldName = field[dcName].ToString();

                    if (fieldName == "timestamp") continue;

                    Column2Schema[fieldName] = new ColumnSchema

                    {

                        AllowDBNull = bool.Parse(field[dcNull].ToString()),

                        DataType = field[dcType].ToString(),

                        Size = int.Parse(field[dcSize].ToString())

                    };

                }

            }

        }

    }
    */
    /*
    public partial class Metadata<F> : Metadata

    {

        public IDictionary<string, PropertyInfo> Column2Property { get; set; }

        private IDictionary<string, ColumnSchema> column2Schema;

        public IDictionary<string, ColumnSchema> Property2Schema { get; set; }

        public Metadata() : base(typeof(F).Name, typeof(F).Namespace.Split("."), null)

        {

            //var ggg = typeof(F).Namespace.Split(".");

            TableName = AttributeReader.Class<TableAttribute>(typeof(F))?.Name;

            Schema = AttributeReader.Class<TableAttribute>(typeof(F))?.Schema;

            var p2c = AttributeReader.Property2Attribute<ColumnAttribute>(typeof(F));

            // Column2Property = AttributeReader.Property2Attribute<ColumnAttribute>(typeof(F)).

            //            ToDictionary(kvp => kvp.Value.Name, kvp => kvp.Key);

            Column2Property = p2c.ToDictionary(kvp => kvp.Value.Name, kvp => kvp.Key);



            column2Schema = p2c.ToDictionary(kvp => kvp.Value.Name,

                kvp => new ColumnSchema

                {

                    AllowDBNull = kvp.Key.PropertyType.ToString().Contains("Nullable"),

                    DataType = kvp.Key.PropertyType.ToString()

                });



            Property2Schema = p2c.ToDictionary(kvp => kvp.Key.Name,

                kvp => new ColumnSchema

                {

                    AllowDBNull = kvp.Key.PropertyType.ToString().Contains("Nullable"),

                    DataType = kvp.Key.PropertyType.ToString()

                });

        }



        public Metadata(DbContext cnt) : base(typeof(F).Name, null, null)

        {

            ConnectionString = cnt.Database.GetDbConnection().ConnectionString;

            IEntityType entityType = cnt.Model.GetEntityTypes().Where(x => x.ClrType.Equals(typeof(F))).FirstOrDefault();

            TableName = entityType.SqlServer().TableName;

            Schema = entityType.SqlServer().Schema;

            Column2Property = typeof(F).

                        GetProperties(

                            BindingFlags.GetProperty |

                            BindingFlags.Instance |

                            BindingFlags.Public).

                        Select(p =>

                            new

                            {

                                cnt.Model.GetEntityTypes().Where(x => x.ClrType.Equals(typeof(F))).FirstOrDefault().

                                    GetProperties().Where(x => x.Name.Equals(p.Name)).FirstOrDefault()?.

                                    SqlServer().ColumnName,

                                p

                            }).

                        Where(p => p.ColumnName != null).

                        ToDictionary(kvp => kvp.ColumnName, kvp => kvp.p);

        }



        //public void eeee()

        //{

        //    foreach (var t in Column2Property)

        //    {

        //        // var ffff = t.Value.PropertyType.ReflectedType.ToString();

        //        var dddd = t.Value.PropertyType.ToString();

        //    }

        //}





        public override IDictionary<string, ColumnSchema> Column2Schema => column2Schema;

        //Column2Property.ToDictionary(kvp => kvp.Key,

        //    kvp => new ColumnSchema

        //    {

        //        AllowDBNull = kvp.Value.PropertyType.ToString().Contains("Nullable"),

        //        DataType = kvp.Value.PropertyType.ToString()

        //    });





        //public override IDictionary<string, ColumnSchema> Column2Schema

        //public void fff()

        //{

        //    Column2Property.Values.Select(i => i.p)

        //}



    }
    */

    /*
    public partial class Metadata

    {

        public string Select

        {

            get

            {

                var enter = false;

                string cast;

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("select");

                // sb.AppendLine("    top 10");

                Column2Schema.

                    OrderBy(i => i.Key).

                    ToList().

                    ForEach(i => {

                        if (enter)

                            sb.AppendLine(",");

                        else

                            enter = true;

                        cast = i.Value.IsFloat ? $"cast([{i.Key}] as float)" : $"[{i.Key}]";

                        sb.Append($"    {cast}  as   [{i.Key}]");

                    });

                sb.AppendLine();

                sb.AppendLine("from");

                sb.AppendLine($"    [{Schema}].[{TableName}]");

                return sb.ToString();

            }

        }



        public void SelectToFile()

        {

            Encoding MyEncoding = Encoding.GetEncoding("utf-8");

            StringBuilder sb = new StringBuilder();



            //sb.AppendLine($"#define {model.ToUpper()}");

            sb.AppendLine("using System;");

            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");

            sb.AppendLine("namespace NavFeeder.Lib");

            sb.AppendLine("{");

            sb.AppendLine($"    public partial class {Model}");

            sb.AppendLine("     {");

            sb.AppendLine("        public static string Read { get; }");

            sb.AppendLine("");

            sb.AppendLine($"        static {Model}()");

            sb.AppendLine("        {");

            sb.AppendLine("             Read =");

            sb.AppendLine("@\"");

            sb.AppendLine($"{Select}");

            sb.AppendLine("\";");

            sb.AppendLine("        }");

            sb.AppendLine("    }");

            sb.AppendLine("}");

            File.WriteAllText($"../NavFeeder.Lib/Trash/{Model}.Read.cs", sb.ToString(), MyEncoding);

        }

    }
    */
    /*
    public partial class Metadata<F> : Metadata

    {

        public string To<T>(Metadata<T> pto)

        {

            var f = Column2Property.Values.

                OrderBy(p => p.Name).

                Select(p => p.Name).

                ToList();

            var t = pto.Column2Property.Values.

                OrderBy(p => p.Name).

                Select(p => p.Name).

                ToList();

            var x = Column2Schema;







            StringBuilder sb = new StringBuilder();

            var enter = false;

            var uName = typeof(T).Name;

            sb.AppendLine($"        public {uName} To{uName} => new {uName}");

            sb.AppendLine($"        {{");

            t.

            Select(to => new {

                from = f.Find(from => from.ToUpper().Equals(to.ToUpper())),

                to

            }).

            ToList().

            ForEach(i => {

                if (enter)

                    sb.AppendLine(",");

                else

                    enter = true;

                //sb.Append($"            {i.to}  =   {i.from ?? pto.Property2Schema[i.to].Default}");





                var from = i.from != null ?

                (

                    Property2Schema[i.from].AllowDBNull &&

                    !pto.Property2Schema[i.to].AllowDBNull ?



                    $"{i.from} ?? {pto.Property2Schema[i.to].Default}" : i.from

                ) : pto.Property2Schema[i.to].Default;

                sb.Append($"            {i.to}  =   {from}");



            });

            sb.AppendLine();

            sb.AppendLine($"        }};");

            return sb.ToString();

        }



        public void ToToFile<T>(Metadata<T> pto)

        {

            Encoding MyEncoding = Encoding.GetEncoding("utf-8");

            StringBuilder sb = new StringBuilder();



            //sb.AppendLine($"#define {model.ToUpper()}");

            sb.AppendLine("using Library;");

            //sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");

            sb.AppendLine($"using {String.Join(".", pto.Path)};");

            sb.AppendLine($"namespace {String.Join(".", Path)}");

            sb.AppendLine("{");

            sb.AppendLine($"    public partial class {Model}");

            sb.AppendLine("    {");

            sb.AppendLine($"{To(pto)}");

            sb.AppendLine("    }");

            sb.AppendLine("}");

            var c = $"../{String.Join("/", Path)}/{Model}.To.cs";

            File.WriteAllText($"../{String.Join("/", Path)}/{Model}.To.cs", sb.ToString(), MyEncoding);

        }

    }
    */
/*
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

                result = command.ExecuteReader(CommandBehavior.CloseConnection);

            }

            catch (Exception ex)

            {

                //Console.WriteLine("Failed to GetDataReader for " + selector.Script, ex, selector.Parameters);

                Log.Error(ex, "Failed to GetDataReader");

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

                Log.Error(ex, "Failed to GetDataReader");

                throw;

            }

            return result;

        }

    }
*/
    
/*
    public class MsSqlAccess : DataBaseAccess<SqlConnection, SqlCommand>, IDataBaseAccess

    {

        public int? BatchSize { get; set; }

        public MsSqlAccess(string connectionString) : base(connectionString) { }

        public void BulkCopy<T>(IEnumerable<T> items, Metadata<T> metadata)

        {

            if (items.Count() == 0) return;

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_c as SqlConnection)
            {

                DestinationTableName = (metadata.Schema != null) ?

                    $"[{metadata.Schema}].[{metadata.TableName}]" :

                    $"[{metadata.TableName}]",

                BulkCopyTimeout = 1800,

                BatchSize = BatchSize ?? 50000,

                EnableStreaming = true
            })

            {

                metadata.Column2Property.ToList().

                    ForEach(i => bulkCopy.ColumnMappings.Add(i.Value.Name, i.Key));

                bulkCopy.WriteToServer(items.ToDataReader(metadata));

            }

        }

    }
*/


    //public static class DbContextExtensions

    //{

    //    public static DBMetadata DbMetadata<T>(this DbContext context)

    //    {

    //        IEntityType entityType = context.Model.GetEntityTypes().Where(x => x.ClrType.Equals(typeof(T))).FirstOrDefault();

    //        return new DBMetadata

    //        {

    //            ConnectionString = context.Database.GetDbConnection().ConnectionString,

    //            TableName = entityType.SqlServer().TableName,

    //            Schema = entityType.SqlServer().Schema,

    //            Column2Property = typeof(T).

    //                        GetProperties(

    //                            BindingFlags.GetProperty |

    //                            BindingFlags.Instance |

    //                            BindingFlags.Public).

    //                        Select(p =>

    //                            new

    //                            {

    //                                context.Model.GetEntityTypes().Where(x => x.ClrType.Equals(typeof(T))).FirstOrDefault().

    //                                    GetProperties().Where(x => x.Name.Equals(p.Name)).FirstOrDefault()?.

    //                                    SqlServer().ColumnName,

    //                                p

    //                            }).

    //                        Where(p => p.ColumnName != null).

    //                        ToDictionary(kvp => kvp.ColumnName, kvp => kvp.p)

    //        };

    //    }





    //    //public int BulkInsert<T>(IEnumerable<BufferEntry> entries)

    //    //{

    //    //    int count;

    //    //    if ((count = entries.Count()) == 0) return 0;

    //    //    var metadata = context.DbMetadata<BufferEntry>();

    //    //    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(context.SqlConnection)

    //    //    {

    //    //        DestinationTableName = $"[{metadata.Schema}].[{metadata.TableName}]",

    //    //        BulkCopyTimeout = 1800,

    //    //        BatchSize = 50000,

    //    //        EnableStreaming = true

    //    //    })

    //    //    {

    //    //        metadata.Column2Property.ToList().

    //    //            ForEach(i => bulkCopy.ColumnMappings.Add(i.Value.Name, i.Key));

    //    //        bulkCopy.WriteToServer(entries.ToDataReader(metadata));

    //    //    }

    //    //    return count;

    //    //}





    //}

    /*
    public static class DbDataReaderExtensions

    {

        public static ListFactory<T> ToList<T>

        (

            this DbDataReader reader,

            IDictionary<string, PropertyInfo> column2Property

        ) where T : new() => new ListFactory<T>(reader, column2Property);



        //public static void CreateModel(this DbDataReader reader)

        //{

        //    Encoding MyEncoding = Encoding.GetEncoding("utf-8");

        //    StringBuilder sb = new StringBuilder();

        //   // string fileNameColumn = "ModelDraftColumn.cs";

        //  //  string fileNameEF = "ModelDraftEF.cs";

        //    var schemaTable = reader.GetSchemaTable();



        //    Func<string, bool, string> t2p = (t,n) =>

        //     {

        //         switch (t)

        //         {

        //             case "System.Double": return "double" + (n ? "?" : "");

        //             case "System.Decimal": return "decimal" + (n ? "?" : "");

        //             case "System.String": return "string";

        //             case "System.DateTime": return "DateTime" + (n ? "?" : "");

        //             case "System.Int32": return "int" + (n ? "?" : "");

        //             case "System.Byte": return "byte" + (n ? "?" : "");

        //             default: return "unknown";

        //         }

        //     };



        //    DataColumn dcName = schemaTable.Columns[schemaTable.Columns.IndexOf("ColumnName")];

        //   // DataColumn dcOrdinal = schemaTable.Columns[schemaTable.Columns.IndexOf("ColumnOrdinal")];

        //    DataColumn dcType = schemaTable.Columns[schemaTable.Columns.IndexOf("DataType")];

        //    DataColumn dcNull = schemaTable.Columns[schemaTable.Columns.IndexOf("AllowDBNull")];

        //    //var o2p = new PropertyInfo[schemaTable.Rows.Count];



        //    Enumerable.

        //        Range(1, 2).ToList().ForEach(i =>

        //        {

        //            sb.AppendLine("using System;");

        //            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");

        //            sb.AppendLine("namespace NavFeeder.Con");

        //            sb.AppendLine("{");

        //            sb.AppendLine($"    public class ModelDraft_{i}");

        //            sb.AppendLine("    {");

        //            foreach (DataRow field in schemaTable.Rows)

        //            {

        //                var fieldNull = bool.Parse(field[dcNull].ToString());

        //                var fieldType = field[dcType].ToString();

        //                var fieldName = field[dcName].ToString();

        //                if (fieldName == "timestamp") continue;

        //                var propertyName = fieldName.ClearColumn();

        //                if (i == 1) sb.AppendLine($"        [Column(\"{fieldName}\")]");

        //                sb.AppendLine($"        public {t2p(fieldType, fieldNull)} {propertyName} {{ get; set; }}");

        //            }

        //            sb.AppendLine("    }");

        //            sb.AppendLine("}");                  

        //            File.WriteAllText($"ColumnModel_{i}.cs", sb.ToString(), MyEncoding);



        //            sb.Clear();

        //        });

        //    sb.AppendLine("/*");

        //    foreach (DataRow field in schemaTable.Rows)

        //    {              

        //        var fieldNull = bool.Parse(field[dcNull].ToString());

        //        var fieldType = field[dcType].ToString();

        //        var fieldName = field[dcName].ToString();

        //        if (fieldName == "timestamp") continue;

        //        var propertyName = fieldName.ClearColumn();

        //        sb.AppendLine($"        entity.Property(e => e.{propertyName}).HasColumnName(\"{fieldName}\");");

        //    }

        //    sb.AppendLine("*//*");

        //    File.WriteAllText("EFModel.cs", sb.ToString(), MyEncoding);



        //}

    }

*/
    /*
    public class ListFactory<T> : IEnumerable<T> where T : new()

    {

        private IList<T> list = new List<T>();

        private DbDataReader reader;

        public ListFactory(DbDataReader reader, IDictionary<string, PropertyInfo> column2Property)

        {

            this.reader = reader;

            // Массив свойств класса. Индекс массива - номер позиции

            var schemaTable = reader.GetSchemaTable();

            DataColumn dcName = schemaTable.Columns[schemaTable.Columns.IndexOf("ColumnName")];

            DataColumn dcOrdinal = schemaTable.Columns[schemaTable.Columns.IndexOf("ColumnOrdinal")];

            var o2p = new PropertyInfo[schemaTable.Rows.Count];

            foreach (DataRow field in schemaTable.Rows)

            {

                var fieldName = field[dcName].ToString();

                if (column2Property.ContainsKey(fieldName))

                    o2p[int.Parse(field[dcOrdinal].ToString())] = column2Property[fieldName];

            }



            // Начитываем по ридеру           

            while (reader.Read())

            {

                T obj = new T();

                Enumerable.

                    Range(0, o2p.Length).

                    Where(i => !reader.IsDBNull(i)).

                    ToList().

                    ForEach(i => o2p[i]?.SetValue(obj, reader[i]));

                list.Add(obj);

            }

        }



        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => reader.GetEnumerator();

    }

    public static class IEnumerableExtensions

    {

        public static DbDataReaderFactory<T> ToDataReader<T>(this IEnumerable<T> list, Metadata<T> metadata) =>

            new DbDataReaderFactory<T>(list, metadata);

    }



    public class DbDataReaderFactory<T> : DbDataReader, IDataReader

    {

        private IEnumerator<T> list = null;

        private List<PropertyInfo> properties;

        private Dictionary<string, int> nameLookup = new Dictionary<string, int>();



        public DbDataReaderFactory(IEnumerable<T> list, Metadata<T> metadata)

        {

            this.list = list.GetEnumerator();

            properties = metadata.Column2Property.Values.ToList();

            Enumerable.

                Range(0, properties.Count).

                ToList().

                ForEach(i => nameLookup[properties[i].Name] = i);

        }



        #region IDataReader Members



        public override void Close()

        {

            closed = true;

            list.Dispose();

        }



        public override int Depth => 0;



        public override DataTable GetSchemaTable()

        {

            var dt = new DataTable();

            properties.ForEach(p => dt.Columns.Add(new DataColumn(p.Name, p.PropertyType)));

            return dt;

            //throw new NotImplementedException();

        }

        private bool closed;

        public override bool IsClosed => closed;



        public override bool HasRows => true;



        public override bool NextResult()

        {

            throw new NotImplementedException();

        }



        public override bool Read()

        {

            if (IsClosed)

                throw new InvalidOperationException("DataReader is closed");



            return list.MoveNext();

        }



        public override int RecordsAffected => -1;

        //{

        //    get { throw new NotImplementedException(); }

        //}



        #endregion



        #region IDisposable Members



        public new void Dispose()

        {

            Close();

        }



        #endregion



        #region IDataRecord Members



        public override int FieldCount => properties.Count;



        public override bool GetBoolean(int i)

        {

            return (bool)GetValue(i);

        }



        public override byte GetByte(int i)

        {

            return (byte)GetValue(i);

        }



        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)

        {

            throw new NotImplementedException();

        }



        public override char GetChar(int i)

        {

            return (char)GetValue(i);

        }



        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)

        {

            throw new NotImplementedException();

        }



        public new IDataReader GetData(int i)

        {

            throw new NotImplementedException();

        }



        public override string GetDataTypeName(int i)

        {

            throw new NotImplementedException();

        }



        public override DateTime GetDateTime(int i)

        {

            return (DateTime)GetValue(i);

        }



        public override decimal GetDecimal(int i)

        {

            return (decimal)GetValue(i);

        }



        public override double GetDouble(int i)

        {

            return (double)GetValue(i);

        }



        public override Type GetFieldType(int i)

        {

            return properties[i].PropertyType;

        }



        public override float GetFloat(int i)

        {

            return (float)GetValue(i);

        }



        public override Guid GetGuid(int i)

        {

            return (Guid)GetValue(i);

        }



        public override short GetInt16(int i)

        {

            return (short)GetValue(i);

        }



        public override int GetInt32(int i)

        {

            return (int)GetValue(i);

        }



        public override long GetInt64(int i)

        {

            return (long)GetValue(i);

        }



        public override string GetName(int i)

        {

            return properties[i].Name;

        }



        public override int GetOrdinal(string name)

        {

            if (nameLookup.ContainsKey(name))

            {

                return nameLookup[name];

            }

            else

            {

                return -1;

            }

        }



        public override string GetString(int i)

        {

            return (string)GetValue(i);

        }



        public override object GetValue(int i)

        {

            return properties[i].GetValue(list.Current, null);

        }



        public override int GetValues(object[] values)

        {

            int getValues = Math.Max(FieldCount, values.Length);



            for (int i = 0; i < getValues; i++)

            {

                values[i] = GetValue(i);

            }



            return getValues;

        }



        public override bool IsDBNull(int i)

        {

            return GetValue(i) == null;

        }



        public override IEnumerator GetEnumerator()

        {

            return this.list;

        }



        public override object this[string name]

        {

            get

            {

                return GetValue(GetOrdinal(name));

            }

        }



        public override object this[int i]

        {

            get

            {

                return GetValue(i);

            }

        }



        #endregion

    }

    public class DBARepo<T> : Repo<T>, IRepo<T> where T : new()

    {

        public DBARepo(IDataBaseAccess dataBaseAccess) : base(dataBaseAccess) =>

           Metadata =

                new Metadata<T>();

    }

    public class AttributeReader

    {

        public static U Class<U>(Type type) where U : class =>

            type.

                GetCustomAttributes(false).Where(x => x is U).FirstOrDefault() as U;



        public static IDictionary<PropertyInfo, U> Property2Attribute<U>(Type type) where U : class =>

            new List<PropertyInfo>(

                type.

                    GetProperties(

                        BindingFlags.GetProperty |

                        BindingFlags.Instance |

                        BindingFlags.Public)).

                Select(p => new

                {

                    Property = p,

                    Attribute = p.GetCustomAttributes(false).OfType<U>().FirstOrDefault() as U

                }).

                Where(a => a.Attribute != null).

                ToDictionary(k => k.Property, k => k.Attribute);

    }

    public class Debugger

    {

        private static DateTime last = DateTime.Now;

        public static Dictionary<string, TimeSpan> Durations = new Dictionary<string, TimeSpan>();



        //public static void Tick(string message) =>

        //    Tick(message, "");



        public static TimeSpan Tick(string task)

        {

            var duration = DateTime.Now - last;

            if (Durations.ContainsKey(task))

                Durations[task] += duration;

            else

                Durations[task] = duration;

            //Console.WriteLine($"{duration} - {message}");

            last = DateTime.Now;

            return duration;

        }

    }

    public class Json

    {

        public static void SaveSection<T>(T obj)

        {

            SettingsAttribute info = AttributeReader.Class<SettingsAttribute>(typeof(T));

            using (StreamWriter writer = new StreamWriter(info.Json))

            {



                writer.Write(JsonConvert.SerializeObject(

                    new Dictionary<string, T>

                    {

                        [info.Section] = obj

                    }, Formatting.Indented));

            }

        }



        public static T ReadSection<T>()

        {

            T result = default(T);

            SettingsAttribute info = AttributeReader.Class<SettingsAttribute>(typeof(T));

            using (StreamReader reader = new StreamReader(info.Json))

            {

                Dictionary<string, T> i =

                    JsonConvert.DeserializeObject<Dictionary<string, T>>(reader.ReadToEnd());

                if (i.ContainsKey(info.Section))

                    result = i[info.Section];

            }

            return result;

        }

    }

    /*
    public class Mapper

    {

        public static void FromTo<F, T>()

        {

            var f = new List<PropertyInfo>(

                typeof(F).

                    GetProperties(

                        BindingFlags.GetProperty |

                        BindingFlags.Instance |

                        BindingFlags.Public)).

                OrderBy(p => p.Name).

                Select(p => p.Name).

                ToList();

            var t = new List<PropertyInfo>(

                typeof(T).

                    GetProperties(

                        BindingFlags.GetProperty |

                        BindingFlags.Instance |

                        BindingFlags.Public)).

                OrderBy(p => p.Name).

                Select(p => p.Name).

                ToList();





            Encoding MyEncoding = Encoding.GetEncoding("utf-8");

            StringBuilder sb = new StringBuilder();

            var prop = new List<string>();

            sb.AppendLine("/*");



            var uName = typeof(T).Name;

            sb.AppendLine($"        public {uName} To{uName} => new {uName}");

            sb.AppendLine($"        {{");



            var enter = false;

            t.

            Select(to => new {
                from = f.Find(from => from.Equals(to)),

                to
            }).

            ToList().

            ForEach(i => {

                if (enter)

                    sb.AppendLine(",");

                else

                    enter = true;

                sb.Append($"            {i.to}  =   {i.from ?? "null"}");

            });



            sb.AppendLine();

            sb.AppendLine($"        }};");

            sb.AppendLine("*//*");

            File.WriteAllText("Mapper.cs", sb.ToString(), MyEncoding);

        }

    }
*/
    /*
    public class MyWebClient : WebClient

    {

        private int timeOut;



        public MyWebClient(int timeOut) => this.timeOut = timeOut;



        protected override WebRequest GetWebRequest(Uri uri)

        {

            WebRequest w = base.GetWebRequest(uri);

            w.Timeout = timeOut;

            return w;

        }

    }
    */
}
