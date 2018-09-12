#if UNITY_2018_1_OR_NEWER

using System.IO;
using System.Text;
using System.Xml;
using UnityEditor.Android;
using UnityEngine;

namespace ua.org.gdg.devfest
{
    public class ModifyUnityAndroidAppManifest : IPostGenerateGradleAndroidProject
    {

        public void OnPostGenerateGradleAndroidProject(string basePath)
        {
            // If needed, add condition checks on whether you need to run the modification routine.
            // For example, specific configuration/app options enabled

            Debug.LogWarning("Android manifest will be modified.");
            
            var androidManifest = new AndroidManifest(GetManifestPath(basePath));

            androidManifest.SetHardwareAcceleratedMode(true);

            androidManifest.Save();
        }

        public int callbackOrder { get { return 1; } }

        private string _manifestFilePath;

        private string GetManifestPath(string basePath)
        {
            if (string.IsNullOrEmpty(_manifestFilePath))
            {
                var pathBuilder = new StringBuilder(basePath);
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
                _manifestFilePath = pathBuilder.ToString();
            }
            return _manifestFilePath;
        }
}


internal class AndroidXmlDocument : XmlDocument
{
    private string m_Path;
    protected XmlNamespaceManager nsMgr;
    public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
    public AndroidXmlDocument(string path)
    {
        m_Path = path;
        using (var reader = new XmlTextReader(m_Path))
        {
            reader.Read();
            Load(reader);
        }
        nsMgr = new XmlNamespaceManager(NameTable);
        nsMgr.AddNamespace("android", AndroidXmlNamespace);
    }

    public string Save()
    {
        return SaveAs(m_Path);
    }

    public string SaveAs(string path)
    {
        using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
        {
            writer.Formatting = Formatting.Indented;
            Save(writer);
        }
        return path;
    }
}


    internal class AndroidManifest : AndroidXmlDocument
    {
        public AndroidManifest(string path) : base(path)
        {
        }

        private XmlAttribute CreateAndroidAttribute(string key, string value)
        {
            XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
            attr.Value = value;
            return attr;
        }

        internal void SetHardwareAcceleratedMode(bool isEnabled)
        {
            var attributes = GetActivityWithLaunchIntent().Attributes;
            if (attributes != null)
            {
                attributes.Append(CreateAndroidAttribute("hardwareAccelerated", isEnabled.ToString()));
            }
        }

        private XmlNode GetActivityWithLaunchIntent()
        {
            return SelectSingleNode(
                "/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and " +
                "intent-filter/category/@android:name='android.intent.category.LAUNCHER']", nsMgr);
        }
    }

}

#endif