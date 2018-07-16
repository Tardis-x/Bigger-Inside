using System;
using System.Collections.Generic;
using ProBuilder2.Common;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

namespace ua.org.gdg.devfest
{
	public class HallInterraction : InteractableObject
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private Renderer _highlightRenderer;
		[SerializeField] private Texture _defaultTexture;
		[SerializeField] private Hall _hall;
		[SerializeField] private API _api;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Start()
		{
			_listScript = PanelManager.Instance.SchedulePanel;
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private ScrollableListScript _listScript;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public override void Interact()
		{
			// Show schedule
			_listScript.Enable();
			
			_api.Request(_hall, 0);
		}

		public override void Disable()
		{
			_highlightRenderer.material.mainTexture = _defaultTexture;
		}
	}
}
