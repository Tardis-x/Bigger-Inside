using UnityEngine;
using Firebase.Auth;
using Google;
using UnityEngine.SceneManagement;

namespace ua.org.gdg.devfest
{
  public class Logout : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    public void OnLogout()
    {
      FirebaseAuth.DefaultInstance.SignOut();
      GoogleSignIn.DefaultInstance.SignOut();
      SceneManager.LoadScene("SignInScene");
    }
  }
}