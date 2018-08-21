// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGGPS.cs
//


#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Class for accessing GPS location
	///     Permissions:
	///         <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
	///     Uses-feature:
	///         <!-- Needed only if your app targets Android 5.0 (API level 21) or higher. -->
	///         <uses-feature android:name="android.hardware.location.gps" android:required="false" />
	/// </summary>
	[PublicAPI]
	public static class AGGPS
	{
		const string GPS_PROVIDER = "gps";

		static LocationListenerProxy _currentListener;

		/// <summary>
		/// GPS Location.
		/// </summary>
		[PublicAPI]
		public sealed class Location
		{
			readonly double _latitude;
			readonly double _longitude;
			readonly bool _hasAccuracy;
			readonly float _accuracy;
			readonly long _timestamp;

			/// <summary>
			/// Gets the latitude.
			/// </summary>
			/// <value>The latitude.</value>
			[PublicAPI]
			public double Latitude
			{
				get { return _latitude; }
			}

			/// <summary>
			/// Gets the Longitude.
			/// </summary>
			/// <value>The Longitude.</value>
			[PublicAPI]
			public double Longitude
			{
				get { return _longitude; }
			}

			/// <summary>
			/// Gets a value indicating whether this instance has accuracy.
			/// </summary>
			/// <value><c>true</c> if this instance has accuracy; otherwise, <c>false</c>.</value>
			[PublicAPI]
			public bool HasAccuracy
			{
				get { return _hasAccuracy; }
			}

			/// <summary>
			/// Gets the location accuracy.
			/// </summary>
			/// <value>The location accuracy.</value>
			[PublicAPI]
			public float Accuracy
			{
				get { return _accuracy; }
			}

			/// <summary>
			/// True if this location has a speed.
			/// </summary>
			[PublicAPI]
			public bool HasSpeed { get; set; }

			/// <summary>
			/// Get the speed if it is available, in meters/second over ground.
			/// 
			// If this location does not have a speed then 0.0 is returned.
			/// </summary>
			[PublicAPI]
			public float Speed { get; set; }

			/// <summary>
			/// Whethere the location has bearing
			/// </summary>
			[PublicAPI]
			public bool HasBearing { get; set; }

			/// <summary>
			/// Get the bearing, in degrees.
			// Bearing is the horizontal direction of travel of this device, and is not related to the device orientation.It is guaranteed to be in the range (0.0, 360.0] if the device has a bearing.
			// If this location does not have a bearing then 0.0 is returned.
			/// </summary>
			[PublicAPI]
			public float Bearing { get; set; }

			/// <summary>
			/// <c>true</c> if this location has an altitude.
			/// </summary>
			[PublicAPI]
			public bool HasAltitude { get; set; }

			/// <summary>
			/// Get the altitude if available, in meters above the WGS 84 reference ellipsoid.
			/// If this location does not have an altitude then 0.0 is returned.
			/// </summary>
			[PublicAPI]
			public double Altitude { get; set; }

			/// <summary>
			/// WARNING! This API was added in API level 18. It will always return false if Android version is below 18;
			///
			/// Returns true if the Location came from a mock provider.
			/// </summary>
			[PublicAPI]
			public bool IsFromMockProvider { get; set; }

			/// <summary>
			/// Return the UTC time of this fix, in milliseconds since January 1, 1970.
			/// </summary>
			[PublicAPI]
			public long Timestamp
			{
				get { return _timestamp; }
			}

			public Location(double latitude, double longitude, bool hasAccuracy, float accuracy, long timestamp)
			{
				_latitude = latitude;
				_longitude = longitude;
				_hasAccuracy = hasAccuracy;
				_accuracy = accuracy;
				_timestamp = timestamp;
			}

			/// <summary>
			/// Returns the approximate distance in meters between this location and the given location.
			/// </summary>
			/// <param name="destination">Location to get distance to</param>
			/// <returns></returns>
			[PublicAPI]
			public float DistanceTo(Location destination)
			{
				var result = new float[1];
				DistanceBetween(_latitude, _longitude, destination.Latitude, destination.Longitude, result);
				return result[0];
			}

			public override string ToString()
			{
				return string.Format("[Location: Latitude={0}, Longitude={1}, " +
				                     "HasAccuracy={2}, Accuracy={3}, " +
				                     "Timestamp={4}, " +
				                     "HasSpeed={5}, Speed={6}, " +
				                     "HasBearing={7}, Bearing={8}, " +
				                     "HasAltitude={9}, Altitude={10}, " +
				                     "IsFromMockProvider={11}]",
					Latitude, Longitude,
					HasAccuracy, Accuracy,
					Timestamp,
					HasSpeed, Speed,
					HasBearing, Bearing,
					HasAltitude, Altitude,
					IsFromMockProvider);
			}
		}

		/// <summary>
		/// Computes the approximate distance in meters between two locations, and optionally the initial and final bearings of the shortest path between them.
		/// Distance and bearing are defined using the WGS84 ellipsoid.
		/// 
		/// The computed distance is stored in results[0]. 
		/// If results has length 2 or greater, the initial bearing is stored in results[1]. 
		/// If results has length 3 or greater, the final bearing is stored in results[2].
		/// </summary>
		/// <param name="startLatitude"></param>
		/// <param name="startLongitude"></param>
		/// <param name="endLatitude"></param>
		/// <param name="endLongitude"></param>
		/// <param name="results"></param>
		[PublicAPI]
		public static void DistanceBetween(
			double startLatitude,
			double startLongitude,
			double endLatitude,
			double endLongitude,
			float[] results)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (results == null || results.Length < 1)
			{
				throw new ArgumentException("results is null or has length < 1");
			}

			LocationUtils.ComputeDistanceAndBearing(startLatitude, startLongitude, endLatitude, endLongitude, results);
		}

		/// <summary>
		/// Checks if device has GPS module
		/// </summary>
		/// <returns>True if device has GPS module</returns>
		[PublicAPI]
		public static bool DeviceHasGPS()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			return AGDeviceInfo.SystemFeatures.HasSystemFeature(AGDeviceInfo.SystemFeatures.FEATURE_LOCATION_GPS);
		}

		/// <summary>
		/// Determines if is GPS enabled.
		/// </summary>
		/// <returns><c>true</c> if is GPS enabled; otherwise, <c>false</c>.</returns>
		[PublicAPI]
		public static bool IsGPSEnabled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			return AGSystemService.LocationService.Call<bool>("isProviderEnabled", GPS_PROVIDER);
		}

		/// <summary>
		/// Gets the last known GPS location.
		/// </summary>
		/// <returns>The last known GPS location.</returns>
		[PublicAPI]
		public static Location GetLastKnownGPSLocation()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}

			try
			{
				var locationAJO = AGSystemService.LocationService.Call<AndroidJavaObject>("getLastKnownLocation",
					GPS_PROVIDER);
				return LocationFromAJO(locationAJO);
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Requests the location updates.
		/// </summary>
		/// <param name="minTime">Minimum time interval between location updates, in milliseconds.</param>
		/// <param name="minDistance">Minimum distance between location updates, in meters.</param>
		/// <param name="onLocationChangedCallback">On location changed callback.</param>
		[PublicAPI]
		public static void RequestLocationUpdates(long minTime, float minDistance,
			Action<Location> onLocationChangedCallback)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (minTime <= 0)
			{
				throw new ArgumentOutOfRangeException("minTime", "Time cannot be less then zero");
			}

			if (minDistance <= 0)
			{
				throw new ArgumentOutOfRangeException("minDistance", "minDistance cannot be less then zero");
			}

			if (onLocationChangedCallback == null)
			{
				throw new ArgumentNullException("onLocationChangedCallback", "Location changed callback cannot be null");
			}

			_currentListener = new LocationListenerProxy(onLocationChangedCallback);

			try
			{
				AGUtils.RunOnUiThread(() =>
					AGSystemService.LocationService.Call("requestLocationUpdates", GPS_PROVIDER, minTime, minDistance,
						_currentListener));
			}
			catch (Exception e)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning(
						"Failed to register for location updates. Current device probably does not have GPS. Please check if device has GPS before invoking this method. " +
						e.Message);
				}
			}
		}

		/// <summary>
		/// Removes all location updates.
		/// </summary>
		[PublicAPI]
		public static void RemoveUpdates()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (_currentListener == null)
			{
				return;
			}

			AGUtils.RunOnUiThread(() => { AGSystemService.LocationService.Call("removeUpdates", _currentListener); });
		}

		[SuppressMessage("ReSharper", "UnusedMember.Local")]
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		class LocationListenerProxy : AndroidJavaProxy
		{
			readonly Action<Location> _onLocationChanged;

			public LocationListenerProxy(Action<Location> onLocationChanged)
				: base("android.location.LocationListener")
			{
				_onLocationChanged = onLocationChanged;
			}

			void onLocationChanged( /*Location*/ AndroidJavaObject locationAJO)
			{
				var location = LocationFromAJO(locationAJO);
				GoodiesSceneHelper.Queue(() => _onLocationChanged(location));
			}

			void onProviderDisabled(string provider)
			{
			}

			void onProviderEnabled(string provider)
			{
			}

			void onStatusChanged(string provider, int status, /*Bundle*/ AndroidJavaObject extras)
			{
			}

			static bool thatWasMe;

			// proxy for int java.lang.Object.hashCode()
			int hashCode()
			{
				thatWasMe = true;
				return GetHashCode();
			}

			// proxy for boolean java.lang.Object.equals(Object o)
			bool equals(AndroidJavaObject o)
			{
				thatWasMe = false;
				o.Call<int>("hashCode");
				return thatWasMe;
			}
		}

		static Location LocationFromAJO( /*Location*/ AndroidJavaObject locationAJO)
		{
			using (locationAJO)
			{
				var latitude = locationAJO.Call<double>("getLatitude");
				var longitude = locationAJO.Call<double>("getLongitude");

				var hasAltitude = locationAJO.Call<bool>("hasAltitude");
				var altitude = locationAJO.Call<double>("getAltitude");

				var hasAccuracy = locationAJO.Call<bool>("hasAccuracy");
				var accuracy = locationAJO.Call<float>("getAccuracy");

				long time = locationAJO.Call<long>("getTime");

				var hasSpeed = locationAJO.CallBool("hasSpeed");
				var speed = locationAJO.Call<float>("getSpeed");
				var hasBearing = locationAJO.CallBool("hasBearing");
				var bearing = locationAJO.Call<float>("getBearing");


				var result = new Location(latitude, longitude, hasAccuracy, accuracy, time);

				if (hasSpeed)
				{
					result.HasSpeed = true;
					result.Speed = speed;
				}

				if (hasBearing)
				{
					result.HasBearing = true;
					result.Bearing = bearing;
				}

				if (hasAltitude)
				{
					result.HasAltitude = true;
					result.Altitude = altitude;
				}
				
				bool isFromMockProvider = false;
				try
				{
					isFromMockProvider = locationAJO.CallBool("isFromMockProvider");
				}
				catch (Exception)
				{
					// Ignore
				}

				result.IsFromMockProvider = isFromMockProvider;

				return result;
			}
		}
	}
}

#endif