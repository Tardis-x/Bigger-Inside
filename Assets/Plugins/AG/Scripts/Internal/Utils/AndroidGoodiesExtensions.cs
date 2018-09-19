
#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using JetBrains.Annotations;
	using UnityEngine;

	static class AndroidGoodiesExtensions
	{
		[PublicAPI]
		public static string GetAbsolutePath(this AndroidJavaObject ajo)
		{
			if (ajo.IsJavaNull())
			{
				return null;
			}
			
			if (ajo.GetClassName() != C.JavaIoFile)
			{
				Debug.LogError("Trying to get absolute path but object class is not " + C.JavaIoFile);
				return string.Empty;
			}

			return ajo.CallStr("getAbsolutePath");
		}
		
		[PublicAPI]
		public static string GetClassName(this AndroidJavaObject ajo)
		{
			return ajo.GetJavaClass().Call<string>("getName");
		}

		[PublicAPI]
		public static string GetClassSimpleName(this AndroidJavaObject ajo)
		{
			return ajo.GetJavaClass().Call<string>("getSimpleName");
		}

		[PublicAPI]
		public static AndroidJavaObject GetJavaClass(this AndroidJavaObject ajo)
		{
			return ajo.CallAJO("getClass");
		}

		[PublicAPI]
		public static string JavaToString(this AndroidJavaObject ajo)
		{
			return ajo.Call<string>("toString");
		}

		#region AndroidJavaObject_Call_Proxy

		[PublicAPI]
		public static string CallStr(this AndroidJavaObject ajo, string methodName, params object[] args)
		{
			return ajo.Call<string>(methodName, args);
		}

		[PublicAPI]
		public static bool CallBool(this AndroidJavaObject ajo, string methodName, params object[] args)
		{
			return ajo.Call<bool>(methodName, args);
		}

		[PublicAPI]
		public static float CallFloat(this AndroidJavaObject ajo, string methodName, params object[] args)
		{
			return ajo.Call<float>(methodName, args);
		}

		[PublicAPI]
		public static int CallInt(this AndroidJavaObject ajo, string methodName, params object[] args)
		{
			return ajo.Call<int>(methodName, args);
		}

		[PublicAPI]
		public static long CallLong(this AndroidJavaObject ajo, string methodName, params object[] args)
		{
			return ajo.Call<long>(methodName, args);
		}

		[PublicAPI]
		public static string CallStaticStr(this AndroidJavaObject ajo, string methodName, params object[] args)
		{
			return ajo.CallStatic<string>(methodName, args);
		}

		[PublicAPI]
		public static AndroidJavaObject CallAJO(this AndroidJavaObject ajo, string methodName, params object[] args)
		{
			return ajo.Call<AndroidJavaObject>(methodName, args);
		}
		
		[PublicAPI]
		public static AndroidJavaObject CallAJOSafe(this AndroidJavaObject ajo, string methodName, params object[] args)
		{
			try
			{
				return ajo.CallAJO(methodName, args);
			}
			catch (Exception e)
			{
				Debug.Log(e);
				return null;
			}
		}
		
		[PublicAPI]
		public static AndroidJavaObject CallStaticAJO(this AndroidJavaObject ajo, string methodName, params object[] args)
		{
			return ajo.CallStatic<AndroidJavaObject>(methodName, args);
		}

		#endregion

		#region AndroidJavaClass_Call_Proxy

		// GET STATIC
		[PublicAPI]
		public static string GetStaticStr(this AndroidJavaClass ajc, string fieldName)
		{
			return ajc.GetStatic<string>(fieldName);
		}

		[PublicAPI]
		public static bool GetStaticBool(this AndroidJavaClass ajc, string fieldName)
		{
			return ajc.GetStatic<bool>(fieldName);
		}

		[PublicAPI]
		public static int GetStaticInt(this AndroidJavaClass ajc, string fieldName)
		{
			return ajc.GetStatic<int>(fieldName);
		}

		// CALL STATIC
		[PublicAPI]
		public static string CallStaticStr(this AndroidJavaClass ajc, string methodName, params object[] args)
		{
			return ajc.CallStatic<string>(methodName, args);
		}

		[PublicAPI]
		public static int CallStaticInt(this AndroidJavaClass ajc, string methodName, params object[] args)
		{
			return ajc.CallStatic<int>(methodName, args);
		}

		[PublicAPI]
		public static long CallStaticLong(this AndroidJavaClass ajc, string methodName, params object[] args)
		{
			return ajc.CallStatic<long>(methodName, args);
		}

		[PublicAPI]
		public static bool CallStaticBool(this AndroidJavaClass ajc, string methodName, params object[] args)
		{
			return ajc.CallStatic<bool>(methodName, args);
		}

		[PublicAPI]
		public static AndroidJavaObject CallStaticAJO(this AndroidJavaClass ajc, string methodName, params object[] args)
		{
			return ajc.CallStatic<AndroidJavaObject>(methodName, args);
		}

		[PublicAPI]
		public static T AJCCallStaticOnce<T>(this string className, string methodName, params object[] args)
		{
			using (var ajc = new AndroidJavaClass(className))
			{
				return ajc.CallStatic<T>(methodName, args);
			}
		}

		[PublicAPI]
		public static AndroidJavaObject AJCCallStaticOnceAJO(this string className, string methodName, params object[] args)
		{
			return className.AJCCallStaticOnce<AndroidJavaObject>(methodName, args);
		}

		#endregion
	}
}
#endif