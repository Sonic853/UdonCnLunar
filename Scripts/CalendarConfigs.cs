
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Utility;


namespace Sonic853.Udon.CnLunar
{
    public class CalendarConfigs : UdonSharpBehaviour
    {
        // 01963B
        public Color32 normalColor = new Color32(1, 150, 59, 255);
        public Color32 holidaysColor = new Color32(253, 40, 21, 255);
        [SerializeField] Transform calendarHolidayInfos;
        CalendarHolidayInfo[] _calendarHolidayInfos;
        [SerializeField] SkinnedMeshRenderer[] lunarBodys;
        [SerializeField] CalendarRender calendarRender;
        [SerializeField] string testDate = "";
        void Start()
        {
            if (calendarHolidayInfos != null)
            {
                var udons = (CalendarHolidayInfo[])calendarHolidayInfos.GetComponentsInChildren(typeof(UdonBehaviour));
                var indexlist = new DataList();
                for (var i = 0; i < udons.Length; i++)
                {
                    var udonTypeName = udons[i].GetUdonTypeName();
                    if (udonTypeName == "Sonic853.Udon.CnLunar.CalendarHolidayInfo")
                    {
                        indexlist.Add(i);
                    }
                }
                _calendarHolidayInfos = new CalendarHolidayInfo[indexlist.Count];
                for (int i = 0; i < indexlist.Count; i++)
                {
                    _calendarHolidayInfos[i] = udons[indexlist[i].Int];
                }
            }
            if (calendarRender.calendarHolidayInfos.Length == 0) calendarRender.calendarHolidayInfos = _calendarHolidayInfos;
            else
            {
                var lists = new CalendarHolidayInfo[calendarRender.calendarHolidayInfos.Length + _calendarHolidayInfos.Length];
                Array.Copy(calendarRender.calendarHolidayInfos, lists, calendarRender.calendarHolidayInfos.Length);
                Array.Copy(_calendarHolidayInfos, 0, lists, calendarRender.calendarHolidayInfos.Length, _calendarHolidayInfos.Length);
                calendarRender.calendarHolidayInfos = lists;
            }
            calendarRender.normalColor = normalColor;
            calendarRender.holidaysColor = holidaysColor;
            if (lunarBodys.Length > 0) calendarRender.lunarBodys = lunarBodys;
            calendarRender.Run(testDate);
        }
    }
}
