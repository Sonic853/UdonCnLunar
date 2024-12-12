﻿
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.CnLunar
{
    public class Holidays : UdonSharpBehaviour
    {
        public static DataDictionary LegalsolarTermsHolidayDic() => new DataDictionary() {
            {
                "清明", "清明节"
            },
        };
        public static DataDictionary LegalHolidaysDic() => new DataDictionary() {
            {
                "1,1", "元旦节"
            },
            {
                "5,1", "元旦节"
            },
            {
                "10,1", "元旦节"
            },
        };
        public static DataDictionary LegalLunarHolidaysDic() => new DataDictionary() {
            {
                "1,1", "春节"
            },
            {
                "5,5", "端午节"
            },
            {
                "8,15", "中秋节"
            },
        };
        public static DataList OtherHolidaysList() => new DataList() {
            new DataDictionary() {
                { 8, "周恩来逝世纪念日" },
                { 10, "中国公安110宣传日" },
                { 21, "列宁逝世纪念日" },
                { 26, "国际海关日" },
            }, // 1月
            new DataDictionary() {
                {2, "世界湿地日" },
                {4, "世界抗癌日" },
                {7, "京汉铁路罢工纪念" },
                {10, "国际气象节" },
                {14, "情人节" },
                {19, "邓小平逝世纪念日" },
                {21, "国际母语日" },
                {24, "第三世界青年日"},
            },
            new DataDictionary() {
                {1, "国际海豹日" },
                {3, "全国爱耳日" },
                {5, "周恩来诞辰纪念日,中国青年志愿者服务日" },
                {6, "世界青光眼日" },
                {8, "国际劳动妇女节" },
                {12, "孙中山逝世纪念日,中国植树节" },
                {14, "马克思逝世纪念日" },
                {15, "国际消费者权益日" },
                {17, "国际航海日" },
                {18, "全国科技人才活动日" },
                {21, "世界森林日,世界睡眠日" },
                {22, "世界水日" },
                {23, "世界气象日" },
                {24, "世界防治结核病日"},
            },
            new DataDictionary() {
                {1, "国际愚人节" },
                {2, "国际儿童图书日" },
                {7, "世界卫生日" },
                {22, "列宁诞辰纪念日" },
                {23, "世界图书和版权日" },
                {26, "世界知识产权日"},
            },
            new DataDictionary() {
                {3, "世界新闻自由日" },
                {4, "中国青年节" },
                {5, "马克思诞辰纪念日" },
                {8, "世界红十字日" },
                {11, "世界肥胖日" },
                {23, "世界读书日" },
                {27, "上海解放日" },
                {31, "世界无烟日"},
            },
            new DataDictionary() {
                {1, "国际儿童节" },
                {5, "世界环境日" },
                {6, "全国爱眼日" },
                {8, "世界海洋日" },
                {11, "中国人口日" },
                {14, "世界献血日"},
            },
            new DataDictionary() {
                {1, "中国共产党诞生日,香港回归纪念日" },
                {7, "中国人民抗日战争纪念日" },
                {11, "世界人口日"},
            },
            new DataDictionary() {
                {1, "中国人民解放军建军节" },
                {5, "恩格斯逝世纪念日" },
                {6, "国际电影节" },
                {12, "国际青年日" },
                {22, "邓小平诞辰纪念日"},
            },
            new DataDictionary() {
                {3, "中国抗日战争胜利纪念日" },
                {8, "世界扫盲日" },
                {9, "毛泽东逝世纪念日" },
                {10, "中国教师节" },
                {14, "世界清洁地球日" },
                {18, "“九·一八”事变纪念日" },
                {20, "全国爱牙日" },
                {21, "国际和平日" },
                {27, "世界旅游日"},
            },
            new DataDictionary() {
                {4, "世界动物日" },
                {10, "辛亥革命纪念日" },
                {13, "中国少年先锋队诞辰日" },
                {25, "抗美援朝纪念日"},
            },
            new DataDictionary() {
                {12, "孙中山诞辰纪念日" },
                {28, "恩格斯诞辰纪念日"},
            },
            new DataDictionary() {
                {1, "世界艾滋病日" },
                {12, "西安事变纪念日" },
                {13, "南京大屠杀纪念日" },
                {24, "平安夜" },
                {25, "圣诞节" },
                {26, "毛泽东诞辰纪念日"},
            }, // 12月
        };
        // 复活节:每年春分后月圆第一个星期天  母亲节:每年5月份的第2个星期日  父亲节:每年6月份的第3个星期天感恩节 每年11月最后一个星期四
        public static DataDictionary OtherEastHolidaysList() => new DataDictionary() {
            {
                "5,2,7", "母亲节"
            },
            {
                "6,3,7", "父亲节"
            },
        };
        public static DataList OtherLunarHolidaysList() => new DataList() {
            new DataDictionary() {
                {1, "弥勒佛圣诞"},
                {8, "五殿阎罗天子诞"},
                {9, "玉皇上帝诞"},
                {15, "元宵节"},
            }, // 1月
            new DataDictionary() {
                {1, "一殿秦广王诞"},
                {2, "春龙节-福德土地正神诞"},
                {3, "文昌帝君诞"},
                {6, "东华帝君诞"},
                {8, "释迦牟尼佛出家"},
                {15, "释迦牟尼佛般涅槃"},
                {17, "东方杜将军诞"},
                {18, "至圣先师孔子讳辰"},
                {19, "观音大士诞"},
                {21, "普贤菩萨诞"},
            },
            new DataDictionary() {
                {1, "二殿楚江王诞"},
                {3, "三月三-玄天上帝诞"},
                {8, "六殿卞城王诞"},
                {15, "昊天上帝诞"},
                {16, "准提菩萨诞"},
                {19, "中岳大帝诞"},
                {20, "子孙娘娘诞"},
                {27, "七殿泰山王诞"},
                {28, "苍颉至圣先师诞"},
            },
            new DataDictionary() {
                {1, "八殿都市王诞"},
                {4, "文殊菩萨诞"},
                {8, "释迦牟尼佛诞"},
                {14, "纯阳祖师诞"},
                {15, "钟离祖师诞"},
                {17, "十殿转轮王诞"},
                {18, "紫徽大帝诞"},
                {20, "眼光圣母诞"},
            },
            new DataDictionary() {
                {1, "南极长生大帝诞"},
                {8, "南方五道诞"},
                {11, "天下都城隍诞"},
                {12, "炳灵公诞"},
                {13, "关圣降"},
                {16, "天地元气造化万物之辰"},
                {18, "张天师诞"},
                {22, "孝娥神诞"},
            },
            new DataDictionary() {
                {19, "观世音菩萨成道日"},
                {24, "关帝诞"},
            },
            new DataDictionary() {
                {7, "七夕-魁星诞"},
                {13, "长真谭真人诞-大势至菩萨诞"},
                {15, "中元节"},
                {18, "西王母诞"},
                {19, "太岁诞"},
                {22, "增福财神诞"},
                {29, "杨公忌"},
                {30, "地藏菩萨诞"},
            },
            new DataDictionary() {
                {1, "许真君诞"},
                {3, "司命灶君诞"},
                {5, "雷声大帝诞"},
                {10, "北斗大帝诞"},
                {12, "西方五道诞"},
                {16, "天曹掠刷真君降"},
                {18, "天人兴福之辰"},
                {23, "汉恒候张显王诞"},
                {24, "灶君夫人诞"},
                {29, "至圣先师孔子诞"},
            },
            new DataDictionary() {
                {1, "北斗九星降世"},
                {3, "五瘟神诞"},
                {9, "重阳节-酆都大帝诞"},
                {13, "孟婆尊神诞"},
                {17, "金龙四大王诞"},
                {19, "观世音菩萨出家"},
                {30, "药师琉璃光佛诞"},
            },
            new DataDictionary() {
                {1, "寒衣节"},
                {3, "三茅诞"},
                {5, "达摩祖师诞"},
                {8, "佛涅槃日"},
                {15, "下元节"},
            },
            new DataDictionary() {
                {4, "至圣先师孔子诞"},
                {6, "西岳大帝诞"},
                {11, "太乙救苦天尊诞"},
                {17, "阿弥陀佛诞"},
                {19, "太阳日宫诞"},
                {23, "张仙诞"},
                {26, "北方五道诞"},
            },
            new DataDictionary() {
                {8, "腊八节-释迦如来成佛之辰"},
                {16, "南岳大帝诞"},
                {21, "天猷上帝诞"},
                {23, "小年"},
                {24, "子时灶君上天朝玉帝"},
                {29, "华严菩萨诞"},
                {30, "除夕"},
            }, // 12月
        };
    }
}
