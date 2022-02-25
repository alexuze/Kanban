    ﻿using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class User : DalObject
    {
        public const string PasswordColumnName = "Password";
        public const string NicknameColumnName = "Nickname";
        public const string EmailHostColumnName = "EmailHost";

        private string email;
        private string password;
        private string nickname;
        private string emailhost;

        public User(int dalID, string email, string password, string nickname, string emailhost, bool flag) : base(new DalUserController())
        {

            this.email = email;
            this.password = password;
            this.nickname = nickname;
            DalUserController temp = (DalUserController)_controller;
            this.emailhost = emailhost;
            if (flag)
                DalID = temp.Insert(this);
            else
                DalID = dalID;
        }

        //simple constructor
        public User() : base(new DalTaskController()) { }

        //getter and setter for email
        public string Email
        {
            get => email;
            set
            {
                DalUserController temp = (DalUserController)_controller;
                temp.Update(this.Email, EmailColumnName, value);
                email = value;
            }
        }

        //getter and setter for password
        public string Password
        {
            get => password;
            set
            {
                DalUserController temp = (DalUserController)_controller;
                temp.Update(this.Email, PasswordColumnName, value);
                password = value;

            }
        }

        //getter for email host
        public string EmailHost
        {
            get => emailhost;
        }

        //getter and setter for Nick Name
        public string Nickname
        {
            get => nickname;
            set
            {
                DalUserController temp = (DalUserController)_controller;
                temp.Update(this.Email, NicknameColumnName, value);
                nickname = value;
            }
        }
    }
}
