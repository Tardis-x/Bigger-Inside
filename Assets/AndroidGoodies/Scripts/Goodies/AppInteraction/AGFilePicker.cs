// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGFilePicker.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;

	/// <summary>
	/// Methods to pick different files on device
	/// </summary>
	public static class AGFilePicker
	{
		static Action<AudioPickResult> _onAudioSuccessAction;
		static Action<string> _onAudioCancelAction;

		static Action<VideoPickResult> _onVideoSuccessAction;
		static Action<string> _onVideoCancelAction;

		#region audio_picker

		/// <summary>
		/// Pick the audio file from the file system
		/// </summary>
		/// <param name="onSuccess">Audio file was successfully picked by the user. Result is received in a callback.</param>
		/// <param name="onError">Picking audio file failed.</param>
		public static void PickAudio(Action<AudioPickResult> onSuccess, Action<string> onError)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(onSuccess, "onSuccess");
			Check.Argument.IsNotNull(onError, "onError");
			_onAudioSuccessAction = onSuccess;
			_onAudioCancelAction = onError;

			AGActivityUtils.PickAudio();
		}

		public static void OnAudioSuccessTrigger(string json)
		{
			if (_onAudioSuccessAction != null)
			{
				var result = AudioPickResult.FromJson(json);
				_onAudioSuccessAction(result);
				_onAudioSuccessAction = null;
			}
		}

		public static void OnAudioErrorTrigger(string message)
		{
			if (_onAudioCancelAction != null)
			{
				_onAudioCancelAction(message);
				_onAudioCancelAction = null;
			}
		}

		#endregion

		#region pick_video

		/// <summary>
		/// Pick the video file from the file system
		/// </summary>
		/// <param name="onSuccess">Video file was successfully picked by the user. Result is received in a callback.</param>
		/// <param name="onError">Picking video file failed.</param>
		public static void PickVideo(Action<VideoPickResult> onSuccess, Action<string> onError, bool generatePreviewImages = true)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(onSuccess, "onSuccess");
			Check.Argument.IsNotNull(onError, "onError");
			_onVideoSuccessAction = onSuccess;
			_onVideoCancelAction = onError;

			AGActivityUtils.PickVideoDevice(generatePreviewImages);
		}

		public static void OnVideoSuccessTrigger(string json)
		{
			if (_onVideoSuccessAction != null)
			{
				var result = VideoPickResult.FromJson(json);
				_onVideoSuccessAction(result);
				_onVideoSuccessAction = null;
			}
		}

		public static void OnVideoErrorTrigger(string message)
		{
			if (_onVideoCancelAction != null)
			{
				_onVideoCancelAction(message);
				_onVideoCancelAction = null;
			}
		}

		#endregion

		#region file_picker

		static Action<FilePickerResult> _onFileSuccessAction;
		static Action<string> _onFileCancelAction;

		public static void PickFile(Action<FilePickerResult> onSuccess, Action<string> onError, string mimeType)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}
			
			Check.Argument.IsNotNull(onSuccess, "onSuccess");
			Check.Argument.IsNotNull(onError, "onError");
			_onFileSuccessAction = onSuccess;
			_onFileCancelAction = onError;
			
			AGActivityUtils.PickFile(mimeType);
		}
		
		public static void OnFileSuccessTrigger(string json)
		{
			if (_onFileSuccessAction != null)
			{
				var result = FilePickerResult.FromJson(json);
				_onFileSuccessAction(result);
				_onFileSuccessAction = null;
			}
		}

		public static void OnFileErrorTrigger(string message)
		{
			if (_onFileCancelAction != null)
			{
				_onFileCancelAction(message);
				_onFileCancelAction = null;
			}
		}

		#endregion
	}
}
#endif