using System;
using UnityEngine;
using System.Collections;
using QFramework;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;




public class SeekCtrl : MonoBehaviour ,IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler{

    // public MediaPlayerCtrl m_srcVideo;
    public Slider Slider;


    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (mIOnValueChange.IsNotNull())
        {
            mIOnValueChange.onValueStop((int) (Slider.value));
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (mIOnValueChange.IsNotNull())
        {
            mIOnValueChange.onValueChange((int) (Slider.value));
        }
        
    }

    public IOnValueChange mIOnValueChange;

    public void setIOnValueChange(IOnValueChange mIOnValueChange)
    {
        this.mIOnValueChange = mIOnValueChange;
    }
    public interface IOnValueChange
    {
        // 音量变化改变文本显示
        void onValueChange(int progress);
        //手指抬起，发送消息更新设备音量
        void onValueStop(int progress);
    }

}