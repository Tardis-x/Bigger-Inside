using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ua.org.gdg.devfest
{

	public class StandUpVButtonClick : Clickable
	{
		///---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private GameObject _chairs;
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private CharacterAnimmationScript _character;

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public override void OnClick()
		{
			_character = GetRandomCharacter();
			_character.StandUpAndAsk();
		}
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		private CharacterAnimmationScript GetRandomCharacter()
		{
			var characters = _chairs.GetComponentsInChildren<CharacterAnimmationScript>();
			int index = (int) Math.Round(Random.value * characters.Length);
			return characters[index];
		}
	}
}