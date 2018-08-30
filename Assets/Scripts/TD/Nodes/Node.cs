using UnityEngine;

namespace ua.org.gdg.devfest
{
	[RequireComponent(typeof(Collider))]
	public class Node : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private Node _nextNode;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------
		
		private void OnTriggerEnter(Collider other)
		{
			var agent = other.gameObject.GetComponent<Agent>();
			if (agent != null)
			{
				agent.GetNextNode(this);
			}
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public Node GetNextNode()
		{
			return _nextNode;
		}
	}
}