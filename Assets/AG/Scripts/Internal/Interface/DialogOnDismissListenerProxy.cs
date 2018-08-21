
#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using UnityEngine;

	class DialogOnDismissListenerProxy : AndroidJavaProxy
	{
		readonly Action _onDismiss;

		public DialogOnDismissListenerProxy(Action onDismiss)
			: base("android.content.DialogInterface$OnDismissListener")
		{
			_onDismiss = onDismiss;
		}

		void onDismiss(AndroidJavaObject dialog)
		{
			GoodiesSceneHelper.Queue(_onDismiss);
		}
	}
}
#endif