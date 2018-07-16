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

    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private RectTransform _contentContainer;

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
      Hide();
      _panelManagerInstance = PanelManager.Instance;
      _speechDescriptionPanel = _panelManagerInstance.SpeechDescriptionPanel;
    }
    
    private void Update()
    {
      if(Input.GetKeyDown(KeyCode.Escape) && !_speechDescriptionPanel.Active) Disable();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public bool IsActive { get; private set; }
    
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
  }
}