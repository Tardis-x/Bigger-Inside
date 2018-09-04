using Facebook.Unity;
using UnityEngine;
using Firebase.Auth;
using Google;

namespace ua.org.gdg.devfest
{
  public class Logout : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    public void OnLogout()
    {
      if(FB.IsLoggedIn) FB.LogOut();
      if(FirebaseAuth.DefaultInstance.CurrentUser != null) FirebaseAuth.DefaultInstance.SignOut();
      GoogleSignIn.DefaultInstance.SignOut();
    }
  }
}