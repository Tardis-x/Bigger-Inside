using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
  //---------------------------------------------------------------------
  // Editor
  //---------------------------------------------------------------------
  
  [SerializeField] private float _fadeRate;
  
  //---------------------------------------------------------------------
  // Internal
  //---------------------------------------------------------------------
  
  private Image _image;
  private float _targetAlpha;
  
  //---------------------------------------------------------------------
  // Messages
  //---------------------------------------------------------------------
  
  // Use this for initialization
  void Start ()
  {
    _image = GetComponent<Image>();
    if(_image==null)
    {
      Debug.LogError("Error: No image on " + name);
    }
    _targetAlpha = _image.color.a;
  }
     
  // Update is called once per frame
  void Update () 
  {
    Color curColor = _image.color;
    float alphaDiff = Mathf.Abs(curColor.a - _targetAlpha);
    
    if (alphaDiff>0.0001f)
    {
      curColor.a = Mathf.Lerp(curColor.a, _targetAlpha, _fadeRate * Time.deltaTime);
      _image.color = curColor;
    }
  }
 
  //---------------------------------------------------------------------
  // Helpers
  //---------------------------------------------------------------------
  
  public void FadeOut()
  {
    _targetAlpha = 0.0f;
  }
 
  public void FadeIn()
  {
    gameObject.SetActive(true);
    _targetAlpha = 1.0f;
  }
}