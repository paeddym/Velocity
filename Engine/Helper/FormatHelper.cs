using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class FormatHelper
    {
        public static string FormatTime(double timeSeconds)
        {
            int minutes = (int)(timeSeconds / 60);
            int seconds = (int)(timeSeconds % 60);
            int milliseconds = (int)((timeSeconds - Math.Floor(timeSeconds)) * 1000);
            return $"{minutes:D2}:{seconds:D2}.{milliseconds:D3}";
        }

        public static string FormatSplitTime(double timeSeconds)
        {
            double absTime = Math.Abs(timeSeconds);
            int seconds = (int)(absTime % 60);
            int milliseconds = (int)((absTime - Math.Floor(absTime)) * 1000);
            return timeSeconds < 0 ? $"- {seconds:D2}.{milliseconds:D3}" : $"+ {seconds:D2}.{milliseconds:D3}";
        }
    }
}
