using System;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class Task :DalObject
    {
        public const string IDName = "ID";
        public const string TitleColumnName = "Title";
        public const string DescriptionColumnName = "Description";
        public const string CreationTimeColumnName = "CreationTime";
        public const string DueDateColumnName = "DueDate";
        public const string ColDalIDColumnName = "ColumnDalID";

        private int id;
        private string title;
        private string description;
        private string email;
        private DateTime creationtime;
        private DateTime dueDate;
        private int colDalID;

        //simple constructor
        public Task():base(new DalTaskController()) { }

        public Task(int dalID,int id, string title, string description, string email, DateTime creationtime, DateTime dueDate,int colDal,bool flag) : base(new DalTaskController())
        {
            this.colDalID = colDal;
            this.id = id;
            this.title = title;
            this.description = description;
            this.email = email;
            this.creationtime = creationtime;
            this.dueDate = dueDate;
            DalTaskController temp =  (DalTaskController)_controller;
            if (flag)//case of new task
                DalID = temp.Insert(this);
            else//case of loading task from database
                DalID = dalID;
        }

        //getter and setter for task id
        public int Id {
            get => id;
            set{
                this.id = value;
                _controller.Update(DalID, IDName, value);
            }
        }

        //getter and setter for title
        public string Title {
            get => title;
            set{
                this.title = value;
                _controller.Update(DalID, TitleColumnName, value);
            }
        }

        //getter and setter for description
        public string Description {
            get => description;
            set{
                this.description = value;
                _controller.Update(DalID, DescriptionColumnName, value);
            }
        }

        //getter and setter for email.
        public string Email
        {
            get => email;
            set
            {
                this.email = value;
                _controller.Update(DalID, EmailColumnName, value);
            }
        }

        //getter and setter for creation time
        public DateTime Creationtime {
            get => creationtime;
            set {
                this.Creationtime = value;
                _controller.Update(DalID, CreationTimeColumnName, value.ToString());
            }
        }

        //getter and setter for due date
        public DateTime DueDate {
            get => dueDate;
            set {
                this.dueDate = value;
                _controller.Update(DalID, DueDateColumnName, value.ToString());
            }
        }

        //getter and setter for column ordinal
        public int ColDalID {
            get => colDalID;
            set
            {
                this.colDalID = value;
                _controller.Update(DalID, ColDalIDColumnName, value);
            }
        }
    }
}
