#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;

	/// <summary>
	/// Class to interact with clipboard
	/// </summary>
	public static class AGClipboard
	{
		[PublicAPI]
		public static void SetClipBoardText([NotNull] string label, [NotNull] string text)
		{
			if (label == null)
			{
				throw new ArgumentNullException("label");
			}
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			AGUtils.RunOnUiThread(() =>
			{
				var clip = C.AndroidContentClipData.AJCCallStaticOnceAJO("newPlainText", label, text);
				AGSystemService.ClipboardService.Call("setPrimaryClip", clip);
			});
		}
	}
}
#endif