﻿using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using RFID_SHTP.ConnectDatabase.BAL;
using RFID_SHTP.ConnectDatabase.BEL;
using RFID_SHTP.Objects;
using RFID_SHTP.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static RFID_SHTP.Helpers.ReadCardHelper;

namespace RFID_SHTP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow _mainWindow;
        public static SerialClient _serial;
        public static Image _cam1, _cam2;
        string _lastDevice1, _lastDevice2;
        public static string _IDCard;
        int i = 1;
        int index = 0;
        List<MyObject> MyList = new List<MyObject>();

        public static List<InfoInObj> _currentVehicle = new List<InfoInObj>();

        Operations _operation = new Operations();
        InfoInObj _infoCurrent = new InfoInObj();

        public System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public static System.Windows.Threading.DispatcherTimer getIDCardTimer = new System.Windows.Threading.DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            //ReportLbl.Content = "Thống kê ra vào trong ngày " + DateTime.Now.ToString("dd/MM/yyyy");

            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();


            _mainWindow = this;
            _cam1 = videoSourcePlayer1;
            _cam2 = videoSourcePlayer2;

            //DataTable bindingDataTable = _operation.xenhanvien();
            //DataGridView.DataContext = bindingDataTable.DefaultView;
            //BitmapImage logo = new BitmapImage();
            //logo.BeginInit();
            //logo.UriSource = new Uri("pack://application:,,,/RFID_SHTP;component/Image/black_background.jpg");
            //logo.EndInit();
            //_cam1.Source = _cam2.Source = logo;
            //DisconnectBtn.IsEnabled = false;
            //DeviceNameLbl.Content = "Chưa kết nối với thiết bị nào";
            //ManagerTab.Model = Model;
        }

        public void GetIDCard(object sender, DataStreamEventArgs e)
        {
            _IDCard = System.Text.Encoding.UTF8.GetString(e.Response);
            //MessageBox.Show(""+_IDCard, "");
        }

        public void GetIDCardTimer_Tick(object sender, EventArgs e)
        {
            //TimeSystemLbl.Content = _IDCard;      
            try
            {
                _currentVehicle = _infoCurrent.getListCurrentVehicle();
                if (_currentVehicle.Count == 0)
                {
                    Random rand = new Random();
                    byte[] camImg1 = ConvertImgToByte(TakeSnapshot2(videoSourcePlayer1, ImgOut1));
                    byte[] camImg2 = ConvertImgToByte(TakeSnapshot2(videoSourcePlayer2, ImgOut2));
                    string[] plate1 = new[] { "11", "12", "13", "14", "15", "16", "17", "18" };
                    string[] plate2 = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };
                    string licensePlate = "" + plate1[rand.Next(0, plate1.Length)] + plate2[rand.Next(0, plate2.Length)];
                    _operation.CarIN(_IDCard, licensePlate, camImg1, camImg2);
                }
                else
                {
                    //for (int i = 0; i < _currentVehicle.Count; i++)
                    //{
                    if (_IDCard == null)
                    {

                    }
                    else
                    {
                        if (checkIsHave(_IDCard))//vehicle having in park >>> vehicle go out park
                        {
                            MessageBox.Show(""+_IDCard, "");
                            //MessageBox.Show("" + _currentVehicle[i]._Mathe + " " + _currentVehicle[i]._Hoten + " " + _currentVehicle[i]._Giovao + " " + _currentVehicle[i]._Ngayvao, "");
                            byte[] camImg1 = ConvertImgToByte(TakeSnapshot2(videoSourcePlayer1, ImgOut1));
                            byte[] camImg2 = ConvertImgToByte(TakeSnapshot2(videoSourcePlayer2, ImgOut2));
                            _operation.CarOut2(_IDCard, Guid.NewGuid(), camImg1, camImg2);//can not write in card. 
                            _IDCard = null;
                        }
                        else//vehicle go in park
                        {
                            Random rand = new Random();
                            byte[] camImg1 = ConvertImgToByte(TakeSnapshot2(videoSourcePlayer1, ImgOut1));
                            byte[] camImg2 = ConvertImgToByte(TakeSnapshot2(videoSourcePlayer2, ImgOut2));
                            string[] plate1 = new[] { "11", "12", "13", "14", "15", "16", "17", "18" };
                            string[] plate2 = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };
                            string licensePlate = "" + plate1[rand.Next(0, plate1.Length)] + plate2[rand.Next(0, plate2.Length)];
                            _operation.CarIN(_IDCard, licensePlate, camImg1, camImg2);
                            _IDCard = null;
                        }
                    }

                    //}
                }

            }
            catch
            {
                //MessageBox.Show("", "sdasdsd");
                _IDCard = null;
            }

        }

        public bool checkIsHave(string idCardCheck)
        {
            bool flagIsHave = false;
            for (int i = 0; i < _currentVehicle.Count; i++)
            {
                if (_currentVehicle[i]._Mathe.Equals(idCardCheck))
                {
                    flagIsHave = true;
                    break;
                }
            }
            return flagIsHave;
        }

        public byte[] ConvertImgToByte(ImageSource imageSource)
        {
            var image = imageSource as BitmapSource;
            byte[] data;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimerSystem.Content = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            //DateTime time1 = DateTime.Now;
            //DateTime time2 = new DateTime(2017, 1, 17, 8, 53, 50);
            //TimeSpan count = time1 - time2;
            //double result = count.TotalSeconds;

            //if (result > 86400)
            //{
            //    //MessageBox.Show("Vào được " + (int)count.TotalDays + " ngày trước", "");
            //    //TimeSystemLbl.Content = "Vào được " + (int)count.TotalDays + " ngày trước";
            //}
            //else if ((result > 3600) && (result < 86400))
            //{
            //    //MessageBox.Show("Vào được " + (int)count.TotalHours + " tiếng trước", "");
            //    //TimeSystemLbl.Content = "Vào được " + (int)count.TotalHours + " tiếng trước";
            //}
            //else if ((result > 60) && (result < 3600))
            //{
            //    //MessageBox.Show("Vào được " + (int)count.TotalMinutes + " phút trước", "");
            //    //TimeSystemLbl.Content = "Vào được " + (int)count.TotalMinutes + " phút trước";
            //}
            //else if (result < 60)
            //{
            //    //MessageBox.Show("Vào được " + (int)count.TotalSeconds + " giây trước", "");
            //    //TimeSystemLbl.Content = "Vào được " + (int)count.TotalSeconds + " giây trước";
            //}
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            MessageBoxResult result = showMessageBox("Thoát", "Bạn có chắc  muốn thoát không?", MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                _mainWindow = null;
                Application.Current.Shutdown(0);
                Environment.Exit(-1);
            }
            else if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }

        }

        private void SelectionTabItemChange(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (thongtinravaoTabItem.IsSelected)
                {
                    InvokeGuiThread(thongtinravaoTabItem_Loaded);
                    //InvokeGuiThread(() =>
                    //{
                    //    //    //MessageBox.Show("a", "Tab1");
                    //    thongtinravaoTabItem_Loaded();
                    //});
                }
                else if (thongkexetrongbaiTabItem.IsSelected)
                {
                    InvokeGuiThread(thongkexetrongbaiTabItem_Loaded);
                }
                else if (thongkexeraTabItem.IsSelected)
                {
                    InvokeGuiThread(thongkexeraTabItem_Loaded);
                }
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

        ImageSource TakeSnapshot2(Image videoSourcePlayer, Image imgOut)
        {
            imgOut.Source = videoSourcePlayer.Source;
            //string fileName = "C://Users//minhh//Desktop//abc";
            //if (!File.Exists(fileName))
            //{
            //    SaveToJpg(videoSourcePlayer, fileName);
            //}
            //else
            //{
            //    fileName += "(" + i + ")";
            //    i++;
            //    SaveToJpg(videoSourcePlayer, fileName);
            //}
            //BitmapImage logo = new BitmapImage();
            //logo.BeginInit();
            //logo.UriSource = new Uri(fileName);
            //logo.EndInit();
            //imgOut.Source = logo;
            return imgOut.Source;
        }

        private void setOnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                ((DataGridTextColumn)e.Column).Binding.StringFormat = "dd/MM/yyyy";
            }
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

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void SearchByDateInOutMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchByLicensePlateMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingReaderDeviceMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingReaderDeviceWindow setReadDevWindow = new SettingReaderDeviceWindow();
            setReadDevWindow.Show();
        }



        public string returnLastVideoCaptureDevice1()
        {
            return _lastDevice1;
        }

        private void thongtinravaoTabItem_Loaded()
        {
            MessageBox.Show("1", "Tab1");
            //while (index < 100)
            //{
            //    MyObject newObj = new MyObject();
            //    newObj.ID = index.ToString() + " row";
            //    newObj.Date = index.ToString() + " row";
            //    MyList.Add(newObj);
            //    index++;
            //}
            //thongkexetrongbaiDataGridView.ItemsSource = MyList;
            //thongkexetrongbaiCountVehicleLbl.Content = thongkexetrongbaiDataGridView.Items.Count;
        }

        private void thongkexetrongbaiTabItem_Loaded()
        {
            MessageBox.Show("2", "Tab2");
            _currentVehicle = _infoCurrent.getListCurrentVehicle();
            thongkexetrongbaiDataGridView.ItemsSource = _currentVehicle;
            //DataTable bindingDataTable = _operation.dataCurrent();
            //thongkexetrongbaiDataGridView.DataContext = bindingDataTable.DefaultView;
            //thongkexetrongbaiDataGridView.Columns[0].Header = "Họ và tên";
            //thongkexetrongbaiDataGridView.Columns[1].Header = "Giờ vào";
            //thongkexetrongbaiDataGridView.Columns[2].Header = "Ngày vào";
            //thongkexetrongbaiDataGridView.Columns[3].Header = "Biển số xe";
            //while (index < 100)
            //{
            //    MyObject newObj = new MyObject();
            //    newObj.ID = index.ToString() + " row";
            //    newObj.Date = index.ToString() + " row";
            //    MyList.Add(newObj);
            //    index++;
            //}
            //thongkexetrongbaiDataGridView.ItemsSource = MyList;
            thongkexetrongbaiCountVehicleLbl.Content = thongkexetrongbaiDataGridView.Items.Count;


        }

        private void thongkexeraTabItem_Loaded()
        {
            MessageBox.Show("3", "Tab3");
        }

        private void thongkexeraDataGridViewMouseDouble_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("" + thongkexeraDataGridView.SelectedIndex, "Index");
            if (thongkexeraDataGridView.SelectedIndex != 1)
            {
                OutDetailWindow detailWindow = new OutDetailWindow();
                detailWindow.Show();
            }

        }

        private void thongkexetrongbaiDataGridViewMouseDouble_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("" + thongkexetrongbaiDataGridView.SelectedIndex, "Index");
            if (thongkexetrongbaiDataGridView.SelectedIndex != -1)
            {
                InDetailWindow detailWindow = new InDetailWindow();
                detailWindow.Show();
            }

        }

        private void DateFilterPicker_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StaffListMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InfoStaffWindow infoStaffWindow = new InfoStaffWindow();
            infoStaffWindow.Show();
        }

        public MessageBoxResult showMessageBox(string titleMsgBox, string contentMsgBox, MessageBoxImage msgImage)
        {
            MessageBoxResult result = MessageBox.Show(contentMsgBox, titleMsgBox, MessageBoxButton.YesNo, msgImage);

            return result;
        }

        public string returnLastVideoCaptureDevice2()
        {
            return _lastDevice2;
        }
    }
}
