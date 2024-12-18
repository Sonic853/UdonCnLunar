
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


namespace Sonic853.Udon.CnLunar
{
    public class CalendarConfigs : UdonSharpBehaviour
    {
        // 01963B
        public Color32 normalColor = new Color32(1, 150, 59, 255);
        public Color32 holidaysColor = new Color32(253, 40, 21, 255);
        [SerializeField] CalendarHolidayInfo[] calendarHolidayInfos;
        [SerializeField] CalendarRender calendarRender;
        [SerializeField] string testDate = "";
        void Start()
        {
            if (calendarRender.calendarHolidayInfos.Length == 0) calendarRender.calendarHolidayInfos = calendarHolidayInfos;
            else
            {
                var lists = new CalendarHolidayInfo[calendarRender.calendarHolidayInfos.Length + calendarHolidayInfos.Length];
                Array.Copy(calendarRender.calendarHolidayInfos, lists, calendarRender.calendarHolidayInfos.Length);
                Array.Copy(calendarHolidayInfos, 0, lists, calendarRender.calendarHolidayInfos.Length, calendarHolidayInfos.Length);
                calendarRender.calendarHolidayInfos = lists;
            }
            calendarRender.normalColor = normalColor;
            calendarRender.holidaysColor = holidaysColor;
            calendarRender.Run(testDate);
        }
    }
}
