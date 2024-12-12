﻿// 24节气模块\节气数据16进制加解密
// author: cuba3/Sonic853
// github: https://github.com/OPN48/pyLunarCalendar
using System;
using Sonic853.Udon.ArrayPlus;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.CnLunar
{
    public class Solar24 : UdonSharpBehaviour
    {
        /// <summary>
        /// 解压缩16进制用
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rangeEndNum"></param>
        /// <param name="charCountLen"></param>
        /// <returns></returns>
        public static long[] UnZipSolarTermsList(long data, long rangeEndNum = 24, long charCountLen = 2)
        {
            var list = new long[0];
            for (var i = 1; i <= rangeEndNum; i++)
            {
                var right = charCountLen * (rangeEndNum - i);
                long x = data >> (int)right;
                long c = 1 << (int)charCountLen;
                Math.DivRem(x, c, out var q);
                list = UdonArrayPlus.Insert(ref list, 0, q);
            }
            return Tools.AbListMerge(list, Config.ENC_VECTOR_LIST());
        }
        public static long[] GetTheYearAllSolarTermsList(int year)
        {
            return UnZipSolarTermsList(Config.SOLAR_TERMS_DATA_LIST()[year - Config.START_YEAR()]);
        }
    }
}