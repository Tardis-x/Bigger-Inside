using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class ObjectRotationScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private float _rotationTime;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private float _time;
    private float _startRotationY = 0;
    private float _endRotationY = 360;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void OnEnable()
    { 
      StartCoroutine(RotationCoroutine());
    }

    private void OnDisable()
    {
      StopAllCoroutines();
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private IEnumerator RotationCoroutine()
    {
      while (true)
      {
        _time += Time.deltaTime;

        if (_time > _rotationTime) _time -= _rotationTime;

        var rotation = transform.localEulerAngles;
        rotation.y = Mathf.Lerp(_startRotationY, _endRotationY, _time / _rotationTime);
        transform.localEulerAngles = rotation;

        yield return new WaitForSeconds(.01f);
      }
    }
  }
}