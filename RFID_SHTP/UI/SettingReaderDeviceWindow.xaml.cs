using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static RFID_SHTP.Helpers.ReadCardHelper;

namespace RFID_SHTP.UI
{
    /// <summary>
    /// Interaction logic for SettingReaderDeviceWindow.xaml
    /// </summary>
    public partial class SettingReaderDeviceWindow : Window
    {
        SerialClient _serial;
        string _idCard;
        System.Windows.Threading.DispatcherTimer getIDCardTimer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Background);
        public SettingReaderDeviceWindow()
        {
            InitializeComponent();
            PortCmb.ItemsSource = GetAllPorts();
            BaudRateCmb.Items.Add("9600");
            BaudRateCmb.Items.Add("115200");
        }

        public List<string> GetAllPorts()
        {
            List<string> allPorts = new List<string>();
            foreach (string portName in SerialPort.GetPortNames())
            {
                allPorts.Add(portName);
            }
            return allPorts;
        }

        void receiveHandler(object sender, DataStreamEventArgs e)
        {
            _idCard = System.Text.Encoding.UTF8.GetString(e.Response);
        }

        public void GetIDCardTimer_Tick(object sender, EventArgs e)
        {
            //StatusLbl.Content = _idCard;
        }

        private void OpenPortBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(PortCmb.Text=="")
                {
                    StatusLbl.Content = "Chưa chọn thiết bị";
                }
                else if (BaudRateCmb.Text == "")
                {
                    StatusLbl.Content = "Chưa chọn tần số thẻ";
                }
                else
                {
                    string SelectPort = PortCmb.Text;
                    int BaudRate = Convert.ToInt32(BaudRateCmb.Text);
                    _serial = new SerialClient(SelectPort, BaudRate);

                    MainWindow._serial = _serial;
                    MainWindow._serial.OpenConn(SelectPort, BaudRate);
                    MainWindow._serial.OnReceiving += new EventHandler<DataStreamEventArgs>(MainWindow._mainWindow.GetIDCard);

                    MainWindow.getIDCardTimer.Tick += new EventHandler(MainWindow._mainWindow.GetIDCardTimer_Tick);
                    MainWindow.getIDCardTimer.Interval = new TimeSpan(0, 0, 0, 1);
                    MainWindow.getIDCardTimer.Start();

                    //_serial.OnReceiving += new EventHandler<DataStreamEventArgs>(receiveHandler);
                    StatusLbl.Content = "Đã kết nối";
                    ConnectProgBar.Value = 100;
                    OpenPortBtn.IsEnabled = false;
                    ClosePortBtn.IsEnabled = true;
                    //if (!_serial.OpenConn())
                    //{
                    //    System.Windows.MessageBox.Show("The Port Cannot Be Opened", "Serial Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Không thể kết nối đầu đọc thẻ", "Lỗi kết nối đầu đọc");
                ConnectProgBar.Value = 0;
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        public void InitializedForm()
        {
            if (MainWindow._serial != null)
            {
                _serial = MainWindow._serial;
                for (int i = 0; i < PortCmb.Items.Count; i++)
                {
                    if (_serial.Port.Equals(PortCmb.Items[i]))
                    {
                        PortCmb.SelectedIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < BaudRateCmb.Items.Count; i++)
                {
                    if (_serial.BaudRate.ToString().Equals(BaudRateCmb.Items[i]))
                    {
                        BaudRateCmb.SelectedIndex = i;
                        break;
                    }
                }
                ClosePortBtn.IsEnabled = true;
                OpenPortBtn.IsEnabled = false;
                ConnectProgBar.Value = 100;
                StatusLbl.Content = "Đã kết nối";
            }
            else
            {
                if(PortCmb.Items.Count==0)
                {
                    System.Windows.MessageBox.Show("Hiện không có thiết bị nào kết nối", "Không tìm thấy thiết bị");
                }
                ClosePortBtn.IsEnabled = false;
                OpenPortBtn.IsEnabled = true;
                ConnectProgBar.Value = 0;
                StatusLbl.Content = "Ngắt kết nối";
            }
        }

        private void SettingReaderDeviceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializedForm();
        }

        private void ClosePortBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow._serial.CloseConn();
            _serial.CloseConn();
            ClosePortBtn.IsEnabled = false;
            OpenPortBtn.IsEnabled = true;
            ConnectProgBar.Value = 0;
            StatusLbl.Content = "Ngắt kết nối";
        }
    }
}
