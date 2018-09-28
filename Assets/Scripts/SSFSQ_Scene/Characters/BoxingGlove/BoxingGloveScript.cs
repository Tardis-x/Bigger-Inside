using UnityEngine;

namespace ua.org.gdg.devfest
{
  [RequireComponent(typeof(AudioSource))]
  public class BoxingGloveScript : MonoBehaviour
  {

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private Animator _animator;
    private AudioSource _audioSource;

    private readonly Vector3 _offset = new Vector3
    {
      x = -.1356f,
      y = -.1020f,
      z = -.2250f
    };

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _audioSource = GetComponent<AudioSource>();
      _animator = GetComponentInChildren<Animator>();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void HitObject(Transform obj)
    {
      PrepareToHitObject(obj);
      Hit();
    }


    public void Appear()
    {
      gameObject.SetActive(true);
    }

    public void Disappear()
    {
      gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void Hit()
    {
      _animator.SetTrigger("Hit");
      _audioSource.PlayDelayed(0.18f);
    }

    private void PrepareToHitObject(Transform obj)
    {
      transform.SetPositionAndRotation(obj.position, transform.rotation);
      transform.localPosition += _offset;
      Appear();
    }
  }
}
