using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using SuperKid;
using UnityEngine;
using UnityEngine.UI;

public class ItemFamilySetting : MonoBehaviour
{
    public Image ImgHead;
    public Text TextMobile;
    public Text TextRelation;
    public Toggle Toggle;
    public Image Checkmark;
    private FamilyModel mFamilyModel;
    private ResLoader mResLoader  = ResLoader.Allocate();
    // toggle_transfer_sel toggle_subtract_sel
    private Texture2D mTexture2DTransfer, mTexture2DSubtract;
    private Texture2D mTexture2DFather, mTexture2DMother
        , mTexture2DGrandFather, mTexture2DGrandMother
        , mTexture2DBrother, mTexture2DSister;

    public void SetContent(FamilyModel familyModel, int type)
    {
        this.mFamilyModel = familyModel;
        mTexture2DTransfer = mResLoader.LoadSync<Texture2D>("toggle_transfer_sel");
        mTexture2DSubtract = mResLoader.LoadSync<Texture2D>("toggle_subtract_sel");
        mTexture2DFather = mResLoader.LoadSync<Texture2D>("ic_head_father");
        mTexture2DMother = mResLoader.LoadSync<Texture2D>("ic_head_mother");
        mTexture2DGrandFather = mResLoader.LoadSync<Texture2D>("ic_head_grand_father");
        mTexture2DGrandMother = mResLoader.LoadSync<Texture2D>("ic_head_grand_mother");
        mTexture2DBrother = mResLoader.LoadSync<Texture2D>("ic_head_brother");
        mTexture2DSister = mResLoader.LoadSync<Texture2D>("ic_head_sister");
        
        if (familyModel.logoUrl.IsNotNullAndEmpty())
        {
            ImageDownloadUtils.Instance.SetAsyncImage(familyModel.logoUrl, ImgHead);
        } 
        else if (familyModel.babyRelation.Equals("父亲"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DFather,
                new Rect(0, 0, mTexture2DFather.width, mTexture2DFather.height), Vector2.one * 0.5f);
        }
        else if (familyModel.babyRelation.Equals("母亲"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DMother,
                new Rect(0, 0, mTexture2DMother.width, mTexture2DMother.height), Vector2.one * 0.5f);
        }
        else if (familyModel.babyRelation.Equals("祖父"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DGrandFather,
                new Rect(0, 0, mTexture2DGrandFather.width, mTexture2DGrandFather.height), Vector2.one * 0.5f);
        }
        else if (familyModel.babyRelation.Equals("祖母"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DGrandMother,
                new Rect(0, 0, mTexture2DGrandMother.width, mTexture2DGrandMother.height), Vector2.one * 0.5f);
        }
        else if (familyModel.babyRelation.Equals("哥哥"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DBrother,
                new Rect(0, 0, mTexture2DBrother.width, mTexture2DBrother.height), Vector2.one * 0.5f);
        }
        else if (familyModel.babyRelation.Equals("姐姐"))
        {
            ImgHead.sprite = Sprite.Create(mTexture2DSister,
                new Rect(0, 0, mTexture2DSister.width, mTexture2DSister.height), Vector2.one * 0.5f);
        }
        
        TextMobile.text = familyModel.mobile;
        if (familyModel.id == PlayerPrefsUtil.GetUserId())
        {
            TextRelation.text = "我";
        }
        else
        {
            TextRelation.text = mFamilyModel.babyRelation;
        }
        //0：移除，1：转让
        if (type == 0)
        {
            Checkmark.sprite = Sprite.Create(mTexture2DSubtract,
                new Rect(0, 0, mTexture2DSubtract.width, mTexture2DSubtract.height), Vector2.one * 0.5f);
        }
        else
        {
            Checkmark.sprite = Sprite.Create(mTexture2DTransfer,
                new Rect(0, 0, mTexture2DTransfer.width, mTexture2DTransfer.height), Vector2.one * 0.5f);
        }
    }
    
    private void OnDestroy()
    {
        mResLoader.Recycle2Cache();
        mResLoader = null;
    }

}
