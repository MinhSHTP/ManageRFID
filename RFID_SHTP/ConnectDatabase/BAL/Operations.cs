using RFID_SHTP.ConnectDatabase.BEL;
using RFID_SHTP.ConnectDatabase.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID_SHTP.ConnectDatabase.BAL
{
    public class Operations
    {
        DataTable getTable;
        public Dbconection db = new Dbconection();
        public Informations info = new Informations();

        public Operations() { }

        public void CarIN(string ID, string Licenese, byte[] hinhcam1vao, byte[] hinhcam2vao)
        {
            DateTime localDate = DateTime.Now;
            string location = "en-GB";
            var culture = new CultureInfo(location);
            DateTime time = localDate.Date;



            string senddata = "INSERT into CURRENTVEHICLE (mathe,uniqueID,bienso,giovao,ngayvao,hinhcam1vao,hinhcam2vao) VALUES (@mathe,@uniqueID,@bienso,@giovao,@ngayvao,@hinhcam1vao,@hinhcam2vao) ";
            SqlCommand command = new SqlCommand(senddata);
            command.Parameters.AddWithValue("@mathe", ID);
            command.Parameters.AddWithValue("@uniqueID", Guid.NewGuid());
            command.Parameters.AddWithValue("@bienso", Licenese);

            command.Parameters.AddWithValue("@giovao", DateTime.Now.TimeOfDay);
            command.Parameters.AddWithValue("@ngayvao", DateTime.Now.Date);



            command.Parameters.AddWithValue("@hinhcam1vao", hinhcam1vao);
            command.Parameters.AddWithValue("@hinhcam2vao", hinhcam2vao);

            db.ExeNonQuery(command);

        }

        public void CarOut(string ID, Guid cardcode, byte[] hinhcam1ra, byte[] hinhcam2ra)
        {

            string comparedata = @"SELECT uniqueID FROM CURRENTVEHICLE WHERE mathe =@mathe";
            SqlCommand comand = new SqlCommand(comparedata);
            comand.Parameters.AddWithValue("@mathe", ID);
            Guid severcode = (Guid)db.ExeScaler(comand);
            if (cardcode == severcode)
            {
                comparedata = @"SELECT* FROM CURRENTVEHICLE WHERE mathe = @mathe";
                comand = new SqlCommand(comparedata);
                comand.Parameters.AddWithValue("@mathe", ID);
                DataTable datatoLog = (DataTable)db.ExeScaler(comand);

                comparedata = @"DELETE FROM CURRENTVEHICLE WHERE mathe = @mathe";
                comand = new SqlCommand(comparedata);
                comand.Parameters.AddWithValue("@mathe", ID);
                db.ExeNonQuery(comand);

                comparedata = "INSERT into LOG (mathe,uniqueID,bienso,giovao,ngayvao,giora,ngayra,hinhcam1vao,hinhcam2vao,hinhcam1ra,hinhcam2ra) VALUES (@mathe,@uniqueID,@bienso,@ngayvao,@giovao,@giora,@ngayra,@hinhcam1vao,@hinhcam2vao,@hinhcam1ra,@hinhcam2ra";
                comand = new SqlCommand(comparedata);
                foreach (DataRow row in datatoLog.Rows)
                {
                    comand.Parameters.AddWithValue("@mathe", row["mathe"]);
                    comand.Parameters.AddWithValue("@uniqueID", row["uniqueID"]);
                    comand.Parameters.AddWithValue("@bienso", row["bienso"]);
                    comand.Parameters.AddWithValue("@giovao", row["giovao"]);
                    comand.Parameters.AddWithValue("@ngayvao", row["ngayvao"]);
                    comand.Parameters.AddWithValue("@hinhcam1vao", row["hinhcam1vao"]);
                    comand.Parameters.AddWithValue("@hinhcam2vao", row["hinhcam2vao"]);
                }
                comand.Parameters.AddWithValue("@hinhcam1ra", hinhcam1ra);
                comand.Parameters.AddWithValue("@hinhcam2ra", hinhcam2ra);
                comand.Parameters.AddWithValue("@ngayra", DateTime.Now.TimeOfDay);
                comand.Parameters.AddWithValue("@ngayra", DateTime.Now.Date);
                db.ExeNonQuery(comand);
            }


        }

        public DataTable dataLog()
        {
            string getdatalog = @"SELECT* FROM LOG";
            return (DataTable)db.ExeReader(new SqlCommand(getdatalog));

        }


        public DataTable dataCurrent()
        {
            string getdatalog = @"SELECT hoten, biensoxe, loaixe, giovao, ngayvao, hinhnhanvien, hinhcam1vao, hinhcam2vao, uniqueID
                                  FROM XENHANVIEN, CURRENTVEHICLE
                                  WHERE XENHANVIEN.mathe=CURRENTVEHICLE.mathe";
            return (DataTable)db.ExeReader(new SqlCommand(getdatalog));
        }

        public DataTable dataRowCurrent()
        {
            string getdatalog = @"SELECT *
                                  FROM XENHANVIEN, CURRENTVEHICLE
                                  WHERE XENHANVIEN.mathe=CURRENTVEHICLE.mathe";
            return (DataTable)db.ExeReader(new SqlCommand(getdatalog));
        }

        public DataTable xenhanvien()
        {
            string getdatalog = @"SELECT* FROM XENHANVIEN";
            return (DataTable)db.ExeReader(new SqlCommand(getdatalog));
        }


        public void cardout()
        {
            // find GIUD with 
        }
        public void cardin()
        {
            //add GUID, create new table 
        }


    }
}
