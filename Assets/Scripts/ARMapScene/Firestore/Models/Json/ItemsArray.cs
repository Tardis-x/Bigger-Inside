﻿using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class ItemsArray
  {
    public List<JsonMapValue<JsonSessionItem>> arrayValue;
  }
}