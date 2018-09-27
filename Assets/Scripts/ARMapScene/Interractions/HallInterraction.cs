using UnityEngine;
using UnityEngine.EventSystems;

namespace ua.org.gdg.devfest
{
	public class HallInterraction : InteractableObject, IPointerClickHandler
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private string _hall;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void SetDay(int day)
		{
			_day = day;
			if(PanelManager.Instance.CurrentHall == _hall) PanelManager.Instance.SchedulePanelNew.EnablePanel(_day, _hall);
		}
		
		public override void Interact()
		{
			if (!PanelManager.Instance.SchedulePanelNew.Active)
			{
				PanelManager.Instance.CurrentHall = _hall;
				PanelManager.Instance.SchedulePanelNew.EnablePanel(2, _hall);
				PanelManager.Instance.SchedulePanelNew.EnablePanel(1, _hall);
			}
		}

		public override void Disable()
		{
			
		}
		
		public void OnPointerClick(PointerEventData eventData)
		{
			Interact();
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private int _day = 1;
	}
}
