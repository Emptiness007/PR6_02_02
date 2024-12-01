using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aspose.Imaging;

namespace RegIN_Filimonova.Pages
{
    /// <summary>
    /// Логика взаимодействия для Regin.xaml
    /// </summary>
    public partial class Regin : Page
    {
        OpenFileDialog FileDialogImage = new OpenFileDialog();
        bool BCorrectLogin = false;
        bool BCorrectPassword = false;
        bool BCorrectConfirmPassword = false;
        bool BSetImages = false;
        public Regin()
        {
            InitializeComponent();
            MainWindow.mainWindow.UserLogIn.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogIn.HandlerInCorrectLogin += InCorrectLogin;
            FileDialogImage.Filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg";
            FileDialogImage.RestoreDirectory = true;
            FileDialogImage.Title = "Choose a photo for your avatar";
        }
        private void CorrectLogin()
        {
            SetNotification("Login already in use", Brushes.Red);
            BCorrectLogin = false;
        }
        private void InCorrectLogin() =>
            SetNotification("", Brushes.Black);

        private void SelectImage(object sender, MouseButtonEventArgs e)
        {
            if (FileDialogImage.ShowDialog() == true)
            {
                // конвертируем размер изображения
                using (Aspose.Imaging.Image image = Aspose.Imaging.Image.Load(FileDialogImage.FileName))
                {
                    // создаём ширину изображения
                    int NewWidth = 0;
                    // Создаём высоту изображения
                    int NewHeight = 0;
                    // проверяем какая из сторон больше
                    if (image.Width > image.Height)
                    {
                        // Расчитываем новую ширину относительно высоты
                        NewWidth = (int)(image.Width * (256f / image.Height));
                        // Задаём высоту изображения
                        NewHeight = 256;
                    }
                    else
                    {
                        // Задаём ширину изображения
                        NewWidth = 256;
                        // Расчитываем новую высоту относительно высоты
                        NewHeight = (int)(image.Height * (256f / image.Width));
                    }
                    // Изменяем изобружение
                    image.Resize(NewWidth, NewHeight);
                    // Сохраняем изображение
                    image.Save("IUser.jpg");
                }
                // обрезаем изображение
                using (Aspose.Imaging.RasterImage rasterImage = (Aspose.Imaging.RasterImage)Aspose.Imaging.Image.Load("IUser.jpg"))
                {
                    // Перед кадрированием изображение следует кэшировать для лучшей производительности.
                    if (!rasterImage.IsCached)
                    {
                        rasterImage.CacheData();
                    }
                    // Задаём X
                    int X = 0;
                    // Задаём ширину изображения
                    int Width = 256;
                    // Задаём Y
                    int Y = 0;
                    // Задаём высоту изображения
                    int Height = 256;

                    // Если ширина изображения больше чем высота
                    if (rasterImage.Width > rasterImage.Height)
                        // Расчитываем X как середину изображения
                        X = (int)((rasterImage.Width - 256f) / 2);
                    else
                        // Если высота больше
                        // Расчитываем Y как середину
                        Y = (int)((rasterImage.Height - 256f) / 2);

                    // Создайте экземпляр класса Rectangle нужного размера и обрежьте изображение.
                    Aspose.Imaging.Rectangle rectangle = new Aspose.Imaging.Rectangle(X, Y, Width, Height);
                    rasterImage.Crop(rectangle);

                    // Сохраните обрезанное изображение.
                    rasterImage.Save("IUser.jpg");
                }
                // Создаём анимацию старта
                DoubleAnimation StartAnimation = new DoubleAnimation();
                // Указываем значение от которого она выполняется
                StartAnimation.From = 1;
                // Указываем значение до которого она выполняется
                StartAnimation.To = 0;
                // Указываем продолжительность выполнения
                StartAnimation.Duration = TimeSpan.FromSeconds(0.6);
                // Присваиваем событие при конце анимации
                StartAnimation.Completed += delegate
                {
                    // Устанавливаем изображение
                    IUser.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\IUser.jpg"));
                    // Создаём анимацию конца
                    DoubleAnimation EndAnimation = new DoubleAnimation();
                    // Указываем значение от которого она выполняется
                    EndAnimation.From = 0;
                    // Указываем значение до которого она выполняется
                    EndAnimation.To = 1;
                    // Указываем продолжительность выполнения
                    EndAnimation.Duration = TimeSpan.FromSeconds(1.2);
                    // Запускаем анимацию плавной смены на изображении
                    IUser.BeginAnimation(System.Windows.Controls.Image.OpacityProperty, EndAnimation);
                };
                // Запускаем анимацию плавной смены на изображении
                IUser.BeginAnimation(System.Windows.Controls.Image.OpacityProperty, StartAnimation);
                // Запоминаем что изображение указано
                BSetImages = true;
            }
            else
                // Запоминаем что изображение не указано
                BSetImages = false;
        }

        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetLogin();
        }

        private void SetLogin(object sender, RoutedEventArgs e)
        {
            SetLogin();
        }
        public void SetLogin()
        {
            Regex regex = new Regex(@"([a-zA-Z0-9._-]{4,}@[a-zA-Z0-9._-]{2,}\.[a-zA-Z0-9_-]{2,})");
            BCorrectLogin = regex.IsMatch(TbLogin.Text);
            if (regex.IsMatch(TbLogin.Text) == true)
            {
                SetNotification("", Brushes.Black);
                MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);
            }
            else
                SetNotification("Invalid login", Brushes.Red);
            OnRegin();
        }
        

        private void SetName(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsLetter(e.Text, 0));
        }
        #region SetPassword
        private void SetPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetPassword();
        }

        private void SetPassword(object sender, RoutedEventArgs e)
        {
            SetPassword();
        }
        public void SetPassword()
        {
            Regex regex = new Regex(@"(?=.*[0-9])(?=.*[!@#$%^&?*\-_=])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&?*\-_=]{10,}");

            BCorrectPassword = regex.IsMatch(TbPassword.Password);
            if (regex.IsMatch(TbPassword.Password) == true)
            {
                SetNotification("", Brushes.Black);
                if (TbConfirmPassword.Password.Length > 0)
                    ConfirmPassword(true);
                OnRegin();
            }
            else
                SetNotification("Invalid password", Brushes.Red);

        }
        #endregion

        #region SetConfirmPassword
        private void ConfirmPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ConfirmPassword();
        }

        private void ConfirmPassword(object sender, RoutedEventArgs e)
        {
            ConfirmPassword();
        }
        public void ConfirmPassword(bool Pass = false)
        {
            BCorrectConfirmPassword = TbConfirmPassword.Password == TbPassword.Password;
            if (TbConfirmPassword.Password != TbPassword.Password)
                SetNotification("Passwords do not match", Brushes.Red);
            else
            {
                SetNotification("", Brushes.Black);
                if (!Pass)
                    SetPassword();
            }
        }
        #endregion

        void OnRegin()
        {
            if (!BCorrectLogin)
                return;
            if (TbName.Text.Length == 0)
                return;
            if (!BCorrectPassword)
                return;
            if (!BCorrectConfirmPassword)
                return;
            MainWindow.mainWindow.UserLogIn.Login = TbLogin.Text;
            MainWindow.mainWindow.UserLogIn.Password = TbPassword.Password;
            MainWindow.mainWindow.UserLogIn.Name = TbName.Text;
            if (BSetImages)
                MainWindow.mainWindow.UserLogIn.Image = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\IUser.jpg");
            MainWindow.mainWindow.UserLogIn.DateUpdate = DateTime.Now;
            MainWindow.mainWindow.UserLogIn.DateCreate = DateTime.Now;
            MainWindow.mainWindow.OpenPage(new Confirmation(Confirmation.TypeConfirmation.Regin));
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }
    }
}
