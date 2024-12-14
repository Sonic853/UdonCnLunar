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
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;
using Sonic853.Udon.CnLunar.Extensions;

namespace Sonic853.Udon.CnLunar
{
    public class Lunar : UdonSharpBehaviour
    {
        public Config config;
        public Holidays holidays;
        public DateTime date = DateTime.Now;
        public string godType = "8char";
        public string year8Char = "year";
        public string month8Char = "";
        public string day8Char = "";
        public int twohourNum = -1;
        public bool isLunarLeapMonth = false;
        public int lunarYear = -1;
        public int lunarMonth = -1;
        public int lunarDay = -1;
        public int spanDays = -1;
        public int[] monthDaysList = new int[0];
        public string lunarYearCn = "";
        public string lunarMonthCn = "";
        public string lunarDayCn = "";
        public bool lunarMonthLong = false;
        public string phaseOfMoon = "";
        public string todaySolarTerms = "";
        public int[][] thisYearSolarTermsDateList = new int[0][];
        public int nextSolarNum = -1;
        public string nextSolarTerm = "";
        public int nextSolarTermYear = -1;
        public int nextSolarTermsMonth = -1;
        public int nextSolarTermsDay = -1;
        public int _x = -1;
        public int dayHeavenlyEarthNum = -1;
        public int yearEarthNum = -1;
        public int monthEarthNum = -1;
        public int dayEarthNum = -1;
        public int yearHeavenNum = -1;
        public int monthHeavenNum = -1;
        public int dayHeavenNum = -1;
        public int seasonType = -1;
        public int seasonNum = -1;
        public string lunarSeason = "";
        public string[] twohour8CharList = new string[0];
        public string twohour8Char = "";
        public string today12DayOfficer = "";
        public string today12DayGod = "";
        public string dayName = "";
        public string chineseYearZodiac = "";
        public string chineseZodiacClash = "";
        public string zodiacMark6 = "";
        public string[] zodiacMark3List = new string[0];
        public string zodiacWin = "";
        public string zodiacLose = "";
        public string weekDayCn = "";
        public string starZodiac = "";
        public string todayEastZodiac = "";
        public string today28Star = "";
        public DataDictionary thisYearSolarTermsDic = new DataDictionary();
        public string meridians = "";
        public string[] goodGodNames;
        public string[] badGodNames;
        public string[] goodThings;
        public string[] badThings;
        public int todayLevel;
        public string todayLevelName;
        public string thingLevelName;
        public bool isDe;
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
            twohourNum = this.GetTwohourNum();
            isLunarLeapMonth = false;
            this.GetLunarDateNum(
                out lunarYear,
                out lunarMonth,
                out lunarDay,
                out isLunarLeapMonth,
                out spanDays,
                out monthDaysList
            );
            Debug.Log($"year:{lunarYear}, month:{lunarMonth}, day:{lunarDay}, isLunarLeapMonth:{isLunarLeapMonth}, spanDays:{spanDays}");
            this.GetLunarCn(
                out lunarYearCn,
                out lunarMonthCn,
                out lunarDayCn,
                out lunarMonthLong
            );
            Debug.Log($"lunarYearCn:{lunarYearCn}, lunarMonthCn:{lunarMonthCn}, lunarDayCn:{lunarDayCn}, lunarMonthLong:{lunarMonthLong}");
            phaseOfMoon = this.GetPhaseOfMoon();
            todaySolarTerms = this.GetTodaySolarTerms(
                out thisYearSolarTermsDateList,
                out nextSolarNum,
                out nextSolarTerm,
                out nextSolarTermYear,
                out nextSolarTermsMonth,
                out nextSolarTermsDay
            );
            // 立春干支参数
            _x = this.GetBeginningOfSpringX();

            this.GetThe8char(
                out year8Char,
                out month8Char,
                out day8Char,
                out dayHeavenlyEarthNum
            );
            this.GetEarthNum(
                out yearEarthNum,
                out monthEarthNum,
                out dayEarthNum
            );
            this.GetHeavenNum(
                out yearHeavenNum,
                out monthHeavenNum,
                out dayHeavenNum
            );
            this.GetSeason(
                out seasonType,
                out seasonNum,
                out lunarSeason
            );
            twohour8CharList = this.GetTwohour8CharList();
            twohour8Char = this.GetTwohour8Char();
            dayName = this.GetToday12DayOfficer(
                out today12DayOfficer,
                out today12DayGod
            );

            chineseYearZodiac = this.GetChineseYearZodiac();
            chineseZodiacClash = this.GetChineseZodiacClash(
                out zodiacMark6,
                out zodiacMark3List,
                out zodiacWin,
                out zodiacLose
            );
            weekDayCn = this.GetWeekDayCn();
            starZodiac = this.GetStarZodiac();
            todayEastZodiac = this.GetEastZodiac();
            // self.thisYearSolarTermsDic = dict(zip(SOLAR_TERMS_NAME_LIST, self.thisYearSolarTermsDateList))
            thisYearSolarTermsDic = GetSolarTermsDic(thisYearSolarTermsDateList);
            today28Star = this.GetThe28Stars();

            this.GetAngelDemon(
                out todayLevel,
                out todayLevelName,
                out thingLevelName,
                out isDe,
                out goodGodNames,
                out badGodNames,
                out goodThings,
                out badThings
            );
            meridians = this.GetMeridians();
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
        /// 生肖
        /// </summary>
        /// <param name="lunarYear">农历年</param>
        /// <param name="_x"></param>
        /// <returns></returns>
        public static string GetChineseYearZodiac(int lunarYear, int _x)
        {
            return Config.ChineseZodiacNameList()[(lunarYear - 4) % 12 - _x];
        }
        public static string GetChineseZodiacClash(
            int dayEarthNum,
            out string zodiacMark6,
            out string[] zodiacMark3List,
            out string zodiacWin,
            out string zodiacLose
        )
        {
            var zodiacNum = dayEarthNum;
            var zodiacClashNum = (zodiacNum + 6) % 12;
            var zodiacMark6Index = (25 - zodiacNum) % 12;
            if (zodiacMark6Index < 0) zodiacMark6Index += 12;
            var chineseZodiacNameList = Config.ChineseZodiacNameList();
            zodiacMark6 = chineseZodiacNameList[zodiacMark6Index];
            zodiacMark3List = new string[] { chineseZodiacNameList[(zodiacNum + 4) % 12], chineseZodiacNameList[(zodiacNum + 8) % 12] };
            zodiacWin = chineseZodiacNameList[zodiacNum];
            zodiacLose = chineseZodiacNameList[zodiacClashNum];
            return $"{zodiacWin}日冲{zodiacLose}";
        }
        /// <summary>
        /// 输出当前日期中文星期几
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetWeekDayCn(DateTime date) => Config.WeekDay()[(int)date.DayOfWeek];
        /// <summary>
        /// 农历月数<br />
        /// 计算阴历月天数
        /// </summary>
        /// <param name="lunarYear">农历年</param>
        /// <param name="monthDay">30或29</param>
        /// <param name="leapMonth">该年闰月</param>
        /// <param name="leapDays">闰月天数</param>
        public static void GetMonthLeapMonthLeapDays(int lunarYear, int lunarMonth, out int monthDay, out int leapMonth, out int leapDays, out int[] monthDaysList)
        {
            // 闰几月，该月多少天 传入月份多少天
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
        /// <summary>
        /// 获取数字形式的农历日期<br />
        /// 返回的月份，高4bit为闰月月份，低4bit为其它正常月份
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="lunarYear">农历年</param>
        /// <param name="lunarMonth">农历月</param>
        /// <param name="lunarDay">农历日</param>
        public static void GetLunarDateNum(DateTime date, out int lunarYear, out int lunarMonth, out int lunarDay, out bool isLunarLeapMonth, out int spanDays, out int[] monthDaysList)
        {
            lunarYear = date.Year;
            lunarMonth = 1;
            lunarDay = 1;
            var codeYear = Config.LunarNewYearList()[lunarYear - Config.START_YEAR()];
            // 获取当前日期与当年春节的差日
            var _spanDays = (date - new DateTime(lunarYear, (codeYear >> 5) & 0x3, (codeYear >> 0) & 0x1f)).Days;
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
        public static int[][] GetSolarTermsDateList(int year)
        {
            var solarTermsList = Solar24.GetTheYearAllSolarTermsList(year);
            var solarTermsListLength = solarTermsList.Length;
            var solarTermsDateList = new int[solarTermsListLength][];
            for (var i = 0; i < solarTermsListLength; i++)
            {
                var day = solarTermsList[i];
                var month = i / 2 + 1;
                solarTermsDateList[i] = new int[] { month, (int)day };
            }
            return solarTermsDateList;
        }
        public static int GetNextNum(int[] findDate, int[][] solarTermsDateList)
        {
            var _findDate = findDate[0] * 100 + findDate[1];
            var nextSolarNum = 0;
            for (var i = 0; i < solarTermsDateList.Length; i++)
            {
                var date = solarTermsDateList[i][0] * 100 + solarTermsDateList[i][1];
                if (date <= _findDate)
                    nextSolarNum++;
            }
            return nextSolarNum % 24;
        }
        public static int GetIndexInYearSolarTerms(int[] findDate, int[][] solarTermsDateList)
        {
            var findMonth = findDate[0];
            var findDay = findDate[1];
            for (var i = 0; i < solarTermsDateList.Length; i++)
            {
                if (solarTermsDateList[i][0] == findMonth && solarTermsDateList[i][1] == findDay)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 获取今天的节气
        /// </summary>
        /// <param name="date"></param>
        /// <param name="thisYearSolarTermsDateList"></param>
        /// <param name="nextSolarNum"></param>
        /// <param name="nextSolarTerm"></param>
        /// <param name="nextSolarTermYear"></param>
        /// <param name="nextSolarTermsMonth"></param>
        /// <param name="nextSolarTermsDay"></param>
        /// <returns>节气</returns>
        public static string GetTodaySolarTerms(
            DateTime date,
            out int[][] thisYearSolarTermsDateList,
            out int nextSolarNum,
            out string nextSolarTerm,
            out int nextSolarTermYear,
            out int nextSolarTermsMonth,
            out int nextSolarTermsDay
        )
        {
            var year = date.Year;
            var solarTermsDateList = GetSolarTermsDateList(year);
            thisYearSolarTermsDateList = solarTermsDateList;
            var findDate = new int[] { date.Month, date.Day };
            nextSolarNum = GetNextNum(findDate, solarTermsDateList);
            var index = GetIndexInYearSolarTerms(findDate, solarTermsDateList);
            var SOLAR_TERMS_NAME_LIST = Config.SOLAR_TERMS_NAME_LIST();
            var todaySolarTerm = index != -1 ? SOLAR_TERMS_NAME_LIST[index] : "无";
            // 次年节气
            if (findDate[0] == solarTermsDateList[solarTermsDateList.Length - 1][0] && findDate[1] >= solarTermsDateList[solarTermsDateList.Length - 1][1])
            {
                year++;
                solarTermsDateList = GetSolarTermsDateList(year);
            }
            nextSolarTerm = SOLAR_TERMS_NAME_LIST[nextSolarNum];
            nextSolarTermsMonth = solarTermsDateList[nextSolarNum][0];
            nextSolarTermsDay = solarTermsDateList[nextSolarNum][1];
            nextSolarTermYear = year;
            return todaySolarTerm;
        }
        public static DataDictionary GetSolarTermsDic(int[][] thisYearSolarTermsDateList)
        {
            var SOLAR_TERMS_NAME_LIST = Config.SOLAR_TERMS_NAME_LIST();
            var thisYearSolarTermsDic = new DataDictionary();
            for (var i = 0; i < SOLAR_TERMS_NAME_LIST.Length; i++)
            {
                var data = new DataList();
                data.Add(thisYearSolarTermsDateList[i][0]);
                data.Add(thisYearSolarTermsDateList[i][1]);
                thisYearSolarTermsDic.Add(SOLAR_TERMS_NAME_LIST[i], data);
            }
            return thisYearSolarTermsDic;
        }
        public static string GetEastZodiac(string nextSolarTerm)
        {
            var NextSolarTermIndex = (Array.IndexOf(Config.SOLAR_TERMS_NAME_LIST(), nextSolarTerm) - 1) % 24;
            if (NextSolarTermIndex < 0) NextSolarTermIndex += 24;
            NextSolarTermIndex /= 2;
            return Config.EAST_ZODIAC_LIST()[NextSolarTermIndex];
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
        public static string GetMonth8Char(DateTime date, int nextSolarNum)
        {
            var nextNum = nextSolarNum;
            // 2019年正月为丙寅月
            if (nextNum == 0 && date.Month == 12)
            {
                nextNum = 24;
            }
            var apartNum = (nextNum + 1) / 2;
            // (year-2019)*12+apartNum每年固定差12个月回到第N年月柱，2019小寒甲子，加上当前过了几个节气除以2+(nextNum-1)//2，减去1
            return Config.The60HeavenlyEarth()[((date.Year - 2019) * 12 + apartNum) % 60];
        }
        public static string GetDay8Char(DateTime date, int twohourNum, out int dayHeavenlyEarthNum)
        {
            var date2019 = DateTime.Parse("2019-01-29T00:00:00.000+08:00");
            var apart = date - date2019;
            var the60HeavenlyEarth = Config.The60HeavenlyEarth();
            var baseNum = Array.IndexOf(the60HeavenlyEarth, "丙寅");
            if (twohourNum == 12)
            {
                baseNum++;
            }
            dayHeavenlyEarthNum = (apart.Days + baseNum) % 60;
            return the60HeavenlyEarth[dayHeavenlyEarthNum];
        }
        public static string[] GetTwohour8CharList(string day8Char)
        {
            var the60HeavenlyEarth = Config.The60HeavenlyEarth();
            var the60HeavenlyEarthLength = the60HeavenlyEarth.Length;
            var twohour8CharList = new string[13];
            var begin = Array.IndexOf(the60HeavenlyEarth, day8Char) * 12 % the60HeavenlyEarthLength;
            var begin13 = begin + 13;
            var index = 0;
            for (var i = begin; i < begin13; i++)
            {
                twohour8CharList[index] = $"{the60HeavenlyEarth[i % the60HeavenlyEarthLength]}";
                index++;
            }
            return twohour8CharList;
        }
        public static string GetTwohour8Char(string[] twohour8CharList, int twohourNum) => twohour8CharList[twohourNum % 12];
        public static void GetThe8char(
            DateTime date,
            int lunarYear,
            int _x,
            int nextSolarNum,
            int twohourNum,
            out string year8Char,
            out string month8Char,
            out string day8Char,
            out int dayHeavenlyEarthNum
        )
        {
            year8Char = GetYear8Char(lunarYear, _x);
            month8Char = GetMonth8Char(date, nextSolarNum);
            day8Char = GetDay8Char(date, twohourNum, out dayHeavenlyEarthNum);
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
            yearNum = Array.IndexOf(strings, year8Char[index].ToString());
            monthNum = Array.IndexOf(strings, month8Char[index].ToString());
            dayNum = Array.IndexOf(strings, day8Char[index].ToString());
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
        /// <summary>
        /// 季节
        /// </summary>
        /// <param name="monthEarthNum"></param>
        /// <param name="seasonType"></param>
        /// <param name="seasonNum"></param>
        /// <param name="lunarSeason"></param>
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
        /// <summary>
        /// 星座
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetStarZodiac(DateTime date)
        {
            // return STAR_ZODIAC_NAME[len(list(filter(lambda y: y <= (self.date.month, self.date.day), STAR_ZODIAC_DATE))) % 12]
            var findDateAsInt = date.Month * 100 + date.Day;
            var STAR_ZODIAC_NAME = Config.STAR_ZODIAC_NAME();
            var STAR_ZODIAC_DATE = Config.STAR_ZODIAC_DATE();
            var filterLength = 0;
            for (var i = 0; i < STAR_ZODIAC_DATE.Length; i++)
            {
                var month = STAR_ZODIAC_DATE[i][0];
                var day = STAR_ZODIAC_DATE[i][1];
                if (month * 100 + day <= findDateAsInt)
                {
                    filterLength++;
                }
            }
            return STAR_ZODIAC_NAME[filterLength % 12];
        }
        public static string[] GetLegalHolidays(
            Holidays holidays,
            DateTime date,
            int lunarMonth,
            int lunarDay,
            string todaySolarTerms
        )
        {
            var temp = new DataList();
            var legalsolarTermsHolidayDic = holidays.LegalsolarTermsHolidayDic;
            if (legalsolarTermsHolidayDic.TryGetValue(todaySolarTerms, out var value))
            {
                // temp += "清明节";
                temp.Add(value.String);
            }
            var legalHolidaysDic = holidays.LegalHolidaysDic;
            if (legalHolidaysDic.TryGetValue($"{date.Month},{date.Day}", out value))
            {
                temp.Add(value.String);
            }
            var legalLunarHolidaysDic = holidays.LegalLunarHolidaysDic;
            if (
                lunarMonth <= 12
                && legalLunarHolidaysDic.TryGetValue($"{lunarMonth},{lunarDay}", out value)
            )
            {
                temp.Add(value.String);
            }
            var tempCount = temp.Count;
            var _temp = new string[tempCount];
            for (var i = 0; i < tempCount; i++)
            {
                _temp[i] = temp[i].String;
            }
            return _temp;
        }
        /// <summary>
        /// 建除十二神，《淮南子》曰：正月建寅，则寅为建，卯为除，辰为满，巳为平，主生；午为定，未为执，主陷；申为破，主衡；酉为危，主杓；戍为成，主小德；亥为收，主大备；子为开，主太阳；丑为闭，主太阴。
        /// </summary>
        public static string GetToday12DayOfficer(
            string godType,
            int lunarMonth,
            int monthEarthNum,
            int dayEarthNum,
            out string today12DayOfficer,
            out string today12DayGod
        )
        {
            // chinese12DayGods=["青龙","明堂","天刑","朱雀","金贵","天德","白虎","玉堂","天牢","玄武","司命","勾陈"]
            int men;
            if (godType == "cnlunar")
            {
                // 使用农历月份与八字日柱算神煞（辨方书文字） 农历(1-12)，-1改编号，[0-11]，+2位移，% 12 防止溢出
                men = (lunarMonth - 1 + 2) % 12;
            }
            else
            {
                // 使用八字月柱与八字日柱算神煞（辨方书配图和部分文字）
                men = monthEarthNum;
            }
            var thisMonthStartGodNum = men % 12;
            var apartNum = dayEarthNum - thisMonthStartGodNum;
            today12DayOfficer = Config.Chinese12DayOfficers()[apartNum % 12].ToString();
            // 青龙定位口诀：子午寻申位，丑未戌上亲；寅申居子中，卯酉起于寅；辰戌龙位上，巳亥午中寻。
            // [申戌子寅辰午]
            // 十二神凶吉口诀：道远几时通达，路遥何日还乡
            // 辶为吉神(0, 1, 4, 5, 7, 10)
            // 为黄道吉日
            var eclipticGodNum = (dayEarthNum - Config.EclipticGodNums()[men]) % 12;
            if (eclipticGodNum < 0) eclipticGodNum += 12;
            today12DayGod = Config.Chinese12DayGods()[eclipticGodNum % 12];
            string dayName;
            if (Array.IndexOf(Config.DayNames(), eclipticGodNum) != -1)
            {
                dayName = "黄道日";
            }
            else
            {
                dayName = "黑道日";
            }
            return dayName;
        }
        /// <summary>
        /// 八字与五行
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetThe28Stars(DateTime date)
        {
            var date2019 = DateTime.Parse("2019-01-17T00:00:00.000+08:00");
            var apart = date - date2019;
            return Config.The28StarsList()[apart.Days % 28];
        }
        public static int GetTwohourNum(DateTime date) => (date.Hour + 1) / 2;
        public static string GetMeridians(int twohourNum) => Config.MeridiansName()[twohourNum % 12];
        /// <summary>
        /// 宜忌等第表 计算凶吉
        /// </summary>
        public static int GetTodayThingLevel(
            Config config,
            string month8Char,
            string[] goodGodNames,
            string[] badGodNames,
            string today12DayOfficer,
            out int todayLevel,
            out string todayLevelName,
            out string thingLevelName,
            out bool isDe
        )
        {
            var badGodDic = config.BadGodDic;
            var todayAllGodNames = new string[goodGodNames.Length + badGodNames.Length + 1];
            Array.Copy(goodGodNames, todayAllGodNames, goodGodNames.Length);
            Array.Copy(badGodNames, 0, todayAllGodNames, goodGodNames.Length, badGodNames.Length);
            todayAllGodNames[goodGodNames.Length + badGodNames.Length] = $"{today12DayOfficer}日";
            var l = -1;
            foreach (var gnoItem in todayAllGodNames)
            {
                if (
                    badGodDic.TryGetValue(gnoItem, out var value)
                    && value.TokenType == TokenType.DataList
                )
                {
                    var valueList = value.DataList;
                    var valueListCount = valueList.Count;
                    for (var i = 0; i < valueListCount; i++)
                    {
                        var item = valueList[i].DataList;
                        if (
                            item[0].String.Contains(month8Char[1])
                            && item[1].TokenType == TokenType.DataList
                        )
                        {
                            var godnames = item[1].DataList;
                            var itemInt = item[2].Int;
                            for (var j = 0; j < godnames.Count; j++)
                            {
                                var godname = godnames[j].String;
                                if (
                                    Array.IndexOf(todayAllGodNames, godname) != -1
                                    && itemInt > l
                                )
                                {
                                    l = itemInt;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            var levelList = config.LevelList;
            todayLevel = l;
            todayLevelName = l == -1 ? "无" : levelList[l].String;
            var thingLevelList = config.ThingLevelList;
            isDe = false;
            var thingLevelStrings = Config.ThingLevelStrings();
            foreach (var goodGodName in goodGodNames)
            {
                // if (thingLevelStrings.Contains(goodGodName))
                if (Array.IndexOf(thingLevelStrings, goodGodName) != -1)
                {
                    isDe = true;
                    break;
                }
            }
            var thingLevel = -1;
            switch (l)
            {
                // 下下：凶叠大凶，遇德亦诸事皆忌；卯酉月 灾煞遇 月破、月厌  月厌遇灾煞、月破；
                case 5:
                    thingLevel = 3;
                    break;
                // 下：凶又逢凶，遇德从忌不从宜，不遇诸事皆忌；
                case 4:
                    thingLevel = isDe ? 2 : 3;
                    break;
                // 中次：凶胜于吉，遇德从宜亦从忌，不遇从忌不从宜；
                case 3:
                    thingLevel = isDe ? 1 : 2;
                    break;
                // 中：吉不抵凶，遇德从宜不从忌，不遇从忌不从宜；
                case 2:
                    thingLevel = isDe ? 0 : 2;
                    break;
                // 上次：吉足抵凶，遇德从宜不从忌，不遇从宜亦从忌；
                case 1:
                    thingLevel = isDe ? 0 : 1;
                    break;
                // 上：吉足胜凶，从宜不从忌；
                case 0:
                    thingLevel = 0;
                    break;
                // 无，例外 从宜不从忌
                default:
                    thingLevel = 1;
                    break;
            }
            thingLevelName = thingLevelList[thingLevel].String;
            return thingLevel;
        }
        /// <summary>
        /// 神煞宜忌
        /// </summary>
        public static void GetAngelDemon(
            Config config,
            DateTime date,
            DataDictionary thisYearSolarTermsDic,
            string today12DayOfficer,
            string today28Star,
            string day8Char,
            string godType,
            string phaseOfMoon,
            string month8Char,
            int dayEarthNum,
            int dayHeavenlyEarthNum,
            int seasonNum,
            int yearHeavenNum,
            int yearEarthNum,
            int monthEarthNum,
            int lunarDay,
            int lunarMonth,
            int nextSolarTermYear,
            int nextSolarNum,
            out int todayLevel,
            out string todayLevelName,
            out string thingLevelName,
            out bool isDe,
            out string[] goodGodNames,
            out string[] badGodNames,
            out string[] goodThings,
            out string[] badThings
        )
        {
            // the10HeavenlyStems =["甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸"]
            // the12EarthlyBranches = ["子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥"]
            // 相冲+6
            // 四绝日：一切用事皆忌之，立春，立夏，立秋，立冬前一日。忌出行、赴任、嫁娶、进人、迁移、开市、立券、祭祀
            // 四离日：春分、秋分、夏至、冬至前一天。日值四离，大事勿用
            // 杨公13忌日：忌开张、动工、嫁娶、签订合同
            // 红纱日、正红纱日：四孟金鸡（酉）四仲蛇（巳），四季丑日是红纱，惟有孟仲合吉用 ，季月逢之俱不佳（正红纱日）
            // 凤凰日、麒麟日：凤凰压朱雀，麒麟制白虎，克制朱雀白虎中效果。春井，夏尾，秋牛，冬壁，是麒麟日，春危，夏昴，秋胃，冬毕是凤凰日
            // 月德、月德合:申子辰三合，阳水为壬，月德合丁；巳酉丑三合，阳金为庚，月德合乙；寅午戌三合，阳火为丙，月德合辛；亥卯未三合，阳木为甲，月德合己
            // 天德、天德合:《子平赋》说：印绶得同天德，官刑不至，至老无灾.
            // 岁德、岁德合:《协纪辨方书·义例一·岁德》：曾门经曰：岁德者，岁中德神也。https://www.jianshu.com/p/ec0432f31060
            // 月恩:正月逢丙是月恩，二月见丁三庚真，四月己上五月戊，六辛七壬八癸成，九月庚上十月乙，冬月甲上腊月辛。
            // 天恩:四季何时是天恩，甲子乙丑丙寅建。丁卯戊辰兼己卯，庚辰辛巳壬午言，癸未隔求己酉日，庚戌辛亥亦同联，壬子癸丑无差误，此是天恩吉日传
            var officerThings = config.OfficerThings;
            var goodThing = new DataList();
            var badThing = new DataList();
            if (
                officerThings.TryGetValue(today12DayOfficer, out var dataToken)
                && dataToken.TokenType == TokenType.DataList
            )
            {
                var dataList = dataToken.DataList;
                if (dataList.Count > 0)
                {
                    goodThing = dataList[0].DataList;
                }
                if (dataList.Count > 1)
                {
                    badThing = dataList[1].DataList;
                }
            }
            var gbDic = Config.GbDic(goodThing, badThing);
            var mrY13 = Config.MrY13();
            var tomorrow = date + TimeSpan.FromDays(1);
            var tmd = new int[] { tomorrow.Month, tomorrow.Day };
            var tomorrowAsInt = tmd[0] * 100 + tmd[1];
            var t4larr = new string[] { "春分", "夏至", "秋分", "冬至" };
            var t4jarr = new string[] { "立春", "立夏", "立秋", "立冬" };
            var t4l = new DataList();
            // var t4lInts = new DataList();
            DataToken value;
            foreach (var i in t4larr)
            {
                if (
                    thisYearSolarTermsDic.TryGetValue(i, out value)
                    && value.TokenType == TokenType.DataList
                )
                {
                    var valueDataList = value.DataList;
                    // t4lInts.Add(valueDataList[0].Int * 100 + valueDataList[1].Int);
                    t4l.Add(valueDataList);
                }
            }
            var t4j = new DataList();
            var t4jInts = new DataList();
            var filteredCount = 0;
            foreach (var i in t4jarr)
            {
                if (
                    thisYearSolarTermsDic.TryGetValue(i, out value)
                    && value.TokenType == TokenType.DataList
                )
                {
                    var valueDataList = value.DataList;
                    var t4jInt = valueDataList[0].Int * 100 + valueDataList[1].Int;
                    t4jInts.Add(t4jInt);
                    if (t4jInt < tomorrowAsInt)
                    {
                        filteredCount++;
                    }
                    t4j.Add(valueDataList);
                }
            }
            var twys = new int[2];
            if (t4j.TryGetValue(filteredCount % 4, out value))
            {
                twys[0] = value.DataList[0].Int;
                twys[1] = value.DataList[1].Int;
            }
            var s = today28Star;
            var o = today12DayOfficer;
            var d = day8Char;
            var den = dayEarthNum;
            var dhen = dayHeavenlyEarthNum;
            var sn = seasonNum; // 季节
            var yhn = yearHeavenNum;
            var yen = yearEarthNum;
            var ldn = lunarDay;
            var lmn = lunarMonth;
            int men;
            if (godType == "cnlunar")
            {
                // 使用农历月份与八字日柱算神煞（辨方书文字） 农历(1-12)，-1改编号，[0-11]，+2位移，% 12 防止溢出
                men = (lmn - 1 + 2) % 12;
            }
            else
            {
                // 使用八字月柱与八字日柱算神煞（辨方书配图和部分文字）
                men = monthEarthNum;
            }
            var day8CharThings = config.Day8CharThing;
            // item参数规则，（name,当日判断结果,判断规则,宜事,忌事）
            var day8CharThingKeys = day8CharThings.GetKeys();
            for (int i = 0; i < day8CharThingKeys.Count; i++)
            {
                var key = day8CharThingKeys[i].String;
                if (
                   d.Contains(key)
                   && day8CharThings.TryGetValue(key, out value)
                   && value.TokenType == TokenType.DataList
                )
                {
                    var valueList = value.DataList;
                    gbDic["goodThing"].DataList.AddRange(valueList[0].DataList);
                    gbDic["badThing"].DataList.AddRange(valueList[1].DataList);
                }
            }
            // 雨水后立夏前执日、危日、收日 宜 取鱼
            // if self.nextSolarNum in range(4, 9) and o in ['执', '危', '收']:
            var thingFishStrings = Config.ThingFishStrings();
            // if (nextSolarNum >= 4 && nextSolarNum < 9 && thingFishStrings.Contains(o))
            if (nextSolarNum >= 4 && nextSolarNum < 9 && Array.IndexOf(thingFishStrings, o) != -1)
            {
                if (!gbDic["goodThing"].DataList.Contains("取鱼"))
                    gbDic["goodThing"].DataList.Add("取鱼");
            }
            // 霜降后立春前执日、危日、收日 宜 畋猎
            // if (self.nextSolarNum in range(20, 24) or self.nextSolarNum in range(0, 3)) and o in ['执', '危', '收']:
            // if (((nextSolarNum >= 20 && nextSolarNum < 24) || (nextSolarNum >= 0 && nextSolarNum < 3)) && thingFishStrings.Contains(o))
            if (((nextSolarNum >= 20 && nextSolarNum < 24) || (nextSolarNum >= 0 && nextSolarNum < 3)) && Array.IndexOf(thingFishStrings, o) != -1)
            {
                if (!gbDic["goodThing"].DataList.Contains("畋猎"))
                    gbDic["goodThing"].DataList.Add("畋猎");
            }
            // 立冬后立春前危日 午日 申日 宜 伐木
            // if (self.nextSolarNum in range(21, 24) or self.nextSolarNum in range(0, 3)) and (o in ['危'] or d in ['午', '申']):
            if (((nextSolarNum >= 21 && nextSolarNum < 24) || (nextSolarNum >= 0 && nextSolarNum < 3)) && (o == "危" || d == "午" || d == "申"))
            {
                if (!gbDic["goodThing"].DataList.Contains("伐木"))
                    gbDic["goodThing"].DataList.Add("伐木");
            }
            // 每月一日 六日 十五 十九日 二十一日 二十三日 忌 整手足甲
            var days = new int[] { 1, 6, 15, 19, 21, 23 };
            // if (days.Contains(ldn))
            if (Array.IndexOf(days, ldn) != -1)
            {
                if (!gbDic["badThing"].DataList.Contains("整手足甲"))
                    gbDic["badThing"].DataList.Add("整手足甲");
            }
            // 每月十二日 十五日 忌 整容剃头
            if (ldn == 12 || ldn == 15)
            {
                var dataList = gbDic["badThing"].DataList;
                if (!dataList.Contains("整容"))
                    gbDic["badThing"].DataList.Add("整容");
                if (!dataList.Contains("剃头"))
                    gbDic["badThing"].DataList.Add("剃头");
            }
            // 每月十五日 朔弦望月 忌 求医疗病
            if (ldn == 15 || !string.IsNullOrEmpty(phaseOfMoon))
            {
                if (!gbDic["badThing"].DataList.Contains("求医疗病"))
                    gbDic["badThing"].DataList.Add("求医疗病");
            }
            // 由于正月建寅，men参数使用排序是从子开始，所以对照书籍需要将循环八字列向右移两位，也就是映射正月的是在第三个字
            var angel = config.Angel(yhn, men, sn, den, dhen, d, s);
            var demon = config.Demon(den, yen, men, ldn, lmn, sn, nextSolarTermYear, tmd, twys, d, s, date, t4j, t4l);

            GetTodayGoodBadThing(ref gbDic, angel, "goodName");
            GetTodayGoodBadThing(ref gbDic, demon, "badName");

            var goodNameDataList = gbDic["goodName"].DataList;
            goodGodNames = new string[goodNameDataList.Count];
            for (var i = 0; i < goodNameDataList.Count; i++)
            {
                goodGodNames[i] = goodNameDataList[i].String;
            }
            var badNameDataList = gbDic["badName"].DataList;
            badGodNames = new string[badNameDataList.Count];
            for (var i = 0; i < badNameDataList.Count; i++)
            {
                badGodNames[i] = badNameDataList[i].String;
            }

            // 今日凶吉判断
            var thingLevel = GetTodayThingLevel(
                config,
                month8Char,
                goodGodNames,
                badGodNames,
                today12DayOfficer,
                out todayLevel,
                out todayLevelName,
                out thingLevelName,
                out isDe
            );

            // 0:'从宜不从忌',1:'从宜亦从忌',2:'从忌不从宜',3:'诸事皆忌'
            switch (thingLevel)
            {
                case 0:
                    // 从宜不从忌
                    GoodOppressBad(ref gbDic);
                    break;
                case 1:
                    // 从宜亦从忌
                    BadDrewGood(ref gbDic);
                    break;
                case 2:
                    // 从忌不从宜
                    BadOppressGood(ref gbDic);
                    break;
                case 3:
                    // 诸事皆忌
                    NothingGood(ref gbDic);
                    break;
            }

            var goodThingDataList = gbDic["goodThing"].DataList.DeepClone();
            var badThingDataList = gbDic["badThing"].DataList.DeepClone();

            // 遇德犹忌之事
            var deIsBadThingDic = new DataDictionary();
            // for i in angel[:6]:
            var angelKeys = angel.GetKeys();
            for (var i = 0; i < 6; i++)
            {
                var key = angelKeys[i].String;
                var angelItem = angel[key].DataList;
                var angelItemFirst = key;
                if (!deIsBadThingDic.ContainsKey(angelItemFirst))
                {
                    deIsBadThingDic.Add(angelItemFirst, angelItem[3]);
                }
            }
            var deIsBadThingDataList = new DataList();
            if (isDe)
            {
                foreach (var goodGodName in goodGodNames)
                {
                    if (deIsBadThingDic.ContainsKey(goodGodName))
                    {
                        var goodGodNameDataList = deIsBadThingDic[goodGodName].DataList;
                        var goodGodNameDataListCount = goodGodNameDataList.Count;
                        for (var i = 0; i < goodGodNameDataListCount; i++)
                        {
                            var item = goodGodNameDataList[i].String;
                            if (!deIsBadThingDataList.Contains(item))
                                deIsBadThingDataList.Add(item);
                        }
                    }
                }
            }
            var deIsBadThing = new string[deIsBadThingDataList.Count];
            for (var i = 0; i < deIsBadThingDataList.Count; i++)
            {
                deIsBadThing[i] = deIsBadThingDataList[i].String;
            }

            if (thingLevel != 3)
            {
                // 凡宜宣政事，布政事之日，只注宜宣政事。
                if (
                    goodThingDataList.Contains("宣政事")
                    && goodThingDataList.Contains("布政事")
                )
                {
                    goodThingDataList.RemoveAll("布政事");
                }
                // 凡宜营建宫室、修宫室之日，只注宜营建宫室。
                if (
                    goodThingDataList.Contains("营建宫室")
                    && goodThingDataList.Contains("修宫室")
                )
                {
                    goodThingDataList.RemoveAll("修宫室");
                }
                // 凡德合、赦愿、月恩、四相、时德等日，不注忌进人口、安床、经络、酝酿、开市、立券、交易、纳财、开仓库、出货财。如遇德犹忌，及从忌不从宜之日，则仍注忌。
                var isDeSheEnSixiang = false;
                var deStrings = new string[] { "岁德合", "月德合", "天德合", "天赦", "天愿", "月恩", "四相", "时德" };
                foreach (var goodGodName in goodGodNames)
                {
                    // if (deStrings.Contains(goodGodName))
                    if (Array.IndexOf(deStrings, goodGodName) != -1)
                    {
                        isDeSheEnSixiang = true;
                        break;
                    }
                }
                // 以上判断 凡德合、赦愿、月恩、四相、时德等日，isDeSheEnSixiang = True
                if (isDeSheEnSixiang && thingLevel != 2)
                {
                    // 不注忌 但条件是 非 从忌不从宜之日，所以 thingLevel != 2
                    var removeStrings = new string[] { "进人口", "安床", "经络", "酝酿", "开市", "立券交易", "纳财", "开仓库", "出货财" };
                    foreach (var removeString in removeStrings)
                        badThingDataList.RemoveAll(removeString);
                    foreach (var item in deIsBadThing)
                    {
                        if (!badThingDataList.Contains(item))
                            badThingDataList.Add(item);
                    }
                }

                // 凡天狗寅日忌祭祀，不注宜求福、祈嗣。
                if (
                    // badGodNames.Contains("天狗")
                    Array.IndexOf(badGodNames, "天狗") != -1
                    || d.Contains("寅")
                )
                {
                    if (!badThingDataList.Contains("祭祀"))
                        badThingDataList.Add("祭祀");
                    var removeStrings = new string[] { "祭祀", "求福", "祈嗣" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                }
                // 凡卯日忌穿井，不注宜开渠。壬日忌开渠，不注宜穿井。
                if (d.Contains("卯"))
                {
                    if (!badThingDataList.Contains("穿井"))
                        badThingDataList.Add("穿井");
                    var removeStrings = new string[] { "穿井", "开渠" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                }
                if (d.Contains("壬"))
                {
                    if (!badThingDataList.Contains("开渠"))
                        badThingDataList.Add("开渠");
                    var removeStrings = new string[] { "开渠", "穿井" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                }
                // 凡巳日忌出行，不注宜出师、遣使。
                if (d.Contains("巳"))
                {
                    if (!badThingDataList.Contains("出行"))
                        badThingDataList.Add("出行");
                    var removeStrings = new string[] { "出行", "出师", "遣使" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                }
                // 凡酉日忌宴会，亦不注宜庆赐、赏贺。
                if (d.Contains("酉"))
                {
                    if (!badThingDataList.Contains("宴会"))
                        badThingDataList.Add("宴会");
                    var removeStrings = new string[] { "宴会", "庆赐", "赏贺" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                }
                // 凡丁日忌剃头，亦不注宜整容。
                if (d.Contains("丁"))
                {
                    if (!badThingDataList.Contains("剃头"))
                        badThingDataList.Add("剃头");
                    var removeStrings = new string[] { "剃头", "整容" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                }
                // 凡吉足胜凶，从宜不从忌者，如遇德犹忌之事，则仍注忌。
                if (todayLevel == 1 || (todayLevel == 0 && thingLevel == 0))
                {
                    foreach (var item in deIsBadThing)
                    {
                        if (!badThingDataList.Contains(item))
                            badThingDataList.Add(item);
                    }
                }
                // 凡吉凶相抵，不注宜亦不注忌者，如遇德犹忌之事，则仍注忌。
                if (todayLevel == 1)
                {
                    // 凡吉凶相抵，不注忌祈福，亦不注忌求嗣。
                    if (!badThingDataList.Contains("祈福"))
                        badThingDataList.RemoveAll("求嗣");
                    // 凡吉凶相抵，不注忌结婚姻，亦不注忌冠带、纳采问名、嫁娶、进人口，如遇德犹忌之日则仍注忌。
                    if (
                        !badThingDataList.Contains("结婚姻")
                        && !isDe
                    )
                    {
                        var removeStrings = new string[] { "冠带", "纳采问名", "嫁娶", "进人口" };
                        foreach (var removeString in removeStrings)
                            badThingDataList.RemoveAll(removeString);
                    }
                    // 凡吉凶相抵，不注忌嫁娶，亦不注忌冠带、结婚姻、纳采问名、进人口、搬移、安床，如遇德犹忌之日，则仍注忌。
                    if (
                        !badThingDataList.Contains("嫁娶")
                        && !isDe
                    )
                    {
                        // 遇不将而不注忌嫁娶者，亦仍注忌。
                        // if (!goodGodNames.Contains("不将"))
                        if (Array.IndexOf(goodGodNames, "不将") == -1)
                        {
                            var removeStrings = new string[] { "冠带", "纳采问名", "结婚姻", "进人口", "搬移", "安床" };
                            foreach (var removeString in removeStrings)
                                badThingDataList.RemoveAll(removeString);
                        }
                    }
                }
                // 遇亥日，厌对、八专、四忌、四穷而仍注忌嫁娶者，只注所忌之事，其不忌者仍不注忌。
                if (d.Contains("亥"))
                {
                    var bads = new string[] { "厌对", "八专", "四忌", "四穷" };
                    foreach (var bad in bads)
                    {
                        var godItem = demon[bad].DataList;
                        if (
                            IfGodItem(godItem[0], godItem[1])
                            && !badThingDataList.Contains("嫁娶")
                        )
                        {
                            badThingDataList.Add("嫁娶");
                        }
                    }
                }
                // 凡吉凶相抵，不注忌搬移，亦不注忌安床。不注忌安床，亦不注忌搬移。如遇德犹忌之日，则仍注忌。
                if (todayLevel == 1 && !isDe)
                {
                    if (!badThingDataList.Contains("搬移"))
                        badThingDataList.RemoveAll("安床");
                    if (!badThingDataList.Contains("安床"))
                        badThingDataList.RemoveAll("搬移");
                    // 凡吉凶相抵，不注忌解除，亦不注忌整容、剃头、整手足甲。如遇德犹忌之日，则仍注忌。
                    if (!badThingDataList.Contains("解除"))
                    {
                        var removeStrings = new string[] { "整容", "剃头", "整手足甲" };
                        foreach (var removeString in removeStrings)
                            badThingDataList.RemoveAll(removeString);
                    }
                    // 凡吉凶相抵，不注忌修造动土、竖柱上梁，亦不注忌修宫室、缮城郭、筑提防、修仓库、鼓铸、苫盖、修置产室、开渠穿井、安碓硙、补垣塞穴、修饰垣墙、平治道涂、破屋坏垣。如遇德犹忌之日，则仍注忌。
                    if (
                        !badThingDataList.Contains("修造")
                        || !badThingDataList.Contains("竖柱上梁")
                    )
                    {
                        var removeStrings = new string[] {
                            "修宫室", "缮城郭", "整手足甲", "筑提", "修仓库", "鼓铸", "苫盖", "修置产室", "开渠穿井",
                            "安碓硙", "补垣塞穴", "修饰垣墙", "平治道涂", "破屋坏垣"
                        };
                        foreach (var removeString in removeStrings)
                            badThingDataList.RemoveAll(removeString);
                    }
                }
                // 凡吉凶相抵，不注忌开市，亦不注忌立券交易、纳财。不注忌纳财，亦不注忌开市、立券交易。不注忌立券交易，亦不注忌开市、纳财。
                // 凡吉凶相抵，不注忌开市、立券交易，亦不注忌开仓库、出货财。
                if (todayLevel == 1)
                {
                    if (!badThingDataList.Contains("开市"))
                    {
                        var removeStrings = new string[] { "立券交易", "纳财", "开仓库", "出货财" };
                        foreach (var removeString in removeStrings)
                            badThingDataList.RemoveAll(removeString);
                    }
                    if (!badThingDataList.Contains("纳财"))
                    {
                        var removeStrings = new string[] { "立券交易", "开市" };
                        foreach (var removeString in removeStrings)
                            badThingDataList.RemoveAll(removeString);
                    }
                    if (!badThingDataList.Contains("立券交易"))
                    {
                        var removeStrings = new string[] { "纳财", "开市", "开仓库", "出货财" };
                        foreach (var removeString in removeStrings)
                            badThingDataList.RemoveAll(removeString);
                    }
                }
                // 如遇专忌之日，则仍注忌。【需要询问原作者以取得进一步解释并对此做出优化】
                // 凡吉凶相抵，不注忌牧养，亦不注忌纳畜。不注忌纳畜，亦不注忌牧养。
                if (todayLevel == 1)
                {
                    if (!badThingDataList.Contains("牧养"))
                        badThingDataList.RemoveAll("纳畜");
                    if (!badThingDataList.Contains("纳畜"))
                        badThingDataList.RemoveAll("牧养");
                    // 凡吉凶相抵，有宜安葬不注忌启攒，有宜启攒不注忌安葬。
                    if (!goodThingDataList.Contains("安葬"))
                        badThingDataList.RemoveAll("启攒");
                    if (!goodThingDataList.Contains("启攒"))
                        badThingDataList.RemoveAll("安葬");
                }
                // 凡忌诏命公卿、招贤，不注宜施恩、封拜、举正直、袭爵受封。    #本版本无 封拜 袭爵受封
                if (badThingDataList.Contains("诏命公卿") || badThingDataList.Contains("招贤"))
                {
                    var removeStrings = new string[] { "施恩", "举正直" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                }
                // 凡忌施恩、封拜、举正直、袭爵受封，亦不注宜诏命公卿、招贤。
                if (badThingDataList.Contains("施恩") || badThingDataList.Contains("举正直"))
                {
                    var removeStrings = new string[] { "诏命公卿", "招贤" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                }
                // 凡宜宣政事之日遇往亡则改宣为布。
                // if (goodThingDataList.Contains("宣政事") && badGodNames.Contains("往亡"))
                if (goodThingDataList.Contains("宣政事") && Array.IndexOf(badGodNames, "往亡") != -1)
                {
                    goodThingDataList.RemoveAll("宣政事");
                    goodThingDataList.Add("布政事");
                }
                // 凡月厌忌行幸、上官，不注宜颁诏、施恩封拜、诏命公卿、招贤、举正直。遇宜宣政事之日，则改宣为布。
                // if (badGodNames.Contains("月厌"))
                if (Array.IndexOf(badGodNames, "月厌") != -1)
                {
                    var removeStrings = new string[] { "颁诏", "施恩", "招贤", "举正直" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                    if (goodThingDataList.Contains("宣政事"))
                    {
                        goodThingDataList.RemoveAll("宣政事");
                        goodThingDataList.Add("布政事");
                    }
                    // 凡土府、土符、地囊，只注忌补垣，亦不注宜塞穴。
                    var badStrings = new string[] { "土府", "土符", "地囊" };
                    foreach (var badString in badStrings)
                    {
                        // if (badGodNames.Contains(badString))
                        if (Array.IndexOf(badGodNames, badString) != -1)
                        {
                            if (!badThingDataList.Contains("补垣"))
                                badThingDataList.Add("补垣");
                            goodThingDataList.RemoveAll("塞穴");
                            break;
                        }
                    }
                }
                // 凡开日，不注宜破土、安葬、启攒，亦不注忌。遇忌则注。
                if (today12DayOfficer.Contains("开"))
                {
                    var removeStrings = new string[] { "破土", "安葬", "启攒" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                }
                // 凡四忌、四穷只忌安葬。如遇鸣吠、鸣吠对亦不注宜破土、启攒。
                // if (badGodNames.Contains("四忌") || badGodNames.Contains("四穷"))
                if (Array.IndexOf(badGodNames, "四忌") != -1 || Array.IndexOf(badGodNames, "四穷") != -1)
                {
                    if (!badThingDataList.Contains("安葬"))
                        badThingDataList.Add("安葬");
                    var removeStrings = new string[] { "破土", "启攒" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                }
                // if (goodGodNames.Contains("鸣吠") || goodGodNames.Contains("鸣吠对"))
                if (Array.IndexOf(goodGodNames, "鸣吠") != -1 || Array.IndexOf(goodGodNames, "鸣吠对") != -1)
                {
                    var removeStrings = new string[] { "破土", "启攒" };
                    foreach (var removeString in removeStrings)
                        goodThingDataList.RemoveAll(removeString);
                }
                // 凡天吏、大时不以死败论者，遇四废、岁薄、逐阵仍以死败论。
                // 凡岁薄、逐阵日所宜事，照月厌所忌删，所忌仍从本日。
                // 二月甲戌、四月丙申、六月甲子、七月戊申、八月庚辰、九月辛卯、十月甲子、十二月甲子，德和与赦、愿所汇之辰，诸事不忌。
                if (d.Contains(new string[] { "空", "甲戌", "空", "丙申", "空", "甲子", "戊申", "庚辰", "辛卯", "甲子", "空", "甲子" }[lmn - 1]))
                {
                    badThingDataList.Clear();
                    badThingDataList.Add("诸事不忌");
                }
                // if len(set(self.goodGodName).intersection(set(['岁德合', '月德合', '天德合']))) and len(set(self.goodGodName).intersection(set(['天赦', '天愿']))):
                // if (goodGodNames.Intersect(new string[] { "岁德合", "月德合", "天德合" }).Count() > 0 && goodGodNames.Intersect(new string[] { "天赦", "天愿" }).Count() > 0)
                var deheStrings = new string[] { "岁德合", "月德合", "天德合" };
                var deheStringFound = false;
                var tianStrings = new string[] { "天赦", "天愿" };
                var tianStringFound = false;
                foreach (var deheString in deheStrings)
                {
                    if (Array.IndexOf(goodGodNames, deheString) != -1)
                    {
                        deheStringFound = true;
                        break;
                    }
                }
                foreach (var tianString in tianStrings)
                {
                    if (Array.IndexOf(goodGodNames, tianString) != -1)
                    {
                        tianStringFound = true;
                        break;
                    }
                }
                if (deheStringFound && tianStringFound)
                {
                    badThingDataList.Clear();
                    badThingDataList.Add("诸事不忌");
                }
                // 书中未明注忌不注宜
            }
            var rmThings = new DataList();
            for (var i = 0; i < badThingDataList.Count; i++)
            {
                var item = badThingDataList[i].String;
                if (goodThingDataList.Contains(item))
                    rmThings.Add(item);
            }
            if (rmThings.Count > 0 && !rmThings[0].String.Contains("诸事"))
            {
                for (var i = 0; i < rmThings.Count; i++)
                    goodThingDataList.RemoveAll(rmThings[i].String);
            }

            // 为空清理
            if (badThingDataList.Count == 0)
                badThingDataList.Add("诸事不忌");
            if (goodThingDataList.Count == 0)
                goodThingDataList.Add("诸事不宜");

            goodThings = new string[goodThingDataList.Count];
            for (var i = 0; i < goodThingDataList.Count; i++)
            {
                goodThings[i] = goodThingDataList[i].String;
            }

            badThings = new string[badThingDataList.Count];
            for (var i = 0; i < badThingDataList.Count; i++)
            {
                badThings[i] = badThingDataList[i].String;
            }
            // 输出排序调整
            Array.Sort((Array)goodThings);
            Array.Sort((Array)badThings);
        }
        /// <summary>
        /// 配合angel、demon的数据结构的吉神凶神筛选
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="godDb"></param>
        /// <param name="nameKey"></param>
        /// <returns></returns>
        static DataDictionary GetTodayGoodBadThing(ref DataDictionary dic, DataDictionary godDb, string nameKey)
        {
            var godDbKeys = godDb.GetKeys();
            for (var i = 0; i < godDbKeys.Count; i++)
            {
                var godItem0 = godDbKeys[i].String;
                var godItem = godDb[godItem0].DataList;
                var godItem1T = godItem[0];
                var godItem2T = godItem[1];
                var godItem3 = godItem[2].DataList;
                var godItem4 = godItem[3].DataList;
                // Debug.Log($"godItem0:{godItem0} godItem1T:{godItem1T} godItem2T:{godItem2T} godItem3:{godItem3} godItem4:{godItem4}");
                if (IfGodItem(godItem1T, godItem2T))
                    AddGoodBadThing(ref dic, nameKey, godItem0, godItem3, godItem4);
            }
            return dic;
        }
        static bool IfGodItem(DataToken godItem1T, DataToken godItem2T)
        {
            if (godItem1T.TokenType == TokenType.String && godItem2T.TokenType == TokenType.String)
            {

                var godItem1 = godItem1T.String;
                var godItem2 = godItem2T.String;
                if (godItem2.Contains(godItem1))
                    return true;
            }
            else if (godItem1T.TokenType == TokenType.Boolean)
            {
                return godItem1T.Boolean;
            }
            else if (godItem1T.TokenType == TokenType.Int)
            {
                var godItem1 = godItem1T.Int;
                var godItem2 = godItem2T.DataList;
                if (godItem2.Contains(godItem1))
                    return true;
            }
            else if (godItem1T.TokenType == TokenType.DataList)
            {
                var godItem1 = godItem1T.DataList;
                var godItem2 = godItem2T.DataList;
                var godItem2Count = godItem2.Count;
                var godItem1First = godItem1[0].Int;
                var godItem1Second = godItem1[1];
                var item1IsString = godItem1Second.TokenType == TokenType.String;
                // Debug.Log($"godItem1First:{godItem1First} godItem1Second:{godItem1Second} item1IsString:{item1IsString}");
                for (var j = 0; j < godItem2Count; j++)
                {
                    var godItem2Item = godItem2[j].DataList;
                    if (godItem2Item.Count != 2) { continue; }
                    var godItem2ItemFirst = godItem2Item[0].Int;
                    if (godItem1First != godItem2ItemFirst) { continue; }
                    var godItem2ItemSecond = godItem2Item[1];
                    if (item1IsString && godItem1Second.String == godItem2ItemSecond.String)
                        return true;
                    else if (godItem1Second.TokenType == TokenType.Int
                    && godItem1Second.Int == godItem2ItemSecond.Int)
                        return true;
                }
            }
            // switch (true)
            // {
            //     case true
            //     when godItem1T.TokenType == TokenType.String
            //     && godItem2T.TokenType == TokenType.String:
            //         {
            //             var godItem1 = godItem1T.String;
            //             var godItem2 = godItem2T.String;
            //             if (godItem2.Contains(godItem1))
            //                 return true;
            //         }
            //         break;
            //     case true
            //     when godItem1T.TokenType == TokenType.Boolean:
            //         // && godItem2T.TokenType == TokenType.Boolean:
            //         {
            //             var godItem1 = godItem1T.Boolean;
            //             if (godItem1)
            //                 return true;
            //         }
            //         break;
            //     case true
            //     when godItem1T.TokenType == TokenType.Int:
            //         // && godItem2T.TokenType == TokenType.DataList:
            //         {
            //             var godItem1 = godItem1T.Int;
            //             var godItem2 = godItem2T.DataList;
            //             if (godItem2.Contains(godItem1))
            //                 return true;
            //         }
            //         break;
            //     case true
            //     when godItem1T.TokenType == TokenType.DataList:
            //         // && godItem2T.TokenType == TokenType.DataList:
            //         {
            //             var godItem1 = godItem1T.DataList;
            //             var godItem2 = godItem2T.DataList;
            //             var godItem1First = godItem1[0].Int;
            //             var godItem1Second = godItem1[1];
            //             var item1IsString = godItem1Second.TokenType == TokenType.String;
            //             for (var j = 0; j < godItem2.Count; j++)
            //             {
            //                 var godItem2Item = godItem2[j].DataList;
            //                 var godItem2ItemFirst = godItem2Item[0].Int;
            //                 if (godItem1First != godItem2ItemFirst) { continue; }
            //                 var godItem2ItemSecond = godItem2Item[1];
            //                 if (item1IsString && godItem1Second.String == godItem2ItemSecond.String)
            //                     return true;
            //                 else if (godItem1Second.TokenType == TokenType.Int
            //                 && godItem1Second.Int == godItem2ItemSecond.Int)
            //                     return true;
            //             }
            //         }
            //         break;
            // }
            return false;
        }
        static DataDictionary AddGoodBadThing(ref DataDictionary dic, string nameKey, string godItem0, DataList godItem3, DataList godItem4)
        {
            var goodThingKey = "goodThing";
            var badThingKey = "badThing";
            if (!dic[nameKey].DataList.Contains(godItem0))
                dic[nameKey].DataList.Add(godItem0);

            for (var j = 0; j < godItem3.Count; j++)
            {
                var item = godItem3[j].String;
                if (!dic[goodThingKey].DataList.Contains(item))
                    dic[goodThingKey].DataList.Add(item);
            }
            for (var j = 0; j < godItem4.Count; j++)
            {
                var item = godItem4[j].String;
                if (!dic[badThingKey].DataList.Contains(item))
                    dic[badThingKey].DataList.Add(item);
            }
            return dic;
        }
        /// <summary>
        /// 第一方案：《钦定协纪辨方书》古书影印版，宜忌等第表<br />
        /// 凡铺注《万年历》、《通书》，先依用事次第察其所宜忌之日，于某日下注宜某事，某日下注忌某事，次按宜忌，较量其凶吉之轻重，以定去取。<br />
        /// 从忌亦从宜
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        static DataDictionary BadDrewGood(ref DataDictionary dic)
        {
            // for removeThing in list(set(dic['goodThing']).intersection(set(dic['badThing']))):
            var goodThings = dic["goodThing"].DataList.DeepClone();
            for (var i = 0; i < goodThings.Count; i++)
            {
                var goodThingString = goodThings[i].String;
                if (dic["badThing"].DataList.Contains(goodThingString))
                {
                    dic["badThing"].DataList.RemoveAll(goodThingString);
                    dic["goodThing"].DataList.RemoveAll(goodThingString);
                }
            }
            return dic;
        }

        /// <summary>
        /// 从忌不从宜
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        static DataDictionary BadOppressGood(ref DataDictionary dic)
        {
            var goodThings = dic["goodThing"].DataList.DeepClone();
            for (var i = 0; i < goodThings.Count; i++)
            {
                var goodThingString = goodThings[i].String;
                if (!dic["badThing"].DataList.Contains(goodThingString))
                {
                    dic["goodThing"].DataList.RemoveAll(goodThingString);
                }
            }
            return dic;
        }
        /// <summary>
        /// 从宜不从忌
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        static DataDictionary GoodOppressBad(ref DataDictionary dic)
        {
            var badThings = dic["badThing"].DataList.DeepClone();
            for (var i = 0; i < badThings.Count; i++)
            {
                var badThingString = badThings[i].String;
                if (!dic["goodThing"].DataList.Contains(badThingString))
                {
                    dic["badThing"].DataList.RemoveAll(badThingString);
                }
            }
            return dic;
        }
        /// <summary>
        /// 诸事不宜
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        static DataDictionary NothingGood(ref DataDictionary dic)
        {
            dic["goodThing"].DataList.Clear();
            dic["goodThing"].DataList.Add("诸事不宜");
            dic["badThing"].DataList.Clear();
            dic["badThing"].DataList.Add("诸事不宜");
            return dic;
        }
    }
}
