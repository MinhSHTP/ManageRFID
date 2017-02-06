using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class InDetailWindow : Window
    {
        public InDetailWindow()
        {
            InitializeComponent();
        }

        private void InDetailWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int i = MainWindow._mainWindow.thongkexetrongbaiDataGridView.SelectedIndex;
            AvatarImg.Source = ConvertByteArrayToBitmapImage(File.ReadAllBytes(@"..//..//Image/huynhngochoangminh.jpg"));
            NameLbl.Content = MainWindow._currentVehicle[MainWindow._mainWindow.thongkexetrongbaiDataGridView.SelectedIndex]._Hoten;
            TypeVehicleLbl.Content = MainWindow._currentVehicle[MainWindow._mainWindow.thongkexetrongbaiDataGridView.SelectedIndex]._Loaixe;
            LicensePlateLbl.Content = MainWindow._currentVehicle[MainWindow._mainWindow.thongkexetrongbaiDataGridView.SelectedIndex]._Biensoxe;
            DateLbl.Content = MainWindow._currentVehicle[MainWindow._mainWindow.thongkexetrongbaiDataGridView.SelectedIndex]._Ngayvao;
            TimeLbl.Content = MainWindow._currentVehicle[MainWindow._mainWindow.thongkexetrongbaiDataGridView.SelectedIndex]._Giovao;
        }

        public static BitmapImage ConvertByteArrayToBitmapImage(Byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
