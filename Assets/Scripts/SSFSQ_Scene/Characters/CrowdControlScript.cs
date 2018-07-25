using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ua.org.gdg.devfest
{

	public class CrowdControlScript : MonoBehaviour
	{
		///---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private GameObject _chairs;
		
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
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		private CharacterAnimationScript GetRandomCharacter()
		{
			var characters = _chairs.GetComponentsInChildren<CharacterAnimationScript>();
			int index = (int) Math.Round(Random.value * characters.Length);
			return characters[index];
		}
	}
}