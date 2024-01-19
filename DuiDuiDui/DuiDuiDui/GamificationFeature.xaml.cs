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

namespace DuiDuiDui
{
    /// <summary>
    /// Interaction logic for GamificationFeature.xaml
    /// </summary>
    public partial class GamificationFeature : Window
    {
        public GamificationFeature()
        {
            InitializeComponent();

            // load GIF
            Uri gifUri = new Uri("pack://application:,,,/DuiDuiDui;component/img/letsgogif.gif");
            BitmapImage bitmapImage = new BitmapImage(gifUri);
            gif.Source = bitmapImage;
        }

        private void btnExit(object sender, RoutedEventArgs e)
        {
            // Close the application
            Application.Current.Shutdown();
        }
    }
}
