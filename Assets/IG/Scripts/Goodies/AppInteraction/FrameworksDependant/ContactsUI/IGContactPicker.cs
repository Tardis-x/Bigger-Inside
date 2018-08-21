#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System;
	using System.Runtime.InteropServices;
	using AOT;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Class to interact with contacts
	/// </summary>
	[PublicAPI]
	public static class IGContactPicker
	{
		[MonoPInvokeCallback(typeof(IGUtils.ActionStringCallbackDelegate))]
		static void ContactPickerCallback(IntPtr actionPtr, string data)
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("ContactPickerCallback: " + data);
			}
			if (actionPtr != IntPtr.Zero)
			{
				var action = actionPtr.Cast<Action<Contact>>();
				var contact = Contact.FromJson(data);
				action(contact);
			}
		}

		[PublicAPI]
		public static void PickContact(Action<Contact> onSuccess, Action onCancel)
		{
			Check.Argument.IsNotNull(onSuccess, "onSuccess");
			Check.Argument.IsNotNull(onCancel, "onCancel");

			_showContactPicker(ContactPickerCallback, IGUtils.ActionStringCallaback, onSuccess.GetPointer(), onCancel.GetPointer());
		}

		[DllImport("__Internal")]
		static extern void _showContactPicker(IGUtils.ActionStringCallbackDelegate successCallback, IGUtils.ActionStringCallbackDelegate failureCallback, IntPtr successPtr, IntPtr cancelPtr);
	}
}
#endif