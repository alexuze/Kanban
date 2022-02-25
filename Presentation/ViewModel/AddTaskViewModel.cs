using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class AddTaskViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        private string _userEmail;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="user">the user model that want to create a task</param>
        public AddTaskViewModel(UserModel user)
        {
            Controller = user.Controller;
            this._userEmail = user.Email;
        }

        /// <summary>
        /// getter and setter for title
        /// </summary>
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }
        /// <summary>
        /// getter and setter for description
        /// </summary>
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                this._description = value;
                RaisePropertyChanged("Description");
            }
        }
        /// <summary>
        /// getter and setter for due date
        /// </summary>
        private DateTime _dueDate;
        public DateTime DueDate
        {
            get
            {
                if (_dueDate == DateTime.MinValue)
                    return DateTime.Now;
                return _dueDate;
            } 
            set
            {
                this._dueDate = value;
                RaisePropertyChanged("DueDate");
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
        /// a function that Create the new task
        /// </summary>
        /// <returns>the task we created</returns>
        public TaskModel AddTask()
        {
            TaskModel toReturn;
            Message = "";
            try
            {
                toReturn = new TaskModel(_userEmail,Controller, Controller.AddTask(_userEmail, Title, Description,DueDate));
                Message = "Task added succesfully";
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
