
using AForge.Video.DirectShow;
using RFID_SHTP.Objects;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RFID_SHTP.Helpers
{
    public class GetListCamerasHelper
    {
        FilterInfoCollection _curVideoDevices;
        
        public GetListCamerasHelper()
        {

        }

        public FilterInfoCollection getListCameras()
        {
            try
            {
                _curVideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                //if (_videoDevices.Count == 0)
                //{
                //    throw new Exception();
                //}
            }
            catch
            {
                MessageBox.Show("Không tìm thấy bất kì camera nào", "Lỗi camera");
            }
            return _curVideoDevices;
        }

       

    }
}
