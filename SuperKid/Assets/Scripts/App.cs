using System;
using System.Collections.Generic; 
using Newtonsoft.Json; 
using QFramework;
using SuperKid.Entity;
using SuperKid.Utils;
using UniRx;
using UnityEngine; 

namespace SuperKid
{
    public class App : MonoBehaviour
    {
        // var soundOnTexture = mResLoader.LoadSync<Texture2D>("sound_on");
        // AudioImage.sprite = Sprite.Create(soundOnTexture, new Rect(0, 0, soundOnTexture.width, soundOnTexture.height), Vector2.one * 0.5f);

        private bool isMusicNeedOn;
        
        private void Awake()
        {
            ResMgr.Init();
            AudioManager.On();
            UIMgr.SetResolution(1334,750, CanvasWidthOrHeightUtil.GetCanvasScaler());
            Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Escape)).Subscribe(_ =>
            {
                int mSiblingIndex = -1;
                int index = -1;
                List<string> Keys = new List<string>(UIManager.Instance.mAllUI.Keys);
                for (int i = 0; i < UIManager.Instance.mAllUI.Count; i++)
                {
                   int siblingIndex =  UIManager.Instance.mAllUI[Keys[i]].Transform.GetSiblingIndex();
                   if (siblingIndex > mSiblingIndex)
                   {
                       mSiblingIndex = siblingIndex;
                       index = i;
                   }
                }
                if (index != -1)
                {
                    if ("MainPanel".Equals(Keys[index]))
                    {
                        Application.Quit();
                    } //BookDayListPanel
                    else if ("LoginPanel".Equals(Keys[index]) 
                             || "UpdateTipPanel".Equals(Keys[index]) 
                             || "TipPanel".Equals(Keys[index]) 
                             || "MessageMaskPanel".Equals(Keys[index]) 
                             || "MainGuidePanel".Equals(Keys[index]) 
                             || "MedalAnimationPanel".Equals(Keys[index]) 
                             || "BookDayListPanel".Equals(Keys[index]))
                    {
                        
                    }
                    else
                    {
                        UIManager.Instance.Back(Keys[index]);
                    }
                }
            });
        }

        public static string BaseUrl { get; set; }
        public static int IsFirstInitialize { get; set; }
        public static string IPhonePlayerCompressStatus { get; set; }

        public void FirstInitialize(int isFirstInitialize)
        {
            IsFirstInitialize = isFirstInitialize;
        }
        /**
         * 设置baseurl
         */
        public void UrlConfig(string url)
        {
            Log.I("UrlConfig: " + url);
            BaseUrl = url;
            if (PlayerPrefsUtil.GetPhone().IsNotNullAndEmpty() && PlayerPrefsUtil.GetPwd().IsNotNullAndEmpty())
            {
                StartResquestForDoLogin(PlayerPrefsUtil.GetPhone(), PlayerPrefsUtil.GetPwd());
            }
            else
            {
                UIMgr.OpenPanel<LoginPanel>(new LoginPanelData());
            }
        }

        public void IPhonePlayerVideoCompress(string msg)
        {
            Debug.Log("IOS调 unity ["+ msg + "]");
            IPhonePlayerCompressStatus = msg;
        }

        private void Start()
        {
            IsFirstInitialize = 1;
            IPhonePlayerCompressStatus = String.Empty;
            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
            {
                UrlConfig(UrlConst.BaseUrlDebug);
            }
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                FirstInitialize(0);
            }
        } 
        
        void StartResquestForDoLogin(string phone, string pwd)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("mobile", phone);
            paramDict.Add("passwd", pwd);
            HttpUtil.PostWithSign<UserInfoModel>(UrlConst.Login, paramDict)
                .Subscribe(response =>
                    {
                        PlayerPrefsUtil.UserInfo = response;
                        if (response.babyInfoVo.IsNull())
                        {
                            // BabyInfoActivity
                            UIMgr.OpenPanel<LoginPanel>(new LoginPanelData());
                        } 
                        else if (response.babyRelation.IsNullOrEmpty())
                        {
                            // BabyRelationShipActivity
                            UIMgr.OpenPanel<LoginPanel>(new LoginPanelData());
                        }
                        else
                        {
                            UIMgr.OpenPanel<MainPanel>(new MainPanelData());
                        }
                    }
                    , e =>
                    {
                        if (e is HttpException)
                        {
                            HttpException httpException = e as HttpException;
                            ToastManager.GetInstance().CreatToast(httpException.Message);
                            UIMgr.OpenPanel<LoginPanel>(new LoginPanelData(), UITransitionType.CLOUD);
                        }
                    }).AddTo(this);
        }
        
        

        /**
         * 设备绑定配网
         * {deviceId,"xx",state:"1"}
         */
        public void BindDevice(string msg)
        {
            Log.I("BindDevice: " + msg);
            BindDeviceModel model = SerializeHelper.FromJson<BindDeviceModel>(msg);
            
            if (model != null)
            {
                SimpleEventSystem.Publish(new BindDeviceResult(model));
            }
        }
        
        
        public void AudioToBind()
        {
            Log.I("AudioToBind");
            UIMgr.CloseAllPanel();
            UIMgr.OpenPanel<BindConfirmBootPanel>(new BindConfirmBootPanelData()
            {
                isFromAudio = true
            });
        }
        
        public void ScanQR(string msg)
        {
            Log.I("ScanQR: " + msg);
            if (msg.IsNotNullAndEmpty())
            {
                SimpleEventSystem.Publish(new ScanQRResult(msg));

            }
        }
        
        
        public void UpdateVol(string msg)
        {
            Log.I("UpdateVol: " + msg);
            if (msg.IsNotNullAndEmpty())
            {
                SimpleEventSystem.Publish(new UpdateVol(msg));

            }
        }

        public void PauseMusic(string msg)
        {
            if (AudioManager.IsMusicOn)
            {
                Log.I("PauseMusic");
                isMusicNeedOn = true;
                AudioManager.Off();
            }
        }
        
        
        public void ResumeMusic(string msg)
        {
            if (isMusicNeedOn)
            {
                Log.I("ResumeMusic");
                isMusicNeedOn = false;
                AudioManager.On();
            }
        }

        public void IPhoneGetWiFiSSIDResult(string msg)
        {
            SimpleEventSystem.Publish(new SSIDResult(msg));
        }
        
        
        public void IPhoneFinishLottie(string msg)
        {
            FirstInitialize(1);
            SimpleEventSystem.Publish(new LottieAnimationFinish());
        }
        
        /**
         * app通知unity网络请求错误
         */
        public void HttpError(string json)
        {
            Log.I("HttpError： " + json);
            BaseEntity<string> httpError = JsonConvert.DeserializeObject<BaseEntity<string>>(json);
            CommonUtil.error(httpError.errCode, httpError.message, httpError.status);
            CommonUtil.toast(httpError.message);
        }
        
        /**
         * 视频压缩
         */
        public void VideoCompress(string json)
        {
            Log.I("VideoCompress: " + json);
            VideoCompressModel model = SerializeHelper.FromJson<VideoCompressModel>(json);
            if (model != null)
            {
                SimpleEventSystem.Publish(new VideoCompressResult(model));
            }
        }
        
        
        /**
         * 重新播放音频
         */
        public void ReloadPlayMusic(string json)
        {
            MicrophoneManager.GetInstance().StartRecord();
            MicrophoneManager.GetInstance().PauseRecord();
            Debug.Log("ReloadPlayMusic" + " - END"); 
        }
        
        /**
         * 分享回调 1成功2失败3取消
         */
        public void ShareResult(string status)
        {
            Log.I("ShareResult: " + status);
            SimpleEventSystem.Publish(new ShareResult(status));
        }
    }
}