﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class AnswerVButtonScript : VirtualButtonOnClick
  {
    public override void OnClick()
    {
      if(GameManager.Instance.GameActive) GameManager.Instance.Answer();
    }
  }
}