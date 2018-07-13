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

		[SerializeField] private RectTransform _schedule;
		[SerializeField] private RectTransform _show;
		[SerializeField] private Renderer _highlightRenderer;
		[SerializeField] private Texture _defaultTexture;
		[SerializeField] private Texture _selectedTexture;
		[SerializeField] private Hall _hall;
		[SerializeField] private API _api;
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private ScrollableListScript _listScript;
		private List<RectTransform> _content;
		private SpeechScript _speechScript;
		private FirebaseUser _user;
		
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Start()
		{
			_listScript = _schedule.GetComponent<ScrollableListScript>();
			_speechScript = _show.GetComponent<SpeechScript>();
			_user = FirebaseAuth.DefaultInstance.CurrentUser;
			NavigationStateAfterInterraction = NavigationManager.State.List;
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public override void Interact()
		{
			// Show schedule
			_listScript.Enable();
			
			_api.Request(_hall, 0);
			//_highlightRenderer.material.mainTexture = _selectedTexture;
		}

		public override void Disable()
		{
			_highlightRenderer.material.mainTexture = _defaultTexture;
			//_listScript.Disable();
		}
	}
}
