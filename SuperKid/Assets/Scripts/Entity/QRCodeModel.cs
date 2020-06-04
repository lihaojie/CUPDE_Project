namespace SuperKid.Entity
{
    public class QRCodeModel
    
    {
        public int planId;
        public int boxId;
        public int content; // 1盒子，2视频，3音频
        public int duration;
        public int id; // content为3时代表音频id
        public string path;
        public string resourceIntroduction;
        public string resourcePath;
        public string resourceTitle;
        public int subject;
        public int month;
        public int resourceId;
    }
}