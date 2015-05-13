using SozialHeap.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SozialHeap.Utils
{
    /// <summary>
    /// Utils class is extra-features class for time calculation, getting search autocomplete list, and log view activity for statistics.
    /// functions are static so we don't need an instance of the class.
    /// </summary>
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

        /// <summary>
        /// Search data for given string
        /// </summary>
        /// <param name="query">query string</param>
        /// <returns>List of strings which start with your input</returns>
        public static List<string> getKeywords(string query)
        {
            if (query.Contains(" ") || query.Contains("-"))
            {
                // breaks if you have space or dash in the search string to prenvent bad input
                // the space is because we only have words, then we can save the db-request
                return new List<string>();
            }

            SqlConnection cnn;
            cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            List<string> res = new List<string>();
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM keywords WHERE word LIKE @query", cnn);
                command.Parameters.Add(new SqlParameter("query", query+"%"));

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

        /// <summary>
        /// Logs action to the Syslog table for later inspection or statistics
        /// </summary>
        /// <param name="userID">userID if user is logged in</param>
        /// <param name="ipAddr">ip of user</param>
        /// <param name="action">what is he visiting</param>
        public static void LogAction(string userID, string ipAddr, string action)
        {
            SqlConnection conn;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            try
            {
                conn.Open();
                // input the query and bind the parameters before execute...
                using (SqlCommand command = new SqlCommand("INSERT INTO Syslog(userName, viewed, ipAddress) VALUES(@User, @Action, @Ip)", conn))
                {
                    command.Parameters.Add(new SqlParameter("User", userID));
                    command.Parameters.Add(new SqlParameter("Ip", ipAddr));
                    command.Parameters.Add(new SqlParameter("Action", action));
                    command.ExecuteNonQuery();
                }
                
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}