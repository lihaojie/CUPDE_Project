using System.Collections;
using System.Collections.Generic;
using QFramework;
using SuperKid;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoFamilyEmpty : MonoBehaviour
{
    public Button BtnBind;
    
    private void Awake()
    {
        BtnBind.onClick.AddListener(() =>
        {
            AudioManager.PlaySound("Button_Audio");
            UIMgr.OpenPanel<BindConfirmBootPanel>(new BindConfirmBootPanelData()
            {
                isFromSetting = true
            }, UITransitionType.CIRCLE);
        });
    }

}
