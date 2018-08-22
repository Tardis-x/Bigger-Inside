using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace ua.org.gdg.devfest
{
	public class Agent : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private int _destPoint = 0;
		public Node _currentNode;
		private Vector3 _destination;

		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private NavMeshAgent _navMeshAgent;

		//---------------------------------------------------------------------
		// Property
		//---------------------------------------------------------------------

		public Node HappyExitNode { get; set; }

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Update()
		{
			var squareStoppingDistance = _navMeshAgent.stoppingDistance * _navMeshAgent.stoppingDistance;
			if (Vector3.SqrMagnitude(_destination - transform.position) < squareStoppingDistance &&
			    _currentNode.GetNextNode() != null)
			{
				GetNextNode(_currentNode);
			}
			else
			{
				_navMeshAgent.SetDestination(_destination);
			}
		}

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void GetNextNode(Node currentlyEnteredNode)
		{
			if (_currentNode != currentlyEnteredNode)
			{
				return;
			}

			if (_currentNode == null)
			{
				Debug.LogError("Cannot find current node");
				return;
			}

			Node nextNode = _currentNode.GetNextNode();

			if (nextNode == null)
			{
				_navMeshAgent.isStopped = true;
				GetComponent<EnemyScript>().Disappear();
				return;
			}

			SetNode(nextNode);
			MoveToNode();
		}

		public void SetNode(Node node)
		{
			_currentNode = node;
		}

		public void SetSpeed(float speed)
		{
			// Cannot set negative speed
			if (speed < 0) return;

			_navMeshAgent.speed = speed;
		}

		public void MoveToNode()
		{
			var nodePosition = _currentNode.transform.position;
			nodePosition.y = _currentNode.transform.position.y;
			_destination = nodePosition;
			NavigateTo(_destination);
		}

		public void Die()
		{
			SetNode(HappyExitNode);
			MoveToNode();
		}

		public void Initialize(Node startNode)
		{
			SetNode(startNode);
			MoveToNode();
		}

		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		private void NavigateTo(Vector3 nextPoint)
		{
			_navMeshAgent.SetDestination(nextPoint);
		}
	}
}