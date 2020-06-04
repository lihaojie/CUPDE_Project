using System.Collections;
using System.Collections.Generic;
using QFramework;
using SuperKid;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoFamilyTitle : MonoBehaviour
{
    public Button BtnSubtract, BtnAdd, BtnTransfer;
    public Image ImageLine;
    
    private void Awake()
    {
        BtnSubtract.onClick.AddListener(() =>
        {
            AudioManager.PlaySound("Button_Audio");
            // FamilySettingPanel //0：移除，1：转让
            UIMgr.OpenPanel<FamilySettingPanel>(new FamilySettingPanelData()
            {
                type = 0
            }, UITransitionType.CIRCLE);
        });
        BtnAdd.onClick.AddListener(() =>
        {
            AudioManager.PlaySound("Button_Audio");
            // FamilyAddPanel
            UIMgr.OpenPanel<FamilyAddPanel>(new FamilyAddPanelData(), UITransitionType.CIRCLE);
        });
        BtnTransfer.onClick.AddListener(() =>
        {
            AudioManager.PlaySound("Button_Audio");
            // FamilySettingPanel
            UIMgr.OpenPanel<FamilySettingPanel>(new FamilySettingPanelData()
            {
                type = 1
            }, UITransitionType.CIRCLE);

        });
    }

    public void setContent(bool hasOther)
    {
        if (hasOther)
        {
            BtnSubtract.gameObject.SetActive(true);
            BtnTransfer.gameObject.SetActive(true);
            ImageLine.gameObject.SetActive(true);
        }
        else
        {
            BtnSubtract.gameObject.SetActive(false);
            BtnTransfer.gameObject.SetActive(false);
            ImageLine.gameObject.SetActive(false);
        }
    }

}
