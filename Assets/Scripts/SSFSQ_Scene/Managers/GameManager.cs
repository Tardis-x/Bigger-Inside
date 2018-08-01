﻿using System.Collections;
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
      
      StartCoroutine(OnSpeakerReady());
    }

    public void Answer()
    {
      OnAnswer();
      StartCoroutine(OnSpeakerReady());
    }

    public void Hit()
    {
      OnHit();
      StartCoroutine(OnSpeakerReady());
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
      StartCoroutine(OnSpeakerReady());
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

    private IEnumerator OnSpeakerReady()
    {
      while (AnimationManager.Instance.SpeakerAnimation.AnimationBusy)
      {
        yield return false;
      }
      
      AskQuestion();
    }
    
    private void OnHit()
    {
      AnimationManager.Instance.SpeakerAnimation.Hit();
      
      if(_currentQuestion.Good) SubtractStar();
      else _score++;
    }

    private void OnAnswer()
    {
      AnimationManager.Instance.SpeakerAnimation.Answer();
      
      if(!_currentQuestion.Good) SubtractBrain();
      else _score++;
    }
    
    private void SubtractStar()
    {
      if (_starsCount >= 0)
      {
        UIManager.Instance.HealthTimePanel.SubtractStar();
        _starsCount--;
      }

      if (_starsCount == 0)
      {
        GameOver();
        AnimationManager.Instance.CrowdControl.StartThrowing();
        AnimationManager.Instance.SpeakerAnimation.StartBeingScared();
      }
    }

    private void SubtractBrain()
    {
      if (_brainsCount >= 0)
      {
        UIManager.Instance.HealthTimePanel.SubtractBrain();
        _brainsCount--;
      }

      if (_brainsCount == 0)
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
      UIManager.Instance.GameOverPanel.SetScore(_score);
      UIManager.Instance.GameOverPanel.ShowPanel();
      UIManager.Instance.HealthTimePanel.HidePanel();
      UIManager.Instance.ButtonsToPauseMode();
      GameActive = false;
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