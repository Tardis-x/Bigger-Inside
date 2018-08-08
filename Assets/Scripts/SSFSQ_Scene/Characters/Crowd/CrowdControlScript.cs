using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ua.org.gdg.devfest
{

	public class CrowdControlScript : MonoBehaviour
	{
		[Header("Events")]
		[SerializeField] private IntVariable _starsCount;
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private CharacterAnimationScript[] _characters;
		
		//---------------------------------------------------------------------
		// Message
		//---------------------------------------------------------------------

		private void Start()
		{
			_characters = GetComponentsInChildren<CharacterAnimationScript>();
		}
		
		//---------------------------------------------------------------------
		// Events
		//---------------------------------------------------------------------

		public void OnGameOver()
		{
			if(_starsCount.RuntimeValue == 0) StartThrowing();
		}

		public void OnCountdownStart()
		{
			StopThrowing();
			StopBeingScared();
		}

		public void OnSpeakerDied()
		{
			StartBeingScared();
		}

		public void OnSpeakerHit()
		{
			CurrentCharacter.GetHit();
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