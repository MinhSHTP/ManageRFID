using RFID_SHTP.ConnectDatabase.BEL;
using RFID_SHTP.ConnectDatabase.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public DataTable test()
        {
            //db.SendImage();
            string queryString = "Select * from Items";
            SqlCommand command = new SqlCommand(queryString);
            getTable = db.ExeReader(command);

            return getTable;
        }
        public byte[] getImage()
        {

            return db.getDocument();
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
