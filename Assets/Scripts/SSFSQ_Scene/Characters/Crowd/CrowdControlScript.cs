using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ua.org.gdg.devfest
{

	public class CrowdControlScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private CharacterAnimationScript[] _characters;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Start()
		{
			_characters = GetComponentsInChildren<CharacterAnimationScript>();
		}

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public CharacterAnimationScript CurrentCharacter { get; private set; }
		
		public void AskQuestion()
		{
			CurrentCharacter = GetRandomCharacter();
			CurrentCharacter.StandUpAndAsk();
		}

		public void StartThrowing()
		{
			foreach (var character in _characters)
			{
				character.StartThrowing();
			}
		}
		
		public void StopThrowing()
		{
			foreach (var character in _characters)
			{
				character.StopThrowing();
			}
		}

		public void StartBeingScared()
		{
			foreach (var character in _characters)
			{
				character.StartBeingScared();
			}
		}

		public void StopBeingScared()
		{
			foreach (var character in _characters)
			{
				character.StopBeingScared();
			}
		}
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		private CharacterAnimationScript GetRandomCharacter()
		{
			int index = (int) Math.Floor(Random.value * _characters.Length);
			return _characters[index];
		}
	}
}