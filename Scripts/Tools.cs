
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.CnLunar
{
    public class Tools : UdonSharpBehaviour
    {
        public static long[] AbListMerge(long[] a, long[] b, long type = 1)
        {
            var c = new long[a.Length];
            for (var i = 0; i < a.Length; i++)
            {
                c[i] = a[i] + b[i] * type;
            }
            return c;
        }
    }
}
