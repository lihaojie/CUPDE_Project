using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class MasterAnimation : MonoBehaviour
{
    public AnimationReferenceAsset defaultStand;
    public AnimationReferenceAsset jumpHighLeft;
    public AnimationReferenceAsset jumpLeft;
    public AnimationReferenceAsset jumpHighRight;
    public AnimationReferenceAsset jumpRight;
    
    SkeletonAnimation skeletonAnimation;
   
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>(); 
    }

    public void DefaultStand()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, defaultStand, true);
    }
    public void JumpHighLeft()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, jumpHighLeft, true);
    }
    public void JumpHighRight()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, jumpHighRight, true);
    }
    
    public void JumpLeft()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, jumpLeft, true);
    } 
    
    public void JumpRight()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, jumpRight, true);
    }
}
