
#if UNITY_IOS
namespace DeadMosquito.IosGoodies.Internal
{
	using System;
	using System.Runtime.InteropServices;
	using AOT;
	using UnityEngine;

	public static class IGUtils
	{
		public static bool IsIosCheck()
		{
			return Application.platform != RuntimePlatform.IPhonePlayer;
		}

		public static IntPtr GetPointer(this object obj)
		{
			return obj == null ? IntPtr.Zero : GCHandle.ToIntPtr(GCHandle.Alloc(obj));
		}

		public static T Cast<T>(this IntPtr instancePtr)
		{
			var instanceHandle = GCHandle.FromIntPtr(instancePtr);
			if (!(instanceHandle.Target is T))
			{
				throw new InvalidCastException("Failed to cast IntPtr");
			}

			var castedTarget = (T) instanceHandle.Target;
			return castedTarget;
		}

		[MonoPInvokeCallback(typeof(ActionVoidCallbackDelegate))]
		public static void ActionVoidCallback(IntPtr actionPtr)
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("ActionVoidCallback");
			}
			if (actionPtr != IntPtr.Zero)
			{
				var action = actionPtr.Cast<Action>();
				action();
			}
		}

		[MonoPInvokeCallback(typeof(ActionStringCallbackDelegate))]
		public static void ActionStringCallaback(IntPtr actionPtr, string data)
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("ActionStringCallaback: " + data);
			}
			if (actionPtr != IntPtr.Zero)
			{
				var action = actionPtr.Cast<Action<string>>();
				action(data);
			}
		}

		[MonoPInvokeCallback(typeof(ActionIntCallbackDelegate))]
		public static void ActionIntCallaback(IntPtr actionPtr, int data)
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("ActionStringCallaback: " + data);
			}
			if (actionPtr != IntPtr.Zero)
			{
				var action = actionPtr.Cast<Action<int>>();
				action(data);
			}
		}

		[DllImport("__Internal")]
		public static extern void _openUrl(string url);

		internal delegate void ActionVoidCallbackDelegate(IntPtr actionPtr);

		internal delegate void ActionStringCallbackDelegate(IntPtr actionPtr, string data);

		internal delegate void ActionIntCallbackDelegate(IntPtr actionPtr, int data);
		
		public static int ClampYScreenPos(Vector2 position)
		{
			return (int) Mathf.Clamp(position.y, 0, Screen.height);
		}

		public static int ClampXScreenPos(Vector2 position)
		{
			return (int) Mathf.Clamp(position.x, 0, Screen.width);
		}
	}
}
#endif