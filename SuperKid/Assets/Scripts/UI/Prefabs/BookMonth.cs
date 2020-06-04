using System;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using SuperKid;
using SuperKid.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


public class BookMonth : MonoBehaviour
{

    private Tween helpPanelTween;
    public List<GameObject> itemMonthList;
    
    private void Awake()
    {
        helpPanelTween = transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.InOutCubic);
        helpPanelTween.SetAutoKill(false);
        helpPanelTween.Pause();
        IsShow.Subscribe(IsShow =>
        {
            if (IsShow)
            {
                SimpleEventSystem.Publish(new CanvasDotMove());
                helpPanelTween.PlayForward();
            }
            else
            {
                SimpleEventSystem.Publish(new CanvasCanMove());
                helpPanelTween.PlayBackwards(); 
            }
        });
    }
    
    public BoolReactiveProperty IsShow = new BoolReactiveProperty(false);
    
    
    public void UpdateItemDate()
    {
        List<BoxModel> boxList =PlayerPrefsUtil.ContentModel.boxList;
        for (int i = 0; i < boxList.Count; i++)
        {
            GameObject item = itemMonthList[i];
            BoxModel box = boxList[i];
            Image imgThum = item.transform.Find("ImageMask").Find("Img_Thum").GetComponent<Image>();
            ImageDownloadUtils.Instance.SetAsyncImage(box.frontCover, imgThum);
            Text tvTitle = item.transform.Find("Tv_Title").GetComponent<Text>();
            tvTitle.text = box.name;
            Transform lockTr = item.transform.Find("ImageMask").Find("Img_Lock");
            item.GetComponent<Button>().onClick.RemoveAllListeners();
            item.GetComponent<Button>().onClick
                .AddListener(delegate { OnClick( box); });
            if (DoCheckIsLock( box.boxId))
            {
                lockTr.gameObject.SetActive(false);
            }
            else
            {
                lockTr.gameObject.SetActive(true);
            }  
        }
    }
    
    
    private void OnClick(BoxModel boxModel)
    {
        AudioManager.PlaySound("Button_Audio");
        // 保存当前最新点击的第几个盒子
        if (DoCheckIsLock(boxModel.boxId)) // 判断当前盒子是否已经解锁
        {
            IsShow.Value= !IsShow.Value;
            PlayerPrefsUtil.SaveBoxMonthIndex(boxModel.month - 1);
            SendMessageUpwards("UpdateMonth");
        }
        else
        {
            AudioManager.PlayVoice("Unlock_book");
            UIMgr.OpenPanel<TipPanel>(new TipPanelData()
            {
                action = TipAction.Plan,
                message = "当前盒子未解锁",
                isHideCancelButton = true,
                removeConfirmCallback = true,
                strConfirm = "我知道了"
            });
        }
    }

    
    private bool DoCheckIsLock(int boxId)
    {
        foreach (BabyStudyLockModel babyStudyLockBean in PlayerPrefsUtil.LockModels)
        {
            if (babyStudyLockBean.boxId == boxId)
            {
                return true;
            }
        }
        return false;
    }
    
}