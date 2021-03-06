﻿using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class HitVButtonScript : VirtualButtonOnClick
	{
		[Header("Events")]
		[SerializeField] private GameEvent _onHit;
		
		public override void OnClick()
		{
			_onHit.Raise();
		}
	}
}