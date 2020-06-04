using System.Collections.Generic;

public class ProvinceModel : AddressModel
{
    public List<CityModel> cityList;
}

public class CityModel : AddressModel
{
    public List<AreaModel> areaList;
}

public class AreaModel : AddressModel
{
   
}

public class AddressModel
{
    public string code;
    public int id;
    public string name;
    public string cityName;
    public string cityCode;
    public string areaName;
    public string areaCode;
}