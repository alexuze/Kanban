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
using static Presentation.View.AddColumnView;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for UpdateColumn.xaml
    /// </summary>
    public partial class UpdateColumnView : Window
    {
        private UpdateColumnViewModel viewModel;
        private BoardView Board;
        public UpdateColumnView(ColumnModel column, int columnOrdinal,BoardView board)
        {
            InitializeComponent();
            viewModel = new UpdateColumnViewModel(column.Controller,column,columnOrdinal);
            this.DataContext = viewModel;
            this.Board = board;
        }
        /// <summary>
        /// tries to move the selected column to the right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveColumnRight_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.MoveColumnRight(Board.ViewModel) != "")
            {
                this.Close();
            }
        }

        /// <summary>
        /// tries to move the selected column to the left
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveColumnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.MoveColumnLeft(Board.ViewModel) != "")
            {
                this.Close();
            }
        }
        /// <summary>
        /// updates the name of the selected column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateName_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateName();
        }
        /// <summary>
        /// updates the limit of the selected column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateLimit_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateLimit();
        }
        /// <summary>
        /// returns to the board window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
