namespace AndroidGoodiesExamples
{
	using UnityEngine;

	public class EnableOnClick : MonoBehaviour
	{
		public bool enable;

		public GameObject target;

		public void OnClick()
		{
			target.SetActive(enable);
		}
	}
}