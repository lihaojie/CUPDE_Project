using System.Collections.Generic;

/**
 * 每一天的盒子详情Model
 */
public class BoxDayDetailModel
{
    /**
     * 当天的动作列表
     */
    public List<DayActionModel> boxDayActionList;
    
    /**
     * 第几天
     */
    public int day;
    
    /**
     * 主题
     */
    public string topic;

    /**
     * 主题图片
     */
    public string thumbUrl;

    /**
     * 盒子ID
     */
    public int boxId;
    
}