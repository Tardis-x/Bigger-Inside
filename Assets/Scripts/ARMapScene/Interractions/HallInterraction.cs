using System;
using System.Collections.Generic;
using ProBuilder2.Common;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

namespace ua.org.gdg.devfest
{
	public class HallInterraction : InterractibleObject
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
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public override void Interract()
		{
			// Show schedule
			_listScript.Enable();
			
			_api.Request(_hall, 0);
			
//			// Init content
//			_content = new List<RectTransform>();
//			
//			// Start/end time (tmp)
//			TimeSpan now = DateTime.Now.TimeOfDay;
//			TimeSpan inAnHour = DateTime.Now.AddHours(1).TimeOfDay;
//
//			string startTime = "Start: " + now.Hours + ":" + now.Minutes;
//			string endTime = "End: " + inAnHour.Hours + ":" + inAnHour.Minutes;
//			string date = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
//			
//			for (int i = 0; i < 40; i++)
//			{
//				RectTransform show = _showScript.GetInstance(startTime, endTime, _user.DisplayName);
//        _content.Add(show);
//			}
//			
//			
//			
//			// Add content to list
//			_listScript.AddContent(_content);
			
			// Set highlighter color to selected
			_highlightRenderer.material.mainTexture = _selectedTexture;
		}

		public override void Disable()
		{
			_highlightRenderer.material.mainTexture = _defaultTexture;
			//_listScript.Disable();
		}
	}
}
