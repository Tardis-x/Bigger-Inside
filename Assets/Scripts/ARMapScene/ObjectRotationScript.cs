using System.Collections;
using System.Collections.Generic;
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

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
	    StartCoroutine(FadeCoroutine());
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private IEnumerator FadeCoroutine()
    {
      while (true)
      {
        _time += Time.deltaTime;
	      
	      if (_time > _rotationTime) _time -= _rotationTime;

	      var rotation = transform.localEulerAngles;
	      rotation.y = Mathf.Lerp(0, 360, _time / _rotationTime);
	      transform.localEulerAngles = rotation;
        
        yield return new WaitForSeconds(.01f);
      }
    }
	}
}