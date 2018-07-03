﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class ObjectClick : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private LayerMask _clicableObjects;
    [SerializeField] private LayerMask _uiLayer;
    [SerializeField] private RectTransform _scrollableList;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private bool _fingerMoved;
    private ScrollableListScript _listScript;
    private InterractibleObject _lastInterracted;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _listScript = _scrollableList.GetComponent<ScrollableListScript>();
    }

    // Update is called once per frame
    void Update()
    {
      Touch touch = new Touch();

      try
      {
        touch = Input.GetTouch(0);
      }
      catch
      {
        return;
      }

      if (touch.phase == TouchPhase.Began) _fingerMoved = false;

      if (touch.phase == TouchPhase.Moved) _fingerMoved = true;

      if (touch.phase == TouchPhase.Ended && !_fingerMoved)
      {
        var ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, _clicableObjects))
        {
          {
            Debug.Log("Object hit");
            InterractibleObject
              obj = hit.transform.gameObject.GetComponent<InterractibleObject>(); //get interraction

            if (obj != null) //if obj is interractible
            {
              if(_lastInterracted != null) _lastInterracted.Disable();
              _lastInterracted = obj;
              Debug.Log("Interraction started");
              obj.Interract(); 
            }
          }
        }

        _uiLayer = ~_uiLayer;
        
        // If ray casts not on list
        if (!Physics.Raycast(ray, _uiLayer))
        {
          // Hide list and clear it
          _listScript.Disable();
          if(_lastInterracted != null) _lastInterracted.Disable();
        }
      }
    }
  }
}