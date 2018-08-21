
#if UNITY_ANDROID

namespace DeadMosquito.AndroidGoodies
{
	using UnityEngine;

	/// <summary>
	/// Information you can retrieve about a particular application. 
	/// This corresponds to information collected from the AndroidManifest.xml's <application> tag.
	/// 
	/// https://developer.android.com/reference/android/content/pm/PackageInfo.html
	/// </summary>
	public sealed class PackageInfo
	{
		/// <summary>
		/// The name of this package.
		/// </summary>
		public string PackageName { get; private set; }

		/// <summary>
		/// The version number of this package, as specified by the <manifest> tag's versionCode attribute.
		/// </summary>
		public int VersionCode { get; private set; }

		/// <summary>
		/// The version name of this package, as specified by the <manifest> tag's versionName attribute.
		/// </summary>
		public string VersionName { get; private set; }


		public static PackageInfo FromJavaObj(AndroidJavaObject ajo)
		{
			using (ajo)
			{
				var result = new PackageInfo
				{
					PackageName = ajo.Get<string>("packageName"),
					VersionCode = ajo.Get<int>("versionCode"),
					VersionName = ajo.Get<string>("versionName")
				};
				return result;
			}
		}

		public override string ToString()
		{
			return string.Format("PackageName: {0}, VersionCode: {1}, VersionName: {2}", PackageName, VersionCode, VersionName);
		}
	}
}

#endif