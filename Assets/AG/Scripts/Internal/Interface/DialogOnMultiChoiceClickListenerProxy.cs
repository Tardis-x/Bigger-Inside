
#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using UnityEngine;

	class DialogOnMultiChoiceClickListenerProxy : AndroidJavaProxy
	{
		readonly Action<int, bool> _onClick;

		public DialogOnMultiChoiceClickListenerProxy(Action<int, bool> onClick)
			: base("android.content.DialogInterface$OnMultiChoiceClickListener")
		{
			_onClick = onClick;
		}

		void onClick(AndroidJavaObject dialog, int which, bool isChecked)
		{
			GoodiesSceneHelper.Queue(() => _onClick(which, isChecked));
		}
	}
}
#endif