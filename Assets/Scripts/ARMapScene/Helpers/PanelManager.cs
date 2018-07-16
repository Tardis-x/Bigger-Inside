using System.Collections;
using System.Collections.Generic;
using ua.org.gdg.devfest;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
  //---------------------------------------------------------------------
  // Editor
  //---------------------------------------------------------------------

	public ScrollableListScript SchedulePanel;
	public DescriptionPanelScript SpeechDescriptionPanel;
	
	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------
	
	public static PanelManager DefaultInstance { get; private set; }

	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------
	
	private void Awake ()
	{
		if (DefaultInstance == null) DefaultInstance = this;
		else if(DefaultInstance != this) Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}
}
