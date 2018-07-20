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
		[SerializeField] private string _hall;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public override void Interact()
		{
			if (!PanelManager.Instance.SchedulePanel.Active)
			{
				PanelManager.Instance.SchedulePanel.EnablePanel();
				PanelManager.Instance.SchedulePanel.SetContentForHall(_hall);
			}
		}

		public override void Disable()
		{
			_highlightRenderer.material.mainTexture = _defaultTexture;
		}
	}
}
