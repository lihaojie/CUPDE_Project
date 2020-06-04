using System;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 当选择日期的委托
/// </summary>
public delegate void OnAddressUpdate(AddressType addressType);


public enum AddressType
{
    Province,
    City,
    Area
}

/// <summary>
/// 日期选择器
/// 2019.4.10
/// wxw
/// </summary>
public class AddressPicker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 日期类型 （年月日时分秒）
    /// </summary>
    public AddressType _addressType;

    /// <summary>
    /// 子节点数量（奇数）
    /// </summary>
    public int _itemNum = 5;

    [HideInInspector]
    /// <summary>
    /// 更新选择的目标值
    /// </summary>
    public float _updateLength;

    /// <summary>
    /// 子节点预制体
    /// </summary>
    public GameObject _itemObj;

    /// <summary>
    /// 子节点容器对象
    /// </summary>
    public Transform _itemParent;

    /// <summary>
    /// 我属于的日期选择组
    /// </summary>
    [HideInInspector] public AddressPickerGroup myGroup;

    /// <summary>
    /// 当选择日期的委托事件
    /// </summary>
    public event OnAddressUpdate _onDateUpdate;


    private Color nowColor;

    void Awake()
    {
        _itemObj.SetActive(false);
        _updateLength = _itemObj.GetComponent<RectTransform>().sizeDelta.y;
    }

    /// <summary>
    /// 初始化时间选择器
    /// </summary>
    public void Init()
    {
        _itemParent.DestroyAllChild();
        for (int i = 0; i < _itemNum; i++)
        {
            //计算真实位置
            int real_i = -(_itemNum / 2) + i;
            SpawnItem(real_i);
        }

        RefreshDateList();
    }

    /// <summary>
    /// 生成子对象
    /// </summary>
    /// <param name="real_i">真实位置</param>
    /// <returns></returns>
    GameObject SpawnItem(float real_i)
    {
        GameObject _item = Instantiate(_itemObj);
        _item.SetActive(true);
        _item.transform.SetParent(_itemParent);
        _item.transform.localScale = Vector3.one;
        _item.transform.localEulerAngles = Vector3.zero;
        if (real_i != 0)
        {
            // _item.GetComponent<Text>().color = new Color(1, 1, 1, 1 - 0.2f - (Mathf.Abs(real_i) / (_itemNum / 2 + 1)));
            ColorUtility.TryParseHtmlString("#666666", out nowColor);
            _item.GetComponent<Text>().color = nowColor;
            _item.GetComponent<Text>().fontSize = 32;
        }

        return _item;
    }

    Vector3 oldDragPos;

    /// <summary>
    /// 当开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        oldDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateSelectDate(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _itemParent.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 更新地址的时间
    /// </summary>
    /// <param name="eventData"></param>
    void UpdateSelectDate(PointerEventData eventData)
    {
        //判断拖拽的长度是否大于目标值
        if (Mathf.Abs(eventData.position.y - oldDragPos.y) >= _updateLength)
        {
            int num;
            //判断加减
            if (eventData.position.y > oldDragPos.y)
            {
                //+
                num = 1;
            }
            else
            {
                //-
                num = -1;
            }

            //判断是否在列表里 保证不越界
            if (IsInDate(num))
            {
                switch (_addressType)
                {
                    case AddressType.Province:
                        myGroup._selectDate[_addressType] = myGroup._selectDate[_addressType] + num;
                        break;
                    case AddressType.City:
                        myGroup._selectDate[_addressType] = myGroup._selectDate[_addressType] + num;
                        break;
                    case AddressType.Area:
                        myGroup._selectDate[_addressType] = myGroup._selectDate[_addressType] + num;
                        break;
                }

                oldDragPos = eventData.position;
                _onDateUpdate(_addressType);
            }

            _itemParent.localPosition = Vector3.zero;
        }
        else
        {
            _itemParent.localPosition = new Vector3(0, (eventData.position.y - oldDragPos.y), 0);
        }
    }

    /// <summary>
    /// 刷新地址列表
    /// </summary>
    public void RefreshDateList()
    {
        for (int i = 0; i < _itemNum; i++)
        {
            //计算真实位置
            int real_i = -(_itemNum / 2) + i;
            string str = "";
            //当前选择的位置position
            int currentSelectIndex = myGroup._selectDate[_addressType];
            int realItemSize = GetRealItemSize();
            if (currentSelectIndex + real_i >= 0 && currentSelectIndex + real_i <= realItemSize)
            {
                if (IsInDate(real_i))
                {
                    switch (_addressType)
                    {
                        case AddressType.Province:
                            str = myGroup.provinceModels[myGroup._selectDate[AddressType.Province] + real_i].name;
                            break;
                        case AddressType.City:
                            str = myGroup.provinceModels[myGroup._selectDate[AddressType.Province]]
                                .cityList[myGroup._selectDate[AddressType.City] + real_i].name;
                            break;
                        case AddressType.Area:
                            str = myGroup.provinceModels[myGroup._selectDate[AddressType.Province]]
                                .cityList[myGroup._selectDate[AddressType.City]]
                                .areaList[myGroup._selectDate[AddressType.Area] + real_i].name;
                            break;
                    }
                }
            }

            _itemParent.GetChild(i).GetComponent<Text>().text = str;
        }
    }

    /// <summary> 
    /// 判断某个日期是否在某段日期范围内，返回布尔值
    /// </summary> 
    /// <param name="dt">要判断的日期</param> 
    /// <param name="dt_min">开始日期</param> 
    /// <param name="dt_max">结束日期</param> 
    /// <returns></returns>  
    public bool IsInDate(int num)
    {
        int currentSelectIndex = myGroup._selectDate[_addressType];
        if (currentSelectIndex + num >= 0 && currentSelectIndex + num < GetRealItemSize())
        {
            return true;
        }

        return false;
    }

    public int GetRealItemSize()
    {
        int size = 0;
        switch (_addressType)
        {
            case AddressType.Province:
                size = myGroup.provinceModels.Count;
                break;
            case AddressType.City:
                size = myGroup.provinceModels[myGroup._selectDate[AddressType.Province]].cityList.Count;
                break;
            case AddressType.Area:
                size = myGroup.provinceModels[myGroup._selectDate[AddressType.Province]]
                    .cityList[myGroup._selectDate[AddressType.City]].areaList.Count;
                break;
        }

        return size;
    }
}