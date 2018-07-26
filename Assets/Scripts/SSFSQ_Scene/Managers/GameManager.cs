using System;
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

      UIManager.Instance.GameOverPanel.HidePanel();
      UIManager.Instance.HealthTimePanel.ResetPanel();
      UIManager.Instance.HealthTimePanel.ShowPanel();
      
      ResetBrains();
      ResetStars();
      
      AskQuestion();
    }

    public void StopGame()
    {
      if(!GameActive) return;
      
      GameActive = false;
      
      UIManager.Instance.GameOverPanel.HidePanel();
      UIManager.Instance.HealthTimePanel.HidePanel();
    }

    public void Answer()
    {
      OnAnswer();
      AskQuestion();
    }

    public void Hit()
    {
      OnHit();
      AskQuestion();
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private int _starsCount;
    private int _brainsCount;
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

    private void OnTimeout()
    {
      SubtractStar();
      SubtractBrain();
      AskQuestion();
    }

    private void AskQuestion()
    {
      _currentQuestion = GetQuestion();
      UIManager.Instance.ScreenQuestionText.text = _currentQuestion.Text;
      UIManager.Instance.HealthTimePanel.StartCountdown(_timeForAnswer, OnTimeout);
    }
    
    private void OnHit()
    {
      if(_currentQuestion.Good) SubtractStar();
    }

    private void OnAnswer()
    {
      if(!_currentQuestion.Good) SubtractBrain();
    }
    
    private void SubtractStar()
    {
      if (_starsCount >= 0)
      {
        UIManager.Instance.HealthTimePanel.SubtractStar();
        _starsCount--;
      }

      if (_starsCount == 0) GameOver();
    }

    private void SubtractBrain()
    {
      if (_brainsCount >= 0)
      {
        UIManager.Instance.HealthTimePanel.SubtractBrain();
        _brainsCount--;
      }

      if (_brainsCount == 0) GameOver();
    }
    
    private QuestionModel GetQuestion()
    {
      int index = new Random().Next(_questions.Length);
      return _questions[index];
    }

    private void GameOver()
    {
      UIManager.Instance.GameOverPanel.ShowPanel();
      UIManager.Instance.HealthTimePanel.HidePanel();
      GameActive = false;
      UIManager.Instance.SetPlayButton(true);
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