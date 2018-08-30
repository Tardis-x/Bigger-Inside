using UnityEngine;

namespace ua.org.gdg.devfest
{
    public class PlaneTrackableEventHandler : DefaultTrackableEventHandler
    {

        //---------------------------------------------------------------------
        // Editor
        //---------------------------------------------------------------------

        [SerializeField] private GameEvent _onTrackingLost;
        
        //---------------------------------------------------------------------
        // Messages
        //---------------------------------------------------------------------

        protected override void OnTrackingLost()
        {
            base.OnTrackingLost();
            _onTrackingLost.Raise();
        }
    }
}