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
    [SerializeField] private VirtualButtonEventHandler _playVirtualButton;
    [SerializeField] private VirtualButtonEventHandler _answerVirtualButton;
    [SerializeField] private VirtualButtonEventHandler _hitVirtualButton;

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
      _hitVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      _hitVirtualButton.SetButtonEnabled(false);
    }

    private void ShowHitButton()
    {
      _hitVirtualButton.SetVirtualButtonMaterial(_hitButtonMaterial);
      _hitVirtualButton.SetButtonEnabled(true);
    }

    private void HideAnswerButton()
    {
      Debug.Log("UI Manager: HideAnswerButton");
      _answerVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      _answerVirtualButton.SetButtonEnabled(false);
    }

    private void ShowAnswerButton()
    {
      _answerVirtualButton.SetVirtualButtonMaterial(_answerButtonMaterial);
      _answerVirtualButton.SetButtonEnabled(true);
    }

    private void HidePlayButton()
    {
      _playVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      _playVirtualButton.SetButtonEnabled(false);
    }

    private void ShowPlayButton()
    {
      Debug.Log("UI Manager: ShowPlayButton");
      _playVirtualButton.SetVirtualButtonMaterial(_playButtonMaterial);
      _playVirtualButton.SetButtonEnabled(true);
    }
  }
}