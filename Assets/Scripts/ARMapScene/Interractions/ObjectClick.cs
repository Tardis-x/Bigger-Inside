using System.Collections;
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
    [SerializeField] private NavigationManager _navigationManager;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private bool _fingerMoved;
    private ScrollableListScript _listScript;
    private InteractableObject _lastInterracted;

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
            InteractableObject
              obj = hit.transform.gameObject.GetComponent<InteractableObject>(); //get interraction

            if (obj != null) //if obj is interactable
            {
              if(_lastInterracted != null) _lastInterracted.Disable();
              _lastInterracted = obj;
              _navigationManager.CurrentState = obj.NavigationStateAfterInterraction;
              Debug.Log("Interraction started");
              if(!_listScript.IsActive) obj.Interact(); 
            }
          }
        }

        //_uiLayer = ~_uiLayer;
        
         //If ray casts not on list
//        if (!Physics.Raycast(ray, _uiLayer))
//        {
//          // Hide list and clear it
//          if (_lastInterracted != null)
//          {
//            _lastInterracted.Disable();
//          }
//        }
      }
    }
  }
}