using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public enum CloudState
    {
        OPENANDCLOSE,
        CLOESE,
        OPEN
    }
    public class CloudOpenAnimation
    {
    }

    public class CloudCloseAnimation
    {
	 
    }
    public class CloudAnimationController : MonoBehaviour
    {
        [SerializeField] public Animator Mask;

        [SerializeField] private CloudState mCloudState;

        public void PlayAnimation(CloudState cloudState)
        {
            if (cloudState == null)
            {
                cloudState = CloudState.OPENANDCLOSE;
            }
            mCloudState = cloudState; 
            if (cloudState == CloudState.OPEN)
            {
                Mask.Play("OpenCloudAnimation",0,0);
            }
            else if (cloudState == CloudState.OPENANDCLOSE)
            {
                Mask.Play("OpenCloudAnimation",0,0);
            }else if (cloudState == CloudState.CLOESE)
            {
                Mask.Play("CloseCloudAnimation",0,0);
            } 
           
        }

        public void OpenCloudFinish()
        {
            if (mCloudState == CloudState.OPENANDCLOSE)
            {
                Mask.Play("CloseCloudAnimation",0,0);
            }
            SimpleEventSystem.Publish(new CloudOpenAnimation());
        }
    
        public void CloseCloudFinish()
        { 
            SendMessageUpwards("CloudCloseAnimation");
        }
    }
}