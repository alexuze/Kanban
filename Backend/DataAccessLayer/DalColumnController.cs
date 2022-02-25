using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DalColumnController : DalController
    {
        private const string ColumnTableName = "Columns";

        /// <summary>
        /// simple constructor
        /// </summary>
        public DalColumnController() : base(ColumnTableName)
        {

        }

        /// <summary>
        /// a function that convert all rows from data base to DAL columns
        /// </summary>
        /// <returns>list of all dal columns</returns>
        public List<Column> SelectAllColumns()
        {
            List<Column> result = Select().Cast<Column>().ToList();
            return result;
        }

        /// <summary>
        /// a function that conver a row from the data base into DAL column
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            Column result = new Column(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4), false);
            return result;
        }

        /// <summary>
        /// a function that create new row of the DAL column in the data base
        /// </summary>
        /// <param name="column">the DAL column we want to presist</param>
        /// <returns>the DAL id of the Column</returns>
        public int Insert(Column column)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} ({Column.NameColumnName},{Column.LimitColumnName},{Column.ColOrdinalColumnName},{DalObject.EmailColumnName}) " +
                        $"VALUES (@NameVal,@LimitVal,@ColVal,@EmailVal);";

                    SQLiteParameter NameParam = new SQLiteParameter(@"NameVal", column.Name);
                    SQLiteParameter LimitParam = new SQLiteParameter(@"LimitVal", column.Limit);
                    SQLiteParameter ColParam = new SQLiteParameter(@"ColVal", column.ColOridinal);
                    SQLiteParameter EmailParam = new SQLiteParameter(@"EmailVal", column.Email);
                    command.Parameters.Add(NameParam);
                    command.Parameters.Add(LimitParam);
                    command.Parameters.Add(ColParam);
                    command.Parameters.Add(EmailParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "select last_insert_rowid()";
                    Int64 LastRowID64 = (Int64)command.ExecuteScalar();

                    // Then grab the bottom 32-bits as the unique ID of the row.
                    //
                    res = (int)LastRowID64;
                }
                catch (Exception ex)
                {
                    //log error
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res;
            }
        }
    }
}