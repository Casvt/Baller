namespace Baller
{
	internal enum Material
	{
		Aluminium,
		Gold,
		Iron,
		Silver,
		Glass,
		Wood,
		Cork,
		Rubber
	}

	internal static class Constants
	{
		// kg m^-3
		public static readonly Dictionary<Material, float> MaterialDensity = new()
		{
			{ Material.Aluminium, 2.70e3F },
			{ Material.Gold, 19.3e3F },
			{ Material.Iron, 7.87e3F },
			{ Material.Silver, 10.5e3F },
			{ Material.Glass, 2.5e3F},
			{ Material.Wood, 0.78e3F },
			{ Material.Cork, 0.25e3F },
			{ Material.Rubber, 1.4e3F }
		};

		public const float GravitationalConstant = -9.81F;

		public const float AirDensity = 1.293F;

		public const float DragCoefficientBall = 0.47F;
	}
}
