using Presentation.Model;
using Presentation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class MainViewModel : NotifiableObject
    {
        /// <summary>
        /// getter and setter for the controller
        /// </summary>
        public BackendController Controller { get; private set; }

        /// <summary>
        /// constuctor
        /// </summary>
        public MainViewModel()
        {
            Controller = new BackendController();
        }

        /// <summary>
        /// getter and setter for email
        /// </summary>
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                this._email = value;
                RaisePropertyChanged("Email");
            }
        }

        /// <summary>
        /// getter and setter for password
        /// </summary>
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// getter and setter for message
        /// </summary>
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        /// <summary>
        /// a function that pass the login details to the controller to login the user
        /// </summary>
        /// <returns>the user which has logged in or null if login failed</returns>
        public BoardView Login()
        {
            Message = "";
            try
            {
                UserModel u=Controller.Login(Email, Password);
                BoardView boardView = new BoardView(u);
                return boardView;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
    }
}
