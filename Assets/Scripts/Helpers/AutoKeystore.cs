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

        private const string KeystoreName = Credentials.KEYSTORE_NAME;
        private const string KeystorePass = Credentials.KEYSTORE_PASS;
        private const string KeyAliasName = Credentials.ALIAS_NAME;
        private const string KeyAliasPass = Credentials.ALIAS_PASS;
        
        
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

