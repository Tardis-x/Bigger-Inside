#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using JetBrains.Annotations;

	/// <summary>
	/// The size of the resulting image. Pick smaller value to save memory.
	/// </summary>
	[PublicAPI]
	public enum ImageResultSize
	{
		[PublicAPI]
		Original = 0,

		[PublicAPI]
		Max256 = 256,

		[PublicAPI]
		Max512 = 512,

		[PublicAPI]
		Max1024 = 1024,

		[PublicAPI]
		Max2048 = 2048
	}
}
#endif