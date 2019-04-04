using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmina.Web.Extension
{
    public static class DateTimeExtensions
    {
        /*
         * get first day of week 
         */
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        /*
         * get first day of week 
         */
        public static DateTime GetFirstDayOfWeek(this DateTime dayInWeek, DayOfWeek firstDay)
        {
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        /*
         * get datetime as string
         */
        public static string GetLongDateString(this DateTime dt)
        {
            return dt.ToString(DateFormat);
        }
        /*
         * datetime format and default value added
         */
        public static string DateFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

        /*
         * get datetime as different datetime format
         */
        public static DateTime GetDateByDateTimeFormat(this DateTime dt)
        {
            return DateTime.ParseExact(dt.GetLongDateString(), DateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
