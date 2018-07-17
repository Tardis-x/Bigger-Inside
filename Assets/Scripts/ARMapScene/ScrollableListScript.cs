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

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private float _contentWidth;
    
    /// <summary>
    /// Clear list content.
    /// </summary>
    private void ClearContent()
    {
      var items = _contentContainer.GetComponentsInChildren<RectTransform>().Where(x => x.parent == _contentContainer);

      foreach (var item in items)
      {
        Destroy(item.gameObject);
      }
    }

    /// <summary>
    /// Clear panel and set active to false.
    /// </summary>
    private void DisablePanel()
    {
      Active = false;
      ClearContent();
      gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Add chedule item to list.
    /// </summary>
    /// <param name="contentItem">Schedule item.</param>
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

    /// <summary>
    /// Panel avtive state.
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// Set panel content depending on hall.
    /// </summary>
    /// <param name="hall">Hall name.</param>
    public void SetContentForHall(string hall)
    {
      List<ScheduleItemUiModel> listContent;
      if (!FirebaseManager.Instance.RequestHallSchedule(hall, out listContent)) return;

      foreach (var item in listContent)
      {
        AddContentItem(_speechScript.GetInstance(item));
      }
    }

    /// <summary>
    /// Clear panel and set it active.
    /// </summary>
    public void EnablePanel()
    {
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      Active = true;
      ClearContent();
      gameObject.SetActive(true);
    }
  }
}