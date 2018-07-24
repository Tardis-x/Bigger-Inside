using ua.org.gdg.devfest;
using UnityEngine;

namespace ua.org.gdg.devfest
{

	public class SpeakerAnimationScript : Singleton<SpeakerAnimationScript>
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private Animator _animator;
		[SerializeField] private Transform _transform;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Awake()
		{
			_zPosition = _transform.localPosition.z;
		}

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void WalkAnotherWay()
		{
			_animator.SetBool("WalkOneWay", !_animator.GetBool("WalkOneWay"));
			ResetPosition();
			ResetRotation();
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private float _zPosition;
		
		private void ResetPosition()
		{
			_transform.localPosition = new Vector3(_transform.localPosition.x, transform.localPosition.y, _zPosition);
		}

		private void ResetRotation()
		{
			_transform.Rotate(Vector3.up, -transform.eulerAngles.y + 180);
		}
	}
}