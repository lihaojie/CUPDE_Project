using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class MainAnimation : MonoBehaviour
{
    public AnimationReferenceAsset defaultStand;
    public AnimationReferenceAsset wifiNotify;
    public AnimationReferenceAsset dionNotify;
    public AnimationReferenceAsset lionNotify;
    public AnimationReferenceAsset tongTongNotify;
    
    public SkeletonAnimation skeletonAnimation;
    
    public void DefaultStand()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, defaultStand, true);
    }
    
    public void WifiNotify()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, wifiNotify, true);
    }
    public void DionNotify()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, dionNotify, true);
    }
    
    public void LionNotify()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, lionNotify, true);
    } 
    
    public void TongTongNotify()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, tongTongNotify, true);
    }
}
