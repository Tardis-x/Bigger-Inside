using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class SchedulePanelScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private DescriptionPanelScript _descriptionPanel;
    [SerializeField] private RectTransform _contentContainer;
    [SerializeField] private TimeslotScript _timeslot;
    [SerializeField] private GameEvent _showMenu;
    [SerializeField] private Animator _underscore;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private Text _hallName;
    [SerializeField] private GameEvent _showLoading;
    [SerializeField] private GameEvent _dismissLoading;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private float _contentWidth;
    private List<TimeslotScript> _day1, _day2;
    private List<string> _tags = new List<string>();
    private bool _tagsPanelOpen;
    private int _currentDay;

    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------

    public bool Active { get; private set; }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Update()
    {
      if (!Input.GetKeyDown(KeyCode.Escape) || !Active || _descriptionPanel.Active) return;
      
      DisablePanelWithDelay();
      _showMenu.Raise();
      _dismissLoading.Raise();
    }

    private void OnEnable()
    {
      _showLoading.Raise();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void IncludeTag(string tag)
    {
      if (!_tags.Contains(tag))
      {
        _tags.Add(tag);
        if (tag == "Other")
        {
          _tags.Add("Workshop");
          _tags.Add("General");
        }
      }
      else
      {
        _tags.Remove(tag);
        if (tag == "Other")
        {
          _tags.Remove("Workshop");
          _tags.Remove("General");
        }
      }

      FilterByTags(_tags.ToArray());
    }

    public void DisablePanel()
    {
      Active = false;
      ClearContent();
      gameObject.SetActive(false);
    }

    public void DisablePanelWithDelay()
    {
      Invoke("DisablePanel", .1f);
    }

    public void OnBackButtonClick()
    {
      DisablePanel();
      _showMenu.Raise();
    }

    public void SetButtonsUnderscore(int day)
    {
      _underscore.SetBool("Day1", day == 1);
    }

    public void EnablePanel(int day)
    {
      gameObject.SetActive(true);
      Active = true;
      SetButtonsUnderscore(day);
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      _hallName.text = "Schedule";
      var timeslots = day == 1 ? _day1 : _day2;

      if (timeslots == null)
      {
        StartCoroutine(SetContentCoroutine(day));
      }
      else
      {
        FilterByDay(day);
      }
      
      FilterByTags(_tags.ToArray());
    }

    public void EnablePanel(int day, string hall)
    {
      gameObject.SetActive(true);
      Active = true;
      SetButtonsUnderscore(day);
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      _hallName.text = hall;
      var timeslots = day == 1 ? _day1 : _day2;

      if (timeslots == null)
      {
        StartCoroutine(SetContentCoroutine(day, hall));
      }
      else
      {
        FilterByDay(day);
      }

      FilterByTags(_tags.ToArray());
    }

    public void ClosePanelDelayed()
    {
      Invoke("ClosePanel", 0.1f);
    }

    public void ClosePanel()
    {
      Active = false;
      ClearContent();
      gameObject.SetActive(false);
    }

    public void ShowTags()
    {
      _tagsPanelOpen = !_tagsPanelOpen;

      if (_tagsPanelOpen || _tags.Count == 0) return;

      _tags.Clear();
      FilterByTags(_tags.ToArray());
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private IEnumerator SetContentCoroutine(int day)
    {
      var timeslots = day == 1 ? _day1 : _day2;

      List<TimeslotModel> listContent;
      
      while (true)
      {
        yield return new WaitForSeconds(.01f);
        if(FirestoreManager.Instance.RequestFullSchedule(day, out listContent)) break;
        
        if (FirestoreManager.Instance.Error)
        {
          _dismissLoading.Raise();
          break;
        }
      }

      if (!FirestoreManager.Instance.Error)
      {
        timeslots = new List<TimeslotScript>();

        foreach (var item in listContent)
        {
          var contentItem = _timeslot.GetInstance(item.Items, item.StartTime, _canvas.rect.width);

          if (!contentItem.Empty)
          {
            AddContentItem(contentItem);
            timeslots.Add(contentItem);
          }
          else Destroy(contentItem.gameObject);
        }

        SaveDaysTimeslots(day, timeslots);
        FilterByDay(day);
        _dismissLoading.Raise();
      }

      yield return null;
    }

    private IEnumerator SetContentCoroutine(int day, string hall)
    {
      var timeslots = day == 1 ? _day1 : _day2;

      List<TimeslotModel> listContent;
      
      while (true)
      {
        yield return new WaitForSeconds(.01f);
        if (FirestoreManager.Instance.RequestFullSchedule(day, hall, out listContent)) break;
        
        if (FirestoreManager.Instance.Error)
        {
          _dismissLoading.Raise();
          break;
        }
      }

      if (!FirestoreManager.Instance.Error)
      {
        timeslots = new List<TimeslotScript>();

        foreach (var item in listContent)
        {
          var contentItem = _timeslot.GetInstance(item.Items, item.StartTime, _canvas.rect.width);

          if (!contentItem.Empty)
          {
            AddContentItem(contentItem);
            timeslots.Add(contentItem);
          }
          else Destroy(contentItem.gameObject);
        }

        SaveDaysTimeslots(day, timeslots);
        FilterByDay(day);
        _dismissLoading.Raise();
      }

      yield return null;
    }

    private void FilterByTags(string[] tags)
    {
      FilterDayByTags(_day1, tags);
      FilterDayByTags(_day2, tags);

      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
    }

    private void FilterDayByTags(List<TimeslotScript> day, string[] tags)
    {
      if (day == null) return;
      
      foreach (var ts in day)
      {
        ts.SetTags(tags);
      }
    }

    private void FilterByDay(int day)
    {
      if (_day1 != null)
      {
        foreach (var ts in _day1)
        {
          ts.gameObject.SetActive(day == 1);
        }
      }

      if (_day2 == null) return;

      foreach (var ts in _day2)
      {
        ts.gameObject.SetActive(day == 2);
      }
    }

    private void SaveDaysTimeslots(int day, List<TimeslotScript> timeslots)
    {
      if (day == 1)
      {
        _day1 = timeslots;
      }
      else
      {
        _day2 = timeslots;
      }
    }

    private void ClearContent()
    {
      foreach (Transform item in _contentContainer.transform)
      {
        Destroy(item.gameObject);
      }

      _day1 = null;
      _day2 = null;
    }

    private void AddContentItem(TimeslotScript contentItem)
    {
      contentItem.GetComponent<RectTransform>().SetParent(_contentContainer, false);
    }
  }
}