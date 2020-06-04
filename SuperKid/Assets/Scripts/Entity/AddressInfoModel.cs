using System.Collections.Generic;

public class AddressInfoModel
{
    public int current;

    public List<AddressInfo> records;
    public class AddressInfo
    {
        public string areaCode;
        public string areaName;
        public int babyId;
        public int id;
        public int isDefault;
        public string cityCode;
        public string cityName;
        public string provinceCode;
        public string provinceName;
        public string consignee;
        public string consigneeAddress;
        public string consigneeMobile;
        public string createTime;
    }
}