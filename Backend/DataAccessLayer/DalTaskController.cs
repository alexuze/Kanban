using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DalTaskController : DalController
    {
        private const string TaskTableName = "Tasks";

        /// <summary>
        /// simple constructor
        /// </summary>
        public DalTaskController() : base(TaskTableName)
        {

        }

        /// <summary>
        /// a function the reads all task from data base
        /// </summary>
        /// <returns>a list of DAL tasks</returns>
        public List<Task> SelectAllTasks()
        {
            List<Task> result = Select().Cast<Task>().ToList();
            return result;
        }

        /// <summary>
        /// a function that convert row from data base into a task
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>DAL Task</returns>
        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            Task result = new Task(int.Parse(reader.GetValue(0).ToString()), int.Parse(reader.GetValue(1).ToString()), reader.GetString(2),reader.IsDBNull(3)? null : reader.GetString(3), reader.GetString(4),DateTime.Parse(reader.GetValue(5).ToString()), DateTime.Parse(reader.GetValue(6).ToString()),reader.GetInt32(7),false);
            return result;
        }

        /// <summary>
        /// create a new row of the task in the data base
        /// </summary>
        /// <param name="task">the DAL task we want to presist</param>
        /// <returns>the row id</returns>
        public int Insert(Task task)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({Task.IDName} ,{Task.TitleColumnName},{Task.DescriptionColumnName},{Task.EmailColumnName},{Task.CreationTimeColumnName},{Task.DueDateColumnName},{Task.ColDalIDColumnName}) " +
                        $"VALUES (@idVal,@titleVal,@descriptionVal,@email,@creationVal,@dueDateval,@ColIDVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.Id);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", task.Description);
                    SQLiteParameter creationParam = new SQLiteParameter(@"creationVal", task.Creationtime);
                    SQLiteParameter duedateParam = new SQLiteParameter(@"dueDateval", task.DueDate);
                    SQLiteParameter emailParam = new SQLiteParameter(@"email", task.Email);
                    SQLiteParameter colIDParam = new SQLiteParameter(@"ColIDVal", task.ColDalID);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(creationParam);
                    command.Parameters.Add(duedateParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(colIDParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    
                    // return the row id of the task
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
