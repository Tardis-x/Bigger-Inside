using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class TrackableEventHandler : DefaultTrackableEventHandler
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private GameEvent _onTrackingFound;
		[SerializeField] private GameEvent _onTrackingLost;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		protected override void OnTrackingLost()
		{
			base.OnTrackingLost();
			if(_onTrackingLost != null) _onTrackingLost.Raise();
		}

		protected override void OnTrackingFound()
		{
			base.OnTrackingFound();
			if (_onTrackingFound != null) _onTrackingFound.Raise();
		}
	}
}