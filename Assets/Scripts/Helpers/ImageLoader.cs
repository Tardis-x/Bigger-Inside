using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class ImageLoader : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		public void LoadImage(string logoUrl, RawImage image)
		{
			WWW req = new WWW(logoUrl);
			StartCoroutine(OnResponse(req, image));
		}

		private IEnumerator OnResponse(WWW req, RawImage image)
		{
			yield return req;

			image.texture = req.texture;
		}
	}
}