namespace DeadMosquito.IosGoodies.Example
{
	using UnityEngine;

	public class IGMapsExample : MonoBehaviour
	{
#if UNITY_IOS
		public void OnOpenMapsLocation()
		{
			const float latitude = 52.3781019f;
			const float longitude = 4.8983588f;
			var amsterdamCentralStation = new IGMaps.Location(latitude, longitude);
			IGMaps.OpenMapLocation(amsterdamCentralStation, "Label-X", IGMaps.MapViewType.Satellite);
		}

		public void OnOpenMapsAddress()
		{
			const string address = "1,Infinite Loop,Cupertino,California";
			IGMaps.OpenMapAddress(address, "Label-Y", IGMaps.MapViewType.Hybrid);
		}

		public void OnSimpleMapSearch()
		{
			const string searchQuery = "Eiffel tower, Paris";
			IGMaps.PerformSearch(searchQuery);
		}

		public void OnSearchWithLocation()
		{
			const float latitude = 50.894967f;
			const float longitude = 4.341626f;
			var atomiumLocation = new IGMaps.Location(latitude, longitude);
			const int zoom = 5;
			IGMaps.PerformSearch("Fish restaurant", atomiumLocation, zoom, IGMaps.MapViewType.Standard);
		}

		public void OnGetDirections()
		{
			const string sourceAddress = "221B Baker Street, London, Great Britain";
			const string destAddress = "Manchester, Great Britain";
			IGMaps.GetDirections(destAddress, sourceAddress, IGMaps.TransportType.ByCar, IGMaps.MapViewType.Hybrid);
		}
#endif
	}
}