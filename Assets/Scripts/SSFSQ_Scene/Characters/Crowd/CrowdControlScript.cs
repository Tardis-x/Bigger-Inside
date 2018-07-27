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
			var characters = GetComponentsInChildren<CharacterAnimationScript>();
			
			foreach (var character in characters)
			{
				character.StartThrowing();
			}
		}
		
		public void StopThrowing()
		{
			var characters = GetComponentsInChildren<CharacterAnimationScript>();
			
			foreach (var character in characters)
			{
				character.StopThrowing();
			}
		}		
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		private CharacterAnimationScript GetRandomCharacter()
		{
			var characters = GetComponentsInChildren<CharacterAnimationScript>();
			int index = (int) Math.Round(Random.value * characters.Length);
			return characters[index];
		}
	}
}