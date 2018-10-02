using UnityEngine;
using UnityEngine.Serialization;

namespace ua.org.gdg.devfest
{
  public class PlayButton : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [FormerlySerializedAs("_tutorialPanel")] [SerializeField]
    private TDTutorialPanelScript _tdTutorialPanel;

    [SerializeField] private GameEvent _gameStart;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    public void OnClick()
    {
      gameObject.SetActive(false);
      _tdTutorialPanel.ShowPanel(true);
    }
  }
}