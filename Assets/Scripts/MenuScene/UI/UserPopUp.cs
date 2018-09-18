using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

namespace ua.org.gdg.devfest
{
  public class UserPopUp : MonoBehaviour
  {
    private const int START_HEIGHT = 60;
    private const int END_HEIGHT = 80;
    private const int START_WIDTH = 240;
    private const int END_WIDTH = 320;
    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("UI")] 
    [SerializeField] private CanvasGroup _buttonCanvasGroup;
    [SerializeField] private RectTransform _buttonTransform;
    [SerializeField] private GameObject _hideLogoutButton;
    
    [Space]
    [Header("Specs")]
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _fadeRate;

    [Space] 
    [Header("Events")] 
    [SerializeField]
    private GameEvent _signInRequest;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private float _time;
    private float _targetAlpha;
    private float _startAlpha;

    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------

    public bool Active { get; set; }
    public bool InMenu { private get; set; }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      InMenu = true;
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void ProfileButtonOnClick()
    {
      if (FirebaseAuth.DefaultInstance.CurrentUser == null) _signInRequest.Raise();
      else
      {
        if (InMenu) Toggle(!Active);
      }
    }


    public void ShowSignout()
    {
      Active = _buttonCanvasGroup.interactable = true;
      _hideLogoutButton.SetActive(true);
      _startAlpha = 0.0f;
      _targetAlpha = 1.0f;
      _time = 0;
      StartCoroutine(FadeCoroutine());
      StartCoroutine(SizeCoroutine());
    }

    public void HideSignout()
    {
      Active = _buttonCanvasGroup.interactable = false;
      _hideLogoutButton.SetActive(false);
      _startAlpha = 1.0f;
      _targetAlpha = 0.0f;
      _time = 0;
      StartCoroutine(FadeCoroutine());
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void Toggle(bool value)
    {
      Active = _buttonCanvasGroup.interactable = value;
      _hideLogoutButton.SetActive(value);
      _startAlpha = value ? 0.0f : 1.0f;
      _targetAlpha = value ? 1.0f : 0.0f;
      _time = 0;
      StartCoroutine(FadeCoroutine());
      if (value) StartCoroutine(SizeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
      while (_fadeTime - _time > .005f)
      {
        _time += _fadeRate;

        _buttonCanvasGroup.alpha = Mathf.Lerp(_startAlpha, _targetAlpha, _time / _fadeTime);

        yield return new WaitForSeconds(_fadeRate);
      }

      _buttonCanvasGroup.alpha = _targetAlpha;
    }

    private IEnumerator SizeCoroutine()
    {
      while (_fadeTime - _time > .0005f)
      {
        _time += _fadeRate;
        
        _buttonTransform.sizeDelta = new Vector2( 
          Mathf.Lerp(START_WIDTH, END_WIDTH, _time / _fadeTime),
          Mathf.Lerp(START_HEIGHT, END_HEIGHT, _time / _fadeTime));

        yield return new WaitForSeconds(_fadeRate);
      }
    }
  }
}