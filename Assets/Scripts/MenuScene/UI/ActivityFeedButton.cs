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
        var wasShown = GetSocialUi.CreateGlobalActivityFeedView()
          .SetButtonActionListener((s, post) =>
          {
          })
          .Show();
      });
    }
  }
}