using System;
using System.Collections;
using System.Collections.Generic;
using SuperKid;
using Newtonsoft.Json;
using QFramework;
using SuperKid.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


public class MasterController : MonoBehaviour
{

    public CameraMove MainCamera;
    public CameraMove MasterCamera;
    public Transform Master;
    public MasterAnimation MasterAnimation;
    public float[] ItemPositionY = new[] {-4.19f, -3.7f};
    public int CurrentIndex = 0;
    public GameObject CurrentItem;
    public bool IsRun;
    public List<float> ItemPositionList = new List<float>();
    private BoxModel BoxModel;
    private List<int> UnlockDays = new List<int>(); // 未解锁的天集合
    public int LockedDay = 0; // 取出当前盒子已经解锁的天数
    private int BoxMonthIndex;

    public BoxDayDetailModel SelectDayDetailModel;
    public bool IsOpenDay;

    
    private void Awake()
    {
        MainCamera = transform.Find("MainCamera").GetComponent<CameraMove>();
        MasterCamera = transform.Find("MasterCamera").GetComponent<CameraMove>();
        Master = transform.Find("Master");
        MasterAnimation = Master.GetComponent<MasterAnimation>();
        InitItemPosition();
        SimpleEventSystem.GetEvent<CanvasCanMove>()
            .Subscribe(_ => { SetItemActive(true);}).AddTo(this);
        SimpleEventSystem.GetEvent<CanvasDotMove>()
            .Subscribe(_ => { SetItemActive(false);}).AddTo(this);
    }

    public void SetData(BoxModel boxModel)
    {
        if (PlayerPrefsUtil.LockModels.Count <= PlayerPrefsUtil.GetBoxMonthIndex())
        {
            PlayerPrefsUtil.SaveBoxMonthIndex(0);
        }
        CurrentIndex = PlayerPrefsUtil.GetUserDayPosition(boxModel);
        BoxMonthIndex = PlayerPrefsUtil.GetBoxMonthIndex();
        BoxModel = boxModel;
        UnlockDays.Clear();
        UpdatePosition();
        UpdateItem(PlayerPrefsUtil.LockModels[BoxMonthIndex], boxModel.boxContentList);
    }

    private void UpdateItem(BabyStudyLockModel lockModel, List<BoxDayDetailModel> boxContentList)
    {
        for (int i = 0; i < boxContentList.Count; i++)
        {
            if (i >= ItemCanvasList.ItemCanvasObjList.Count)
            {
                return;
            }

            var dayDetailModel = boxContentList[i];
            var dayAction = dayDetailModel.boxDayActionList[0];
            GameObject game = ItemCanvasList.ItemCanvasObjList[i];
            Image restImage = game.transform.Find("RestImage").GetComponent<Image>();
            var maskTr = game.transform.Find("MaskImage");
            var punchImg = game.transform.Find("punchImage").GetComponent<Image>();
            var tvName = game.transform.Find("Tv_Name").GetComponent<Text>();
            var image = maskTr.Find("Img_Thumbnail").GetComponent<Image>();
            var block = maskTr.Find("BlockImage").GetComponent<Image>();
            var lockImg = maskTr.Find("ImageLock").GetComponent<Image>();

            tvName.text = dayDetailModel.topic;
            ImageDownloadUtils.Instance.SetAsyncImage(dayDetailModel.thumbUrl, image);
            game.transform.GetComponent<Button>().onClick.RemoveAllListeners();
            game.transform.GetComponent<Button>().onClick
                .AddListener(delegate { OnClick(game, dayDetailModel); });
            GameObject mask = game.transform.Find("MaskImage").gameObject;
            if (lockModel.boxDayList == null || !lockModel.boxDayList.Contains(dayDetailModel.day))
            {
                punchImg.gameObject.SetActive(false);
            }
            else
            {
                punchImg.gameObject.SetActive(true);
            }

            if (dayAction.action == 70)
            {
                restImage.gameObject.SetActive(true);
                mask.SetActive(false);
                tvName.gameObject.SetActive(false);
            }
            else
            {
                if (dayDetailModel.day > lockModel.day)
                {
                    UnlockDays.Add(dayDetailModel.day);
                    block.gameObject.SetActive(true);
                    lockImg.gameObject.SetActive(true);
                }
                else
                {
                    block.gameObject.SetActive(false);
                    lockImg.gameObject.SetActive(false);
                }

                restImage.gameObject.SetActive(false);
                mask.SetActive(true);
                tvName.gameObject.SetActive(true);
            }
        }
    }

    public void OnClick(GameObject obj, BoxDayDetailModel dayDetailModel)
    {
        if (IsOpenDay)
        {
            return;
        }
        
        if (IsRun)
        {
            return;
        }

        if (MainCamera.isMove || MainCamera.clickCount < 2)
        {
            return;
        }
        AudioManager.PlaySound("Button_Audio");
        if (dayDetailModel.boxDayActionList[0].action == 70)
        {
            AudioManager.PlayVoice("Today_is_sunday");
            UIMgr.OpenPanel<TipPanel>(new TipPanelData()
            {
                action = TipAction.Plan,
                message = "今天是休息日哦~",
                isHideCancelButton = true,
                removeConfirmCallback = true,
                strConfirm = "我知道了"
            });
        }
        else // 当前点击的不是休息日
        {
            if (UnlockDays.Count == 0)
            {
                // 移动
                DoMove(obj,dayDetailModel);
            }
            else
            {
                if (dayDetailModel.day == UnlockDays[0]) // 正要解锁
                {
                    // 移动
                    DoMove(obj, dayDetailModel);
                    // 调用接口解锁并存储本地
                    DoAddBabyStudyRecord(dayDetailModel);
                }
                else if (dayDetailModel.day > UnlockDays[0]) // 前面有任务未完成
                {
                    AudioManager.PlayVoice("Unlock_day");
                    UIMgr.OpenPanel<TipPanel>(new TipPanelData()
                    {
                        action = TipAction.Plan,
                        message = "前面的任务还没有学习哦~",
                        isHideCancelButton = true,
                        removeConfirmCallback = true,
                        strConfirm = "我知道了"
                    });
                }
                else if (dayDetailModel.day < UnlockDays[0])
                {
                    // test(unlockDays);
                    // Debug.Log("44444444444444444" + "===========" + unlockDays[0] + "===============" + dayDetailModel.day);
                    // 移动
                    DoMove(obj,dayDetailModel);
                }
            }
        }
    }

    private void DoMove(GameObject obj, BoxDayDetailModel dayDetailModel)
    {
        IsOpenDay = true;
        SelectDayDetailModel = dayDetailModel;
        IsRun = true;
        Vector3 position = obj.transform.position;
        int index = int.Parse(obj.name);
        if (CurrentIndex == 0)
        {
            if (index == 1)
            {
                MasterAnimation.JumpRight();
                CurrentItem = obj;
                CanvasItemDown();
                StartCoroutine(JumpForSeconds(false, false, position.x));
            }
            else if (index == 2)
            {
                _fallTime = 20 + 8;
                _jumpTime = 20 + 8;
                MasterAnimation.JumpRight();
                CurrentItem = obj;
                CanvasItemDown();
                StartCoroutine(JumpForSeconds(false, false, position.x));
            }
            else
            {
                _fallTime = 20 + 8 * (index - 1);
                _jumpTime = 20 + 8 * (index - 1);
                MasterAnimation.JumpHighRight();
                CurrentItem = obj;
                CanvasItemDown();
                StartCoroutine(JumpForSeconds(true, false, position.x, 0.12f));
            }
        }
        else
        {
            if (CurrentIndex == index)
            {
                IsRun = false;
                SimpleEventSystem.Publish(new CanvasAnimationFinish());
            }
            else if (CurrentIndex < index)
            {
                if (index - CurrentIndex == 1)
                {
                    MasterAnimation.JumpRight();
                    CurrentItem = obj;
                    CanvasItemDown();
                    StartCoroutine(JumpForSeconds(false, false, position.x));
                }
                else if (index - CurrentIndex == 2)
                {
                    MasterAnimation.JumpRight();
                    CurrentItem = obj;
                    CanvasItemDown();
                    StartCoroutine(JumpForSeconds(false, true, position.x));
                }
                else
                {
                    MasterAnimation.JumpHighRight();
                    CurrentItem = obj;
                    CanvasItemDown();
                    StartCoroutine(JumpForSeconds(true, false, position.x, 0.12f));
                }
            }
            else
            {
                if (CurrentIndex - index == 1)
                {
                    MasterAnimation.JumpLeft();
                    CurrentItem = obj;
                    CanvasItemDown();
                    StartCoroutine(JumpForSeconds(false, false, position.x));
                }
                else if (CurrentIndex - index == 2)
                {
                    MasterAnimation.JumpLeft();
                    CurrentItem = obj;
                    CanvasItemDown();
                    StartCoroutine(JumpForSeconds(false, true, position.x));
                }
                else
                {
                    MasterAnimation.JumpHighLeft();
                    CurrentItem = obj;
                    CanvasItemDown();
                    StartCoroutine(JumpForSeconds(true, false, position.x, 0.12f));
                }
            }
        }

        CurrentIndex = index;
        PlayerPrefsUtil.SaveUserDayPosition(BoxModel, CurrentIndex);
    }

    public bool IsInView(Vector3 worldPos)
    {
        Transform camTransform = Camera.main.transform;
        Vector2 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        Vector3 dir = (worldPos - camTransform.position).normalized;
        float dot = Vector3.Dot(camTransform.forward, dir); //判断物体是否在相机前面

        if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            return true;
        else
            return false;
    }

    public void SetItemActive(bool flag)
    {
        MainCamera.GetComponent<CameraMove>().enabled = flag;
        MasterCamera.GetComponent<CameraMove>().enabled = flag;
        foreach (GameObject item in ItemCanvasList.ItemCanvasObjList)
        {
            item.GetComponent<UIRaycaster>().enabled = flag;
        }
    }

    float _jumpHeight = 2f;
    int _fallTime = 20;
    int _jumpTime = 20;

    private IEnumerator JumpForSeconds(bool isFar, bool jumpTwo, float positionX, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(routine: JumpForSeconds(isFar, jumpTwo, positionX));
    }

    private IEnumerator JumpForSeconds(bool isFar, bool jumpTwo, float positionX)
    {
        bool isGoToNextScene = false;
        if (isFar)
        {
            _jumpHeight = 5f;
        }
        else
        {
            _jumpHeight = 2f;
        }

        var jumpPositionX = positionX - Master.transform.position.x;
        var targetIndex = int.Parse(CurrentItem.name);
        var targetPositionY = targetIndex % 2 == 0 ? ItemPositionY[0] : ItemPositionY[1];
        ;
        var jumpPositionY = targetPositionY - Master.transform.position.y;
        var time = _jumpTime;
        var speed = _jumpHeight / _jumpTime;
        if (jumpTwo)
        {
            jumpPositionX = jumpPositionX / 2;
            targetPositionY = (targetIndex - 1) % 2 == 0 ? ItemPositionY[0] : ItemPositionY[1];
            jumpPositionY = targetPositionY - Master.transform.position.y;
        }

        float speedX = jumpPositionX / _jumpTime / 2;
        while (time > 0)
        {
            time--;
            Master.transform.Translate(transform.up * speed + new Vector3(1, 0, 0) * speedX);
            yield return new WaitForEndOfFrame();
        }

        time = _fallTime;
        speed = (-_jumpHeight + jumpPositionY) / _fallTime;
        speedX = jumpPositionX / _fallTime / 2;
        var tag = true;
        while (time > 0)
        {
            time--;
            if (tag && !jumpTwo && Math.Abs(Master.transform.position.x - positionX) < 1f)
            {
                tag = false;
                isGoToNextScene = true;
                CurrentItem.GetComponent<Animator>().Play("CanvasAnimation", 0, 0);
            }

            Master.transform.Translate(transform.up * speed + new Vector3(1, 0, 0) * speedX);
            yield return new WaitForEndOfFrame();
        }

        if (jumpTwo)
        {
            time = _jumpTime;
            speed = _jumpHeight / _jumpTime;
            speedX = jumpPositionX / _jumpTime / 2;
            targetPositionY = targetIndex % 2 == 0 ? ItemPositionY[0] : ItemPositionY[1];
            jumpPositionY = targetPositionY - Master.transform.position.y;
            while (time > 0)
            {
                time--;
                Master.transform.Translate(transform.up * speed + new Vector3(1, 0, 0) * speedX);
                yield return new WaitForEndOfFrame();
            }

            time = _fallTime;
            speed = (-_jumpHeight + jumpPositionY) / _fallTime;
            speedX = jumpPositionX / _fallTime / 2;
            while (time > 0)
            {
                time--;
                if (tag && Math.Abs(Master.transform.position.x - positionX) < 1f)
                {
                    tag = false;
                    isGoToNextScene = true;
                    CurrentItem.GetComponent<Animator>().Play("CanvasAnimation", 0, 0);
                }

                Master.transform.Translate(transform.up * speed + new Vector3(1, 0, 0) * speedX);
                yield return new WaitForEndOfFrame();
            }
        }

        if (!isGoToNextScene)
        {
            CurrentItem.GetComponent<Animator>().Play("CanvasAnimation", 0, 0);
        }

        MasterAnimation.DefaultStand();
        IsRun = false;
    }


    /**
     * 请求接口保存解锁记录并更新本地存储天数数据
     * <param name="paramDict"></param>
     */
    // private void DoAddBabyStudyRecord(Dictionary<string, object> dict)
    // {
    //     var sign = SignUtil.getSign(dict);
    //     var hashtable = new Hashtable();
    //     hashtable.Add("appid", AppConst.APPID);
    //     hashtable.Add("babyId", dict["babyId"]);
    //     hashtable.Add("planId", dict["planId"]);
    //     hashtable.Add("boxId", dict["boxId"]);
    //     hashtable.Add("boxDayId", dict["boxDayId"]);
    //     hashtable.Add("month", dict["month"]);
    //     hashtable.Add("day", dict["day"]);
    //     hashtable.Add("creator", dict["creator"]);
    //     hashtable.Add("sign", sign);
    //     var methodParams = SignUtil.getMethodParamStr(dict);
    //     var url = AppConst.TestBaseUrl + AppConst.MethodDoAddBabyStudyLock + methodParams + "&sign=" + sign;
    //     
    //     var request = new Request("post", url, hashtable);
    //     Debug.Log("url = " + url);
    //     request.Send(re =>
    //     {
    //         Debug.Log(re.response.Text);
    //         var baseModel = JsonConvert.DeserializeObject<BaseModel>(re.response.Text);
    //         if (baseModel.status == 1)
    //         {
    //              
    //             
    //             // 更新本地存储的天数
    //             object day = dict["day"];
    //             PlayerPrefsUtil.SaveUserDayPosition(BoxModel,(int) day);
    //             // 更新本地lockModel中的数值
    //             var lockBean = App.lockModel.data[BoxMonthIndex];
    //             lockBean.day = (int) day;
    //             // 修改未解锁天数集合
    //             UnlockDays.RemoveAt(0);
    //         }
    //     });
    // }

    
    private void DoAddBabyStudyRecord(BoxDayDetailModel dayDetailModel)
    {
        
        Dictionary<string, object> paramDict = new Dictionary<string, object>();
        paramDict.Add("babyId", PlayerPrefsUtil.UserInfo.babyInfoVo.id);
        paramDict.Add("planId", PlayerPrefsUtil.SelectPlanId);
        paramDict.Add("boxId", dayDetailModel.boxId);
        paramDict.Add("boxDayId", dayDetailModel.boxDayActionList[0].id);
        paramDict.Add("month", BoxModel.month);
        paramDict.Add("day", dayDetailModel.day);
        paramDict.Add("creator", PlayerPrefsUtil.UserInfo.id);
        HttpUtil.PostWithSign<object>(UrlConst.MethodDoAddBabyStudyLock, paramDict)
            .Subscribe(response =>
                {
                    PlayerPrefsUtil.LockModels[BoxMonthIndex].day = dayDetailModel.day;
                    // 修改未解锁天数集合
                    UnlockDays.RemoveAt(0);
                }
                , e =>
                {
                    if (e is HttpException)
                    {
                        HttpException http = e as HttpException;
                        Log.E("弹吐司" + http.Message);
                    }
                }).AddTo(this);
    }
    
    /**
     * 请求接口保存解锁记录并更新本地存储天数数据
     * Dictionary<string, object> paramDict = new Dictionary<string, object>();
        paramDict.Add("planId", PlayerPrefsUtil.SelectPlanId);
        paramDict.Add("babyId", PlayerPrefsUtil.UserInfo.babyInfoVo.id);
        paramDict.Add("creator", PlayerPrefsUtil.UserInfo.id);
        paramDict.Add("day", dayDetailModel.day);
        paramDict.Add("month", BoxModel.month);
        paramDict.Add("boxId", dayDetailModel.boxId);
        paramDict.Add("boxDayId", dayDetailModel.boxDayActionList[0].id);
     * <param name="paramDict"></param>
     */
    private void DoAddBabyStudyRecord(Dictionary<string, object> dict)
    {
        // var sign = SignUtil.getSign(dict);
        // var hashtable = new WWWForm();
        // hashtable.AddField("appid", AppConst.APPID);
        // hashtable.AddField("babyId", dict["babyId"].ToString());
        // hashtable.AddField("planId", dict["planId"].ToString());
        // hashtable.AddField("boxId", dict["boxId"].ToString());
        // hashtable.AddField("boxDayId", dict["boxDayId"].ToString());
        // hashtable.AddField("month", dict["month"].ToString());
        // hashtable.AddField("day", dict["day"].ToString());
        // hashtable.AddField("creator", dict["creator"].ToString());
        // hashtable.AddField("sign", sign);
        // var methodParams = SignUtil.getMethodParamStr(dict);
        // var url = AppConst.BaseUrl + AppConst.MethodDoAddBabyStudyLock + methodParams + "&sign=" + sign;
        // ObservableWWW.Post(url, hashtable)
        //     .Subscribe(response =>
        //         {
        //             var baseModel = JsonConvert.DeserializeObject<BaseModel>(response);
        //             if (baseModel.status == 1)
        //             {
        //                 // 更新本地存储的天数
        //                 object day = dict["day"];
        //                 PlayerPrefsUtil.SaveUserDayPosition(BoxModel, (int) day);
        //                 // 更新本地lockModel中的数值
        //                 var lockBean = App.lockModel.data[BoxMonthIndex];
        //                 lockBean.day = (int) day;
        //                 // 修改未解锁天数集合
        //                 UnlockDays.RemoveAt(0);
        //             }
        //         }
        //         , e => { });
    }

    private void InitItemPosition()
    {
        for (int i = 0; i < 30; i++)
        {
            ItemPositionList.Add(9.6f + i * 4.685f);
        }
    }

    private void CanvasItemDown()
    {
        for (int i = 0; i < ItemCanvasList.ItemCanvasObjList.Count; i++)
        {
            Animator animator = ItemCanvasList.ItemCanvasObjList[i].GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("CanvasUpAnimation"))
            {
                animator.Play("CanvasDownAnimation", 0, 0);
            }
        }
    }

    private void UpdatePosition()
    {
        float y = CurrentIndex % 2 == 0 ? ItemPositionY[0] : ItemPositionY[1];
        float x = 0;
        if (CurrentIndex != 0)
        {
            x = ItemPositionList[CurrentIndex - 1];
        }

        MasterCamera.InitPosition();
        MainCamera.InitPosition();
        if (CurrentIndex == 0)
        {
            x = 5.27f;
            y = -3.53f;
        }

        if (CurrentIndex != 0)
        {
            ItemCanvasList.ItemCanvasObjList[CurrentIndex - 1].GetComponent<Animator>().Play("CanvasUpAnimation", 0, 0);
        }
        else
        {
            CanvasItemDown();
        }

        if (CurrentIndex == 30)
        {
            MainCamera.transform.position =
                new Vector3(ItemPositionList[CurrentIndex - 2], MainCamera.transform.position.y,
                    MainCamera.transform.position.z);
            MasterCamera.transform.position =
                new Vector3(ItemPositionList[CurrentIndex - 2], MasterCamera.transform.position.y,
                    MasterCamera.transform.position.z);
        }
        else if (CurrentIndex > 2)
        {
            Debug.Log("currentIndex==" + CurrentIndex);
            MainCamera.transform.position =
                new Vector3(x, MainCamera.transform.position.y, MainCamera.transform.position.z);
            MasterCamera.transform.position =
                new Vector3(x, MasterCamera.transform.position.y, MasterCamera.transform.position.z);
        }

        Master.transform.position = new Vector3(x, y, Master.transform.position.z);
    }
}