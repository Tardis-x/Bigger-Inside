/*============================================================================== 
 Copyright (c) 2016-2017 PTC Inc. All Rights Reserved.
 
 Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/

using System.Collections;
using System.Net.Configuration;
using UnityEngine;
using Vuforia;


namespace ua.org.gdg.devfest
{
  public class VirtualButtonEventHandler : MonoBehaviour,
    IVirtualButtonEventHandler
  {
    //-----------------------------------------------
    // Editor
    //-----------------------------------------------

    [SerializeField] private Material m_VirtualButtonDefault;
    [SerializeField] private Material m_VirtualButtonPressed;
    [SerializeField] private float m_ButtonReleaseTimeDelay;
    
    //-----------------------------------------------
    // Properties
    //-----------------------------------------------
    
    public bool ButtonEnabled { get; private set; }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
      if(!ButtonEnabled) return;
      
      Debug.Log("OnButtonPressed: " + vb.VirtualButtonName);

      SetVirtualButtonMaterial(m_VirtualButtonPressed);

      StopAllCoroutines();
      _onClick.OnClick();
      BroadcastMessage("HandleVirtualButtonPressed", SendMessageOptions.DontRequireReceiver);
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
      if(!ButtonEnabled) return;
      
      Debug.Log("OnButtonReleased: " + vb.VirtualButtonName);

      SetVirtualButtonMaterial(m_VirtualButtonDefault);

      StartCoroutine(DelayOnButtonReleasedEvent(m_ButtonReleaseTimeDelay, vb.VirtualButtonName));
    }

    public void SetVirtualButtonMaterial(Material material)
    {
      if (material != null)
      {
        _virtualButtonBehaviour.GetComponent<MeshRenderer>().sharedMaterial = material;
      }
    }

    public void SetButtonEnabled(bool value)
    {
      ButtonEnabled = value;
    }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      // Register with the virtual buttons TrackableBehaviour
      _virtualButtonBehaviour = GetComponent<VirtualButtonBehaviour>();
      _virtualButtonBehaviour.RegisterEventHandler(this);
      _onClick = GetComponent<VirtualButtonOnClick>();
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private VirtualButtonBehaviour _virtualButtonBehaviour;
    private VirtualButtonOnClick _onClick;

    IEnumerator DelayOnButtonReleasedEvent(float waitTime, string buttonName)
    {
      yield return new WaitForSeconds(waitTime);

      BroadcastMessage("HandleVirtualButtonReleased", SendMessageOptions.DontRequireReceiver);
    }
  }
}