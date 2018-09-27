using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [RequireComponent(typeof(AudioSource))]
  public class TDAudioManager : MonoBehaviour
  {
    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Audio Clips")] 
    [SerializeField] private AudioClip _coinsClip;
    [SerializeField] private AudioClip _hallUnlockClip;
    [SerializeField] private AudioClip _gameOverClip;
    [SerializeField] private AudioClip _buyTowerClip;
    [SerializeField] private AudioClip _upgradeTowerClip;
    [SerializeField] private AudioClip _sellTowerClip;
    [SerializeField] private List<AudioClip> _eatClipList;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private AudioSource _audioSource;
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      _audioSource = GetComponent<AudioSource>();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void PlayCoins()
    {
      Play(_coinsClip, 0.3f);
    }
    
    public void PlayHallUnlock()
    {
      Play(_hallUnlockClip);
    }    
    
    public void PlayGameOver()
    {
      Play(_gameOverClip);
    }

    public void PlayAudioClipData(int soundNumber)
    {
      switch ((Sound) soundNumber)
      {
        case Sound.BuyTower:
          Play(_buyTowerClip);
          break;
        case Sound.UpgradeTower:
          Play(_upgradeTowerClip);
          break;
        case Sound.SellTower:
          Play(_sellTowerClip);
          break;
        case Sound.Eat:
          PlayRandomEatClip();
          break;
      }
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void PlayRandomEatClip()
    {
      var position = Random.Range(0, _eatClipList.Count - 1);
      var volume = Random.Range(0.6f, 1f);
      Play(_eatClipList[position], volume);
    }
    
    private void Play(AudioClip audioClip, float volume = 1f)
    { 
      if (audioClip == null) return;
      
      _audioSource.PlayOneShot(audioClip, volume);
    }  
  }
}