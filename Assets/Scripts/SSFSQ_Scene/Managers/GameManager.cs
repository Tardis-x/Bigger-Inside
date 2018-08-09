﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace ua.org.gdg.devfest
{
  public class GameManager : Singleton<GameManager>
  {
    private Environment _environmentInstance;
    
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

    private PlayerChoice _playerChoice;
    
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
    [SerializeField] private QuestionVariable _currentQuestion;

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
    [Header("Animations")] 
    [SerializeField] private AnimationClip _talkingClip;
    [SerializeField] private AnimationClip _yellingClip;
    [SerializeField] private AnimationClip _angryClip;

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

    public void OnEnvironmentInstantiated()
    {
      _environmentInstance = FindObjectOfType<Environment>();
    }

    public void OnGameStart()
    {
      Debug.Log("Game Manager: OnGameStart");
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
      AskQuestion();
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
      _score.ResetValue();
    }

    private void OnTimeout()
    {
      SubtractStar();
      SubtractBrain();
      AskQuestion();
    }

    private void AskQuestion()
    {
      if(!GameActive) return;
      
      _currentQuestion.Value = GetQuestion();
      UIManager.Instance.ScreenQuestionText.text = _currentQuestion.Value.Text;
      UIManager.Instance.HealthTimePanel.StartCountdown(_timeForAnswer, OnTimeout);
      UIManager.Instance.ButtonsToPlayMode();
      AnimationManager.Instance.CrowdControl.AskQuestion();
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