using System;
using UnityEngine;
using UnityEngine.UI;
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

      UIManager.Instance.GameOverPanel.gameObject.SetActive(false);
      UIManager.Instance.HealthTimePanel.gameObject.SetActive(true);

      UIManager.Instance.HealthTimePanel.ResetPanel();
      
      ResetBrains();
      ResetStars();
    }

    public void StopGame()
    {
      if(!GameActive) return;
      
      GameActive = false;
      
      UIManager.Instance.GameOverPanel.gameObject.SetActive(false);
      UIManager.Instance.HealthTimePanel.gameObject.SetActive(false);
    }

    public void SubtractStar()
    {
      if (_starsCount >= 0)
      {
        UIManager.Instance.HealthTimePanel.SubtractStar();
        _starsCount--;
      }

      if (_starsCount == 0) GameOver();
    }

    public void SubtractBrain()
    {
      if (_brainsCount >= 0)
      {
        UIManager.Instance.HealthTimePanel.SubtractBrain();
        _brainsCount--;
      }

      if (_brainsCount == 0) GameOver();
    }

    public void AskQuestion()
    {
      QuestionModel q = GetQuestion();
      UIManager.Instance.ScreenQuestionText.text = q.Text;
      UIManager.Instance.HealthTimePanel.StartCountdown(_timeForAnswer, OnTimeOut);
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private int _starsCount;
    private int _brainsCount;
    
    //Questions
    private readonly QuestionModel[] _questions = {new QuestionModel
    {
      Good = true,
      Text = "Good question"
    }, 
      new QuestionModel
      {
        Good = false,
        Text = "BadQuestion"
      }
    };

    private void OnTimeOut(QuestionModel qm)
    {
      if(qm.Good) SubtractStar();
      else SubtractBrain();
    }
    
    private QuestionModel GetQuestion()
    {
      int index = new Random().Next(_questions.Length);
      return _questions[index];
    }

    private void GameOver()
    {
      UIManager.Instance.GameOverPanel.gameObject.SetActive(true);
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