using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ua.org.gdg.devfest
{
  public class Clickable3DObject : MonoBehaviour, IPointerClickHandler
  {
    public UnityEvent OnClick;


    public void OnPointerClick(PointerEventData eventData)
    {
      OnClick.Invoke();
    }
  }
}