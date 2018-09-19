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
	public sealed class ContactRelation
	{
		[PublicAPI]
		public string Label { private set; get; }

		[PublicAPI]
		public string Value { private set; get; }

		public static List<ContactRelation> ParseList(List<object> contactRelations)
		{
			return contactRelations
				.Cast<Dictionary<string, object>>()
				.Select(dic => new ContactRelation
				{
					Label = dic.GetStr("label"),
					Value = dic.GetStr("value")
				}).ToList();
		}

		public override string ToString()
		{
			return string.Format("[ContactRelation: Label={0}, Value={1}]", Label, Value);
		}
	}
}
#endif