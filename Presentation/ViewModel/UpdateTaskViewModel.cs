using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class UpdateTaskViewModel :NotifiableObject
    {
        private Model.TaskModel _task;
        /// <summary>
        /// constuctor
        /// </summary>
        /// <param name="task">the task we want to update</param>
        public UpdateTaskViewModel(Model.TaskModel task, int colOrdinal,string email)
        {
            this.taskID = task.TaskID;
            this._task = task;
            this.Title = task.Title;
            this.Description = task.Description;
            this.DueDate = task.DueDate;
            this.Assign = task.EmailAssignee;
            this.email = email;
            this.colOrdinal = colOrdinal;
        }
        private int colOrdinal;
        private int taskID;
        private string email;
        /// <summary>
        /// getter and setter for title
        /// </summary>
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
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
                _description = value;
                RaisePropertyChanged("Description");
            }
        }
        /// <summary>
        /// getter and setter for due date
        /// </summary>
        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        /// <summary>
        /// getter and setter for email assign 
        /// </summary>
        private string _emailAssign;
        public string Assign
        {
            get => _emailAssign;
            set
            {
                if(_emailAssign!=value)
                    BorderBrush = "Black";
                _emailAssign = value;
                RaisePropertyChanged("Assign");
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
                _message = value;
                RaisePropertyChanged("Message");
            }
        }

        private string _borderbursh;
        public string BorderBrush
        {
            get => _borderbursh;
            set
            {
                _borderbursh = value;
                RaisePropertyChanged("BorderBrush");
            }
        }
        /// <summary>
        /// getter and setter for column ordinal
        /// </summary>
        public int ColOrdinal {
            get => colOrdinal;
            set
            {
                colOrdinal = value;
                RaisePropertyChanged("colOrdinal");
            }
        }

        /// <summary>
        /// getter and setter for email
        /// </summary>
        public string Email { get => email; set => email = value; }

        /// <summary>
        /// a function that calls the controller to update the title
        /// </summary>
        public void UpdateTitle()
        {
            Message = "";
            try
            {
                _task.Controller.UpdateTaskTitle(email, colOrdinal, taskID, Title);
                Message = "Title Changed successfully";
                _task.Title = Title;
            }
            catch (Exception ee)
            {
                Message = ee.Message;
            }
        }

        /// <summary>
        /// a function that calls the controller to update the description
        /// </summary>
        public void UpdateDescription()
        {
            Message = "";
            try
            {
                _task.Controller.UpdateTaskDes(email, colOrdinal, taskID, Description);
                Message = "Description Changed successfully";
                _task.Description = Description;
            }
            catch (Exception ee)
            {
                Message = ee.Message;
            }
        }

        /// <summary>
        /// a function that calls the controller to update the due date
        /// </summary>
        public void UpdateDueDate()
        {
            Message = "";
            try
            {
                _task.Controller.UpdateTaskDueDate(email, colOrdinal, taskID, DueDate);
                Message = "DueDate Changed successfully";
                _task.DueDate = DueDate;
            }
            catch (Exception ee)
            {
                Message = ee.Message;
            }
        }

        /// <summary>
        /// a function that calls the controller to update the assign email
        /// </summary>
        public void UpdateAssign()
        {
            Message = "";
            try
            {
                _task.Controller.UpdateTaskAssign(email, colOrdinal, taskID, Assign);
                Message = "Assignee Email Changed successfully";
                _task.EmailAssignee = Assign;
                _task.BorderBrush = "Black";
            }
            catch (Exception ee)
            {
                Message = ee.Message;
            }
        }

        /// <summary>
        /// a function that calls the controller to advance the task
        /// </summary>
        /// <returns>the message we want to pass to board view</returns>
        public string AdvanceTask(BoardViewModel board)
        {
            Message = "";
            try
            {
                _task.Controller.AdvanceTask(email, colOrdinal, taskID);
                Message = "Task Advanced successfully";
                board.Board.Columns[colOrdinal + 1].Tasks.Add(_task);
                board.Board.Columns[colOrdinal].Tasks.Remove(_task);
                return Message;
            }
            catch (Exception ee)
            {
                Message = ee.Message;
                return "";
            }
        }
    }
}
