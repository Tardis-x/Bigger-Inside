// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGFlashLight.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using UnityEngine;

	/// <summary>
	/// Android flash light.
	/// 
	/// Required permissions:
	///     <uses-permission android:name="android.permission.CAMERA" />
	///     <uses-feature android:name="android.hardware.camera" />
	///
	///     <uses-permission android:name="android.permission.FLASHLIGHT"/>
	///     <uses-feature android:name="android.hardware.camera.flash" android:required="false" />
	/// 
	/// Android flash light.
	/// </summary>
	public static class AGFlashLight
	{
		const string CameraParameters_FLASH_MODE_TORCH = "torch";

		static AndroidJavaObject _camera;
		static string _cameraId;

		/// <summary>
		/// Determines if device has flashlight.
		/// </summary>
		/// <returns><c>true</c> if device has flashlight; otherwise, <c>false</c>.</returns>
		public static bool HasFlashlight()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			return AGDeviceInfo.SystemFeatures.HasFlashlight;
		}

		/// <summary>
		/// 
		/// Turns on the camera flashlight if available
		/// 
		/// Required permissions in manifest:
		///     <uses-permission android:name="android.permission.CAMERA" />
		///     <uses-feature android:name="android.hardware.camera" />
		///
		///     <uses-permission android:name="android.permission.FLASHLIGHT"/>
		///     <uses-feature android:name="android.hardware.camera.flash" android:required="false" />
		/// </summary>
		public static void Enable()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (!HasFlashlight())
			{
				Debug.LogWarning("This device does not have a flashlight");
				return;
			}

			if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.M)
			{
				TurnOnNew();
			}
			else
			{
				TurnOnOld();
			}
		}

		static void TurnOnNew()
		{
			try
			{
				_cameraId = AGSystemService.CameraService.Call<string[]>("getCameraIdList")[0];
				AGSystemService.CameraService.Call("setTorchMode", _cameraId, true);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		static void TurnOnOld()
		{
			if (_camera != null)
			{
				Debug.LogWarning("Flashlight is already on");
				return;
			}
			try
			{
				AGUtils.RunOnUiThread(() =>
				{
					using (var camAJC = new AndroidJavaClass(C.AndroidHardwareCamera))
					{
						var cam = camAJC.CallStaticAJO("open");
						var camParams = cam.CallAJO("getParameters");
						camParams.Call("setFlashMode", CameraParameters_FLASH_MODE_TORCH);
						cam.Call("setParameters", camParams);
						cam.Call("startPreview");
						_camera = cam;
					}
				});
			}
			catch (Exception e)
			{
				if (Debug.isDebugBuild)
				{
					Debug.Log("Could not enable flashlight:" + e.Message);
				}
			}
		}

		/// <summary>
		/// Turns off the camera flashlight
		/// </summary>
		public static void Disable()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.M)
			{
				TurnOffNew();
			}
			else
			{
				TurnOffOld();
			}
		}

		static void TurnOffNew()
		{
			try
			{
				AGSystemService.CameraService.Call("setTorchMode", _cameraId, false);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		static void TurnOffOld()
		{
			if (_camera == null)
				return;
			AGUtils.RunOnUiThread(() =>
			{
				_camera.Call("stopPreview");
				_camera.Call("release");
				_camera.Dispose();
				_camera = null;
			});
		}
	}
}

#endif