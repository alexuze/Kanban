using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        private readonly string _userEmail;
        private ObservableCollection<TaskModel> _tasks;
        public ObservableCollection<TaskModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }

        public string _name;
        public string Name
        {
            get => _name;
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        }

        public int _limit;
        public int Limit
        {
            get => _limit;
            set
            {
                this._limit = value;
                RaisePropertyChanged("Limit");
            }
        }


        public string UserEmail => _userEmail;

        public ColumnModel(BackendController controller,string name,int limit,string userEmail) : base(controller)
        {
            this.Name = name;
            this.Limit = limit;
            this._userEmail = userEmail;
        }
        
        public ColumnModel(BackendController controller,Column column,string userEmail) : this(controller,column.Name,column.Limit,userEmail)
        {
            this.Tasks = new ObservableCollection<TaskModel>(column.Tasks.
                Select((c, i) => new TaskModel(userEmail,controller, c)));
        }
    }
}
