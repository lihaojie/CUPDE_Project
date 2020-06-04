using System.Collections;
using System.Collections.Generic;
using QFramework;
using SuperKid;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUser : MonoBehaviour
{
    public Button BtnEdit;
    public Image ImgHead;
    public Text TextMobile;
    public Text TextRelation;
    private UserInfoModel mUserInfoModel;
    private ResLoader mResLoader  = ResLoader.Allocate();
    private Texture2D mTexture2DFather, mTexture2DMother
        , mTexture2DGrandFather, mTexture2DGrandMother
        , mTexture2DBrother, mTexture2DSister;
    
    public void SetContent(UserInfoModel userInfoModel)
    {
        this.mUserInfoModel = userInfoModel;
        BtnEdit.onClick.AddListener(() =>
        {
            AudioManager.PlaySound("Button_Audio");
            UIMgr.OpenPanel<UserInfoPanel>(new UserInfoPanelData()
            {
                UserInfoModel = mUserInfoModel
            }, UITransitionType.CIRCLE);
        });
        mTexture2DFather = mResLoader.LoadSync<Texture2D>("ic_head_father");
        mTexture2DMother = mResLoader.LoadSync<Texture2D>("ic_head_mother");
        mTexture2DGrandFather = mResLoader.LoadSync<Texture2D>("ic_head_grand_father");
        mTexture2DGrandMother = mResLoader.LoadSync<Texture2D>("ic_head_grand_mother");
        mTexture2DBrother = mResLoader.LoadSync<Texture2D>("ic_head_brother");
        mTexture2DSister = mResLoader.LoadSync<Texture2D>("ic_head_sister");
        if (userInfoModel.logoUrl.IsNotNullAndEmpty())
        {
            ImageDownloadUtils.Instance.SetAsyncImage(userInfoModel.logoUrl, ImgHead);
        } 
        else if (userInfoModel.babyRelation.Equals("父亲"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DFather,
                new Rect(0, 0, mTexture2DFather.width, mTexture2DFather.height), Vector2.one * 0.5f);
        }
        else if (userInfoModel.babyRelation.Equals("母亲"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DMother,
                new Rect(0, 0, mTexture2DMother.width, mTexture2DMother.height), Vector2.one * 0.5f);
        }
        else if (userInfoModel.babyRelation.Equals("祖父"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DGrandFather,
                new Rect(0, 0, mTexture2DGrandFather.width, mTexture2DGrandFather.height), Vector2.one * 0.5f);
        }
        else if (userInfoModel.babyRelation.Equals("祖母"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DGrandMother,
                new Rect(0, 0, mTexture2DGrandMother.width, mTexture2DGrandMother.height), Vector2.one * 0.5f);
        }
        else if (userInfoModel.babyRelation.Equals("哥哥"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DBrother,
                new Rect(0, 0, mTexture2DBrother.width, mTexture2DBrother.height), Vector2.one * 0.5f);
        }
        else if (userInfoModel.babyRelation.Equals("姐姐"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DSister,
                new Rect(0, 0, mTexture2DSister.width, mTexture2DSister.height), Vector2.one * 0.5f);
        }
        TextMobile.text = userInfoModel.mobile;
        TextRelation.text = "和宝宝关系：" +userInfoModel.babyRelation;
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
