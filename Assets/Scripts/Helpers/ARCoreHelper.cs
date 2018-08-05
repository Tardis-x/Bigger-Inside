using UnityEngine;

namespace ua.org.gdg.devfest {
    #if UNITY_ANDROID
    public class ARCoreHelper: MonoBehaviour {
        public static AndroidJavaObject GetAppContext() {
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var context = activity.Call<AndroidJavaObject>("getApplicationContext");
            return context;
        }

        public static bool ArCoreCheck() {
            var arCoreApk = new AndroidJavaClass("com.google.ar.core.ArCoreApk");
            var arCoreApkInstance = arCoreApk.CallStatic<AndroidJavaObject>("getInstance");
            var availability = arCoreApkInstance.Call<AndroidJavaObject>("checkAvailability", GetAppContext());

            var isUnsupported = availability.Call<bool>("isUnsupported");
            return !isUnsupported;
        }
    }
    #endif
}