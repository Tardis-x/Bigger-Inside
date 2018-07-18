/*============================================================================== 
 Copyright (c) 2016-2017 PTC Inc. All Rights Reserved.
 
 Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/

using System.Collections;
using UnityEngine;
using Vuforia;

/// <summary>
/// This class implements the IVirtualButtonEventHandler interface and
/// contains the logic to start animations depending on what 
/// virtual button has been pressed.
/// </summary>
public class VirtualButtonEventHandler : MonoBehaviour,
  IVirtualButtonEventHandler
{
  #region PUBLIC_MEMBERS

  public Material m_VirtualButtonDefault;
  public Material m_VirtualButtonPressed;
  public Renderer m_ResultButton;
  public float m_ButtonReleaseTimeDelay;

  #endregion // PUBLIC_MEMBERS

  #region PRIVATE_MEMBERS

  VirtualButtonBehaviour virtualButtonBehaviour;

  #endregion // PRIVATE_MEMBERS

  #region MONOBEHAVIOUR_METHODS

  void Start()
  {
    // Register with the virtual buttons TrackableBehaviour
    virtualButtonBehaviour = GetComponent<VirtualButtonBehaviour>();
    virtualButtonBehaviour.RegisterEventHandler(this);
  }

  #endregion // MONOBEHAVIOUR_METHODS


  #region PUBLIC_METHODS

  /// <summary>
  /// Called when the virtual button has just been pressed:
  /// </summary>
  public void OnButtonPressed(VirtualButtonBehaviour vb)
  {
    Debug.Log("OnButtonPressed: " + vb.VirtualButtonName);

    SetVirtualButtonMaterial(m_VirtualButtonPressed);
    m_ResultButton.sharedMaterial = m_VirtualButtonDefault;
    
    StopAllCoroutines();

    BroadcastMessage("HandleVirtualButtonPressed", SendMessageOptions.DontRequireReceiver);
  }

  /// <summary>
  /// Called when the virtual button has just been released:
  /// </summary>
  public void OnButtonReleased(VirtualButtonBehaviour vb)
  {
    Debug.Log("OnButtonReleased: " + vb.VirtualButtonName);

    SetVirtualButtonMaterial(m_VirtualButtonDefault);
    m_ResultButton.sharedMaterial = m_VirtualButtonDefault;

    StartCoroutine(DelayOnButtonReleasedEvent(m_ButtonReleaseTimeDelay, vb.VirtualButtonName));
  }

  #endregion //PUBLIC_METHODS


  #region PRIVATE_METHODS

  void SetVirtualButtonMaterial(Material material)
  {
    if (material != null)
    {
      virtualButtonBehaviour.GetComponent<MeshRenderer>().sharedMaterial = material;
    }
  }

  IEnumerator DelayOnButtonReleasedEvent(float waitTime, string buttonName)
  {
    yield return new WaitForSeconds(waitTime);

    BroadcastMessage("HandleVirtualButtonReleased", SendMessageOptions.DontRequireReceiver);
  }

  #endregion // PRIVATE METHODS
}