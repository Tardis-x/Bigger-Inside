using ua.org.gdg.devfest;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestManager : MonoBehaviour {
	public Text Text;
	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID
		var isArCoreSupported = ARCoreHelper.CheckArCoreSupport();
		Text.text = isArCoreSupported.ToString();
		#endif
	}

	public void LoadScene()
	{
		SceneManager.LoadScene(1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
