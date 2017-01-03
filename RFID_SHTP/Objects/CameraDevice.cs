using RFID_SHTP.Helpers;

namespace RFID_SHTP.Objects
{
    public class CameraDevice
    {
        public CameraDevice(string cameraName, string hardwareId)
        {
           _nameDeice = cameraName;
           _hardwareId = hardwareId;
        }
        public string _nameDeice { get; set; }
        public string _hardwareId { get; set; }
    }
}
