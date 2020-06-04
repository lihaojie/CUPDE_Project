using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

/// <summary>
/// 地址选择组
/// </summary>
public class AddressPickerGroup : MonoBehaviour
{
    /// <summary>
    /// 选择的第几个
    /// </summary>
    public Dictionary<AddressType, int> _selectDate;
    public ProvinceModel selectAddress;
    public List<ProvinceModel> provinceModels;

    public List<ProvinceModel> ProvinceModels
    {
        get => provinceModels;
        set => provinceModels = value;
    }

    /// <summary>
    /// 地址选择器列表
    /// </summary>
    public List<AddressPicker> _datePickerList;

    public void Init(List<ProvinceModel> provinceModels, ProvinceModel selectAddress)
    {
        this.provinceModels = provinceModels;
        if (selectAddress != null)
        {
            this.selectAddress = selectAddress;
            _selectDate = new Dictionary<AddressType, int>();
            for (int i = 0; i < provinceModels.Count; i++)
            {
                if (selectAddress.code.Equals(provinceModels[i].code))
                {
                    _selectDate.Add(AddressType.Province, i);
                    for (int j = 0; j < provinceModels[i].cityList.Count; j++)
                    {
                        if (selectAddress.cityCode.Equals(provinceModels[i].cityList[j].code))
                        {
                            _selectDate.Add(AddressType.City, j);
                            for (int k = 0; k < provinceModels[i].cityList[j].areaList.Count; k++)
                            {
                                if (selectAddress.areaCode.Equals(provinceModels[i].cityList[j].areaList[k].code))
                                {
                                    _selectDate.Add(AddressType.Area, k);
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < _datePickerList.Count; i++)
            {
                _datePickerList[i].myGroup = this;
                _datePickerList[i].Init();
                _datePickerList[i]._onDateUpdate += onDateUpdate;
            }
        }
        else
        {
            this.provinceModels = provinceModels;
            this.selectAddress = new ProvinceModel();
            _selectDate = new Dictionary<AddressType, int>();
            _selectDate.Add(AddressType.Province, 0);
            _selectDate.Add(AddressType.City, 0);
            _selectDate.Add(AddressType.Area, 0);
            selectAddress.name = provinceModels[0].name;
            selectAddress.code = provinceModels[0].code;
            selectAddress.cityName = provinceModels[0]
                .cityList[0].name;
            selectAddress.cityCode = provinceModels[0]
                .cityList[0].code;
            selectAddress.areaName = provinceModels[0]
                .cityList[0]
                .areaList[0].name;
            selectAddress.areaCode = provinceModels[0]
                .cityList[0]
                .areaList[0].code;
            for (int i = 0; i < _datePickerList.Count; i++)
            {
                _datePickerList[i].myGroup = this;
                _datePickerList[i].Init();
                _datePickerList[i]._onDateUpdate += onDateUpdate;
            }
        }
    }
    
    public void Init(List<ProvinceModel> provinceModels)
    {
        this.provinceModels = provinceModels;
        this.selectAddress = new ProvinceModel();
        _selectDate = new Dictionary<AddressType, int>();
        _selectDate.Add(AddressType.Province, 0);
        _selectDate.Add(AddressType.City, 0);
        _selectDate.Add(AddressType.Area, 0);
        selectAddress.name = provinceModels[0].name;
        selectAddress.code = provinceModels[0].code;
        selectAddress.cityName = provinceModels[0]
            .cityList[0].name;
        selectAddress.cityCode = provinceModels[0]
            .cityList[0].code;
        selectAddress.areaName = provinceModels[0]
            .cityList[0]
            .areaList[0].name;
        selectAddress.areaCode = provinceModels[0]
            .cityList[0]
            .areaList[0].code;
        for (int i = 0; i < _datePickerList.Count; i++)
        {
            _datePickerList[i].myGroup = this;
            _datePickerList[i].Init();
            _datePickerList[i]._onDateUpdate += onDateUpdate;
        }
    }

    
    /// <summary>
    /// 当选择的日期更新
    /// </summary>
    public void onDateUpdate(AddressType addressType)
    {
        switch (addressType)
        {
            case AddressType.Province:
                _selectDate[AddressType.City] = 0;
                _selectDate[AddressType.Area] = 0;
                break;
            case AddressType.City:
                _selectDate[AddressType.Area] = 0;
                break;
        }

        Log.I("当前选择地址：" + provinceModels[_selectDate[AddressType.Province]].name
                        + provinceModels[_selectDate[AddressType.Province]].cityList[_selectDate[AddressType.City]].name
                        + provinceModels[_selectDate[AddressType.Province]].cityList[_selectDate[AddressType.City]]
                            .areaList[_selectDate[AddressType.Area]].name
        );
        selectAddress.name = provinceModels[_selectDate[AddressType.Province]].name;
        selectAddress.code = provinceModels[_selectDate[AddressType.Province]].code;
        selectAddress.cityName = provinceModels[_selectDate[AddressType.Province]]
            .cityList[_selectDate[AddressType.City]].name;
        selectAddress.cityCode = provinceModels[_selectDate[AddressType.Province]]
            .cityList[_selectDate[AddressType.City]].code;
        selectAddress.areaName = provinceModels[_selectDate[AddressType.Province]]
            .cityList[_selectDate[AddressType.City]]
            .areaList[_selectDate[AddressType.Area]].name;
        selectAddress.areaCode = provinceModels[_selectDate[AddressType.Province]]
            .cityList[_selectDate[AddressType.City]]
            .areaList[_selectDate[AddressType.Area]].code;
        for (int i = 0; i < _datePickerList.Count; i++)
        {
            _datePickerList[i].RefreshDateList();
        }
    }
}