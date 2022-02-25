using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class DalController
    {
        protected readonly string _connectionString;
        private readonly string _tableName;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tableName">the name of the table in the data base</param>
        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
        }
        
        /// <summary>
        /// a function that update an attribute value in the data base
        /// </summary>
        /// <param name="id"> the id of the object in the data base</param>
        /// <param name="attributeName">the name of the column we want to update</param>
        /// <param name="attributeValue">the value of the attribute</param>
        /// <returns>true if update succeed</returns>
        public bool Update(long id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where {DalObject.IDColumnName}={id}"
                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    //log
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        /// <summary>
        /// a function that update an attribute value in the data base
        /// </summary>
        /// <param name="id"> the id of the object in the data base</param>
        /// <param name="attributeName">the name of the column we want to update</param>
        /// <param name="attributeValue">the value of the attribute</param>
        /// <returns>true if update succeed</returns>
        public bool Update(long id, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where {DalObject.IDColumnName}={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res=command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }

        /// <summary>
        /// a function that reads all the DAL Objects of specific type from data base
        /// </summary>
        /// <returns>a list of the dal objects</returns>
        protected List<DalObject> Select()
        {
            List<DalObject> results = new List<DalObject>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch(Exception ee) { }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        /// <summary>
        /// converts a row in the data  base into a Dal Object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Dal Object of specific row</returns>
        protected abstract DalObject ConvertReaderToObject(SQLiteDataReader reader);

        /// <summary>
        /// delete a DalObject from the data base
        /// </summary>
        /// <param name="dalObject">the object we want to delete</param>
        /// <returns>true if delete succeed</returns>
        public bool Delete(DalObject dalObject)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName} WHERE {DalObject.IDColumnName}='{dalObject.DalID}'"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch(Exception ee) { }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        /// <summary>
        /// a function that clear all data from data base
        /// </summary>
        /// <returns>true if delete succeed</returns>
        public bool DeleteAll()
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = $"DELETE FROM sqlite_sequence";
                    command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
    }
}