using UnityEngine;

namespace ua.org.gdg.devfest
{
  [RequireComponent(typeof(AudioSource))]
  public class BackgroundMusicManager : MonoBehaviour
  {
    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;
    
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

    public void PlayMenuMusic()
    {
      Play(_menuMusic);
    }

    public void PlayGameMusic()
    {
      Play(_gameMusic);
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
    
    private void Play(AudioClip audioClip)
    { 
      Stop();

      if (audioClip == null) return;
      
      _audioSource.clip = audioClip;
      _audioSource.Play();
    }
    
    private void Stop()
    {
      if (_audioSource.isPlaying)
      {
        _audioSource.Stop();
      }
    }
    
  }
}