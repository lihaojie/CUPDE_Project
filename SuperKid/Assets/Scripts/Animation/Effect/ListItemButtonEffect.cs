using System.Collections;
using System.Collections.Generic;
using QFramework;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ListItemButtonEffect : MonoBehaviour
{
    public Animator mAnimator;
    public BoolReactiveProperty IsBig = new BoolReactiveProperty(false);
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonClick);
        mAnimator = GetComponent<Animator>();
        IsBig.Subscribe(_ =>
        {
            if (IsBig.Value)
            {
                mAnimator.Play("ItemClickAnimation", 0, 0f);
            }
            else
            {
                mAnimator.Play("NarrowAnimation", 0, 0f);
            }
        });
        SimpleEventSystem.GetEvent<ListItemButton>().Subscribe(_ => { IsBig.Value = false; }).AddTo(this);

    }
    
    public void ButtonClick()
    {
        AudioManager.PlaySound("Button_Audio");
        SimpleEventSystem.Publish(new ListItemButton()); 
        IsBig.Value = true;
    }

}
