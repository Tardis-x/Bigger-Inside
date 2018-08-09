using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class UIManager : Singleton<UIManager>
  {
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
      HealthTimePanel.HidePanel();
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
    }

    //-----------------------------------------------
    // Public
    //-----------------------------------------------

    private void ResetUI()
    {
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
      Debug.Log("UI Manager: ButtonsToPauseMode");
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

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private void HideHitButton()
    {
      Debug.Log("UI Manager: Hide hit");
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
      Debug.Log("UI Manager: HideAnswerButton");
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
      PlayVirtualButton.SetVirtualButtonMaterial(_transparentButtonMaterial);
      PlayVirtualButton.SetButtonEnabled(false);
    }

    private void ShowPlayButton()
    {
      Debug.Log("UI Manager: ShowPlayButton");
      Instance.PlayVirtualButton.SetVirtualButtonMaterial(_playButtonMaterial);
      Instance.PlayVirtualButton.SetButtonEnabled(true);
    }
  }
}