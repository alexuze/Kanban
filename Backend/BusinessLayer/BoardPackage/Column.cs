    ﻿using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    internal class Column
    {
        private const int maxName = 15;
        private const int Default_Limit = 100;
        private DataAccessLayer.Column dalCol;
        private string name;
        private int limit;
        private int colOridinal;
        private List<Task> taskByID;
        private int dalID;
        private string email;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Column(int colOridinal, string email, string name)
        {
            checkName(name, 'n');
            this.email = email;
            this.limit = Default_Limit;

            this.colOridinal = colOridinal;
            this.taskByID = new List<Task>();
            this.name = name;
            this.dalCol = new DataAccessLayer.Column(0, this.name, this.limit, this.colOridinal, this.email, true);
            this.dalID = dalCol.DalID;
        }
        public Column() { }
        public Column(DataAccessLayer.Column dalCol, List<Task> tasks)
        {
            this.dalID = dalCol.DalID;
            this.email = dalCol.Email;
            this.limit = dalCol.Limit;
            this.name = dalCol.Name;
            this.colOridinal = dalCol.ColOridinal;
            this.taskByID = tasks;
            this.dalCol = dalCol;
        }

        public int Limit { get => limit; set => limit = value; }
        internal List<Task> TaskByID { get => taskByID; }

        public string Name
        {
            get => name;
            set
            {
                checkName(value, 'c');
                name = value;
                dalCol.Name = value;
            }
        }
        public int ColOridinal
        {
            get => colOridinal;
            set
            {
                this.colOridinal = value;
                this.dalCol.ColOridinal = value;
            }
        }

        public int DalID { get => dalID; set => dalID = value; }
        internal DataAccessLayer.Column DalCol { get => dalCol; set => dalCol = value; }

        /// <summary>
        /// updates the limit of the column
        /// </summary>
        /// <param name="lim">the new limit</param>
        /// <returns>true if the update succeed</returns>
        public bool updateLimit(int lim)
        {
            if (lim == -1)// -1 means that there is no limit
                this.limit = lim;
            else
            {
                if (lim < taskByID.Count())//check if there is more tasks than the limit
                {
                    log.Warn($"update limit failed to {this.email} : already have more tasks in {this.name} column than the limit");
                    throw new Exception("the column already contain more  than" + lim + " tasks");
                }
                if (lim < 0)//limit cannot be negative
                    return false;
                this.limit = lim;
                dalCol.Limit = lim;
            }
            return true;
        }
        public void delete()
        {
            this.dalCol.delete();
        }
        //public List<Task> sortTasksByDueDate()
        //{
        //    //return taskByID.Sort()
        //}
        public void checkName(string name, char status)
        // status resymbol whitch action was taken in order to add the appropriate log (the name tried to be changed or created)
        //n=new c=change
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                if (status == 'n')
                    log.Warn($"{this.email} tried to create a new task with empty name");
                if (status == 'c')
                    log.Warn($"{this.email} tried to change column{this.colOridinal} to empty name");
                throw new Exception("Name cannot be empty");
            }
            if (name.Length > maxName)
            {
                if (status == 'n')
                    log.Warn($"{this.email} tried to create a new task with illeagal name:" + name);
                if (status == 'c')
                    log.Warn($"{this.email} tried to change column{this.colOridinal} to illeagal name:" + name);
                throw new Exception("Name length is too long");
            }
        }
        /// <summary>
        /// add a new task to the column
        /// </summary>
        /// <param name="newTask">the task we want to add</param>
        public void addTask(Task newTask)
        {
            if (this.limit != -1)
            {
                if (this.limit == this.taskByID.Count())
                {
                    log.Warn($"{this.name} column assosiated to {this.email} is full");
                    throw new Exception("backlog column is full");
                }
            }
            taskByID.Add(newTask);
        }
        /// <summary>
        /// a function the remove a task from the specipic column
        /// </summary>
        /// <param name="taskID">the id of the task we want to remove</param>
        /// <returns>the task that we removed</returns>
        public Task removeTask(int taskID)
        {
            Task deleted = GetTaskByID(taskID);
            taskByID.Remove(GetTaskByID(taskID));
            return deleted;
        }
        /// <summary>
        /// a function that returns a task by is id.
        /// </summary>
        /// <param name="ID">the task id we wants to get</param>
        /// <returns>the task with the ID we inserted</returns>
        public Task GetTaskByID(int ID)
        {
            foreach (Task val in taskByID)
                if (val.Id == ID)
                    return val;
            log.Warn($"the task{ID} of {this.email} is not exists in the {this.name} column");
            throw new Exception("There is no Task");
        }
    }
}
