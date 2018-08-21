#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System.Collections.Generic;
	using System.Linq;
	using Internal;
	using JetBrains.Annotations;

	/// <summary>
	/// Represents the contact relations
	/// </summary>
	[PublicAPI]
	public sealed class ContactSocialProfile
	{
		[PublicAPI]
		public string Label { private set; get; }

		[PublicAPI]
		public string UrlString { private set; get; }

		[PublicAPI]
		public string Username { private set; get; }

		[PublicAPI]
		public string Service { private set; get; }

		[PublicAPI]
		public string UserId { private set; get; }

		public static List<ContactSocialProfile> ParseList(List<object> socialProfiles)
		{
			return socialProfiles
				.Cast<Dictionary<string, object>>()
				.Select(dic => new ContactSocialProfile
				{
					Label = dic.GetStr("label"),
					UrlString = dic.GetStr("urlString"),
					Username = dic.GetStr("username"),
					Service = dic.GetStr("service"),
					UserId = dic.GetStr("userId")
				}).ToList();
		}

		public override string ToString()
		{
			return string.Format("[ContactSocialProfile: Label={0}, UrlString={1}, Username={2}, Service={3}, UserId={4}]", Label, UrlString, Username, Service, UserId);
		}
	}
}
#endif