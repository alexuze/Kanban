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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        BoardViewModel viewModel;

        internal BoardViewModel ViewModel { get => viewModel;}

        public BoardView(UserModel user)
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(user);
            this.DataContext = viewModel;
        }
        /// <summary>
        /// function triggered when event of add task occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTask_DataChanged(object sender, TaskEventArgs e)
        {
            viewModel.Board.Columns[0].Tasks.Add(e.Task);
        }
        /// <summary>
        /// deletion of a selected task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteTask();
        }
        /// <summary>
        /// delete the selected column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteColumn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteColumn();
            
        }
        /// <summary>
        /// creates new window to update the column properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateColumn_click(object sender, RoutedEventArgs e)
        {
            UpdateColumnView updateColumn = viewModel.UpdateColumn(this);
            updateColumn.ShowDialog();
            ColumnList.UpdateLayout();
        }

        /// <summary>
        /// creates a new window to add column and subscribing to an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            AddColumnView addColumn = new AddColumnView(viewModel.User);
            addColumn.DataChanged += AddColumn_DataChanged;
            addColumn.ShowDialog();
        }
        /// <summary>
        /// event triggered when you added a column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddColumn_DataChanged(object sender, ColumnEventArgs e)
        {
            viewModel.Board.Columns.Insert(e.ID,e.Column);
            
        }
        /// <summary>
        /// creates a new window to add a task and subscribing to an event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTask_Click_1(object sender, RoutedEventArgs e)
        {
            AddTaskView addTask = new AddTaskView(viewModel.User);
            addTask.DataChanged += AddTask_DataChanged;
            addTask.ShowDialog();
        }
        /// <summary>
        /// creates a new window to update the selected task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            UpdateTaskView updateTask = viewModel.UpdateTask(this);
            updateTask.ShowDialog();
            ColumnList.UpdateLayout();
        }
        /// <summary>
        /// logout function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout(this);
        }
        /// <summary>
        /// Due Date sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            viewModel.SortByDueDate();
            
        }
    }
}
