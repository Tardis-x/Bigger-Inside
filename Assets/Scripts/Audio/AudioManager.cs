using UnityEngine;

namespace ua.org.gdg.devfest
{
  [RequireComponent(typeof(AudioSource))]
  public class AudioManager : MonoBehaviour
  {
    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _shutUpClip;
    [SerializeField] private AudioClip _answerClip;
    [SerializeField] private AudioClip _dieClip;
    [SerializeField] private AudioClip _crowdClip;
    [SerializeField] private AudioClip _crowdOnDieClip;
    [SerializeField] private AudioClip _rightActionClip;
    [SerializeField] private AudioClip _wrongActionClip;
    [SerializeField] private AudioClip _gameOverClip;
    
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

    public void PlayShutUp()
    {
      Play(_shutUpClip, 0.7f);
    }
    
    public void PlayAnswerDelayed()
    {
      Invoke("PlayAnswer", 0.2f);
    }

    public void PlayDie()
    {
      Play(_dieClip, 1.3f);
      Play(_crowdOnDieClip, 0.5f);
    }
  
    public void PlayTomatoThrow()
    {
      Play(_crowdClip, 0.5f);
    }

    public void PlayRightAction()
    {
      Play(_rightActionClip, 0.8f);
    }

    public void PlayWrongAction()
    {
      Play(_wrongActionClip, 0.8f);
    }

    public void PlayGameOver()
    {
      Play(_gameOverClip, 1.3f);
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void PlayAnswer()
    {
      Play(_answerClip, 1.2f);
    }
    
    private void Play(AudioClip audioClip, float volume = 1f)
    { 
      if (audioClip == null) return;
      
      _audioSource.PlayOneShot(audioClip, volume);
    }   
  }
}