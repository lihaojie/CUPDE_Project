using System.Collections.Generic;

namespace SuperKid.Entity
{
    public class BabyScoreExchangeModel
    {
        public int current;
        public int pages;
        public bool searchCount;
        public int size;
        public int total;
        public List<IntegralExchangeModel> orders;
        public List<IntegralExchangeModel> records;
        
        
    }
    
    
    
    public class IntegralExchangeModel
    {

        public int babyId;
        public string createTime;
        public string icon;
        public int id;
        public string name;
        public int score;
        public int type;
        public int orderId;
    }
}