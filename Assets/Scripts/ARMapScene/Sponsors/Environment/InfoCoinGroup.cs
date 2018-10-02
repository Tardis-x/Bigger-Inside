using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class InfoCoinGroup : MonoBehaviour, ISelectable
  {    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private InfoCoinsManager _infoCoinsManager;

    [Header("Data")] 
    [SerializeField] private SponsorGroup _sponsorGroup;
    [SerializeField] private List<InfoCoinElement> _infoCoinElements;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private Animator _animator;
    
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public bool HasSponsors
    {
      get { return _sponsorGroup.SponsorModels.Count > 0; }
    }

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
      HandleCoinElements();
      
      _infoCoinsManager.OnCoinGroupSelected(this);
      
      _animator.ResetTrigger("Close");
      _animator.SetTrigger("Open");
    }

    public void Deselect()
    {
      _animator.SetTrigger("Close");
      _animator.ResetTrigger("Open");
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void HandleCoinElements()
    {
      foreach (var infoCoinElement in _infoCoinElements)
      {
        var infoCoinHasName = IsInfoCoinHasName(infoCoinElement);

        infoCoinElement.gameObject.SetActive(infoCoinHasName);
      }
    }

    private bool IsInfoCoinHasName(InfoCoinElement infoCoinElement)
    {
      var hasSponsorData = false;
      
      foreach (var sponsorModel in _sponsorGroup.SponsorModels)
      {
        if (sponsorModel.Id.Equals(infoCoinElement.SponsorId) && !string.IsNullOrEmpty(sponsorModel.Name))
        {
          hasSponsorData = true;
          break;
        }
      }

      return hasSponsorData;
    }
  }
}