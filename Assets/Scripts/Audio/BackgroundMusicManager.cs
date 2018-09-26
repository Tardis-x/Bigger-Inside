using UnityEngine;

namespace ua.org.gdg.devfest
{
  [RequireComponent(typeof(AudioSource))]
  public class BackgroundMusicManager : MonoBehaviour
  {
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

    public void Play(AudioClip audioClip)
    { 
      Stop();

      if (audioClip == null) return;
      
      _audioSource.clip = audioClip;
      _audioSource.Play();
    }

    public void Pause()
    {
      _audioSource.Pause();
    }

    public void Resume()
    {
      _audioSource.Play();
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
    
    private void Stop()
    {
      if (_audioSource.isPlaying)
      {
        _audioSource.Stop();
      }
    }
    
  }
}