
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.CnLunar
{
    public class CalendarHolidayInfo : UdonSharpBehaviour
    {
        public string solarTerm = "";
        public bool isLunarHoliday = false;
        public int month = 0;
        public int day = 0;
        public Sprite image;
        public int template = 0;
    }
}
