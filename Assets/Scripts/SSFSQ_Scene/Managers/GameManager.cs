using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace ua.org.gdg.devfest
{
  public class GameManager : Singleton<GameManager>
  {
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
    [SerializeField] private PlayerChoiceVariable _playerChoice;

    [Space]
    [Header("Events")] 
    [SerializeField] private GameEvent _onGameOver;
    [SerializeField] private GameEvent _onNewQuestion;
    
    [Space]
    [Header("Values")]
    [SerializeField] private float _timeForAnswer;

    [Space] 
    [Header("Prefabs")] 
    [SerializeField] private GameObject _environment;

    [Space]
    [Header("Targets")]
    [SerializeField] private GameObject _imageTarget;
    [SerializeField] private GameObject _planeFinder;

    [Space]
    [Header("Debug")]
    [SerializeField] private Text _text;
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      var arCoreSupport = ARCoreHelper.CheckArCoreSupport();
      PrepareScene(arCoreSupport);
      
      _text.text = "ARCore support: " + arCoreSupport;
    }
    
    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnGameStart()
    {
      Debug.Log("Game Manager: OnGameStart");
      Invoke("NewGame", 0.1f);
    }

    public void OnAnswer()
    {
      Debug.Log("Game Manager: OnAnswer");
      _playerChoice.Value = PlayerChoice.Answer;
    }

    public void OnHit()
    {
      Debug.Log("Game Manager: OnHit");
      _playerChoice.Value = PlayerChoice.Hit;
    }

    public void OnSpeakerAnimationEnd()
    {
      switch (_playerChoice.Value)
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
      Debug.Log("Game Manager: OnTimeout");
      SubtractStar();
      SubtractBrain();
      AskQuestion();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void NewGame()
    {
      if(GameActive) return;
      
      GameActive = true;

      ResetHealthAndScore();

      AskQuestion();
    }

    public void Answer()
    {
      if(!_currentQuestion.Value.IsGood) SubtractBrain();
      else _score.RuntimeValue++;
      
      AskQuestion();
    }

    public void Hit()
    {
      if(_currentQuestion.Value.IsGood) SubtractStar();
      else _score.RuntimeValue++;
      
      if(_starsCount.RuntimeValue <= 0) return;
      
      AskQuestion();
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private void PrepareScene(bool arCoreSupport)
    {
      _planeFinder.gameObject.SetActive(arCoreSupport);
      
      _imageTarget.SetActive(!arCoreSupport);
      
      if (!arCoreSupport)
      {
        Instantiate(_environment, _imageTarget.transform);        
      }
    }

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
      Debug.Log("Game Manager: RAISING OnNewQuestion");
      _onNewQuestion.Raise();
    }
    
    private void SubtractStar()
    {
      if (_starsCount.RuntimeValue > 0)
      {
        _starsCount.RuntimeValue--;
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

    public void GameOver()
    {
      GameActive = false;
      Debug.Log("RAISING OnGameOver");
      _onGameOver.Raise();
    }

    private void ResetBrains()
    {
      UIManager.Instance.HealthTimePanel.SetBrainsCount(_brainsCount.InitialValue);
      _brainsCount.ResetValue();
    }

    private void ResetStars()
    {
      UIManager.Instance.HealthTimePanel.SetStarsCount(_starsCount.InitialValue);
      _starsCount.ResetValue();
    }
  }
}