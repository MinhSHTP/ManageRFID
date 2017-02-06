using System;
using System.Windows;
using System.Windows.Controls;
using System.Management;
using System.Collections.Generic;
using RFID_SHTP.Helpers;
using RFID_SHTP.Objects;
using AForge.Video.DirectShow;
using System.IO;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Video;
using System.Reflection;
using System.Windows.Media;
using System.Linq;

namespace RFID_SHTP.UI
{
    /// <summary>
    /// Interaction logic for SettingWindows.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        GetListCamerasHelper _getListCamerasHelper = new GetListCamerasHelper();

        FilterInfoCollection _videoDevices, _lastVdeoDevices;
        string _lastDevice1, _lastDevice2;
        public static VideoCaptureDevice _videoSource1, _videoSource2;

        public SettingWindow()
        {
            InitializeComponent();
        }

        private void _videoSource1_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            getNewFrame(MainWindow._cam1, eventArgs);
        }

        void getNewFrame(System.Windows.Controls.Image camera, NewFrameEventArgs eventArgs)
        {
            try
            {
                System.Drawing.Image img = (Bitmap)eventArgs.Frame.Clone();
                img.RotateFlip(RotateFlipType.Rotate180FlipY);

                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage screenMain = new BitmapImage();
                screenMain.BeginInit();
                screenMain.StreamSource = ms;
                screenMain.EndInit();

                screenMain.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    BitmapImage screenTransfer = new BitmapImage();
                    screenTransfer.BeginInit();
                    screenTransfer.UriSource = new Uri("pack://application:,,,/RFID_SHTP;component/Image/black_background.jpg");
                    screenTransfer.EndInit();
                    camera.Source = screenTransfer;
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        camera.Source = screenMain;
                    }));
                }));
            }
            catch (Exception ex)
            {
            }
        }

        private void _videoSource2_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            getNewFrame(MainWindow._cam2, eventArgs);
        }

        private void DevicesList2_SelectonChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((DevicesList2.SelectedIndex == DevicesList1.SelectedIndex) && (DevicesList2.SelectedIndex != 0))
            {
                MessageBox.Show("Camera không hỗ trợ đa luồng - Camera 2 phải khác Camera 1", "Lỗi hiển thị camera", MessageBoxButton.OK);
                DevicesList2.SelectedIndex = 0;
            }
        }

        private void DevicesList1_SelectonChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((DevicesList2.SelectedIndex == DevicesList1.SelectedIndex) && (DevicesList1.SelectedIndex != 0))
            {
                MessageBox.Show("Camera không hỗ trợ đa luồng - Camera 1 phải khác Camera 2", "Lỗi hiển thị camera", MessageBoxButton.OK);
                DevicesList1.SelectedIndex = 0;
            }
        }

        private void StartCameraBtn_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage screenTransfer = new BitmapImage();
            screenTransfer.BeginInit();
            screenTransfer.UriSource = new Uri("pack://application:,,,/RFID_SHTP;component/Image/black_background.jpg");
            screenTransfer.EndInit();
            //if (DevicesList1.SelectedIndex != DevicesList2.SelectedIndex)
            //{
            if (DevicesList1.SelectedIndex != 0)
            {
                if (_videoSource1 != null)
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        MainWindow._cam1.Source = screenTransfer;
                    }));
                    _videoSource1.Stop();
                }
                _videoSource1 = new VideoCaptureDevice(_videoDevices[DevicesList1.SelectedIndex - 1].MonikerString);
                _lastDevice1 = _videoDevices[DevicesList1.SelectedIndex - 1].Name;
                _videoSource1.DesiredFrameRate = 10;
                _videoSource1.NewFrame += _videoSource1_NewFrame;
                _videoSource1.Start();
            }
            else
            {
                
                if (_videoSource1 != null)
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        MainWindow._cam1.Source = screenTransfer;

                    }));
                    _videoSource1.Stop();
                }
               
            }


            if (DevicesList2.SelectedIndex != 0)
            {
                if (_videoSource2 != null)
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        MainWindow._cam2.Source = screenTransfer;
                    }));
                    _videoSource2.Stop();
                }
                _videoSource2 = new VideoCaptureDevice(_videoDevices[DevicesList2.SelectedIndex - 1].MonikerString);
                _lastDevice2 = _videoDevices[DevicesList2.SelectedIndex - 1].Name;
                _videoSource2.DesiredFrameRate = 10;
                _videoSource2.NewFrame += _videoSource2_NewFrame;
                _videoSource2.Start();
            }
            else
            {
                if (_videoSource2 != null)
                {
                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                    MainWindow._cam2.Source = screenTransfer;
                    }));
                    _videoSource2.Stop();
                }
                
            }
            MainWindow._mainWindow.storeVideoCaptureDevice(_lastDevice1, _lastDevice2);
            //}
            //else
            //{
            //MessageBox.Show("Camera không hỗ trợ đa luồng - Camera 1 phải khác Camera 2", "Lỗi hiển thị camera", MessageBoxButton.OK);
            //}

        }

        private void SettingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DevicesList1.Items.Add("");
            DevicesList2.Items.Add("");
            DevicesList1.SelectedIndex = 0;
            DevicesList2.SelectedIndex = 0;
            _videoDevices = _getListCamerasHelper.getListCameras();
            _lastDevice1 = MainWindow._mainWindow.returnLastVideoCaptureDevice1();
            _lastDevice2 = MainWindow._mainWindow.returnLastVideoCaptureDevice2();
            for (int i = 0; i < _videoDevices.Count; i++)
            {
                string cameraName = "[" + (i + 1) + "] : " + _videoDevices[i].Name;
                if (_videoDevices[i].Name.Equals(_lastDevice1))
                {
                    DevicesList1.SelectedIndex = i;
                }
                if (_videoDevices[i].Name.Equals(_lastDevice2))
                {
                    DevicesList2.SelectedIndex = i;
                }
                DevicesList1.Items.Add(cameraName);
                DevicesList2.Items.Add(cameraName);
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
