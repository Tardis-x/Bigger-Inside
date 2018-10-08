using UnityEngine;

namespace ua.org.gdg.devfest 
{
  public class ApplicationQuitScript : MonoBehaviour
  {
    #if UNITY_ANDROID && !UNITY_EDITOR
    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
        Application.Quit();
      }
    }
    #endif
  }
}