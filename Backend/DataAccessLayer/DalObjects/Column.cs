using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class Column:DalObject
    {
        public const string NameColumnName = "Name";
        public const string LimitColumnName = "Lim";
        public const string ColOrdinalColumnName = "ColumnOrdinal";
        private string name;
        private int limit;
        private int colOridinal;
        private string email;

        //simple constructor
        public Column() : base(new DalColumnController()) { }


        public Column(int dalID,string name, int limit, int colOridinal,string email,bool flag) : base(new DalColumnController())
        {
            this.email = email;
            this.name = name;
            this.limit = limit;
            this.colOridinal = colOridinal;
            DalColumnController temp = (DalColumnController)_controller;
            if (flag)//case of new column
                DalID = temp.Insert(this);
            else//case of loading column from data base
                DalID = dalID;
        }

        //getter and setter for Name
        public string Name
        {
            get => name;
            set
            {
                this.name = value;
                _controller.Update(DalID,NameColumnName, value);
            }
        }

        //getter and setter for limit
        public int Limit {
            get => limit;
            set {
                this.limit = value;
                _controller.Update(DalID,LimitColumnName,value);
            }
        }

        //getter and setter for column Ordinal
        public int ColOridinal
        {
            get => colOridinal;
            set
            {
                this.colOridinal = value;
                _controller.Update(DalID,ColOrdinalColumnName, value);
            }
        }

        //getter and setter for email
        public string Email {
            get => email;
            set
            {
                this.email = value;
                _controller.Update(DalID,EmailColumnName, value);
            }
        }
    }
}
