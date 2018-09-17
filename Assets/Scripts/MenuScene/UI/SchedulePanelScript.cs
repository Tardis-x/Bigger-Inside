﻿using System.Collections.Generic;
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
    [SerializeField] private GameObject _day1Underscore;
    [SerializeField] private GameObject _day2Underscore;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private Text _hallName;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private float _contentWidth;

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
        _showMenu.Raise();
    }
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

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
      _day1Underscore.SetActive(day == 1);
      _day2Underscore.SetActive(day == 2);
    }

    public void SetContent(int day)
    {
      List<TimeslotModel> listContent;
      if (!FirestoreManager.Instance.RequestFullSchedule(day, out listContent)) return;

      foreach (var item in listContent)
      {
        var contentItem = _timeslot.GetInstance(item.Items, item.StartTime, _canvas.rect.width);

        if (!contentItem.Empty) AddContentItem(contentItem);
        else Destroy(contentItem.gameObject);
      }
    }

    public void SetContent(int day, string hall)
    {
      List<TimeslotModel> listContent;
      if (!FirestoreManager.Instance.RequestFullSchedule(day, hall, out listContent)) return;

      foreach (var item in listContent)
      {
        var contentItem = _timeslot.GetInstance(item.Items, item.StartTime, _canvas.rect.width);

        if (!contentItem.Empty) AddContentItem(contentItem);
        else Destroy(contentItem.gameObject);
      }
    }

    public void EnablePanel(int day)
    {
      gameObject.SetActive(true);
      SetButtonsUnderscore(day);
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      Active = true;
      ClearContent();
      SetContent(day);
      _hallName.text = "Schedule";
    }

    public void EnablePanel(int day, string hall)
    {
      gameObject.SetActive(true);
      SetButtonsUnderscore(day);
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      Active = true;
      ClearContent();
      SetContent(day, hall);
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
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

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