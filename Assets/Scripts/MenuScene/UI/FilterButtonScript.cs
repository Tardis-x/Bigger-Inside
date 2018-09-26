using UnityEngine;
using UnityEngine.UI;

public class FilterButtonScript : MonoBehaviour 
{
  //---------------------------------------------------------------------
  // Editor
  //---------------------------------------------------------------------

  [SerializeField] private Color _activeColor;
  [SerializeField] private Color _inactiveColor;
  [SerializeField] private Image _buttonImage;
  [SerializeField] private Sprite _borderSprite;
  [SerializeField] private Sprite _fillSprite;
  [SerializeField] private Text _buttonText;
  
  //---------------------------------------------------------------------
  // Properties
  //---------------------------------------------------------------------

  private bool _filled;
  
  //---------------------------------------------------------------------
  // Public
  //---------------------------------------------------------------------

  public void Toggle()
  {
    if (_filled)
    {
      _buttonText.color = _inactiveColor;
      _buttonImage.color = _inactiveColor;
      _buttonImage.sprite = _borderSprite;
    }
    else
    {
      _buttonText.color = Color.white;
      _buttonImage.sprite = _fillSprite;
      _buttonImage.color = _activeColor;
    }

    _filled = !_filled;
  }

  public void Reset()
  {
    _buttonText.color = _inactiveColor;
    _buttonImage.sprite = _borderSprite;
    _buttonImage.color = _inactiveColor;
    _filled = false;
  }
}
