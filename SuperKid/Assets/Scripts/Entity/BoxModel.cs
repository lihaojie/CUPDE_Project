using System.Collections.Generic;

/**
 * 盒子Model
 */
public class BoxModel
{
    /**
     * 每1天的盒子对象
     */
    public List<BoxDayDetailModel> boxContentList;

    /**
     * 盒子音频列表
     */
    public List<DayActionModel> audioList;

    /**
     * 盒子绘本列表
     */
    public List<DayActionModel> picBookList;

    /**
     * 盒子视频列表
     */
    public List<DayActionModel> videoList;

    /**
      * 盒子名称
      */
    public string name;

    /**
     * 第几个盒子
     */
    public int month;

    /**
     * 盒子简介
     */
    public string intro;

    /**
     * 盒子ID
     */
    public int boxId;

    /**
     * 盒子封面
     */
    public string frontCover;

    /**
     * 盒子版本号
     */
    public string boxVersion;

    /**
     * 盒子标签
     */
    public List<string> boxTag;
    
    /**
     * 是否删除
     */
    public int isDel;
}