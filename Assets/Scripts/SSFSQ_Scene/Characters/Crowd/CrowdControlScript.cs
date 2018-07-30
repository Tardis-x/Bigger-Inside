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

		private CharacterAnimationScript _character;
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

		public void AskQuestion()
		{
			_character = GetRandomCharacter();
			_character.StandUpAndAsk();
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