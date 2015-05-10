using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SozialHeap.Utils
{
    public class Utils
    {
        /// <summary>
        /// Calculates the time since given time in days or Hours/minutes
        /// </summary>
        /// <param name="time">timestamp to calculate until now</param>
        /// <returns>days string or hours/minutes string</returns>
        public static string TimeSince(DateTime? time)
        {
            if(time == null)
            {
                return "";
            }
            DateTime currTime = DateTime.Now;
            DateTime time1 = (DateTime)time;
            TimeSpan diff = currTime - time1;

            if(diff.TotalDays > 0.9999999)
            {
                if(diff.TotalDays > 31)
                {

                }
                return Math.Round(diff.TotalDays, 0, MidpointRounding.ToEven).ToString() + " days ago";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                
                int hours = (int)Math.Round(diff.TotalHours, 0, MidpointRounding.ToEven);
                int minutes = (int)Math.Round(diff.TotalMinutes, 0, MidpointRounding.ToEven);
                if(hours > 0)
                {
                    sb.Append(hours.ToString());
                    if(hours > 1)
                    {
                        sb.Append(" hours");
                    }
                    else
                    {
                        sb.Append(" hour");
                    }
                    
                }
                if(minutes > 0)
                {
                    if(hours > 0)
                    {
                        sb.Append(" and ");
                    }
                    sb.Append(minutes.ToString());
                    if(minutes > 1)
                    {
                        sb.Append(" minutes ago");
                    }
                    else
                    {
                        sb.Append(" minute ago");
                    }
                    
                }
                else if(hours > 0)
                {
                    sb.Append(" ago");
                }
                if(hours == 0 && minutes == 0)
                { 
                    sb.Append("just now");
                }
                return sb.ToString();
            }
        }
    }
}