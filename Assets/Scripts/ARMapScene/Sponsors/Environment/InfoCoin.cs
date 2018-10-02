using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class InfoCoin : MonoBehaviour, ISelectable
  {
    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private InfoCoinsManager _infoCoinsManager;

    [Space]
    [Header("Values")] 
    [SerializeField] private string _name; 
    [SerializeField] private bool _hasSchedule;
    
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public bool HasSchedule
    {
      get { return _hasSchedule; }
    }

    public string Name
    {
      get { return _name; }
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