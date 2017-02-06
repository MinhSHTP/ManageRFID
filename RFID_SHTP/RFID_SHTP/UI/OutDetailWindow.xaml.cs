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

namespace RFID_SHTP.UI
{
    /// <summary>
    /// Interaction logic for OutDetailWindow.xaml
    /// </summary>
    public partial class OutDetailWindow : Window
    {
        public OutDetailWindow()
        {
            InitializeComponent();
        }

        private void OutDetailWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
