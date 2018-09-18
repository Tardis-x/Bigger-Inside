using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class TimeslotScript : MonoBehaviour
  {
    private const int ITEM_HEIGHT_W_O_SPEAKER = 300;
    private const int ITEM_HEIGHT_W_SPEAKER = 600;
    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("UI")] 
    [SerializeField] private Text _startTimeHoursText;
    [SerializeField] private Text _startTimeMinutesText;
    
    [Space]
    [Header("Prefabs")]
    [SerializeField] private SpeechItemScript _speechPrefab;

    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------
    
    public bool Empty { get; private set; }
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public TimeslotScript GetInstance(List<SpeechItemModel> speeches, string startTime, float width)
    {
      var instance = Instantiate(this);
      instance.SetStartTime(startTime);

      instance.Empty = true;
      
      var tsHeight = 170;
      
      foreach (var speech in speeches)
      {
        var item = _speechPrefab.GetInstance(speech);
        item.transform.SetParent(instance.transform);
        (item.transform as RectTransform).sizeDelta = new Vector2(width, item.General ?
          ITEM_HEIGHT_W_O_SPEAKER : ITEM_HEIGHT_W_SPEAKER);
        tsHeight += item.General ? ITEM_HEIGHT_W_O_SPEAKER : ITEM_HEIGHT_W_SPEAKER;
        instance.Empty = false;
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