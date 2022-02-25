    ﻿using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using IntroSE.Kanban.Backend.DataAccessLayer;
namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    internal class Board
    {
        private int taskCount;
        private const int MinNumOfColumns = 2;
        private LinkedList<Column> columns;
        private string email;
        private bool isLogged;
        private int dalID;
        private string loggedBoard;
        private DataAccessLayer.Board dalBoard;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool IsLogged { get => isLogged; set => isLogged = value; }
        public int TaskCount { get => taskCount; set => taskCount = value; }
        public LinkedList<Column> Columns { get => columns; set => columns = value; }
        public string Email { get => email; set => email = value; }
        public string LoggedBoard { get => loggedBoard; set => loggedBoard = value; }

        internal Board(string email)//normal constructor
        {
            this.columns = new LinkedList<Column>();
            this.columns.AddFirst(new Column(2, email, "done"));
            this.columns.AddFirst(new Column(1, email, "in progress"));
            this.columns.AddFirst(new Column(0, email, "backlog"));
            this.email = email;
            this.isLogged = false;
            this.taskCount = 0;
            this.loggedBoard = "";
            this.dalBoard = new DataAccessLayer.Board(0, this.email, this.TaskCount, true);

            this.dalID = dalBoard.DalID;
        }
        public Board(DataAccessLayer.Board dalBoard, LinkedList<Column> columns)//constructor after load
        {
            this.dalID = dalBoard.DalID;
            this.columns = new LinkedList<Column>();
            this.columns = columns;
            this.Email = dalBoard.UserEmail;
            this.loggedBoard = "";
            this.isLogged = false;
            this.taskCount = dalBoard.TaskCount;
            this.dalBoard = dalBoard;
        }
        public bool CheckIfColNameExists(string name)
        {
            foreach (Column a in columns)
            {
                if (a.Name == name)
                {
                    return true;
                }
            }
            return false;
        }
        public Column GetColumn(int ColumnOrdinal)
        {
            if (!(this.isLogged))//checks if user is online
            {
                log.Warn($"{Email} failed to get column while he is offline");

                throw new Exception("user is offline");
            }
            if (ColumnOrdinal < 0 || ColumnOrdinal > this.columns.Count - 1)//checks in columnOrdinal is in range
            {
                log.Warn($"{Email} failed to get column with illegal column Ordinal:{ColumnOrdinal}");

                throw new Exception("Illegal ColumnOrdinal");
            }
            return this.columns.ElementAt(ColumnOrdinal);
        }
        //function that get the name of a column and returns the column
        public Column GetColumn(string ColumnName)
        {
            if (!(this.isLogged))//checks if user is online
            {
                log.Warn($"{Email} failed to get column while he is offline");
                throw new Exception("user is offline");
            }
            Column helper = null;
            foreach (Column a in columns)//find in each column name if the name exists
            {
                if (a.Name.Equals(ColumnName))
                {
                    helper = a;
                    break;
                }
            }
            if (helper == null)//case the name is not exists
            {
                log.Warn($"{Email} failed to get a column without an existing name:{ColumnName}");

                throw new Exception("cant get column without an existing name");
            }
            return helper;
        }
        public void Remove(int colNum)
        {
            if (columns.Count <= MinNumOfColumns)//case board has minimun number of columns
            {
                log.Warn($"{Email} failed to remove column{colNum} while he have minimum number of columns");

                throw new Exception("minimum columns allowed is " + MinNumOfColumns);
            }
            if (colNum < 0 || colNum > columns.Count - 1)//case the column ordinal is not exists
            {
                log.Warn($"{Email} failed to remove column{colNum} which is not exists");

                throw new Exception("illeagal column oridinal");
            }
            Column toDelete = GetColumn(colNum);
            Column toPass;
            if (colNum != 0)
                toPass = GetColumn(colNum - 1);
            else
                toPass = GetColumn(1);
            if (toDelete.TaskByID.Count() + toPass.TaskByID.Count() > toPass.Limit && toPass.Limit != -1)
            {
                log.Warn($"{Email} failed to remove column{colNum} while he dont have available place to move all the column tasks");

                throw new Exception("there is no where to pass tasks");
            }
            else
            {
                foreach (Task a in toDelete.TaskByID)//changes all the tasks colDalID to the new column
                    a.ColDalID = toPass.DalID;
                toPass.TaskByID.AddRange(toDelete.TaskByID);
            }
            columns.Remove(toDelete);
            toDelete.delete();
            fix();
        }
        public Column AddColumn(int columnOrdinal, string Name)
        {
            if (columnOrdinal < 0 || columnOrdinal > columns.Count)//checks if the column ordinal is legal
            {
                log.Warn($"{Email} failed to add column with illeagal column ordianl:{columnOrdinal}");
                throw new Exception("illeagal column oridinal");
            }
            foreach (Column a in columns)
            {
                if (a.Name == Name)
                {
                    log.Warn($"{Email} failed to add column with the name {Name} which is allready exists");

                    throw new Exception("Column name already exist in the board");
                }
            }
            Column NewColumn = new Column(columnOrdinal, this.Email, Name);

            if (columns.Count == columnOrdinal)
                columns.AddLast(NewColumn);
            else
            {
                LinkedListNode<Column> find = columns.Find(GetColumn(columnOrdinal));
                columns.AddBefore(find, NewColumn);
            }
            fix();
            return NewColumn;
        }
        public Column MoveColumnRight(int columnOrdinal)
        {
            if (columnOrdinal == columns.Count - 1)
            {
                log.Warn($"{Email} failed to move the rightmost column right");

                throw new Exception("can't swip with the most righted column");
            }
            LinkedListNode<Column> find = columns.Find(GetColumn(columnOrdinal));
            Column colLeft = find.Value;
            find.Value = find.Next.Value;
            find.Next.Value = colLeft;
            find.Value.ColOridinal--;
            find.Next.Value.ColOridinal++;
            return colLeft;
        }
        public Column MoveColumnLeft(int columnOrdinal)
        {
            if (columnOrdinal == 0)
            {
                log.Warn($"{Email} failed to move the leftmost column left");

                throw new Exception("can't swip with the most lefted column");
            }
            LinkedListNode<Column> find = columns.Find(GetColumn(columnOrdinal));
            Column colRight = find.Value;
            find.Value = find.Previous.Value;
            find.Previous.Value = colRight;
            find.Value.ColOridinal++;
            find.Previous.Value.ColOridinal--;
            return colRight;
        }
        public void fix()//fix all columns ordinal by place in columns linked list
        {
            int i = 0;
            foreach (Column a in columns)
            {
                a.ColOridinal = i;
                i++;
            }
        }
        //function that create a new task
        public Task CreateNewTask(string title, string des, DateTime dueDate, string email)
        {
            Task newTask;
            if (!(this.isLogged))//checks if the user is online
            {
                log.Warn($"{email} failed to create new task for the reason he is not online");
                throw new Exception("user is offline");
            }
            if (string.IsNullOrWhiteSpace(title) || title.Length > Task.maxLenghTitle)//checks if the title is valid
            {
                log.Warn($"{email} failed to create a new task with the title:{title}");
                throw new Exception($"title must be between 1 to {Task.maxLenghTitle} chars");
            }
            if (des != null)
            {
                if (des.Length > Task.maxLenghDescription)//checks if the description is valid
                {
                    log.Warn($"{email} failed to create a new task with the description:{des}");
                    throw new Exception("description is too long");
                }
            }
            if (dueDate <= DateTime.Now)//checks if the dueDate is valid
            {
                log.Warn($"{email} failed to set duedate:{dueDate} because it has passed");
                throw new Exception("cannot set dueDate from the past");
            }
            if (GetColumn(0).Limit == GetColumn(0).TaskByID.Count())//checks if the backlog is not full
            {
                log.Warn($"{email} try to add task while first column has reached to the limit");
                throw new Exception("leftmost culumn has reached to the limit");
            }
            Column first = this.columns.First.Value;//adds the task to the first column
            newTask = new Task(taskCount, title, des, email, dueDate, first.DalID);

            first.addTask(newTask);
            taskCount++;//a counter for the tasks id
            dalBoard.TaskCount = taskCount;
            return newTask;
        }
        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public void advanceTask(int columnOrdinal, int taskID)
        {
            if (!(this.isLogged))//checks if the user is online
            {
                log.Warn($"{Email} failed to make changes for the reason he is offline");

                throw new Exception("user is offline");
            }
            if (columnOrdinal == columns.Count - 1)// impossible to advance task from the done column
            {
                log.Warn($"{Email} failed to advance task{taskID} since it's under the rightmost column");

                throw new Exception("can't advance tasks that under the right most column ");
            }
            if (GetColumn(columnOrdinal + 1).Limit == GetColumn(columnOrdinal + 1).TaskByID.Count())//checks if the next column is not full
            {
                log.Warn($"{Email} failed to advance task{taskID} because the next column has reached to the limit");

                throw new Exception("the next column has reached to the limit");
            }
            GetColumn(columnOrdinal + 1).addTask(GetColumn(columnOrdinal).removeTask(taskID));//adds the task to the next column and delete from the current
            GetColumn(columnOrdinal + 1).GetTaskByID(taskID).ColDalID = GetColumn(columnOrdinal + 1).DalID;//changes the ColDalID of the task to curr column
        }
    }
}