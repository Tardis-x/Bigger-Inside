using System;
using System.Collections.Generic;
using ProBuilder2.Common;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class HallInterraction : InterractibleObject
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private RectTransform _schedule;
		[SerializeField] private RectTransform _show;

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private ScrollableListScript _listScript;
		private List<RectTransform> _content;
		private ShowScript _showScript;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Start()
		{
			_listScript = _schedule.GetComponent<ScrollableListScript>();
			_showScript = _show.GetComponent<ShowScript>();
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public override void Interract()
		{
			_listScript.Enable();
			
			_content = new List<RectTransform>();

			TimeSpan now = DateTime.Now.TimeOfDay;
			TimeSpan inAnHour = DateTime.Now.AddHours(1).TimeOfDay;

			string startTime = "Start: " + now.Hours + ":" + now.Minutes;
			string endTime = "End: " + inAnHour.Hours + ":" + inAnHour.Minutes;
			string date = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
			
			for (int i = 0; i < 40; i++)
			{
				RectTransform show = _showScript.GetInstance(startTime, endTime, date);
        _content.Add(show);
			}
			
			_listScript.AddContent(_content);
		}
	}
}
