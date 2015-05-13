using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
            StringBuilder sb = new StringBuilder();
                
            int days = (int)diff.TotalDays;
            if(days > 0)
            {
                if(days > 30)
                {
                    int months = days / 30;
                    sb.Append(months.ToString());
                    if(months > 1)
                    {
                        sb.Append(" months ago");
                        return sb.ToString();
                    }
                    else
                    {
                        sb.Append(" month ago");
                        return sb.ToString();
                    }
                }
                sb.Append(days.ToString());
                if(days > 1)
                {
                    sb.Append(" days ago");
                }
                else
                {
                    sb.Append(" day ago");
                }
                return sb.ToString();
            }
            else
            {
                
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
                if((minutes % 60) > 0)
                {
                    if(hours > 0)
                    {
                        sb.Append(" and ");
                    }
                    sb.Append((minutes % 60).ToString());
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

        public static List<string> getKeywords(string query)
        {
            if (query.Contains(' ') || query.Contains('-'))
            {
                // breaks if you have space or dash in the search string to prenvent bad input
                return new List<string>();
            }
            SqlConnection cnn;
            cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            string sql = "SELECT * FROM keywords WHERE word LIKE '" + query + "%'";
            List<string> res = new List<string>();
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(sql, cnn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    res.Add(dataReader.GetValue(0).ToString());
                }
                dataReader.Close();
                command.Dispose();
                cnn.Close();
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return new List<string>();
        }

    }
}