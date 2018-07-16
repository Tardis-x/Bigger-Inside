using System;
using UnityEngine;

namespace ua.org.gdg.devfest
{

	public class AnimationInterraction : InteractableObject
	{

		private Animator _animator;

		void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		public override void Interact()
		{
			_animator.enabled = !_animator.enabled;
		}

		public override void Disable()
		{
			throw new NotImplementedException();
		}
	}
}
