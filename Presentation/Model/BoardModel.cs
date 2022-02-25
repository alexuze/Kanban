using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private readonly UserModel _user;
        private ObservableCollection<ColumnModel> _columns;
        public ObservableCollection<ColumnModel> Columns
        {
            get => _columns;
            set
            {
                _columns = value;
                RaisePropertyChanged("Columns");
            }
        }
        public BoardModel(BackendController controller,UserModel user) : base(controller)
        {
            this._user = user;
            this.Columns = new ObservableCollection<ColumnModel>(Controller.GetColumnsNames(_user).
                Select((c, i) => new ColumnModel(Controller, Controller.GetColumn(user.Email, c), user.Email)));
        }
        public BoardModel(BackendController controller, ObservableCollection<ColumnModel> columns) : base(controller)
        {
            this.Columns = columns;
        }

        internal void DeleteColumn(ColumnModel selectedColumn)
        {
            int index = Columns.IndexOf(selectedColumn);
            if(index == 0)
            {
                MoveTaskFromColumn(selectedColumn, Columns[1]);
            }
            else
            {
                MoveTaskFromColumn(selectedColumn, Columns[index - 1]);
            }
            Columns.Remove(selectedColumn);
        }

        private void MoveTaskFromColumn(ColumnModel columnToMoveFrom,ColumnModel columnToMoveTo)
        {
            foreach (TaskModel task in columnToMoveFrom.Tasks)
            {
                columnToMoveTo.Tasks.Add(task);
            }
        }
    }
}
