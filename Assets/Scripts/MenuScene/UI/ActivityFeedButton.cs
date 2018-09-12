using Firebase.Auth;
using GetSocialSdk.Core;
using GetSocialSdk.Ui;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class ActivityFeedButton : MonoBehaviour
  {

    public void ShowActivityFeed()
    {
      GetSocial.WhenInitialized(() =>
      {
        GetSocialUi.CreateGlobalActivityFeedView()
          .SetWindowTitle("GDG News")
          .Show();
      });
    }
  }
}