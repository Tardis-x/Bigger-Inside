namespace ua.org.gdg.devfest
{
	public class HitVButtonScript : VirtualButtonOnClick
	{
		public override void OnClick()
		{
			if(GameManager.Instance.GameActive) GameManager.Instance.Hit();
		}
	}
}