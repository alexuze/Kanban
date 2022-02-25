using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class DalUserController : DalController
    {
        private const string UserTableName = "Users";

        /// <summary>
        /// simple constructor
        /// </summary>
        public DalUserController() : base(UserTableName)
        {

        }

        /// <summary>
        /// a function that reads all Users from data base
        /// </summary>
        /// <returns>List of all DAL Users</returns>
        public List<User> SelectAllUsers()
        {
            List<User> result = Select().Cast<User>().ToList();
            return result;
        }
        
        /// <summary>
        /// a function that convert a row from data base into DAL user
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            
            User result = new User(int.Parse(reader.GetValue(0).ToString()), reader.GetString(1), reader.GetString(2),reader.GetString(3),reader.GetString(4),false);
            return result;
        }

        /// <summary>
        /// update changes in the data base
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="attributeName">the property we want to change</param>
        /// <param name="attributeValue">the value we want to change to</param>
        /// <returns></returns>
        public bool Update(string email, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {UserTableName} set [{attributeName}]=@{attributeName} where email='{email}'"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
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

        /// <summary>
        /// inserts a new user to data base
        /// </summary>
        /// <param name="user">Dal user we want to pressit</param>
        /// <returns>the row id</returns>
        public int Insert(User user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserTableName} ({User.EmailColumnName},{User.PasswordColumnName},{User.NicknameColumnName},{User.EmailHostColumnName}) " +
                        $"VALUES (@email,@passVal,@nickVal,@emailHost);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"email",user.Email );
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passVal", user.Password);
                    SQLiteParameter nicknameParam = new SQLiteParameter(@"nickVal", user.Nickname);
                    SQLiteParameter emailHostParam = new SQLiteParameter(@"emailHost", user.EmailHost);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Parameters.Add(nicknameParam);
                    command.Parameters.Add(emailHostParam);

                    command.Prepare();
                    res = command.ExecuteNonQuery();

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
