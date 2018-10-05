using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private int _timeForAnswer = 5;
    
    [Space]
    [Header("Overlay UI")] 
    [SerializeField] private HealthTimePanelScript _healthTimePanel;

    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private ARCorePanelScript _arCorePanel;
    [SerializeField] private Text _hintText;

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
      ResetUI();
    }

    public void OnContentPlaced(GameObject environment)
    {
      ShowARCorePanel(true);
    }
    
    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnGameOver()
    {
      _healthTimePanel.HidePanel();
      ButtonsToPauseMode();
    }

    public void OnCountdownStart()
    {
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

    public void OnTrackingLost()
    {
      ShowARCorePanel(false);
    }

    public void OnTrackingFound()
    {
      ShowARCorePanel(ARCoreHelper.CheckArCoreSupport());
    }

    public void OnShowTutorial()
    {
      _tutorialPanel.SetActive(true);
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

    public void SubstractBrain()
    {
      _healthTimePanel.SubtractBrain();
    }

    public void SubstractStar()
    {
      _healthTimePanel.SubtractStar();
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private void ButtonsToPlayMode()
    {
      HidePlayButton();
      ShowAnswerButton();
      ShowHitButton();
    }

    private void ButtonsToPauseMode()
    {
      ShowPlayButton();
      HideAnswerButton();
      HideHitButton();
    }

    private void ToAnswerMode()
    {
      HideAnswerButton();
      HideHitButton();
      _healthTimePanel.PauseCountDown(true);
    }
    
    private void ShowARCorePanel(bool value)
    {
      _arCorePanel.gameObject.SetActive(value);
    }

    private void HideHitButton()
    {
      _hitVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      _hitVirtualButton.SetButtonEnabled(false);
      _arCorePanel.ShowHitButton(false);
    }

    private void ShowHitButton()
    {
      _hitVirtualButton.SetVirtualButtonMaterial(_hitButtonMaterial);
      _hitVirtualButton.SetButtonEnabled(true);
      _arCorePanel.ShowHitButton(true);
    }

    private void HideAnswerButton()
    {
      _answerVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      _answerVirtualButton.SetButtonEnabled(false);
      _arCorePanel.ShowAnswerButton(false);
    }

    private void ShowAnswerButton()
    {
      _answerVirtualButton.SetVirtualButtonMaterial(_answerButtonMaterial);
      _answerVirtualButton.SetButtonEnabled(true);
      _arCorePanel.ShowAnswerButton(true);
    }

    private void HidePlayButton()
    {
      _playVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      _playVirtualButton.SetButtonEnabled(false);
      _arCorePanel.ShowPlayButton(false);
    }

    private void ShowPlayButton()
    {
      _playVirtualButton.SetVirtualButtonMaterial(_playButtonMaterial);
      _playVirtualButton.SetButtonEnabled(true);
      _arCorePanel.ShowPlayButton(true);
    }
  }
}