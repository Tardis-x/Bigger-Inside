using System.Collections;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Application = UnityEngine.Application;

public class QuestLeaderBoardController : MonoBehaviour
{
	public GameObject leaderboardEntryPrefab;
	public Transform gridForList;
	QuestManager _questManager;
	public QuestFirebaseData firebaseData;
	public Scrollbar scrollbar;
	public Sprite currentUserSprite;
	public GameObject position1Holder;
	public GameObject position2Holder;
	public GameObject position3Holder;
	
	void Awake()
	{
		Debug.Log("QuestLeaderBoardController.Awake");
		// obtain reference to object that represents quest manager
		QuestManagerReferenceInitialization();
	}

	void OnEnable()
	{
		Debug.Log("QuestLeaderBoardController.OnEnable");
		_questManager.UpdateUserScoreInLeaderBoard();
		UpdateLeaderBoard();
	}

	void QuestManagerReferenceInitialization()
	{
		var questManagerTemp = GameObject.Find("QuestManager");

		if (questManagerTemp != null)
		{
			_questManager = questManagerTemp.GetComponent<QuestManager>();

			if (_questManager == null)
			{
				Debug.LogError("Could not locate QuestManager component on " + questManagerTemp.name);
			}
		}
		else
		{
			Debug.LogError("Could not locate quest manager object in current scene!");
		}
	} 
	
	void UpdateLeaderBoard()
	{
		var position = 1;
		
		CreateImageFolder();
		
		foreach (var pair in _questManager.QuestLeaderboardData.OrderByDescending(entry => entry.Value.globalScore))
		{
			var userInfo = new GameObject();
			if (position == 1)
			{
				position1Holder.gameObject.SetActive(true);
				userInfo = position1Holder;
			}
			else if (position == 2)
			{
				position2Holder.gameObject.SetActive(true);
				userInfo = position2Holder;
			}
			else if (position == 3)
			{
				position3Holder.gameObject.SetActive(true);
				userInfo = position3Holder;
			}
			else
			{
				userInfo = Instantiate(leaderboardEntryPrefab, transform.position, Quaternion.identity, gridForList);
			}
			
			var texts = userInfo.GetComponentsInChildren<Text>();

			foreach (var text in texts)
			{
				if (text.CompareTag("PositionText"))
				{
					text.text = position.ToString();
				}
				else if (text.CompareTag("UserNameText"))
				{
					text.text = pair.Key;
				}
				else if (text.CompareTag("UserScoreText"))
				{
					text.text = pair.Value.globalScore.ToString();
				}
			}
			
			var images = userInfo.GetComponentsInChildren<Image>();
			
			var path = Application.persistentDataPath + "/UserImages/" + pair.Key.Replace(" ", string.Empty) + ".png";
			Debug.Log("QuestLeaderboard: filepath - " + path);
			
			foreach (var image in images)
			{
				if (image.CompareTag("UserImage"))
				{
					if (!File.Exists(path))
					{
						Debug.Log("QuestLeaderboard: Loading Image from URL.");
						StartCoroutine(LoadUserImageFromUrl(pair.Value.userPhotoUrl.ToString(), image, path));
					}
					else
					{
						Debug.Log("QuestLeaderboard: Loading Image from folder.");
						LoadUserImageFromFolder(image, path);
					}
				}
			}

			if (position > 3)
			{
				//Extra for current user
				if (pair.Key == firebaseData.currentUserUserId)
				{
					var color = Color.white;
					color.a = 1;
					userInfo.GetComponent<Image>().color = color;
					userInfo.GetComponent<Image>().sprite = currentUserSprite;
				}
			}
			position++;
		}
		scrollbar.value = 1;
	}

	static IEnumerator LoadUserImageFromUrl([CanBeNull]string url, Image image, string filePath)
	{
		if (url != null)
		{
			var www = new WWW(url);
			yield return www;
			image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
			//Saving texture to file
			File.WriteAllBytes(filePath, www.texture.EncodeToPNG());
		}
	}
	
	static void LoadUserImageFromFolder(Image image, string filePath)
	{
		var texture = new Texture2D(image.mainTexture.width, image.mainTexture.height, TextureFormat.ARGB32, false);
		texture.LoadImage(File.ReadAllBytes(filePath));
		image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
	}

	static void CreateImageFolder()
	{
		var path = Application.persistentDataPath + "/UserImages";
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}
}
