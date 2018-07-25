using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace ua.org.gdg.devfest
{
  public class HealthTimePanelScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private Image _timerImage;
    [SerializeField] private RectTransform _brainsContainer;
    [SerializeField] private RectTransform _starsContainer;
    [SerializeField] private RectTransform _brainPrefab;
    [SerializeField] private RectTransform _starPrefab;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void SetStarsCount(int count)
    {
      for (int i = 0; i < count; i++)
      {
        var star = Instantiate(_starPrefab);
        star.SetParent(_starsContainer);
      }
    }

    public void SubtractStar()
    {
      var star = _starsContainer.GetComponentsInChildren<RectTransform>().First(x => x.parent == _starsContainer);
      Destroy(star.gameObject);
    }

    public void SetBrainsCount(int count)
    {
      for (int i = 0; i < count; i++)
      {
        var brain = Instantiate(_brainPrefab);
        brain.SetParent(_brainsContainer);
      }
    }

    public void SubtractBrain()
    {
      var brain = _brainsContainer.GetComponentsInChildren<RectTransform>().First(x => x.parent == _brainsContainer);
      Destroy(brain.gameObject);
    }

    public void ClearStarsContainer()
    {
      var stars = _starsContainer.GetComponentsInChildren<RectTransform>().Where(x => x.parent == _starsContainer);

      foreach (var s in stars)
      {
        Destroy(s.gameObject);
      }
    }
    
    public void ClearBrainsContainer()
    {
      var brains = _brainsContainer.GetComponentsInChildren<RectTransform>().Where(x => x.parent == _brainsContainer);

      foreach (var b in brains)
      {
        Destroy(b.gameObject);
      }
    }
  }
}