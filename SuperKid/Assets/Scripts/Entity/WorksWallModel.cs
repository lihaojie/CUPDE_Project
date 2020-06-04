using System.Collections.Generic;

public class WorksWallModel
{
    public List<WorksWall> records;
}

public class WorksWall
{
    public int id;
    public int babyId;
    public string birthday;
    public string age;
    public int boxDay;
    public string name;
    public string punchPathUrl;
    public string punchText;
    public int punchType; // 0图文，1音频，2视频
    public int relBoxId;
    public string logoRelativePathUrl;
    public int duration;
    public string createTime;
    public List<Comment> comments;
    public float cellSize;
    public int shareScore;
    public int finishPunchNum;
    public string thumbnailPath;
    public int subject; // 1英语， 2语文， 0强国
}

public class Comment
{
    public string commentContent;
    public string commentPathUrl;
    public int commentType;
    public int duration;
    public int id;
}