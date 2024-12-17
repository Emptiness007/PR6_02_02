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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegIN_Filimonova.Pages
{
    /// <summary>
    /// Логика взаимодействия для PinCode.xaml
    /// </summary>
    public partial class PinCode : Page
    {
        public PinCode()
        {
            InitializeComponent();
        }

        private void ConfirmPin_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TbPinCode.Text, out int pin) && TbPinCode.Text.Length == 4)
            {
                MainWindow.mainWindow.UserLogIn.PinCode = TbPinCode.Text;
                MainWindow.mainWindow.UserLogIn.UpdatePinCode();
                MessageBox.Show("The PIN code has been successfully installed!");
                MainWindow.mainWindow.OpenPage(new Login());
            }
            else
            {
                MessageBox.Show("Enter the correct 4-digit PIN code.");
            }
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }

    }
}
