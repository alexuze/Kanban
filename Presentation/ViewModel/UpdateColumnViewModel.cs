using Presentation.Model;
using Presentation.View;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class UpdateColumnViewModel :NotifiableObject
    {
        private BackendController _controller;
        private Model.ColumnModel _column;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="controller">the controller of the system</param>
        /// <param name="column">the column we want to update</param>
        public UpdateColumnViewModel(BackendController controller,Model.ColumnModel column,int columnOridinal)
        {
            this._column = column;
            this._controller = controller;
            this.Name = column.Name;
            this.Limit = column.Limit;
            this.columnOridinal = columnOridinal;
        }
        private int columnOridinal;
        /// <summary>
        /// getter and setter for name
        /// </summary>
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        /// <summary>
        /// getter and setter for limit
        /// </summary>
        private int _lim;
        public int Limit
        {
            get => _lim;
            set
            {
                _lim = value;
                RaisePropertyChanged("Limit");
            }
        }
        /// <summary>
        /// getter and setter for message
        /// </summary>
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }
        /// <summary>
        /// a function that calls the controller to update the the column name
        /// </summary>
        public void UpdateName()
        {
            Message = "";
            try
            {
                _controller.UpdateColumnName(_column.UserEmail,columnOridinal,Name);
                Message = "Name Changed successfully";
                _column.Name = Name;
            }
            catch (Exception ee)
            {
                Message = ee.Message;
            }
        }

        /// <summary>
        /// a function that calls the controller to update the the column limit
        /// </summary>
        public void UpdateLimit()
        {
            Message = "";
            try
            {
                _controller.UpdateColumnLim(_column.UserEmail, columnOridinal, _lim);
                Message = "Limit Changed successfully";
                _column.Limit = Limit;
            }
            catch (Exception ee)
            {
                Message = ee.Message;
            }
        }
        /// <summary>
        /// a function that calls the controller to move the column right
        /// </summary>
        /// <returns>the messsage occurs</returns>
        public string MoveColumnRight(BoardViewModel board)
        {
            Message = "";
            try
            {
                _controller.MoveColRight(_column.UserEmail, columnOridinal);
                Model.ColumnModel ColToRight = board.Board.Columns[columnOridinal];
                Model.ColumnModel ColToLeft = board.Board.Columns[columnOridinal + 1];
                board.Board.Columns[columnOridinal] = ColToLeft;
                board.Board.Columns[columnOridinal + 1] = ColToRight;
                return "Column moved Right successfully";
                
            }
            catch (Exception ee)
            {
                Message = ee.Message;
                return "";
            }
        }

        /// <summary>
        /// a function that calls the controller to move the column left
        /// </summary>
        /// <returns>the message occurs</returns>
        public string MoveColumnLeft(BoardViewModel board)
        {
            Message = "";
            try
            {
                _controller.MoveColLeft(_column.UserEmail, columnOridinal);
                Model.ColumnModel colToLeft = board.Board.Columns[columnOridinal];
                Model.ColumnModel colToRight = board.Board.Columns[columnOridinal - 1];
                board.Board.Columns[columnOridinal] = colToRight;
                board.Board.Columns[columnOridinal - 1] = colToLeft;
                return "Column moved Left successfully";
            }
            catch (Exception ee)
            {
                Message = ee.Message;
                return "";
            }
        }
    }
}
