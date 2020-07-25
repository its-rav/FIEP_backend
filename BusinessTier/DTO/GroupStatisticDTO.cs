using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BusinessTier.DTO
{
    public class GroupStatisticDTO
    {

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public int Followers { get; set; }

        public int EventsCount { get; set; }
        public int ActiveEventsCount { get; set; }

        public static PropertyInfo[] GetAllProperties()
        {
            PropertyInfo[] properties = typeof(GroupStatisticDTO).GetProperties();
            return properties;
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
