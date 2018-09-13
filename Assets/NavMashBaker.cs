using UnityEngine;
using UnityEngine.AI;

namespace ua.org.gdg.devfest
{
	public class NavMashBaker : MonoBehaviour
	{
		[SerializeField] private NavMeshSurface _navMeshSurface;

		// Use this for initialization
		public void OnTrackableFound()
		{
			_navMeshSurface.BuildNavMesh();
		}
	}
}