using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class Fade : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Tooltip("Refreshment rate")]
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _fadeRate;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private Image _image;
    private float _targetAlpha;
    private float _startAlpha;
    private float _time;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    // Use this for initialization
    void Start()
    {
      _image = GetComponent<Image>();
      if (_image == null)
      {
        Debug.LogError("Error: No image on " + name);
      }

      _targetAlpha = _image.color.a;
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
  
    public void FadeOut()
    {
      _startAlpha = 1.0f;
      _targetAlpha = 0.0f;
      _time = 0;
      StartCoroutine(FadeCoroutine());
    }

    public void FadeIn()
    {
      _startAlpha = 0.0f;
      _targetAlpha = 1.0f;
      _time = 0;
      StartCoroutine(FadeCoroutine());
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private IEnumerator FadeCoroutine()
    {
      while (_time <= _fadeTime)
      {
        _time += _fadeRate;
        
        Color curColor = _image.color;
        curColor.a = Mathf.Lerp(_startAlpha, _targetAlpha, _time / _fadeTime);
        _image.color = curColor;

        yield return new WaitForSeconds(_fadeRate);
      }
    }
  }
}