using System.Collections.Generic;

namespace SuperKid.Entity
{
    public class MedalModel
    {

     public int totalDays;
     
     public int medalShareCount;

     public int studyShareCount;

     public int medalCount;

     public List<MedalBean> medalList;

     public class MedalBean
        {
            
            /*
             * 是否领取
             */
            public int isDraw;
        
        
            /*
             * 是否分享 0未分享1已分享
             */
            public int isShare;

            /*
             * 勋章等级
             */
            public int medalGrade;
        
            /*
             * 勋章对应的学习天数
             */
            public int medalDays;
        
            /*
             * 勋章 name
             */
            public string medalName;

        
            /*
             * 勋章积分
             */
            public int medalScore;

        
            /*
             * 宝宝 ID
             */
            public int relBabyId;

        
            /*
             * 勋章分享得的积分
             */
            public int medalShareScore;
        }
        
    }
}