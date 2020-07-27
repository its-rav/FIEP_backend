using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BusinessTier.DTO
{
    class UserStatisticDTO
    {
        public Guid UserId { get; set; }

        public DateTime? CreateDate { get; set; }

        public static PropertyInfo[] GetAllProperties()
        {
            PropertyInfo[] properties = typeof(UserStatisticDTO).GetProperties();
            return properties;
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
