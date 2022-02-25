using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    internal class UserController
    {
        private Dictionary<string, User> AllUsers;
        private string currentUser;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UserController()
        {
            this.currentUser = "";
            this.AllUsers = new Dictionary<string, User>();
        }
        /// <summary>
        /// login the user to the system
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password">user password</param>
        /// <returns>the user that logged in</returns>
        public User Login(string email, string password)
        {
            if (this.currentUser == "")
            {
                if (!AllUsers.ContainsKey(email))
                {
                    log.Error("Login failed: somone trying to login to non existing user");
                    throw new Exception("email is not exist");
                }
                if (AllUsers[email].Password != password)
                {
                    log.Warn("Login failed: somone is trying to enter a user with wrong password");
                    throw new Exception("wrong password");
                }
                AllUsers[email].IsLogged = true;
                AllUsers[email].Board.IsLogged = true;
                AllUsers[email].Board.LoggedBoard = email;
                currentUser = email;
                log.Info("the user " + email + " logged in successfully");
                return AllUsers[email];
            }
            else
            {
                if (currentUser == email)
                {
                    log.Error("somone that is already online is trying to login");
                    throw new Exception("the user is already online");
                }
                else
                {
                    log.Warn("somone is trying to login while there is online user");
                    throw new Exception("there is already online user");
                }
            }
        }
        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public bool Logout(string email)
        {
            if (currentUser != email)
            {
                log.Error("someone is trying to log out while he is not logged in");
                throw new Exception("user already not online");
            }
            if (!AllUsers.ContainsKey(email))
            {
                log.Error("somone is trying to log out with not existing email");
                throw new Exception("User not found");
            }
            AllUsers[email].IsLogged = false;
            AllUsers[email].Board.IsLogged = false;
            AllUsers[email].Board.LoggedBoard="";
            currentUser = "";
            log.Info("the user " + email + " logged out");
            return true;
        }
        /// <summary>
        /// register a new user in the system
        /// </summary>
        /// <param name="email">register user email</param>
        /// <param name="password">register user password</param>
        /// <param name="nickname">register user nickname</param>
        /// <param name="bc">the system boardcontroller</param>
        /// <returns>true if register succeed</returns>
        public bool Register(string email, string password, string nickname, BoardController bc,string emailhost)
        {
            if (AllUsers.ContainsKey(email))
            {
                log.Error("Register failed :someone tried to register with the email :" + email + "which is already exist in the system");
                throw new Exception("User already exists in the system");
            }
            User newUser = new User();
            if (string.IsNullOrEmpty(emailhost))
                newUser = newUser.Register(email, password, nickname, null);
            else
                newUser = newUser.Register(email, password, nickname, bc.Boards[emailhost]);
            AllUsers.Add(email, newUser);
            bc.Boards.Add(email, newUser.Board);
            log.Info("new user added to the system with the email : " + email);
            return true;
        }
        /// <summary>        
        /// Loads the data. Intended be invoked only when the program starts
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error.</returns>
        public void LoadData(BoardController bc)
        {
            DalUserController dalUserCont = new DalUserController();
            List<DataAccessLayer.User> DalUsers = new List<DataAccessLayer.User>();
            DalUsers = dalUserCont.SelectAllUsers();
            foreach (DataAccessLayer.User dalUser in DalUsers)
            {
                bc.addBoardToDictionary(dalUser.Email, bc.Boards[dalUser.EmailHost]);
                User newUser = new User(dalUser, bc.Boards[dalUser.EmailHost]);
                AllUsers.Add(newUser.Email, newUser);
            }
            log.Info("system has been successfully loaded users");
        }
        ///<summary>Remove all persistent data.</summary>
        public void DeleteALL()
        {
            DalController dalUserCont = new DalUserController();
            DalController dalBoardCont = new DalBoardController();
            DalController dalTaskCont = new DalTaskController();
            DalController dalColumnCont = new DalColumnController();
            dalTaskCont.DeleteAll();
            dalColumnCont.DeleteAll();
            dalBoardCont.DeleteAll();
            dalUserCont.DeleteAll();
            this.AllUsers = new Dictionary<string, User>();
        }
    }
}