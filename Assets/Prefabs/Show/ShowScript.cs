using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class ShowScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Value References")]
    [SerializeField] private Text _timeText;
    [SerializeField] private Text _nameText;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public RectTransform GetInstance()
    {
      return Instantiate(GetComponent<RectTransform>());
    }

    public RectTransform GetInstance(string time, string name)
    {
      ShowScript instance = Instantiate(this);
      instance.SetName(name);
      instance.SetTime(time);
      return instance.GetComponent<RectTransform>();
    }
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private void SetTime(string timeText)
    {
      _timeText.text = timeText;
    }

    private void SetName(string nameText)
    {
      _nameText.text = nameText;
    }
  }
}