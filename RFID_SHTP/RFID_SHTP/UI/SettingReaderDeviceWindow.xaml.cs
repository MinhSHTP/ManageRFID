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
    /// Interaction logic for SettingReaderDeviceWindow.xaml
    /// </summary>
    public partial class SettingReaderDeviceWindow : Window
    {
        public SettingReaderDeviceWindow()
        {
            InitializeComponent();
        }

        private void OpenPortBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
