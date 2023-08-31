using SkiaSharp;
using System.Numerics;

namespace Baller
{
	internal class Ball
	{
		public Vector2 Position;
		public readonly float Radius; // cm
		public readonly SKPaint Color;

		private readonly float BounceFactor; // 0.0-1.0

		internal Vector2 Velocity; // m/s
		private readonly float Mass; // k/g
		private readonly double AirResistancePreCalc;

		private readonly Vector2 GravityBuoyancy; // N

		public Ball(
			Vector2 Position, float Radius,
			SKColor Colour, Material Material,
			Vector2? Velocity = null,
			float BounceFactor = 0.7F
		)
		{
			this.Position = Position;
			this.Radius = Radius;
			this.BounceFactor = BounceFactor;
			this.Color = new SKPaint()
			{
				Color = Colour,
				IsAntialias = true
			};

			// If desired, give ball starting velocity and direction
			this.Velocity = Velocity ?? Vector2.Zero;

			// The gravitational pull and buoyancy won't change so calculate them once, find their resulting force and store it
			float Volume = Physics.CalculateVolume(Radius);
			Mass = Physics.CalculateMass(Volume, Material);
			Vector2 Gravity = Physics.CalculateGravitationalForce(Mass);
			Vector2 Buoyancy = Physics.CalculateBuoyancyForce(Volume);
			GravityBuoyancy = Gravity + Buoyancy;

			// In the air resistance equation, only the velocity will change so pre-calculate the rest
			AirResistancePreCalc = 0.5 * Constants.AirDensity * Constants.DragCoefficientBall * (Math.PI * Math.Pow(Radius / 100, 2));
		}

		public void CalculateNextPosition()
		{
			// Skip everything if ball is laying still
			if (!(Position.Y == Radius && Velocity.Length() == 0))
			{
				Vector2 Offsets = CalculateOffset(Window.IterationDelta);

				// If new location will be in the wall, calculate bounce
				Vector2 TheoreticalPosition = Position + Offsets;
				if (TheoreticalPosition.Y <= Radius)
				{
					// Bottom wall
					float PreBounceTime = (Radius - Position.Y) / Offsets.Y;
					float PostBounceTime = 1 - PreBounceTime;

					// Move ball to position where it touches wall
					Position += Offsets * PreBounceTime;

					// Apply bounce effects
					Velocity *= BounceFactor;
					Velocity.Y *= -1;

					// Move ball to position after bounce
					Offsets = CalculateOffset(Window.IterationDelta * PostBounceTime);
					if (Offsets.Y >= 0) Position += Offsets;
					else
					{
						// Ball finished bouncing
						Position.X += Offsets.X;
						Position.Y = Radius;
						Velocity = Vector2.Zero;
					}
				}
				else if (Window.WindowSize[1] <= TheoreticalPosition.Y + Radius)
				{
					// Top wall
					float PreBounceTime = (Window.WindowSize[1] - Position.Y - Radius) / Offsets.Y;
					float PostBounceTime = 1 - PreBounceTime;

					// Move ball to position where it touches wall
					Position += Offsets * PreBounceTime;

					// Apply bounce effects
					Velocity *= BounceFactor;
					Velocity.Y *= -1;

					// Move ball to position after bounce
					Offsets = CalculateOffset(Window.IterationDelta * PostBounceTime);
					Position += Offsets;
				}
				else if (TheoreticalPosition.X <= Radius)
				{
					// Left wall
					float PreBounceTime = (Radius - Position.X) / Offsets.X;
					float PostBounceTime = 1 - PreBounceTime;

					// Move ball to position where it touches wall
					Position += Offsets * PreBounceTime;

					// Apply bounce effects
					Velocity *= BounceFactor;
					Velocity.X *= -1;

					// Move ball to position after bounce
					Offsets = CalculateOffset(Window.IterationDelta * PostBounceTime);
					Position += Offsets;
				}
				else if (Window.WindowSize[0] <= TheoreticalPosition.X + Radius)
				{
					// Right wall
					float PreBounceTime = (Window.WindowSize[0] - Position.X - Radius) / Offsets.X;
					float PostBounceTime = 1 - PreBounceTime;

					// Move ball to position where it touches wall
					Position += Offsets * PreBounceTime;

					// Apply bounce effects
					Velocity *= BounceFactor;
					Velocity.X *= -1;

					// Move ball to position after bounce
					Offsets = CalculateOffset(Window.IterationDelta * PostBounceTime);
					Position += Offsets;
				}
				else
				{
					Position = TheoreticalPosition;
				}
			}
		}

		public Vector2 CalculateOffset(float StepSize)
		{
			Vector2 ResultingForce;
			if (Velocity.Length() != 0)
			{
				Vector2 AirResistance = Physics.CalculateAirResistance(Velocity, AirResistancePreCalc);
				ResultingForce = GravityBuoyancy + AirResistance;
			}
			else if (Position.Y != Radius)
			{
				// The ball has no velocity so the only force, and thus also the resulting force, is gravity + buoyancy.
				ResultingForce = GravityBuoyancy;
			}
			else
			{
				ResultingForce = Vector2.Zero;
			}

			if (ResultingForce.Length() != 0)
			{
				// Calculate acceleration and velocity
				Vector2 ResultingVelocity = ResultingForce * (StepSize / Mass); // m/s
				Velocity += ResultingVelocity;
			}

			if (Velocity.Length() != 0)
			{
				return Velocity * (StepSize * 100); // cm
			}
			else
			{
				return Vector2.Zero;
			}
		}

		public override string ToString()
			=> $"({Position.X}; {Position.Y}) {Velocity.Length():0.00} m/s";
	}
}
