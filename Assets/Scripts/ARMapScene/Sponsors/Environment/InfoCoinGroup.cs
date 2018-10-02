using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class InfoCoinGroup : MonoBehaviour, ISelectable
  {    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private InfoCoinsManager _infoCoinsManager;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private Animator _animator;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      _animator = GetComponent<Animator>();
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void Select()
    {
      _infoCoinsManager.OnCoinGroupSelected(this);
      
      _animator.ResetTrigger("Close");
      _animator.SetTrigger("Open");
    }

    public void Deselect()
    {
      _animator.SetTrigger("Close");
      _animator.ResetTrigger("Open");
    }
  }
}