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

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private RectTransform _contentContainer;
    private float _contentWidth;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      var children = GetComponentsInChildren<RectTransform>();

      foreach (var child in children)
      {
        if (child.name != "ListContent") continue;
        _contentContainer = child;
        return;
      }
      
      
    }

    private void Start()
    {
      Hide();
    }
    
    private void Update()
    {
      if(Input.GetKeyDown(KeyCode.Escape)) Disable();
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
      GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
      IsActive = true;
      ClearContent();
      gameObject.SetActive(true);
    }
  }
}