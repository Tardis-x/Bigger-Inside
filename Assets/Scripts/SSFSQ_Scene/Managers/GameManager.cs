using UnityEngine;

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
    [SerializeField] private HealthTimePanelScript _healthPanel;
    [SerializeField] private GameObject _gameOverPanel;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void NewGame()
    {
      GameActive = true;

      _gameOverPanel.SetActive(false);
      _healthPanel.gameObject.SetActive(true);

      _healthPanel.ClearBrainsContainer();
      _healthPanel.ClearStarsContainer();

      _healthPanel.SetBrainsCount(_maxBrains);
      _healthPanel.SetStarsCount(_maxStars);

      _brainsCount = _maxBrains;
      _starsCount = _maxStars;
    }

    public void SubtractStar()
    {
      if (_starsCount >= 0)
      {
        _healthPanel.SubtractStar();
        _starsCount--;
      }

      if (_starsCount == 0) GameOver();
    }

    public void SubtractBrain()
    {
      if (_brainsCount >= 0)
      {
        _healthPanel.SubtractBrain();
        _brainsCount--;
      }

      if (_brainsCount == 0) GameOver();
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private int _starsCount;
    private int _brainsCount;

    private void GameOver()
    {
      _gameOverPanel.SetActive(true);
      GameActive = false;
    }
  }
}