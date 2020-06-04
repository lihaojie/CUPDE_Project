/****************************************************************************
 * Copyright (c) 2017 xiaojun
 * Copyright (c) 2017 imagicbell
 * Copyright (c) 2018.5 ~ 8  liangxie
 * 
 * http://qframework.io
 * https://github.com/liangxiegame/QFramework
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using DG.Tweening;
using UniRx;
using UnityEngine.Events;

namespace QFramework
{
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using System;

#if SLUA_SUPPORT
	using SLua;
#endif
    public enum UILevel
    {
        AlwayBottom = -3, //如果不想区分太复杂那最底层的UI请使用这个
        Bg = -2, //背景层UI
        AnimationUnderPage = -1, //动画层
        Common = 0, //普通层UI
        AnimationOnPage = 1, // 动画层
        PopUI = 2, //弹出层UI
        Guide = 3, //新手引导层
        Const = 4, //持续存在层UI
        Toast = 5, //对话框层UI
        Forward = 6, //最高UI层用来放置UI特效和模型
        AlwayTop = 7, //如果不想区分太复杂那最上层的UI请使用这个
    }

#if SLUA_SUPPORT
	[CustomLuaClass]
#endif
    //// <summary>
    /// <inheritdoc />
    /// <![CDATA[The 'member' start tag on line 2 position 2 does not match the end tag of 'summary'. Line 3, position 3.]]>
    [MonoSingletonPath("UIRoot")]
    public class UIManager : QMgrBehaviour, ISingleton
    {
        public Dictionary<string, IPanel> mAllUI = new Dictionary<string, IPanel>();

        [SerializeField] private Transform mBgTrans;
        [SerializeField] private Transform mAnimationUnderTrans;
        [SerializeField] private Transform mCommonTrans;
        [SerializeField] private Transform mAnimationOnTrans;
        [SerializeField] private Transform mPopUITrans;
        [SerializeField] private Transform mConstTrans;
        [SerializeField] private Transform mToastTrans;
        [SerializeField] private Transform mForwardTrans;

        [SerializeField] private Camera mUICamera;
        [SerializeField] private Canvas mCanvas;
        [SerializeField] private CanvasScaler mCanvasScaler;
        [SerializeField] private CloudAnimationController mCloudAnimationController;
        [SerializeField] private Image mCircleMask;
        [SerializeField] private GraphicRaycaster mGraphicRaycaster;

        public Stack<UIPanelInfo> mUIStack = new Stack<UIPanelInfo>();

        private bool isPushingView;

        public bool mIsChangingView;
        void ISingleton.OnSingletonInit()
        {
        }

        private static UIManager mInstance;

        public static UIManager Instance
        {
            get
            {
                if (null == mInstance)
                {
                    mInstance = FindObjectOfType<UIManager>();
                }

                if (null == mInstance)
                {
                    Instantiate(Resources.Load<GameObject>("UIRoot"));
                    mInstance = MonoSingletonProperty<UIManager>.Instance;
                    mInstance.name = "UIRoot";
                    DontDestroyOnLoad(mInstance);
                }

                return mInstance;
            }
        }

        public Canvas RootCanvas
        {
            get { return mCanvas; }
        }

        public CloudAnimationController CloudAnimationController
        {
            get { return mCloudAnimationController; }
        }

        public Camera UICamera
        {
            get { return mUICamera; }
        }

        public void SetResolution(int width, int height)
        {
            mCanvasScaler.referenceResolution = new UnityEngine.Vector2(width, height);
        }

        public Vector2 GetResolution()
        {
            return mCanvasScaler.referenceResolution;
        }

        public CanvasScaler GetCanvasScaler()
        {
            return mCanvasScaler;
        }

        public void SetMatchOnWidthOrHeight(float heightPercent)
        {
            mCanvasScaler.matchWidthOrHeight = heightPercent;
        }

        public float GetMatchOnWithOrHeight()
        {
            return mCanvasScaler.matchWidthOrHeight;
        }

        public IPanel OpenUI(string uiBehaviourName, UITransitionType transitionType, UILevel canvasLevel, 
            IUIData uiData = null, string assetBundleName = null, bool CanOpenPrevious = true)
        {
            IPanel retPanel = null;

            if (!mAllUI.TryGetValue(uiBehaviourName, out retPanel))
            {
                retPanel = CreateUI(uiBehaviourName, transitionType, canvasLevel, uiData, assetBundleName, CanOpenPrevious);
            }

            retPanel.Open(uiData);
            retPanel.Show();
            return retPanel;
        }

        /// <summary>
        /// 显示UIBehaiviour
        /// </summary>
        /// <param name="uiBehaviourName"></param>
        public void ShowUI(string uiBehaviourName)
        {
            IPanel iuiPanel = null;
            if (mAllUI.TryGetValue(uiBehaviourName, out iuiPanel))
            {
                iuiPanel.Show();
            }
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <param name="uiBehaviourName"></param>
        public void HideUI(string uiBehaviourName)
        {
            IPanel iuiPanel = null;
            if (mAllUI.TryGetValue(uiBehaviourName, out iuiPanel))
            {
                iuiPanel.Hide();
            }
        }
        /// <summary>
        /// 删除所有UI层
        /// </summary>
        public void CloseAllUI()
        {
            foreach (var layer in mAllUI)
            {
                layer.Value.Close();
                Destroy(layer.Value.Transform.gameObject);
            }

            mAllUI.Clear();
        }

        /// <summary>
        /// 隐藏所有 UI
        /// </summary>
        public void HideAllUI()
        {
            mAllUI.ForEach(keyValuePair => keyValuePair.Value.Hide());
        }

        /// <summary>
        /// 关闭并卸载UI
        /// </summary>
        /// <param name="behaviourName"></param>
        /// <param name="withTransition"></param>
        public void CloseUI(string behaviourName)
        {
            IPanel behaviour = null;
            mAllUI.TryGetValue(behaviourName, out behaviour);

            if ((behaviour as UIPanel))
            {
                behaviour.Close();
                mAllUI.Remove(behaviourName);
            }
        }

        public void CloudCloseAnimation()
        {
            mIsChangingView = false;
            mCloudAnimationController.transform.parent.Hide();
        }

        public void Push<T>() where T : UIPanel
        {
            Push(GetUI<T>());
        }

        public void Push(IPanel view,UITransitionType transitionType = UITransitionType.NULL)
        {
            if (view != null)
            {
                if (transitionType == UITransitionType.CIRCLE)
                {
                    if (!isPushingView)
                    {
                        isPushingView = true;
                        IDisposable subscribe = null;
                        subscribe = Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(_ =>
                        {
                            isPushingView = false;
                            mUIStack.Push(view.PanelInfo);
                            view.Close();
                            mAllUI.Remove(view.Transform.name);
                            subscribe.Dispose();
                        });
                    }
                }else if (transitionType == UITransitionType.CLOUD)
                {
                    if (!isPushingView)
                    {
                        isPushingView = true;
                        IDisposable subscribe = null;
                        subscribe = SimpleEventSystem.GetEvent<CloudOpenAnimation>().Subscribe(_ =>
                        {
                            isPushingView = false;
                            mUIStack.Push(view.PanelInfo);
                            view.Close();
                            mAllUI.Remove(view.Transform.name);
                            subscribe.Dispose();
                        });
                    }
                }
                else if (transitionType == UITransitionType.NULL)
                {
                    mUIStack.Push(view.PanelInfo);
                    view.Close();
                    mAllUI.Remove(view.Transform.name);
                }
            }
        }


        public void Back(string currentPanelName)
        {
            IPanel behaviour = null;
            mAllUI.TryGetValue(currentPanelName, out behaviour);
            if ((behaviour as UIPanel))
            {
                if (behaviour.PanelInfo.TransitionType == UITransitionType.CIRCLE)
                {
                    if (!mIsChangingView)
                    {
                        mIsChangingView = true;
                        mCircleMask.transform.parent.gameObject.Show();
                        var tweenerCore = mCircleMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
                        mCircleMask.DOFade(1, 0.5f);
                        tweenerCore.onComplete = delegate()
                        {
                            if (behaviour.PanelInfo.CanOpenPrevious)
                            {
                                var previousPanelInfo = mUIStack.Pop();
                                OpenUI(previousPanelInfo.PanelName,previousPanelInfo.TransitionType,
                                    previousPanelInfo.Level, previousPanelInfo.UIData,
                                    previousPanelInfo.AssetBundleName, previousPanelInfo.CanOpenPrevious);
                            }
                            CloseUI(currentPanelName);
                            var t = mCircleMask.DOFade(0, 0.5f).SetEase(Ease.Linear);
                            t.onComplete = delegate()
                            {
                                mCircleMask.transform.localScale = Vector3.zero;
                                mCircleMask.transform.parent.gameObject.Hide();
                                mIsChangingView = false;
                            };
                        };
                    }   
                }else if (behaviour.PanelInfo.TransitionType == UITransitionType.CLOUD)
                {
                    if (!mIsChangingView)
                    {
                        mIsChangingView = true;
                        mCloudAnimationController.transform.parent.Show();
                        mCloudAnimationController.PlayAnimation(CloudState.OPENANDCLOSE);
                        IDisposable subscribe = null;
                        subscribe = SimpleEventSystem.GetEvent<CloudOpenAnimation>().Subscribe(_ =>
                        {
                            if (behaviour.PanelInfo.CanOpenPrevious)
                            {
                                var previousPanelInfo = mUIStack.Pop();
                                OpenUI(previousPanelInfo.PanelName,previousPanelInfo.TransitionType,
                                    previousPanelInfo.Level, previousPanelInfo.UIData,
                                    previousPanelInfo.AssetBundleName, previousPanelInfo.CanOpenPrevious);
                            }
                            CloseUI(currentPanelName);
                            subscribe.Dispose();
                        });
                    }
                }
                else
                {
                    if (behaviour.PanelInfo.CanOpenPrevious)
                    {
                        var previousPanelInfo = mUIStack.Pop();
                        OpenUI(previousPanelInfo.PanelName,previousPanelInfo.TransitionType,
                            previousPanelInfo.Level, previousPanelInfo.UIData,
                            previousPanelInfo.AssetBundleName, previousPanelInfo.CanOpenPrevious);
                    }
                    CloseUI(currentPanelName);
                }
            }
        }


        /// <summary>
        /// 获取UIBehaviour
        /// </summary>
        /// <param name="uiBehaviourName"></param>
        /// <returns></returns>
        public UIPanel GetUI(string uiBehaviourName)
        {
            IPanel retIuiPanel = null;

            if (mAllUI.TryGetValue(uiBehaviourName, out retIuiPanel))
            {
                return retIuiPanel as UIPanel;
            }

            return null;
        }

        public override int ManagerId
        {
            get { return QMgrID.UI; }
        }

        /// <summary>
        /// 命名空间对应名字的缓存
        /// </summary>
        private Dictionary<string, string> mFullname4UIBehaviourName = new Dictionary<string, string>();

        private string GetUIBehaviourName<T>()
        {
            var fullBehaviourName = typeof(T).ToString();
            string retValue = null;

            if (mFullname4UIBehaviourName.TryGetValue(fullBehaviourName, out retValue))
            {
            }
            else
            {
                retValue = typeof(T).Name;
            }

            return retValue;
        }

        public IPanel CreateUI(string uiBehaviourName, UITransitionType transitionType, UILevel level = UILevel.Common, IUIData uiData = null,
            string assetBundleName = null, bool CanOpenPrevious = true)
        {
            IPanel ui;

            if (mAllUI.TryGetValue(uiBehaviourName, out ui))
            {
                return ui;
            }

            ui = UIPanel.Load(uiBehaviourName, assetBundleName);

            switch (level)
            {
                case UILevel.Bg:
                    ui.Transform.SetParent(mBgTrans);
                    break;
                case UILevel.AnimationUnderPage:
                    ui.Transform.SetParent(mAnimationUnderTrans);
                    break;
                case UILevel.Common:
                    ui.Transform.SetParent(mCommonTrans);
                    break;
                case UILevel.AnimationOnPage:
                    ui.Transform.SetParent(mAnimationOnTrans);
                    break;
                case UILevel.PopUI:
                    ui.Transform.SetParent(mPopUITrans);
                    break;
                case UILevel.Const:
                    ui.Transform.SetParent(mConstTrans);
                    break;
                case UILevel.Toast:
                    ui.Transform.SetParent(mToastTrans);
                    break;
                case UILevel.Forward:
                    ui.Transform.SetParent(mForwardTrans);
                    break;
            }

            var uiGoRectTrans = ui.Transform as RectTransform;

            uiGoRectTrans.offsetMin = Vector2.zero;
            uiGoRectTrans.offsetMax = Vector2.zero;
            uiGoRectTrans.anchoredPosition3D = Vector3.zero;
            uiGoRectTrans.anchorMin = Vector2.zero;
            uiGoRectTrans.anchorMax = Vector2.one;

            ui.Transform.LocalScaleIdentity();
            ui.Transform.gameObject.name = uiBehaviourName;

            ui.PanelInfo = new UIPanelInfo
                {AssetBundleName = assetBundleName,TransitionType = transitionType, Level = level, PanelName = uiBehaviourName, CanOpenPrevious = CanOpenPrevious};

            mAllUI.Add(uiBehaviourName, ui);

            ui.Init(uiData);

            return ui;
        }

        #region UnityCSharp Generic Support

        /// <summary>
        /// Create&ShowUI
        /// </summary>
        public void OpenPanel<T>(UILevel canvasLevel = UILevel.Common, 
            IUIData uiData = null, UITransitionType transitionType = UITransitionType.NULL,
            UIPanel view  = null, string assetBundleName = null,
            string prefabName = null, UnityAction action = null) where T : UIPanel
        {
            if (view.IsNotNull())
            {
                Push(view,transitionType);
            }
            if (transitionType==UITransitionType.CIRCLE)
            {
                if (!mIsChangingView)
                {
                    mIsChangingView = true;
                    mCircleMask.transform.parent.gameObject.Show();
                    var tweenerCore = mCircleMask.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
                    DOTweenModuleUI.DOFade(mCircleMask, 1, 0.5f);
                    tweenerCore.onComplete = delegate()
                    {
                        action?.Invoke();
                        OpenUI(prefabName ?? GetUIBehaviourName<T>(),transitionType, 
                            canvasLevel, uiData, assetBundleName, view.IsNotNull());
                        var t = mCircleMask.DOFade(0, 0.5f).SetEase(Ease.OutQuint);
                        t.onComplete = delegate()
                        {
                            mCircleMask.transform.parent.gameObject.Hide();
                            mCircleMask.transform.localScale = Vector3.zero;
                            mCircleMask.color=new Color(255,255,255,0);
                            mIsChangingView = false;
                        };
                    };
                }
            }
            else if (transitionType == UITransitionType.CLOUD)
            {
                if (!mIsChangingView)
                {
                    mIsChangingView = true;
                    mCloudAnimationController.transform.parent.Show();
                    mCloudAnimationController.PlayAnimation(CloudState.OPENANDCLOSE);
                    IDisposable disposable = null;
                    disposable = SimpleEventSystem.GetEvent<CloudOpenAnimation>().Subscribe(_ =>
                    {
                        disposable.Dispose();
                        action?.Invoke();
                        OpenUI(prefabName ?? GetUIBehaviourName<T>(), transitionType,
                            canvasLevel, uiData, assetBundleName, view);
                    });
                }
            }
            else
            {
                OpenUI(prefabName ?? GetUIBehaviourName<T>(),transitionType,
                    canvasLevel, uiData, assetBundleName, view);
            }
        }

        public void ShowUI<T>() where T : UIPanel
        {
            ShowUI(GetUIBehaviourName<T>());
        }

        public void HideUI<T>() where T : UIPanel
        {
            HideUI(GetUIBehaviourName<T>());
        }

        public void CloseUI<T>() where T : UIPanel
        {
            CloseUI(GetUIBehaviourName<T>());
        }

        public T GetUI<T>() where T : UIPanel
        {
            return GetUI(GetUIBehaviourName<T>()) as T;
        }

        #endregion

        public void CloseAllOtherPanel(string name)
        {
            List<string> Keys = new List<string>(mAllUI.Keys);
            for (int i = mAllUI.Count - 1; i >= 0; i--)
            {
                if (!Keys[i].Equals(name))
                {
                    mAllUI[Keys[i]].Close();
                    Destroy(mAllUI[Keys[i]].Transform.gameObject);
                    mAllUI.Remove(Keys[i]);
                }
            }

            // foreach (var layer in mAllUI)
            // {
            //     if (!layer.Key.Equals(name))
            //     {
            //         Destroy(layer.Value.Transform.gameObject);
            //         mAllUI.Remove(layer.Key);
            //     } 
            // }
        }
    }

    public static class UIMgr
    {

        public static Camera Camera
        {
            get { return UIManager.Instance.UICamera; }
        }

        public static void SetResolution(int width, int height, float matchOnWidthOrHeight)
        {
            UIManager.Instance.SetResolution(width, height);
            UIManager.Instance.SetMatchOnWidthOrHeight(matchOnWidthOrHeight);
        }

        public static Vector2 GetResolution()
        {
            return UIManager.Instance.GetResolution();
        }

        public static float GetMatchOrWidthOrHeight()
        {
            return UIManager.Instance.GetMatchOnWithOrHeight();
        }

        #region 高频率调用的 api 只能在 Mono 层使用

        
        // internal static void OpenPanel<T>(IUIData uiData,
        //     UITransitionType transitionType, UnityAction action ) where T : UIPanel
        // {
        //     UIManager.Instance.OpenPanel<T>(UILevel.Common, uiData, transitionType,action);
        // }
        //

        internal static void OpenPanel<T>(IUIData uiData = null,  UITransitionType transitionType = UITransitionType.NULL,
            UIPanel view  = null, string assetBundleName = null, string prefabName = null, UnityAction action = null) where T : UIPanel
        {
            UIManager.Instance.OpenPanel<T>(UILevel.Common, uiData, transitionType, view, assetBundleName, prefabName, action);
        }
        
        internal static void ClosePanel<T>() where T : UIPanel
        {
            UIManager.Instance.CloseUI<T>();
        }

        internal static void ShowPanel<T>() where T : UIPanel
        {
            UIManager.Instance.ShowUI<T>();
        }

        internal static void HidePanel<T>() where T : UIPanel
        {
            UIManager.Instance.HideUI<T>();
        }

        public static void CloseAllPanel()
        {
            UIManager.Instance.CloseAllUI();
        }

        public static void HideAllPanel()
        {
            UIManager.Instance.HideAllUI();
        }

        internal static T GetPanel<T>() where T : UIPanel
        {
            return UIManager.Instance.GetUI<T>();
        }

        #endregion

        #region 给脚本层用的 api

        public static UIPanel GetPanel(string panelName)
        {
            return UIManager.Instance.GetUI(panelName);
        }

        public static UIPanel OpenPanel(string panelName, UILevel level = UILevel.Common, string assetBundleName = null)
        {
            return UIManager.Instance.OpenUI(panelName, UITransitionType.NULL,level, null, assetBundleName) as UIPanel;
        }

        public static UIPanel OpenPanel(string panelName)
        {
            return UIManager.Instance.OpenUI(panelName, UITransitionType.NULL,UILevel.Common, null, null) as UIPanel;
        }

        public static void ClosePanel(string panelName)
        {
            UIManager.Instance.CloseUI(panelName);
        }

        public static void ShowPanel(string panelName)
        {
            UIManager.Instance.ShowUI(panelName);
        }

        public static void HidePanel(string panelName)
        {
            UIManager.Instance.HideUI(panelName);
        }

        #endregion

        public static void CloseAllOtherPanel(string name)
        {
            UIManager.Instance.CloseAllOtherPanel(name);
        }
    }

    [Obsolete("弃用啦")]
    public class QUIManager : UIManager
    {
    }
}