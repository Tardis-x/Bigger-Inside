using UnityEngine;
using System.Collections;

namespace DeadMosquito.IosGoodies.Internal
{
	using System;
	using System.Collections.Generic;

	public static class JsonUtils
	{
		static T GetValue<T>(this Dictionary<string, object> dic, string key)
		{
			if (dic.ContainsKey(key))
			{
				return (T) dic[key];
			}

			return default(T);
		}

		public static string GetStr(this Dictionary<string, object> dic, string key)
		{
			return dic.GetValue<string>(key);
		}


		public static int GetInt(this Dictionary<string, object> dic, string key)
		{
			return (int) dic.GetLong(key);
		}

		static long GetLong(this Dictionary<string, object> dic, string key)
		{
			return dic.GetValue<long>(key);
		}

		public static List<object> GetArr(this Dictionary<string, object> dic, string key)
		{
			return dic.GetValue<List<object>>(key);
		}

		public static Dictionary<string, object> GetDict(this Dictionary<string, object> dic, string key)
		{
			return dic.GetValue<Dictionary<string, object>>(key);
		}

		public static DateTime? DeserializeDateTime(Dictionary<string, object> dic)
		{
			var year = dic.GetLong("year");
			var month = (int) dic.GetLong("month");
			var day = (int) dic.GetLong("day");

			if (year == 0 && month == 0 && day == 0)
			{
				return null;
			}

			var fixedYear = year.Equals(long.MaxValue) ? DateTime.Now.Year : (int) year;
			return new DateTime(fixedYear, month, day);
		}
	}
}