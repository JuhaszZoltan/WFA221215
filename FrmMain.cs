using WFA221215.Properties;

namespace WFA221215
{
    public partial class FrmMain : Form
    {
        int shotCount = 0;
        static Random rnd = new();
        List<PictureBox> snowflakes = new();
        bool isTmrRunning = false;

        public FrmMain()
        {
            InitializeComponent();
            tmr.Tick += OnTick;
            btnStart.Click += OnStartButtonClick;
        }

        private void OnStartButtonClick(object? sender, EventArgs e)
        {
            if (isTmrRunning)
            {
                isTmrRunning = false;
                tmr.Stop();
            }
            else
            {
                isTmrRunning = true;
                tmr.Start();
            }
        }

        private void OnTick(object? sender, EventArgs e)
        {
            if (rnd.Next(100) > 70 && snowflakes.Count < 100)
            {
                int snfHnW = rnd.Next(20, 100);

                snowflakes.Add(
                    new PictureBox()
                    {
                        Image = Resources.snowflake,
                        Bounds = new(
                            x: rnd.Next(this.Width), 
                            y: rnd.Next(this.Height),
                            height: snfHnW,
                            width: snfHnW),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Transparent,
                    });
                snowflakes.Last().Click += OnSnowflakeClick;

                this.Controls.Add(snowflakes.Last());
            }
            else
            {
                List<PictureBox> toKill = new();

                foreach (var snowflake in snowflakes)
                {
                    if (snowflake.Location.Y > this.Height) toKill.Add(snowflake);
                    else if (rnd.Next(100) < 50)
                    {
                        int newY = snowflake.Location.Y + (rnd.Next(20) + 1);
                        snowflake.Location = new(
                            x: snowflake.Location.X,
                            y: newY);
                    }
                }

                foreach (var oow in toKill)
                {
                    snowflakes.Remove(oow);
                    oow.Dispose();
                }
                toKill.Clear();
            }
        }

        private void OnSnowflakeClick(object? sender, EventArgs e)
        {
            snowflakes.Remove(sender as PictureBox);

            (sender as PictureBox).Image = Resources.blood;
            shotCount++;
            lblShotCount.Text = $"killed snowflake: {shotCount}";

            if (shotCount == 100)
            {
                tmr.Stop();
                foreach (var snf in snowflakes) snf.Dispose();
                this.BackgroundImageLayout = ImageLayout.Zoom;
                this.BackgroundImage = Resources.mxm;
                btnStart.Visible = false;
                lblShotCount.Visible = false;
            }
        }
    }
}