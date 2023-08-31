using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Numerics;
using System.Timers;

namespace Baller
{
	internal partial class Window : Form
	{
		internal static byte SizeScale = 6;
		internal static readonly int[] WindowSize = new int[2] { 100, 100 }; // cm
		private static readonly int[] WindowSizeP = new int[2] { WindowSize[0] * SizeScale, WindowSize[1] * SizeScale }; // pixels

		private static SKColor BallColor = new(255, 255, 255);
		private static float BallRadius = 1.5F; // cm
		private static Material BallMaterial = Material.Aluminium;
		private static readonly List<Ball> Balls = new(500);

		internal const float IterationDelta = 0.001F; // s
		private const float WindowUpdateDelta = 0.016F; // s
		private static readonly System.Diagnostics.Stopwatch Stopwatch = new();
		private static readonly System.Timers.Timer WindowUpdater = new(WindowUpdateDelta * 1000);

		public Window()
		{
			InitializeComponent();
			WindowUpdater.Elapsed += new ElapsedEventHandler(CalculatePositions);
			WindowUpdater.Start();
		}

		private void RenderWindow(object sender, SKPaintGLSurfaceEventArgs e)
		{
			SKCanvas Canvas = e.Surface.Canvas;
			Canvas.Clear(SKBackgroundColor);
			foreach (Ball ball in Balls)
			{
				Canvas.DrawCircle(
					cx: ball.Position.X * SizeScale,
					cy: WindowSizeP[1] - (ball.Position.Y * SizeScale),
					radius: ball.Radius * SizeScale,
					paint: ball.Color
				);
			};
		}

		private void CalculatePositions(object? sender, ElapsedEventArgs e)
		{
			// Windows can only run timers set to >=16ms consistently, so that's used.
			// This means that every 16ms, we simulate the scene 16 times and then
			// render once (to avoid ball not moving and then suddenly moving fast).
			// This way the simulation runs at 1ms precision, and the window gets updated
			// at +- 62FPS (1 frame per 16ms)
			Stopwatch.Restart();
			for (int c = 0; c < WindowUpdateDelta * 1000; c++)
			{
				foreach (Ball ball in Balls)
					ball.CalculateNextPosition();
			};
			skglControl1.Invalidate();

			float FPS = (float)(1 / ((double)Stopwatch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency / (WindowUpdateDelta * 1000)));
			FpsLabel.Text = $"{FPS:0.00} FPS";
			CountLabel.Text = $"{Balls.Count:n0} Balls";
		}

		private void ResetWindow(object sender, EventArgs e)
		{
			Balls.Clear();
		}

		private void AddBall(object sender, EventArgs e)
		{
			Balls.Add(new Ball(
				new Vector2(WindowSize[0] / 2, WindowSize[1] - (2 * BallRadius) - 1),
				BallRadius, BallColor, BallMaterial,
				Vector2.UnitX
			));
		}

		private void ResizeWindow(object sender, EventArgs e)
		{
			SizeScale = (byte)WindowScale.Value;
			WindowSize[0] = (int)WindowX.Value;
			WindowSize[1] = (int)WindowY.Value;
			WindowSizeP[0] = WindowSize[0] * SizeScale;
			WindowSizeP[1] = WindowSize[1] * SizeScale;
			SetScreenSize();
		}

		private void SetRadius(object sender, EventArgs e)
		{
			BallRadius = (float)Radius.Value;
		}

		private void SetMaterial(object sender, EventArgs e)
		{
			BallMaterial = (Material)MaterialType.SelectedItem;
		}

		private void OpenColor(object sender, EventArgs e)
		{
			if (BallColorDialog.ShowDialog() == DialogResult.OK)
			{
				BallColor = BallColorDialog.Color.ToSKColor();
			};
		}
	}
}
