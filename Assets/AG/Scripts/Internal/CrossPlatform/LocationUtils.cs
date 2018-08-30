
#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;

	public static class LocationUtils
	{
		public static void ComputeDistanceAndBearing(double lat1, double lon1,
			double lat2, double lon2, float[] results)
		{
			// Based on http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf
			// using the "Inverse Formula" (section 4)

			int MAXITERS = 20;
			// Convert lat/long to radians
			lat1 *= Math.PI / 180.0;
			lat2 *= Math.PI / 180.0;
			lon1 *= Math.PI / 180.0;
			lon2 *= Math.PI / 180.0;

			double a = 6378137.0; // WGS84 major axis
			double b = 6356752.3142; // WGS84 semi-major axis
			double f = (a - b) / a;
			double aSqMinusBSqOverBSq = (a * a - b * b) / (b * b);

			double L = lon2 - lon1;
			double A = 0.0;
			double U1 = Math.Atan((1.0 - f) * Math.Tan(lat1));
			double U2 = Math.Atan((1.0 - f) * Math.Tan(lat2));

			double cosU1 = Math.Cos(U1);
			double cosU2 = Math.Cos(U2);
			double sinU1 = Math.Sin(U1);
			double sinU2 = Math.Sin(U2);
			double cosU1cosU2 = cosU1 * cosU2;
			double sinU1sinU2 = sinU1 * sinU2;

			double sigma = 0.0;
			double deltaSigma = 0.0;
			double cosSqAlpha = 0.0;
			double cos2SM = 0.0;
			double cosSigma = 0.0;
			double sinSigma = 0.0;
			double cosLambda = 0.0;
			double sinLambda = 0.0;

			double lambda = L; // initial guess
			for (int iter = 0; iter < MAXITERS; iter++)
			{
				double lambdaOrig = lambda;
				cosLambda = Math.Cos(lambda);
				sinLambda = Math.Sin(lambda);
				double t1 = cosU2 * sinLambda;
				double t2 = cosU1 * sinU2 - sinU1 * cosU2 * cosLambda;
				double sinSqSigma = t1 * t1 + t2 * t2; // (14)
				sinSigma = Math.Sqrt(sinSqSigma);
				cosSigma = sinU1sinU2 + cosU1cosU2 * cosLambda; // (15)
				sigma = Math.Atan2(sinSigma, cosSigma); // (16)
				double sinAlpha = (sinSigma == 0) ? 0.0 : cosU1cosU2 * sinLambda / sinSigma; // (17)
				cosSqAlpha = 1.0 - sinAlpha * sinAlpha;
				cos2SM = (cosSqAlpha == 0) ? 0.0 : cosSigma - 2.0 * sinU1sinU2 / cosSqAlpha; // (18)

				double uSquared = cosSqAlpha * aSqMinusBSqOverBSq; // defn
				A = 1 + (uSquared / 16384.0) * // (3)
				    (4096.0 + uSquared *
				     (-768 + uSquared * (320.0 - 175.0 * uSquared)));
				double B = (uSquared / 1024.0) * // (4)
				           (256.0 + uSquared *
				            (-128.0 + uSquared * (74.0 - 47.0 * uSquared)));
				double C = (f / 16.0) *
				           cosSqAlpha *
				           (4.0 + f * (4.0 - 3.0 * cosSqAlpha)); // (10)
				double cos2SMSq = cos2SM * cos2SM;
				deltaSigma = B * sinSigma * // (6)
				             (cos2SM + (B / 4.0) *
				              (cosSigma * (-1.0 + 2.0 * cos2SMSq) -
				               (B / 6.0) * cos2SM *
				               (-3.0 + 4.0 * sinSigma * sinSigma) *
				               (-3.0 + 4.0 * cos2SMSq)));

				lambda = L +
				         (1.0 - C) * f * sinAlpha *
				         (sigma + C * sinSigma *
				          (cos2SM + C * cosSigma *
				           (-1.0 + 2.0 * cos2SM * cos2SM))); // (11)

				double delta = (lambda - lambdaOrig) / lambda;
				if (Math.Abs(delta) < 1.0e-12)
				{
					break;
				}
			}

			float distance = (float) (b * A * (sigma - deltaSigma));
			results[0] = distance;
			if (results.Length > 1)
			{
				float initialBearing = (float) Math.Atan2(cosU2 * sinLambda,
					cosU1 * sinU2 - sinU1 * cosU2 * cosLambda);
				initialBearing *= 180.0f / (float) Math.PI;
				results[1] = initialBearing;
				if (results.Length > 2)
				{
					float finalBearing = (float) Math.Atan2(cosU1 * sinLambda,
						-sinU1 * cosU2 + cosU1 * sinU2 * cosLambda);
					finalBearing *= 180.0f / (float) Math.PI;
					results[2] = finalBearing;
				}
			}
		}
	}
}
#endif