using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class Board: DalObject
    {
        public const string TaskCountColumnName = "TaskCount";
        private string userEmail;
        private int taskCount;
        public Board(int dalID,string userEmail,int taskCount,bool flag) : base(new DalBoardController())
        {
            this.userEmail = userEmail;
            this.taskCount = taskCount;
            DalBoardController temp = (DalBoardController)_controller;
            if (flag)//case of new object
                DalID = temp.Insert(this);
            else//case of loading object from database
                DalID = dalID;
        }

        //simple consturctor
        public Board() : base(null) { }

        //setter and getter for UserEmail
        public string UserEmail {
            get => userEmail;
            set{
                this.userEmail = value;
                _controller.Update(DalID,EmailColumnName, value);
            }
        }

        //setter and getter for TaskCount
        public int TaskCount { get => taskCount;
            set
            {
                this.taskCount = value;
                _controller.Update(DalID, TaskCountColumnName, value);
            }
        }
    }
}

