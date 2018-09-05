using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class TimeslotScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Value References")] 
    [SerializeField] private Text _startTimeHoursText;
    [SerializeField] private Text _startTimeMinutesText;
    [SerializeField] private SpeechItemScript _speechPrefab;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public TimeslotScript GetInstance(List<SpeechItemModel> speeches, string startTime, float width)
    {
      TimeslotScript instance = Instantiate(this);
      instance.SetStartTime(startTime);

      int tsHeight = 170;
      
      foreach (var speech in speeches)
      {
        var item = _speechPrefab.GetInstance(speech);
        item.transform.SetParent(instance.transform);
        (item.transform as RectTransform).sizeDelta = new Vector2(width, item.General ? 330 : 510);
        tsHeight += item.General ? 330 : 510;
      }
      
      (instance.transform as RectTransform).sizeDelta = new Vector2(0, tsHeight);
      
      return instance;
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
    
    private void SetStartTime(string startTimeText)
    {
      _startTimeHoursText.text = startTimeText.Split(':')[0];
      _startTimeMinutesText.text = startTimeText.Split(':')[1];
    }
  }
}