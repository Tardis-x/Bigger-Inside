
#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System.Collections.Generic;
	using UnityEngine;

	public static class AGConvertUtils
	{
		public static Dictionary<string, object> FromJavaMap(this AndroidJavaObject javaMap)
		{
			if (javaMap == null || javaMap.IsJavaNull())
			{
				return new Dictionary<string, object>();
			}

			int size = javaMap.CallInt("size");
			var dictionary = new Dictionary<string, object>(size);

			var iterator = javaMap.CallAJO("keySet").CallAJO("iterator");
			while (iterator.CallBool("hasNext"))
			{
				string key = iterator.CallStr("next");
				var value = javaMap.CallAJO("get", key);
				dictionary.Add(key, ParseJavaBoxedValue(value));
			}

			javaMap.Dispose();
			return dictionary;
		}

		public static List<T> FromJavaList<T>(this AndroidJavaObject javaList)
		{
			if (javaList == null || javaList.IsJavaNull())
			{
				return new List<T>();
			}

			int size = javaList.CallInt("size");
			var list = new List<T>(size);

			for (int i = 0; i < size; i++)
			{
				list.Add(javaList.Call<T>("get", i));
			}

			javaList.Dispose();
			return list;
		}

		public static object ParseJavaBoxedValue(AndroidJavaObject boxedValueAjo)
		{
			if (boxedValueAjo == null || boxedValueAjo.IsJavaNull())
			{
				return null;
			}

			var className = boxedValueAjo.GetClassSimpleName();
			switch (className)
			{
				case "Boolean":
					return boxedValueAjo.CallBool("booleanValue");
				case "Float":
					return boxedValueAjo.CallFloat("floatValue");
				case "Integer":
					return boxedValueAjo.CallInt("intValue");
				case "Long":
					return boxedValueAjo.CallLong("longValue");
				case "String":
					return boxedValueAjo.CallStr("toString");
			}
			return boxedValueAjo;
		}

		public static int ToAndroidColor(this Color32 color32)
		{
			return color32.r * 65536 + color32.g * 256 + color32.b;
		}
	}
}

#endif