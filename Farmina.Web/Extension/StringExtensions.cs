using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmina.Web.Extension
{
    public static class StringExtensions
    {
        public static string GetHourAndMinutesFromMinutes(this int? input)
        {
            if (!input.HasValue)
                return "";
            //
            //get hours 
            int hour = (int)input / 60;
            //
            //get minutes
            int minute = (int)input % 60 > 0 ? (int)input % 60 : 0;
            //string format
            string sFormat = "{0:D2}h" + (minute > 0 ? "{1:D2}m" : "");
            return string.Format(sFormat, hour, minute);
        }

        public static string GetDayOfWeekShortNameAsGerman(this string day)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { "Monday", "Mo." }, //Montag(Mo)
                { "Tuesday", "Di." }, //Dienstag(Di)
                { "Wednesday", "Mi." }, // Mittwoch(Mi)
                { "Thursday", "Do." }, // Donnerstag(Do.)
                { "Friday", "Fr." },  // Freitag(Fr.)
                { "Saturday", "Sa." },// Samstag(Sa.)
                { "Sunday", "So." } // Sonntag(So.)
            };

            if (dictionary.ContainsKey(day))
                day = dictionary.Where(x => x.Key == day).FirstOrDefault().Value;

            return day;
        }

        public static string GetDayOfWeekLongNameAsGerman(this string day)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { "Monday", "Montag" },
                { "Tuesday", "Dienstag" },
                { "Wednesday", "Mittwoch" },
                { "Thursday", "Donnerstag" },
                { "Friday", "Freitag" },
                { "Saturday", "Samstag" },
                { "Sunday", "Sonntag" }
            };

            if (dictionary.ContainsKey(day))
                day = dictionary.Where(x => x.Key == day).FirstOrDefault().Value;

            return day;
        }
        
        public static string GetAppSetting(this string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string CleanFromSpecialChars(this string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return string.Empty;

                //clean string with space
                fileName = fileName.Trim();

                //get all invalid chars and special chars
                var charsForFileName = Path.GetInvalidFileNameChars().Select(x => x.ToString());
                var charsForPathName = Path.GetInvalidPathChars().Select(x => x.ToString());
                var chars = new List<string>() { "\\\"", "[", "]", "{", "}", ",", "=", "&", "#", "'", "\"", "/", "." };
                //
                chars.AddRange(charsForFileName);
                chars.AddRange(charsForPathName);
                //
                //Removed invalid chars
                foreach (string c in chars)
                {
                    if (fileName.IndexOf(c) < 0)
                        continue;

                    fileName = fileName.Replace(c, string.Empty);
                }
                //
                //If special chars have in middle on the words then remove all white space
                var names = fileName.Split(' ').ToList()
                                    .Where(x => !string.IsNullOrEmpty(x))
                                    .Select(s => s).ToList();
                //
                //join again as sentence
                fileName = string.Join(" ", names);
                return fileName;
            }
            catch
            {
                return fileName;
            }

        }


        public static bool IsNull<T>(this T me)
        {
            if (me is INullable && (me as INullable).IsNull) return true;
            var type = typeof(T);
            if (type.IsValueType)
            {
                if (!ReferenceEquals(Nullable.GetUnderlyingType(type), null) && me.GetHashCode() == 0) return true;
            }
            else
            {
                if (ReferenceEquals(me, null)) return true;
                if (Convert.IsDBNull(me)) return true;
            }
            return false;
        }

        public static bool IsNull<T>(this T? me) where T : struct
        {
            return !me.HasValue;
        }

        public static string CreateVersion()
        {
            return Guid.NewGuid().ToString().ToUpperInvariant().Replace("-", "");
        }
    }
}
