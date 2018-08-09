using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class UIManager : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Variables")] 
    [SerializeField] private IntVariable _score;
    [SerializeField] private IntVariable _brainsCount;
    [SerializeField] private IntVariable _starsCount;
    [SerializeField] private PlayerChoiceVariable _playerChoice;
    [SerializeField] private QuestionVariable _currentQuestion;
    [SerializeField] private int _timeForAnswer = 5;
    
    [Space]
    [Header("Overlay UI")] 
    [SerializeField] private HealthTimePanelScript _healthTimePanel;

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

    public void OnGameOver()
    {
      Debug.Log("UI Manager: OnGameOver");
      _healthTimePanel.HidePanel();
      ButtonsToPauseMode();
    }

    public void OnCountdownStart()
    {
      Debug.Log("UI Manager: OnCountDownStart");
      HidePlayButton();
    }

    public void OnGameStart()
    {
      ResetUI();
    }

    public void OnAnswerAndHit()
    {
      ToAnswerMode();
    }

    public void OnNewQuestion()
    {
      ButtonsToPlayMode();
      _healthTimePanel.StartCountdown(_timeForAnswer);
    }

    public void OnSpeakerAnimationEnd()
    {
      switch (_playerChoice.Value)
      {
        case PlayerChoice.Answer:
          if(!_currentQuestion.Value.IsGood) _healthTimePanel.SubtractBrain();
          break;
        case PlayerChoice.Hit:
          if(_currentQuestion.Value.IsGood) _healthTimePanel.SubtractStar();
          break;
        default: 
          return;
      }
    }

    //-----------------------------------------------
    // Public
    //-----------------------------------------------

    private void ResetUI()
    {
      _healthTimePanel.ResetPanel();
      _healthTimePanel.SetBrainsCount(_brainsCount.InitialValue);
      _healthTimePanel.SetStarsCount(_starsCount.InitialValue);
      _healthTimePanel.ShowPanel();
      ButtonsToPlayMode();
    }

    public void ButtonsToPlayMode()
    {
      HidePlayButton();
      ShowAnswerButton();
      ShowHitButton();
    }

    public void ButtonsToPauseMode()
    {
      Debug.Log("UI Manager: ButtonsToPauseMode");
      ShowPlayButton();
      HideAnswerButton();
      HideHitButton();
    }

    public void ToAnswerMode()
    {
      HideAnswerButton();
      HideHitButton();
      _healthTimePanel.PauseCountDown(true);
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private void HideHitButton()
    {
      Debug.Log("UI Manager: Hide hit");
      HitVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      HitVirtualButton.SetButtonEnabled(false);
    }

    private void ShowHitButton()
    {
      HitVirtualButton.SetVirtualButtonMaterial(_hitButtonMaterial);
      HitVirtualButton.SetButtonEnabled(true);
    }

    private void HideAnswerButton()
    {
      Debug.Log("UI Manager: HideAnswerButton");
      AnswerVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      AnswerVirtualButton.SetButtonEnabled(false);
    }

    private void ShowAnswerButton()
    {
      AnswerVirtualButton.SetVirtualButtonMaterial(_answerButtonMaterial);
      AnswerVirtualButton.SetButtonEnabled(true);
    }

    private void HidePlayButton()
    {
      PlayVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      PlayVirtualButton.SetButtonEnabled(false);
    }

    private void ShowPlayButton()
    {
      Debug.Log("UI Manager: ShowPlayButton");
      PlayVirtualButton.SetVirtualButtonMaterial(_playButtonMaterial);
      PlayVirtualButton.SetButtonEnabled(true);
    }
  }
}