using UnityEngine;

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

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		protected override void OnTrackingFound()
		{
			if (_environment != null) return;
			
			var prefab = Resources.Load<GameObject>(_prefabPath);
			_environment = Instantiate(prefab, transform);

			var navigationResolver = _environment.GetComponent<NavigationResolver>();
			if (navigationResolver != null)
			{
				navigationResolver.SetupNavigationTarget(_position);
			}
			
			_trackableFound.Raise();
		}

		protected override void OnTrackingLost()
		{
			if (_environment != null)
			{
				Destroy(_environment);
			}
			
			_trackableLost.Raise();
		}
	}
}
