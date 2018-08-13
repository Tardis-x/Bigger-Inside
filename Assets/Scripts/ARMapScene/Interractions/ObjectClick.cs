using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class ObjectClick : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private LayerMask _clicableObjects;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private bool _fingerMoved;
    private InteractableObject _lastInterracted;
    
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public bool IsInteractable { get; set; }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
      if(!IsInteractable) return;
      
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
            InteractableObject obj = hit.transform.gameObject.GetComponent<InteractableObject>(); //get interraction

            if (obj != null) //if obj is interactable
            {
              if (_lastInterracted != null) _lastInterracted.Disable();
              _lastInterracted = obj;
              obj.Interact();
            }
          }
        }
      }
    }
  }
}