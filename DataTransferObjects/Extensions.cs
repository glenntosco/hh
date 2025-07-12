using Microsoft.Azure.Amqp.Framing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pro4Soft.DataTransferObjects
{
    public static class Extensions
	{
		//https://docs.microsoft.com/en-us/dotnet/visual-basic/language-reference/operators/like-operator
		public static bool Like(this string toSearch, string toFind, char? split = ',')
        {
            if (split != null)
                return toFind.Split(split.Value)
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Select(c => c.Trim())
                    .Any(c => toSearch.Like(c, null));

            return new Regex($@"\A{new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\")
                    .Replace(toFind, c => $@"\{c}")
                    .Replace('_', '.')
                    .Replace("%", ".*")}\z", RegexOptions.Singleline) 
                .IsMatch(toSearch);
        }

		public static DateTimeOffset ToDateTimeOffset(this long epochMilliseconds)
		{
			return new DateTimeOffset(1970, 1, 1, 0, 0, 0, DateTimeOffset.Now.Offset).AddMilliseconds(epochMilliseconds);
		}

		public static double ToUnixSeconds(this DateTimeOffset date)
		{
			return (date - new DateTimeOffset(1970, 1, 1, 0, 0, 0, date.Offset)).TotalSeconds;
		}

		public static long ToUnixMilliseconds(this DateTimeOffset date)
		{
			return Convert.ToInt64((date - new DateTimeOffset(1970, 1, 1, 0, 0, 0, date.Offset)).TotalMilliseconds);
		}

        public static string Truncate(this string value, int maxLength)
        {
            return string.IsNullOrEmpty(value) ? value : value.Substring(0, Math.Min(value.Length, maxLength));
        }

        public static DateTimeOffset RoundDown(this DateTimeOffset dt, TimeSpan d)
		{
			return dt.AddTicks(-(dt.Ticks%d.Ticks));
		}

		public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
		{
			return dict.TryGetValue(key, out var value) ? value : defaultValue;
		}

        public static Guid? GetGuid(this IDictionary<string, object> dict, string key, bool throwError = true, Guid? defaultValue = null)
        {
            try
            {
                return dict.TryGetValue(key, out var guidObj)
                    ? Guid.TryParse(guidObj?.ToString(), out var g) ? g : throw new BusinessWebException($"Invalid Guid at [{key}]")
                    : throw new BusinessWebException($"Invalid Guid at [{key}]");
            }
            catch 
            {
                if (throwError)
                    throw;
                return defaultValue;
            }
        }

        public static int? GetInt(this IDictionary<string, object> dict, string key, bool throwError = true, int? defaultValue = null)
        {
            try
            {
                return dict.TryGetValue(key, out var intObj)
                    ? int.TryParse(intObj?.ToString(), out var g) ? g : throw new BusinessWebException($"Invalid int at [{key}]")
                    : throw new BusinessWebException($"Invalid Guid at [{key}]");
            }
            catch 
            {
                if (throwError)
                    throw;
                return defaultValue;
            }
        }

        public static string GetString(this IDictionary<string, object> dict, string key, bool throwError = false, string defaultValue = null)
        {
            try
            {
                return dict.TryGetValue(key, out var guidObj) ? guidObj?.ToString() : throw new BusinessWebException($"Value at [{key}] is missing");
            }
            catch
            {
                if (throwError)
                    throw;
                return defaultValue;
            }
        }

        public static T GetEnum<T>(this IDictionary<string, object> dict, string key, bool throwError = false, T defaultValue = default) where T:struct
        {
            try
            {
                var str = dict.GetString(key, throwError);
                if (string.IsNullOrWhiteSpace(str))
                    throw new BusinessWebException($"Invalid value at [{key}]");
                return Enum.TryParse(str, out T res)?res:!throwError?defaultValue: throw new BusinessWebException($"Invalid value at [{key}]");
            }
            catch
            {
                if (throwError)
                    throw;
                return defaultValue;
            }
        }

        public static bool EqualsTo(this string source, string dest)
        {
            return source.ToLower().Trim() == dest.ToLower().Trim();
        }

        public static void Append(this DataTable data, DataRowCollection rows)
        {
            foreach (DataRow row in rows)
                data.Rows.Add(row.ItemArray);
        }


        public static T ParseEnum<T>(this string value, bool throwExc = true, T defaultVal = default)
        {
            try
            {
                return (T)Enum.Parse(Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T), value, true);
            }
            catch
            {
                if (throwExc)
                    throw;
                return defaultVal;
            }
        }

        public static long ParseLong(this string value, bool throwExc = true, long defaultVal = default)
        {
            return long.TryParse(value, out var result) ? result : !throwExc ? defaultVal : throw new ArgumentException($"Invalid long [{value}]");
        }

        public static int ParseInt(this string value, bool throwExc = true, int defaultVal = default)
        {
            return int.TryParse(value, out var result) ? result : !throwExc ? defaultVal : throw new ArgumentException($"Invalid int [{value}]");
        }

        public static double ParseDouble(this string value, bool throwExc = true, double defaultVal = default)
        {
            return double.TryParse(value, out var result) ? result : !throwExc ? defaultVal : throw new ArgumentException($"Invalid double [{value}]");
        }

        public static decimal ParseDecimal(this string value, bool throwExc = true, decimal defaultVal = default)
        {
            return decimal.TryParse(value, out var result) ? result : !throwExc ? defaultVal : throw new ArgumentException($"Invalid decimal [{value}]");
        }

        public static Guid ParseGuid(this string value, bool throwExc = true, Guid defaultVal = default)
        {
            return Guid.TryParse(value, out var result) ? result : !throwExc ? defaultVal : throw new ArgumentException($"Invalid GUID [{value}]");
        }

        public static bool ParseBool(this string value, bool throwExc = true, bool defaultVal = default)
        {
            return bool.TryParse(value, out var result) ? result : !throwExc ? defaultVal : throw new ArgumentException($"Invalid bool [{value}]");
        }

        public static DateTime ParseDateTime(this string value, bool throwExc = true, DateTime defaultVal = default)
        {
            return DateTime.TryParse(value, out var result) ? result : !throwExc ? defaultVal : throw new ArgumentException($"Invalid DateTime [{value}]");
        }

        public static DateTimeOffset ParseDateTimeOffset(this string value, bool throwExc = true, DateTimeOffset defaultVal = default)
        {
            return DateTimeOffset.TryParse(value, out var result) ? result : !throwExc ? defaultVal : throw new ArgumentException($"Invalid DateTimeOffset [{value}]");
        }

        public static long? ParseLongNullable(this string value, bool throwExc = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value.ParseLong(throwExc);
        }

        public static int? ParseIntNullable(this string value, bool throwExc = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value.ParseInt(throwExc);
        }

        public static double? ParseDoubleNullable(this string value, bool throwExc = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value.ParseDouble(throwExc);
        }

        public static decimal? ParseDecimalNullable(this string value, bool throwExc = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value.ParseDecimal(throwExc);
        }

        public static Guid? ParseGuidNullable(this string value, bool throwExc = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value.ParseGuid(throwExc);
        }

        public static bool? ParseBoolNullable(this string value, bool throwExc = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value.ParseBool(throwExc);
        }

        public static DateTime? ParseDateTimeNullable(this string value, bool throwExc = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value.ParseDateTime(throwExc);
        }

        public static DateTimeOffset? ParseDateTimeOffsetNullable(this string value, bool throwExc = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value.ParseDateTimeOffsetNullable(throwExc);
        }


        public static void AddRecordSegment(this StringBuilder source, decimal? value, int min, int max)
		{
			source.AddRecordSegment(value?.ToString("0.##"), min, max);
		}

		public static void AddRecordSegment(this StringBuilder source, long? value, int min, int max)
		{
			source.AddRecordSegment(value?.ToString(), min, max);
		}

		public static void AddRecordSegment(this StringBuilder source, int? value, int min, int max)
		{
			source.AddRecordSegment(value?.ToString(), min, max);
		}

		public static void AddRecordSegment(this StringBuilder source, DateTimeOffset? value, int min, int max)
		{
			source.AddRecordSegment(value?.Date.ToString("yyyyMMdd"), min, max);
		}

		public static void AddRecordSegment(this StringBuilder source, string value, int min, int max)
		{
			if (value == null)
				value = string.Empty;
			if (value.Length > max)
				value = value.Substring(0, max);
			source.Append($"{value}^");
		}

		public static void EndRecord(this StringBuilder source)
		{
			source.Remove(source.Length - 1, 1);
			source.AppendLine();
		}

        public static int DigitsCount(this int n)
        {
            if (n >= 0)
            {
                if (n < 10) return 1;
                if (n < 100) return 2;
                if (n < 1000) return 3;
                if (n < 10000) return 4;
                if (n < 100000) return 5;
                if (n < 1000000) return 6;
                if (n < 10000000) return 7;
                if (n < 100000000) return 8;
                return n < 1000000000 ? 9 : 10;
            }

            if (n > -10) return 2;
            if (n > -100) return 3;
            if (n > -1000) return 4;
            if (n > -10000) return 5;
            if (n > -100000) return 6;
            if (n > -1000000) return 7;
            if (n > -10000000) return 8;
            if (n > -100000000) return 9;
            return n > -1000000000 ? 10 : 11;
        }

        public static T ToObject<T>(this IDictionary<string, object> source) where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
                someObjectType.GetProperty(item.Key)?.SetValue(someObject, item.Value, null);
            return someObject;
        }

        public static async Task ExecuteInParallel<T>(this IEnumerable<T> collection, Func<T, Task> processor, int degreeOfParallelism)
        {
            var queue = new ConcurrentQueue<T>(collection);
            var tasks = Enumerable.Range(0, degreeOfParallelism).Select(async _ =>
            {
                while (queue.TryDequeue(out var item))
                    await processor(item);
            });

            await Task.WhenAll(tasks);
        }

        public static string ReadableTimeSpan(this TimeSpan timeSpan, Func<string, string> translate = null)
        {
			var days = timeSpan.Days > 0 ? timeSpan.Days + (translate?.Invoke("d") ?? "d") : null;
			var hours = timeSpan.Hours > 0 ? timeSpan.Hours + (translate?.Invoke("hr") ?? "hr") : null;
            var mins = timeSpan.Minutes > 0 ? timeSpan.Minutes + (translate?.Invoke("m") ?? "m") : null;
            var secs = timeSpan.Seconds > 0 ? timeSpan.Seconds + (translate?.Invoke("s") ?? "s") : null;
            return $"{days} {hours} {mins} {secs}";
        }

        public static string ToUrlString(this DateTimeOffset? date)
        {
            if (date == null)
                return null;
            return date.Value.ToUrlString();
        }

        public static string ToUrlString(this DateTimeOffset date)
        {
            return date.Offset < TimeSpan.Zero ? 
                $"{date:yyyy-MM-ddTHH:mm:ss}-{date.Offset:hh\\:mm}" : 
                $"{date:yyyy-MM-ddTHH:mm:ss}+{date.Offset:hh\\:mm}";
        }

        public static string GetRandomNumber(this Random source, int length, int min = 0, int max = 999999)
        {
            return source.Next(min, max).ToString().PadLeft(length, '0');
        }
    }

	//[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	//public class XlsxColumnAttribute : Attribute
	//{
	//	public string ColumnName { get; set; }
	//}
}