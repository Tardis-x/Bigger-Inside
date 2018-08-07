using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class UIManager : Singleton<UIManager>
  {
    private GameOverPanelScript _gameOverPanel;
    private Text _screenQuestionText;
    private Text _getReadyText;
    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Variables")] 
    [SerializeField] private IntVariable _score;
    
    [Space]
    [Header("Overlay UI")] 
    public HealthTimePanelScript HealthTimePanel;

    [Space] 
    [Header("Virtual Buttons")] 
    public VirtualButtonEventHandler PlayVirtualButton;
    public VirtualButtonEventHandler AnswerVirtualButton;
    public VirtualButtonEventHandler HitVirtualButton;

    [Space] 
    [Header("VirtualButtonsMaterials")] 
    [SerializeField] private Material _playButtonMaterial;
    [SerializeField] private Material _hitButtonMaterial;
    [SerializeField] private Material _answerButtonMaterial;
    [SerializeField] private Material _transparentButtonMaterial;
    [SerializeField] private int _getReadyTime = 3;
    
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public Text ScreenQuestionText
    {
      get { return _screenQuestionText; }
    }

    //-----------------------------------------------
    // Messages
    //-----------------------------------------------

    private void Start()
    {
      ButtonsToPauseMode();
    }
    
    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------
    
    public void OnEnvironmentInstantiated()
    {
      var environmentInstance = GameManager.Instance.EnvironmentInstance;
      
      _gameOverPanel = environmentInstance.GameOverPanel;
      _screenQuestionText = environmentInstance.ScreenQuestionText;
      _getReadyText = environmentInstance.GetReadyText;
    }

    public void OnGameOver()
    {
      _gameOverPanel.SetScore(_score.RuntimeValue);
      _gameOverPanel.ShowPanel();
      HealthTimePanel.HidePanel();
      ScreenQuestionTextSetActive(false);
      ButtonsToPauseMode();
    }

    //-----------------------------------------------
    // Public
    //-----------------------------------------------

    public void ResetUI()
    {
      _gameOverPanel.HidePanel();
      HealthTimePanel.ResetPanel();
      HealthTimePanel.ShowPanel();
      ButtonsToPlayMode();
    }

    public void ButtonsToPlayMode()
    {
      Instance.HidePlayButton();
      Instance.ShowAnswerButton();
      Instance.ShowHitButton();
    }

    public void ButtonsToPauseMode()
    {
      Instance.ShowPlayButton();
      Instance.HideAnswerButton();
      Instance.HideHitButton();
    }

    public void ToAnswerMode()
    {
      HideAnswerButton();
      HideHitButton();
      HealthTimePanel.PauseCountDown(true);
    }

    public void StartGetReadyCountdown(Action onCountdownFinished)
    {
      if (_countdown) return;

      _countdown = true;
      GetReadyTextSetActive(true);
      ScreenQuestionTextSetActive(false);
      if(_gameOverPanel != null) _gameOverPanel.HidePanel();
      HidePlayButton();
      _timeLeft = _getReadyTime;
      StartCoroutine(GetReadyCountDown(onCountdownFinished));
    }
    
    public void ScreenQuestionTextSetActive(bool value)
    {
      if(_screenQuestionText != null) _screenQuestionText.gameObject.SetActive(value);
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private int _timeLeft;
    private bool _countdown;

    private IEnumerator GetReadyCountDown(Action onCountdownFinished)
    {
      while (_timeLeft >= -1)
      {
        if (_timeLeft > 0)
        {
          _getReadyText.text = "GET READY!\n" + _timeLeft;
        }
        else if (_timeLeft == 0)
        {
          _getReadyText.text = "GET READY!\nGO!";
        }
        else
        {
          EndGetReadyCountdown(onCountdownFinished);
        }
        
        yield return new WaitForSeconds(1);
        _timeLeft--;
      }
    }

    private void EndGetReadyCountdown(Action onCountdownFinished)
    {
      _countdown = false;
      ScreenQuestionTextSetActive(true);
      GetReadyTextSetActive(false);
      StopCoroutine(GetReadyCountDown(onCountdownFinished));
      onCountdownFinished();
    }

    private void GetReadyTextSetActive(bool value)
    {
      if(_getReadyText != null) _getReadyText.gameObject.SetActive(value);
    }

    private void HideHitButton()
    {
      Instance.HitVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      Instance.HitVirtualButton.SetButtonEnabled(false);
    }

    private void ShowHitButton()
    {
      Instance.HitVirtualButton.SetVirtualButtonMaterial(_hitButtonMaterial);
      Instance.HitVirtualButton.SetButtonEnabled(true);
    }

    private void HideAnswerButton()
    {
      Instance.AnswerVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      Instance.AnswerVirtualButton.SetButtonEnabled(false);
    }

    private void ShowAnswerButton()
    {
      Instance.AnswerVirtualButton.SetVirtualButtonMaterial(_answerButtonMaterial);
      Instance.AnswerVirtualButton.SetButtonEnabled(true);
    }

    private void HidePlayButton()
    {
      Instance.PlayVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      Instance.PlayVirtualButton.SetButtonEnabled(false);
    }

    private void ShowPlayButton()
    {
      Instance.PlayVirtualButton.SetVirtualButtonMaterial(_playButtonMaterial);
      Instance.PlayVirtualButton.SetButtonEnabled(true);
    }
  }
}