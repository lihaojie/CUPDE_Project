namespace SuperKid.Entity
{
    public class DeviceModel
    {
        public Device deviceStatus;
        public bool deviceOnline;
        public string current;
        
        public class Device
        {
            public int vol; // 音量volume; // 0 - 100
            public int battery; // 电量battery;// 0 - 100
        
        }
    }
}