using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SnackbarScript : MonoBehaviour
  {
    private const float FADE_IN_TIME = .25f;
    private const float FADE_OUT_TIME = .2f;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private CanvasGroup _canvasGroup;
    private float _time;
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _canvasGroup = GetComponent<CanvasGroup>();
    }
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void Show(string message)
    {
      
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
    
    private IEnumerator FadeInCoroutine()
    {
      _time = 0;
      
      while (FADE_IN_TIME - _time > 0)
      {
        _time += Time.deltaTime;

        _canvasGroup.alpha = Mathf.Lerp(0, 1, _time / FADE_IN_TIME);

        yield return new WaitForSeconds(.01f);
      }

      _canvasGroup.alpha = 1;
    }
    
    private IEnumerator FadeOutCoroutine()
    {
      _time = 0;
      
      while (FADE_OUT_TIME - _time > 0)
      {
        _time += Time.deltaTime;

        _canvasGroup.alpha = Mathf.Lerp(1, 0, _time / FADE_OUT_TIME);

        yield return new WaitForSeconds(.01f);
      }

      _canvasGroup.alpha = 0;
    }
  }
}