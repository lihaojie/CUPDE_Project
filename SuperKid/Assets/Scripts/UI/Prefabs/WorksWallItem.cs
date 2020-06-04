using System;
using System.Collections;
using EnhancedUI.EnhancedScroller;
using QFramework;
using SuperKid;
using SuperKid.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorksWallItem : EnhancedScrollerCellView
{
    private Image ImageUserIcon;
    private Image ImageThumIcon;
    private Image ImagePunchType;
    private Image ImgAudio;
    private Image ImgPlayState;

    private Text TextName;
    private Text TextTimeLine;
    private Text TextShareContent;
    private Text TextAge;
    private RectTransform MidPanelTransform;
    private Text TextReply;
    private Text TextReplyContent;
    private Text TextTime;
    private GameObject AudioContent;
    private GameObject ImgSharePreview;
    private GameObject ImgAudioPreview;
    private GameObject Line;
    private Button BtnPlayState;
    private Button BtnShare;
    private Button BtnAudioPreview;
    private Button BtnImgPreview;

    private AudioSource AudioContentAudioSource;
    private AudioSource AudioPreviewAudioSource;
    private BoolReactiveProperty IsAudioPreviewPlaying = new BoolReactiveProperty(false);
    private BoolReactiveProperty IsAudioContenPlaying = new BoolReactiveProperty(false);
    public IntReactiveProperty AudioContenPlayingTime = new IntReactiveProperty();
    public IntReactiveProperty AudioContenTotleTime = new IntReactiveProperty();
    private ResLoader mResLoader = ResLoader.Allocate();

    private void Awake()
    {
        ImageUserIcon = transform.Find("ImgUserIconBG/ImgUserIconMask/ImageUserIcon").GetComponent<Image>();
        TextName = transform.Find("MidPanel/UserNameContent/TextName").GetComponent<Text>();
        TextAge = transform.Find("MidPanel/UserNameContent/TextAge").GetComponent<Text>();
        TextTimeLine = transform.Find("MidPanel/TextTimeLine").GetComponent<Text>();
        TextShareContent = transform.Find("MidPanel/TextShareContent").GetComponent<Text>();
        MidPanelTransform = transform.Find("MidPanel").GetComponent<RectTransform>();
        TextReply = transform.Find("MidPanel/TextReply").GetComponent<Text>();
        Line = transform.Find("MidPanel/Line").gameObject;
        TextReplyContent = transform.Find("MidPanel/TextReplyContent").GetComponent<Text>();
        AudioContent = transform.Find("MidPanel/AudioContent").gameObject;
        AudioContentAudioSource = transform.Find("MidPanel/AudioContent").GetComponent<AudioSource>();
        TextTime = transform.Find("MidPanel/AudioContent/TextTime").GetComponent<Text>();
        ImgPlayState = transform.Find("MidPanel/AudioContent/Image/ImgPlayState").GetComponent<Image>();
        BtnPlayState = transform.Find("MidPanel/AudioContent/Image/BtnPlayState").GetComponent<Button>();
        BtnShare = transform.Find("BtnShare").GetComponent<Button>();
        ImageThumIcon = transform.Find("ImgSharePreview/ImgUserIconMask/ImageThumIcon").GetComponent<Image>();
        ImagePunchType = transform.Find("ImgSharePreview/ImgUserIconMask/ImagePunchType").GetComponent<Image>();
        ImgSharePreview = transform.Find("ImgSharePreview").gameObject;
        BtnImgPreview = transform.Find("ImgSharePreview").GetComponent<Button>();
        ImgAudioPreview = transform.Find("ImgAudioPreview").gameObject;
        BtnAudioPreview = transform.Find("ImgAudioPreview").GetComponent<Button>();
        AudioPreviewAudioSource = transform.Find("ImgAudioPreview").GetComponent<AudioSource>();
        ImgAudio = transform.Find("ImgAudioPreview/ImgAudio").GetComponent<Image>();
    }

    // Update is called once per frame
    public void SetData(WorksWall data)
    {
        float workWallItemHeight = 0;
        if (data.babyId == PlayerPrefsUtil.GetBabyId())
        {
            BtnShare.gameObject.SetActive(true);
        }
        else
        {
            BtnShare.gameObject.SetActive(false);
        }
        // 分享事件
        BtnShare.onClick.AddListener(() =>
        {
            AttendanceModel model = new AttendanceModel();
            model.boxDay = data.boxDay;
            model.punchPath = data.punchPathUrl;
            model.punchText = data.punchText;
            model.punchType = data.punchType;
            model.shareScore = data.shareScore;
            model.finishPunchNum = data.finishPunchNum;
            model.relBoxId = data.relBoxId;
            model.id = data.id;
            model.subject = data.subject;
            UIMgr.OpenPanel<AttendanceSharePanel>(new AttendanceSharePanelData()
            {
                AttendanceModel = model,
                isWall = true
            }, UITransitionType.CIRCLE);
        });
        // 查看我的打卡 图片or 视频
        BtnImgPreview.onClick.AddListener(() =>
        {
            if (data.punchType == 0) //图文
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    AndroidForUnity.CallAndroidForShowPic(data.punchPathUrl);
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    UIMgr.OpenPanel<PhotoBrowserPanel>(new PhotoBrowserPanelData()
                    {
                        ImageUrl = data.punchPathUrl,
                    },UITransitionType.NULL);
                }
            }
            else if (data.punchType == 2) //视频
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    AndroidForUnity.CallAndroidForPlayVideo(data.punchPathUrl);
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    Dictionary<string, object> paramDic = new Dictionary<string, object>();
                    Dictionary<string, object> subParam = new Dictionary<string, object>();
                    subParam.Add("url",data.punchPathUrl);
                    paramDic.Add("target", AppConst.VIDEO_IOS);
                    paramDic.Add("method",IOSClientUtil.VideoMediaPlay);
                    paramDic.Add("params",subParam);
                    IOSClientUtil.CommonMethodCallIOSClient(paramDic.ToJson());
                }
            }
        });
        TextShareContent.text = StringUtil.Emoji(data.punchText);
        TextAge.text = data.age;
        TextName.text = StringUtil.Emoji(data.name);
        TextTimeLine.text = data.createTime;
        IsAudioPreviewPlaying.Subscribe(isPlaying =>
        {
            Texture2D soundOnTexture = mResLoader.LoadSync<Texture2D>(isPlaying ? "ic_works_wall_audio_pause" : "ic_works_wall_audio_start");
            ImgAudio.sprite = Sprite.Create(soundOnTexture, new Rect(0, 0, soundOnTexture.width, soundOnTexture.height), Vector2.one * 0.5f);
        }).AddTo(this);
        IsAudioContenPlaying.Subscribe(isPlaying =>
        {
            Texture2D soundOnTexture = mResLoader.LoadSync<Texture2D>(isPlaying ? "ic_audio_playing" : "ic_audiio_pause");
            ImgPlayState.sprite = Sprite.Create(soundOnTexture, new Rect(0, 0, soundOnTexture.width, soundOnTexture.height), Vector2.one * 0.5f);
        }).AddTo(this);
        AudioContenPlayingTime.Subscribe(time =>
        {
            if (time == 0)
            {
                TextTime.text = AudioContenTotleTime.Value + "\"";
            }
            else
            {
                TextTime.text = time +"\"" + "/" +AudioContenTotleTime.Value + "\"";
            }
        }).AddTo(this);

        // 用户头像,没有传头像 使用默认的
        Texture2D mTexture2DHBoy = mResLoader.LoadSync<Texture2D>("ic_head_boy");
        Texture2D mTexture2DHGirl = mResLoader.LoadSync<Texture2D>("ic_head_girl");
        if (PlayerPrefsUtil.UserInfo.babyInfoVo.babyLogoUrl.IsNotNullAndEmpty())
        {
            ImageDownloadUtils.Instance.SetAsyncImage(data.logoRelativePathUrl, ImageUserIcon);
        } 
        else if (PlayerPrefsUtil.UserInfo.babyInfoVo.sex == 1)
        {
            ImageUserIcon.sprite = Sprite.Create(mTexture2DHBoy,
                new Rect(0, 0, mTexture2DHBoy.width, mTexture2DHBoy.height), Vector2.one * 0.5f);
        }
        else if (PlayerPrefsUtil.UserInfo.babyInfoVo.sex == 2)
        {
            ImageUserIcon.sprite = Sprite.Create(mTexture2DHGirl,
                new Rect(0, 0, mTexture2DHGirl.width, mTexture2DHGirl.height), Vector2.one * 0.5f);
        }
        // 0-图文打卡 1-音频打卡 2-视频打卡
        if (data.punchType == 0)
        {
            ImgSharePreview.SetActive(true);
            ImgAudioPreview.SetActive(false);
            ImageDownloadUtils.Instance.SetAsyncImage(data.punchPathUrl, ImageThumIcon);
            ImagePunchType.gameObject.SetActive(false);
        }
        else if (data.punchType == 1)
        {
            ImgSharePreview.SetActive(false);
            ImgAudioPreview.SetActive(true);
            BtnAudioPreview.onClick.AddListener(() =>
            {
                if (AudioContentAudioSource.isPlaying)
                {
                    AudioContentAudioSource.Pause();
                }
                Log.I("data.punchPathUrl==" + data.punchPathUrl);
                if (AudioPreviewAudioSource.isPlaying)
                {
                    AudioPreviewAudioSource.Pause();
                }
                else
                {
                    StartCoroutine(SetAudioClip(data.punchPathUrl, AudioPreviewAudioSource));
                }
            });
            ImagePunchType.gameObject.SetActive(false);
        }
        else if (data.punchType == 2)
        {
            ImgSharePreview.SetActive(true);
            ImgAudioPreview.SetActive(false);
            ImageDownloadUtils.Instance.SetAsyncImage(data.thumbnailPath, ImageThumIcon);
            ImagePunchType.gameObject.SetActive(true);
        }

        if (data.comments.IsNotNull() && data.comments.Count > 0)
        {
            Line.SetActive(true);
            TextReply.gameObject.SetActive(true);
            TextReplyContent.gameObject.SetActive(data.comments[0].commentContent.IsNotNullAndEmpty());
            TextReplyContent.text = StringUtil.Emoji(data.comments[0].commentContent);
            if (data.comments[0].commentPathUrl.IsNotNullAndEmpty())
            {
                AudioContent.SetActive(true);
                TextTime.text = data.comments[0].duration / 1000 + "\"";
                BtnPlayState.onClick.AddListener(() =>
                {
                    if (AudioPreviewAudioSource.isPlaying)
                    {
                        AudioPreviewAudioSource.Pause();
                    }
                    
                    if (AudioContentAudioSource.isPlaying)
                    {
                        AudioContentAudioSource.Pause();
                    }
                    else
                    {
                        StartCoroutine(SetAudioClip(data.comments[0].commentPathUrl, AudioContentAudioSource));
                    }
                });
            }
            else
            {
                AudioContent.SetActive(false);
            }
        }
        else
        {
            Line.SetActive(false);
            TextReply.gameObject.SetActive(false);
            TextReplyContent.gameObject.SetActive(false);
            AudioContent.SetActive(false);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(MidPanelTransform); 
        int height = (int) (Mathf.Abs(MidPanelTransform.rect.height)+168);
        Debug.Log("MidPanelTransform 的高度 是 "+ MidPanelTransform.rect.height);
        Debug.Log("TextShareContent 的高度 是 "+ (TextShareContent.rectTransform.rect.height));
        workWallItemHeight += TextName.rectTransform.rect.height; // 名称
        workWallItemHeight += 10; // spacing
        workWallItemHeight += TextTimeLine.rectTransform.rect.height; // 发布时间
        workWallItemHeight += 10; // spacing
        workWallItemHeight += TextShareContent.rectTransform.rect.height; // 发布的内容
        workWallItemHeight += 10; // spacing
        if (data.comments.Count > 0)
        { 
            workWallItemHeight += TextReply.rectTransform.rect.height; // 老师回复
            workWallItemHeight += 10; // spacing
            workWallItemHeight += TextReplyContent.rectTransform.rect.height; // 点评的内容
            if (data.comments[0].commentPathUrl.IsNotNullAndEmpty())
            { 
                workWallItemHeight += 44; // 回复音频高度
                workWallItemHeight += 10; // spacing
            }
            
        }
        workWallItemHeight += (34 + 60); // spacing
        
        if (workWallItemHeight < 312)
        {
            workWallItemHeight = 312;
        }

        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(940, workWallItemHeight);
    }

    private void Update()
    {
        IsAudioContenPlaying.Value = AudioContentAudioSource.isPlaying;
        IsAudioPreviewPlaying.Value = AudioPreviewAudioSource.isPlaying;
        if (AudioContentAudioSource.isPlaying)
        {
            AudioContenPlayingTime.Value = (int)AudioContentAudioSource.time;
            AudioContenTotleTime.Value = (int)AudioContentAudioSource.clip.length;
        }
        else
        {
            AudioContenPlayingTime.Value = 0;
        }
    }

    IEnumerator SetAudioClip(string path, AudioSource audioSource, int time = 0)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.UNKNOWN))
        {
            //不卡顿的2行代码
            ((DownloadHandlerAudioClip) uwr.downloadHandler).compressed = false;
            ((DownloadHandlerAudioClip) uwr.downloadHandler).streamAudio = true;
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
            {
                Log.E(uwr.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                audioSource.clip = clip;
                audioSource.time = time;
                audioSource.Play(); //播放
            }
        }
    }
}