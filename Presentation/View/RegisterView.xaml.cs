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
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        RegisterViewModel viewModel;
        MainWindow mw;
        public RegisterView(BackendController controller, MainWindow mw)
        {
            InitializeComponent();
            this.viewModel = new RegisterViewModel(controller);
            this.DataContext = viewModel;
            this.mw = mw;
        }
        /// <summary>
        /// tries to register and if succesfully closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Register() == "")
            {
                mw.ViewModel.Message = "Successfully registered";
                this.Close();
            }
        }
    }
}
