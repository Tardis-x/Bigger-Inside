//
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGMediaRecorder.cs
//

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using UnityEngine;

	/// <summary>
	/// IMPORTANT: This class requires the following permission in AndroidManifest.xml file:
	/// 
	/// 	<uses-permission android:name="android.permission.RECORD_AUDIO" />
	/// 
	/// Class for recording audio files using device microphone
	/// </summary>
	public static class AGMediaRecorder
	{
		const int MediaRecorderAudioSourceMIC = 0x00000001;
		static AndroidJavaObject _mediaRecorder;
		
		/// <summary>
		/// Defines the audio encoding. 
		/// </summary>
		public enum AudioEncoder
		{
			/// <summary>
			/// AAC Low Complexity (AAC-LC) audio codec
			/// </summary>
			[PublicAPI]
			AAC = 0x00000003,
			
			/// <summary>
			/// Enhanced Low Delay AAC (AAC-ELD) audio codec
			/// </summary>
			[PublicAPI]
			AAC_ELD = 0x00000005,
			
			/// <summary>
			/// AMR (Narrowband) audio codec
			/// </summary>
			[PublicAPI]
			AMR_NB = 0x00000001,
			
			/// <summary>
			/// AMR (Wideband) audio codec
			/// </summary>
			[PublicAPI]
			AMR_WB = 0x00000002,
			
			[PublicAPI]
			DEFAULT = 0,
			
			/// <summary>
			/// High Efficiency AAC (HE-AAC) audio codec
			/// </summary>
			[PublicAPI]
			HE_AAC = 0x00000004,
			
			/// <summary>
			/// Ogg Vorbis audio codec
			/// </summary>
			[PublicAPI]
			VORBIS = 0x00000006
		}

		[PublicAPI]
		[SuppressMessage("ReSharper", "UnusedMember.Global")]
		public enum OutputFormat
		{
			/// <summary>
			/// AAC ADTS file format
			/// </summary>
			[PublicAPI]
			AAC_ADTS = 0x00000006,
			
			/// <summary>
			/// AMR NB file format
			/// </summary>
			[PublicAPI]
			AMR_NB = 0x00000003,
			
			/// <summary>
			/// AMR WB file format
			/// </summary>
			[PublicAPI]
			AMR_WB = 0x00000004,
			
			/// <summary>
			/// Default file format
			/// </summary>
			[PublicAPI]
			DEFAULT = 0x00000000,
			
			/// <summary>
			/// H.264/AAC data encapsulated in MPEG2/TS
			/// </summary>
			[PublicAPI]
			MPEG_2_TS = 0x00000008,
			
			/// <summary>
			/// MPEG4 media file format
			/// </summary>
			[PublicAPI]
			MPEG_4 = 0x00000002,
			
			[Obsolete("This constant was deprecated in API level 16. Deprecated in favor of MediaRecorder.OutputFormat.AMR_NB")]
			[PublicAPI]
			RAW_AMR = 0x00000003,
			
			/// <summary>
			/// 3GPP media file format
			/// </summary>
			[PublicAPI]
			THREE_GPP = 0x00000001,
			
			/// <summary>
			/// VP8/VORBIS data in a WEBM container
			/// </summary>
			[PublicAPI]
			WEBM = 0x00000009
		}

		/// <summary>
		/// Start recording audio
		/// </summary>
		/// <param name="fullFilePath">Full path to file where the recording will be stored. If you provide the path on external storage make sure the app has permissions to write external storage.</param>
		/// <param name="format">Audio format</param>
		/// <param name="audioEncoder">Audio enode to user</param>
		[PublicAPI]
		public static void StartRecording([NotNull] string fullFilePath, OutputFormat format = OutputFormat.THREE_GPP, AudioEncoder audioEncoder = AudioEncoder.AMR_NB)
		{
			if (fullFilePath == null)
			{
				throw new ArgumentNullException("fullFilePath");
			}
			
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (_mediaRecorder != null)
			{
				Debug.LogWarning(
					"Can't start recording while another recording is in progress. Please call StopRecording() to stop previous recording.");
				return;
			}

			try
			{
				_mediaRecorder = new AndroidJavaObject(C.AndroidMediaMediaRecorder);
				_mediaRecorder.Call("setAudioSource", MediaRecorderAudioSourceMIC);
				_mediaRecorder.Call("setOutputFormat", (int) format);
				_mediaRecorder.Call("setAudioEncoder", (int) audioEncoder);
				_mediaRecorder.Call("setOutputFile", fullFilePath);
				_mediaRecorder.Call("prepare");
				_mediaRecorder.Call("start");
			}
			catch (Exception e)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogError("Failed to stard recording audio");
					Debug.LogException(e);
				}
			}
		}

		/// <summary>
		/// Stops the recording. After calling this method you can read the file which you provided to <see cref="StartRecording"/>
		/// </summary>
		[PublicAPI]
		public static bool StopRecording()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			if (_mediaRecorder == null)
			{
				return false;
			}

			_mediaRecorder.Call("stop");
			_mediaRecorder.Call("release");
			_mediaRecorder.Dispose();
			_mediaRecorder = null;
			return true;
		}
	}
}
#endif