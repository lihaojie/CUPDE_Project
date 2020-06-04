using System.Runtime.InteropServices;
using UnityEngine;

public class IOSClientUtil
{
    
    
    [DllImport("__Internal")]
    private static extern void UnityCallNative(string str);

    
    
    [DllImport("__Internal")]
    private static extern void UnityCallBackToNative(string str);
    
    
    [DllImport("__Internal")]
    private static extern void UnityCallNativeUMShare(string str);
    
    
    [DllImport("__Internal")]
    private static extern void UnityCallNativeSaveImage(string str);
    
    [DllImport("__Internal")]
    private static extern void _NativeGallery_ClearAppCache();
    
    
    [DllImport("__Internal")]
    private static extern void UnityCallNativeConnectMQTTT(string str);
    
    [DllImport("__Internal")]
    private static extern void UnityCallNativeDisConnectMQTTT();
    
    [DllImport("__Internal")]
    private static extern void UnityCallNativeCommonMehod(string str);
    
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _UnityNativeGallery_BindDevice(string msg);
        
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _UnityNativeGallery_StopBindDevice(string msg);

    public static void StartBindDevice(string msg)
    {
        _UnityNativeGallery_BindDevice(msg);
    }
    public static void StopBindDevice(string msg)
    {
        _UnityNativeGallery_StopBindDevice(msg);
    }
    
    public static void CallIOSClient(string msg)
    {
        UnityCallNative(msg);
    }
    
    public static void PageBackCallIOSClient(string msg)
    {
         UnityCallBackToNative("back");
    }
    
        
    public static void SaveImageToAlubmCallIOSClient(string msg)
    {
        UnityCallNativeSaveImage(msg);
    }
    
        
    public static void ShareObjectCallIOSClient(string msg)
    {
        UnityCallNativeUMShare(msg);
    }

    public static void ClearCacheCallIOSClient()
    {
        _NativeGallery_ClearAppCache();
    }
    
    
    public static void ConnectMQTTIOSClient(string msg)
    {
        UnityCallNativeConnectMQTTT(msg);
    }
    
    public static void DisConnectMQTTIOSClient()
    {
        UnityCallNativeDisConnectMQTTT();
    }

    public static void CommonMethodCallIOSClient(string msg)
    {
        UnityCallNativeCommonMehod(msg);
    }
    
    public const string SettingShare = "SettingShare";
    public const string AudioPlay = "AudioPlay";
    public const string Toast = "toast";
    public const string ExchangeDeviceVolume = "ExchangeDeviceVolume";
    public const string QueryDeviceVolume = "QueryDeviceVolume";
    public const string MainPanelOnInit = "MainPanelOnInit";
    public const string UserInfoDidChange = "UserInfoDidChange";
    public const string VideoMediaPlay = "VideoMediaPlay";
    
}
