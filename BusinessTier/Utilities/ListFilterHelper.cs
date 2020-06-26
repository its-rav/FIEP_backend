using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessTier.Utilities
{
    public class ListFilterHelper<T> where T:class
    {
        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public List<T> FilterByQuery(List<T> inputList,String query,String propName)
        {
            List<T> result=new List<T>();

            //result = inputList.Where(x => GetPropValue(x,propName).Contains(query)).ToList();

            return result;

        }
    }
}
