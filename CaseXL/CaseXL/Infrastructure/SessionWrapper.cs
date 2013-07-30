using CaseXL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseXL.Infrastructure
{
    public class SessionBase
    {
        public static int CaseId
        {
            get
            {
                return SessionWrapper.GetFromSession<int>("caseid");
            }
            set
            {
                SessionWrapper.SetInSession<int>("caseid", value);
            }
        }

        public static Firm Firm
        {
            get
            {
                return SessionWrapper.GetFromSession<Firm>("firm");
            }
            set
            {
                SessionWrapper.SetInSession<Firm>("firm", value);
            }
        }
        public static App_User User
        {
            get
            {
                return SessionWrapper.GetFromSession<App_User>("user");
            }
            set
            {
                SessionWrapper.SetInSession<App_User>("user", value);
            }
        }

    }
    public static class SessionWrapper
    {
        public static T GetFromSession<T>(string key)
        {
            object obj = HttpContext.Current.Session[key];
            if (obj == null)
            {
                return default(T);
            }
            return (T)obj;
        }

        public static void SetInSession<T>(string key, T value)
        {
            if (value == null)
            {
                HttpContext.Current.Session.Remove(key);
            }
            else
            {
                HttpContext.Current.Session[key] = value;
            }
        }

        public static T GetFromApplication<T>(string key)
        {
            return (T)HttpContext.Current.Application[key];
        }

        public static void SetInApplication<T>(string key, T value)
        {
            if (value == null)
            {
                HttpContext.Current.Application.Remove(key);
            }
            else
            {
                HttpContext.Current.Application[key] = value;
            }
        }

    }
}