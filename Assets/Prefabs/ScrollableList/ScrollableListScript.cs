using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class ScrollableListScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    
    [SerializeField] private RectTransform _contentContainer;
    [SerializeField] private SpeechScript _speechScript;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private float _contentWidth;
    private PanelManager _panelManagerInstance;
    private DescriptionPanelScript _speechDescriptionPanel;
    private FirebaseManager _firebaseManager;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      Hide();
      _panelManagerInstance = PanelManager.Instance;
      _speechDescriptionPanel = _panelManagerInstance.SpeechDescriptionPanel;
      _firebaseManager = FirebaseManager.Instance;
    }
    
    private void Update()
    {
      if(Input.GetKeyDown(KeyCode.Escape) && !_speechDescriptionPanel.Active) Disable();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public bool IsActive { get; private set; }

    public void SetContentForHall(Hall hall)
    {
      ScheduleDay schedule;
      Dictionary<string, Speaker> speakers;
      List<SessionItem> sessions;
      _firebaseManager.RequestData(out schedule, out speakers, out sessions);
      AddContent(ComposeScheduleForHall(hall, schedule, sessions, speakers));
    }
    
    public void AddContentItem(RectTransform contentItem)
    {
      contentItem.SetParent(_contentContainer, false);
    }

    public void AddContent(List<SpeechScript> content)
    {
      foreach (var item in content)
      {
        item.GetComponent<RectTransform>().SetParent(_contentContainer, false);
      }
    }

    public void ClearContent()
    {
      var items = _contentContainer.GetComponentsInChildren<RectTransform>().Where(x => x.parent == _contentContainer);

      foreach (var item in items)
      {
        Destroy(item.gameObject);
      }
    }

    public void Hide()
    {
      IsActive = false;
      gameObject.SetActive(false);
    }

    public void Show()
    {
      GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
      IsActive = true;
      gameObject.SetActive(true);
    }

    public void Disable()
    {
      IsActive = false;
      ClearContent();
      gameObject.SetActive(false);
    }

    public void Enable()
    {
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      IsActive = true;
      ClearContent();
      gameObject.SetActive(true);
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
		
    private List<SpeechScript> ComposeScheduleForHall(Hall h, ScheduleDay day,
      List<SessionItem> sessions, Dictionary<string, Speaker> speakers)
    {
      // Get all sessions from schedule
      var sList = day.Timeslots.SelectMany(x => x.Sessions).Where(s => s.Hall == h).ToList();
      var result = new List<SpeechScript>();

      for (int i = 0; i < sList.Count; i++)
      {
        SessionItem s = sessions.Find(x => sList[i].Items.Contains(x.Name));
        result.Add(_speechScript.GetInstance(day.DateReadable, day.Timeslots[i].StartTime, 
          day.Timeslots[i].EndTime, s.Speakers.Count > 0 ? speakers[s.Speakers[0]] : null, s,
          HallToString(h)));
      }

      return result;
    }
		
    private static string HallToString(Hall h)
    {
      switch (h)
      {
        case Hall.Conference:
          return "Conference hall";
        case Hall.Expo:
          return "Expo hall";
        default:
          return "Workshops";
      }
    }
  }
}