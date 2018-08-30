using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class Particles : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private ParticleSystem _stars;

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void StarBurst()
		{
			_stars.Play();
		}
	}
}