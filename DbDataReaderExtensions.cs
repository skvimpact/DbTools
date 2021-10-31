using System;
using System.Data;
using System.Data.Common;


namespace DbTools
{
    public static class DbDataReaderExtensions
    {
        public static Metadata Metadata(this DbDataReader reader) =>
            new MetadataFactory(reader);
    }

    public class MetadataFactory : Metadata
    {
        public MetadataFactory(DbDataReader reader)
        {
            var schemaTable = reader.GetSchemaTable();
            TableName = schemaTable.Rows[0].Field<string>("BaseTableName");
            Schema = schemaTable.Rows[0].Field<string>("BaseSchemaName");
           
            foreach (DataRow row in schemaTable.Rows)
            {
                TableName = row.Field<string>("BaseTableName");
                Schema = row.Field<string>("BaseSchemaName");
                Columns.Add(new ColumnSchema {
                    AllowDBNull = row.Field<bool>("AllowDBNull"),
                    DataType = row.Field<Type>("DataType").Name,
                    Size = row.Field<int>("ColumnSize"),
                    Name = row.Field<string>("ColumnName")
                });
            }
        }
    }
}
