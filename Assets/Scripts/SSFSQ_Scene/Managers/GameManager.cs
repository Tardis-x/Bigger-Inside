using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace ua.org.gdg.devfest
{
  public class GameManager : Singleton<GameManager>
  {
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public bool GameActive { get; private set; }

    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private int _maxBrains;
    [SerializeField] private int _maxStars;
    [SerializeField] private float _timeForAnswer;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void NewGame()
    {
      if(GameActive) return;
      
      GameActive = true;

      ResetUI();
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

    private int _starsCount;
    private int _brainsCount;
    private int _score;
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

    private void ResetUI()
    {
      UIManager.Instance.GameOverPanel.HidePanel();
      UIManager.Instance.HealthTimePanel.ResetPanel();
      UIManager.Instance.HealthTimePanel.ShowPanel();
      UIManager.Instance.ButtonsToPlayMode();
    }

    private void ResetHealthAndScore()
    {
      ResetBrains();
      ResetStars();
      _score = 0;
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
      else _score++;
      AskQuestion();
    }

    private void OnAnswer()
    {
      if(!_currentQuestion.Good) SubtractBrain();
      else _score++;
      AskQuestion();
    }
    
    private void SubtractStar()
    {
      if (_starsCount > 0)
      {
        UIManager.Instance.HealthTimePanel.SubtractStar();
        _starsCount--;
      }

      if (_starsCount <= 0)
      {
        GameOver();
        AnimationManager.Instance.CrowdControl.StartThrowing();
        AnimationManager.Instance.SpeakerAnimation.StartBeingScared();
      }
    }

    private void SubtractBrain()
    {
      if (_brainsCount > 0)
      {
        UIManager.Instance.HealthTimePanel.SubtractBrain();
        _brainsCount--;
      }

      if (_brainsCount <= 0)
      {
        GameOver();
        AnimationManager.Instance.SpeakerAnimation.Die();
        
        if(_starsCount == 0) AnimationManager.Instance.CrowdControl.StartThrowing();
      }
    }
    
    private QuestionModel GetQuestion()
    {
      int index = new Random().Next(_questions.Length);
      return _questions[index];
    }

    private void GameOver()
    {
      GameActive = false;
      UIManager.Instance.GameOverPanel.SetScore(_score);
      UIManager.Instance.GameOverPanel.ShowPanel();
      UIManager.Instance.HealthTimePanel.HidePanel();
      UIManager.Instance.ScreenQuestionTextSetActive(false);
      UIManager.Instance.ButtonsToPauseMode();
    }

    private void ResetBrains()
    {
      UIManager.Instance.HealthTimePanel.SetBrainsCount(_maxBrains);
      _brainsCount = _maxBrains;
    }

    private void ResetStars()
    {
      UIManager.Instance.HealthTimePanel.SetStarsCount(_maxStars);
      _starsCount = _maxStars;
    }
  }
}