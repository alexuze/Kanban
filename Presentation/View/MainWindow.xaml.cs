using Presentation.Model;
using Presentation.View;
using Presentation.ViewModel;
using System.Windows;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel;

        internal MainViewModel ViewModel { get => viewModel;}

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            this.DataContext = viewModel;
        }
        /// <summary>
        /// creates a new register window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterView register = new RegisterView(viewModel.Controller,this);
            register.ShowDialog();
        }
        /// <summary>
        /// tries to login and if succesfully opens up the board window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            BoardView u = viewModel.Login();
            if (u != null)
            {
                
                u.Show();
                this.Close();
            }
        }
    }
}
