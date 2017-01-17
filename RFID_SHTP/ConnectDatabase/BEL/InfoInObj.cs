using RFID_SHTP.ConnectDatabase.BAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID_SHTP.ConnectDatabase.BEL
{
    public class InfoInObj
    {
        Operations _operation = new Operations();
        public string _Stt { get; set; }
        public string _Hoten { get; set; }
        public string _Biensoxe { get; set; }
        public string _Loaixe { get; set; }

        public string _Giovao { get; set; }
        public string _Ngayvao { get; set; }

        public string _Hinhnhanvien { get; set; }
        public string _Hinhcam1vao { get; set; }
        public string _Hinhcam2vao { get; set; }

        public string _UniqueID { get; set; }

        public string _Tracking { get; set; }

        public byte[] _WarningIcon { get; set; }

        public int _CountDay = 0;

        public List<InfoInObj> getListCurrentVehicle()
        {
            List<InfoInObj> resultList = new List<InfoInObj>();
            DataTable bindingDataTable = _operation.dataCurrent();

            for (int i = 0; i < bindingDataTable.Rows.Count; i++)//Rows
            {
                InfoInObj newInfoInObj = new InfoInObj();
                newInfoInObj._Stt = (i + 1).ToString();
                newInfoInObj._Hoten = bindingDataTable.Rows[i][0].ToString();
                newInfoInObj._Biensoxe = bindingDataTable.Rows[i][1].ToString();
                newInfoInObj._Loaixe = bindingDataTable.Rows[i][2].ToString();
                newInfoInObj._Giovao = bindingDataTable.Rows[i][3].ToString();
                newInfoInObj._Ngayvao = Convert.ToDateTime(bindingDataTable.Rows[i][4]).ToString("dd/MM/yyyy");
                newInfoInObj._Hinhnhanvien = bindingDataTable.Rows[i][5].ToString();
                newInfoInObj._Hinhcam1vao = bindingDataTable.Rows[i][6].ToString();
                newInfoInObj._Hinhcam2vao = bindingDataTable.Rows[i][7].ToString();
                newInfoInObj._UniqueID = bindingDataTable.Rows[i][8].ToString();
                newInfoInObj._Tracking = getTimeTracking(newInfoInObj._Ngayvao, newInfoInObj._Giovao);
                if(_CountDay>1)//more than 1 day
                {
                    newInfoInObj._WarningIcon = File.ReadAllBytes(@"..//..//Image/warning_icon_fix.png");
                }
                else
                {
                    newInfoInObj._WarningIcon = File.ReadAllBytes(@"..//..//Image/white_background.png");
                }
                resultList.Add(newInfoInObj);
                _CountDay = 0;
            }
            return resultList;
        }

        public string getTimeTracking(string ngayvao, string giovao)
        {
            string timeTracking = "";
            DateTime timeNow = DateTime.Now;
            string[] giovaoConverter = giovao.Split(':');
            string[] ngayvaoConverter = ngayvao.Split('/');
            DateTime timeIn = new DateTime(Int32.Parse(ngayvaoConverter[2]), Int32.Parse(ngayvaoConverter[1]), Int32.Parse(ngayvaoConverter[0]), Int32.Parse(giovaoConverter[0]), Int32.Parse(giovaoConverter[1]), Int32.Parse(giovaoConverter[2]));
            TimeSpan countTime = timeNow - timeIn;
            double resultTIme = countTime.TotalSeconds;
            if (resultTIme > 86400)// 1 day = 86400 second
            {
                timeTracking = "Đã vào từ " + (int)countTime.TotalDays + " ngày trước";
                _CountDay = (int)countTime.TotalDays;
            }
            else if ((resultTIme > 3600) && (resultTIme < 86400))//less than 1 day
            {
                timeTracking = "Đã vào từ " + (int)countTime.TotalHours + " tiếng trước";
            }
            else if ((resultTIme < 3600) && (resultTIme > 60))//less than 1 hour
            {
                timeTracking = "Đã vào từ " + (int)countTime.TotalMinutes + " phút trước";
            }
            else if (resultTIme < 60)// less than 1 minute
            {
                timeTracking = "Đã vào từ vài giây trước";
            }
            return timeTracking;
        }
    }
}
