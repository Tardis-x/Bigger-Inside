using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class SSFSQ_TutorialPanelScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Cards")] [SerializeField] private GameObject _cardIntro;
    [SerializeField] private GameObject _cardSaveSpeaker;
    [SerializeField] private GameObject _cardWhack;
    [SerializeField] private GameObject _cardAnswer;

    [Space] [Header("Buttons")] [SerializeField]
    private Button _whackCardNextButton;

    [SerializeField] private Button _answerCardNextButton;

    [Space] [Header("Events")] [SerializeField]
    private GameEvent _startCountdown;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void ShowSaveSpeakerCard()
    {
      _cardIntro.SetActive(false);
      _cardSaveSpeaker.SetActive(true);
    }

    public void ShowWhackCard()
    {
      _cardSaveSpeaker.SetActive(false);
      _cardWhack.SetActive(true);
    }

    public void SlideWhackCard()
    {
      _whackCardNextButton.onClick.RemoveAllListeners();
      _whackCardNextButton.onClick.AddListener(ShowAnswerCard);
    }

    public void ShowAnswerCard()
    {
      _cardWhack.SetActive(false);
      _cardAnswer.SetActive(true);
    }

    public void SlideAnswerCard()
    {
      _answerCardNextButton.onClick.RemoveAllListeners();
      _answerCardNextButton.onClick.AddListener(DisablePanel);
      PlayerPrefsHandler.SetTutorStateSSFSQ(true);
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
    
    public void DisablePanel()
    {
      _startCountdown.Raise();
      gameObject.SetActive(false);
    }
  }
}