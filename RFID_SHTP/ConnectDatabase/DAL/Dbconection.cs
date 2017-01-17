using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID_SHTP.ConnectDatabase.DAL
{
    public class Dbconection
    {
        byte[] img = File.ReadAllBytes(@"..//..//Image//phamtruongan.jpg");
        public SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-4KLT09L\SQLEXPRESS;Initial Catalog = DATABASE; User ID = sa; Password=Kimthu@17293");
        //public SqlConnection con = new SqlConnection(@"Data Source=HOST002\SEVER04;Initial Catalog=DATABASE;User ID=SQLTest;Password=12345");
        public SqlConnection getcon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }

        public int ExeNonQuery(SqlCommand cmd)
        {
            cmd.Connection = getcon();
            int rowsaffected = -1;
            rowsaffected = cmd.ExecuteNonQuery();
            con.Close();
            return rowsaffected;
        }

        public object ExeScaler(SqlCommand cmd)
        {
            cmd.Connection = getcon();
            object obj = -1;
            obj = cmd.ExecuteScalar();
            con.Close();
            return obj;
        }
        public DataTable ExeReader(SqlCommand cmd)
        {
            cmd.Connection = getcon();
            SqlDataReader sdr;
            DataTable dt = new DataTable();
            sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }
        public void SendImage()
        {

            string sendImage = "INSERT into Items (ItemID,ItemNumber,ItemDescription,ItemImage) VALUES (@ItemID,@ItemNumber,@ItemDescription,@ItemImage) ";
            SqlCommand command = new SqlCommand(sendImage);
            command.Connection = getcon();
            command.Parameters.AddWithValue("@ItemID", Guid.NewGuid());
            command.Parameters.AddWithValue("@ItemNumber", SqlDbType.VarChar).Value = "fese";
            command.Parameters.AddWithValue("@ItemDescription", SqlDbType.VarChar).Value = "test";
            command.Parameters.AddWithValue("@ItemImage", SqlDbType.VarBinary).Value = img;
            command.ExecuteNonQuery();


        }

        public byte[] getDocument()
        {
            SqlConnection cn = getcon();
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandText = @"
            SELECT ItemImage
            FROM   Items
            WHERE  ItemID = @Id";
                cm.Parameters.AddWithValue("@Id", "bf27e531-0717-4d0a-9ca0-07c43214f60b");

                return cm.ExecuteScalar() as byte[];
            }
        }
        //Data Source = DESKTOP - ENEEJRL; Initial Catalog = RFID_SMART; Integrated Security = True
    }
}
