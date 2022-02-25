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
    public class ColumnEventArgs : EventArgs
    {
        private ColumnModel column;
        public ColumnModel Column
        {
            get => column;
            set
            {
                column = value;
            }
        }
        private int id;
        public int ID
        {
            get => id;
            set
            {
                id = value;
            }
        }

        public bool IsMovedRight { get => isMovedRight; set => isMovedRight = value; }
        public bool IsMovedLeft { get => isMovedLeft; set => isMovedLeft = value; }

        private bool isMovedRight=false;
        private bool isMovedLeft=false;
    }
    /// <summary>
    /// Interaction logic for AddColumnView.xaml
    /// </summary>
    public partial class AddColumnView : Window
    {
        private AddColumnViewModel viewModel;
        public delegate void DataChangedEventHandler(object sender, ColumnEventArgs e);

        public event DataChangedEventHandler DataChanged;
        public AddColumnView(UserModel user)
        {
            InitializeComponent();
            viewModel = new AddColumnViewModel(user);
            this.DataContext = viewModel;
        }
        /// <summary>
        /// trigger to the functions that subscribed to the event
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnAddColumnView(ColumnEventArgs args)
        {
            DataChanged?.Invoke(this, args);
        }
        /// <summary>
        /// tries to add a column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ColumnModel addedCol = viewModel.AddColumn();
            if(addedCol != null)
            {
                ColumnEventArgs args = new ColumnEventArgs();
                args.Column = addedCol;
                args.ID = viewModel.Id;
                OnAddColumnView(args);
                this.Close();
            }
        }
    }
}
