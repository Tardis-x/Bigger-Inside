namespace AndroidGoodiesExamples
{
	using DeadMosquito.AndroidGoodies;
	using JetBrains.Annotations;
	using UnityEngine;
	using UnityEngine.UI;

	public class FingerprintScannerTest : MonoBehaviour
	{
		public Text statusText;
		public Text encryptionStatusText;

		const string Key = "MyKey";
		const string ToEncrypt = "This is secret text to be encrypted";

		string _encryptedValue;

#if UNITY_ANDROID
		[UsedImplicitly]
		public void OnStartAuth()
		{
			Debug.Log("Attempting to perform fingerprint auth...");

			if (!AGFingerprintScanner.HasFingerprintHardware)
			{
				Debug.Log("Current device doesn't have fingerprint hardware");
				return;
			}

			if (!AGFingerprintScanner.HasEnrolledFingerprint)
			{
				Debug.Log("Current device doesn't have enrolled fingerprints");
			}

			AGFingerprintScanner.Authenticate(
				() =>
				{
					statusText.color = Color.green;
					statusText.text = "Auth success";
					Debug.Log("Fingerprint authentication sucessful");
				},
				warning =>
				{
					statusText.color = Color.yellow;
					statusText.text = "WARNING: " + warning;
					Debug.Log("Fingerprint authentication failed with warning: " + warning);
				},
				error =>
				{
					statusText.color = Color.red;
					statusText.text = "ERROR: " + error;
					Debug.Log("Fingerprint authentication failed with error: " + error);
				});

			statusText.color = Color.white;
			statusText.text = "Put your finger on scanner";
		}

		[UsedImplicitly]
		public void OnCancelAuth()
		{
			Debug.Log("Attempting to cancel fingerprint auth...");

			AGFingerprintScanner.Cancel();

			statusText.color = Color.white;
			statusText.text = "Cancelled";
		}

		[UsedImplicitly]
		public void OnEncrypt()
		{
			if (!AGFingerprintScanner.HasFingerprintHardware)
			{
				Debug.Log("Current device doesn't have fingerprint hardware");
				return;
			}

			if (!AGFingerprintScanner.HasEnrolledFingerprint)
			{
				Debug.Log("Current device doesn't have enrolled fingerprints");
			}

			AGFingerprintScanner.Encrypt(Key, ToEncrypt,
				encryptedValue =>
				{
					encryptionStatusText.color = Color.green;
					encryptionStatusText.text = "Successfully encrypted: " + encryptedValue;
					_encryptedValue = encryptedValue;
				},
				warning =>
				{
					encryptionStatusText.color = Color.yellow;
					encryptionStatusText.text = "WARNING: " + warning;
					Debug.Log("Value encryption failed with warning: " + warning);
				},
				error =>
				{
					encryptionStatusText.color = Color.red;
					encryptionStatusText.text = "WARNING: " + error;
					Debug.Log("Value encryption failed with error: " + error);
				});

			encryptionStatusText.color = Color.white;
			encryptionStatusText.text = "Put your finger on scanner (encrypt)";
		}

		[UsedImplicitly]
		public void OnDecrypt()
		{
			if (!AGFingerprintScanner.HasFingerprintHardware)
			{
				Debug.Log("Current device doesn't have fingerprint hardware");
				return;
			}

			if (!AGFingerprintScanner.HasEnrolledFingerprint)
			{
				Debug.Log("Current device doesn't have enrolled fingerprints");
			}

			if (string.IsNullOrEmpty(_encryptedValue))
			{
				Debug.Log("Encrypt the value first");
				return;
			}

			AGFingerprintScanner.Decrypt(Key, _encryptedValue,
				decryptedValue =>
				{
					encryptionStatusText.color = Color.green;
					encryptionStatusText.text = "Successfully decrypted: " + decryptedValue;
				},
				warning =>
				{
					encryptionStatusText.color = Color.yellow;
					encryptionStatusText.text = "WARNING: " + warning;
					Debug.Log("Value decryption failed with warning: " + warning);
				},
				error =>
				{
					encryptionStatusText.color = Color.red;
					encryptionStatusText.text = "WARNING: " + error;
					Debug.Log("Value encryption failed with error: " + error);
				});

			encryptionStatusText.color = Color.white;
			encryptionStatusText.text = "Put your finger on scanner (decrypt)";
		}
#endif
	}
}