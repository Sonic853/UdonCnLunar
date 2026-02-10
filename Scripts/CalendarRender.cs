
using System;
using Sonic853.Udon.CnLunar.Extensions;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.CnLunar
{
    public class CalendarRender : UdonSharpBehaviour
    {
        [SerializeField] bool run = false;
        [SerializeField] string testDate = "";
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
        /// 日期Obj
        /// </summary>
        [SerializeField] GameObject[] DateObjs;
        /// <summary>
        /// 日
        /// </summary>
        [SerializeField] Text[] DayTextUIs;
        /// <summary>
        /// 节日
        /// </summary>
        [SerializeField] Text[] HolidayTextUIs;
        /// <summary>
        /// 节日图
        /// </summary>
        [SerializeField] Image[] HolidayImageUIs;
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
        public SkinnedMeshRenderer[] lunarBodys;
        // 01963B
        public Color32 normalColor = new Color32(1, 150, 59, 255);
        public Color32 holidaysColor = new Color32(253, 40, 21, 255);
        [SerializeField] Image[] images;
        [SerializeField] Text[] texts;
        public CalendarHolidayInfo[] calendarHolidayInfos;
        DataDictionary calendarHolidayData = new DataDictionary();
        DataDictionary lunarHolidayData = new DataDictionary();
        DataDictionary solarTermData = new DataDictionary();
        string[] monthsEn = new string[] { "None", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        string[] months = new string[] { "None", "一月大", "二月平", "三月大", "四月小", "五月大", "六月小", "七月大", "八月大", "九月小", "十月大", "十一月小", "十二月大" };
        void Start()
        {
            if (run) Run();
        }
        void Update()
        {
            // 如果今天过了一天，更新UI
            if (string.IsNullOrEmpty(testDate) && date.Day != DateTime.Now.Day) UpdateUI();
        }
        public void Run() => Run("");
        public void Run(string _testDate)
        {
            testDate = _testDate;
            for (var i = 0; i < calendarHolidayInfos.Length; i++)
            {
                var item = calendarHolidayInfos[i];
                if (item.month != 0 && item.day != 0)
                {
                    if (!item.isLunarHoliday) calendarHolidayData.Add($"{item.month},{item.day}", i);
                    else lunarHolidayData.Add($"{item.month},{item.day}", i);
                    if (!string.IsNullOrEmpty(item.solarTerm) && !solarTermData.ContainsKey(item.solarTerm)) solarTermData.Add(item.solarTerm, i);
                }
                else if (!string.IsNullOrEmpty(item.solarTerm) && !solarTermData.ContainsKey(item.solarTerm)) solarTermData.Add(item.solarTerm, i);
            }
            if (string.IsNullOrEmpty(testDate)) UpdateUI();
            else
            {
                DateTime.TryParse(testDate, out date);
                UpdateUI(date);
            }
        }
        void UpdateUI() => UpdateUI(DateTime.Now);
        void UpdateUI(DateTime _date)
        {
            if (_date == null) _date = DateTime.Now;
            date = new DateTime(_date.Year, _date.Month, _date.Day);
            lunar.Init(date);

            // 文字更新
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
            // 下一个节气农历日期
            var nextSolarTermText = "明日";
            if (lunar.nextSolarTermsDay - 1 != date.Day)
            {
                Lunar.GetLunarDateNum(
                    new DateTime(lunar.nextSolarTermYear, lunar.nextSolarTermsMonth, lunar.nextSolarTermsDay),
                    out var nextSolarTermYearLunar,
                    out var nextSolarTermMonthLunar,
                    out var nextSolarTermDayLunar,
                    out var ____, out var __, out var ___
                );
                nextSolarTermText = Lunar.GetLunarDayCN(nextSolarTermDayLunar);
            }
            NextSolarTerm.text = $"{nextSolarTermText}{lunar.nextSolarTerm}";
            var maxNum = 6;
            var _texts = new string[6];
            if (lunar.goodThings.Length < maxNum)
            {
                _texts = lunar.goodThings;
            }
            else
            {
                Array.Copy(lunar.goodThings, _texts, maxNum);
            }
            GoodsTextUI.text = string.Join("，", _texts);
            GoodInfoTextUI.text = lunar.todayLevelName;
            maxNum = 6;
            _texts = new string[6];
            if (lunar.badThings.Length < maxNum)
            {
                _texts = lunar.badThings;
            }
            else
            {
                Array.Copy(lunar.badThings, _texts, maxNum);
            }
            BadsTextUI.text = string.Join("，", _texts);
            // maxNum = 5;
            // _texts = new string[5];
            GodsTextUI.text = string.Join("\n", lunar.GetLuckyGodsDirection());
            SeasonTextUI.text = $"<b>季节</b> {lunar.lunarSeasonName}";
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

            // 日历颜色
            var holidays = lunar.GetLegalHolidays();
            if (holidays.Length > 0 || date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                foreach (var image in images)
                    image.color = holidaysColor;
                foreach (var text in texts)
                    text.color = holidaysColor;
            }
            else
            {
                foreach (var image in images)
                    image.color = normalColor;
                foreach (var text in texts)
                    text.color = normalColor;
            }
            var holidayText = holidays.Length > 0 ? holidays[0] : lunar.todaySolarTerms != "无" ? lunar.todaySolarTerms : "";
            if (!string.IsNullOrEmpty(holidayText) && holidayText != "无")
            {
                foreach (var holidayUI in HolidayTextUIs)
                {
                    holidayUI.text = holidayText;
                }
            }
            else
            {
                foreach (var holidayUI in HolidayTextUIs)
                {
                    holidayUI.text = "";
                }
            }
            foreach (var dateObj in DateObjs)
            {
                dateObj.SetActive(false);
            }
            DateObjs[0].SetActive(true);

            var holidayInfo = GetHolidayInfo(lunar) ?? GeHolidayText(holidayText);
            Debug.Log($"holidayInfo:{holidayInfo != null}");
            if (holidayInfo != null)
            {
                if (holidayInfo.image != null)
                {
                    foreach (var image in HolidayImageUIs)
                    {
                        image.sprite = holidayInfo.image;
                        if (holidayInfo.useImageColor) image.color = Color.white;
                    }
                }
                var template = holidayInfo.template + 1;
                for (var i = 0; i < DateObjs.Length; i++)
                {
                    DateObjs[i].SetActive(i == template);
                }
            }

            if (lunarBodys.Length > 0)
            {
                DateTime startOfYear = new DateTime(date.Year, 1, 1); // 今年第一天
                DateTime endOfYear = new DateTime(date.Year + 1, 1, 1).AddSeconds(-1); // 今年最后一天的最后一秒
                                                                                       // 计算总时间跨度
                double totalSeconds = (endOfYear - startOfYear).TotalSeconds;
                // 计算当前时间与起点的时间差
                double elapsedSeconds = (date - startOfYear).TotalSeconds;
                var progress = (float)(elapsedSeconds / totalSeconds * 100f);
                // Debug.Log($"totalSeconds:{totalSeconds} elapsedSeconds:{elapsedSeconds} progress:{progress}");
                foreach (var lunarBody in lunarBodys)
                    lunarBody.SetBlendShapeWeight(0, progress);
            }
        }
        CalendarHolidayInfo GetHolidayInfo(Lunar lunar)
        {
            var holidayInfo = GetCalendarHolidayInfoLunar(lunar.lunarMonth, lunar.lunarDay);
            if (holidayInfo != null) return holidayInfo;
            holidayInfo = GetCalendarHolidayInfo(date.Month, date.Day);
            if (holidayInfo != null) return holidayInfo;
            if (lunar.todaySolarTerms != "无") holidayInfo = GetSolarTerm(lunar.todaySolarTerms);
            return holidayInfo;
        }
        CalendarHolidayInfo GetCalendarHolidayInfo(int month, int day)
        {
            if (calendarHolidayData.TryGetValue($"{month},{day}", out var index))
            {
                return calendarHolidayInfos[index.Int];
            }
            return null;
        }
        CalendarHolidayInfo GetCalendarHolidayInfoLunar(int month, int day)
        {
            if (lunarHolidayData.TryGetValue($"{month},{day}", out var index))
            {
                return calendarHolidayInfos[index.Int];
            }
            return null;
        }
        CalendarHolidayInfo GetSolarTerm(string solarTerm)
        {
            if (string.IsNullOrEmpty(solarTerm)) return null;
            if (solarTermData.TryGetValue(solarTerm, out var index))
            {
                return calendarHolidayInfos[index.Int];
            }
            return null;
        }
        CalendarHolidayInfo GeHolidayText(string solarTerm) => GetSolarTerm(solarTerm);
    }
}
