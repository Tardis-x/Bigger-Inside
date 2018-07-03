using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{

	public class AnimationInterraction : InterractibleObject
	{

		private Animator _animator;

		void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		public override void Interract()
		{
			_animator.enabled = !_animator.enabled;
		}

		public override void Disable()
		{
			throw new NotImplementedException();
		}
	}
}
