using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BusinessTier.DTO
{
    public class EventStatisticDTO
    {

        public int EventID { get; set; }

        public string EventName { get; set; }

        public int Followers { get; set; }

        public int PostCount { get; set; }

        public static PropertyInfo[] GetAllProperties()
        {
            PropertyInfo[] properties =typeof(EventStatisticDTO).GetProperties();
            return properties;
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
