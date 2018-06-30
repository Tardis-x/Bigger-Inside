using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class ScrollableListScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

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

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void AddContentItem(RectTransform contentItem)
    {
      contentItem.SetParent(_contentContainer, false);
    }

    public void AddContent(List<RectTransform> content)
    {
      foreach (var item in content)
      {
        item.SetParent(_contentContainer, false);
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
      gameObject.SetActive(false);
    }

    public void Show()
    {
      gameObject.SetActive(true);
    }

    public void Disable()
    {
      ClearContent();
      gameObject.SetActive(false);
    }

    public void Enable()
    {
      ClearContent();
      gameObject.SetActive(true);
    }
  }
}