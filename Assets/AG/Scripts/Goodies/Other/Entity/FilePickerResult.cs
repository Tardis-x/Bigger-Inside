#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using JetBrains.Annotations;
	using MiniJSON;

	/// <summary>
	/// Result of picking a file
	/// </summary>
	[PublicAPI]
	public class FilePickerResult
	{
		/// <summary>
		/// Path to the processed file. This will always be a local path on the device.
		/// </summary>
		[PublicAPI]
		public string OriginalPath { get; private set; }

		/// <summary>
		/// Display name of the file
		/// </summary>
		[PublicAPI]
		public string DisplayName { get; private set; }

		/// <summary>
		/// Get the size of the processed file in bytes
		/// </summary>
		[PublicAPI]
		public int Size { get; private set; }
		
		/// <summary>
		/// File creation date
		/// </summary>
		[PublicAPI]
		public DateTime CreatedAt { get; private set; }
		
		public static FilePickerResult FromJson(string json)
		{
			var videoResult = new FilePickerResult();
			var dic = Json.Deserialize(json) as Dictionary<string, object>;

			videoResult.OriginalPath = dic.GetStr("originalPath");
			
			videoResult.DisplayName = dic.GetStr("displayName");
			videoResult.Size = (int) (long) dic["size"];
			videoResult.CreatedAt = CommonUtils.DateTimeFromMillisSinceEpoch((long) dic["createdAt"]);

			return videoResult;
		}

		public override string ToString()
		{
			return string.Format("[FilePickerResult: OriginalPath={0}, DisplayName={1}, Size={2}, CreatedAt={3}]", OriginalPath, DisplayName, Size, CreatedAt);
		}
	}
}
#endif