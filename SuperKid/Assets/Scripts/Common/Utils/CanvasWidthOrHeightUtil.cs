using QFramework;
using UnityEngine;

public class CanvasWidthOrHeightUtil
{
    public static int GetCanvasScaler()
    {
        float standard_width = 1334f; //初始宽度
        float standard_height = 750f; //初始高度
        float device_width = Screen.width; //当前设备宽度
        float device_height = Screen.height; //当前设备高度
        float width = device_width > device_height ? device_width : device_height;
        float height = device_width < device_height ? device_width : device_height;
        Log.I("device_width: " + device_width + " device_height: " + device_height);
        float adjustor = 0f; //屏幕矫正比例
        //计算宽高比例
        float standard_aspect = standard_width / standard_height;
        float device_aspect = width / height;
        //计算矫正比例
        if (device_aspect < standard_aspect)
        {
            Log.I("adjustor==="+adjustor);
            return 0; 
        }
        Log.I("adjustor==="+adjustor);
        return 1;
    }
}