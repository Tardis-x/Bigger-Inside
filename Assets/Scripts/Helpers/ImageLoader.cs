using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class ImageLoader : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------
		
		private void Awake()
		{
			LOGO_BASE_PATH = Application.persistentDataPath + "Graphics/Logo";
		}

		private string LOGO_BASE_PATH;

		public void LoadImage(string logoUrl, RawImage image)
		{
			string filePath = GetFilePathFromUrl(logoUrl);

			if (LoadFromFile(filePath, image)) return;

			WWW req = new WWW(logoUrl);
			StartCoroutine(OnResponse(req, filePath, image));
		}

		private IEnumerator OnResponse(WWW req, string filePath, RawImage image)
		{
			yield return req;

			image.texture = req.texture;
			SaveLogoToFile(filePath, req.bytes);
		}

		private bool LoadFromFile(string fileName, RawImage image)
		{
			var filePath = LOGO_BASE_PATH + fileName;

			if (!File.Exists(filePath)) return false;

			var fileData = File.ReadAllBytes(filePath);
			SetImageTexture(image, fileData);
			return true;
		}

		private void SetImageTexture(RawImage image, byte[] data)
		{
			var texture2D = new Texture2D(0, 0, TextureFormat.BGRA32, false);
			texture2D.LoadImage(data);
			image.texture = texture2D;
		}

		private void SaveLogoToFile(string fileName, byte[] logoBytes)
		{
			var filePath = LOGO_BASE_PATH + fileName;
			var directoryName = Path.GetDirectoryName(filePath);

			if (directoryName == null) return;

			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}

			File.WriteAllBytes(filePath, logoBytes);
		}

		private string GetFilePathFromUrl(string url)
		{
			if (!url.Contains('%')) return url.Split('/').Last();

			return url.Split('%').First(x => x.Contains("?")).Split('?').First(y => y.Contains(".jpg"));
		}
	}
}