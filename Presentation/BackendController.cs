using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public class BackendController
    {
        private IService Service { get; set; }

        /// <summary>
        /// simple constructor
        /// </summary>
        /// <param name="service">the interface of the service</param>
        public BackendController(IService service)
        {
            this.Service = service;
        }

        /// <summary>
        /// simple constructor
        /// </summary>
        public BackendController()
        {
            this.Service = new Service();
            Service.LoadData();
        }

        /// <summary>
        /// a function that calls the service to Register new user
        /// </summary>
        /// <param name="email">the user email</param>
        /// <param name="password">the user password</param>
        /// <param name="nickname">the user nickname</param>
        /// <param name="boardemail">the user board email</param>
        public void Register(string email,string password,string nickname,string boardemail)
        {
            Response res;
            if(string.IsNullOrEmpty(boardemail))
            {
                res = Service.Register(email, password, nickname);           
            }
            else
            {
                res = Service.Register(email, password, nickname, boardemail);
            }
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }     
        }

        /// <summary>
        /// a function that calls the service to remove column
        /// </summary>
        /// <param name="email">board email</param>
        /// <param name="id">the column ordinal</param>
        internal void RemoveColumn(string email, int id)
        {
            Response res = Service.RemoveColumn(email,id);
            if(res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// a function that calls the service to add a new column
        /// </summary>
        /// <param name="email">column email</param>
        /// <param name="id">column ordinal</param>
        /// <param name="name">column name</param>
        /// <returns>the service column</returns>
        public Column AddColumn(string email, int id,string name)
        {
            Response<Column> res = Service.AddColumn(email, id, name);
            if(res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.Value;  
        }

        /// <summary>
        /// a function that update the column name
        /// </summary>
        /// <param name="email">board email</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <param name="Newname">the name we want to update</param>
        public void UpdateColumnName(string email, int columnOrdinal, string Newname)
        {
            Response res = Service.ChangeColumnName(email,columnOrdinal,Newname);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// a function that calls the service to remove a task
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="columnOrdinal">the column ordinal of the task</param>
        /// <param name="taskId">task id</param>
        public bool RemoveTask(string email, int columnOrdinal, int taskId)
        {
            Response res = Service.DeleteTask(email, columnOrdinal, taskId);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return !res.ErrorOccured;
        }

        /// <summary>
        /// a function that calls the service to update a column limit
        /// </summary>
        /// <param name="email">the user email</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <param name="lim">the new limit</param>
        public void UpdateColumnLim(string email, int columnOrdinal, int lim)
        {
            Response res = Service.LimitColumnTasks(email, columnOrdinal, lim);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// a function that calls the service to update the task title
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="columnOrdinal">the column ordinal the task belong to</param>
        /// <param name="taskID">the task id</param>
        /// <param name="title">the new title</param>
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskID, string title)
        {
            Response res = Service.UpdateTaskTitle(email, columnOrdinal, taskID, title);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        internal bool Logout(string email)
        {
            Response res = Service.Logout(email);
            if(res.ErrorOccured)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// a function that calls the service to update the task Description
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="columnOrdinal">the column ordinal the task belong to</param>
        /// <param name="taskID">the task id</param>
        /// <param name="des">the new description</param>
        public void UpdateTaskDes(string email, int columnOrdinal, int taskID, string des)
        {
            Response res = Service.UpdateTaskDescription(email, columnOrdinal, taskID, des);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// a function that calls the service to update the task due date
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="columnOrdinal">the column ordinal the task belong to</param>
        /// <param name="taskID">the task id</param>
        /// <param name="dueDate">the new due date</param>
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskID, DateTime dueDate)
        {
            Response res = Service.UpdateTaskDueDate(email, columnOrdinal, taskID, dueDate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// a function that calls the service to update the task Assign email
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="columnOrdinal">the column ordinal the task belong to</param>
        /// <param name="taskID">the task id</param>
        /// <param name="AssignEmail">the new Assign email</param>
        public void UpdateTaskAssign(string email, int columnOrdinal, int taskID, string AssignEmail)
        {
            Response res = Service.AssignTask(email,columnOrdinal,taskID,AssignEmail);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// a function that calls the service to advance task
        /// </summary>
        /// <param name="email">the board email</param>
        /// <param name="columnOrdinal">the column ordianl which the task belong to</param>
        /// <param name="taskID">the task id</param>
        public void AdvanceTask(string email, int columnOrdinal, int taskID)
        {
            Response res = Service.AdvanceTask(email, columnOrdinal, taskID);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// a function that call the service to move the column right
        /// </summary>
        /// <param name="email">the board email</param>
        /// <param name="columnOrdinal">the column ordinal of the Column</param>
        public void MoveColRight(string email, int columnOrdinal)
        {
            Response res = Service.MoveColumnRight(email, columnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// a function that call the service to move the column left
        /// </summary>
        /// <param name="email">the board email</param>
        /// <param name="columnOrdinal">the column ordinal of the Column</param>
        public void MoveColLeft(string email, int columnOrdinal)
        {
            Response res = Service.MoveColumnLeft(email, columnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// a function that calls the service to log in specific user
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password">user password</param>
        /// <returns>a UserModel which presents the user</returns>
        public UserModel Login(string email,string password)
        {
            Response<User> user = Service.Login(email, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, email);
        }

        /// <summary>
        /// a function that get all the columnNames of a userBoard
        /// </summary>
        /// <param name="user">the user Model</param>
        /// <returns>Read only collection of the column names</returns>
        public IReadOnlyCollection<string> GetColumnsNames(UserModel user)
        {
            Response<Board> board = Service.GetBoard(user.Email);
            if(board.ErrorOccured)
            {
                throw new Exception(board.ErrorMessage);
            }
            return board.Value.ColumnsNames;
        }

        /// <summary>
        /// a function which calls the service to create a new task
        /// </summary>
        /// <param name="userEmail">the task email creator</param>
        /// <param name="title">task title</param>
        /// <param name="description">task description</param>
        /// <param name="dueDate">task due date</param>
        /// <returns>a New service task</returns>
        internal IntroSE.Kanban.Backend.ServiceLayer.Task AddTask(string userEmail, string title, string description, DateTime dueDate)
        {
            Response<IntroSE.Kanban.Backend.ServiceLayer.Task> task = Service.AddTask(userEmail, title, description, dueDate);
            if(task.ErrorOccured)
            {
                throw new Exception(task.ErrorMessage);
            }
            return task.Value;
        }

        /// <summary>
        /// a function that get a column by the name
        /// </summary>
        /// <param name="email">the user email</param>
        /// <param name="name">column name</param>
        /// <returns>the specific column</returns>
        public Column GetColumn(string email,string name)
        {
            Response<Column> column = Service.GetColumn(email, name);
            if(column.ErrorOccured)
            {
                throw new Exception(column.ErrorMessage);
            }
            return column.Value;
        }
    }
}
