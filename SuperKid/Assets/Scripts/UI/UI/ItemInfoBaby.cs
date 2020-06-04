using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using SuperKid;
using SuperKid.Entity;
using SuperKid.Utils;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoBaby : MonoBehaviour
{
    public Button BtnEdit;
    public Image ImgHead;
    public Text TextName;
    public Text TextBirthday;
    public Image ImgSex;
    private ResLoader mResLoader  = ResLoader.Allocate();
    private Texture2D mTexture2DBoy, mTexture2DGirl;
    private Texture2D mTexture2DHBoy, mTexture2DHGirl;
    private BabyInfoModel mBabyInfoModel;
    

    public void SetContent(BabyInfoModel babyInfoModel)
    {
        this.mBabyInfoModel = babyInfoModel;
        BtnEdit.onClick.AddListener(() =>
        {
            AudioManager.PlaySound("Button_Audio");
            UIMgr.OpenPanel<BaByInfoPanel>(new BaByInfoPanelData()
            {
                BabyInfoModel = mBabyInfoModel
            }, UITransitionType.CIRCLE);
        });
        mTexture2DBoy = mResLoader.LoadSync<Texture2D>("ic_baby_boy");
        mTexture2DGirl = mResLoader.LoadSync<Texture2D>("ic_baby_girl");
        mTexture2DHBoy = mResLoader.LoadSync<Texture2D>("ic_head_boy");
        mTexture2DHGirl = mResLoader.LoadSync<Texture2D>("ic_head_girl");
        TextName.text = babyInfoModel.name;
        TextBirthday.text = babyInfoModel.birthday;
        if (babyInfoModel.sex == 1) //男
        {
            ImgSex.sprite = Sprite.Create(mTexture2DBoy,
                new Rect(0, 0, mTexture2DBoy.width, mTexture2DBoy.height), Vector2.one * 0.5f);
        }
        else
        {
            ImgSex.sprite = Sprite.Create(mTexture2DGirl,
                new Rect(0, 0, mTexture2DGirl.width, mTexture2DGirl.height), Vector2.one * 0.5f);
        }

        if (babyInfoModel.babyLogoUrl.IsNotNullAndEmpty())
        {
            ImageDownloadUtils.Instance.SetAsyncImage(babyInfoModel.babyLogoUrl, ImgHead);
        } 
        else if (babyInfoModel.sex == 1)
        {
            ImgHead.sprite = Sprite.Create(mTexture2DHBoy,
                new Rect(0, 0, mTexture2DHBoy.width, mTexture2DHBoy.height), Vector2.one * 0.5f);
        }
        else if (babyInfoModel.sex == 2)
        {
            ImgHead.sprite = Sprite.Create(mTexture2DHGirl,
                new Rect(0, 0, mTexture2DHGirl.width, mTexture2DHGirl.height), Vector2.one * 0.5f);
        }
        
    }

    private void OnDestroy()
    {
        if (mResLoader.IsNotNull())
        {
            mResLoader.Recycle2Cache();
            mResLoader = null;
        }
    }
}
