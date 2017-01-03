using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using RFID_SHTP.Objects;
using RFID_SHTP.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RFID_SHTP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow _mainWindow;
        public static Image _cam1, _cam2;
        string _lastDevice1, _lastDevice2;
        int i = 1;
        int index = 0;
        List<MyObject> MyList = new List<MyObject>();
        public MainWindow()
        {
            InitializeComponent();
            ReportLbl.Content = "Thống kê ra vào trong ngày " + DateTime.Now.ToString("dd/MM/yyyy");
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            _mainWindow = this;
            _cam1 = videoSourcePlayer1;
            _cam2 = videoSourcePlayer2;
            while(index<100)
            {
                MyObject newObj = new MyObject();
                newObj.ID = index.ToString();
                newObj.Date = index.ToString();
                MyList.Add(newObj);
                index++;
            }
            DataGridView.ItemsSource = MyList;
            //BitmapImage logo = new BitmapImage();
            //logo.BeginInit();
            //logo.UriSource = new Uri("pack://application:,,,/RFID_SHTP;component/Image/black_background.jpg");
            //logo.EndInit();
            //_cam1.Source = _cam2.Source = logo;
            //DisconnectBtn.IsEnabled = false;
            //DeviceNameLbl.Content = "Chưa kết nối với thiết bị nào";
            //ManagerTab.Model = Model;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimerSystem.Content = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Model.Dispose();

            _mainWindow = null;

            Application.Current.Shutdown(0);

            Environment.Exit(-1);
        }

        private void SelectionTabItemChange(object sender, SelectionChangedEventArgs e)
        {
            if (thongtinravaoTabItem.IsSelected)
            {
                InvokeGuiThread(() =>
                {
                    //MessageBox.Show("a", "Tab1");

                });
            }
            else if (thongketrongngayTabItem.IsSelected)
            {
                InvokeGuiThread(() =>
                {
                    //MessageBox.Show("aa","Tab2");
                });
            }
        }

        void SaveToJpg(FrameworkElement visual, string fileName)
        {
            var encoder = new JpegBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            BitmapFrame frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);
            var stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            encoder.Save(stream);

            stream.Flush();
            stream.Close();
            stream.Dispose();
        }

        void TakeSnapshot(Image videoSourcePlayer, Image imgOut)
        {
            string fileName = "C://Users//minhh//Desktop//abc";
            if (!File.Exists(fileName))
            {
                SaveToJpg(videoSourcePlayer, fileName);
            }
            else
            {
                fileName += "(" + i + ")";
                i++;
                SaveToJpg(videoSourcePlayer, fileName);
            }
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(fileName);
            logo.EndInit();
            imgOut.Source = logo;
        }

        private void InvokeGuiThread(Action action)
        {
            Dispatcher.BeginInvoke(action);
        }

        private void RegisterMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SettingCameraMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow frmSettingWindows = new SettingWindow();
            frmSettingWindows.Show();
        }

        private void ReportMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DetailWindow detailWindow = new DetailWindow();
            detailWindow.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void storeVideoCaptureDevice(string device1, string device2)
        {
            _lastDevice1 = device1;
            _lastDevice2 = device2;
        }

        private void SnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            TakeSnapshot(videoSourcePlayer1, ImgOut1);
            TakeSnapshot(videoSourcePlayer2, ImgOut2);
        }

        private void Form_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SettingDatabaseMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Imported_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void DataGridViewMouseDouble_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("" + DataGridView.SelectedIndex, "Test");
        }

        public string returnLastVideoCaptureDevice1()
        {
            return _lastDevice1;
        }

        public string returnLastVideoCaptureDevice2()
        {
            return _lastDevice2;
        }
    }
}
