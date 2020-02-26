using System;
using System.Globalization;

namespace Cafe7.Infrastructure.DateTimeHelper
{
    //public enum PersianDayOfWeek
    //{

    //}
    public class PersianDateTime : BaseDateTime //: IComparable, IFormattable, IConvertible, ISerializable, IComparable<PersianDateTime>, IEquatable<PersianDateTime>
    {
        /// <summary>
        /// Represents the largest possible value of Rayatis.PersianDateTime. This field is read-only.
        /// </summary>
        public static readonly PersianDateTime MaxValue = new PersianDateTime(9999, 12, 29, 23, 59, 59, 999);

        /// <summary>
        /// Represents the smallest possible value of Rayatis.DateTimeLibrary. This field is read-only.
        /// </summary>
        public static readonly PersianDateTime MinValue = new PersianDateTime(1, 1, 1, 12, 0, 0, 0);

        /// <summary>
        /// Gets the date component of this instance.
        /// 
        /// Returns:
        ///     A new Rayatis.DateTimeLibrary with the same date as this instance, and the time value
        ///     set to 12:00:00 midnight (00:00:00).
        /// </summary>
        public PersianDateTime Date
        {
            get
            {
                return new PersianDateTime(Year, Month, Day, 0, 0, 0, 0);
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
                int sum = 0;
                for (int i = 1; i < Year; i++)
                {
                    sum += 365;
                    if (IsLeapYear(i)) sum++;
                }
                sum += DayOfYear;
                sum %= 7;
                return (sum >= 3 ? sum - 2 : sum + 5);
            }
        }

        public string DayName
        {
            get
            {
                switch (DayOfWeek)
                {
                    case 1:
                        return "شنبه";
                    case 2:
                        return "يكشنبه";
                    case 3:
                        return "دوشنبه";
                    case 4:
                        return "سه شنبه";
                    case 5:
                        return "چهارشنبه";
                    case 6:
                        return "پنجشنبه";
                    default:
                        return "جمعه";
                }
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
                int sum = 0;
                for (int i = 1; i < Month; i++)
                {
                    sum += PersianDateTime.DaysInMonth(Year, i);
                }
                return sum + Day;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static PersianDateTime Now
        {
            get
            {
                return new PersianDateTime(DateTime.Now);
            }
        }



        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>
        public static PersianDateTime Today
        {
            get
            {
                return new PersianDateTime(DateTime.Today);
            }
        }

        //public static PersianDateTime UtcNow { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        //private string GetDayOfWeek(DayOfWeek dayOfWeek)
        //{
        //    string dow = "";
        //    switch (dayOfWeek)
        //    {
        //        case System.DayOfWeek.Saturday:
        //            dow = "شنبه";
        //            break;
        //        case System.DayOfWeek.Sunday:
        //            dow = "يكشنبه";
        //            break;
        //        case System.DayOfWeek.Monday:
        //            dow = "دوشنبه";
        //            break;
        //        case System.DayOfWeek.Tuesday:
        //            dow = "سه شنبه";
        //            break;
        //        case System.DayOfWeek.Wednesday:
        //            dow = "چهارشنبه";
        //            break;
        //        case System.DayOfWeek.Thursday:
        //            dow = "پنجشنبه";
        //            break;
        //        case System.DayOfWeek.Friday:
        //            dow = "جمعه";
        //            break;
        //    }
        //    return dow;
        //}

        public static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "فروردين";
                case 2:
                    return "ارديبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تير";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهريور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دي";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PersianDateTime()
            : this(1, 1, 1, 12, 0, 0)
        {
            //Year = 1;
            //Month = 1;
            //Day = 1;
            //Hour = 12;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticks"></param>
        public PersianDateTime(long ticks)
        {
            if (ticks < 0 || ticks > 3155378941640251632)
                throw new Exception("Ticks value must be between 0 and 3155378941640251632.");
            //DateTime d = new DateTime(ticks);
            //PersianCalendar cc = new PersianCalendar();
            Year = 1;
            Month = 1;
            Day = 1;

            while (ticks >= 315360000000000)
            {
                ticks -= 315360000000000;
                if (PersianDateTime.IsLeapYear(Year) && ticks >= 864000000000)
                    ticks -= 864000000000;
                Year++;
            }

            long tmp = PersianDateTime.DaysInMonth(Year, Month) * 864000000000;
            while (ticks >= tmp)
            {
                ticks -= tmp;
                tmp = PersianDateTime.DaysInMonth(Year, Month) * 864000000000;
                Month++;
            }
            //Year = cc.GetYear(d);
            //Month = cc.GetMonth(d);

            Day += (int)(ticks / 864000000000);
            ticks %= 864000000000;
            Hour += (int)(ticks / 36000000000);
            ticks %= 36000000000;
            Minute += (int)(ticks / 600000000);
            ticks %= 600000000;
            Second += (int)(ticks / 10000000);
            ticks %= 10000000;
            Millisecond += (int)(ticks / 10000);
        }

        //public PersianDateTime(long ticks,DateTimeKind kind)
        //{

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public PersianDateTime(int year, int month, int day)
            : this(year, month, day, 12, 0, 0, 0)
        {

        }

        //public PersianDateTime(int year, int month, int day, Calendar calendar)
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
        public PersianDateTime(int year, int month, int day, int hour, int minute, int second)
            : this(year, month, day, hour, minute, second, 0)
        {

        }

        //public PersianDateTime(int year, int month, int day, int hour, int minute, int second, Calendar calendar)
        //{

        //}
        //public PersianDateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
        //{

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        public PersianDateTime(DateTime date)
        {
            PersianCalendar cc = new PersianCalendar();
            if (date >= DateTime.Parse("0622/3/21"))
            {
                Year = cc.GetYear(date);
                Month = cc.GetMonth(date);
                Day = cc.GetDayOfMonth(date);
                Hour = cc.GetHour(date);
                Minute = cc.GetMinute(date);
                Second = cc.GetSecond(date);
                Millisecond = (int)cc.GetMilliseconds(date);
            }
            else
            {
                Year = 1;
                Month = 1;
                Day = 1;
                Hour = 0;
                Minute = 0;
                Second = 0;
                Millisecond = 0;

            }
        }

        public static PersianDateTime FromDateTime(DateTime date)
        {
            return new PersianDateTime(date);
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
        public PersianDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            Millisecond = millisecond;
        }

        //public PersianDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar)
        //{

        //}
        //public PersianDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
        //{

        //}
        //public PersianDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, DateTimeKind kind)
        //{

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static TimeSpan operator -(PersianDateTime d1, PersianDateTime d2)
        {
            TimeSpan ts = new TimeSpan(d1.Ticks - d2.Ticks);
            return ts;
            //DateTime date1 = PersianDateTime.ToDateTime(d1);
            //DateTime date2 = PersianDateTime.ToDateTime(d2);
            //return date1 - date2;
        }

        //public static PersianDateTime operator -(PersianDateTime d, TimeSpan t)
        //{
        //    DateTime date = PersianDateTime.ToDateTime(d);


        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool operator !=(PersianDateTime d1, PersianDateTime d2)
        {
            return (d1.ToString() != d2.ToString());

            //return (d1.Year != d2.Year || d1.Month != d2.Month || d1.Day != d2.Day || 
            //    d1.Hour != d2.Hour || d1.Minute != d2.Minute || d1.Second != d2.Second || d1.Millisecond != d2.Millisecond);
            //DateTime date1 = PersianDateTime.ToDateTime(d1);
            //DateTime date2 = PersianDateTime.ToDateTime(d2);
            //return date1 != date2;
        }

        //public static DateTime operator +(PersianDateTime d, TimeSpan t)
        //{
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <(PersianDateTime t1, PersianDateTime t2)
        {
            DateTime date1 = PersianDateTime.ToDateTime(t1);
            DateTime date2 = PersianDateTime.ToDateTime(t2);
            return date1 < date2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <=(PersianDateTime t1, PersianDateTime t2)
        {
            DateTime date1 = PersianDateTime.ToDateTime(t1);
            DateTime date2 = PersianDateTime.ToDateTime(t2);
            return date1 <= date2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool operator ==(PersianDateTime d1, PersianDateTime d2)
        {
            return (d1.ToString() == d2.ToString());
            //        return (d1.Year != d2.Year && d1.Month != d2.Month && d1.Day != d2.Day &&
            //d1.Hour != d2.Hour && d1.Minute != d2.Minute && d1.Second != d2.Second && d1.Millisecond != d2.Millisecond);

            //DateTime date1 = PersianDateTime.ToDateTime(d1);
            //DateTime date2 = PersianDateTime.ToDateTime(d2);
            //return date1 == date2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >(PersianDateTime t1, PersianDateTime t2)
        {
            DateTime date1 = PersianDateTime.ToDateTime(t1);
            DateTime date2 = PersianDateTime.ToDateTime(t2);
            return date1 > date2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >=(PersianDateTime t1, PersianDateTime t2)
        {
            DateTime date1 = PersianDateTime.ToDateTime(t1);
            DateTime date2 = PersianDateTime.ToDateTime(t2);
            return date1 >= date2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(PersianDateTime date)
        {
            PersianCalendar cc = new PersianCalendar();
            return cc.ToDateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        public static DateTime ToDateTime(string date)
        {
            return PersianDateTime.ToDateTime(PersianDateTime.Parse(date));
        }

        public long Ticks
        {
            get
            {
                long sum = 0;
                for (int i = 1; i < Year; i++)
                {
                    sum += 365;
                    if (PersianDateTime.IsLeapYear(i)) sum++;
                }
                sum += DayOfYear - 1;
                sum *= 864000000000;
                sum += Hour * 36000000000;
                sum += Minute * 600000000L;
                sum += Second * 10000000L;
                sum += Millisecond * 10000L;
                return sum;
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
        /// <returns></returns>
        public override DateTime ToDateTime()
        {
            return PersianDateTime.ToDateTime(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PersianDateTime Add(TimeSpan value)
        {
            DateTime date = PersianDateTime.ToDateTime(this);
            PersianDateTime pdate = new PersianDateTime(date.Add(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PersianDateTime AddDays(double value)
        {
            DateTime date = ToDateTime(this);
            return new PersianDateTime(date.AddDays(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PersianDateTime AddHours(double value)
        {
            DateTime date = PersianDateTime.ToDateTime(this);
            PersianDateTime pdate = new PersianDateTime(date.AddHours(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PersianDateTime AddMilliseconds(double value)
        {
            DateTime date = PersianDateTime.ToDateTime(this);
            PersianDateTime pdate = new PersianDateTime(date.AddMilliseconds(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PersianDateTime AddMinutes(double value)
        {
            DateTime date = PersianDateTime.ToDateTime(this);
            PersianDateTime pdate = new PersianDateTime(date.AddMinutes(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="months"></param>
        /// <returns></returns>
        public PersianDateTime AddMonths(int months)
        {
            DateTime date = PersianDateTime.ToDateTime(this);
            PersianDateTime pdate = new PersianDateTime(date.AddMonths(months));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PersianDateTime AddSeconds(double value)
        {
            DateTime date = PersianDateTime.ToDateTime(this);
            PersianDateTime pdate = new PersianDateTime(date.AddSeconds(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PersianDateTime AddTicks(long value)
        {
            DateTime date = PersianDateTime.ToDateTime(this);
            PersianDateTime pdate = new PersianDateTime(date.AddTicks(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PersianDateTime AddYears(int value)
        {
            DateTime date = PersianDateTime.ToDateTime(this);
            PersianDateTime pdate = new PersianDateTime(date.AddYears(value));
            return pdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static int Compare(PersianDateTime t1, PersianDateTime t2)
        {
            DateTime date1 = PersianDateTime.ToDateTime(t1);
            DateTime date2 = PersianDateTime.ToDateTime(t2);
            return DateTime.Compare(date1, date2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int CompareTo(PersianDateTime value)
        {
            DateTime date1 = PersianDateTime.ToDateTime(value);
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
            return date2.CompareTo(PersianDateTime.ToDateTime(value.ToString()));
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
                PersianCalendar cc = new PersianCalendar();
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
        public bool Equals(PersianDateTime value)
        {
            DateTime date1 = PersianDateTime.ToDateTime(value);
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

        public static bool Equals(PersianDateTime t1, PersianDateTime t2)
        {
            DateTime date1 = PersianDateTime.ToDateTime(t1);
            DateTime date2 = PersianDateTime.ToDateTime(t2);
            return DateTime.Equals(date1, date2);
        }

        public static PersianDateTime FromBinary(long dateData)
        {
            PersianDateTime pdate = new PersianDateTime(DateTime.FromBinary(dateData));
            return pdate;
        }

        public static PersianDateTime FromFileTime(long fileTime)
        {
            PersianDateTime pdate = new PersianDateTime(DateTime.FromFileTime(fileTime));
            return pdate;
        }

        public static PersianDateTime FromFileTimeUtc(long fileTime)
        {
            PersianDateTime pdate = new PersianDateTime(DateTime.FromFileTimeUtc(fileTime));
            return pdate;
        }

        public static PersianDateTime FromOADate(double d)
        {
            PersianDateTime pdate = new PersianDateTime(DateTime.FromOADate(d));
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
            switch (year % 33)
            {
                case 1:
                case 5:
                case 9:
                case 13:
                case 17:
                case 22:
                case 26:
                case 30:
                    return true;
            }
            return false;
        }

        public static PersianDateTime Parse(string s)
        {
            PersianDateTime pdate = null;
            try
            {
                string[] d = s.Split('/');
                if (d.Length != 3)
                    throw new FormatException("String was not recognized as a valid PersianDateTime.");
                int year = Convert.ToInt32(d[0]), month = Convert.ToInt32(d[1]), day = Convert.ToInt32(d[2].Split(' ')[0]);
                PersianCalendar cc = new PersianCalendar();
                pdate = new PersianDateTime(cc.ToDateTime(year, month, day, 12, 0, 0, 0));
            }
            catch
            {
                throw new Exception("تاريخ نا معتبر مي باشد.");
            }
            return pdate;
        }

        //public static PersianDateTime Parse(string s, IFormatProvider provider)
        //{
        //    PersianDateTime pdate = new PersianDateTime(DateTime.Parse(s, provider));
        //    return pdate;
        //}
        //public static PersianDateTime Parse(string s, IFormatProvider provider, DateTimeStyles styles)
        //{
        //    PersianDateTime pdate = new PersianDateTime(DateTime.Parse(s, provider, styles));
        //    return pdate;
        //}
        //public static PersianDateTime ParseExact(string s, string format, IFormatProvider provider)
        //{
        //    PersianDateTime pdate = new PersianDateTime(DateTime.Parse(s, format, provider));
        //    return pdate;
        //}
        //public static PersianDateTime ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
        //{
        //    PersianDateTime pdate = new PersianDateTime(DateTime.Parse(s, format, provider, style));
        //    return pdate;
        //}
        //public static PersianDateTime ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
        //{
        //    PersianDateTime pdate = new PersianDateTime(DateTime.Parse(s,formats, provider, style));
        //    return pdate;
        //}
        //public static PersianDateTime SpecifyKind(DateTime value, DateTimeKind kind) { }

        public TimeSpan Subtract(PersianDateTime value)
        {
            DateTime date1 = PersianDateTime.ToDateTime(value);
            DateTime date2 = ToDateTime();
            return date2.Subtract(date1);
        }

        public PersianDateTime Subtract(TimeSpan value)
        {
            DateTime date = ToDateTime();
            PersianDateTime pdate = new PersianDateTime(date.Subtract(value));
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

        public static bool TryParse(string s, out PersianDateTime result)
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
                PersianCalendar cc = new PersianCalendar();
                result = new PersianDateTime(cc.ToDateTime(year, month, day, 12, 0, 0, 0));
            }
            catch
            {
                result = null;
                return false;
            }
            return true;
        }

        


    }
}
