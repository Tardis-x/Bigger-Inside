using System;

namespace ua.org.gdg.devfest
{
	[Serializable]
	public class QuestionModel
	{
		public string Text { get; set; }
		public bool IsGood { get; set; }
	}
}