using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class RegisterViewModel : NotifiableObject
    {
        /// <summary>
        /// getter and setter for the controller
        /// </summary>
       public BackendController Controller { get; private set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="controller">the controller of the system</param>
        public RegisterViewModel(BackendController controller)
        {
            Controller = controller;
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
        public string _password;
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
        /// getter and setter for nick name
        /// </summary>
        public string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                this._nickname = value;
                RaisePropertyChanged("Nickname");
            }
        }

        /// <summary>
        /// getter and setter for board email
        /// </summary>
        public string _boardEmail;
        public string BoardEmail
        {
            get => _boardEmail;
            set
            {
                this._boardEmail = value;
                RaisePropertyChanged("BoardEmail");
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
        /// a function that calls the controller to register with the details
        /// </summary>
        /// <returns> the message occurs</returns>
        public string Register()
        {
            Message = "";
            try
            {
                Controller.Register(Email, Password, Nickname, BoardEmail);
            }
            catch(Exception ee)
            {
                Message = ee.Message;
            }
            return Message;
        }
    }
}
