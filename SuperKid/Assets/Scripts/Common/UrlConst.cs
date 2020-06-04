using System;
using System.IO;
using QFramework;
using UnityEngine;

namespace SuperKid
{
    public class UrlConst
    {
        // 测试环境
        public static string BaseUrlDebug => "http://test.i.hqkid.cn/robot-mobile/";

        // 正式环境
        public static string BaseUrlRelease => "https://i.hqkid.cn/robot-mobile/";

        // 接口方法

        private static string BaseUrl => App.BaseUrl;
        
        /**
        * 登录
        */
        public static string Login => BaseUrl + "api/login/doLogin";

        /**
        * 注册校验
        */
        public static string CheckIsRegist => BaseUrl + "api/login/doCheckIsRegist";

        /**
        * 获取验证码
        */
        public static string SendSMS => BaseUrl + "api/register/sendSMS";

        /**
        * 校验验证码是否正确
        */
        public static string CheckVerfyNoValid => BaseUrl + "api/register/checkVerfyNoValid";

        /**
        * 注册
        */
        public static string DoRegister => BaseUrl + "api/register/doRegister";

        /**
        * 登录前忘记密码 设置
        */
        public static string ModifyPasswd => BaseUrl + "api/login/modifyPasswd";
        
        /**
        * 登录后忘记密码 设置
        */
        public static string DoUpdatePasswd => BaseUrl + "api/modify/doUpdatePasswd";

        /**
        * 新增宝宝信息
        */
        public static string DoAddBaby => BaseUrl + "api/register/doAddBaby";

        /**
        * 设置关系
        */
        public static string DoSetRelation => BaseUrl + "api/register/doSetRelation";
        
        /**
        * 修改宝宝信息
        */
        public static string DoUpdateBabyInfo => BaseUrl + "api/modify/doUpdateBabyInfo";

        /**
        * 根据计划id获取计划包
        */
        public static string PlanInfo => BaseUrl + "api/plan/info";
       
        /**
        * 计划包列表
        */
        public static string BabyPlanList => BaseUrl + "api/plan/babyPlanListNew";

        /**
         * 获取当前宝宝解锁的盒子列表信息
         */
        public static string BabyStudyLockInfo => BaseUrl  +"api/babyDailyStudyUnlock/list";
        /**
         * 获取当前宝宝解锁的盒子列表信息
         */
        public static string BabyAddressListPage => BaseUrl  +"api/babyAddress/page";
        /**
         * 新增收货地址
         */
        public static string AddBabyAddress => BaseUrl  +"api/babyAddress/add";
        /**
         * 编辑收货地址
         */
        public static string EditBabyAddress => BaseUrl  +"api/babyAddress/edit";
        /**
         * 删除收货地址
         */
        public static string DelBabyAddress => BaseUrl  +"api/babyAddress/del";

        /**
          * 解锁当天
          */
        public static string MethodDoAddBabyStudyLock => BaseUrl + "api/babyDailyStudyRecord/add";

        /**
         * 月计划详情
         */
        public static string MethodPlanDetail => "api/plan/detail";

        /**
         * 盒子详情
         */
        public static string MethodBoxContent => "api/box/boxContent";

        /**
         * 资源内容
         */
        public static string MethodPickBookResource => "api/picBookItem/getResource";

        /**
         * 视频列表
         */
        public static string MethodVideoList => "api/plan/videoList";

        /**
         * 音频列表
         */
        public static string MethodAudioList => "api/plan/audioList";

        /**
         * 绘本列表页面
         */
        public static string MethodPickBookList => "api/picbook/list"; 
        /**
         * 获取省市区
         */
        public static string ProvinceCityAreaList => BaseUrl +"api/common/findProvinceCityAreaList";
        /**
         * 意见反馈
         */
        public static string AddFeedBack => BaseUrl +"api/app/feedback/addFeedBack";
        /**
         * 随便听听
         */
        public static string GetResourcePageUrl => BaseUrl +"api/tuling/getResourcePageUrl";
        /**
         * 扫码领取成功
         */
        public static string ScanQR => BaseUrl +"api/plan/scanQR";
        /**
         * 获取设备信息
         */
        public static string GetDeviceStatus => BaseUrl +"api/tuling/getDeviceStatus";
        /**
         * 设备版本检查
         */
        public static string CheckDeviceVersion => BaseUrl +"api/tuling/checkDeviceVersion";
        /**
         * 设备解绑
         */
        public static string Unbind => BaseUrl +"api/userRobotBind/unbind";

        /**
         * 勋章墙数据
         */
        public static string FindBabyAllMedalList => BaseUrl + "api/babymedal/findBabyAllMedalList";

        /**
         * 历史照片墙
         */
        public static string FindPhotoWall => BaseUrl + "api/babyinfo/findPhotowallV20200430";

         /**
         * 家庭组
         */
        public static string FindMemberInfo => BaseUrl + "api/homeGroup/findMemberInfo";

        /**
         * 邀请家长
         */
        public static string DoCheckInviteValid => BaseUrl + "api/invite/doCheckInviteValid";

        /**
         * 转让管理员
         */
        public static string DoTransferManager => BaseUrl + "api/homeGroup/doTransferManager";

        /**
         * 移除家长
         */
        public static string DoRejectMember => BaseUrl + "api/homeGroup/doRejectMember";
        /**
         * 绑定
         */
        public static string Bind => BaseUrl + "api/userRobotBind/bindNew";
        /**
         * 版本更新
         */
        public static string FindReleaseInfo => BaseUrl + "api/app/findReleaseInfo";

        /**
         * 获取当前宝宝获得勋章信息
         */
        public static string FindBabyMedalList => BaseUrl + "api/babymedal/findList";

        /**
         * 领取勋章
         */
        public static string DrawMedal => BaseUrl + "api/babymedal/doDrawMedal";

        /**
         * 校验是否已经签到
         */
        public static string ChechIsSignIn => BaseUrl + "api/attendance/checkIsSignIn";
        
        /**
         * q签到
         */
        public static string AttendanceAdd => BaseUrl + "api/attendance/add";
         
        /**
         * 礼品列表
         */
        public static string GoodsList => BaseUrl + "api/goods/list";
        
        /**
         * 积分明细
         */
        public static string BabyScore => BaseUrl + "api/babyScore/page";
        /**
         * 打卡post
         */
        public static string Punch => BaseUrl + "api/appletBabyPunch/punch";
        /**
         * 统一上传文件post
         */
        public static string Upload => BaseUrl + "api/common/upload";
        /**
         * 学习强国
         */
        public static string DrawPowerSpecialPlan => BaseUrl + "api/plan/drawPowerSpecialPlan";

        /**
         * 兑换商品
         */
        public static string OrderAdd => BaseUrl + "api/order/add";
        
        /**
         * 兑换商品详情
         */
        public static string OrderOne => BaseUrl + "api/order/one";
        
        /**
         * 获取打卡分享图
         */
        public static string SharePoster => BaseUrl + "api/appletBabyPunch/share/poster";
        
        /**
         * 作品墙列表
         */
        public static string WorksWall => BaseUrl + "api/appletBabyPunch/punchWall";
        /**
         * 根据当前登录人获取设备标识信息
         */
        public static string DeviceIdByUserId => BaseUrl + "api/userRobotBind/getDeviceIdByUserId";
        /**
         * 刷新个人登录信息
         */
        public static string GetUserInfo => BaseUrl + "api/login/me";
        /**
         * 新增分享记录增加积分
         */
        public static string AddRecord => BaseUrl + "api/share/addRecord";


    }
}
