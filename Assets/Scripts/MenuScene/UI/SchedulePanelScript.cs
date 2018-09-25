using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private float _contentWidth;
    private List<TimeslotModel> _timeslots;
    private List<string> _tags = new List<string>();
    private bool _tagsPanelOpen;

    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------
    
    public bool Active { get; private set; }
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape) && Active && !_descriptionPanel.Active)
      {
        Invoke("DisablePanel", .1f);
        _showMenu.Raise();
      }
    }
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void IncludeTag(string tag)
    {
      if (!_tags.Contains(tag))
      {
        _tags.Add(tag);
      }
      else
      {
        _tags.Remove(tag);
      }
      
      FilterByTags(_tags.ToArray());
    }
    
    public void DisablePanel()
    {
      Active = false;
      ClearContent();
      gameObject.SetActive(false);
    }

    public void OnBackButtonClick()
    {
      _showMenu.Raise();
      DisablePanel();
    }

    public void SetButtonsUnderscore(int day)
    {
      _underscore.SetBool("Day1", day == 1);
    }

    public void SetContent(int day)
    {
      List<TimeslotModel> listContent;
      if (!FirestoreManager.Instance.RequestFullSchedule(day, out listContent)) return;

      _timeslots = new List<TimeslotModel>();
      
      foreach (var item in listContent)
      {
       // var contentItem = _timeslot.GetInstance(item.Items, item.StartTime, _canvas.rect.width);

        _timeslots.Add(item);
        
       // if (!contentItem.Empty) AddContentItem(contentItem);
       // else Destroy(contentItem.gameObject);
      }
    }

    public void SetContent(int day, string hall)
    {
      List<TimeslotModel> listContent;
      if (!FirestoreManager.Instance.RequestFullSchedule(day, hall, out listContent)) return;

      _timeslots = new List<TimeslotModel>();
      
      foreach (var item in listContent)
      {
        //var contentItem = _timeslot.GetInstance(item.Items, item.StartTime, _canvas.rect.width);

        _timeslots.Add(item);
        
        //if (!contentItem.Empty) AddContentItem(contentItem);
        //else Destroy(contentItem.gameObject);
      }
    }

    public void EnablePanel(int day)
    {
      gameObject.SetActive(true);
      Active = true;
      SetButtonsUnderscore(day);
      StartCoroutine(EnableCoroutine(day));
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      _hallName.text = "Schedule";
    }

    public void EnablePanel(int day, string hall)
    {
      gameObject.SetActive(true);
      Active = true;
      SetButtonsUnderscore(day);
      StartCoroutine(EnableCoroutine(day, hall));
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      _hallName.text = hall;
    }

    public void ClosePanelDelayed()
    {
      Invoke("ClosePanel", 0.1f);
    }
 
    public void ClosePanel()
    {
      Active = false;
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

    private IEnumerator EnableCoroutine(int day)
    {
      ClearContent();
      SetContent(day);
      FilterByTags(_tags.ToArray());
      yield return null;
    }
    
    private IEnumerator EnableCoroutine(int day, string hall)
    {
      ClearContent();
      SetContent(day, hall);

      FilterByTags(_tags.ToArray());
      yield return null;
    }
    
    private void FilterByTags(string[] tags)
    {
      if(_timeslots == null) return;
      
      ClearContent();
      
      foreach (var ts in _timeslots)
      {
        var contentItem = tags.Length == 0 ? _timeslot.GetInstance(ts.Items, ts.StartTime, _canvas.rect.width) : 
          _timeslot.GetInstance(ts.Items, ts.StartTime, _canvas.rect.width, tags);
        
        if (!contentItem.Empty) AddContentItem(contentItem);
        else Destroy(contentItem.gameObject);
      }
    }
    
    private void ClearContent()
    {
      var items = _contentContainer.GetComponentsInChildren<RectTransform>().Where(x => x.parent == _contentContainer);

      foreach (var item in items)
      {
        Destroy(item.gameObject);
      }
    }

    private void AddContentItem(TimeslotScript contentItem)
    {
      contentItem.GetComponent<RectTransform>().SetParent(_contentContainer, false);
    }
  }
}