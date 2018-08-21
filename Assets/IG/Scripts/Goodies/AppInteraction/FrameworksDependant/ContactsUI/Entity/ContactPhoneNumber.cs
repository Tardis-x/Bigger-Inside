#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System.Collections.Generic;
	using System.Linq;
	using Internal;
	using JetBrains.Annotations;

	/// <summary>
	/// Represents the contact phone number
	/// </summary>
	public sealed class ContactPhoneNumber
	{
		[PublicAPI]
		public string Label { private set; get; }

		[PublicAPI]
		public string Number { private set; get; }

		public static List<ContactPhoneNumber> ParseList(List<object> phoneNumbers)
		{
			return phoneNumbers
				.Cast<Dictionary<string, object>>()
				.Select(dic => new ContactPhoneNumber
				{
					Label = dic.GetStr("label"),
					Number = dic.GetStr("value")
				}).ToList();
		}

		public override string ToString()
		{
			return string.Format("[ContactPhoneNumber: Label={0}, Number={1}]", Label, Number);
		}
	}
}
#endif