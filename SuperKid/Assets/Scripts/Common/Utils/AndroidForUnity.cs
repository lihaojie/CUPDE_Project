using System;
using QFramework;
using SuperKid;
using UnityEngine;

public class AndroidForUnity : MonoBehaviour
{

    /**
     * 有动画
     */
    public static void CallAndroidStartActivityForAnim(string message, string anim)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            //注解1
            bool Tag = true;
            while (Tag)
            {
                try
                {
                    using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                        {
                            //调用Android插件中UnityPlayerActivity中startActivity方法，message表示它的参数
                            jo.Call("unityForStartActivityWithAnim", message, anim);
                        }
                    }
                    Tag = false;
                }
                catch (Exception e)
                {
                    
                }
            }
        }
    }
    
    /**
     * 无动画
     */
    public static void CallAndroidStartActivity(string message)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            //注解1
            bool Tag = true;
            while (Tag)
            {
                try
                {
                    using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                        {
                            //调用Android插件中UnityPlayerActivity中startActivity方法，message表示它的参数
                            jo.Call("unityForStartActivity", message);
                        }
                    }
                    Tag = false;
                }
                catch (Exception e)
                {
                    
                }
            }
        }
    }

    /**
     * 隐藏过度遮挡
     */
    public static void CallAndroidHideSplash()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            bool Tag = true;
            while (Tag)
            {
                try
                {
                    using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                        {
                            //调用Android插件中UnityPlayerActivity中startActivity方法，message表示它的参数
                            jo.Call("unityForHideSplash");
                        }
                    }
                    Tag = false;
                }
                catch (Exception e)
                {
                    
                }
            }
        }
    }
    
     
    /**
     * 吐司
     */
    public static void CallAndroidForToast(string message)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //调用Android插件中UnityPlayerActivity中openAlbum方法，message表示它的参数
                    jo.Call("unityForShowToast", message);
                }
            }
        }
    }
    
    /**
     * 开始设备联网，发送wifi账号密码
     */
    public static void CallAndroidForStartDeviceConnect(string name, string pwd)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //调用Android插件中UnityPlayerActivity中openAlbum方法，message表示它的参数
                    jo.Call("unityForStartForDeviceConnect", name, pwd);
                }
            }
        }
    }
    
    
    /**
     * 停止设备联网
     */
    public static void CallAndroidForStopDeviceConnect()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //调用Android插件中UnityPlayerActivity中openAlbum方法，message表示它的参数
                    jo.Call("unityForStopForDeviceConnect");
                }
            }
        }
    }
   
      
    /**
     * 开启mqtt服务
     */
    public static void CallAndroidForStartMqttService(string message)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //调用Android插件中UnityPlayerActivity中openAlbum方法，message表示它的参数
                    jo.Call("unityForStartMqttService", message);
                }
            }
        }
    }
    
    
    /**
     * 停止mqtt服务，如退出登录时
     */
    public static void CallAndroidForStopMqttService()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //调用Android插件中UnityPlayerActivity中openAlbum方法，message表示它的参数
                    jo.Call("unityForStopMqttService");
                }
            }
        }
    }

    
    /**
     * 查询设备音量
     */
    public static void CallAndroidForQueryVol()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //调用Android插件中UnityPlayerActivity中openAlbum方法，message表示它的参数
                    jo.Call("unityForQueryVol");
                }
            }
        }
    }
    
    /**
     * 更新设备音量
     */
    public static void CallAndroidForUpdateVol(string message)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //调用Android插件中UnityPlayerActivity中openAlbum方法，message表示它的参数
                    jo.Call("unityForUpdateVol", message);
                }
            }
        }
    }
    
    /**
     * 更新版本(弃用)
     */
    public static void CallAndroidForUpdateApp(string message)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //调用Android插件中UnityPlayerActivity中openAlbum方法，message表示它的参数
                    jo.Call("unityForUpdateApp", message);
                }
            }
        }
    }

    /**
     * 分享
     */
    public static void CallAndroidForShare(string message)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //调用Android插件中UnityPlayerActivity中openAlbum方法，message表示它的参数
                    jo.Call("unityForShare", message);
                }
            }
        }
    }
    
    /**
     * 保存到相册
     */
    public static void CallAndroidForSavePicToAlbum(string url)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //调用Android插件中UnityPlayerActivity中openAlbum方法，message表示它的参数
                    jo.Call("unityForSavePicToAlbum", url);
                }
            }
        }
    }
    
     
    /**
     * 播放视频
     */
    public static void CallAndroidForPlayVideo(string path)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    jo.Call("unityForPlayVideo", path);
                }
            }
        }
    }
     
    /**
     * 查看图片
     */
    public static void CallAndroidForShowPic(string path)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    jo.Call("unityForShowPic", path);
                }
            }
        }
    }
    
    /**
     * 更新用户信息
     */
    public static void CallAndroidForUpdateUserInfo()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    jo.Call("unityForUpdateUserInfo");
                }
            }
        }
    }
    
    
    /**
     * 压缩视频
     */
    public static void CallAndroidForVideoCompress(string path)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    jo.Call("unityForVideoCompress", path);
                }
            }
        }
    }
}
