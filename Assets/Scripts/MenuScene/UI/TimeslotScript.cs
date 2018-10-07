using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class TimeslotScript : MonoBehaviour
  {
    private const int ITEM_HEIGHT_W_O_SPEAKER = 275;
    private const int ITEM_HEIGHT_W_SPEAKER = 600;
    private const int VIEWPORT_HEIGHT = 1620;

    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("UI")]
    [SerializeField] private Text _startTimeHoursText;
    [SerializeField] private Text _startTimeMinutesText;
    [SerializeField] private GameObject _timeTextPlaceholder;

    [Space]
    [Header("Prefabs")]
    [SerializeField] private SpeechItemScript _speechPrefab;
    [SerializeField] private SpeechItemScript _speechPrefabGeneral;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    public bool _inViewport;
    private List<SpeechItemScript> _speeches;
    private string[] _tags;

    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------

    public bool Empty { get; private set; }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Update()
    {
      UpdateChildren();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public TimeslotScript GetInstance(List<SpeechItemModel> speeches, string startTime, float width)
    {
      var instance = Instantiate(this);
      instance.SetStartTime(startTime);

      instance.Empty = true;

      var tsHeight = 170;

      instance._speeches = new List<SpeechItemScript>();
      
      foreach (var speech in speeches)
      {
        if (speech.Tag != "General")
        {
          var item = _speechPrefab.GetInstance(speech);
          item.transform.SetParent(instance.transform);
          (item.transform as RectTransform).sizeDelta =
            new Vector2(width, ITEM_HEIGHT_W_SPEAKER);
          tsHeight += ITEM_HEIGHT_W_SPEAKER;
          item.gameObject.SetActive(false);
          instance.Empty = false;
          instance._speeches.Add(item);
        }
        else
        {
          var item = _speechPrefabGeneral.GetInstance(speech);
          item.transform.SetParent(instance.transform);
          (item.transform as RectTransform).sizeDelta =
            new Vector2(width, ITEM_HEIGHT_W_O_SPEAKER);
          tsHeight += ITEM_HEIGHT_W_O_SPEAKER;
          item.gameObject.SetActive(false);
          instance.Empty = false;
          instance._speeches.Add(item);
        }
      }

      (instance.transform as RectTransform).sizeDelta = new Vector2(0, tsHeight);

      return instance;
    }

    public void SetTags(string[] tags)
    {
      if (_speeches == null) return;

      _tags = tags;

      if (tags.Length == 0)
      {
        EnableAll();
        
        return;
      }

      bool empty = true;
      
      var tsHeight = 170;
      
      foreach (var s in _speeches)
      {
        if (tags.Contains(s.Tag))
        {
          empty = false;
          s.gameObject.SetActive(true);
          tsHeight += s.Tag == "General" ? ITEM_HEIGHT_W_O_SPEAKER : ITEM_HEIGHT_W_SPEAKER;
        }
        else
        {
          s.gameObject.SetActive(false);
        }
      }
      
      (transform as RectTransform).sizeDelta = new Vector2(0, empty ? 0 : tsHeight);
      _timeTextPlaceholder.SetActive(!empty);
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void SetStartTime(string startTimeText)
    {
      _startTimeHoursText.text = startTimeText.Split(':')[0];
      _startTimeMinutesText.text = startTimeText.Split(':')[1];
    }

    private void UpdateChildren()
    {
      var isInViewport = IsInViewport();

      if (_inViewport == isInViewport) return;
      
      _inViewport = isInViewport;
      
      SetChildrenActive();
    }

    private bool IsInViewport()
    {
      var parent = transform.parent as RectTransform;
      var current = transform as RectTransform;

      if (parent == null || current == null) return false;

      return parent.anchoredPosition.y + VIEWPORT_HEIGHT + current.anchoredPosition.y > 0
             && current.rect.height - current.anchoredPosition.y > parent.anchoredPosition.y;
    }

    private void SetChildrenActive()
    {
      if (_tags == null || _tags.Length == 0)
      {
        EnableAll();
        
        return;
      }
      
      foreach (var s in _speeches)
      {
        s.gameObject.SetActive(_tags.Contains(s.Tag) && _inViewport);
        if (!s.ImageLoaded) s.LoadSpeakerPhoto();
      }
    }

    private void EnableAll()
    {
      var tsHeight = 170;
      
      foreach (var s in _speeches)
      {
        s.gameObject.SetActive(_inViewport); 
        tsHeight += s.Tag == "General" ? ITEM_HEIGHT_W_O_SPEAKER : ITEM_HEIGHT_W_SPEAKER;
        if (!s.ImageLoaded) s.LoadSpeakerPhoto();
      }
      
      (transform as RectTransform).sizeDelta = new Vector2(0, tsHeight);
      _timeTextPlaceholder.SetActive(true);
    }
  }
}