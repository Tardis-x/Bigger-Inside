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
    [SerializeField] private AudioClip _hallUnlockClip;
    [SerializeField] private AudioClip _gameOverClip;
    [SerializeField] private AudioClip _buyTowerClip;
    [SerializeField] private AudioClip _upgradeTowerClip;
    [SerializeField] private AudioClip _sellTowerClip;
    [SerializeField] private AudioClip _visitorFedClip;
    [SerializeField] private AudioClip _visitorLeftClip;
    [SerializeField] private List<AudioClip> _eatClipList;
    [SerializeField] private List<AudioClip> _drinkClipList;
    
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

    public void PlayHallUnlock()
    {
      Play(_hallUnlockClip);
    }    
    
    public void PlayGameOver()
    {
      Play(_gameOverClip);
    }   
    
    public void PlayVisitorFed()
    {
      Play(_visitorFedClip, 0.6f);
    }    
    
    public void PlayVisitorLeft()
    {
      Play(_visitorLeftClip, 0.6f);
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
          PlayRandomClip(_eatClipList);
          break;
        case Sound.Drink:
          PlayRandomClip(_drinkClipList);
          break;
      }
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void PlayRandomClip(List<AudioClip> audioClipList)
    {
      var position = Random.Range(0, audioClipList.Count - 1);
      var volume = Random.Range(0.6f, 1f);
      Play(audioClipList[position], volume);
    }
    
    private void Play(AudioClip audioClip, float volume = 1f)
    { 
      if (audioClip == null) return;
      
      _audioSource.PlayOneShot(audioClip, volume);
    }  
  }
}