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
		[SerializeField] private NavigationTargets _position;
		[SerializeField] private ARManager _arManager;
		[SerializeField] private string _prefabPath;

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private GameObject _environment;
		private PositionalDeviceTracker _positionalDeviceTracker;
		private bool _arCoreSupport;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		protected override void Start()
		{
			base.Start();

			_arCoreSupport = ARCoreHelper.CheckArCoreSupport();
		}

		protected override void OnTrackingFound()
		{
			MoveEnvironment();
			EnableEnvironment(true);
			ResolveNavigation(_position);
			EnableDeviceTracker(false);
			
			_trackableFound.Raise();
		}

		protected override void OnTrackingLost()
		{
			_trackableLost.Raise();
			EnableEnvironment(false);
			EnableDeviceTracker(true);
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private void MoveEnvironment()
		{
			if (_environment == null)
			{
				_environment = _arManager.Environment != null ? _arManager.Environment : InstantiateEnvironment();
			}

			_environment.transform.SetParent(transform, false);
		}

		private GameObject InstantiateEnvironment()
		{
			var prefab = Resources.Load<GameObject>(_prefabPath);
			var environment = Instantiate(prefab, transform);
			_arManager.Environment = environment;
			
			return environment;
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
			if (!_arCoreSupport) return;
			
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

		private void EnableEnvironment(bool value)
		{
			if (_environment == null) return;
			
			_environment.SetActive(value);
		}
	}
}
