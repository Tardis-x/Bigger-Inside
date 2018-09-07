using System.Runtime.Serialization.Formatters;
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
    private InteractableObject _lastInteracted;

    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public bool IsInteractable { get; set; }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    void Update()
    {
      if (!IsInteractable) return;

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

        if (Physics.Raycast(ray, out hit, 100, _clicableObjects, QueryTriggerInteraction.Ignore))
        {
          InteractableObject obj = hit.transform.gameObject.GetComponent<InteractableObject>();

          if (obj != null)
          {
            if (_lastInteracted != null) _lastInteracted.Disable();
            _lastInteracted = obj;
            obj.Interact();
          }
        }
      }
    }
  }
}