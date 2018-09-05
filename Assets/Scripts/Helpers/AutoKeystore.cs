#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ua.org.gdg.devfest
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class AutoKeystore : MonoBehaviour {
#if UNITY_EDITOR

        private const string KeystoreName = "tardisx.keystore";
        private const string KeystorePass = "D9K4NTB1sErYd9TvVzt3nY8NWyUSopVR";
        private const string KeyAliasName = "devfest18";
        private const string KeyAliasPass = "1SsADZ3ijAOJUkD54wysQcCtQ7QqfZc7";
        
        
        static AutoKeystore() {
            var appPath = Application.dataPath.Split(char.Parse("/"));
            appPath[appPath.Length - 1] = "";
            var path = string.Join("/", appPath);
            PlayerSettings.Android.keystoreName = path + KeystoreName;
            PlayerSettings.Android.keystorePass = KeystorePass;
            PlayerSettings.Android.keyaliasName = KeyAliasName;
            PlayerSettings.Android.keyaliasPass = KeyAliasPass;
        }
#endif

    }
}

