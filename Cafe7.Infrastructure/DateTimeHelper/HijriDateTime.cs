using System;
using System.Globalization;

namespace Cafe7.Infrastructure.DateTimeHelper
{
    //public enum HijriDayOfWeek
    //{

    //}
    public class HijriDateTime:BaseDateTime //: IComparable, IFormattable, IConvertible, ISerializable, IComparable<HijriDateTime>, IEquatable<HijriDateTime>
    {
        /// <summary>
        /// Represents the largest possible value of Rayatis.HijriDateTime. This field is read-only.
        /// </summary>
        public static readonly HijriDateTime MaxValue = new HijriDateTime(9999, 12, 30, 23, 59, 59, 999);

        /// <summary>
        /// Represents the smallest possible value of Rayatis.DateTimeLibrary. This field is read-only.
        /// </summary>
        public static readonly HijriDateTime MinValue = new HijriDateTime(1, 1, 1, 12, 0, 0, 0);

        /// <summary>
        /// Gets the date component of this instance.
        /// 
        /// Returns:
        ///     A new Rayatis.DateTimeLibrary with the same date as this instance, and the time value
        ///     set to 12:00:00 midnight (00:00:00).
        /// </summary>
        public HijriDateTime Date
        {
            get
            {
                return new HijriDateTime(Year, Month, Day, 0, 0, 0, 0);
            }
        }

        /// <summary>
        ///Gets the day of the week represented by this instance.
        ///
        /// Returns:
        ///     A System.DayOfWeek enumerated constant that indicates the day of the week.
        ///     This property value ranges from zero, indicating Sunday, to six, indicating
        ///     Saturday.
        /// </summary>
        public int DayOfWeek
        {
            get
            {
                return (int)ToDateTime().DayOfWeek;
            }
        }

        public string DayName
        {
            get
            {
                return GetDayOfWeek(ToDateTime().DayOfWeek);
            }
        }

        /// <summary>
        /// Gets the day of the year represented by this instance.
        /// 
        /// Returns:
        ///      The day of the year, expressed as a value between 1 and 366.
        /// </summary>
        public int DayOfYear
        {
            get
            {
                HijriCalendar cc = new HijriCalendar();
                return cc.GetDayOfYear(ToDateTime());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long Ticks
        {
            get
            {
                return ToDateTime().Ticks;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan TimeOfDay
        {
            get
            {
                return ToDateTime().TimeOfDay;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static HijriDateTime Today
        {
            get
            {
                return new HijriDateTime(DateTime.Today);
            }
        }

        //public static HijriDateTime UtcNow { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        private string GetDayOfWeek(DayOfWeek dayOfWeek)
        {
            string dow = "";
            switch (dayOfWeek)
            {
                case System.DayOfWeek.Saturday:
                    dow = "السبت";
                    break;
                case System.DayOfWeek.Sunday:
                    dow = "الأحد";
                    break;
                case System.DayOfWeek.Monday:
                    dow = "الإثنين";
                    break;
                case System.DayOfWeek.Tuesday:
                    dow = "الثلاثاء";
                    break;
                case System.DayOfWeek.Wednesday:
                    dow = "الأربعاء";
                    break;
                case System.DayOfWeek.Thursday:
                    dow = "الخميس";
                    break;
                case System.DayOfWeek.Friday:
                    dow = "الجمعة";
                    break;
            }
            return dow;
        }

        public static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "محرم";
                case 2:
                    return "صفر";
                case 3:
                    return "ربيع الأول";
                case 4:
                    return "ربيع الثاني";
                case 5:
                    return "جمادي الأولي";
                case 6:
                    return "جمادي الثانية";
                case 7:
                    return "رجب";
                case 8:
                    return "شعبان";
                case 9:
                    return "رمضان";
                case 10:
                    return "شوال";
                case 11:
                    return "ذوالقعدة";
                case 12:
                    return "ذوالحجة";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public HijriDateTime()
        {
            Year = 1;
            Month = 1;
            Day = 1;
            Hour = 12;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticks"></param>
        public HijriDateTime(long ticks)
        {
            DateTime d = new DateTime(ticks);
            HijriCalendar cc = new HijriCalendar();
            Year = cc.GetYear(d);
            Month = cc.GetMonth(d);
            Day = cc.GetDayOfMonth(d);
            Hour = cc.GetHour(d);
            Minute = cc.GetMinute(d);
            Second = cc.GetSecond(d);
            Millisecond = (int)cc.GetMilliseconds(d);
        }

        //public HijriDateTime(long ticks,DateTimeKind kind)
        //{

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public HijriDateTime(int year, int month, int day)
            : this(year, month, day, 12, 0, 0, 0)
        {

        }

        //public HijriDateTime(int year, int month, int day, Calendar calendar)
        //{

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        public HijriDateTime(int year, int month, int day, int hour, int minute, int second)
            : this(year, month, day, hour, minute, second, 0)
        {

        }

        //public HijriDateTime(int year, int month, int day, int hour, int minute, int second, Calendar calendar)
        //{

        //}
        //public HijriDateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
        //{

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        public HijriDateTime(DateTime date)
        {
            HijriCalendar cc = new HijriCalendar();
            Year = cc.GetYear(date);
            Month = cc.GetMonth(date);
            Day = cc.GetDayOfMonth(date);
            Hour = cc.GetHour(date);
            Minute = cc.GetMinute(date);
            Second = cc.GetSecond(date);
            Millisecond = (int)cc.GetMilliseconds(date);
        }

        public static HijriDateTime FromDateTime(DateTime date)
        {
            return new HijriDateTime(date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <param name="millisecond"></param>
        public HijriDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            Millisecond = millisecond;
        }

        //public HijriDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar)
        //{

        //}
        //public HijriDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
        //{

        //}
        //public HijriDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, DateTimeKind kind)
        //{

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static TimeSpan operator -(HijriDateTime d1, HijriDateTime d2)
        {
            DateTime date1 = HijriDateTime.ToDateTime(d1);
            DateTime date2 = HijriDateTime.ToDateTime(d2);
            return date1 - date2;
        }

        //public static HijriDateTime operator -(HijriDateTime d, TimeSpan t)
        //{
        //    DateTime date = HijriDateTime.ToDateTime(d);


        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool operator !=(HijriDateTime d1, HijriDateTime d2)
        {
            DateTime date1 = HijriDateTime.ToDateTime(d1);
            DateTime date2 = HijriDateTime.ToDateTime(d2);
            return date1 != date2;
        }

        //public static DateTime operator +(HijriDateTime d, TimeSpan t)
        //{
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <(HijriDateTime t1, HijriDateTime t2)
        {
            DateTime date1 = HijriDateTime.ToDateTime(t1);
            DateTime date2 = HijriDateTime.ToDateTime(t2);
            return date1 < date2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <=(HijriDateTime t1, HijriDateTime t2)
        {
            DateTime date1 = HijriDateTime.ToDateTime(t1);
            DateTime date2 = HijriDateTime.ToDateTime(t2);
            return date1 <= date2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool operator ==(HijriDateTime d1, HijriDateTime d2)
        {
            DateTime date1 = HijriDateTime.ToDateTime(d1);
            DateTime date2 = HijriDateTime.ToDateTime(d2);
            return date1 == date2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >(HijriDateTime t1, HijriDateTime t2)
        {
            DateTime date1 = HijriDateTime.ToDateTime(t1);
            DateTime date2 = HijriDateTime.ToDateTime(t2);
            return date1 > date2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >=(HijriDateTime t1, HijriDateTime t2)
        {
            DateTime date1 = HijriDateTime.ToDateTime(t1);
            DateTime date2 = HijriDateTime.ToDateTime(t2);
            return date1 >= date2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(HijriDateTime date)
        {
            HijriCalendar cc = new HijriCalendar();
            return cc.ToDateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        public static DateTime ToDateTime(string date)
        {
            return HijriDateTime.ToDateTime(HijriDateTime.Parse(date));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DateTime ToDateTime()
        {
            return HijriDateTime.ToDateTime(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HijriDateTime Add(TimeSpan value)
        {
            DateTime date = HijriDateTime.ToDateTime(this);
            HijriDateTime pdate = new HijriDateTime(date.Add(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HijriDateTime AddDays(double value)
        {
            DateTime date = HijriDateTime.ToDateTime(this);
            HijriDateTime pdate = new HijriDateTime(date.AddDays(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HijriDateTime AddHours(double value)
        {
            DateTime date = HijriDateTime.ToDateTime(this);
            HijriDateTime pdate = new HijriDateTime(date.AddHours(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HijriDateTime AddMilliseconds(double value)
        {
            DateTime date = HijriDateTime.ToDateTime(this);
            HijriDateTime pdate = new HijriDateTime(date.AddMilliseconds(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HijriDateTime AddMinutes(double value)
        {
            DateTime date = HijriDateTime.ToDateTime(this);
            HijriDateTime pdate = new HijriDateTime(date.AddMinutes(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="months"></param>
        /// <returns></returns>
        public HijriDateTime AddMonths(int months)
        {
            DateTime date = HijriDateTime.ToDateTime(this);
            HijriDateTime pdate = new HijriDateTime(date.AddMonths(months));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HijriDateTime AddSeconds(double value)
        {
            DateTime date = HijriDateTime.ToDateTime(this);
            HijriDateTime pdate = new HijriDateTime(date.AddSeconds(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HijriDateTime AddTicks(long value)
        {
            DateTime date = HijriDateTime.ToDateTime(this);
            HijriDateTime pdate = new HijriDateTime(date.AddTicks(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public HijriDateTime AddYears(int value)
        {
            DateTime date = HijriDateTime.ToDateTime(this);
            HijriDateTime pdate = new HijriDateTime(date.AddYears(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static int Compare(HijriDateTime t1, HijriDateTime t2)
        {
            DateTime date1 = HijriDateTime.ToDateTime(t1);
            DateTime date2 = HijriDateTime.ToDateTime(t2);
            return DateTime.Compare(date1, date2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int CompareTo(HijriDateTime value)
        {
            DateTime date1 = HijriDateTime.ToDateTime(value);
            DateTime date2 = ToDateTime();
            return date2.CompareTo(date1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int CompareTo(object value)
        {
            DateTime date2 = ToDateTime();
            return date2.CompareTo(HijriDateTime.ToDateTime(value.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int DaysInMonth(int year, int month)
        {
            int days = 30;
            if (month < 7)
            {
                days = 31;
            }
            else if (month == 12)
            {
                HijriCalendar cc = new HijriCalendar();
                if (!cc.IsLeapYear(year))
                    days = 29;
            }
            return days;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Equals(HijriDateTime value)
        {
            DateTime date1 = HijriDateTime.ToDateTime(value);
            DateTime date2 = ToDateTime();
            return date2.Equals(date1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Equals(object value)
        {
            DateTime date = ToDateTime();
            return date.Equals(value);
        }

        public static bool Equals(HijriDateTime t1, HijriDateTime t2)
        {
            DateTime date1 = HijriDateTime.ToDateTime(t1);
            DateTime date2 = HijriDateTime.ToDateTime(t2);
            return DateTime.Equals(date1, date2);
        }

        public static HijriDateTime FromBinary(long dateData)
        {
            HijriDateTime pdate = new HijriDateTime(DateTime.FromBinary(dateData));
            return pdate;
        }

        public static HijriDateTime FromFileTime(long fileTime)
        {
            HijriDateTime pdate = new HijriDateTime(DateTime.FromFileTime(fileTime));
            return pdate;
        }

        public static HijriDateTime FromFileTimeUtc(long fileTime)
        {
            HijriDateTime pdate = new HijriDateTime(DateTime.FromFileTimeUtc(fileTime));
            return pdate;
        }

        public static HijriDateTime FromOADate(double d)
        {
            HijriDateTime pdate = new HijriDateTime(DateTime.FromOADate(d));
            return pdate;
        }

        //public string[] GetDateTimeFormats()
        //{
        //}
        //public string[] GetDateTimeFormats(char format)
        //{
        //}
        //public string[] GetDateTimeFormats(IFormatProvider provider)
        //{
        //}
        //public string[] GetDateTimeFormats(char format, IFormatProvider provider)
        //{
        //}
        
        public override int GetHashCode()
        {
            return 0;
        }

        //public TypeCode GetTypeCode()
        //{
        //}
        //public bool IsDaylightSavingTime()
        //{
        //}
        
        public static bool IsLeapYear(int year)
        {
            HijriCalendar cc = new HijriCalendar();
            return cc.IsLeapYear(year);
        }
        
        public static HijriDateTime Parse(string s)
        {
            HijriDateTime pdate = null;
            try
            {
                string[] d = s.Split('/');
                if (d.Length != 3)
                    throw new FormatException("String was not recognized as a valid HijriDateTime.");
                int year = Convert.ToInt32(d[0]), month = Convert.ToInt32(d[1]), day = Convert.ToInt32(d[2].Split(' ')[0]);
                HijriCalendar cc = new HijriCalendar();
                pdate = new HijriDateTime(cc.ToDateTime(year, month, day, 12, 0, 0, 0));
            }
            catch
            {
                throw new Exception("تاريخ نا معتبر مي باشد.");
            }
            return pdate;
        }
        
        //public static HijriDateTime Parse(string s, IFormatProvider provider)
        //{
        //    HijriDateTime pdate = new HijriDateTime(DateTime.Parse(s, provider));
        //    return pdate;
        //}
        //public static HijriDateTime Parse(string s, IFormatProvider provider, DateTimeStyles styles)
        //{
        //    HijriDateTime pdate = new HijriDateTime(DateTime.Parse(s, provider, styles));
        //    return pdate;
        //}
        //public static HijriDateTime ParseExact(string s, string format, IFormatProvider provider)
        //{
        //    HijriDateTime pdate = new HijriDateTime(DateTime.Parse(s, format, provider));
        //    return pdate;
        //}
        //public static HijriDateTime ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
        //{
        //    HijriDateTime pdate = new HijriDateTime(DateTime.Parse(s, format, provider, style));
        //    return pdate;
        //}
        //public static HijriDateTime ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
        //{
        //    HijriDateTime pdate = new HijriDateTime(DateTime.Parse(s,formats, provider, style));
        //    return pdate;
        //}
        //public static HijriDateTime SpecifyKind(DateTime value, DateTimeKind kind) { }

        public TimeSpan Subtract(HijriDateTime value)
        {
            DateTime date1 = HijriDateTime.ToDateTime(value);
            DateTime date2 = ToDateTime();
            return date2.Subtract(date1);
        }

        public HijriDateTime Subtract(TimeSpan value)
        {
            DateTime date = ToDateTime();
            HijriDateTime pdate = new HijriDateTime(date.Subtract(value));
            return pdate;
        }

        //public long ToBinary(){}
        //public long ToFileTime(){}
        //public long ToFileTimeUtc(){}
        //public DateTime ToLocalTime(){}
        
        //public string ToString(IFormatProvider provider){}
        //public string ToString(string format){}
        //public string ToString(string format, IFormatProvider provider){}
        //public DateTime ToUniversalTime(){}
        
        public static bool TryParse(string s, out HijriDateTime result)
        {
            try
            {
                string[] d = s.Split('/');
                if (d.Length != 3)
                {
                    result = null;
                    return false;
                }
                int year = Convert.ToInt32(d[0]), month = Convert.ToInt32(d[1]), day = Convert.ToInt32(d[2]);
                HijriCalendar cc = new HijriCalendar();
                result = new HijriDateTime(cc.ToDateTime(year, month, day, 12, 0, 0, 0));
            }
            catch
            {
                result = null;
                return false;
            }
            return true;
        }
        
        //public static bool TryParse(string s, IFormatProvider provider, DateTimeStyles styles, out HijriDateTime result) { }
        //public static bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style, out HijriDateTime result) { }
        //public static bool TryParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, out HijriDateTime result) { }

    }
}
