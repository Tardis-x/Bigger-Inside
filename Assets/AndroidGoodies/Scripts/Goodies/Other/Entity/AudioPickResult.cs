#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using JetBrains.Annotations;
	using MiniJSON;

	/// <summary>
	/// Audio pick result
	/// </summary>
	[PublicAPI]
	public class AudioPickResult
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
		/// Mimetype of the file
		/// </summary>
		[PublicAPI]
		public string MimeType { get; private set; }

		/// <summary>
		/// File creation date
		/// </summary>
		[PublicAPI]
		public DateTime CreatedAt { get; private set; }

		public static AudioPickResult FromJson(string json)
		{
			var result = new AudioPickResult();
			var dic = Json.Deserialize(json) as Dictionary<string, object>;

			result.OriginalPath = dic.GetStr("originalPath");
			result.DisplayName = dic.GetStr("displayName");
			result.MimeType = dic.GetStr("mimeType");
			var createdAt = "createdAt";
			if (dic.ContainsKey(createdAt))
			{
				result.CreatedAt = CommonUtils.DateTimeFromMillisSinceEpoch((long) dic[createdAt]);
			}
			result.Size = (int) (long) dic["size"];

			return result;
		}

		public override string ToString()
		{
			return string.Format("[AudioPickResult: OriginalPath={0}, DisplayName={1}, Size={2}, MimeType={3}, CreatedAt={4}]", OriginalPath, DisplayName, Size, MimeType, CreatedAt);
		}
	}
}
#endif