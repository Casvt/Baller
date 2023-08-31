using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace Baller
{
	internal partial class Window
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private readonly Color BackgroundColor = Color.FromArgb(31, 31, 31);
		private readonly SKColor SKBackgroundColor = SKColor.Parse("#1f1f1f");
		private readonly Color LightColor = Color.FromArgb(255, 255, 255);

		private const byte ControlPadding = 10;
		private const byte BoxPadding = 5;
		private const byte BoxTopPadding = 10;
		private const byte ButtonHeight = 25;
		private const byte LabelHeight = 25;

		private static readonly Control Console = new();

		private static readonly GroupBox LabelBox = new();
		private static readonly Label FpsLabel = new();
		private static readonly Label CountLabel = new();
		private const byte FpsLabelWidth = 87;
		private const byte CountLabelWidth = 87;

		private static readonly GroupBox CountControlBox = new();
		private static readonly Button Reset = new();
		private static readonly Button AddBallButton = new();
		private const byte ResetWidth = 60;
		private const byte AddBallButtonWidth = 60;

		private static readonly GroupBox WindowSettingsBox = new();
		private static readonly NumericUpDown WindowX = new();
		private static readonly NumericUpDown WindowY = new();
		private static readonly NumericUpDown WindowScale = new();
		private const byte WindowXWidth = 60;
		private const byte WindowYWidth = 60;
		private const byte WindowScaleWidth = 60;

		private static readonly GroupBox BallSettingsBox = new();
		private static readonly NumericUpDown Radius = new();
		private static readonly NumericUpDown Angle = new();
		private static readonly NumericUpDown Speed = new();
		private static readonly ComboBox MaterialType = new();
		private static readonly Button OpenBallColor = new();
		private static readonly ColorDialog BallColorDialog = new();
		private const byte RadiusWidth = 60;
		private const byte MaterialTypeWidth = (byte)(90 + 0.5 * BoxPadding);
		private const byte OpenBallColorWidth = 60;

		private static readonly SKGLControl skglControl1 = new();

		private void SetScreenSize()
		{
			skglControl1.Size = new Size(WindowSizeP[0], WindowSizeP[1]);

			if (skglControl1.Size.Width >= ClientSize.Width
			|| skglControl1.Size.Height >= ClientSize.Height)
			{
				// Grow window because canvas is bigger than window
				skglControl1.BorderStyle = BorderStyle.None;
				ClientSize = new Size(
					skglControl1.Size.Width,
					skglControl1.Size.Height + BallSettingsBox.Size.Height + 2 * ControlPadding
				);
			}
			else
			{
				// Show border because canvas is smaller than window
				skglControl1.BorderStyle = BorderStyle.FixedSingle;
			}

			// Center console
			Console.Location = new Point((ClientSize.Width - Console.Size.Width) / 2, ControlPadding);
		}

		private void InitializeComponent()
		{
			SuspendLayout();

			BackColor = BackgroundColor;

			Console.Location = new Point(ControlPadding, ControlPadding);
			Controls.Add(Console);

			LabelBox.Location = new Point(0, 0);
			LabelBox.Size = new Size(FpsLabelWidth + BoxPadding * 2, LabelHeight * 2 + BoxPadding * 3 + BoxTopPadding);
			LabelBox.Text = "Stats";
			LabelBox.ForeColor = LightColor;
			LabelBox.BackColor = BackgroundColor;
			LabelBox.FlatStyle = FlatStyle.Flat;
			Console.Controls.Add(LabelBox);

			FpsLabel.Location = new Point(BoxPadding, BoxPadding + BoxTopPadding);
			FpsLabel.Size = new Size(FpsLabelWidth, LabelHeight);
			FpsLabel.TextAlign = ContentAlignment.MiddleCenter;
			FpsLabel.ForeColor = LightColor;
			FpsLabel.BackColor = BackgroundColor;
			FpsLabel.TabIndex = 0;
			LabelBox.Controls.Add(FpsLabel);

			CountLabel.Location = new Point(FpsLabel.Location.X, FpsLabel.Location.Y + FpsLabel.Size.Height + BoxPadding);
			CountLabel.Size = new Size(CountLabelWidth, LabelHeight);
			CountLabel.TextAlign = ContentAlignment.MiddleCenter;
			CountLabel.ForeColor = LightColor;
			CountLabel.BackColor = BackgroundColor;
			CountLabel.TabIndex = 1;
			LabelBox.Controls.Add(CountLabel);

			CountControlBox.Location = new Point(LabelBox.Location.X + LabelBox.Size.Width + ControlPadding, 0);
			CountControlBox.Size = new Size(ResetWidth + BoxPadding * 2, ButtonHeight * 2 + BoxPadding * 3 + BoxTopPadding);
			CountControlBox.Text = "Balls";
			CountControlBox.ForeColor = LightColor;
			CountControlBox.BackColor = BackgroundColor;
			CountControlBox.FlatStyle = FlatStyle.Flat;
			Console.Controls.Add(CountControlBox);

			Reset.Location = new Point(BoxPadding, BoxPadding + BoxTopPadding);
			Reset.Size = new Size(ResetWidth, ButtonHeight);
			Reset.Text = "Reset";
			Reset.TextAlign = ContentAlignment.MiddleCenter;
			Reset.ForeColor = LightColor;
			Reset.BackColor = BackgroundColor;
			Reset.FlatStyle = FlatStyle.Flat;
			Reset.FlatAppearance.BorderColor = LightColor;
			Reset.FlatAppearance.BorderSize = 1;
			Reset.FlatAppearance.MouseDownBackColor = LightColor;
			Reset.TabIndex = 4;
			Reset.Click += new EventHandler(ResetWindow);
			CountControlBox.Controls.Add(Reset);

			AddBallButton.Location = new Point(Reset.Location.X, Reset.Location.Y + Reset.Size.Height + BoxPadding);
			AddBallButton.Size = new Size(AddBallButtonWidth, ButtonHeight);
			AddBallButton.Text = "Add";
			AddBallButton.TextAlign = ContentAlignment.MiddleCenter;
			AddBallButton.ForeColor = LightColor;
			AddBallButton.BackColor = BackgroundColor;
			AddBallButton.FlatStyle = FlatStyle.Flat;
			AddBallButton.FlatAppearance.BorderColor = LightColor;
			AddBallButton.FlatAppearance.BorderSize = 1;
			AddBallButton.FlatAppearance.MouseDownBackColor = LightColor;
			AddBallButton.TabIndex = 5;
			AddBallButton.Click += new EventHandler(AddBall);
			CountControlBox.Controls.Add(AddBallButton);

			WindowSettingsBox.Location = new Point(CountControlBox.Location.X + CountControlBox.Size.Width + ControlPadding, 0);
			WindowSettingsBox.Size = new Size(WindowXWidth + WindowYWidth + BoxPadding * 3, ButtonHeight * 2 + BoxPadding * 3 + BoxTopPadding);
			WindowSettingsBox.Text = "Window Settings";
			WindowSettingsBox.ForeColor = LightColor;
			WindowSettingsBox.BackColor = BackgroundColor;
			WindowSettingsBox.FlatStyle = FlatStyle.Flat;
			Console.Controls.Add(WindowSettingsBox);

			WindowX.Location = new Point(BoxPadding, BoxPadding + BoxTopPadding);
			WindowX.Size = new Size(WindowXWidth, ButtonHeight);
			WindowX.TextAlign = HorizontalAlignment.Center;
			WindowX.ForeColor = LightColor;
			WindowX.BackColor = BackgroundColor;
			WindowX.TabIndex = 6;
			WindowX.Value = WindowSize[0];
			WindowX.Minimum = 0;
			WindowX.Maximum = decimal.MaxValue;
			WindowX.Increment = 1;
			WindowX.DecimalPlaces = 0;
			WindowX.ValueChanged += new EventHandler(ResizeWindow);
			WindowSettingsBox.Controls.Add(WindowX);

			WindowY.Location = new Point(WindowX.Location.X + WindowX.Size.Width + BoxPadding, BoxPadding + BoxTopPadding);
			WindowY.Size = new Size(WindowYWidth, ButtonHeight);
			WindowY.TextAlign = HorizontalAlignment.Center;
			WindowY.ForeColor = LightColor;
			WindowY.BackColor = BackgroundColor;
			WindowY.TabIndex = 7;
			WindowY.Value = WindowSize[1];
			WindowY.Minimum = 0;
			WindowY.Maximum = decimal.MaxValue;
			WindowY.Increment = 1;
			WindowY.DecimalPlaces = 0;
			WindowY.ValueChanged += new EventHandler(ResizeWindow);
			WindowSettingsBox.Controls.Add(WindowY);

			WindowScale.Location = new Point(WindowX.Location.X, WindowX.Location.Y + WindowX.Size.Height + BoxPadding);
			WindowScale.Size = new Size(WindowScaleWidth, ButtonHeight);
			WindowScale.TextAlign = HorizontalAlignment.Center;
			WindowScale.ForeColor = LightColor;
			WindowScale.BackColor = BackgroundColor;
			WindowScale.TabIndex = 8;
			WindowScale.Value = SizeScale;
			WindowScale.Minimum = 1;
			WindowScale.Maximum = 255;
			WindowScale.Increment = 1;
			WindowScale.DecimalPlaces = 0;
			WindowScale.ValueChanged += new EventHandler(ResizeWindow);
			WindowSettingsBox.Controls.Add(WindowScale);

			BallSettingsBox.Location = new Point(WindowSettingsBox.Location.X + WindowSettingsBox.Size.Width + ControlPadding, 0);
			BallSettingsBox.Size = new Size(RadiusWidth + OpenBallColorWidth + BoxPadding * 3, ButtonHeight * 2 + BoxPadding * 3 + BoxTopPadding);
			BallSettingsBox.Text = "Ball Settings";
			BallSettingsBox.ForeColor = LightColor;
			BallSettingsBox.BackColor = BackgroundColor;
			BallSettingsBox.FlatStyle = FlatStyle.Flat;
			Console.Controls.Add(BallSettingsBox);

			Radius.Location = new Point(BoxPadding, BoxPadding + BoxTopPadding);
			Radius.Size = new Size(RadiusWidth, ButtonHeight);
			Radius.TextAlign = HorizontalAlignment.Center;
			Radius.ForeColor = LightColor;
			Radius.BackColor = BackgroundColor;
			Radius.TabIndex = 9;
			Radius.Value = (decimal)BallRadius;
			Radius.Minimum = 0.01M;
			Radius.Maximum = decimal.MaxValue;
			Radius.Increment = 0.1M;
			Radius.DecimalPlaces = 2;
			Radius.ValueChanged += new EventHandler(SetRadius);
			BallSettingsBox.Controls.Add(Radius);

			MaterialType.Location = new Point(BoxPadding, Radius.Location.Y + Radius.Size.Height + BoxPadding);
			MaterialType.Size = new Size(MaterialTypeWidth, ButtonHeight);
			MaterialType.ForeColor = LightColor;
			MaterialType.BackColor = BackgroundColor;
			MaterialType.FlatStyle = FlatStyle.Flat;
			MaterialType.TabIndex = 10;
			Object[] Options = Enum.GetValues(typeof(Material)).Cast<Object>().ToArray();
			MaterialType.Items.AddRange(Options);
			MaterialType.SelectedIndex = (int)BallMaterial;
			MaterialType.SelectedIndexChanged += new EventHandler(SetMaterial);
			BallSettingsBox.Controls.Add(MaterialType);

			OpenBallColor.Location = new Point(Radius.Location.X + Radius.Size.Width + BoxPadding, BoxPadding + BoxTopPadding);
			OpenBallColor.Size = new Size(OpenBallColorWidth, ButtonHeight);
			OpenBallColor.Text = "Color";
			OpenBallColor.TextAlign = ContentAlignment.MiddleCenter;
			OpenBallColor.ForeColor = LightColor;
			OpenBallColor.BackColor = BackgroundColor;
			OpenBallColor.FlatStyle = FlatStyle.Flat;
			OpenBallColor.FlatAppearance.BorderColor = LightColor;
			OpenBallColor.FlatAppearance.BorderSize = 1;
			OpenBallColor.FlatAppearance.MouseDownBackColor = LightColor;
			OpenBallColor.TabIndex = 11;
			OpenBallColor.Click += new EventHandler(OpenColor);
			BallSettingsBox.Controls.Add(OpenBallColor);

			Console.Size = new Size(BallSettingsBox.Location.X + BallSettingsBox.Size.Width, BallSettingsBox.Size.Height);

			SetScreenSize();

			skglControl1.Location = new Point(0, Console.Location.Y + Console.Size.Height + 2 * BoxPadding);
			skglControl1.Size = new Size(WindowSizeP[0], WindowSizeP[1]);
			skglControl1.BackColor = BackgroundColor;
			skglControl1.TabIndex = 12;
			skglControl1.VSync = false;
			skglControl1.PaintSurface += new EventHandler<SKPaintGLSurfaceEventArgs>(RenderWindow);
			Controls.Add(skglControl1);

			StartPosition = FormStartPosition.CenterScreen;
			FormBorderStyle = FormBorderStyle.Sizable;
			Resize += new EventHandler(ResizeWindow);
			Text = "Baller Simulation Window";
			ResumeLayout(false);
		}
	}
}
