#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System.Collections.Generic;
	using System.Linq;
	using Internal;
	using JetBrains.Annotations;
	using MiniJSON;

	/// <summary>
	/// Contact pick result.
	/// </summary>
	[PublicAPI]
	public sealed class ContactPickResult
	{
		/// <summary>
		/// Gets the display name.
		/// </summary>
		/// <value>The display name.</value>
		[PublicAPI]
		public string DisplayName { get; private set; }

		/// <summary>
		/// Gets the photo URI. To load image as Texture2D use <see cref="AGFileUtils.ImageUriToTexture2D"/> passing this URI
		/// </summary>
		/// <value>The photo URI.</value>
		[PublicAPI]
		public string PhotoUri { get; private set; }

		/// <summary>
		/// Gets the phones.
		/// </summary>
		/// <value>The phones.</value>
		[PublicAPI]
		public List<string> Phones { get; private set; }

		/// <summary>
		/// Gets the emails.
		/// </summary>
		/// <value>The emails.</value>
		[PublicAPI]
		public List<string> Emails { get; private set; }

		ContactPickResult()
		{
			Phones = new List<string>();
			Emails = new List<string>();
		}

		public static ContactPickResult FromJson(string json)
		{
			var contact = new ContactPickResult();
			var dic = Json.Deserialize(json) as Dictionary<string, object>;

			contact.DisplayName = dic.GetStr("displayName");
			contact.PhotoUri = dic.GetStr("photoUri");
			contact.Phones = ((List<object>) dic["phones"]).OfType<string>().ToList();
			contact.Emails = ((List<object>) dic["emails"]).OfType<string>().ToList();

			return contact;
		}

		public override string ToString()
		{
			return string.Format("[ContactPickResult: DisplayName={0}]", DisplayName);
		}
	}
}
#endif