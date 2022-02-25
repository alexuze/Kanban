    ﻿using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    internal class Task
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const int maxLenghDescription = 300;
        public const int maxLenghTitle = 50;
        private DataAccessLayer.Task dalTask;
        private int id;
        private string title;
        private string description;
        private string email;
        private DateTime creationtime;
        private DateTime dueDate;
        private int dalID;
        private int colDalID;

        public Task(int id, string title, string description, string email, DateTime dueDate, int colDalID)
        {
            this.colDalID = colDalID;
            this.id = id;
            this.title = title;
            this.description = description;
            this.email = email;

            this.creationtime = DateTime.Now;
            this.dueDate = dueDate;
            this.dalTask = new DataAccessLayer.Task(0, this.id, this.title, this.description, this.Email, this.creationtime, this.dueDate, colDalID, true);

            this.dalID = dalTask.DalID;
        }
        public Task(DataAccessLayer.Task dalTask)
        {
            this.id = dalTask.Id;
            this.title = dalTask.Title;
            this.description = dalTask.Description;
            this.email = dalTask.Email;

            this.creationtime = dalTask.Creationtime;
            this.dueDate = dalTask.DueDate;
            this.colDalID = dalTask.ColDalID;
            this.dalID = dalTask.DalID;
            this.dalTask = dalTask;
        }

        public int Id { get => id; }
        public DateTime Creationtime { get => creationtime; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public string Description { get => description; }
        public string Title { get => title; }
        public int DalID { get => dalID; set => dalID = value; }
        public string Email
        {
            get => email;
            set
            {
                email = value;
                dalTask.Email = value;
            }
        }

        public int ColDalID
        {
            get => colDalID;
            set
            {
                colDalID = value;
                this.dalTask.ColDalID = value;
            }
        }

        public void Delete()
        {
            this.dalTask.delete();
        }

        /// <summary>
        /// updates the task due date
        /// </summary>
        /// <param name="dueDate">the new due date</param>
        /// <returns>true if the updated succeed, else throws exeption</returns>
        public bool updateTaskDueDate(DateTime dueDate)
        {
            if (DateTime.Now >= dueDate)
            {
                log.Warn($"update task : {id} failed: the due date has passed.  the given due date is : {dueDate}");
                throw new Exception("wrong date");
            }
            this.dueDate = dueDate;
            this.dalTask.DueDate = dueDate;
            return true;
        }

        /// <summary>
        /// updates the title of the task
        /// </summary>
        /// <param name="title">the new title</param>
        /// <returns>true if the updated succeed, else throws exeption</returns>
        public bool updateTaskTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title) || title.Length > maxLenghTitle)
            {
                log.Warn($"update task{id} associated to {Email} failed : the title is empty or passed the max length. the gived title is: {title}");

                throw new Exception($"title must be between 1 to {maxLenghTitle} chars");
            }
            this.title = title;
            this.dalTask.Title = title;
            return true;
        }
        /// <summary>
        /// updates the description of the task
        /// </summary>
        /// <param name="description">the new description</param>
        /// <returns>true if the updated succeed, else throws exeption</returns>
        public bool updateTaskDescription(string description)
        {
            if (description != null)
            {
                if (description.Length > maxLenghDescription)
                {
                    log.Warn($"update task{id} associated to: {Email} failed : the description has passed the valid max length. the given desscription is: {description}");

                    throw new Exception("description is too long");
                }
            }
            this.description = description;
            this.dalTask.Description = description;
            return true;
        }
    }
}
