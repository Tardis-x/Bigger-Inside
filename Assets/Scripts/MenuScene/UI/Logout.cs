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
      if (FirebaseAuth.DefaultInstance.CurrentUser == null) return;
      
      FirebaseAuth.DefaultInstance.SignOut();
      
      if(FB.IsLoggedIn) FB.LogOut();
      else GoogleSignIn.DefaultInstance.SignOut();
    }
  }
}