
using System;
using Sonic853.Udon.CnLunar.Extensions;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.CnLunar
{
    public class CalendarRender : UdonSharpBehaviour
    {
        [SerializeField] Lunar lunar;
        DateTime date;
        /// <summary>
        /// 年
        /// </summary>
        [SerializeField] Text YearTextUI;
        /// <summary>
        /// 月
        /// </summary>
        [SerializeField] Text MonthTextUI;
        /// <summary>
        /// 月（英文
        /// </summary>
        [SerializeField] Text MonthEnTextUI;
        /// <summary>
        /// 日
        /// </summary>
        [SerializeField] Text[] DayTextUIs;
        /// <summary>
        /// 农历
        /// </summary>
        [SerializeField] Text DateTextUI;
        /// <summary>
        /// 农历日
        /// </summary>
        [SerializeField] Text DayCnTextUI;
        /// <summary>
        /// 星期
        /// </summary>
        [SerializeField] Text WeekTextUI;
        /// <summary>
        /// 星期（英文
        /// </summary>
        [SerializeField] Text WeekEnTextUI;
        [SerializeField] Text NextSolarTerm;
        /// <summary>
        /// 宜
        /// </summary>
        [SerializeField] Text GoodsTextUI;
        /// <summary>
        /// 宜忌等第
        /// </summary>
        [SerializeField] Text GoodInfoTextUI;
        /// <summary>
        /// 忌
        /// </summary>
        [SerializeField] Text BadsTextUI;
        /// <summary>
        /// 吉神方位
        /// </summary>
        [SerializeField] Text GodsTextUI;
        /// <summary>
        /// 季节
        /// </summary>
        [SerializeField] Text SeasonTextUI;
        /// <summary>
        /// 生肖冲煞
        /// </summary>
        [SerializeField] Text ZodiacClashTextUI;
        /// <summary>
        /// 纳音
        /// </summary>
        [SerializeField] Text NayinTextUI;
        /// <summary>
        /// 星次
        /// </summary>
        [SerializeField] Text EastZodiacTextUI;
        /// <summary>
        /// 星座
        /// </summary>
        [SerializeField] Text StarZodiacTextUI;
        /// <summary>
        /// 时辰凶吉
        /// </summary>
        [SerializeField] Text TwohourLuckyTextUI;
        /// <summary>
        /// 今日六合
        /// </summary>
        [SerializeField] Text ZodiacMark6TextUI;
        /// <summary>
        /// 今日三合
        /// </summary>
        [SerializeField] Text ZodiacMark3TextUI;
        /// <summary>
        /// 今日胎神
        /// </summary>
        [SerializeField] Text FetalGodTextUI;
        [SerializeField] SkinnedMeshRenderer lunarBody;
        string[] monthsEn = new string[] { "None", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        string[] months = new string[] { "None", "一月大", "二月平", "三月大", "四月小", "五月大", "六月小", "七月大", "八月大", "九月小", "十月大", "十一月小", "十二月大" };
        void Start()
        {
            date = DateTime.Now;
            date = new DateTime(date.Year, date.Month, date.Day);
            lunar.Init(date);
            YearTextUI.text = date.Year.ToString();
            MonthTextUI.text = months[date.Month];
            MonthEnTextUI.text = monthsEn[date.Month];
            for (int i = 0; i < DayTextUIs.Length; i++)
            {
                DayTextUIs[i].text = date.Day.ToString();
            }
            DateTextUI.text = $"农历{lunar.year8Char}[{lunar.chineseYearZodiac}]年{lunar.lunarMonthCn}";
            DayCnTextUI.text = $"{lunar.lunarDayCn}日";
            WeekTextUI.text = lunar.weekDayCn;
            WeekEnTextUI.text = date.DayOfWeek.ToString().ToUpper();
            NextSolarTerm.text = $"{Lunar.GetLunarDayCN(lunar.nextSolarTermsDay)}{lunar.nextSolarTerm}";
            var maxNum = 6;
            var texts = new string[6];
            if (lunar.goodThings.Length < maxNum)
            {
                texts = lunar.goodThings;
            }
            else
            {
                Array.Copy(lunar.goodThings, texts, maxNum);
            }
            GoodsTextUI.text = string.Join("，", texts);
            GoodInfoTextUI.text = lunar.todayLevelName;
            maxNum = 6;
            texts = new string[6];
            if (lunar.badThings.Length < maxNum)
            {
                texts = lunar.badThings;
            }
            else
            {
                Array.Copy(lunar.badThings, texts, maxNum);
            }
            BadsTextUI.text = string.Join("，", texts);
            maxNum = 5;
            texts = new string[5];
            GodsTextUI.text = string.Join("\n", lunar.GetLuckyGodsDirection());
            SeasonTextUI.text = $"<b>季节</b> {lunar.lunarSeason}";
            ZodiacClashTextUI.text = lunar.chineseZodiacClash;
            NayinTextUI.text = $"<b>纳音</b> {lunar.GetNayin()}";
            EastZodiacTextUI.text = $"<b>星次</b> {lunar.todayEastZodiac}";
            StarZodiacTextUI.text = $"<b>星座</b> {lunar.starZodiac}";
            var strings = new string[12];
            // 取每个的最后一个字
            for (var i = 0; i < 12; i++)
            {
                strings[i] = lunar.twohour8CharList[i][lunar.twohour8CharList[i].Length - 1].ToString();
            }
            TwohourLuckyTextUI.text = string.Join(" ", strings);
            TwohourLuckyTextUI.text += "\n";
            var twohourLuckyList = lunar.GetTwohourLuckyList();
            strings = new string[12];
            Array.Copy(twohourLuckyList, strings, 12);
            TwohourLuckyTextUI.text += string.Join(" ", strings);
            ZodiacMark6TextUI.text = $"六合-{lunar.zodiacMark6}";
            ZodiacMark3TextUI.text = $"三合{string.Join("", lunar.zodiacMark3List)}";
            var fetalGod = lunar.GetFetalGod();
            // 在 fetalGod 的第三个字后面加个回车
            FetalGodTextUI.text = fetalGod.Insert(3, "\n");
            if (lunarBody != null)
            {
                DateTime startOfYear = new DateTime(date.Year, 1, 1); // 今年第一天
                DateTime endOfYear = new DateTime(date.Year + 1, 1, 1).AddSeconds(-1); // 今年最后一天的最后一秒
                                                                                       // 计算总时间跨度
                double totalSeconds = (endOfYear - startOfYear).TotalSeconds;
                // 计算当前时间与起点的时间差
                double elapsedSeconds = (date - startOfYear).TotalSeconds;
                var progress = (float)(elapsedSeconds / totalSeconds * 100f);
                // Debug.Log($"totalSeconds:{totalSeconds} elapsedSeconds:{elapsedSeconds} progress:{progress}");
                lunarBody.SetBlendShapeWeight(0, progress);
            }
        }
    }
}
