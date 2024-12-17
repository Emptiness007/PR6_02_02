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

namespace RegIN_Filimonova.Elements
{
    /// <summary>
    /// Логика взаимодействия для ElementCapture.xaml
    /// </summary>
    public partial class ElementCapture : UserControl
    {
        public Action HandlerCorrectCapture;
        private string strCapture = "";
        private static readonly int elementWidth = 280;
        private static readonly int elementHeight = 50;
        private Random random = new Random();

        public ElementCapture()
        {
            InitializeComponent();
            CreateCapture();
        }

        public void CreateCapture()
        {
            InputCapture.Text = "";
            Capture.Children.Clear();
            strCapture = "";
            CreateLabels(100, 10, 10, true);  
            CreateLabels(4, 30, 100, false);
        }

        private void CreateLabels(int count, int baseFontSize, int alpha, bool isBackground)
        {
            for (int i = 0; i < count; i++)
            {
                int back = random.Next(0, 10);
                var label = new Label
                {
                    Content = back,
                    FontSize = isBackground ? random.Next(10, 16) : baseFontSize,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromArgb((byte)(isBackground ? 100 : 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255))),
                    Margin = new Thickness(isBackground ? random.Next(0, elementWidth - 20) : elementWidth / 2 - 60 + i * 30, isBackground ? random.Next(0, elementHeight - 20) : random.Next(-10, 10), 0, 0)
                };

                if (!isBackground)
                {
                    strCapture += back.ToString();
                }
                Capture.Children.Add(label);
            }
        }

        public bool OnCapture()
        {
            return strCapture == InputCapture.Text;
        }

        private void EnterCapture(object sender, KeyEventArgs e)
        {
            if (InputCapture.Text.Length == 4)
            {
                if (!OnCapture())
                    CreateCapture();
                else
                    HandlerCorrectCapture?.Invoke();
            }
        }
    }
}
