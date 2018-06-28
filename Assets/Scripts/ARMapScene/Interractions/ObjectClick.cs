using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectClick : MonoBehaviour
{
    private bool _fingerMoved;

    [SerializeField] private LayerMask _clicableObjects;

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
                        Debug.Log("Interraction started");
                        obj.Interract(); //if focused long enough - interract
                    }
                }
            }
        }
    }
}