using SuperKid.Entity;
public class AttendanceModel
{
    public int babyId;
    public int boxDay;
    public int finishPunchNum;
    public string punchPath;
    public string punchText;
    public int punchType; // 0图文，1音频，2视频
    public int relBoxId;
    public int score; // 打卡积分
    public int shareScore; // 分享积分
    public int id; // 打卡id
    public int subject; // 1英语， 2语文， 0强国
    /*
     * 是否分享 0未分享1已分享
     */
    public int isShare;

}