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

namespace DuiDuiDui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnReplacingBooks_Click(object sender, RoutedEventArgs e)
        {
            // open replacing books window
            this.Hide();
            ReplacingBooks rb = new ReplacingBooks();
            rb.Show();
        }

        private void btnIdentifyingAreas_Click(object sender, RoutedEventArgs e)
        {
            // open identifying areas window
            this.Hide();
            IdentifyingAreas ia = new IdentifyingAreas();
            ia.Show();
        }

        private void btnFindingCallNumbers_Click(object sender, RoutedEventArgs e)
        {
            // open finding call numbers window
            this.Hide();
            FindingCallNumbers fcn = new FindingCallNumbers();
            fcn.Show();
        }

        private void btnExit(object sender, RoutedEventArgs e)
        {
            // Close the application
            Application.Current.Shutdown();
        }
    }
}
