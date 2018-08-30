#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using JetBrains.Annotations;
	using MiniJSON;
	using UnityEngine;

	/// <summary>
	/// Represents the contact from address book
	/// </summary>
	[PublicAPI]
	public sealed class Contact
	{
		[PublicAPI]
		public string Identifier { private set; get; }

		[PublicAPI]
		public string NamePrefix { private set; get; }

		[PublicAPI]
		public string GivenName { private set; get; }

		[PublicAPI]
		public string MiddleName { private set; get; }

		[PublicAPI]
		public string FamilyName { private set; get; }

		[PublicAPI]
		public string PreviousFamilyName { private set; get; }

		[PublicAPI]
		public string NameSuffix { private set; get; }

		[PublicAPI]
		public string Nickname { private set; get; }

		[PublicAPI]
		public string OrganizationName { private set; get; }

		[PublicAPI]
		public string DepartmentName { private set; get; }

		[PublicAPI]
		public string JobTitle { private set; get; }

		[PublicAPI]
		public string PhoneticGivenName { private set; get; }

		[PublicAPI]
		public string PhoneticMiddleName { private set; get; }

		[PublicAPI]
		public string PhoneticFamilyName { private set; get; }

		[PublicAPI]
		public string PhoneticOrganizationName { private set; get; }

		[PublicAPI]
		public string Note { private set; get; }

		[PublicAPI]
		public DateTime? Birthday { private set; get; }

		[PublicAPI]
		public List<ContactPhoneNumber> PhoneNumbers { private set; get; }

		[PublicAPI]
		public List<ContactEmail> Emails { private set; get; }

		[PublicAPI]
		public List<ContactPostalAddress> PostalAddresses { private set; get; }

		[PublicAPI]
		public List<ContactUrlAddress> Urls { private set; get; }

		[PublicAPI]
		public List<ContactRelation> Relations { private set; get; }

		[PublicAPI]
		public List<ContactSocialProfile> SocialProfiles { private set; get; }

		[PublicAPI]
		public List<ContactInstantMessageAddress> InstantMessageAddresses { private set; get; }

		[PublicAPI]
		public List<ContactDate> Dates { private set; get; }

		[PublicAPI]
		public static Contact FromJson(string jsonString)
		{
			var dict = Json.Deserialize(jsonString) as Dictionary<string, object>;

			var contact = new Contact
			{
				Identifier = dict.GetStr("identifier"),
				NamePrefix = dict.GetStr("namePrefix"),
				GivenName = dict.GetStr("givenName"),
				MiddleName = dict.GetStr("middleName"),
				FamilyName = dict.GetStr("familyName"),
				PreviousFamilyName = dict.GetStr("previousFamilyName"),
				NameSuffix = dict.GetStr("nameSuffix"),
				Nickname = dict.GetStr("nickname"),
				OrganizationName = dict.GetStr("organizationName"),
				DepartmentName = dict.GetStr("departmentName"),
				JobTitle = dict.GetStr("jobTitle"),
				PhoneticGivenName = dict.GetStr("phoneticGivenName"),
				PhoneticMiddleName = dict.GetStr("phoneticMiddleName"),
				PhoneticFamilyName = dict.GetStr("phoneticFamilyName"),
				PhoneticOrganizationName = dict.GetStr("phoneticOrganizationName"),
				Note = dict.GetStr("note")
			};

			var birthday = dict.GetDict("birthday");
			contact.Birthday = JsonUtils.DeserializeDateTime(birthday);
			
			var phoneNumbers = dict.GetArr("phoneNumbers");
			var emails = dict.GetArr("emails");
			var postalAddresses = dict.GetArr("postalAddresses");
			var urlAddresses = dict.GetArr("urlAddresses");
			var contactRelations = dict.GetArr("contactRelations");
			var socialProfiles = dict.GetArr("socialProfiles");
			var instantMessageAddresses = dict.GetArr("instantMessageAddresses");
			var dates = dict.GetArr("dates");

			contact.PhoneNumbers = ContactPhoneNumber.ParseList(phoneNumbers);
			contact.Emails = ContactEmail.ParseList(emails);
			contact.PostalAddresses = ContactPostalAddress.ParseList(postalAddresses);
			contact.Urls = ContactUrlAddress.ParseList(urlAddresses);
			contact.Relations = ContactRelation.ParseList(contactRelations);
			contact.SocialProfiles = ContactSocialProfile.ParseList(socialProfiles);
			contact.InstantMessageAddresses = ContactInstantMessageAddress.ParseList(instantMessageAddresses);
			contact.Dates = ContactDate.ParseList(dates);
			
			return contact;
		}

		public override string ToString()
		{
			return string.Format("[Contact: Identifier={0}, NamePrefix={1}, GivenName={2}, MiddleName={3}, FamilyName={4}, PreviousFamilyName={5}, NameSuffix={6}, Nickname={7}, OrganizationName={8}, DepartmentName={9}, JobTitle={10}, PhoneticGivenName={11}, PhoneticMiddleName={12}, PhoneticFamilyName={13}, PhoneticOrganizationName={14}, Note={15}, Birthday={16}, PhoneNumbers={17}, Emails={18}, PostalAddresses={19}, Urls={20}, Relations={21}, SocialProfiles={22}, InstantMessageAddresses={23}, Dates={24}]", 
				Identifier, NamePrefix, GivenName, MiddleName, FamilyName, PreviousFamilyName, NameSuffix, Nickname, OrganizationName, DepartmentName, JobTitle, PhoneticGivenName, PhoneticMiddleName, PhoneticFamilyName, PhoneticOrganizationName, Note, Birthday, 
				PhoneNumbers.ToStringList(), Emails.ToStringList(), PostalAddresses.ToStringList(), Urls.ToStringList(), Relations.ToStringList(), SocialProfiles.ToStringList(), InstantMessageAddresses.ToStringList(), Dates.ToStringList());
		}
	}
}
#endif