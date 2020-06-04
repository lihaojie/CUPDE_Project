using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicBookModel
{

    public Data data;

    public class Data
    {
        public class PicBookBean
        {
            /**
               * 绘本编号
               */
            public int id;

            /**
              * 标题
               */
            public string title;

            /**
               * 作者
               */
            public string author;

            /**
              * 出版社
               */
            public string publisher;

            /**
              * 缩略图地址
               */
            public string thumbnailPath;

            /**
              * 科目类型 （1-英语 ；2-语文）
              */
            public int subjectType;

            /**
              * 内容简介
               */
            public string contentIntroduction;

            /**
              * 作者简介
               */
            public string authorIntroduction;

            /**
              * 绘本介绍
              */
            public string picBookIntroduction;

            /**
              * 是否上架（1 -已上架；0-未上架）
              */
            public string isSale;

            /**
            * 创建人
            */
            public int creator;

            /**
            * 创建人
             */
            public string creatorName;

            /**
            * 创建时间
            */
            public DateTime createTime;

            /**
             * 修改人id
            */
            public int modifier;

            /**
            * 修改人
            */
            public string modifierName;

            /**
             * 修改时间
            */
            public DateTime modifyTime;

            /**
            * 缩略图对外访问链接
            */
            public string thumbnailPathUrl;

            /**
            * 领取的计划包是否包含该绘本 1-是 0-否
            */
            public int isGain = 0;
        }
        public List<PicBookBean> records;
    }
    

}
