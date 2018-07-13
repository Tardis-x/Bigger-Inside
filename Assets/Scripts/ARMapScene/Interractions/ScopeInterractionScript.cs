using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class ScopeInterractionScript : MonoBehaviour
	{
		[SerializeField] private RectTransform _scope; //coordinates of focus scope
		[SerializeField] private float _interractionTime; //time needed to interract with object in focus
		[SerializeField] private Image _scopeProgerssBar;

		// Update is called once per frame
		void Update()
		{
			Debug.Log("Interraction script started");
			float timeFocused = 0f; //time of focusing on current obj


			var ray = Camera.main.ScreenPointToRay(_scope.position); //cast ray from scope
			RaycastHit hit = new RaycastHit();

			if (Physics.Raycast(ray, out hit)) //if hit
			{
				Debug.Log("Object hit");
				InteractableObject obj = hit.transform.gameObject.GetComponent<InteractableObject>(); //get interraction

				if (obj != null) //if obj is interractible
				{
					Debug.Log("Interraction started");
					timeFocused += Time.deltaTime; //start counting focus time
					_scopeProgerssBar.fillAmount = timeFocused / _interractionTime;

					if (timeFocused >= _interractionTime) obj.Interact(); //if focused long enough - interract

					return; // else - leave
				}

				timeFocused = 0f; // if lost focus - start counting again
			}
		}
	}
}
