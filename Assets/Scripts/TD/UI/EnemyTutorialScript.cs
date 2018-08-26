using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class EnemyTutorialScript : MonoBehaviour
  {
    [SerializeField] private Image _happyImage;
    
    public void EnableHappyIcon(int value)
    {
      _happyImage.gameObject.SetActive(value == 1);
    }
  }
}