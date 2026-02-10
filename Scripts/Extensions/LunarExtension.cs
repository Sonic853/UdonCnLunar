
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.CnLunar.Extensions
{
    public static class LunarExtension
    {
        public static int GetBeginningOfSpringX(this Lunar lunar) => Lunar.GetBeginningOfSpringX(
            lunar.nextSolarNum,
            lunar.spanDays,
            lunar.year8Char
        );
        public static string GetLunarYearCN(this Lunar lunar) => Lunar.GetLunarYearCN(lunar.lunarYear);
        public static string GetLunarMonthCN(this Lunar lunar) => Lunar.GetLunarMonthCN(
            lunar.lunarMonth,
            lunar.isLunarLeapMonth,
            lunar.monthDaysList,
            out lunar.lunarMonthLong
        );
        public static string GetLunarMonthCN(
            this Lunar lunar,
            out bool lunarMonthLong
        ) => Lunar.GetLunarMonthCN(
            lunar.lunarMonth,
            lunar.isLunarLeapMonth,
            lunar.monthDaysList,
            out lunarMonthLong
        );
        public static string GetLunarCn(this Lunar lunar) => Lunar.GetLunarCn(
            lunar.lunarYear,
            lunar.lunarMonth,
            lunar.lunarDay,
            lunar.isLunarLeapMonth,
            lunar.monthDaysList,
            out lunar.lunarYearCn,
            out lunar.lunarMonthCn,
            out lunar.lunarDayCn,
            out lunar.lunarMonthLong
        );
        public static string GetLunarCn(
            this Lunar lunar,
            out string lunarYearCn,
            out string lunarMonthCn,
            out string lunarDayCn,
            out bool lunarMonthLong
        ) => Lunar.GetLunarCn(
            lunar.lunarYear,
            lunar.lunarMonth,
            lunar.lunarDay,
            lunar.isLunarLeapMonth,
            lunar.monthDaysList,
            out lunarYearCn,
            out lunarMonthCn,
            out lunarDayCn,
            out lunarMonthLong
        );
        /// <summary>
        /// 月相
        /// </summary>
        /// <returns></returns>
        public static string GetPhaseOfMoon(this Lunar lunar) => Lunar.GetPhaseOfMoon(
            lunar.lunarDay,
            lunar.lunarMonthLong
        );
        /// <summary>
        /// 生肖
        /// </summary>
        /// <returns></returns>
        public static string GetChineseYearZodiac(this Lunar lunar) => Lunar.GetChineseYearZodiac(
            lunar.lunarYear,
            lunar._x
        );
        public static string GetChineseZodiacClash(this Lunar lunar) => Lunar.GetChineseZodiacClash(
            lunar.dayEarthNum,
            out lunar.zodiacMark6,
            out lunar.zodiacMark3List,
            out lunar.zodiacWin,
            out lunar.zodiacLose
        );
        public static string GetChineseZodiacClash(
            this Lunar lunar,
            out string zodiacMark6,
            out string[] zodiacMark3List,
            out string zodiacWin,
            out string zodiacLose
        ) => Lunar.GetChineseZodiacClash(
            lunar.dayEarthNum,
            out zodiacMark6,
            out zodiacMark3List,
            out zodiacWin,
            out zodiacLose
        );
        /// <summary>
        /// 输出当前日期中文星期几
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetWeekDayCn(this Lunar lunar) => Lunar.GetWeekDayCn(lunar.date);
        /// <summary>
        /// 获取数字形式的农历日期
        /// 返回的月份，高4bit为闰月月份，低4bit为其它正常月份
        /// </summary>
        public static void GetLunarDateNum(this Lunar lunar) => Lunar.GetLunarDateNum(
            lunar.date,
            out lunar.lunarYear,
            out lunar.lunarMonth,
            out lunar.lunarDay,
            out lunar.isLunarLeapMonth,
            out lunar.spanDays,
            out lunar.monthDaysList
        );
        /// <summary>
        /// 获取数字形式的农历日期
        /// 返回的月份，高4bit为闰月月份，低4bit为其它正常月份
        /// </summary>
        /// <param name="lunarYear">农历年</param>
        /// <param name="lunarMonth">农历月</param>
        /// <param name="lunarDay">农历日</param>
        public static void GetLunarDateNum(
            this Lunar lunar,
            out int lunarYear,
            out int lunarMonth,
            out int lunarDay
        ) => Lunar.GetLunarDateNum(
            lunar.date,
            out lunarYear,
            out lunarMonth,
            out lunarDay,
            out lunar.isLunarLeapMonth,
            out lunar.spanDays,
            out lunar.monthDaysList
        );
        /// <summary>
        /// 获取数字形式的农历日期
        /// 返回的月份，高4bit为闰月月份，低4bit为其它正常月份
        /// </summary>
        /// <param name="lunarYear">农历年</param>
        /// <param name="lunarMonth">农历月</param>
        /// <param name="lunarDay">农历日</param>
        public static void GetLunarDateNum(
            this Lunar lunar,
            out int lunarYear,
            out int lunarMonth,
            out int lunarDay,
            out bool isLunarLeapMonth,
            out int spanDays,
            out int[] monthDaysList
        ) => Lunar.GetLunarDateNum(
            lunar.date,
            out lunarYear,
            out lunarMonth,
            out lunarDay,
            out isLunarLeapMonth,
            out spanDays,
            out monthDaysList
        );
        /// <summary>
        /// 获取今天的节气
        /// </summary>
        /// <returns>节气</returns>
        public static string GetTodaySolarTerms(this Lunar lunar) => Lunar.GetTodaySolarTerms(
            lunar.date,
            out lunar.thisYearSolarTermsDateList,
            out lunar.nextSolarNum,
            out lunar.nextSolarTerm,
            out lunar.nextSolarTermYear,
            out lunar.nextSolarTermsMonth,
            out lunar.nextSolarTermsDay
        );
        /// <summary>
        /// 获取今天的节气
        /// </summary>
        /// <param name="thisYearSolarTermsDateList"></param>
        /// <param name="nextSolarNum"></param>
        /// <param name="nextSolarTerm"></param>
        /// <param name="nextSolarTermYear"></param>
        /// <param name="nextSolarTermsMonth"></param>
        /// <param name="nextSolarTermsDay"></param>
        /// <returns>节气</returns>
        public static string GetTodaySolarTerms(
            this Lunar lunar,
            out int[][] thisYearSolarTermsDateList,
            out int nextSolarNum,
            out string nextSolarTerm,
            out int nextSolarTermYear,
            out int nextSolarTermsMonth,
            out int nextSolarTermsDay
        ) => Lunar.GetTodaySolarTerms(
            lunar.date,
            out thisYearSolarTermsDateList,
            out nextSolarNum,
            out nextSolarTerm,
            out nextSolarTermYear,
            out nextSolarTermsMonth,
            out nextSolarTermsDay
        );
        public static string GetEastZodiac(this Lunar lunar) => Lunar.GetEastZodiac(lunar.nextSolarTerm);
        /// <summary>
        /// 八字部分
        /// </summary>
        /// <returns></returns>
        public static string GetYear8Char(this Lunar lunar) => Lunar.GetYear8Char(
            lunar.lunarYear,
            lunar._x
        );
        /// <summary>
        /// 月八字与节气相关
        /// </summary>
        /// <param name="nextSolarNum"></param>
        /// <param name="_x"></param>
        /// <returns></returns>
        public static string GetMonth8Char(this Lunar lunar) => Lunar.GetMonth8Char(
            lunar.date,
            lunar.nextSolarNum
        );
        public static string GetDay8Char(this Lunar lunar) => Lunar.GetDay8Char(
            lunar.date,
            lunar.twohourNum,
            out lunar.dayHeavenlyEarthNum
        );
        public static string GetDay8Char(
            this Lunar lunar,
            out int dayHeavenlyEarthNum
        ) => Lunar.GetDay8Char(
            lunar.date,
            lunar.twohourNum,
            out dayHeavenlyEarthNum
        );
        public static string[] GetTwohour8CharList(this Lunar lunar) => Lunar.GetTwohour8CharList(lunar.day8Char);
        public static string GetTwohour8Char(this Lunar lunar) => Lunar.GetTwohour8Char(
            lunar.twohour8CharList,
            lunar.twohourNum
        );
        public static void GetThe8char(this Lunar lunar) => Lunar.GetThe8char(
            lunar.date,
            lunar.lunarYear,
            lunar._x,
            lunar.nextSolarNum,
            lunar.twohourNum,
            out lunar.year8Char,
            out lunar.month8Char,
            out lunar.day8Char,
            out lunar.dayHeavenlyEarthNum
        );
        public static void GetThe8char(
            this Lunar lunar,
            out string year8Char,
            out string month8Char,
            out string day8Char,
            out int dayHeavenlyEarthNum
        ) => Lunar.GetThe8char(
            lunar.date,
            lunar.lunarYear,
            lunar._x,
            lunar.nextSolarNum,
            lunar.twohourNum,
            out year8Char,
            out month8Char,
            out day8Char,
            out dayHeavenlyEarthNum
        );
        public static void GetEarthNum(this Lunar lunar) => Lunar.GetEarthNum(
            lunar.year8Char,
            lunar.month8Char,
            lunar.day8Char,
            out lunar.yearEarthNum,
            out lunar.monthEarthNum,
            out lunar.dayEarthNum
        );
        public static void GetEarthNum(
            this Lunar lunar,
            out int yearEarthNum,
            out int monthEarthNum,
            out int dayEarthNum
        ) => Lunar.GetEarthNum(
            lunar.year8Char,
            lunar.month8Char,
            lunar.day8Char,
            out yearEarthNum,
            out monthEarthNum,
            out dayEarthNum
        );
        public static void GetHeavenNum(this Lunar lunar) => Lunar.GetHeavenNum(
            lunar.year8Char,
            lunar.month8Char,
            lunar.day8Char,
            out lunar.yearHeavenNum,
            out lunar.monthHeavenNum,
            out lunar.dayHeavenNum
        );
        public static void GetHeavenNum(
            this Lunar lunar,
            out int yearHeavenNum,
            out int monthHeavenNum,
            out int dayHeavenNum
        ) => Lunar.GetHeavenNum(
            lunar.year8Char,
            lunar.month8Char,
            lunar.day8Char,
            out yearHeavenNum,
            out monthHeavenNum,
            out dayHeavenNum
        );
        /// <summary>
        /// 季节
        /// </summary>
        public static void GetSeason(this Lunar lunar) => Lunar.GetSeason(
            lunar.monthEarthNum,
            out lunar.lunarSeasonType,
            out lunar.lunarSeasonNum,
            out lunar.lunarMonthType,
            out lunar.lunarSeason,
            out lunar.lunarSeasonName
        );
        /// <summary>
        /// 季节
        /// </summary>
        /// <param name="lunarSeasonType"></param>
        /// <param name="lunarSeasonNum"></param>
        /// <param name="lunarMonthType"></param>
        /// <param name="lunarSeason"></param>
        /// <param name="lunarSeasonName"></param>
        public static void GetSeason(
            this Lunar lunar,
            out int lunarSeasonType,
            out int lunarSeasonNum,
            out string lunarMonthType,
            out string lunarSeason,
            out string lunarSeasonName
        ) => Lunar.GetSeason(
            lunar.monthEarthNum,
            out lunarSeasonType,
            out lunarSeasonNum,
            out lunarMonthType,
            out lunarSeason,
            out lunarSeasonName
        );
        /// <summary>
        /// 星座
        /// </summary>
        /// <returns></returns>
        public static string GetStarZodiac(this Lunar lunar) => Lunar.GetStarZodiac(lunar.date);
        public static string[] GetLegalHolidays(this Lunar lunar) => Lunar.GetLegalHolidays(
            lunar.holidays,
            lunar.date,
            lunar.lunarMonth,
            lunar.lunarDay,
            lunar.todaySolarTerms,
            lunar.monthDaysList
        );
        public static string[] GetLegalHolidays(
            this Lunar lunar,
            Holidays holidays,
            DateTime date,
            int lunarMonth,
            int lunarDay,
            string todaySolarTerms
            ) => Lunar.GetLegalHolidays(
            holidays,
            date,
            lunarMonth,
            lunarDay,
            todaySolarTerms,
            lunar.monthDaysList
        );
        public static string[] GetOtherHolidays(this Lunar lunar) => Lunar.GetOtherHolidays(
            lunar.holidays,
            lunar.date
        );
        /// <summary>
        /// 建除十二神，《淮南子》曰：正月建寅，则寅为建，卯为除，辰为满，巳为平，主生；午为定，未为执，主陷；申为破，主衡；酉为危，主杓；戍为成，主小德；亥为收，主大备；子为开，主太阳；丑为闭，主太阴。
        /// </summary>
        public static string GetToday12DayOfficer(this Lunar lunar) => Lunar.GetToday12DayOfficer(
            lunar.godType,
            lunar.lunarMonth,
            lunar.monthEarthNum,
            lunar.dayEarthNum,
            out lunar.today12DayOfficer,
            out lunar.today12DayGod
        );
        /// <summary>
        /// 建除十二神，《淮南子》曰：正月建寅，则寅为建，卯为除，辰为满，巳为平，主生；午为定，未为执，主陷；申为破，主衡；酉为危，主杓；戍为成，主小德；亥为收，主大备；子为开，主太阳；丑为闭，主太阴。
        /// </summary>
        public static string GetToday12DayOfficer(
            this Lunar lunar,
            out string today12DayOfficer,
            out string today12DayGod
        ) => Lunar.GetToday12DayOfficer(
            lunar.godType,
            lunar.lunarMonth,
            lunar.monthEarthNum,
            lunar.dayEarthNum,
            out today12DayOfficer,
            out today12DayGod
        );
        /// <summary>
        /// 八字与五行
        /// </summary>
        /// <returns></returns>
        public static string GetThe28Stars(this Lunar lunar) => Lunar.GetThe28Stars(lunar.date);
        public static string GetNayin(this Lunar lunar) => Lunar.GetNayin(lunar.day8Char);
        public static string[] GetLuckyGodsDirection(this Lunar lunar) => Lunar.GetLuckyGodsDirection(lunar.dayHeavenNum);
        public static string GetFetalGod(this Lunar lunar) => Lunar.GetFetalGod(lunar.day8Char);
        public static string[] GetTwohourLuckyList(this Lunar lunar) => Lunar.GetTwohourLuckyList(lunar.dayHeavenlyEarthNum);
        public static int GetTwohourNum(this Lunar lunar) => Lunar.GetTwohourNum(lunar.date);
        public static string GetMeridians(this Lunar lunar) => Lunar.GetMeridians(lunar.twohourNum);
        /// <summary>
        /// 今日宜忌
        /// </summary>
        public static void GetAngelDemon(
            this Lunar lunar,
            out int todayLevel,
            out string todayLevelName,
            out string thingLevelName,
            out bool isDe,
            out string[] goodGodNames,
            out string[] badGodNames,
            out string[] goodThings,
            out string[] badThings
        ) => Lunar.GetAngelDemon(
            lunar.config,
            lunar.date,
            lunar.thisYearSolarTermsDic,
            lunar.today12DayOfficer,
            lunar.today28Star,
            lunar.day8Char,
            lunar.godType,
            lunar.phaseOfMoon,
            lunar.month8Char,
            lunar.lunarMonthType,
            lunar.isYeargodDuty,
            lunar.dayEarthNum,
            lunar.dayHeavenlyEarthNum,
            lunar.lunarSeasonNum,
            lunar.yearHeavenNum,
            lunar.yearEarthNum,
            lunar.monthEarthNum,
            lunar.lunarDay,
            lunar.lunarMonth,
            lunar.nextSolarTermYear,
            lunar.nextSolarNum,
            out todayLevel,
            out todayLevelName,
            out thingLevelName,
            out isDe,
            out goodGodNames,
            out badGodNames,
            out goodThings,
            out badThings
        );
        /// <summary>
        /// 今日宜忌
        /// </summary>
        public static void GetAngelDemon(this Lunar lunar) => Lunar.GetAngelDemon(
            lunar.config,
            lunar.date,
            lunar.thisYearSolarTermsDic,
            lunar.today12DayOfficer,
            lunar.today28Star,
            lunar.day8Char,
            lunar.godType,
            lunar.phaseOfMoon,
            lunar.month8Char,
            lunar.lunarMonthType,
            lunar.isYeargodDuty,
            lunar.dayEarthNum,
            lunar.dayHeavenlyEarthNum,
            lunar.lunarSeasonNum,
            lunar.yearHeavenNum,
            lunar.yearEarthNum,
            lunar.monthEarthNum,
            lunar.lunarDay,
            lunar.lunarMonth,
            lunar.nextSolarTermYear,
            lunar.nextSolarNum,
            out lunar.todayLevel,
            out lunar.todayLevelName,
            out lunar.thingLevelName,
            out lunar.isDe,
            out lunar.goodGodNames,
            out lunar.badGodNames,
            out lunar.goodThings,
            out lunar.badThings
        );
    }
}
