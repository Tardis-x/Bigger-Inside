using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace ua.org.gdg.devfest
{
  public class GameManager : MonoBehaviour
  {
    private PlayerChoice _playerChoice;
    
    //Questions
    private readonly QuestionModel[] _questions = {new QuestionModel
      {
        IsGood = true,
        Text = "Good question"
      }, 
      new QuestionModel
      {
        IsGood = false,
        Text = "Bad question"
      },
      new QuestionModel
      {
        IsGood = false,
        Text = "Is there any difference between '===' and '='?"
      }
    };
    
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public bool GameActive { get; private set; }

    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    
    [Header("Public Variables")]
    [SerializeField] private IntVariable _score;
    [SerializeField] private IntVariable _brainsCount;
    [SerializeField] private IntVariable _starsCount;
    [SerializeField] private QuestionVariable _currentQuestion;

    [Header("Submanagers")] 
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private SSFSQAudioManager _ssfsqAudioManager;

    [Space]
    [Header("Events")] 
    [SerializeField] private GameEvent _onGameOver;
    [SerializeField] private GameEvent _onNewQuestion;
    
    [Space]
    [Header("Values")]
    [SerializeField] private float _timeForAnswer;
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
        ToMainMenu();
      }
    }
    
    private void OnDestroy()
    {
      Time.timeScale = 1;
    }

    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnGameStart()
    {
      Invoke("NewGame", 0.1f);
    }

    public void OnAnswer()
    {
      _playerChoice = PlayerChoice.Answer;
    }

    public void OnHit()
    {
      _playerChoice = PlayerChoice.Hit;
    }

    public void OnSpeakerAnimationEnd()
    {
      switch (_playerChoice)
      {
          case PlayerChoice.Answer:
            Answer();
            break;
          case PlayerChoice.Hit:
            Hit();
            break;
          default:
            return;
      }
    }

    public void OnTimeout()
    {
      SubtractStar();
      SubtractBrain();
      AskQuestion();
      
      HandleWrongActionAudio();
    }

    public void OnTrackingLost()
    {
      PauseGame();
    }

    public void OnTrackingFound()
    {
      ResumeGame();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void ToMainMenu()
    {
      SceneManager.LoadScene(Scenes.SCENE_MENU);
    }
    
    public void NewGame()
    {
      if(GameActive) return;
      
      GameActive = true;

      ResetHealthAndScore();

      AskQuestion();
    }

    public void Answer()
    {
      if (!_currentQuestion.Value.IsGood)
      {
        SubtractBrain();
        HandleWrongActionAudio();
      }
      else
      {
        _ssfsqAudioManager.PlayRightAction();
        _score.RuntimeValue++;
      }
      
      AskQuestion();
    }

    public void Hit()
    {
      if (_currentQuestion.Value.IsGood)
      {
        SubtractStar();
        HandleWrongActionAudio();
      }
      else
      {
        _ssfsqAudioManager.PlayRightAction();
        _score.RuntimeValue++;
      }
      
      if(_starsCount.RuntimeValue <= 0) return;
      
      AskQuestion();
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private void ResetHealthAndScore()
    {
      ResetBrains();
      ResetStars();
      _score.ResetValue();
    }

    private void AskQuestion()
    {
      if(_brainsCount.RuntimeValue <= 0 || _starsCount.RuntimeValue <= 0) return;
      
      _currentQuestion.Value = GetQuestion();
      _onNewQuestion.Raise();
    }
    
    private void SubtractStar()
    {
      if (_starsCount.RuntimeValue > 0)
      {
        _starsCount.RuntimeValue--;
        _uiManager.SubstractStar();
      }

      if (_starsCount.RuntimeValue <= 0)
      {
        GameOver();
      }
    }

    private void SubtractBrain()
    {
      if (_brainsCount.RuntimeValue > 0)
      {
        _brainsCount.RuntimeValue--;
        _uiManager.SubstractBrain();
      }

      if (_brainsCount.RuntimeValue <= 0)
      {
        GameOver();
      }
    }
    
    private QuestionModel GetQuestion()
    {
      var index = new Random().Next(_questions.Length);
      return _questions[index];
    }
    
    private void HandleWrongActionAudio()
    {
      if (GameActive)
        _ssfsqAudioManager.PlayWrongAction();
    }

    private void GameOver()
    {
      GameActive = false;
      _onGameOver.Raise();
    }

    private void ResetBrains()
    {
      _brainsCount.ResetValue();
    }

    private void ResetStars()
    {
      _starsCount.ResetValue();
    }
    
    private void PauseGame()
    {
      if(!GameActive) return;
      
      Time.timeScale = 0;
    }

    private void ResumeGame()
    {
      Time.timeScale = 1;
    }
  }
}