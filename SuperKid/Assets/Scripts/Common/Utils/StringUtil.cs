using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using QFramework;

namespace SuperKid.Utils
{
    public class StringUtil
    {
        public static bool checkPassword(string password)
        {
            string pattern = "^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{8,50}$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(password);
        }

        public static string SecondsToMinutes(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            string str = "";
            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00") ;
            }

            if (ts.Hours == 0)
            {
                str = ts.Minutes.ToString("00") + ":"  + ts.Seconds.ToString("00");
            }

            return str;
        }

        //2011-03-30
        public static string GetBeforeTime(string time)
        {
            if (time.IsNullOrEmpty())
            {
                return "";
            }
            DateTime nowTime = DateTime.Now;
            DateTime beforeTime = DateTime.Parse(time);
            TimeSpan ts1 = new TimeSpan(nowTime.Ticks);
            TimeSpan ts2 = new TimeSpan(beforeTime.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            if (ts.Days>0)
            {
                return ts.Days + "天之前";
            }
            if (ts.Hours > 0)
            {
                return ts.Hours + "小时之前";
            }
            if (ts.Minutes > 0)
            {
                return ts.Minutes + "分钟之前";
            }
            if (ts.Seconds > 0)
            {
                return ts.Seconds + "秒之前";
            }
            return "刚刚";
        } 
        
        public static string GetBirthdayTime(string time)
        {
            if (time.IsNullOrEmpty())
            {
                return "";
            }
            DateTime nowTime = new DateTime();
            DateTime beforeTime = DateTime.ParseExact(time, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            int Year = nowTime.Year - beforeTime.Year;
            int Month = (nowTime.Year - beforeTime.Year) * 12 + (nowTime.Month - beforeTime.Month);
            if (Year >  0)
            {
                return Year+"岁" + Month + "个月";
            }
            return Month + "个月";
        }

        /// <summary>
        /// 计算两个日期的时间间隔
        /// </summary>
        /// <param name="DateTime1">第一个日期和时间</param>
        /// <param name="DateTime2">第二个日期和时间</param>
        /// <returns></returns>
        private string DateDiffString(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
           
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = ts.Days+"天"
                              + ts.Hours+"小时"
                              + ts.Minutes+"分钟"
                              + ts.Seconds+"秒";
           
            return dateDiff;
        } 
        
        /**
         * 过滤emoji
         */
        public static string Emoji(string snick)
        {
            if (snick.IsNullOrEmpty())
            {
                return "";
            }
            List<string> patten = new List<string>();
            patten.Add(@"\p{Cs}");
            patten.Add(@"\p{Co}");
            for (int i = 0; i < patten.Count; i++)
            {
                snick= Regex.Replace(snick, patten[i], "");//屏蔽emoji   
            }
            return snick;
        }
    }
}