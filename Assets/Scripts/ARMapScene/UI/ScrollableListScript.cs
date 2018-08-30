using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Text _headerText;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private float _contentWidth;
    
    private void ClearContent()
    {
      var items = _contentContainer.GetComponentsInChildren<RectTransform>().Where(x => x.parent == _contentContainer);

      foreach (var item in items)
      {
        Destroy(item.gameObject);
      }
    }

    private void DisablePanel()
    {
      Active = false;
      ClearContent();
      gameObject.SetActive(false);
    }
    
   
    private void AddContentItem(SpeechScript contentItem)
    {
      contentItem.GetComponent<RectTransform>().SetParent(_contentContainer, false);
    }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape) && !PanelManager.Instance.SpeechDescriptionPanel.Active) DisablePanel();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void OnBackButtonClick()
    {
      Invoke("DisablePanel", .01f);
    }

    public bool Active { get; private set; }

    public void SetContentForHall(string hall)
    {
      List<ScheduleItemUiModel> listContent;
      if (!FirebaseManager.Instance.RequestHallSchedule(hall, out listContent)) return;

      foreach (var item in listContent)
      {
        AddContentItem(_speechScript.GetInstance(item));
      }
      
      SetListHeader(hall);
    }

    public void EnablePanel()
    {
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      Active = true;
      ClearContent();
      gameObject.SetActive(true);
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void SetListHeader(string header)
    {
      _headerText.text = header;
    }
  }
}