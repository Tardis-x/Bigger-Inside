#if UNITY_IOS
namespace DeadMosquito.IosGoodies.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	public static class StringUtils
	{
		const string Comma = ",";

		public static string JoinByComma(this string[] array)
		{
			return string.Join(Comma, array);
		}

		// http://answers.unity3d.com/questions/341147/encode-string-to-url-friendly-format.html
		public static string Escape(this string str)
		{
			return str == null ? string.Empty : WWW.EscapeURL(str).Replace("+", "%20");
		}

		public static string ToBase64(this string str)
		{
			if (str == null)
			{
				return string.Empty;
			}

			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(str);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string ToStringList<T>(this List<T> list)
		{
			if (list == null)
			{
				return "null";
			}

			if (list.Count == 0)
			{
				return "[]";
			}

			var listStr = list.Select(x => x.ToString()).ToList();
			return string.Join(",", listStr.ToArray());
		}
	}
}
#endif