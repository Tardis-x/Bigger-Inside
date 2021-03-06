using System.Collections.Generic;
using System.Linq;
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

    private readonly List<SponsorModel> _sponsorWithNameList = new List<SponsorModel>();
    
    private Animator _animator;
    private bool _isSelected;
    private InfoCoinElement _lastSelectedElement;

    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public List<SponsorModel> SponsorWithNameList
    {
      get { return _sponsorWithNameList; }
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
      if (_isSelected) return;
      
      _isSelected = true;
      
      HandleSponsorNames();
      HandleCoinElements();
      
      _infoCoinsManager.OnCoinGroupSelected(this);
      
      _animator.ResetTrigger("Close");
      _animator.SetTrigger("Open");
    }

    public void Deselect()
    {
      if (!_isSelected) return;
      
      _isSelected = false;
      
      DeselectActiveElement();
      
      _animator.SetTrigger("Close");
      _animator.ResetTrigger("Open");
    }
    
    public void OnSponsorSelected(string sponsorId)
    {
      foreach (var infoCoinElement in _infoCoinElements)
      {
        if (string.Equals(infoCoinElement.SponsorId, sponsorId))
        {
          UpdateSelection(infoCoinElement);
        }
      }
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void HandleSponsorNames()
    {
      _sponsorWithNameList.Clear();

      foreach (var sponsorModel in _sponsorGroup.SponsorModels)
      {
        if (!string.IsNullOrEmpty(sponsorModel.Name))
        {
          _sponsorWithNameList.Add(sponsorModel);
        } 
      }
    }
    
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
      
      foreach (var sponsorModel in _sponsorWithNameList)
      {
        if (sponsorModel.Id.Equals(infoCoinElement.SponsorId))
        {
          hasSponsorData = true;
          break;
        }
      }

      return hasSponsorData;
    }
    
    private void UpdateSelection(InfoCoinElement infoCoinElement)
    {
      DeselectActiveElement();

      infoCoinElement.Select();
      _lastSelectedElement = infoCoinElement;
    }
    
    private void DeselectActiveElement()
    {
      if (_lastSelectedElement != null) _lastSelectedElement.Deselect();
      _lastSelectedElement = null;
    }
  }
}