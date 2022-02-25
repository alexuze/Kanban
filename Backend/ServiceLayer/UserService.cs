using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class UserService
    {
        private UserController _UserController;
        public UserService()
        {
            _UserController = new UserController();
        }
        /// <summary>        
        /// Loads the data. Intended be invoked only when the program starts
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error.</returns>
        public Response LoadData(BoardController bc)
        {
            Response toReturn;
            try
            {
                _UserController.LoadData(bc);
                toReturn = new Response();
            }
            catch(Exception ee)
            {
                toReturn = new Response(ee.Message);
            }
            return toReturn;            
        }
        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname,BoardController bc,string emailhost)
        {
            Response toReturn;
            try
            {
                
                _UserController.Register(email, password, nickname,bc,emailhost);
                toReturn = new Response();
            }
            catch (Exception ee)
            {
                toReturn = new Response(ee.Message);
            }
            return toReturn;
        }
        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string email, string password)
        {
            Response<User> toReturn;
            try
            {
                BusinessLayer.UserPackage.User userToLogin = _UserController.Login(email, password);
                toReturn = new Response<User>(new User(email, userToLogin.Nickname));
            }
            catch (Exception ee)
            {
                toReturn = new Response<User>(ee.Message);

            }
            return toReturn;
        }

        /// <summary>
        /// log out a user from the system
        /// </summary>
        /// <param name="email">the email we want to logout with</param>
        /// <returns>a response object</returns>
        public Response Logout(string email)
        {
            Response toReturn;
            try
            {
                _UserController.Logout(email);
                toReturn = new Response();
            }
            catch (Exception ee)
            {
                toReturn = new Response(ee.Message);
            }
            return toReturn;
        }
        ///<summary>Remove all persistent data.</summary>
        public Response DeleteAll()
        {
            Response toReturn;
            try
            {
                _UserController.DeleteALL();
                toReturn = new Response();
            }
            catch (Exception ee)
            {
                toReturn = new Response(ee.Message);
            }
            return toReturn;
        }
    }
}
