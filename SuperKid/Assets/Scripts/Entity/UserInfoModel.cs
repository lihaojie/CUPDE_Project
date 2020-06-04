using SuperKid.Entity;

public class UserInfoModel
{
    public BabyInfoModel babyInfoVo;
    public int id;
    public int relBabyId; // 关联的宝宝ID  如果是0， 就代表没有设置宝宝信息 （也可以作为是否设置了宝宝的条件 与babyInfoVo一致）
    public string token;
    public string babyRelation; // 与宝宝的关系  如果此值没有 代表没有设置与宝宝的关系
    public string deviceId;
    public string mobile;
    public string logoUrl;
    public int chPlanId;
    public int enPlanId;
    public int qgPlanId;
    public MqttInfoModel mqttInfo;
    public int medalDrawCount;
}