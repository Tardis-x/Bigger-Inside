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
    [SerializeField] private string _buttonName; 
    [SerializeField] private bool _hasSchedule;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private Animator _selectionAnimator;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------
    
    private void Awake()
    {
      _selectionAnimator = GetComponent<Animator>();
    }

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
    
    public string ButtonName
    {
      get { return _buttonName; }
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public virtual void Select()
    {  
      _infoCoinsManager.OnCoinSelected(this);
      _selectionAnimator.SetBool("IsSelected", true);
    }

    public virtual void Deselect()
    {
      _selectionAnimator.SetBool("IsSelected", false);
    }
  }
}