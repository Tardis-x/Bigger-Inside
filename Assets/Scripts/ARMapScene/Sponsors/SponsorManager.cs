using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [RequireComponent(typeof(ARMapRealtimeDB))]
  public class SponsorManager : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private List<SponsorGroup> _sponsorGroups;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private ARMapRealtimeDB _arMapRealtimeDb;
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------
    
    private void Awake()
    {
      _arMapRealtimeDb = GetComponent<ARMapRealtimeDB>();
    }

    private void Start()
    {
      _arMapRealtimeDb.LoadSponsorData(OnLoadSuccess, OnLoadError);
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
    
    private void OnLoadSuccess(List<SponsorModel> sponsorList)
    {
      ClearSponsorGroups();

      foreach (var sponsorModel in sponsorList)
      {
        FindSponsorModelGroup(sponsorModel);
      }
    }

    private void FindSponsorModelGroup(SponsorModel sponsorModel)
    {
      foreach (var sponsorGroup in _sponsorGroups)
      {
        var sponsorKeys = sponsorGroup.SponsorKeys;
        var sponsorModels = sponsorGroup.SponsorModels;
        
        if (sponsorKeys.Contains(sponsorModel.Id))
        {
          sponsorModels.Add(sponsorModel);
          return;
        }
      }
    }
    
    private void OnLoadError()
    {
    }

    private void ClearSponsorGroups()
    {
      foreach (var sponsorGroup in _sponsorGroups)
      {
        sponsorGroup.SponsorModels.Clear();
      }
    }
  }
}