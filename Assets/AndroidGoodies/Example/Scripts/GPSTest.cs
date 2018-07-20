namespace AndroidGoodiesExamples
{
	using System;
	using DeadMosquito.AndroidGoodies;
	using JetBrains.Annotations;
	using UnityEngine;
	using UnityEngine.UI;

	public class GPSTest : MonoBehaviour
	{
		public const double AmsterdamLatitude = 52.3745913;
		public const double AmsterdamLongitude = 4.8285751;
		public const double BrusselsLatitude = 50.854954;
		public const double BrusselsLongitude = 4.3053508;

		public Text gpsText;
		public Text gpsInfoText;

#if UNITY_ANDROID
		void Start()
		{
			try
			{
				gpsText.text = AGGPS.GetLastKnownGPSLocation().ToString();
			}
			catch (Exception)
			{
				Debug.Log("Could not get last known location.");
			}

			TestDistanceBetween();
		}

		void TestDistanceBetween()
		{
			var results = new float[3];
			AGGPS.DistanceBetween(AmsterdamLatitude, AmsterdamLongitude,
				BrusselsLatitude, BrusselsLongitude, results);
			gpsInfoText.text = string.Format("DistanceBetween results: {0}, Initial bearing: {1}, Final bearing: {2}",
				results[0], results[1], results[2]);
		}

		[UsedImplicitly]
		public void OnStartTrackingLocation()
		{
			// Minimum time interval between location updates, in milliseconds.
			const long minTimeInMillis = 200;
			// Minimum distance between location updates, in meters.
			const float minDistanceInMetres = 1;
			AGGPS.RequestLocationUpdates(minTimeInMillis, minDistanceInMetres, OnLocationChanged);
		}

		[UsedImplicitly]
		public void OnStopTrackingLocation()
		{
			AGGPS.RemoveUpdates();
		}

		void OnLocationChanged(AGGPS.Location location)
		{
			gpsText.text = location.ToString();
		}
#endif
	}
}

