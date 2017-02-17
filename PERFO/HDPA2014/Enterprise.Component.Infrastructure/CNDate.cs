using System;
using System.Globalization;

namespace Enterprise.Component.Infrastructure
{
    /// <summary>
    /// 格式化时间
    /// </summary>
    public class CNDate
    {
        /// <summary>
        /// 获取当前日期的农历年  
        /// </summary>
        public int year { get; set; }

        /// <summary>
        /// 获取当前日期的农历月份
        /// </summary>
        public int month { get; set; }

        /// <summary>
        /// 获取当前日期的农历月中天数
        /// </summary>
        public int dayOfMonth { get; set; }

        /// <summary>
        /// 获取当前日期是否为闰月中的日期
        /// </summary>
        public bool isLeap { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime time;

        System.Globalization.ChineseLunisolarCalendar cc;

        ///<summary> 
        ///返回指定公历日期的阴历时间  
        ///</summary> 
        ///<paramnameparamname="time"></param> 
        public CNDate(DateTime time)
        {
            cc = new System.Globalization.ChineseLunisolarCalendar();
            if (time > cc.MaxSupportedDateTime || time < cc.MinSupportedDateTime)
                throw new Exception("参数日期时间不在支持的范围内,支持范围： " + cc.MinSupportedDateTime.ToShortDateString() + "到" + cc.MaxSupportedDateTime.ToShortDateString());
            year = cc.GetYear(time);
            month = cc.GetMonth(time);
            dayOfMonth = cc.GetDayOfMonth(time);
            isLeap = cc.IsLeapMonth(year, month);
            if (isLeap) month -= 1;
            this.time = time;
        }
    }
}
