using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class SpeechScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Value References")]
    [SerializeField] private Text _startTimeText;
    [SerializeField] private Text _endTimeText;
    [SerializeField] private Text _nameText;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public RectTransform GetInstance()
    {
      return Instantiate(GetComponent<RectTransform>());
    }

    public RectTransform GetInstance(string startTime, string endTime, string name)
    {
      SpeechScript instance = Instantiate(this);
      instance.SetName(name);
      instance.SetStartTime(startTime);
      instance.SetEndTime(endTime);
      return instance.GetComponent<RectTransform>();
    }
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private void SetStartTime(string startTimeText)
    {
      _startTimeText.text = startTimeText;
    }
    
    private void SetEndTime(string endTimeText)
    {
      _endTimeText.text = endTimeText;
    }

    private void SetName(string nameText)
    {
      _nameText.text = nameText;
    }
  }
}