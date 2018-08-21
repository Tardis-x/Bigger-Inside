//
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGContacts.cs
//

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;

	/// <summary>
	/// Class to pick contacts.
	/// </summary>
	public static class AGContacts
	{
		static Action<ContactPickResult> _onSuccessAction;
		static Action<string> _onCancelAction;

		/// <summary>
		/// Picks the contact from the phone contacts book.
		/// </summary>
		/// <param name="onSuccess">On success callback. Picked contact is passed as parameter</param>
		/// <param name="onError">On failure callback. Failure reason is passed as parameter</param>
		public static void PickContact(Action<ContactPickResult> onSuccess, Action<string> onError)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(onSuccess, "onSuccess");
			Check.Argument.IsNotNull(onError, "onError");
			_onSuccessAction = onSuccess;
			_onCancelAction = onError;

			AGActivityUtils.PickContact();
		}

		public static void OnSuccessTrigger(string message)
		{
			if (_onSuccessAction != null)
			{
				var contact = ContactPickResult.FromJson(message);
				_onSuccessAction(contact);
			}
		}

		public static void OnErrorTrigger(string message)
		{
			if (_onCancelAction != null)
			{
				_onCancelAction(message);
				_onCancelAction = null;
			}
		}
	}
}
#endif