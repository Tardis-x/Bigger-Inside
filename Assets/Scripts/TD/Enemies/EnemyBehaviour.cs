using UnityEngine;
using UnityEngine.AI;

namespace ua.org.gdg.devfest
{
	public class EnemyBehaviour : MonoBehaviour
	{
		
		private int _destPoint = 0;
		private NavMeshAgent _agent;

		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private Transform[] _points;

		void Start()
		{
			_agent = GetComponent<NavMeshAgent>();

			// Disabling auto-braking allows for continuous movement
			// between points (ie, the agent doesn't slow down as it
			// approaches a destination point).
			_agent.autoBraking = false;

			GotoNextPoint();
		}


		void GotoNextPoint()
		{
			// Returns if no points have been set up
			if (_points.Length == 0)
				return;

			// Set the agent to go to the currently selected destination.
			_agent.destination = _points[_destPoint].position;

			// Choose the next point in the array as the destination,
			// cycling to the start if necessary.
			_destPoint = (_destPoint + 1) % _points.Length;
		}


		void Update()
		{
			// Choose the next destination point when the agent gets
			// close to the current one.
			if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
				GotoNextPoint();
		}
	}
}