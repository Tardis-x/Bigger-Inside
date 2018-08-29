
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using UnityEngine;

	[SuppressMessage("ReSharper", "InconsistentNaming")]
	class DialogOnCancelListenerPoxy : AndroidJavaProxy
	{
		readonly Action _onCancel;

		public DialogOnCancelListenerPoxy(Action onCancel)
			: base("android.content.DialogInterface$OnCancelListener")
		{
			_onCancel = onCancel;
		}

		[UsedImplicitly]
		void onCancel(AndroidJavaObject dialog)
		{
			GoodiesSceneHelper.Queue(_onCancel);
		}
	}
}
#endif