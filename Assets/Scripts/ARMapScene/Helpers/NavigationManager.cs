using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ua.org.gdg.devfest
{
  public class NavigationManager : MonoBehaviour
  {
    [SerializeField] private ScrollableListScript _list;
    [SerializeField] private DescriptionPanelScript _descriptionPanel;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      CurrentState = State.Map;
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.anyKey)
      {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          switch (CurrentState)
          {
            case State.Map:
              SceneManager.LoadScene("MenuScene");
              break;
            case State.List:
              _list.Disable();
              CurrentState = State.Map;
              break;
            case State.ListItem:
              CurrentState = State.List;
              _descriptionPanel.SetActive(false);
              break;
          }
        }
      }
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    public State CurrentState;

    //---------------------------------------------------------------------
    // Nested
    //---------------------------------------------------------------------

    public enum State
    {
      Map,
      List,
      ListItem
    }
  }
}