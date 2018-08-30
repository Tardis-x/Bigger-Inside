#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System.Collections.Generic;
	using System.Linq;
	using Internal;
	using JetBrains.Annotations;

	/// <summary>
	/// Represents the contact email address
	/// </summary>
	public sealed class ContactEmail
	{
		[PublicAPI]
		public string Label { private set; get; }

		[PublicAPI]
		public string Email { private set; get; }

		public static List<ContactEmail> ParseList(List<object> emails)
		{
			return emails
				.Cast<Dictionary<string, object>>()
				.Select(dic => new ContactEmail
				{
					Label = dic.GetStr("label"),
					Email = dic.GetStr("value")
				}).ToList();
		}

		public override string ToString()
		{
			return string.Format("[ContactEmail: Label={0}, Email={1}]", Label, Email);
		}
	}
}
#endif