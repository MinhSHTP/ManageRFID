using RFID_SHTP.ConnectDatabase.BAL;
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
    /// Interaction logic for InfoStaffWindow.xaml
    /// </summary>
    public partial class InfoStaffWindow : Window
    {
        Operations _operation = new Operations();
        public InfoStaffWindow()
        {
            InitializeComponent();


        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void danhsachnhanvienDataGridViewSelection_Changed(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("" + danhsachnhanvienDataGridView.Columns.Count, "=))");
        }

        private void InfoStaffWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //DataGridTextColumn textColumn = new DataGridTextColumn();
            //textColumn.Header = "STT";
            //danhsachnhanvienDataGridView.Columns.Add(textColumn);//add a column into datagrid
            danhsachnhanvienDataGridView.DataContext = _operation.xenhanvien().DefaultView;

        }

        private void danhsachnhanvienDataGridView_GenerateColumns(object sender, EventArgs e)
        {
            //Header of columns
            danhsachnhanvienDataGridView.Columns[0].Header = "Họ tên nhân viên";
            danhsachnhanvienDataGridView.Columns[1].Header = "Loại xe đăng ký";
            danhsachnhanvienDataGridView.Columns[2].Header = "Biển số xe";

            //Width of columns
            danhsachnhanvienDataGridView.Columns[0].Width = 250;
            danhsachnhanvienDataGridView.Columns[1].Width = 100;
            danhsachnhanvienDataGridView.Columns[2].Width = 100;

        }

        private void danhsachnhanvienDataGridView_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();//Auto show number row
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AvatarImg_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("Hi!!","=))))");
            ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }
    }
}
