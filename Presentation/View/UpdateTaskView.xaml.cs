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
    /// Interaction logic for UpdateTaskView.xaml
    /// </summary>
    public partial class UpdateTaskView : Window
    {
        private UpdateTaskViewModel viewModel;
        private BoardView Board;

        public UpdateTaskView(TaskModel task,int columnOrdinal,BoardView board)
        {
            InitializeComponent();
            viewModel = new UpdateTaskViewModel(task,columnOrdinal,board.ViewModel.User.Email);
            this.Board = board;
            this.DataContext = viewModel;
        }
        /// <summary>
        /// returns to the board window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Return_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// tries to advance the task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Advance_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.AdvanceTask(Board.ViewModel) != "")
            {
                this.Close();
            }
        }
        /// <summary>
        /// tries to assign the task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Assign_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateAssign();
        }
        /// <summary>
        /// updates the due date of the selected task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DueDate_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateDueDate();
        }
        /// <summary>
        /// updates the description of the selected task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Des_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateDescription();
        }
        /// <summary>
        /// updates the title of the selected task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Title_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateTitle();    
        }
    }
}
