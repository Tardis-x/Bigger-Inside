namespace DeadMosquito.IosGoodies
{
	using System.Collections.Generic;
	using System.Linq;
	using Internal;
	using JetBrains.Annotations;

	/// <summary>
	/// Represents contact url addresses or web sites
	/// </summary>
	public class ContactUrlAddress
	{
		[PublicAPI]
		public string Label { private set; get; }
		
		[PublicAPI]
		public string Url { private set; get; }

		public static List<ContactUrlAddress> ParseList(List<object> urlAddresses)
		{
			return urlAddresses
				.Cast<Dictionary<string, object>>()
				.Select(dic => new ContactUrlAddress
				{
					Label = dic.GetStr("label"),
					Url = dic.GetStr("value")
				}).ToList();
		}

		public override string ToString()
		{
			return string.Format("[ContactUrlAddress: Label={0}, Url={1}]", Label, Url);
		}
	}
}