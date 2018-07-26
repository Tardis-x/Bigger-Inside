using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace ua.org.gdg.devfest
{
  public class HealthTimePanelScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private Image _timerImage;
    [SerializeField] private float _timerRefreshRate;
    [SerializeField] private RectTransform _brainsContainer;
    [SerializeField] private RectTransform _starsContainer;
    [SerializeField] private RectTransform _brainPrefab;
    [SerializeField] private RectTransform _starPrefab;
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      _timerRefreshRate = 1 / _timerRefreshRate;
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private float _timeLeft;
    private float _countdownTime;
    private Coroutine _countdown;
    private Action _onTimeout;

    private IEnumerator Countdown()
    {
      while (_timeLeft >= 0)
      {
        yield return _timerRefreshRate;
        _timerImage.fillAmount = _timeLeft / _countdownTime;
        _timeLeft -= Time.deltaTime;
      }
      
      Debug.Log("TimeOut");
      if (_onTimeout != null) _onTimeout();
    }
    
    private void ClearStarsContainer()
    {
      var stars = _starsContainer.GetComponentsInChildren<RectTransform>().Where(x => x.parent == _starsContainer);

      foreach (var s in stars)
      {
        Destroy(s.gameObject);
      }
    }
    
    private void ClearBrainsContainer()
    {
      var brains = _brainsContainer.GetComponentsInChildren<RectTransform>().Where(x => x.parent == _brainsContainer);

      foreach (var b in brains)
      {
        Destroy(b.gameObject);
      }
    }
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void ShowPanel()
    {
      gameObject.SetActive(true);
    }

    public void HidePanel()
    {
      gameObject.SetActive(false);
    }
    
    public void ResetPanel()
    {
      ResetCountdown();
      ClearBrainsContainer();
      ClearStarsContainer();
    }
    
    public void StartCountdown(float time, Action onTimeout)
    {
      ResetCountdown();
      _countdownTime = time;
      _timeLeft = _countdownTime;
      _onTimeout = onTimeout;
      _countdown = StartCoroutine(Countdown());
    }

    private void ResetCountdown()
    {
      if(_countdown != null) StopCoroutine(_countdown);
      _timerImage.fillAmount = 1;
      _timeLeft = _countdownTime;
    }
    
    public void SetStarsCount(int count)
    {
      for (int i = 0; i < count; i++)
      {
        var star = Instantiate(_starPrefab);
        star.SetParent(_starsContainer);
      }
    }

    public void SubtractStar()
    {
      var star = _starsContainer.GetComponentsInChildren<RectTransform>().First(x => x.parent == _starsContainer);
      Destroy(star.gameObject);
    }

    public void SetBrainsCount(int count)
    {
      for (int i = 0; i < count; i++)
      {
        var brain = Instantiate(_brainPrefab);
        brain.SetParent(_brainsContainer);
      }
    }

    public void SubtractBrain()
    {
      var brain = _brainsContainer.GetComponentsInChildren<RectTransform>().First(x => x.parent == _brainsContainer);
      Destroy(brain.gameObject);
    }
  }
}