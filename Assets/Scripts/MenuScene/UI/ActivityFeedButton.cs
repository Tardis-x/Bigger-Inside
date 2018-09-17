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
      if (!IsUserAuthorized()) return; 

      GetSocialUi.CreateGlobalActivityFeedView()
        .SetWindowTitle("GDG News")
        .SetTagClickListener(OnTagClick)
        .Show();
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void OnTagClick(string tagName)
    {
      GetSocialUi.CreateGlobalActivityFeedView()
        .SetWindowTitle(tagName)
        .SetFilterByTags(tagName)
        .Show();
    }
    
    private bool IsUserAuthorized()
    {
      if (FirebaseAuth.DefaultInstance.CurrentUser != null) return true;
      
      _showSignIn.Raise();
      return false;
    }
  }
}