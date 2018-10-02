﻿using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class NavigationTrackableEventHandler : DefaultTrackableEventHandler
	{

		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

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
			var prefab = Resources.Load<GameObject>(_prefabPath);
			_environment = Instantiate(prefab, transform);

			var navigationResolver = _environment.GetComponent<NavigationResolver>();
			navigationResolver.SetupNavigationTarget(_position);
		}

		protected override void OnTrackingLost()
		{
			if (_environment != null)
			{
				Destroy(_environment);
			}
		}
	}
}
