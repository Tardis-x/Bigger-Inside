using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class InfoCoin : MonoBehaviour, ISelectable
  {
    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private InfoCoinsManager _infoCoinsManager;
    [SerializeField] private bool _hasSchedule;
    
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public bool HasSchedule
    {
      get { return _hasSchedule; }
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public virtual void Select()
    {
      _infoCoinsManager.OnCoinSelected(this);
    }

    public virtual void Deselect()
    {
    }
  }
}