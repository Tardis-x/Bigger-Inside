
#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Object = System.Object;

	class GoodiesSceneHelper : MonoBehaviour
	{
		static GoodiesSceneHelper _instance;
		static readonly object InitLock = new object();
		readonly object _queueLock = new object();
		readonly List<Action> _queuedActions = new List<Action>();
		readonly List<Action> _executingActions = new List<Action>();

		public static GoodiesSceneHelper Instance
		{
			get
			{
				if (_instance == null)
				{
					Init();
				}
				return _instance;
			}
		}

		public static bool IsInImmersiveMode { set; private get; }

		public Texture2D LastTekenScreenshot { get; private set; }

		internal static void Init()
		{
			lock (InitLock)
			{
				if (ReferenceEquals(_instance, null))
				{
					var instances = FindObjectsOfType<GoodiesSceneHelper>();

					if (instances.Length > 1)
					{
						Debug.LogError(typeof(GoodiesSceneHelper) + " Something went really wrong " +
						               " - there should never be more than 1 " + typeof(GoodiesSceneHelper) +
						               " Reopening the scene might fix it.");
					}
					else if (instances.Length == 0)
					{
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<GoodiesSceneHelper>();
						singleton.name = "GoodiesSceneHelper";

						DontDestroyOnLoad(singleton);

						Debug.Log("[Singleton] An _instance of " + typeof(GoodiesSceneHelper) +
						          " is needed in the scene, so '" + singleton.name +
						          "' was created with DontDestroyOnLoad.");
					}
					else
					{
						Debug.Log("[Singleton] Using _instance already created: " + _instance.gameObject.name);
					}
				}
			}
		}

		GoodiesSceneHelper()
		{
		}

		internal static void Queue(Action action)
		{
			if (action == null)
			{
				Debug.LogWarning("Trying to queue null action");
				return;
			}

			lock (_instance._queueLock)
			{
				_instance._queuedActions.Add(action);
			}
		}

		void OnApplicationFocus(bool focusStatus)
		{
			if (focusStatus && IsInImmersiveMode)
			{
				AGUIMisc.EnableImmersiveMode();
			}
		}

		void Update()
		{
			MoveQueuedActionsToExecuting();

			while (_executingActions.Count > 0)
			{
				Action action = _executingActions[0];
				_executingActions.RemoveAt(0);
				action();
			}
		}

		void MoveQueuedActionsToExecuting()
		{
			lock (_queueLock)
			{
				while (_queuedActions.Count > 0)
				{
					Action action = _queuedActions[0];
					_executingActions.Add(action);
					_queuedActions.RemoveAt(0);
				}
			}
		}

		#region share_screenshot

		public void SaveScreenshotToGallery(Action<string> onScreenSaved)
		{
			StartCoroutine(TakeScreenshot(Screen.width, Screen.height, onScreenSaved));
		}

		public IEnumerator TakeScreenshot(int width, int height, Action<string> onScreenSaved)
		{
			yield return new WaitForEndOfFrame();

			var texture = new Texture2D(width, height, TextureFormat.RGB24, true);
			texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			texture.Apply();
			LastTekenScreenshot = texture;
			var imageTitle = "Screenshot-" + DateTime.Now.ToString("yy-MM-dd-hh-mm-ss");
			var uri = AndroidPersistanceUtilsInternal.InsertImage(LastTekenScreenshot, imageTitle, "My awesome screenshot");
			onScreenSaved(uri);
		}

		#endregion

		// These are invoked from Java by UnityPlayer.UnitySendMessage

		#region picker_callbacks

		public void OnPickGalleryImageSuccess(string message)
		{
			AGGallery.OnSuccessTrigger(message);
		}

		public void OnPickGalleryImageError(string message)
		{
			AGGallery.OnErrorTrigger(message);
		}

		public void OnPickPhotoImageSuccess(string message)
		{
			AGCamera.OnSuccessTrigger(message);
		}

		public void OnPickPhotoImageError(string message)
		{
			AGCamera.OnErrorTrigger(message);
		}

		public void OnRequestPermissionsResult(string message)
		{
			AGPermissions.OnRequestPermissionsResult(message);
		}

		

		#endregion
	}
}
#endif