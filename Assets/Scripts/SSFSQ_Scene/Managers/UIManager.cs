using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class UIManager : Singleton<UIManager>
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    [Header("Overlay UI")] public HealthTimePanelScript HealthTimePanel;
    public GameOverPanelScript GameOverPanel;

    [Space] [Header("Virtual Buttons")] public VirtualButtonEventHandler PlayVirtualButton;
    public VirtualButtonEventHandler AnswerVirtualButton;
    public VirtualButtonEventHandler HitVirtualButton;

    [Space] [Header("Environment Screen")] public Text ScreenQuestionText;

    [Space] [Header("VirtualButtonsMaterials")] [SerializeField]
    private Material _playButtonMaterial;

    [SerializeField] private Material _hitButtonMaterial;
    [SerializeField] private Material _answerButtonMaterial;
    [SerializeField] private Material _transparentButtonMaterial;
    [SerializeField] private Text _getReadyText;
    [SerializeField] private int _getReadyTime = 3;

    //-----------------------------------------------
    // Messages
    //-----------------------------------------------

    private void Start()
    {
      ButtonsToPauseMode();
    }

    //-----------------------------------------------
    // Public
    //-----------------------------------------------

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
      GameOverPanel.HidePanel();
      HidePlayButton();
      _timeLeft = _getReadyTime;
      StartCoroutine(GetReadyCountDown(onCountdownFinished));
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
      _getReadyText.gameObject.SetActive(value);
    }

    private void ScreenQuestionTextSetActive(bool value)
    {
      ScreenQuestionText.gameObject.SetActive(value);
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