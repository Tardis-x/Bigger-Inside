using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class MissileScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private Missile _missile;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public MissileScript GetInstance()
		{
			return Instantiate(this);
		}
		
		//---------------------------------------------------------------------
		// Properties
		//---------------------------------------------------------------------

		public int Damage
		{
			get { return _missile.Damage.Value; }
		}

		public MissileType Type
		{
			get { return _missile.Type; }
		}
	}
}