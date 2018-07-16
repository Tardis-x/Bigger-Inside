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

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _panelManagerInstance = PanelManager.Instance;
      _speechDescriptionPanel = _panelManagerInstance.SpeechDescriptionPanel;
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape) && !_speechDescriptionPanel.Active) DisablePanel();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public bool IsActive { get; private set; }

    public void SetContentForHall(string hall)
    {
      List<ScheduleItemUiModel> listContent;
      if (!FirebaseManager.Instance.RequestHallSchedule(hall, out listContent)) return;

      foreach (var item in listContent)
      {
        AddContentItem(_speechScript.GetInstance(item));
      }
    }

    public void AddContentItem(SpeechScript contentItem)
    {
      contentItem.GetComponent<RectTransform>().SetParent(_contentContainer, false);
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

    public void DisablePanel()
    {
      IsActive = false;
      ClearContent();
      gameObject.SetActive(false);
    }

    public void EnablePanel()
    {
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      IsActive = true;
      ClearContent();
      gameObject.SetActive(true);
    }
  }
}