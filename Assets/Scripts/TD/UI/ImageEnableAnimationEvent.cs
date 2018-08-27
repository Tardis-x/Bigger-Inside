using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class ImageEnableAnimationEvent : MonoBehaviour
  {
    [SerializeField] private Image _image;
    
    public void SetImageEnabled(int value)
    {
      _image.gameObject.SetActive(value == 1);
    }
  }
}