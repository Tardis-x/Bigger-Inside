using UnityEngine;

namespace ua.org.gdg.devfest {
    public static class ARCoreHelper {
        public static AndroidJavaObject GetAppContext() {
            #if UNITY_ANDROID
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                var context = activity.Call<AndroidJavaObject>("getApplicationContext");
            
                return context;
            #else
                return null;
            #endif
        }

        public static bool CheckArCoreSupport()
        {
            #if UNITY_ANDROID
                var arCoreApk = new AndroidJavaClass("com.google.ar.core.ArCoreApk");
                var arCoreApkInstance = arCoreApk.CallStatic<AndroidJavaObject>("getInstance");
                var availability = arCoreApkInstance.Call<AndroidJavaObject>("checkAvailability", GetAppContext());
                var isUnsupported = availability.Call<bool>("isUnsupported");
            
                return !isUnsupported;
            #else
                return false;
            #endif
        }
    }
}