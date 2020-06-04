//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using QFramework;
using SuperKid.Utils;
using UniRx;

namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    
    
    public class AttendanceMainPanelData : QFramework.UIPanelData
    {
        public int BoxId;
        public int BoxDay;
    }
    
    public partial class AttendanceMainPanel : QFramework.UIPanel
    {
        
        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {
            throw new System.NotImplementedException ();
        }
        
        protected override void OnInit(QFramework.IUIData uiData)
        { 
            mData = uiData as AttendanceMainPanelData ?? new AttendanceMainPanelData();
            BtnBack.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                Back();
            });
            BtnPicture.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<ChoosePhotoPanel>(new ChoosePhotoPanelData()
                {
                    action = ChoosePhotoAction.AttendancePic,
                });
            });
            BtnAudio.onClick.AddListener(() =>
                {
                    AudioManager.PlaySound("Button_Audio");
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        NativeGallery.RequestPermission((result, action) =>
                        {
                            if (result == (int) NativeGallery.Permission.Granted)
                            {
                                UIMgr.OpenPanel<AttendanceAudioRecordPanel>(new AttendanceAudioRecordPanelData(), UITransitionType.CIRCLE);
                            }
                        }, (int) NativeAction.Audio);
                    }
                    else
                    {
                        UIMgr.OpenPanel<AttendanceAudioRecordPanel>(new AttendanceAudioRecordPanelData(), UITransitionType.CIRCLE);
                    }
                })
                ;
            BtnVideo.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                // UIMgr.OpenPanel<ChoosePhotoPanel>(new ChoosePhotoPanelData()
                // {
                //     action = ChoosePhotoAction.AttendanceVideo,
                //     showTip = true
                // });
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    IPhonePlayerMakeVideoOpenCamera();
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    NativeGallery.RequestPermission((result, action) =>
                    {
                        if (result == (int) NativeGallery.Permission.Granted)
                        {
                            NativeGallery.GetVideoFromGalleryForAndroid( ( json ) =>
                            {
                                if (json.IsNotNullAndEmpty())
                                {
                                    NativeVideoModel model = SerializeHelper.FromJson<NativeVideoModel>(json);
                                    if (model.IsNotNull())
                                    {
                                        if (model.status == 1)
                                        {
                                            UIMgr.OpenPanel<AttendanceAddPanel>(new AttendanceAddPanelData()
                                            {
                                                BoxId = mData.BoxId,
                                                BoxDay = mData.BoxDay,
                                                Path = model.videoPath,
                                                ThumbnailPath = model.imagePath,
                                                Action = AttendanceAdd.Video,
                                            }, UITransitionType.CIRCLE);
                                            Close();
                                        }
                                    }
                                }
                            }, "选择视频", "video/*", action);
                        }
                    }, (int) NativeAction.Camera);
                }
            });
            SimpleEventSystem.GetEvent<RecordPath>().Subscribe(Path =>
            {
                UIMgr.OpenPanel<AttendanceAddPanel>(new AttendanceAddPanelData()
                {
                    BoxId = mData.BoxId,
                    BoxDay = mData.BoxDay,
                    Action = AttendanceAdd.Audio,
                    Path = Path.Path
                }, UITransitionType.CIRCLE);
                Close();
            }).AddTo(this);
            SimpleEventSystem.GetEvent<ChoosePhotoClick>()
                .Subscribe(_ =>
                {
                    if (_.GetPhotoAction == ChoosePhotoAction.AttendancePic)
                    {
                        if (Application.platform == RuntimePlatform.IPhonePlayer)
                        {
                            if (_.GetAction == NativeAction.Camera)
                            {
                                IPhonePlayerSelectPictureOpenCamera();
                            }
                            else if (_.GetAction == NativeAction.Album)
                            {
                                IPhonePlayerSelectPictureOpenAlbum();
                            }
                            else
                            {
                                
                            }
                        }
                        else if (Application.platform == RuntimePlatform.Android)
                        {
                            NativeGallery.RequestPermission((result, action) =>
                            {
                                if (result == (int) NativeGallery.Permission.Granted)
                                {
                                    NativeGallery.GetImageFromGalleryForAndroid( ( path ) =>
                                    {
                                        if (path.IsNotNullAndEmpty())
                                        {
                                            UIMgr.OpenPanel<AttendanceAddPanel>(new AttendanceAddPanelData()
                                            {
                                                BoxId = mData.BoxId,
                                                BoxDay = mData.BoxDay,
                                                Path = path,
                                                ThumbnailPath = path,
                                                Action = AttendanceAdd.Pic
                                            }, UITransitionType.CIRCLE);
                                            Close();
                                        }
                                        Debug.Log( "Image path: " + path );
                                    }, "选择图片", "image/*", true, action);
                                }
                            }, (int) _.GetAction);
                        }
                    }
                    // 视频不需要了
                    else if (_.GetPhotoAction == ChoosePhotoAction.AttendanceVideo)
                    {
                        if (Application.platform == RuntimePlatform.IPhonePlayer)
                        {
                            if (_.GetAction == NativeAction.Album)
                            {
                                IPhonePlayerSelectVideoOpenAlbum();
                            }
                            else if (_.GetAction == NativeAction.Camera)
                            {
                                IPhonePlayerMakeVideoOpenCamera();
                            }
                        }
                        else if (Application.platform == RuntimePlatform.Android)
                        {
                            NativeGallery.RequestPermission((result, action) =>
                            {
                                if (result == (int) NativeGallery.Permission.Granted)
                                {
                                    NativeGallery.GetVideoFromGalleryForAndroid( ( json ) =>
                                    {
                                        if (json.IsNotNullAndEmpty())
                                        {
                                            NativeVideoModel model = SerializeHelper.FromJson<NativeVideoModel>(json);
                                            if (model.IsNotNull())
                                            {
                                                if (model.status == 1)
                                                {
                                                    UIMgr.OpenPanel<AttendanceAddPanel>(new AttendanceAddPanelData()
                                                    {
                                                        BoxId = mData.BoxId,
                                                        BoxDay = mData.BoxDay,
                                                        Path = model.videoPath,
                                                        ThumbnailPath = model.imagePath,
                                                        Action = AttendanceAdd.Video,
                                                    }, UITransitionType.CIRCLE);
                                                    Close();
                                                }
                                            }
                                        }
                                    }, "选择视频", "video/*", action);
                                }
                            }, (int) _.GetAction);
                        }
                    }
                }).AddTo(this);
        }

        /**
         * 打开原生相册 选择照片
         */
        private void IPhonePlayerSelectPictureOpenAlbum()
        {
            NativeGallery.Permission rest = NativeGallery.RequestIPhonePermission(1);
            if (rest == NativeGallery.Permission.Granted)
            {
                NativeGallery.GetImageFromGallery((backPath) =>
                {
                    if (backPath.IsNotNullAndEmpty())
                    {
                        // 打开原生图片选择
                        UIMgr.OpenPanel<AttendanceAddPanel>(new AttendanceAddPanelData()
                        {
                            BoxId = mData.BoxId,
                            BoxDay = mData.BoxDay,
                            Path = backPath,
                            ThumbnailPath = backPath,
                            Action = AttendanceAdd.Pic
                        }, UITransitionType.CIRCLE);
                        Close();
                    }
                }, "选择图片", "image/*");
            }
        }

        /**
         * 打开原生相机拍照
         */
        private void IPhonePlayerSelectPictureOpenCamera()
        {
            NativeGallery.Permission rest = NativeGallery.RequestIPhonePermission(4);
            if (rest == NativeGallery.Permission.Granted)
            {
                NativeGallery.GetIPhoneCameraImageFromGallery((backPath) =>
                {
                    if (backPath.IsNotNullAndEmpty())
                    {
                        // 打开原生图片选择
                        UIMgr.OpenPanel<AttendanceAddPanel>(new AttendanceAddPanelData()
                        {
                            BoxId = mData.BoxId,
                            BoxDay = mData.BoxDay,
                            Path = backPath,
                            ThumbnailPath = backPath,
                            Action = AttendanceAdd.Pic
                        }, UITransitionType.CIRCLE);
                        Close();
                    }
                }, "选择图片", "image/*");
            }
        }
        
        /**
         * 打开原生相册选择 视频
         */
        private void IPhonePlayerSelectVideoOpenAlbum()
        {
            NativeGallery.Permission rest1 = NativeGallery.RequestIPhonePermission(4);
            NativeGallery.Permission rest2 = NativeGallery.RequestIPhonePermission(3);
            if (rest1 == NativeGallery.Permission.Granted)
            {
                NativeGallery.GetVideoFromGallery((backPath) =>
                {
                    if (backPath.IsNotNullAndEmpty())
                    {
                        // 打开原生视频选择
                        UIMgr.OpenPanel<AttendanceAddPanel>(new AttendanceAddPanelData()
                        {
                            BoxId = mData.BoxId,
                            BoxDay = mData.BoxDay,
                            Path = backPath,
                            ThumbnailPath = Application.temporaryCachePath + "/videoThumbnail.png",
                            Action = AttendanceAdd.Video,
                        }, UITransitionType.CIRCLE);
                        Close();
                    }
                                    
                },"选择视频","video/*");
            }
        }
        /**
         * 打开原生相机录制视频
         */
        private void IPhonePlayerMakeVideoOpenCamera()
        {
            NativeGallery.Permission rest1 = NativeGallery.RequestIPhonePermission(4);
            NativeGallery.Permission rest2 = NativeGallery.RequestIPhonePermission(3);
            if (rest1 == NativeGallery.Permission.Granted)
            {
                NativeGallery.IPhoneCameraMakeVideoFromGallery(backPath =>
                {
                    if (backPath.IsNotNullAndEmpty())
                    {
                        // 打开原生视频选择
                        UIMgr.OpenPanel<AttendanceAddPanel>(new AttendanceAddPanelData()
                        {
                            BoxId = mData.BoxId,
                            BoxDay = mData.BoxDay,
                            Path = backPath,
                            ThumbnailPath = Application.temporaryCachePath + "/videoThumbnail.png",
                            Action = AttendanceAdd.Video,
                        }, UITransitionType.CIRCLE);
                        Close();
                    }
                    
                }, "选择视频", "video/*");
            }
        }
        
        protected override void OnOpen(QFramework.IUIData uiData)
        {
        }
        
        protected override void OnShow()
        {
        }
        
        protected override void OnHide()
        {
        }
        
        protected override void OnClose()
        {
        }
    }
}
