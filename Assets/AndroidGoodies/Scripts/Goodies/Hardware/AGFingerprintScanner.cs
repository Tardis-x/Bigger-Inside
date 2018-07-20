// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGFingerprintScanner.cs
//

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Class to handle fingerprint scanner and allows encrypting/decrypting values using the fingerprint scanner
	/// </summary>
	[PublicAPI]
	public class AGFingerprintScanner
	{
		[PublicAPI]
		public enum Warning
		{
			/// <summary>
			/// The image acquired was good.
			/// </summary>
			[PublicAPI]
			Good = 0,

			/// <summary>
			/// Only a partial fingerprint image was detected.
			/// </summary>
			[PublicAPI]
			Partial = 1,

			/// <summary>
			/// The fingerprint image was too noisy to process due to a detected condition.
			/// </summary>
			[PublicAPI]
			Insufficient = 2,

			/// <summary>
			/// The fingerprint image was too noisy due to suspected or detected dirt on the sensor.
			/// </summary>
			[PublicAPI]
			Dirty = 3,

			/// <summary>
			/// The fingerprint image was unreadable due to lack of motion.
			/// </summary>
			[PublicAPI]
			TooSlow = 4,

			/// <summary>
			/// The fingerprint image was incomplete due to quick motion.
			/// </summary>
			[PublicAPI]
			TooFast = 5,

			/// <summary>
			/// Fingerprint valid but not recognized.
			/// </summary>
			[PublicAPI]
			Failure = 6
		}

		[PublicAPI]
		public enum Error
		{
			///
			/// The hardware is unavailable.
			///
			[PublicAPI]
			Unavailable = 0,

			///
			/// Error state returned when the sensor was unable to process the current image.
			///
			[PublicAPI]
			UnableToProcess = 1,

			///
			/// Error state returned when the current request has been running too long.
			///
			[PublicAPI]
			Timeout = 2,

			///
			/// Error state returned for operations like enrollment; the operation cannot be completed because there's not
			/// enough storage remaining to complete the operation.
			///
			[PublicAPI]
			NotEnoughSpace = 3,

			///
			/// The operation was canceled because the fingerprint sensor is unavailable.
			///
			[PublicAPI]
			Canceled = 4,

			///
			/// The operation was canceled because the API is locked out due to too many attempts.
			///
			[PublicAPI]
			Lockout = 5,

			///
			/// CryptoFactory failed to initialize CryptoObject.
			///
			[PublicAPI]
			CryptoObjectInit = 6,

			///
			/// Crypto failed to decrypt the value.
			///
			[PublicAPI]
			DecryptionFailed = 7,

			///
			/// Crypto failed to encrypt the value.
			///
			[PublicAPI]
			EncryptionFailed = 8,

			///
			/// Unknown error happened.
			///
			[PublicAPI]
			Unknown = 9
		}

		static AndroidJavaObject _goldfingerAjo;

		static AndroidJavaObject GoldfingerAJO
		{
			get
			{
				if (_goldfingerAjo == null)
				{
					_goldfingerAjo = new AndroidJavaObject(C.GoldfingerBuilderClass, AGUtils.Activity)
						.CallAJO("build");
				}

				return _goldfingerAjo;
			}
		}

		/// <summary>
		/// Returns true if device has fingerprint hardware, false otherwise.
		/// </summary>
		[PublicAPI]
		public static bool HasFingerprintHardware
		{
			get
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return false;
				}

				return GoldfingerAJO.CallBool("hasFingerprintHardware");
			}
		}

		/// <summary>
		/// Returns true if user has fingerprint in device settings, false otherwise.
		/// </summary>
		[PublicAPI]
		public static bool HasEnrolledFingerprint
		{
			get
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return false;
				}

				return GoldfingerAJO.CallBool("hasEnrolledFingerprint");
			}
		}

		/// <summary>
		/// Authenticate user via Fingerprint.
		/// </summary>
		/// <param name="onSuccess">User successfully authenticated.</param>
		/// <param name="onWarning"></param>
		/// <param name="onError">Authentication or initialization error happened and fingerprint authentication is not active.</param>
		[PublicAPI]
		public static void Authenticate(Action onSuccess, Action<Warning> onWarning, Action<Error> onError)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			GoldfingerAJO.Call("authenticate", new FingerprintCallback(str => onSuccess(), onWarning, onError));
		}

		/// <summary>
		/// Authenticate user via Fingerprint. If user is successfully authenticated, encrypts the value
		/// </summary>
		/// <param name="keyName">unique key identifier for value</param>
		/// <param name="value">String value which will be encrypted if user successfully authenticates</param>
		/// <param name="onSuccess"></param>
		/// <param name="onWarning"></param>
		/// <param name="onError">Authentication or initialization error happened and fingerprint authentication is not active.</param>
		[PublicAPI]
		public static void Encrypt(string keyName, string value, Action<string> onSuccess, Action<Warning> onWarning, Action<Error> onError)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			GoldfingerAJO.Call("encrypt", keyName, value, new FingerprintCallback(onSuccess, onWarning, onError));
		}

		/// <summary>
		/// Authenticate user via Fingerprint. If user is successfully authenticated, decrypts the value
		/// </summary>
		/// <param name="keyName">unique key identifier for value</param>
		/// <param name="value">String value which will be decrypted if user successfully authenticates</param>
		/// <param name="onSuccess"></param>
		/// <param name="onWarning"></param>
		/// <param name="onError">Authentication or initialization error happened and fingerprint authentication is not active.</param>
		[PublicAPI]
		public static void Decrypt(string keyName, string value, Action<string> onSuccess, Action<Warning> onWarning, Action<Error> onError)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			GoldfingerAJO.Call("decrypt", keyName, value, new FingerprintCallback(onSuccess, onWarning, onError));
		}

		/// <summary>
		/// Cancel current active Fingerprint authentication.
		/// </summary>
		[PublicAPI]
		public static void Cancel()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (_goldfingerAjo != null)
			{
				_goldfingerAjo.Call("cancel");
				_goldfingerAjo.Dispose();
				_goldfingerAjo = null;
			}
		}
	}
}
#endif