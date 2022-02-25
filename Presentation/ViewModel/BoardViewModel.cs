using Presentation.Model;
using Presentation.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;


namespace Presentation.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
        private ICollectionView taskView;
        private BackendController _controller;
        private UserModel _user;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="user">the user which the board belong to</param>
        public BoardViewModel(UserModel user)
        {
            this._controller = user.Controller;
            this._user = user;
            Header = "Kanban board for " + user.Email;
            Board = user.GetBoard();
            
            
            //columnView.Filter = o => string.IsNullOrEmpty(Filter) ? true : ((TaskModel)o).Title.Contains(Filter) || ((TaskModel)o).Description.Contains(Filter);
        }
        /// <summary>
        /// getter for the user
        /// </summary>
        public UserModel User
        {
            get => _user;
        }
        /// <summary>
        /// getter for the board
        /// </summary>
        public BoardModel Board { get; private set; }

        public string Header { get; private set; }
        /// <summary>
        /// getter for the Selected column
        /// </summary>
        private ColumnModel _selectColumn;
        public ColumnModel SelectedColumn
        {
            get => _selectColumn;
            set
            {
                
                this._selectColumn = value;
                EnableForwardColumn = value != null;
                RaisePropertyChanged("SelectedColumn");
                EnableForwardTask = value == null;
                this._selectedTask = null;
                RaisePropertyChanged("SelectedTask");
            }
        }

        /// <summary>
        /// function that delete column
        /// </summary>
        internal void DeleteColumn()
        {
            Message = "";
            try
            {
                int id = getIndexOfColumn(SelectedColumn);
                _controller.RemoveColumn(User.Email, id);
                Board.DeleteColumn(SelectedColumn);
                Message = $"Deleted column succesfully";
            }
            catch(Exception ee)
            {
                Message = ee.Message;
            }
            
        }

        internal UpdateColumnView UpdateColumn(BoardView boardView)
        {
            return new UpdateColumnView(SelectedColumn, getIndexOfColumn(SelectedColumn), boardView);
        }

        /// <summary>
        /// function that delete the selected task
        /// </summary>
        internal void DeleteTask()
        {
            Message = "";
            try
            {
                _controller.RemoveTask(User.Email, getTaskColumnOrdinal(SelectedTask), SelectedTask.TaskID);
                Message = $"Deleted Task succesfully";
                Board.Columns[getTaskColumnOrdinal(SelectedTask)].Tasks.Remove(SelectedTask);
            }
            catch (Exception ee)
            {
                Message = ee.Message;
            }

        }

        /// <summary>
        /// getter for the selected task
        /// </summary>
        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                
                this._selectedTask = value;
                EnableForwardTask = value != null;
                RaisePropertyChanged("SelectedTask");
                EnableForwardColumn = value == null;
                this._selectColumn = null;
                RaisePropertyChanged("SelectedColumn");

            }
        }
        /// <summary>
        /// Allow the button to be clicked
        /// </summary>
        private bool _enableForwardColumn;
        public bool EnableForwardColumn
        {
            get => _enableForwardColumn;
            private set
            {
                this._enableForwardColumn = value;
                RaisePropertyChanged("EnableForwardColumn");
            }
        }
        /// <summary>
        /// Allow the button to be clicked
        /// </summary>
        private bool _enableForwardTask;
        public bool EnableForwardTask
        {
            get => _enableForwardTask;
            private set
            {
                this._enableForwardTask = value;
                RaisePropertyChanged("EnableForwardTask");
            }
        }
        

        /// <summary>
        /// a getter and setter for message
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
        /// a function that get the column ordinal
        /// </summary>
        /// <param name="col">the column we want to get her index</param>
        /// <returns></returns>
        internal int getIndexOfColumn(ColumnModel col)
        {
            for(int i=0;i<Board.Columns.Count;i++)
            {
                if (Board.Columns[i].Name == col.Name)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// return an instance of UpdateTaskView According to the selected task 
        /// </summary>
        /// <param name="bv"></param>
        /// <returns></returns>
        internal UpdateTaskView UpdateTask(BoardView bv)
        {
            UpdateTaskView TaskviewModel = new UpdateTaskView(SelectedTask, getTaskColumnOrdinal(SelectedTask),bv);
            return TaskviewModel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        internal int getTaskColumnOrdinal(TaskModel task)
        {
            foreach(ColumnModel col in Board.Columns)
            {
                if (col.Tasks.Contains(task))
                    return getIndexOfColumn(col);
            }
            return -1;
        }

        internal void Logout(BoardView bv)
        {
            if (_controller.Logout(User.Email))
            {
                MainWindow main = new MainWindow();
                main.Show();
                bv.Close();
            }
        }
        /// <summary>
        /// function that sorts the tasks by due date
        /// </summary>
        internal void SortByDueDate()
        {
            foreach (var item in Board.Columns)
            {
                item.Tasks = new ObservableCollection<TaskModel>(item.Tasks.OrderBy(t => t.DueDate));
            }
        }
        
        private string filter;
        /// <summary>
        /// Property binded to the filter textbox
        /// </summary>
        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                if (value != filter)
                {
                    filter = value;
                    UpdateTaskViewFilter();
                    RaisePropertyChanged("Filter");
                }
            }
        }
        /// <summary>
        /// private function to help filter the tasks
        /// </summary>
        private void UpdateTaskViewFilter()
        {
            foreach (var item in Board.Columns)
            {
                taskView = CollectionViewSource.GetDefaultView(item.Tasks);
                taskView.Filter = t => string.IsNullOrEmpty(Filter) ? true : ((TaskModel)t).Title.Contains(Filter) || ((TaskModel)t).Description.Contains(Filter);
            }
        }
    }
}
