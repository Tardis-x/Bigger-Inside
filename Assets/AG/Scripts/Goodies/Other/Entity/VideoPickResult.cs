#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using JetBrains.Annotations;
	using MiniJSON;
	using UnityEngine;

	[PublicAPI]
	public class VideoPickResult
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
		/// Get the path to the preview image of the video
		/// </summary>
		[PublicAPI]
		public string PreviewImagePath { get; private set; }

		/// <summary>
		/// Get the path to the thumbnail(big) of the video
		/// </summary>
		[PublicAPI]
		public string PreviewImageThumbnailPath { get; private set; }

		/// <summary>
		/// Get the path to the thumbnail(small) of the video
		/// </summary>
		[PublicAPI]
		public string PreviewImageSmallThumbnailPath { get; private set; }

		/// <summary>
		/// Get the video width
		/// </summary>
		[PublicAPI]
		public int Width { get; private set; }

		/// <summary>
		/// Get the video height
		/// </summary>
		[PublicAPI]
		public int Height { get; private set; }

		/// <summary>
		/// Video orientation
		/// </summary>
		[PublicAPI]
		public int Orientation { get; private set; }

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

		/// <summary>
		/// Read the picked file and load thumbnail image into <see cref="Texture2D"/>
		/// NOTE: Will be null if "generatePreviewImages" param is set to false when picking image
		/// </summary>
		/// <returns>Video preview image thumbnail as <see cref="Texture2D"/></returns>
		[PublicAPI]
		public Texture2D LoadPreviewImage()
		{
			return CommonUtils.TextureFromFile(PreviewImagePath);
		}

		/// <summary>
		/// Read the picked file and load thumbnail image into <see cref="Texture2D"/>
		/// NOTE: Will be null if "generatePreviewImages" param is set to false when picking image
		/// </summary>
		/// <returns>Video thumbnail image as <see cref="Texture2D"/></returns>
		[PublicAPI]
		public Texture2D LoadThumbnailPreviewImage()
		{
			return CommonUtils.TextureFromFile(PreviewImageThumbnailPath);
		}

		/// <summary>
		/// Read the picked file and load small thumbnail image into <see cref="Texture2D"/>
		/// NOTE: Will be null if "generatePreviewImages" param is set to false when picking image
		/// </summary>
		/// <returns>Video small image thumbnail as <see cref="Texture2D"/></returns>
		[PublicAPI]
		public Texture2D LoadSmallPreviewImage()
		{
			return CommonUtils.TextureFromFile(PreviewImageSmallThumbnailPath);
		}

		public static VideoPickResult FromJson(string json)
		{
			var videoResult = new VideoPickResult();
			var dic = Json.Deserialize(json) as Dictionary<string, object>;

			videoResult.OriginalPath = dic.GetStr("originalPath");

			videoResult.PreviewImagePath = dic.GetStr("previewImage");
			videoResult.PreviewImageThumbnailPath = dic.GetStr("previewImageThumbnail");
			videoResult.PreviewImageSmallThumbnailPath = dic.GetStr("previewImageThumbnailSmall");

			videoResult.DisplayName = dic.GetStr("displayName");
			videoResult.Width = (int) (long) dic["width"];
			videoResult.Height = (int) (long) dic["height"];
			videoResult.Orientation = (int) (long) dic["orientation"];
			videoResult.Size = (int) (long) dic["size"];
			videoResult.CreatedAt = CommonUtils.DateTimeFromMillisSinceEpoch((long) dic["createdAt"]);

			return videoResult;
		}

		public override string ToString()
		{
			return string.Format(
				"[VideoPickResult: OriginalPath={0}, DisplayName={1}, PreviewImagePath={2}, PreviewImageThumbnailPath={3}, PreviewImageSmallThumbnailPath={4}, Width={5}, Height={6}, Orientation={7}, Size={8}, CreatedAt={9}]",
				OriginalPath, DisplayName, PreviewImagePath, PreviewImageThumbnailPath, PreviewImageSmallThumbnailPath, Width, Height, Orientation, Size, CreatedAt);
		}
	}
}
#endif