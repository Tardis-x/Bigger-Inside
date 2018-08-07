using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace ua.org.gdg.devfest
{
  public class GameManager : Singleton<GameManager>
  {
    private Environment _environmentInstance;
    
    private QuestionModel _currentQuestion;
    
    //Questions
    private readonly QuestionModel[] _questions = {new QuestionModel
      {
        Good = true,
        Text = "Good question"
      }, 
      new QuestionModel
      {
        Good = false,
        Text = "Bad question"
      }
    };
    
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public bool GameActive { get; private set; }

    public Environment EnvironmentInstance
    {
      get { return _environmentInstance; }
    }

    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    
    [Header("Public Variables")]
    [SerializeField] private IntVariable _score;
    [SerializeField] private IntVariable _brainsCount;
    [SerializeField] private IntVariable _starsCount;

    [Space]
    [Header("Events")] 
    [SerializeField] private GameEvent _onGameOver;
    
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
      arCoreSupport = false;
      PrepareScene(arCoreSupport);
      
      _text.text = "ARCore support: " + arCoreSupport;
    }
    
    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnEnvironmentInstantiated()
    {
      _environmentInstance = FindObjectOfType<Environment>();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void NewGame()
    {
      if(GameActive) return;
      
      GameActive = true;

      UIManager.Instance.ResetUI();
      ResetHealthAndScore();
      
      StartCoroutine(AwaitSpeakerReady(AskQuestion));
    }

    public void Answer()
    {
      AnimationManager.Instance.SpeakerAnimation.Answer(_currentQuestion.Good);
      
      if(GameActive) StartCoroutine(AwaitSpeakerReady(OnAnswer));
    }

    public void Hit()
    {
      AnimationManager.Instance.SpeakerAnimation.Hit();
      
      if(GameActive) StartCoroutine(AwaitSpeakerReady(OnHit));
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private void PrepareScene(bool arCoreSupport)
    {
      _planeFinder.gameObject.SetActive(arCoreSupport);
      
      _imageTarget.SetActive(!arCoreSupport);
      AnimationManager.Instance.ShowSneaker(!arCoreSupport);
      
      if (!arCoreSupport)
      {
        var environment = Instantiate(_environment, _imageTarget.transform);        
        _environmentInstance = environment.GetComponent<Environment>();
      }
    }

    private void ResetHealthAndScore()
    {
      ResetBrains();
      ResetStars();
      _score.RuntimeValue = 0;
    }

    private void OnTimeout()
    {
      SubtractStar();
      SubtractBrain();
      StartCoroutine(AwaitSpeakerReady(AskQuestion));
    }

    private void AskQuestion()
    {
      if(!GameActive) return;
      
      _currentQuestion = GetQuestion();
      UIManager.Instance.ScreenQuestionText.text = _currentQuestion.Text;
      UIManager.Instance.HealthTimePanel.StartCountdown(_timeForAnswer, OnTimeout);
      UIManager.Instance.ButtonsToPlayMode();
      AnimationManager.Instance.CrowdControl.AskQuestion();
    }

    private IEnumerator AwaitSpeakerReady(Action onSpeakerReady)
    {
      while (AnimationManager.Instance.SpeakerAnimation.AnimationBusy)
      {
        yield return false;
      }

      onSpeakerReady();
    }
    
    private void OnHit()
    {
      if(_currentQuestion.Good) SubtractStar();
      else _score.RuntimeValue++;
      AskQuestion();
    }

    private void OnAnswer()
    {
      if(!_currentQuestion.Good) SubtractBrain();
      else _score.RuntimeValue++;
      AskQuestion();
    }
    
    private void SubtractStar()
    {
      if (_starsCount.RuntimeValue > 0)
      {
        UIManager.Instance.HealthTimePanel.SubtractStar();
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
        UIManager.Instance.HealthTimePanel.SubtractBrain();
        _brainsCount.RuntimeValue--;
      }

      if (_brainsCount.RuntimeValue <= 0)
      {
        GameOver();
      }
    }
    
    private QuestionModel GetQuestion()
    {
      int index = new Random().Next(_questions.Length);
      return _questions[index];
    }

    public void GameOver()
    {
      GameActive = false;
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