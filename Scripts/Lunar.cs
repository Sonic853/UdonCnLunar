// coding=UTF-8
// 1901~2100年农历数据表
// author: cuba3, github: https://github.com/opn48/pylunar
// Sonic853, github: https://github.com/Sonic853
// base code by Yovey , https://www.jianshu.com/p/8dc0d7ba2c2a
// powered by Late Lee, http://www.latelee.org/python/python-yangli-to-nongli.html#comment-78
// other author:Chen Jian, http://www.cnblogs.com/chjbbs/p/5704326.html
// 数据来源: http://data.weather.gov.hk/gts/time/conversion1_text_c.htm
using System;
using System.Text;
using Sonic853.Udon.ArrayPlus;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.CnLunar
{
    public class Lunar : UdonSharpBehaviour
    {
        DateTime date = DateTime.Now;
        string godType = "8char";
        string year8Char = "year";
        string month8Char = "";
        string day8Char = "";
        int twohourNum = -1;
        bool isLunarLeapMonth = false;
        int spanDays = -1;
        int[] monthDaysList = new int[0];
        string phaseOfMoon = "";
        string todaySolarTerms = "";
        int[] thisYearSolarTermsMonth = new int[0];
        int[] thisYearSolarTermsDay = new int[0];
        int nextSolarNum = -1;
        string nextSolarTerm = "";
        int nextSolarTermYear = -1;
        int nextSolarTermsMonth = -1;
        int nextSolarTermsDay = -1;
        int _x = -1;
        int dayHeavenlyEarthNum = -1;
        int yearEarthNum = -1;
        int monthEarthNum = -1;
        int dayEarthNum = -1;
        int yearHeavenNum = -1;
        int monthHeavenNum = -1;
        int dayHeavenNum = -1;
        int seasonType = -1;
        int seasonNum = -1;
        string lunarSeason = "";
        void Start()
        {
            Init(DateTime.Now);
        }
        public void Init(DateTime _date, string _godType = "8char", string _year8Char = "year")
        {
            if (_date == null) _date = DateTime.Now;
            date = _date;
            godType = _godType;
            year8Char = _year8Char;
            twohourNum = date.Hour + 1;
            isLunarLeapMonth = false;
            GetLunarDateNum(
                date,
                out var lunarYear,
                out var lunarMonth,
                out var lunarDay,
                out isLunarLeapMonth,
                out spanDays,
                out monthDaysList
            );
            Debug.Log($"year:{lunarYear}, month:{lunarMonth}, day:{lunarDay}, isLunarLeapMonth:{isLunarLeapMonth}, spanDays:{spanDays}");
            GetLunarCn(
                lunarYear,
                lunarMonth,
                lunarDay,
                isLunarLeapMonth,
                monthDaysList,
                out var lunarYearCn,
                out var lunarMonthCn,
                out var lunarDayCn,
                out var lunarMonthLong
            );
            Debug.Log($"lunarYearCn:{lunarYearCn}, lunarMonthCn:{lunarMonthCn}, lunarDayCn:{lunarDayCn}, lunarMonthLong:{lunarMonthLong}");
            phaseOfMoon = GetPhaseOfMoon(lunarDay, lunarMonthLong);
            todaySolarTerms = GetTodaySolarTerms(
                _date,
                out thisYearSolarTermsMonth,
                out thisYearSolarTermsDay,
                out nextSolarNum,
                out nextSolarTerm,
                out nextSolarTermYear,
                out nextSolarTermsMonth,
                out nextSolarTermsDay
            );
            // 立春干支参数
            _x = GetBeginningOfSpringX(nextSolarNum, spanDays, _year8Char);

            GetThe8char(
                date,
                lunarYear,
                _x,
                nextSolarNum,
                twohourNum,
                out year8Char,
                out month8Char,
                out day8Char,
                out dayHeavenlyEarthNum
            );
            GetEarthNum(
                year8Char,
                month8Char,
                day8Char,
                out yearEarthNum,
                out monthEarthNum,
                out dayEarthNum
            );
            GetHeavenNum(
                year8Char,
                month8Char,
                day8Char,
                out yearHeavenNum,
                out monthHeavenNum,
                out dayHeavenNum
            );
            GetSeason(
                monthEarthNum,
                out seasonType,
                out seasonNum,
                out lunarSeason
            );
            
        }
        public static int GetBeginningOfSpringX(
            int nextSolarNum,
            int spanDays,
            string _year8Char
        )
        {
            var isBeforBeginningOfSpring = nextSolarNum < 3;
            var isBeforLunarYear = spanDays < 0;
            var _x = 0;
            if (_year8Char != "beginningOfSpring")
                return _x;
            // 现在节气在立春之前 且 已过完农历年(农历小于3月作为测试判断)，年柱需要减1
            if (isBeforLunarYear)
            {
                if (!isBeforBeginningOfSpring)
                    _x = -1;
            }
            else
            {
                if (isBeforBeginningOfSpring)
                    _x = 1;
            }
            return _x;
        }
        public static string GetLunarYearCN(int lunarYear)
        {
            var lunarYearStr = lunarYear.ToString();
            var upperYear = new StringBuilder();
            foreach (var i in lunarYearStr)
            {
                upperYear.Append(i);
            }
            return upperYear.ToString();
        }
        public static string GetLunarMonthCN(int lunarMonth, bool isLunarLeapMonth, int[] monthDaysList, out bool lunarMonthLong)
        {
            var lunarMonthStr = Config.LunarMonthNameList()[(lunarMonth - 1) % 12];
            var thisLunarMonthDays = monthDaysList[0];
            if (isLunarLeapMonth)
            {
                lunarMonthStr = $"闰${lunarMonthStr}";
                thisLunarMonthDays = monthDaysList[2];
            }
            lunarMonthLong = thisLunarMonthDays >= 30;
            var s = lunarMonthLong ? "大" : "小";
            return $"{lunarMonthStr}{s}";
        }
        public static string GetLunarCn(int lunarYear, int lunarMonth, int lunarDay, bool isLunarLeapMonth, int[] monthDaysList, out string lunarYearCn, out string lunarMonthCn, out string lunarDayCn, out bool lunarMonthLong)
        {
            lunarYearCn = GetLunarYearCN(lunarYear);
            lunarMonthCn = GetLunarMonthCN(lunarMonth, isLunarLeapMonth, monthDaysList, out lunarMonthLong);
            lunarDayCn = Config.LunarDayNameList()[(lunarDay - 1) % 30];
            return $"{lunarYearCn}年 ${lunarMonthCn}${lunarDayCn}";
        }
        /// <summary>
        /// 月相
        /// </summary>
        /// <param name="lunarDay"></param>
        /// <param name="lunarMonthLong"></param>
        /// <returns></returns>
        public static string GetPhaseOfMoon(int lunarDay, bool lunarMonthLong)
        {
            if (lunarDay - (lunarMonthLong ? 1 : 0) == 15)
            {
                return "望";
            }
            else if (lunarDay == 1)
            {
                return "朔";
            }
            else if (lunarDay >= 7 && lunarDay < 9)
            {
                return "上弦";
            }
            else if (lunarDay >= 22 && lunarDay < 24)
            {
                return "下弦";
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 农历月数
        /// 计算阴历月天数
        /// </summary>
        /// <param name="lunarYear">农历年</param>
        /// <param name="monthDay">30或29</param>
        /// <param name="leapMonth">该年闰月</param>
        /// <param name="leapDays">闰月天数</param>
        public static void GetMonthLeapMonthLeapDays(int lunarYear, int lunarMonth, out int monthDay, out int leapMonth, out int leapDays, out int[] monthDaysList)
        {
            // 闰几月，该月多少天 传入月份多少天
            monthDay = 0;
            leapMonth = 0;
            leapDays = 0;
            // 获取16进制数据 12-1月份农历日数 0=29天 1=30天
            var tmp = Config.LunarMonthData()[lunarYear - Config.START_YEAR()];
            // 表示获取当前月份的布尔值:指定二进制1（假定真），向左移动月数-1，与当年全年月度数据合并取出2进制位作为判断
            if ((tmp & (1 << (lunarMonth - 1))) != 0)
            {
                monthDay = 30;
            }
            else
            {
                monthDay = 29;
            }
            // 闰月
            leapMonth = (tmp >> Config.LEAPMONTH_NUM_BIT()) & 0xf;
            if (leapMonth != 0)
            {
                if ((tmp & (1 << Config.MONTH_DAY_BIT())) != 0)
                {
                    leapDays = 30;
                }
                else
                {
                    leapDays = 29;
                }
            }
            monthDaysList = new int[] { monthDay, leapMonth, leapDays };
        }
        public static void GetLunarDateNum(DateTime _date, out int lunarYear, out int lunarMonth, out int lunarDay) => GetLunarDateNum(_date, out lunarYear, out lunarMonth, out lunarDay, out _, out _, out _);
        /// <summary>
        /// 获取数字形式的农历日期
        /// 返回的月份，高4bit为闰月月份，低4bit为其它正常月份
        /// </summary>
        /// <param name="_date">日期</param>
        /// <param name="lunarYear">农历年</param>
        /// <param name="lunarMonth">农历月</param>
        /// <param name="lunarDay">农历日</param>
        public static void GetLunarDateNum(DateTime _date, out int lunarYear, out int lunarMonth, out int lunarDay, out bool isLunarLeapMonth, out int spanDays, out int[] monthDaysList)
        {
            lunarYear = _date.Year;
            lunarMonth = 1;
            lunarDay = 1;
            var codeYear = Config.LunarNewYearList()[lunarYear - Config.START_YEAR()];
            // 获取当前日期与当年春节的差日
            var _spanDays = (_date - new DateTime(lunarYear, (codeYear >> 5) & 0x3, (codeYear >> 0) & 0x1f)).Days;
            spanDays = _spanDays;
            isLunarLeapMonth = false;
            // var _ = 0;
            if (_spanDays >= 0)
            {
                // 新年后推算日期，差日依序减月份天数，直到不足一个月，剪的次数为月数，剩余部分为日数
                // 先获取闰月
                // 可从迭代递归修改为数学加和    待优化
                GetMonthLeapMonthLeapDays(lunarYear, lunarMonth, out var monthDay, out var leapMonth, out var leapDays, out monthDaysList);
                while (_spanDays >= monthDay)
                {
                    // 获取当前月份天数，从差日中扣除
                    _spanDays -= monthDay;
                    if (lunarMonth == leapMonth)
                    {
                        // 如果当月还是闰月
                        monthDay = leapDays;
                        if (_spanDays < monthDay)
                        {
                            // 指定日期在闰月中
                            isLunarLeapMonth = true;
                            break;
                        }
                        // 否则扣除闰月天数，月份加一
                        _spanDays -= monthDay;
                    }
                    lunarMonth++;
                    // GetMonthLeapMonthLeapDays(lunarYear, lunarMonth, out monthDay, out _, out _); // 兄啊，我开始汗流浃背了
                    GetMonthLeapMonthLeapDays(lunarYear, lunarMonth, out monthDay, out leapMonth, out leapDays, out monthDaysList);
                }
                lunarDay += _spanDays;
            }
            else
            {
                // 新年前倒推去年日期
                lunarMonth = 12;
                lunarYear--;
                GetMonthLeapMonthLeapDays(lunarYear, lunarMonth, out var monthDay, out var leapMonth, out var leapDays, out monthDaysList);
                while (Mathf.Abs(_spanDays) >= monthDay)
                {
                    _spanDays += monthDay;
                    lunarMonth--;
                    if (lunarMonth == leapMonth)
                    {
                        monthDay = leapDays;
                        // 指定日期在闰月中
                        if (Mathf.Abs(_spanDays) <= monthDay)
                        {
                            isLunarLeapMonth = true;
                            break;
                        }
                        _spanDays += monthDay;
                    }
                    // GetMonthLeapMonthLeapDays(lunarYear, lunarMonth, out monthDay, out _, out _);
                    GetMonthLeapMonthLeapDays(lunarYear, lunarMonth, out monthDay, out leapMonth, out leapDays, out monthDaysList);
                }
                // 从月份总数中倒扣 得到天数
                lunarDay += monthDay + _spanDays;
            }
        }
        public static void GetSolarTermsDateList(int year, out int[] solarTermsMonth, out int[] solarTermsDay)
        {
            var solarTermsList = Solar24.GetTheYearAllSolarTermsList(year);
            solarTermsDay = new int[solarTermsList.Length];
            solarTermsMonth = new int[solarTermsList.Length];
            for (var i = 0; i < solarTermsList.Length; i++)
            {
                var day = solarTermsList[i];
                var month = i / 2 + 1;
                solarTermsDay[i] = (int)day;
                solarTermsMonth[i] = month;
            }
        }
        public static int GetNextNum(
            int findMonth,
            int findDay,
            int[] solarTermsMonth,
            int[] solarTermsDay
        )
        {
            // nextSolarNum = len(list(filter(lambda y: y <= findDate, solarTermsDateList))) % 24
            // var nextSolarNum = 0;
            // for (var i = 0; i < solarTermsMonth.Length; i++)
            // {
            //     if (solarTermsMonth[i] >= findMonth && solarTermsDay[i] >= findDay)
            //     {
            //         nextSolarNum = i;
            //         break;
            //     }
            // }
            // return nextSolarNum % 24;
            var findDate = findMonth * 100 + findDay;
            var nextSolarNum = 0;
            for (var i = 0; i < solarTermsMonth.Length; i++)
            {
                var date = solarTermsMonth[i] * 100 + solarTermsDay[i];
                if (date <= findDate)
                    nextSolarNum++;
            }
            return nextSolarNum % 24;
        }
        public static int GetIndexInYearSolarTerms(int findMonth, int findDay, int[] solarTermsMonth, int[] solarTermsDay)
        {
            for (var i = 0; i < solarTermsMonth.Length; i++)
            {
                if (solarTermsMonth[i] == findMonth && solarTermsDay[i] == findDay)
                    return i;
            }
            return -1;
        }
        public static string GetTodaySolarTerms(
            DateTime _date,
            out int[] thisYearSolarTermsMonth,
            out int[] thisYearSolarTermsDay,
            out int nextSolarNum,
            out string nextSolarTerm,
            out int nextSolarTermYear,
            out int nextSolarTermsMonth,
            out int nextSolarTermsDay
        )
        {
            var year = _date.Year;
            GetSolarTermsDateList(year, out thisYearSolarTermsMonth, out thisYearSolarTermsDay);
            var findMonth = _date.Month;
            var findDay = _date.Day;
            nextSolarNum = GetNextNum(findMonth, findDay, thisYearSolarTermsMonth, thisYearSolarTermsDay);
            var index = GetIndexInYearSolarTerms(findMonth, findDay, thisYearSolarTermsMonth, thisYearSolarTermsDay);
            var SOLAR_TERMS_NAME_LIST = Config.SOLAR_TERMS_NAME_LIST();
            var todaySolarTerm = index != -1 ? SOLAR_TERMS_NAME_LIST[index] : "无";
            // 次年节气
            if (findMonth == thisYearSolarTermsMonth[thisYearSolarTermsMonth.Length - 1] && findDay == thisYearSolarTermsDay[thisYearSolarTermsDay.Length - 1])
            {
                year++;
                GetSolarTermsDateList(year, out thisYearSolarTermsMonth, out thisYearSolarTermsDay);
            }
            nextSolarTerm = SOLAR_TERMS_NAME_LIST[nextSolarNum];
            nextSolarTermsMonth = thisYearSolarTermsMonth[nextSolarNum];
            nextSolarTermsDay = thisYearSolarTermsDay[nextSolarNum];
            nextSolarTermYear = year;
            return todaySolarTerm;
        }
        /// <summary>
        /// 八字部分
        /// </summary>
        /// <param name="lunarYear"></param>
        /// <param name="_x"></param>
        /// <returns></returns>
        public static string GetYear8Char(int lunarYear, int _x)
        {
            // 立春年干争议算法
            return Config.The60HeavenlyEarth()[(lunarYear - 4) % 60 - _x];
        }
        /// <summary>
        /// 月八字与节气相关
        /// </summary>
        /// <param name="nextSolarNum"></param>
        /// <param name="_x"></param>
        /// <returns></returns>
        public static string GetMonth8Char(DateTime _date, int nextSolarNum)
        {
            var nextNum = nextSolarNum;
            // 2019年正月为丙寅月
            if (nextNum == 0 && _date.Month == 12)
            {
                nextNum = 24;
            }
            var apartNum = (nextNum + 1) / 2;
            // (year-2019)*12+apartNum每年固定差12个月回到第N年月柱，2019小寒甲子，加上当前过了几个节气除以2+(nextNum-1)//2，减去1
            return Config.The60HeavenlyEarth()[((_date.Year - 2019) * 12 + apartNum) % 60];
        }
        public static string GetDay8Char(DateTime _date, int twohourNum, out int dayHeavenlyEarthNum)
        {
            var apart = _date - new DateTime(2019, 1, 29);
            var the60HeavenlyEarth = Config.The60HeavenlyEarth();
            var baseNum = Array.IndexOf(the60HeavenlyEarth, "丙寅");
            if (twohourNum == 12)
            {
                baseNum++;
            }
            dayHeavenlyEarthNum = (apart.Days + baseNum) % 60;
            return the60HeavenlyEarth[dayHeavenlyEarthNum];
        }
        public static void GetThe8char(DateTime _date, int lunarYear, int _x, int nextSolarNum, int twohourNum, out string year8Char, out string month8Char, out string day8Char, out int dayHeavenlyEarthNum)
        {
            year8Char = GetYear8Char(lunarYear, _x);
            month8Char = GetMonth8Char(_date, nextSolarNum);
            day8Char = GetDay8Char(_date, twohourNum, out dayHeavenlyEarthNum);
        }
        private static void _GetNum(
            string year8Char,
            string month8Char,
            string day8Char,
            int index,
            string[] strings,
            out int yearNum,
            out int monthNum,
            out int dayNum
        )
        {
            // Debug.Log($"{string.Join(", ", strings)}, year8Char[index]:{year8Char[index]}, month8Char[index]:{month8Char[index]}, day8Char[index]:{day8Char[index]}");
            // yearNum = Array.IndexOf(strings, year8Char[index]);
            // monthNum = Array.IndexOf(strings, month8Char[index]);
            // dayNum = Array.IndexOf(strings, day8Char[index]);
            yearNum = UdonArrayPlus.IndexOf(strings, year8Char[index].ToString());
            monthNum = UdonArrayPlus.IndexOf(strings, month8Char[index].ToString());
            dayNum = UdonArrayPlus.IndexOf(strings, day8Char[index].ToString());
            // Debug.Log($"yearNum:{yearNum}, monthNum:{monthNum}, dayNum:{dayNum}");
        }
        public static void GetEarthNum(
            string year8Char,
            string month8Char,
            string day8Char,
            out int yearEarthNum,
            out int monthEarthNum,
            out int dayEarthNum
        ) => _GetNum(
            year8Char,
            month8Char,
            day8Char,
            1,
            Config.The12EarthlyBranches(),
            out yearEarthNum,
            out monthEarthNum,
            out dayEarthNum
        );
        public static void GetHeavenNum(
            string year8Char,
            string month8Char,
            string day8Char,
            out int yearEarthNum,
            out int monthEarthNum,
            out int dayEarthNum
        ) => _GetNum(
            year8Char,
            month8Char,
            day8Char,
            0,
            Config.The10HeavenlyStems(),
            out yearEarthNum,
            out monthEarthNum,
            out dayEarthNum
        );
        public static void GetSeason(
            int monthEarthNum,
            out int seasonType,
            out int seasonNum,
            out string lunarSeason
        )
        {
            seasonType = monthEarthNum % 3;
            // seasonNum = (monthEarthNum - 2) % 12 / 3;
            var w = (monthEarthNum - 2) % 12;
            if (w < 0) w += 12;
            seasonNum = w / 3;
            lunarSeason = $"{"仲季孟"[seasonType]}{"春夏秋冬"[seasonNum]}";
        }
    }
}