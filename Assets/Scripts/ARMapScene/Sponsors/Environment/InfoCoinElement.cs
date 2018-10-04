using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class InfoCoinElement : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private string _sponsorId;
    
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public string SponsorId
    {
      get { return _sponsorId; }
    }
    
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
    // Public
    //---------------------------------------------------------------------

    public void Select()
    {  
      _selectionAnimator.SetBool("IsSelected", true);
    }

    public void Deselect()
    {
      _selectionAnimator.SetBool("IsSelected", false);
    }
  }
}