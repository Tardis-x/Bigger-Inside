#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System.Collections.Generic;
	using System.Linq;
	using Internal;
	using JetBrains.Annotations;

	/// <summary>
	/// Represents the contact instant messenger account
	/// </summary>
	public sealed class ContactInstantMessageAddress
	{
		[PublicAPI]
		public string Label { private set; get; }

		[PublicAPI]
		public string Service { private set; get; }

		[PublicAPI]
		public string Username { private set; get; }

		public static List<ContactInstantMessageAddress> ParseList(List<object> instantMessageAddresses)
		{
			return instantMessageAddresses
				.Cast<Dictionary<string, object>>()
				.Select(dic => new ContactInstantMessageAddress
				{
					Label = dic.GetStr("label"),
					Username = dic.GetStr("username"),
					Service = dic.GetStr("service")
				}).ToList();
		}

		public override string ToString()
		{
			return string.Format("[ContactInstantMessageAddress: Label={0}, Service={1}, Username={2}]", Label, Service, Username);
		}
	}
}
#endif