using System;

namespace Cafe7.Infrastructure.DateTimeHelper
{
    public abstract class BaseDateTime
    {
        public override string ToString()
        {
            return Year + "/" + Month.ToString().PadLeft(2, '0') + "/" + Day.ToString().PadLeft(2, '0') +
                   " " + Hour.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0') + ":" + Second.ToString().PadLeft(2, '0') + "." + Millisecond.ToString().PadLeft(3, '0');
        }

        private int _day;
        /// <summary>
        /// Gets the day of the month represented by this instance.
        /// 
        /// Returns:
        ///      The day component, expressed as a value between 1 and 31.
        /// </summary>
        public int Day { get { return _day; } protected set { _day = value; } }

        private int _hour;
        /// <summary>
        /// 
        /// </summary>
        public int Hour { get { return _hour; } protected set { _hour = value; } }

        //public DateTimeKind Kind { get; protected set; }

        private int _millisecond;
        /// <summary>
        /// 
        /// </summary>
        public int Millisecond { get { return _millisecond; } protected set { _millisecond = value; } }

        private int _minute;
        /// <summary>
        /// 
        /// </summary>
        public int Minute { get { return _minute; } protected set { _minute = value; } }

        private int _month;
        /// <summary>
        /// 
        /// </summary>
        public int Month { get { return _month; } protected set { _month = value; } }


        private int _year;
        /// <summary>
        /// 
        /// </summary>
        public int Year { get { return _year; } protected set { _year = value; } }

        private int _second;
        /// <summary>
        /// 
        /// </summary>
        public int Second { get { return _second; } protected set { _second = value; } }

        public string ToLongDateString()
        {
            return Year + "/" + Month.ToString().PadLeft(2, '0') + "/" + Day.ToString().PadLeft(2, '0');
        }

        public string ToLongTimeString()
        {
            return Hour.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0') + ":" + Second.ToString().PadLeft(2, '0');
        }

        //public double ToOADate(){}

        public string ToShortDateString()
        {
            return Year.ToString().Substring(2, 2) + "/" + Month.ToString().PadLeft(2, '0') + "/" + Day.ToString().PadLeft(2, '0');
        }

        public string ToShortTimeString()
        {
            return Hour.ToString().PadLeft(2, '0') + ":" + Minute.ToString().PadLeft(2, '0');
        }

        public abstract DateTime ToDateTime();
        
        
    }
}
