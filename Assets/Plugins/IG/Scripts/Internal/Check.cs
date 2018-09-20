namespace DeadMosquito.IosGoodies.Internal
{
	using System;

	static class Check
	{
		public delegate bool Condition();

		public static void IsTrue(Condition condition, string message = "")
		{
			if (!condition())
			{
				throw new ArgumentException(message);
			}
		}

		public static class Argument
		{
			public static void IsNotNull(object argument, string argumentName, string message = "")
			{
				if (argument == null)
				{
					throw new ArgumentNullException(argumentName, message);
				}
			}

			public static void IsStrNotNullOrEmpty(string argument, string argumentName, string message = null)
			{
				if (string.IsNullOrEmpty(argument))
				{
					if (message == null)
					{
						message = string.Format("[{0}] must not be null or empty", argumentName);
					}
					throw new ArgumentException(message, argumentName);
				}
			}

			public static void IsNotNegative(int argument, string argumentName)
			{
				if (argument < 0)
				{
					throw new ArgumentOutOfRangeException(argumentName, argumentName + " must not be negative.");
				}
			}
		}
	}
}