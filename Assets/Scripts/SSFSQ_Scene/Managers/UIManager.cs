using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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