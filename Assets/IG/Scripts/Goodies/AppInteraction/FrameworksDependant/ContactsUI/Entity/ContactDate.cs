#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Internal;
	using JetBrains.Annotations;

	/// <summary>
	/// Represents the contact date
	/// </summary>
	public sealed class ContactDate
	{
		[PublicAPI]
		public string Label { private set; get; }

		[PublicAPI]
		public DateTime? Date { private set; get; }

		public override string ToString()
		{
			return string.Format("[ContactDate: Label={0}, Date={1}]", Label, Date);
		}

		public static List<ContactDate> ParseList(List<object> dates)
		{
			return dates
				.Cast<Dictionary<string, object>>()
				.Select(dic => new ContactDate
				{
					Label = dic.GetStr("label"),
					Date = JsonUtils.DeserializeDateTime(dic)
				}).ToList();
		}
	}
}
#endif