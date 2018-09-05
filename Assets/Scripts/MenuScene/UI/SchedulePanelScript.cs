﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class SchedulePanelScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private RectTransform _contentContainer;
    [SerializeField] private TimeslotScript _timeslot;
    [SerializeField] private GameEvent _showMenu;
    [SerializeField] private GameObject _day1Underscore;
    [SerializeField] private GameObject _day2Underscore;
    [SerializeField] private RectTransform _canvas;

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
    
   
    private void AddContentItem(TimeslotScript contentItem)
    {
      contentItem.GetComponent<RectTransform>().SetParent(_contentContainer, false);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void OnBackButtonClick()
    {
      _showMenu.Raise();
      DisablePanel();
    }

    public void SetButtonsUnderscore(int day)
    {
      _day1Underscore.SetActive(day == 1);
      _day2Underscore.SetActive(day == 2);
    }

    public bool Active { get; private set; }

    public void SetContent(int day)
    {
      List<TimeslotModel> listContent;
      if (!FirebaseManager.Instance.RequestFullSchedule(day, out listContent)) return;

      foreach (var item in listContent)
      {
        AddContentItem(_timeslot.GetInstance(item.Items, item.StartTime, _canvas.rect.width));
      }
    }

    public void EnablePanel(int day)
    {
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      Active = true;
      ClearContent();
      SetContent(day);
      gameObject.SetActive(true);
    }
  }
}