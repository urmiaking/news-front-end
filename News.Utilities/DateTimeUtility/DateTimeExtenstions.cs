using System;
using System.Collections.Generic;
using System.Text;

namespace News.Utilities.DateTimeUtility
{
    public static class DateTimeExtenstions
    {
        public static string ElapsedTime(this DateTime value)
        {
            var now = DateTime.Now;
            TimeSpan elapsed = now.Subtract(value);

            var result = "";
            if (elapsed.Days < 1)
            {
                if (elapsed.Hours < 1)
                {
                    result = "در " + Math.Round(elapsed.TotalMinutes) + " دقیقه قبل";
                }
                else
                {
                    result = "در " + Math.Round(elapsed.TotalHours) + " ساعت قبل";
                }
            }
            else
            {
                result = "در " + Math.Round(elapsed.TotalDays) + " روز قبل";
            }

            return result;
        }
    }
}
