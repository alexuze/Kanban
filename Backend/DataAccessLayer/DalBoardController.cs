using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DalBoardController : DalController
    {
        private const string BoardTableName = "Boards";

        //simple constructor
        public DalBoardController() : base(BoardTableName)
        {

        }

        /// <summary>
        /// a function that reads all the boards from data base
        /// </summary>
        /// <returns>a list of all Boards from DataBase</returns>
        public List<Board> SelectAllBoards()
        {
            List<Board> result = Select().Cast<Board>().ToList();
            return result;  
        }

        /// <summary>
        /// a function that converts row from the data base to board
        /// </summary>
        /// <param name="reader">a reader that indicate a row from data base</param>
        /// <returns></returns>
        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            Board result = new Board(int.Parse(reader.GetValue(0).ToString()),reader.GetString(1),int.Parse(reader.GetValue(2).ToString()),false);
            return result;
        }

        /// <summary>
        /// a function that create new row of the dal board in the data base
        /// </summary>
        /// <param name="board">the dal board we want to save</param>
        /// <returns></returns>
        public int Insert(Board board)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName} ({DalObject.EmailColumnName},{Board.TaskCountColumnName}) " +
                        $"VALUES (@emailVal,@TaskCountVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"EmailVal", board.UserEmail);
                    SQLiteParameter TaskCountParam = new SQLiteParameter(@"TaskCountVal", board.TaskCount);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(TaskCountParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "select last_insert_rowid()";
                    Int64 LastRowID64 = (Int64)command.ExecuteScalar();
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
