// 
// DOCUMENTATION FOR THIS CLASS: https://github.com/TarasOsiris/iOS-Goodies-Docs/wiki/IGMaps.cs
//



#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using UnityEngine;

	public static class IGMaps
	{
		/// <summary>
		///     Map view type.
		/// </summary>
		public enum MapViewType
		{
			/// <summary>
			///     The standard view.
			/// </summary>
			Standard = 0,

			/// <summary>
			///     The satellite view.
			/// </summary>
			Satellite = 1,

			/// <summary>
			///     The hybrid view.
			/// </summary>
			Hybrid = 2,

			/// <summary>
			///     The transit view.
			/// </summary>
			Transit = 3
		}

		/// <summary>
		///     Transport type.
		/// </summary>
		public enum TransportType
		{
			/// <summary>
			///     By car.
			/// </summary>
			ByCar = 0,

			/// <summary>
			///     By foot.
			/// </summary>
			ByFoot = 1,

			/// <summary>
			///     By public transport.
			/// </summary>
			ByPublicTransport = 2
		}

		const string MapsUrl = "http://maps.apple.com/?";

		public const int MinMapZoomLevel = 2;
		public const int MaxMapZoomLevel = 23;
		public const int DefaultMapZoomLevel = 7;

		static readonly Dictionary<MapViewType, string> _mapViews = new Dictionary<MapViewType, string>
		{
			{MapViewType.Standard, "m"},
			{MapViewType.Satellite, "k"},
			{MapViewType.Hybrid, "h"},
			{MapViewType.Transit, "r"}
		};

		static readonly Dictionary<TransportType, string> _transportTypes = new Dictionary<TransportType, string>
		{
			{TransportType.ByCar, "d"},
			{TransportType.ByFoot, "w"},
			{TransportType.ByPublicTransport, "r"}
		};

		/// <summary>
		///     Opens the apple maps location.
		/// </summary>
		/// <param name="location">Location to open.</param>
		/// <param name="label">Label of location.</param>
		/// <param name="viewType">Map view type.</param>
		public static void OpenMapLocation(Location location, string label = null, MapViewType viewType = MapViewType.Standard)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			var url = FormatMapsUrl("ll={0}&t={1}", location.ToLinkStr(), _mapViews[viewType]);
			if (label != null)
			{
				// The q parameter can also be used as a label
				// if the location is explicitly defined in the ll or address parameters.
				url = url + "&q=" + label;
			}
			IGUtils._openUrl(url);
		}

		/// <summary>
		///     Opens the map address.
		/// </summary>
		/// <param name="address">Address to open.</param>
		/// <param name="label">Label of location.</param>
		/// <param name="viewType">Map view type.</param>
		public static void OpenMapAddress(string address, string label = null, MapViewType viewType = MapViewType.Standard)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			address = address.EscapeMapQuery();
			Check.Argument.IsStrNotNullOrEmpty(address, "address");

			var url = FormatMapsUrl("address={0}&t={1}", address, _mapViews[viewType]);
			if (label != null)
			{
				// The q parameter can also be used as a label
				// if the location is explicitly defined in the ll or address parameters.
				url = url + "&q=" + label;
			}
			IGUtils._openUrl(url);
		}

		#region navigation

		/// <summary>
		///     Opens apple maps maps application with direction to destination
		/// </summary>
		/// <param name="destinationAddress">Destination address.</param>
		/// <param name="sourceAddress">Source address.</param>
		/// <param name="transportType">Transport type.</param>
		/// <param name="viewType">Map view type.</param>
		public static void GetDirections(string destinationAddress, string sourceAddress = null,
			TransportType transportType = TransportType.ByCar,
			MapViewType viewType = MapViewType.Standard)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			if (string.IsNullOrEmpty(destinationAddress))
			{
				throw new ArgumentException("Destination address must not be null or empty", "destinationAddress");
			}

			var url = FormatMapsUrl("daddr={0}&dirflg={1}&t={2}",
				WWW.EscapeURL(destinationAddress), _transportTypes[transportType], viewType);
			if (!string.IsNullOrEmpty(sourceAddress))
			{
				url = url + "&saddr=" + WWW.EscapeURL(sourceAddress);
			}
			Debug.Log("XXX: " + url);
			IGUtils._openUrl(url);
		}

		#endregion

		static string EscapeMapQuery(this string str)
		{
			return str.Replace(" ", "+");
		}

		static string FormatMapsUrl(string format, params object[] args)
		{
			return MapsUrl + string.Format(format, args);
		}

		public struct Location
		{
			public Location(float latitude, float longitude) : this()
			{
				if (latitude < -90.0f || latitude > 90.0f)
				{
					throw new ArgumentOutOfRangeException("latitude", "Latitude must be from -90.0 to 90.0.");
				}
				if (longitude < -180.0f || longitude > 180.0f)
				{
					throw new ArgumentOutOfRangeException("longitude", "Longitude must be from -180.0 to 180.0.");
				}

				Latitude = latitude;
				Longitude = longitude;
			}

			public float Latitude { get; private set; }

			public float Longitude { get; private set; }

			public string ToLinkStr()
			{
				return string.Format("{0},{1}", Latitude, Longitude);
			}
		}

		#region search

		/// <summary>
		///     Performs the search in apple maps application.
		/// </summary>
		/// <param name="query">
		///     The query. This parameter is treated as if its value had been typed into the Maps search field by
		///     the user.
		/// </param>
		/// <param name="viewType">Map view type.</param>
		public static void PerformSearch(string query, MapViewType viewType = MapViewType.Standard)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsStrNotNullOrEmpty(query, "query");

			var url = FormatMapsUrl("q={0}&t={1}", WWW.EscapeURL(query), _mapViews[viewType]);
			IGUtils._openUrl(url);
		}

		/// <summary>
		///     Performs the search with provided query near specified location.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="searchLocation">Search location.</param>
		/// <param name="zoom">Zoom level. Must be between 2 and 23</param>
		/// <param name="viewType">Map view type.</param>
		public static void PerformSearch(string query, Location searchLocation, int zoom = DefaultMapZoomLevel,
			MapViewType viewType = MapViewType.Standard)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsStrNotNullOrEmpty(query, "query");

			if (zoom < MinMapZoomLevel || zoom > MaxMapZoomLevel)
			{
				throw new ArgumentOutOfRangeException("zoom", "Zoom level must be between 2 and 23");
			}

			var url = FormatMapsUrl("q={0}&sll={1}&z={2}&t={3}",
				WWW.EscapeURL(query), searchLocation.ToLinkStr(), zoom, _mapViews[viewType]);
			IGUtils._openUrl(url);
		}

		#endregion
	}
}
#endif