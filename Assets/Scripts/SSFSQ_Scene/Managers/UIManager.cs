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
    [Header("Overlay UI")] 
    public HealthTimePanelScript HealthTimePanel;
    public GameOverPanelScript GameOverPanel;
    
    [Space] 
    [Header("Virtual Buttons")] 
    public VirtualButtonBehaviour PlayVirtualButton;
    public VirtualButtonBehaviour AnswerVirtualButton;
    public VirtualButtonBehaviour HitVirtualButton;
    
    [Space] 
    [Header("Environment Screen")] public Text ScreenQuestionText;

    [Space] 
    [Header("VirtualButtonsMaterials")] [SerializeField]
    private Material _playButtonMaterial;
    [SerializeField] private Material _stopButtonMaterial;

    public void SetPlayButton(bool value)
    {
      if(value) 
      {
        Instance.PlayVirtualButton.GetComponent<VirtualButtonEventHandler>()
          .m_VirtualButtonDefault = _playButtonMaterial;
        Instance.PlayVirtualButton.GetComponent<VirtualButtonEventHandler>().RefreshMaterial();
      }
      else
      {
        Instance.PlayVirtualButton.GetComponent<VirtualButtonEventHandler>()
          .m_VirtualButtonDefault = _stopButtonMaterial;
        Instance.PlayVirtualButton.GetComponent<VirtualButtonEventHandler>().RefreshMaterial();
      }
    }
  }
}