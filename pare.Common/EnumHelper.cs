using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;

namespace pare.Common
{
    public static class EnumHelper
    {
        public static IEnumerable<dynamic> GetStruct<TEnum>() where TEnum : struct
        {
            return (from v in GetPrivateDisplayList<TEnum>()
                    select new { Id = v, Description = v.ToString() });
        }

        public static IEnumerable<dynamic> GetDisplayList<TEnum>() where TEnum : struct
        {
            return (from v in GetPrivateDisplayList<TEnum>()
                    select new { Id = v, Description = v.GetDisplayString() });
        }

        public static string GetDisplayString(this Enum source)
        {
            var da = source.GetDisplayAttribute();
            return da != null ? da.Description : source.ToString();
        }

        private static IEnumerable<Enum> GetPrivateDisplayList<TEnum>() where TEnum : struct
        {
            var ti = typeof(TEnum).GetTypeInfo();
            if (!ti.IsEnum) return null;

            return ti.GetEnumValues().Cast<Enum>();
        }

        private static DisplayAttribute GetDisplayAttribute(this Enum source)
        {
            var fi = source.GetType().GetTypeInfo().GetField(source.ToString());
            return fi.GetCustomAttribute<DisplayAttribute>();
        }
    }
}
