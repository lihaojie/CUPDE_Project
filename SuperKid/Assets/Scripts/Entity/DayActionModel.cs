using System.Collections.Generic;

/**
 * 盒子动作Model 听儿歌、听故事等等
 */
public class DayActionModel
{
    /**
    * 盒子的动作, 0-亲子互动, 1-听儿歌, 2-听故事, 3-听绘本音频, 4-听卡片音频, 50-看视频, 60-读绘本, 70-休息, 80-词卡包音频
    */
    public int action;

    /**
    * 盒子动作图片
    */
    public string actionIcon;

    /**
    * 盒子动作表主键Id
    */
    public int boxDayActionId;

    /**
     * 盒子Id
     */
    public int boxId;

    /**
    * box_day 第几天
    */
    public int day;

    /**
     * 当前资源隶属于多少天
     */
    public List<int> days;

    /**
    * box_day 主键ID
    */
    public int id;

    /**
    * 名称
    */
    public string name;

    /**
    * 关联资源 id, 是 b_pic_book_item 的 id
    */
    public int resourceId;

    /**
    * 关联资源类型, 0-无资源, 1-音频, 2-视频, 3-绘本
    */
    public int resourceType;

    /**
    * 观看人数
    */
    public int views;
    
    /**
    * 缩略图
    */
    public string thumbnailPath;

    /**
     * 主题
     */
    public string topic;

    /**
     * 主题图片
     */
    public string thumbUrl;

    // 绘本详情补充信息
    /**
     * 作者
     */
    public string author;
    
    /**
     * 作者介绍
     */
    public string authorIntroduction;
    
    /**
     * 内容介绍
     */
    public string contentIntroduction;
    
    /**
     * 绘本介绍
     */
    public string picBookIntroduction;
    
    /**
     * 标题
     */
    public string title;

    public string introPicStr;
}