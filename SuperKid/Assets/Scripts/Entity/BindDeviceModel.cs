using System.Collections.Generic;

namespace SuperKid.Entity
{
    public class BindDeviceModel
    {
        public string deviceId;// 设备 ID

        public string state; // 0失败1成功 
        public int status; // 0失败，1成功
        
        public int requestTag; // 1.获取音频 2.绑定设备
    }
}