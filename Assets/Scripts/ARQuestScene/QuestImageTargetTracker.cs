using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class QuestImageTargetTracker : MonoBehaviour, ITrackableEventHandler
{
	TrackableBehaviour _trackableBehaviour;

	[SerializeField]
	UnityEvent _onDetectImageTarget;
	[SerializeField]
	UnityEvent _onLostImageTarget;
	
	void Start () 
	{
		Debug.Log("QuestImageTargetTracker.Start");
		
		_trackableBehaviour = GetComponent<TrackableBehaviour>();
		
		if (_trackableBehaviour)
		{
			_trackableBehaviour.RegisterTrackableEventHandler(this);
		}
	}

	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			Debug.Log("QuestImageTargetTracker.OnTrackableStateChanged - Image found!");
			_onDetectImageTarget.Invoke();
		}
		else
		{
			Debug.Log("QuestImageTargetTracker.OnTrackableStateChanged - Image lost!");
			//_onLostImageTarget.Invoke();
		}
	}
}
