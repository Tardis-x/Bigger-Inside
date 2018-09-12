using Firebase.Auth;
using GetSocialSdk.Core;
using GetSocialSdk.Ui;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class ActivityFeedButton : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private GameEvent _showSignIn;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void ShowActivityFeed()
    {
      if (FirebaseAuth.DefaultInstance.CurrentUser == null)
      {
        _showSignIn.Raise();
        return;
      }

      GetSocialUi.CreateGlobalActivityFeedView()
        .SetWindowTitle("GDG News")
        .Show();
    }
  }
}