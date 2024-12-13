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
using System.Linq;

namespace Sonic853.Udon.CnLunar
{
    public class Lunar : UdonSharpBehaviour
    {
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

            // 哈哈，我写个锤子
            // angelDemon = GetAngelDemon()
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
        /// 获取数字形式的农历日期
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
            DateTime date,
            int lunarMonth,
            int lunarDay,
            string todaySolarTerms
        )
        {
            var temp = new DataList();
            var legalsolarTermsHolidayDic = Holidays.LegalsolarTermsHolidayDic();
            if (legalsolarTermsHolidayDic.TryGetValue(todaySolarTerms, out var value))
            {
                // temp += "清明节";
                temp.Add(value.String);
            }
            var legalHolidaysDic = Holidays.LegalHolidaysDic();
            if (legalHolidaysDic.TryGetValue($"{date.Month},{date.Day}", out value))
            {
                temp.Add(value.String);
            }
            var legalLunarHolidaysDic = Holidays.LegalLunarHolidaysDic();
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
            var badGodDic = Config.BadGodDic();
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
                                    todayAllGodNames.Contains(godname)
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
            var levelList = Config.LevelList();
            todayLevel = l;
            todayLevelName = l == -1 ? "无" : levelList[l].String;
            var thingLevelList = Config.ThingLevelList();
            isDe = false;
            var thingLevelStrings = Config.ThingLevelStrings();
            foreach (var goodGodName in goodGodNames)
            {
                if (thingLevelStrings.Contains(goodGodName))
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
            DateTime date,
            DataDictionary thisYearSolarTermsDic,
            string today12DayOfficer,
            string today28Star,
            string day8Char,
            string godType,
            string phaseOfMoon,
            int dayEarthNum,
            int dayHeavenlyEarthNum,
            int seasonNum,
            int yearHeavenNum,
            int yearEarthNum,
            int monthEarthNum,
            int lunarDay,
            int lunarMonth,
            int nextSolarTermYear,
            int nextSolarNum
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
            var officerThings = Config.OfficerThings();
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
            var gbDic = new DataDictionary() {
                {
                    "goodName", new DataList()
                },
                {
                    "badName", new DataList()
                },
                {
                    "goodThing", goodThing
                },
                {
                    "badThing", badThing
                },
            };
            var mrY13 = Config.MrY13();
            var tomorrow = date + TimeSpan.FromDays(1);
            var tomorrowAsInt = tomorrow.Month * 100 + tomorrow.Day;
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
            var day8CharThings = Config.Day8CharThing();
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
            if (nextSolarNum >= 4 && nextSolarNum < 9 && thingFishStrings.Contains(o))
            {
                var dataList = gbDic["goodThing"].DataList;
                if (!dataList.Contains("取鱼"))
                    gbDic["goodThing"].DataList.Add("取鱼");
            }
            // 霜降后立春前执日、危日、收日 宜 畋猎
            // if (self.nextSolarNum in range(20, 24) or self.nextSolarNum in range(0, 3)) and o in ['执', '危', '收']:
            if ((nextSolarNum >= 20 && nextSolarNum < 24 || nextSolarNum >= 0 && nextSolarNum < 3) && thingFishStrings.Contains(o))
            {
                var dataList = gbDic["goodThing"].DataList;
                if (!dataList.Contains("畋猎"))
                    gbDic["goodThing"].DataList.Add("畋猎");
            }
            // 立冬后立春前危日 午日 申日 宜 伐木
            // if (self.nextSolarNum in range(21, 24) or self.nextSolarNum in range(0, 3)) and (o in ['危'] or d in ['午', '申']):
            if ((nextSolarNum >= 21 && nextSolarNum < 24 || nextSolarNum >= 0 && nextSolarNum < 3) && (o == "危" || d == "午" || d == "申"))
            {
                var dataList = gbDic["goodThing"].DataList;
                if (!dataList.Contains("伐木"))
                    gbDic["goodThing"].DataList.Add("伐木");
            }
            // 每月一日 六日 十五 十九日 二十一日 二十三日 忌 整手足甲
            var days = new int[] { 1, 6, 15, 19, 21, 23 };
            if (days.Contains(ldn))
            {
                var dataList = gbDic["badThing"].DataList;
                if (!dataList.Contains("整手足甲"))
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
                var dataList = gbDic["badThing"].DataList;
                if (!dataList.Contains("求医疗病"))
                    gbDic["badThing"].DataList.Add("求医疗病");
            }
            // 由于正月建寅，men参数使用排序是从子开始，所以对照书籍需要将循环八字列向右移两位，也就是映射正月的是在第三个字
            var angel = new DataList();
        }
    }
}