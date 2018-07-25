using System.Collections;
using System.Collections.Generic;
using ua.org.gdg.devfest;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class PlayVButtonScript : VirtualButtonOnClick
  {
    public override void OnClick()
    {
      if (!GameManager.Instance.GameActive) GameManager.Instance.NewGame();
    }
  }
}
