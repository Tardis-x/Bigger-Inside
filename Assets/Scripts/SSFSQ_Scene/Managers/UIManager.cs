using UnityEngine;
using UnityEngine.UI;
using Vuforia;

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
      ShowHint(ARCoreHelper.CheckArCoreSupport());
    }

    public void OnContentPlaced(GameObject environment)
    {
      ShowARCorePanel(true);
      ShowHint(false);
    }
    
    public void OnAutomaticHitTest(HitTestResult hitTestResult)
    {
      if (hitTestResult == null)
      {
        ShowHint(true);
        return;
      }
						
      ShowHint(false);
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
      ShowHint(ARCoreHelper.CheckArCoreSupport());
      ShowARCorePanel(false);
    }

    public void OnTrackingFound()
    {
      ShowARCorePanel(ARCoreHelper.CheckArCoreSupport());
      ShowHint(false);
      
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

    public void SubstractBrain()
    {
      _healthTimePanel.SubtractBrain();
    }

    public void SubstractStar()
    {
      _healthTimePanel.SubtractStar();
    }

    public void ShowARCorePanel(bool value)
    {
      _arCorePanel.gameObject.SetActive(value);
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

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

    private void ShowHint(bool value)
    {
      _hintText.gameObject.SetActive(value);
    }
  }
}