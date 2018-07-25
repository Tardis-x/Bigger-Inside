#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System.Collections.Generic;
	using System.Linq;
	using Internal;
	using JetBrains.Annotations;

	/// <summary>
	/// Represents the contact address
	/// </summary>
	public sealed class ContactPostalAddress
	{
		[PublicAPI]
		public string Label { private set; get; }

		[PublicAPI]
		public string Street { private set; get; }

		[PublicAPI]
		public string SubLocality { private set; get; }

		[PublicAPI]
		public string City { private set; get; }

		[PublicAPI]
		public string SubAdministrativeArea { private set; get; }

		[PublicAPI]
		public string State { private set; get; }

		[PublicAPI]
		public string PostalCode { private set; get; }

		[PublicAPI]
		public string Country { private set; get; }

		[PublicAPI]
		public string IsoCountryCode { private set; get; }

		public static List<ContactPostalAddress> ParseList(List<object> postalAddresses)
		{
			return postalAddresses
				.Cast<Dictionary<string, object>>()
				.Select(dic => new ContactPostalAddress
				{
					Label = dic.GetStr("label"),
					Street = dic.GetStr("street"),
					SubLocality = dic.GetStr("subLocality"),
					City = dic.GetStr("city"),
					SubAdministrativeArea = dic.GetStr("subAdministrativeArea"),
					State = dic.GetStr("state"),
					PostalCode = dic.GetStr("postalCode"),
					Country = dic.GetStr("country"),
					IsoCountryCode = dic.GetStr("isoCountryCode")
				}).ToList();
		}

		public override string ToString()
		{
			return string.Format("[ContactPostalAddress: Label={0}, Street={1}, SubLocality={2}, City={3}, SubAdministrativeArea={4}, State={5}, PostalCode={6}, Country={7}, IsoCountryCode={8}]", Label, Street, SubLocality, City, SubAdministrativeArea, State, PostalCode, Country, IsoCountryCode);
		}
	}
}
#endif