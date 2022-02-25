using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    //an abstract class to use for all dal objects
    internal abstract class DalObject
    {
        public const string EmailColumnName = "Email";
        public const string IDColumnName = "DalID";
        protected DalController _controller;
        private int dalID;

        /// <summary>
        /// simple constructor
        /// </summary>
        /// <param name="controller">the controller of specific class</param>
        protected DalObject(DalController controller)
        {
            _controller = controller;
        }

        /// <summary>
        /// delete the dal object from data base
        /// </summary>
        public void delete()
        {
            this._controller.Delete(this);
        }

        public int DalID { get => dalID; set => dalID = value; }
    }
}
