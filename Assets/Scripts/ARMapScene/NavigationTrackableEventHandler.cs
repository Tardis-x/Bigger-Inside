using UnityEngine;
using Vuforia;

namespace ua.org.gdg.devfest
{
	public class NavigationTrackableEventHandler : DefaultTrackableEventHandler
	{

		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[Header("Events")]
		[SerializeField] private GameEvent _trackableFound;
		[SerializeField] private GameEvent _trackableLost;
		
		[Space]
		[SerializeField] private string _prefabPath;
		[SerializeField] private NavigationTargets _position;

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private GameObject _environment;
		private PositionalDeviceTracker _positionalDeviceTracker;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		protected override void OnTrackingFound()
		{
			if (_environment != null) return;

			InstantiateEnvironment();
			ResolveNavigation(_position);
			EnableDeviceTracker(false);
			
			_trackableFound.Raise();
		}

		protected override void OnTrackingLost()
		{
			if (_environment != null)
			{
				Destroy(_environment);
			}
			
			_trackableLost.Raise();
			
			EnableDeviceTracker(true);
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private void InstantiateEnvironment()
		{
			var prefab = Resources.Load<GameObject>(_prefabPath);
			_environment = Instantiate(prefab, transform);
		}

		private void ResolveNavigation(NavigationTargets position)
		{
			var navigationResolver = _environment.GetComponent<NavigationResolver>();
			if (navigationResolver != null)
			{
				navigationResolver.SetupNavigationTarget(position);
			}
		}

		private void EnableDeviceTracker(bool value)
		{
			if (_positionalDeviceTracker == null)
			{
				_positionalDeviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
			}

			if (value)
			{
				_positionalDeviceTracker.Start();
			}
			else
			{
				_positionalDeviceTracker.Stop();
			}
		}
	}
}
