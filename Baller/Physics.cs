using System.Numerics;

namespace Baller
{
	internal static class Physics
	{
		private const double VolumePreCalc = 4.0 / 3 * Math.PI;

		public static float CalculateVolume(float Radius)
			=> (float)(VolumePreCalc * Math.Pow(Radius * 0.01, 3));

		public static float CalculateMass(float Volume, Material Material)
			=> Volume * Constants.MaterialDensity[Material];

		public static Vector2 CalculateGravitationalForce(float Mass)
			=> new(0, Mass * Constants.GravitationalConstant);

		public static Vector2 CalculateBuoyancyForce(float Volume)
			=> new(
				0,
				Volume * Constants.AirDensity * Constants.GravitationalConstant
			);

		public static Vector2 CalculateAirResistance(
			Vector2 Velocity,
			double AirResistancePreCalc
		)
		{
			double Resistance = AirResistancePreCalc * Velocity.LengthSquared();
			Vector2 Result = (-Velocity) * (float)(Resistance / Velocity.Length());
			return Result;
		}
	}
}