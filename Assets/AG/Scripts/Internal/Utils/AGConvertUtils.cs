
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
		
		static int AndroidColor(int alpha,
			int red,
			int green,
			int blue)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0;
			}

			using (var c = new AndroidJavaClass("android.graphics.Color"))
			{
				return c.CallStaticInt("argb", alpha, red, green, blue);
			}
		}
		
		public static int ToAndroidColor(this Color32 color32)
		{
			Color color = color32;
			return ToAndroidColor(color);
		}
		
		public static int ToAndroidColor(this Color color)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0;
			}

			Color32 c = color;
			return AndroidColor(c.a, c.r, c.g, c.b);
		}
		
		public static Color ToUnityColor(this int aCol)
		{
			Color32 c = Color.clear;
			c.b = (byte) (aCol & 0xFF);
			c.g = (byte) ((aCol >> 8) & 0xFF);
			c.r = (byte) ((aCol >> 16) & 0xFF);
			c.a = (byte) ((aCol >> 24) & 0xFF);
			return c;
		}
	}
}

#endif