using System;
using System.Threading.Tasks;
using System.Windows.Media;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation.Model
{
    public class TaskModel : NotifiableModelObject
    {
        private int taskID;

        private string borderBrush="Black";
        public string BorderBrush
        {
            get => borderBrush;
            set
            {
                this.borderBrush = value;
                RaisePropertyChanged("BorderBrush");
            }
        }

        private string _emailAssignee;
        public string EmailAssignee
        {
            get => _emailAssignee;
            set
            {
                this._emailAssignee = value;
                RaisePropertyChanged("EmailAssignee");
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                SolidColorBrush toRetrun;
                if (DateTime.Today > DueDate)
                    toRetrun = new SolidColorBrush(Colors.Red);
                else if ((DateTime.Today - CreationDate).Days >= 0.75 * (DueDate - CreationDate).Days)
                    toRetrun = new SolidColorBrush(Colors.Orange);
                else
                    toRetrun = new SolidColorBrush(Colors.White);
                return toRetrun;
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                this._description = value;
                RaisePropertyChanged("Description");
            }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                this._dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        private DateTime _creationDate;
        public DateTime CreationDate
        {
            get => _creationDate;
            set
            {
                this._creationDate = value;
                RaisePropertyChanged("CreationDate");
            }
        }

        public int TaskID { get => taskID; set => taskID = value; }

        public TaskModel(BackendController controller, int taskID,string email,string title,string desc,DateTime due,DateTime creation) : base(controller)
        {
            this.taskID = taskID;
            this.EmailAssignee = email;
            this.Title = title;
            this.Description = desc;
            this.DueDate = due;
            this.CreationDate = creation;
        }

        public TaskModel(string email,BackendController controller, IntroSE.Kanban.Backend.ServiceLayer.Task task) : this(controller, task.Id,task.emailAssignee, task.Title, task.Description, task.DueDate, task.CreationTime)
        {
            if (email == EmailAssignee)
                borderBrush = "Blue";
            else
                borderBrush = "Black";
        }
    }
}
