using UnityEngine;

#if UNITY_ANDROID
using DeadMosquito.AndroidGoodies;
#endif

namespace ua.org.gdg.devfest
{
    public static class Utils
    {
        public static void ShowMessage(string message)
        {
            #if UNITY_EDITOR
                Debug.Log(message);
            #elif UNITY_ANDROID
                AGUIMisc.ShowToast(message);    
            #endif
        }
    }
}