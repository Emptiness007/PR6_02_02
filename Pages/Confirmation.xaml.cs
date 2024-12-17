using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для Confirmation.xaml
    /// </summary>
    public partial class Confirmation : Page
    {
        public enum TypeConfirmation
        {
            Login, Regin
        }
        TypeConfirmation  thisTypeConfirmation;
        public int Code = 0;
        public Confirmation(TypeConfirmation typeConfirmation)
        {
            InitializeComponent();
            thisTypeConfirmation = typeConfirmation;
            SendMailCode();
        }
        public void SendMailCode()
        {
            Code = new Random().Next(100000, 999999);
            Classes.SendMail.SendMessage($"Login code {Code}", MainWindow.mainWindow.UserLogIn.Login);
            Thread TSendMailCode = new Thread(TimerSendMailCode);
            TSendMailCode.Start();
        }
        public void TimerSendMailCode()
        {
            for (int i = 0; i<60; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    LTimer.Content = $"A second message can be sent after {(60 - i)} seconds";
                });
                Thread.Sleep(1000);
            }
            Dispatcher.Invoke(() =>
            {
                BSendMessage.IsEnabled = true;
                LTimer.Content = "";
            });
        }

        private void SetCode(object sender, RoutedEventArgs e)
        {
            if (TbCode.Text.Length == 6)
                ConfirmCode();
        }

        private void ConfirmCode()
        {
            if (TbCode.Text == Code.ToString() && TbCode.IsEnabled)
            {
                TbCode.IsEnabled = false;
                string message = thisTypeConfirmation == TypeConfirmation.Login
                    ? "Авторизация пользователя успешно подтверждена." : "Регистрация пользователя успешно подтверждена.";

                MessageBox.Show(message);

                if (thisTypeConfirmation == TypeConfirmation.Regin)
                {
                    MainWindow.mainWindow.UserLogIn.SetUser();
                }
                MainWindow.mainWindow.OpenPage(new PinCode());
            }
        }

        private void SendMail(object sender, RoutedEventArgs e)
        {
            SendMailCode();
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}
