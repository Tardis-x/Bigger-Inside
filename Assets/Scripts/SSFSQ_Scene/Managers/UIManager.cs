using UnityEngine;
using UnityEngine.UI;
using Vuforia;

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
      Instance.HitVirtualButton.m_VirtualButtonDefault = _transparentButtonMaterial;
      Instance.HitVirtualButton.RefreshMaterial();
    }

    private void ShowHitButton()
    {
      Instance.HitVirtualButton.m_VirtualButtonDefault = _hitButtonMaterial;
      Instance.HitVirtualButton.RefreshMaterial();
    }

    private void HideAnswerButton()
    {
      Instance.AnswerVirtualButton.m_VirtualButtonDefault = _transparentButtonMaterial;
      Instance.AnswerVirtualButton.RefreshMaterial();
    }

    private void ShowAnswerButton()
    {
      Instance.AnswerVirtualButton.m_VirtualButtonDefault = _answerButtonMaterial;
      Instance.AnswerVirtualButton.RefreshMaterial();
    }

    private void HidePlayButton()
    {
      Instance.PlayVirtualButton.m_VirtualButtonDefault = _transparentButtonMaterial;
      Instance.PlayVirtualButton.RefreshMaterial();
    }

    private void ShowPlayButton()
    {
      Instance.PlayVirtualButton.m_VirtualButtonDefault = _playButtonMaterial;
      Instance.PlayVirtualButton.RefreshMaterial();
    }
  }
}