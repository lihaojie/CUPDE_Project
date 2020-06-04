using QFramework;
using UnityEngine;
using System.Collections.Generic;

namespace SuperKid.Utils
{
    public class CommonUtil
    {
        /**
         * 退出登录
         */
        public static void logout()
        {
            clearLocal();
            UIMgr.CloseAllPanel();
            UIMgr.OpenPanel<LoginPanel>(new LoginPanelData());
        }
        
        /**
         * 清空本地数据及通知原生关闭mqtt服务
         */
        public static void clearLocal()
        {
            PlayerPrefsUtil.UserInfo = null;
            PlayerPrefsUtil.SetPhone(null);
            PlayerPrefsUtil.SetPwd(null);
        }

        /**
         * 吐司
         */
        public static void toast(string message)
        {
            // if (Application.platform == RuntimePlatform.Android)
            // {
            //     AndroidForUnity.CallAndroidForToast(message);
            // }
            // else if (Application.platform == RuntimePlatform.IPhonePlayer)
            // {
            //     
            //     Dictionary<string, object> paramDic = new Dictionary<string, object>();
            //     Dictionary<string, object> subParam = new Dictionary<string, object>();
            //     subParam.Add("message",message);
            //     paramDic.Add("method",IOSClientUtil.Toast);
            //     paramDic.Add("params",subParam);
            //     IOSClientUtil.CommonMethodCallIOSClient(paramDic.ToJson());
            // }
            Debug.Log("Unity Toast Msg:"+message);
            ToastManager.GetInstance().CreatToast(message);
        }

        /**
         * 统一处理网络请求错误信息
         */
        public static void error(string errCode,string message,int status)
        {
            if ("100013".Equals(errCode) || "100012".Equals(errCode))
            {
                logout();
            }
        }
        
        /**
         * 统一打开首页，关闭其他panel
         */
        public static void OpenCloudMain(UIPanel selfBehaviour, IUIData uiData = null)
        {
            UIMgr.OpenPanel<MainPanel>(uiData,UITransitionType.CLOUD, null, 
                null, null, delegate
            {
                if (selfBehaviour != null)
                {
                    UIMgr.CloseAllOtherPanel(MainPanel.NAME);
                    UIMgr.ClosePanel(selfBehaviour.name);
                }
            });
           
        }

    }

    /**
     * 统一弹窗类型
     */
    public enum TipAction
    {
        Logout = 1, 
        FeedBackDel = 2, 
        ClearCache = 3,  
        FamilySubtract = 4, 
        FamilyTransfer = 5,
        AddressDel = 6,
        DeviceLostControl = 7,
        PlanScan = 8,
        BindDevice = 9,
        Plan = 10,
        Unbind = 11,
        DelAttendance = 12,
        BindingBack = 13,
        Contains5GAlter = 14,
        
    }
    
    /**
     * 统一选择图片弹窗来源类型
     */
    public enum ChoosePhotoAction
    {
        Baby = 1, 
        User = 2, 
        FeedBack = 3, 
        AttendancePic = 4,
        AttendanceVideo = 5,
    }
    
    /**
     * 安卓向原生获取数据action
     */
    public enum NativeAction
    {
        Location = 1,
        Album = 2, 
        Camera = 3, 
        DeviceConnect = 4,
        Version = 5,
        VersionJson = 6,
        Cache = 7,
        ClearCache = 8,
        SendSMS = 9,
        Audio = 10,
    }

    public enum AttendanceAdd
    {
        Pic = 0,
        Audio = 1,
        Video = 2,
    }

    public enum ShareAction
    {
        Save = 0,
        Wechat = 1,
        WechatMoments = 2,
    }
}