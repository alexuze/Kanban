using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    internal class BoardController
    {
        private Dictionary<string, Board> boards;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Dictionary<string, Board> Boards { get => boards;}
        public void addBoardToDictionary(string email, Board board)
        {
            if (!boards.ContainsKey(email))
                boards.Add(email, board);
        }
        public BoardController()
        {
            this.boards = new Dictionary<string, Board>();
        }
        /// <summary>
        /// gets an email and check if the email is online and if he is exist
        /// </summary>
        /// <param name="email">the email we want to check</param>
        /// <returns>true if the email is valid</returns>
        private bool validUser(string email)
        {
            if (!(boards.ContainsKey(email)))
            {
                log.Error($"this email: {email} is not exist in the system");
                throw new Exception("there is no such user");
            }
            if (!(boards[email].LoggedBoard.Equals(email)))
            {
                log.Error($"{email} failed to commit changes since he is offline");
                throw new Exception("user is offline");
            }
            return true;
        }
        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public void LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            validUser(email);
            if (!valideteEditColumn(email))
            {
                log.Warn("Limit update failed : " + email + " because it is not the board creator");
                throw new Exception("not the board creator");
            }
            if (!(boards[email].GetColumn(columnOrdinal).updateLimit(limit)))
            {
                log.Warn("limit failed : " + email + " trying to put invalid limit number of tasks");
                throw new Exception("limit must be positive");
            }
            log.Info($"{email} successfully limited column{columnOrdinal} tasks to {limit}");
        }
        /// <summary>
        /// Returns a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Column GetColumn(string email, string name)
        {
            validUser(email);
            Column toReturn = boards[email].GetColumn(name);
            log.Info($"{email} successfully get column:{name}");
            return toReturn;
        }
        /// <summary>
        /// Returns a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Column GetColumn(string email, int columnOrdinal)
        {
            validUser(email);
            Column ToReturn = boards[email].GetColumn(columnOrdinal);
            log.Info($"{email} successfully get column{columnOrdinal}");
            return ToReturn;
        }
        /// <summary>
        /// Returns the board of a user. The user must be logged in
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public IReadOnlyCollection<string> GetBoard(string email)
        {
            validUser(email);
            List<string> ColNames = new List<string>();
            foreach (Column a in boards[email].Columns)
                ColNames.Add(a.Name);
            ReadOnlyCollection<string> readOnlyNames = new ReadOnlyCollection<string>(ColNames);
            log.Info($"{email} successfully get Board");
            return readOnlyNames;
        }

        /// <summary>
        /// Removes a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public void RemoveColumn(string email, int columnOrdinal)
        {
            validUser(email);
            if (!valideteEditColumn(email))
            {
                log.Warn("Remove failed : " + email + " because it is not the board creator");
                throw new Exception("not the board creator");
            }
            boards[email].Remove(columnOrdinal);
            log.Info($"{email} successfully removed column{columnOrdinal}");
        }
        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Location to place to column</param>
        /// <param name="Name">new Column name</param>
        /// <returns>A response object with a value set to the new Column, the response should contain a error message in case of an error</returns>
        public Column AddColumn(string email, int columnOrdinal, string Name)
        {
            validUser(email);
            Column ToReturn;
            if (boards[email].Email == email)
                ToReturn = boards[email].AddColumn(columnOrdinal, Name);
            else
            {
                log.Warn($"{email} tried to add column to someone elses board");
                throw new Exception("cannot  add column to someone elses board");
            }   
            log.Info($"{email} successfully added column:{Name}");
            return ToReturn;
        }
        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Column MoveColumnRight(string email, int columnOrdinal)
        {
            validUser(email);
            if (!valideteEditColumn(email))
            {
                log.Warn("Move failed : " + email + " because it is not the board creator");
                throw new Exception("not the board creator");
            }
            Column ToReturn = boards[email].MoveColumnRight(columnOrdinal);
            log.Info($"{email} successfully move column{columnOrdinal} right");
            return ToReturn;
        }
        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Column MoveColumnLeft(string email, int columnOrdinal)
        {
            validUser(email);
            if (!valideteEditColumn(email))
            {
                log.Warn("Move failed : " + email + " because it is not the board creator");
                throw new Exception("not the board creator");
            }
            Column ToReturn = boards[email].MoveColumnLeft(columnOrdinal);
            log.Info($"{email} successfully move column{columnOrdinal} left");
            return ToReturn;
        }
        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Task AddTask(string email, string title, string description, DateTime dueDate)
        {
            validUser(email);
            Task newAdded = this.boards[email].CreateNewTask(title, description, dueDate, email);
            log.Info(email + " successfully added a new task");
            return newAdded;
        }
        public void clear()
        {
            this.boards = new Dictionary<string, Board>();
        }
        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public bool UpdateTaskDueDate(string email, int columnOrdinal, int taskID, DateTime dueDate)
        {
            validUser(email);
            if (!valideteEditTask(email, columnOrdinal, taskID))
            {
                log.Warn("Update failed : " + email + " because he is not the task assignee");
                throw new Exception("only task creator can update task details");
            }
            if (columnOrdinal == boards[email].Columns.Count - 1)
            {
                log.Warn($"{email} failed to update task{taskID} since its in the rightmost column");
                throw new Exception("update task from rightmost column is forbidden Operation");
            }
            Boards[email].GetColumn(columnOrdinal).GetTaskByID(taskID).updateTaskDueDate(dueDate);
            log.Info($"{email} successfully updated task due date to:{dueDate}");
            return true;
        }
        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public bool UpdateTaskTitle(string email, int columnOrdinal, int taskID, string title)
        {
            validUser(email);
            if (!valideteEditTask(email, columnOrdinal, taskID))
            {
                log.Warn("Update failed : " + email + " because he is not the task assignee");
                throw new Exception("only task creator can update task details");
            }
            if (columnOrdinal == boards[email].Columns.Count - 1)
                throw new Exception("update task from rightmost column is forbidden Operation");
            Boards[email].GetColumn(columnOrdinal).GetTaskByID(taskID).updateTaskTitle(title);
            log.Info($"{email} successfully updated task title to:{title}");
            return true;
        }
        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public bool UpdateTaskDescription(string email, int columnOrdinal, int taskID, string description)
        {
            validUser(email);
            if (!valideteEditTask(email, columnOrdinal, taskID))
            {
                log.Warn("Update failed : " + email + " because he is not the task assignee");
                throw new Exception("only task creator can update task details");
            }
            if (columnOrdinal == boards[email].Columns.Count - 1)
                throw new Exception("update task from rightmost column is forbidden Operation");
            Boards[email].GetColumn(columnOrdinal).GetTaskByID(taskID).updateTaskDescription(description);
            log.Info($"{email} successfully updated task description to:{description}");
            return true;
        }
        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            validUser(email);
            if (!valideteEditTask(email, columnOrdinal, taskId))
            {
                log.Warn("Advance failed : " + email + " because he is not the task assignee");
                throw new Exception("only task creator can advance task");
            }
            Boards[email].advanceTask(columnOrdinal, taskId);
            log.Info($"{email} successfully advanced task{taskId}");
        }
        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            validUser(email);
            if (!valideteEditTask(email, columnOrdinal, taskId))
            {
                log.Warn("Assign failed : " + email + " because he is not the task assignee");
                throw new Exception("only task creator can update task details");
            }
            if (!boards[email].Equals(boards[emailAssignee]))
            {
                log.Warn("Assign failed : " + email + " the new assignee is not part of the board");
                throw new Exception("the new assignee is not part of the board");
            }
            boards[email].GetColumn(columnOrdinal).GetTaskByID(taskId).Email = emailAssignee;
        }

        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            validUser(email);
            if (!valideteEditTask(email, columnOrdinal, taskId))
            {
                log.Warn("Assign failed : " + email + " because he is not the task assignee");
                throw new Exception("only task creator can delete task");
            }
            Column toDelete = boards[email].GetColumn(columnOrdinal);
            toDelete.GetTaskByID(taskId).Delete();
            toDelete.removeTask(taskId);
        }

        /// <summary>        
        /// Loads the data. Intended be invoked only when the program starts
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error.</returns>
        public void LoadData()
        {
            DalBoardController DBC = new DalBoardController();
            DalColumnController DCC = new DalColumnController();
            DalTaskController DTC = new DalTaskController();
            List<DataAccessLayer.Board> boardss = DBC.SelectAllBoards();
            List<DataAccessLayer.Column> columnss = DCC.SelectAllColumns();
            List<DataAccessLayer.Task> Taskss = DTC.SelectAllTasks();
            foreach (DataAccessLayer.Board dalBoard in boardss)
            {
                LinkedList<Column> BusColumns = new LinkedList<Column>();
                List<DataAccessLayer.Column> DalColumns = columnss.FindAll(c => c.Email == dalBoard.UserEmail);
                DalColumns.Sort((a, b) => (a.ColOridinal.CompareTo(b.ColOridinal)));
                foreach (DataAccessLayer.Column dalCol in DalColumns)
                {
                    List<DataAccessLayer.Task> DalTasks = Taskss.FindAll(t => t.ColDalID == dalCol.DalID);
                    List<Task> BusTasks = new List<Task>();
                    foreach (DataAccessLayer.Task dalTask in DalTasks)
                    {
                        BusTasks.Add(new Task(dalTask));
                    }
                    BusColumns.AddLast(new Column(dalCol, BusTasks));
                }
                Board busBoard = new Board(dalBoard, BusColumns);
                boards.Add(dalBoard.UserEmail, busBoard);
            }
            log.Info("system has been successfully loaded boards");
        }
        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            validUser(email);
            if (!valideteEditColumn(email))
            {
                log.Warn("change failed : " + email + " because it is not the board creator");
                throw new Exception("not the board creator");
            }
            if(boards[email].CheckIfColNameExists(newName))
            {
                log.Warn("change failed : " + email + " try to change a column name to exists name");
                throw new Exception("not the board creator");
            }
            Column ColToChange = boards[email].GetColumn(columnOrdinal);
            ColToChange.Name = newName;
        }
        private bool valideteEditColumn(string email)
        {
            if (!boards[email].Email.Equals(email))
            {
                return false;
            }
            return true;
        }
        private bool valideteEditTask(string email, int columnID, int taskID)
        {
            if (!boards[email].GetColumn(columnID).GetTaskByID(taskID).Email.Equals(email))
            {
                return false;
            }
            return true;
        }

    }
}