using System;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.SessionState;

namespace XtremeK
{
	public class TimeZone : System.Web.HttpApplication
	{
		static public int timezone
		{
			get 
			{ 
				if (HttpContext.Current.Session["timezone"] == null) 
				{
					return -1;
				}
				else
				{
					return int.Parse(HttpContext.Current.Session["timezone"].ToString());
				}
			}
			set 
			{
				HttpContext.Current.Session["timezone"] = value.ToString();
			}
		}
		
		static public DateTime ToLocalTime(DateTime time)
		{
			return time.ToUniversalTime().AddMinutes(timezone);
		}
		
		static public DateTime ToLocalTime(string time)
		{
			return DateTime.Parse(time).ToUniversalTime().AddMinutes(timezone);
		}
		
		static public DateTime ToServerTime(string time)
		{
			return DateTime.Parse(time).AddMinutes(-timezone).ToLocalTime();
		}
		
		static public DateTime LocalTime()
		{			
			return DateTime.Now.ToUniversalTime().AddMinutes(timezone);
		}
	}
}

