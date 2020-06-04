using System; 
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; 
using QFramework;
using SuperKid;
using SuperKid.Utils;
using UnityEditor;
using UnityEngine;

public class PlayerPrefsUtil
{
    
    private static UserInfoModel mUserInfo;
    private static ContentModel mContentModel;
    private static List<BabyStudyLockModel> mLockModels;
    private static int mSelectPlanId;
    private static List<ProvinceModel> mProvinceModels;

    public static UserInfoModel UserInfo
    {
        get => GetUserInfo();
        set => SaveUserInfo(value);
    }

    public static ContentModel ContentModel
    {
        get => mContentModel;
        set => SaveContentModel(value);
    }

    public static List<BabyStudyLockModel> LockModels
    {
        get => GetBabyStudyLockModel();
        set => SaveBabyStudyLockModel(value);
    }

    public static List<ProvinceModel> ProvinceModels
    {
        get => GetProvinceModels();
        set => SaveProvinceModels(value);
    }

    
    public static int SelectPlanId
    {
        get => mSelectPlanId;
        set => mSelectPlanId = value;
    }

    /**
     * 通过ssid 读WiFi 的密码
     */
    public static string GetWiFiPWD(string ssid)
    {
        return GetStringValue(ssid, "");
    }
    
    /**
     * 保存 WiFi 的ssid 和密码
     */
    public static void SetWiFiSSIDPWD(string ssid,string pwd)
    {
        SetStringValue(ssid, pwd);
    }

    
    /**
     * 保存用户点击盒子的第几天(1-30 开头)
     */
    public static void SaveUserDayPosition(BoxModel content, int index)
    {
        string dayKey = AppConst.DAY_IDENTIFY + content.boxId + "_" + content.month + "_" +
                        GetUserInfo().relBabyId;
        PlayerPrefs.SetInt(dayKey, index);
    }

    /**
     * 取出用户点击盒子的第几天(1-30 开头)
     */
    public static int GetUserDayPosition(BoxModel content)
    {
        string dayKey = AppConst.DAY_IDENTIFY + content.boxId + "_" + content.month + "_" +
                        GetUserInfo().relBabyId;
        return PlayerPrefs.GetInt(dayKey, 0);
    }

    /**
     * 当前选择月份的下角标（0-11）
     */
    public static void SaveBoxMonthIndex(int index)
    {
        string boxIndetifyKey = AppConst.BOX_IDENTIFY + SelectPlanId + "_" + GetUserInfo().relBabyId;
        PlayerPrefs.SetInt(boxIndetifyKey, index);
    }

    /**
     * 获取选择月份的下角标（0-11）
     */
    public static int GetBoxMonthIndex()
    {
        string boxIndetifyKey = AppConst.BOX_IDENTIFY + SelectPlanId + "_" + GetUserInfo().relBabyId + "";
        return PlayerPrefs.GetInt(boxIndetifyKey, 0);
    }

    public static void SetToken(string token)
    {
        HttpUtil.token = token;
        SetStringValue("token", token);
    }

    public static string GetToken()
    {
        return GetStringValue("token", "");
    }
    
    public static void SetDeviceId(string deviceId)
    {
        SetStringValue("deviceId", deviceId);
    }

    public static string GetDeviceId()
    {
        return GetStringValue("deviceId", "");
    }
    
    public static int GetBabyId()
    {
        return GetIntValue("babyId", 0);
    }

    public static void SetBabyId(int babyId)
    {
        SetIntValue("babyId", babyId);
    }

    public static int GetUserId()
    {
        return GetIntValue("userId", 0);
    }

    
    public static void SetUserId(int userId)
    {
        SetIntValue("userId", userId);
    }

    public static void SetPhone(string phone)
    {
        SetStringValue("phone", phone);
    }

    public static string GetPhone()
    {
        return GetStringValue("phone", "");
    }
    
    public static void SetPwd(string pwd)
    {
        SetStringValue("pwd", pwd);
    }

    public static string GetPwd()
    {
        return GetStringValue("pwd", "");
    }
    public static string GetStringValue(string keyWord, string defaultValue)
    {
        return PlayerPrefs.GetString(keyWord, defaultValue);
    }

    public static void SetStringValue(string keyWord, string value)
    {
        PlayerPrefs.SetString(keyWord, value);
        PlayerPrefs.Save();
    }

    public static int GetIntValue(String keyWord, int defaultValue)
    {
        return PlayerPrefs.GetInt(keyWord, defaultValue);
    }

    public static void SetIntValue(String keyWord, int value)
    {
        PlayerPrefs.SetInt(keyWord, value);
        PlayerPrefs.Save();
    }

    /**
	 * @param key
	 * @param value
	 */
    public static void SetSerializable(string key, object value)
    {
        SetStringValue(key, value.ToJson());
    }

    /**
	 * @param key
	 * @param type
	 * @param <T>
	 * @return
	 */
    public static T GetObject<T>(string key)
    {
        var json = GetStringValue(key, "");
        T deserializeObject;
        try
        {
            deserializeObject = JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return deserializeObject;
    }

    private static void SaveUserInfo(UserInfoModel userInfoModel)
    {
        if (userInfoModel != null)
        {
            if (userInfoModel.babyInfoVo.IsNotNull())
            {
                userInfoModel.babyInfoVo.name = StringUtil.Emoji(userInfoModel.babyInfoVo.name);
            }
            if (userInfoModel.token.IsNotNullAndEmpty())
            {
                SetToken(userInfoModel.token);
            }
            SetUserId(userInfoModel.id);
            SetBabyId(userInfoModel.relBabyId);
            SetDeviceId(userInfoModel.deviceId);
            if (Application.platform == RuntimePlatform.Android)
            {
                if (userInfoModel.deviceId.IsNotNullAndEmpty())
                {
                    if (userInfoModel.mqttInfo.IsNotNull())
                    {
                        AndroidForUnity.CallAndroidForStartMqttService(userInfoModel.mqttInfo.ToJson());
                    }
                }
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (userInfoModel.deviceId.IsNotNullAndEmpty())
                {
                    if (userInfoModel.mqttInfo.IsNotNull())
                    {
                        IOSClientUtil.ConnectMQTTIOSClient(userInfoModel.mqttInfo.ToJson());
                    }
                }
            }
        }
        else
        {
            SetToken("");
            SetUserId(0);
            SetBabyId(0);
            SetDeviceId("");
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidForUnity.CallAndroidForStopMqttService();
                AndroidForUnity.CallAndroidForStopDeviceConnect();
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                IOSClientUtil.DisConnectMQTTIOSClient();
                IOSClientUtil.StopBindDevice("");
            }
        }
        mUserInfo = userInfoModel;
        userInfoModel.SaveJson(Application.temporaryCachePath + "/user.json");
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidForUnity.CallAndroidForUpdateUserInfo();
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            Dictionary<string, object> subParam = new Dictionary<string, object>();
            paramDic.Add("method",IOSClientUtil.UserInfoDidChange);
            paramDic.Add("params",subParam);
            IOSClientUtil.CommonMethodCallIOSClient(paramDic.ToJson());
        }

        
    }

    private static UserInfoModel GetUserInfo()
    {
        if (mUserInfo != null)
        {
            return mUserInfo;
        }

        if (File.Exists(Application.temporaryCachePath + "/user.json"))
        {
            try
            {
                mUserInfo = SerializeHelper.LoadJson<UserInfoModel>(Application.temporaryCachePath + "/user.json");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return mUserInfo;
            }
        }
        return mUserInfo;
    }

    /**
     * 保存计划包数据（增量保存）
     */
    private static void SaveContentModel(ContentModel contentModel)
    {
        string boxModelPath = Application.temporaryCachePath + "/ContentModel/";
        if (!Directory.Exists(boxModelPath))
        {
            Directory.CreateDirectory(boxModelPath);
        }
        
        string absPathName = boxModelPath + "ContentModel_" + contentModel.planId + ".json";
        ContentModel localModel = GetLocalContentModel(contentModel.planId);
        if (localModel.IsNull())
        {
            mContentModel = contentModel;
            contentModel.SaveJson(absPathName);
        }
        else
        {
            localModel.subject = contentModel.subject;
            localModel.applyAge = contentModel.applyAge;
            localModel.planCover = contentModel.planCover;
            localModel.planGoal = contentModel.planGoal;
            localModel.planName = contentModel.planName;
            localModel.isPowerPlan = contentModel.isPowerPlan;
            localModel.levelStrList = contentModel.levelStrList;
            // 记录本地盒子id
            List<int> boxIdList = new List<int>();
            // 替换本地已有的
            for (var i = 0; i < localModel.boxList.Count; i++)
            {
                boxIdList.Add(localModel.boxList[i].boxId);
                for (var j = 0; j < contentModel.boxList.Count; j++)
                {
                    if (localModel.boxList[i].boxId == contentModel.boxList[j].boxId)
                    {
                        localModel.boxList[i] = contentModel.boxList[j];
                    }
                }
            }

            //添加本地没有的
            for (var i = 0; i < contentModel.boxList.Count; i++)
            {
                if (!boxIdList.Contains(contentModel.boxList[i].boxId))
                {
                    localModel.boxList.Add(contentModel.boxList[i]);
                }
            }

            //删除标记为删除的
            for (int i = localModel.boxList.Count - 1; i >= 0; i--)
            {
                if (localModel.boxList[i].isDel == 1)
                    localModel.boxList.Remove(localModel.boxList[i]);
            }

            mContentModel = localModel;
            localModel.SaveJson(absPathName);
        }
    }

    /**
     * 获取计划包数据
     */
    public static ContentModel GetContentModel(int planId)
    {
        string boxModelPath = Application.temporaryCachePath + "/ContentModel/";
        if (!Directory.Exists(boxModelPath))
        {
            Directory.CreateDirectory(boxModelPath);
        }

        string absPathName = boxModelPath + "ContentModel_" + planId + ".json";

        return SerializeHelper.LoadJson<ContentModel>(absPathName);
    }

    private static void SaveBabyStudyLockModel(List<BabyStudyLockModel> data)
    {
        mLockModels = data;
        string babyPath = Application.temporaryCachePath + "/BabyStudyLockModel/";
        if (!Directory.Exists(babyPath))
        {
            Directory.CreateDirectory(babyPath);
        }
        string absPathName = babyPath + "BabyStudyLockModel_" + UserInfo.relBabyId + ".json";
        data.SaveJson(absPathName);
    }

    private static List<BabyStudyLockModel> GetBabyStudyLockModel()
    {
        if (mLockModels!=null)
        {
            return mLockModels;
        }
        string babyPath = Application.temporaryCachePath + "/BabyStudyLockModel/";
        string absPathName = babyPath + "BabyStudyLockModel_" + UserInfo.relBabyId  + ".json";
        Log.E(absPathName);
        var babyStudyLockModels = SerializeHelper.LoadJson<List<BabyStudyLockModel>>(absPathName);
        mLockModels = babyStudyLockModels;
        return babyStudyLockModels;
    }

    public static ContentModel GetLocalContentModel(int planId)
    {
        string boxModelPath = Application.temporaryCachePath + "/ContentModel/";
        if (!Directory.Exists(boxModelPath))
        {
            Directory.CreateDirectory(boxModelPath);
        }
        string absPathName = boxModelPath + "ContentModel_" + planId + ".json";
        ContentModel localModel = null;
        if (File.Exists(absPathName))
        {
           return  SerializeHelper.LoadJson<ContentModel>(absPathName);
        }

        return null;
    }


    public static bool IsFirstIn()
    {
        int playerIsFirstIn = PlayerPrefs.GetInt("player_isFirstIn", 0);
        return playerIsFirstIn == 0;
    }

    public static void SetFirstIn(bool isFirstIn)
    {
        PlayerPrefs.SetInt("player_isFirstIn", isFirstIn ? 0 : 1);
    }
    
    private static List<ProvinceModel> GetProvinceModels()
    {
        if (mProvinceModels != null)
        {
            return mProvinceModels;
        }

        if (File.Exists(Application.temporaryCachePath + "/ProvinceModels.json"))
        {
            try
            {
                mProvinceModels = SerializeHelper.LoadJson<List<ProvinceModel>>(Application.temporaryCachePath + "/ProvinceModels.json");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return mProvinceModels;
            }
        }
        return mProvinceModels;
    }
    
    private static void SaveProvinceModels(List<ProvinceModel> data)
    {
        mProvinceModels = data;
        data.SaveJson(Application.temporaryCachePath + "/ProvinceModels.json");
    }
}