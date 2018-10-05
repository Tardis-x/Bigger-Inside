using UnityEngine;
using Vuforia;

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
		// Internal
		//---------------------------------------------------------------------

		private bool _arCoreSupport;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		protected override void Start()
		{
			base.Start();
			
			_arCoreSupport = ARCoreHelper.CheckArCoreSupport();
		}

		protected override void OnTrackingLost()
		{
			base.OnTrackingLost();
			
			if (_onTrackingLost == null) return;

			if (_arCoreSupport)
			{
				if(!IsDeviceTrackerActive()) return;
			}
			
			_onTrackingLost.Raise();
		}

		protected override void OnTrackingFound()
		{
			base.OnTrackingFound();

			if (_onTrackingFound == null) return;

			_onTrackingFound.Raise();
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private bool IsDeviceTrackerActive()
		{
			var positionalDeviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
			
			return positionalDeviceTracker.IsActive;
		}
	}
}