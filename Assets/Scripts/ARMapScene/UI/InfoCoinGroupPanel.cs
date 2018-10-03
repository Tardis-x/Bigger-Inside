using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace ua.org.gdg.devfest
{
  public class InfoCoinGroupPanel : MonoBehaviour
  {
    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Prefab")] 
    [SerializeField] private GameObject _sponsorItemPrefab; 
    
    [Space]
    [Header("UI")] 
    [SerializeField] private HorizontalScrollSnap _horizontalScrollSnap;
    [SerializeField] private GameObject _scrollRectContent;

    [Space] 
    [Header("Data")] 
    [SerializeField] private SponsorLogoData _sponsorLogoData;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private readonly List<SponsorItem> _sponsorItems = new List<SponsorItem>();
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _horizontalScrollSnap.OnSelectionChangeEndEvent.AddListener(OnScrollSnapPositionChanged);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void OpenPanel(List<SponsorModel> sponsorModelList)
    {
      GenerateSponsorItems(sponsorModelList);
      gameObject.SetActive(true);
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
    
    private void GenerateSponsorItems(List<SponsorModel> sponsorModelList)
    {
      ClearList();
      
      foreach (var sponsorModel in sponsorModelList)
      {
        var sponsorItemPrefab = Instantiate(_sponsorItemPrefab, _scrollRectContent.transform);

        var sponsorItem = sponsorItemPrefab.GetComponent<SponsorItem>();
        sponsorItem.SetSponsorModel(sponsorModel);
        UpdateSponsorItemLogo(sponsorItem, sponsorModel);
          
        _sponsorItems.Add(sponsorItem);
      }
    }

    private void UpdateSponsorItemLogo(SponsorItem sponsorItem, SponsorModel sponsorModel)
    {
      foreach (var sponsorLogo in _sponsorLogoData.SponsorLogos)
      {
        if (!string.Equals(sponsorModel.Name, sponsorLogo.Name)) continue;
        
        sponsorItem.SetSponsorLogo(sponsorLogo);
        return;
      }
    }

    private void ClearList()
    {
      if (_sponsorItems.Count == 0) return;
      
      GameObject[] scrollSnapChildren;
      _horizontalScrollSnap.RemoveAllChildren(out scrollSnapChildren);
      
      foreach (var child in scrollSnapChildren)
      {
        Destroy(child);
      }
      
      _sponsorItems.Clear();
    }
    
    private void OnScrollSnapPositionChanged(int position)
    {
    }
  }
}