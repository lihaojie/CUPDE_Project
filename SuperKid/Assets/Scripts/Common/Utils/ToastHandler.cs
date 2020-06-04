using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class ToastHandler : MonoBehaviour {
    public Image image;
    public Text text;
    
    //初始化
    public void InitToast(string str,System.Action callback) {
        text.text = str;
        int StrLenth = str.Length; 
        int ToastWidth = 1;
        int ToastHeight = 1;
        if (StrLenth > 10)
        {
            ToastHeight = (StrLenth / 10)+1;
            ToastWidth = 10;

        }
        else {
            ToastWidth = StrLenth;
        }
    
        image.rectTransform.sizeDelta = new Vector2(24.6f * ToastWidth + 80, 27.74f * ToastHeight + 80);
        FadeOut(callback);
    }

    public void FadeOut(System.Action callback) {
        Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ =>
        {
            image.DOFade(0,0.5f).OnComplete(()=>
            {
                callback.Invoke();
                Destroy(gameObject);
            });
            text.DOFade(0,0.5f);
        });
        
    }

    //堆叠向上移动
    public void Move(float speed, int targetPos)
    {
        transform.DOLocalMoveY(targetPos * (image.rectTransform.sizeDelta.y + 10), speed);
    }
}