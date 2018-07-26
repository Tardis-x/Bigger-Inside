/*============================================================================== 
 Copyright (c) 2016-2017 PTC Inc. All Rights Reserved.
 
 Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/

using System.Collections;
using UnityEngine;
using Vuforia;


namespace ua.org.gdg.devfest
{
  /// <summary>
  /// This class implements the IVirtualButtonEventHandler interface and
  /// contains the logic to start animations depending on what 
  /// virtual button has been pressed.
  /// </summary>
  public class VirtualButtonEventHandler : MonoBehaviour,
    IVirtualButtonEventHandler
  {
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public Material m_VirtualButtonDefault;
    public Material m_VirtualButtonPressed;
    public float m_ButtonReleaseTimeDelay;

    /// <summary>
    /// Called when the virtual button has just been pressed:
    /// </summary>
    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
      Debug.Log("OnButtonPressed: " + vb.VirtualButtonName);

      SetVirtualButtonMaterial(m_VirtualButtonPressed);

      StopAllCoroutines();
      _onClick.OnClick();
      BroadcastMessage("HandleVirtualButtonPressed", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Called when the virtual button has just been released:
    /// </summary>
    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
      Debug.Log("OnButtonReleased: " + vb.VirtualButtonName);

      SetVirtualButtonMaterial(m_VirtualButtonDefault);

      StartCoroutine(DelayOnButtonReleasedEvent(m_ButtonReleaseTimeDelay, vb.VirtualButtonName));
    }

    public void RefreshMaterial()
    {
      SetVirtualButtonMaterial(m_VirtualButtonDefault);
    }
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    void Start()
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

    private void SetVirtualButtonMaterial(Material material)
    {
      if (material != null)
      {
        _virtualButtonBehaviour.GetComponent<MeshRenderer>().sharedMaterial = material;
      }
    }

    IEnumerator DelayOnButtonReleasedEvent(float waitTime, string buttonName)
    {
      yield return new WaitForSeconds(waitTime);

      BroadcastMessage("HandleVirtualButtonReleased", SendMessageOptions.DontRequireReceiver);
    }
  }
}