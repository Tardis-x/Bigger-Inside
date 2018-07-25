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
    private bool _keepCountdown;

    private IEnumerator Countdown()
    {
      while (_timeLeft >= 0 && _keepCountdown)
      {
        yield return new WaitForSeconds(_timerRefreshRate);
        _timerImage.fillAmount = _timeLeft / _countdownTime;
        _timeLeft -= _timerRefreshRate;
      }
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

    public void ResetPanel()
    {
      ResetCountDown();
      ClearBrainsContainer();
      ClearStarsContainer();
    }
    
    public void StartCountdown(float time, Action<QuestionModel> onTimeOut)
    {
      _countdownTime = _timeLeft = time;
      _keepCountdown = true;
      StartCoroutine(Countdown());
    }

    public void ResetCountDown()
    {
      _keepCountdown = false;
      _timerImage.fillAmount = 1;
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