using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Custom event object to describe column
    /// </summary>
    public class TaskEventArgs : EventArgs
    {
        private TaskModel task;
        public TaskModel Task
        {
            get => task;
            set
            {
                task = value;
            }
        }

        public string Title { get => title; set => title = value; }
        public string Des {
            get => des;
            set
            {
                des = value;
                desChanged = true;
            }
        }
        public string BorderBrush { get => borderBrush; set => borderBrush = value; }
        
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public string EmailAssign {
            get => emailAssign;
            set {
                if (emailAssign != value)
                    borderBrush = "Black";
                emailAssign = value;
            }
        }
        public bool IsDeleted { get => isDeleted; set => isDeleted = value; }
        public bool DesChanged { get => desChanged; }
        private bool isDeleted = false;
        private string title;
        private string des;
        private bool desChanged = false;
        private DateTime dueDate;
        private string emailAssign;
        private string borderBrush;
    }
    /// <summary>
    /// Interaction logic for AddTaskView.xaml
    /// </summary>
    public partial class AddTaskView : Window
    {
        private AddTaskViewModel viewModel;

        public delegate void DataChangedEventHandler(object sender, TaskEventArgs e);
        public event DataChangedEventHandler DataChanged;

        public AddTaskView(UserModel user)
        {
            InitializeComponent();
            this.viewModel = new AddTaskViewModel(user);
            this.DataContext = viewModel;
        }
        /// <summary>
        /// trigger to the functions that subscribed to the event
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnAddTaskView(TaskEventArgs args)
        {
            DataChanged?.Invoke(this, args);
        }

        /// <summary>
        /// tries to add a new task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
           TaskModel added =  viewModel.AddTask();
            if (added != null)
            {
                TaskEventArgs args = new TaskEventArgs();
                args.Task = added;
                OnAddTaskView(args);
                this.Close();
            }
        }
    }
}
