using ua.org.gdg.devfest;
using UnityEngine;

public class SpeakerAnimationScript : Singleton<SpeakerAnimationScript>
{
	[SerializeField] private Animator _animator;

	public void WalkAnotherWay()
	{
		_animator.SetBool("WalkOneWay", !_animator.GetBool("WalkOneWay"));
	}
}
