using System.Linq;
using System.Collections.Generic;
using UnityEngine;

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
			_listScript.SetContentForHall(_hall);
		}

		public override void Disable()
		{
			_highlightRenderer.material.mainTexture = _defaultTexture;
		}
		
		
	}
}
