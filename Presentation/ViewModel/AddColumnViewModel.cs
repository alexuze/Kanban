using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class AddColumnViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        private string _userEmail;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="user">a User model</param>
        public AddColumnViewModel(UserModel user)
        {
            Controller = user.Controller;
            this._userEmail = user.Email;
        }

        /// <summary>
        /// getter and setter for the name
        /// </summary>
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        }
        /// <summary>
        /// getter and setter for id
        /// </summary>
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }

        /// <summary>
        /// getter and setter for messsege
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
        /// a function that add column to the Board
        /// </summary>
        /// <returns>the column model that we wanted to add</returns>
        public ColumnModel AddColumn()
        {
            ColumnModel toReturn;
            Message = "";
            try
            {
                toReturn = new ColumnModel(Controller,Controller.AddColumn(_userEmail,Id,Name),_userEmail);
                Message = "Column added succesfully";
            }
            catch(Exception ee)
            {
                Message = ee.Message;
                toReturn = null;
            }
            return toReturn;
        }
    }
}
