using GetSocialSdk.Core;
using GetSocialSdk.Ui;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class GetSocialController : MonoBehaviour
  {
    private void Awake()
    {
      Debug.Log("awake");
      GetSocial.WhenInitialized(() =>
      {
        Debug.Log("GetSocial Init");
        var wasShown = GetSocialUi.CreateGlobalActivityFeedView()
          .SetButtonActionListener((s, post) =>
          {
            Debug.Log("Button was pressed with action: " + s);
          })
          .Show();
        Debug.Log("Was shown: " + wasShown);
      });
    }
  }
}