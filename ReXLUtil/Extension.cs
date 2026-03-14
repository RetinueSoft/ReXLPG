using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReXL.Util
{
    public static class Extension
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
            throw new ArgumentException("Item not found.", nameof(enumValue));
        }

        public static T ConvertToDataModel<T>(this Type viewModel, T target)
        {
            foreach (var item in target.GetType().GetProperties())
            {
                item.SetValue(target, viewModel.GetType().GetProperty(item.Name).GetValue(viewModel));
            }
            return target;
        }
        public static T ConvertToObject<T>(this object source, T target = default) where T : new()
        {
            if (source == null)
                return target;

            if (target == null)
                target = new T();

            JsonConvert.PopulateObject(JsonConvert.SerializeObject(source), target);
            return target;
        }
        public static string FromDateSqliteString(this DateTime date)
        {
            return "Date('" + date.ToString("yyyy-MM-dd 00:00:00.000") + "')";
        }
        public static string ToDateSqliteString(this DateTime date)
        {
            return "Date('" + date.ToString("yyyy-MM-dd 23:59:59.999") + "')";
        }
        public static DateTime ToDate(this DateTime date)
        {
            return date.AddDays(1).AddMinutes(-1);
        }
        public static int DaysInMonth(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }
        public static bool IsNullOrEmpty(this Guid? guid) => !guid.HasValue || guid == new Guid();
        public static bool IsEmpty(this Guid guid) => guid == new Guid();
        public static bool AddIfNotExist<T>(this List<T> list, T element)
        {
            if (!list.Contains(element))
            {
                list.Add(element);
                return true;
            }
            return false;
        }
    }
    public static class TupleExtensions
    {
        public static Tuple<string, string, string> Create(string error, string prop, string propValue)
        {
            if (prop.Trim() == "" || propValue.Trim() == "")
                return new Tuple<string, string, string>(error, "", "");
            else
                return new Tuple<string, string, string>(error, prop, propValue);
        }
    }
}
