using Game.Elements;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Game : Base
    {
        private GameTimer _GameTimer;
        KeyCode keyCode = KeyCode.getKeyCode(); // Singleton

        // Timer that refreshes the game image
        private Timer _timer;
        
        // Event that is triggered when the mouse button is released on the canvas
        public event EventHandler<MouseEventArgs> Canvas_MouseUp;

        public Game()
        {
            InitializeComponent();
            _GameTimer = new GameTimer();

            _timer = new Timer();
            _timer.Interval = 1000 / 30; // 60 FPS (the interval is not always respected in winforms)
            _timer.Tick += (sender, e) =>
            {
                var _now = DateTime.Now;
                _GameTimer.FrameMilliseconds = (int)(_now - _GameTimer.TimeFrame).TotalMilliseconds;
                _GameTimer.TimeFrame = _now;

                Application.DoEvents();
                this.Update(_GameTimer);  // run game logic
                keyCode.Clear();

                using (DrawProcessor drawProcessor = new DrawProcessor(this.Canvas.Width, this.Canvas.Height))
                {
                    this.Draw(drawProcessor);    // Update the image every frame
                    Canvas.Image = drawProcessor.ImageBase; // assign the image of the new box to the picturebox
                }
            };
        }
        // Information of what keys are pressed
        // protected KeyCode KeyCode { get; set; } 

        private void Game_Load(object sender, EventArgs e)
        {
            
            if (!DesignMode)
                _timer.Start(); // start the game
        }
        private void pcCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (Canvas_MouseUp != null)
                Canvas_MouseUp(sender, e);
        }
        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            keyCode.SetKey(e.KeyData);
        }

        // Upload an image
        protected Image Load_Image(string path)
        {
            try
            {
                return Image.FromFile(path);
            }
            catch
            {
                MessageBox.Show("Load File Error\n" + path);
                return null;
            }
        }
       
        // Upload a text
        protected string Load_Text(string path)
        {
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
                {
                    return sr.ReadToEnd().Trim();
                }
            }
            catch
            {
                MessageBox.Show("Load File Error\n" + path);
                return null;
            }
        }

        protected void Check_File(string path)
        {
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
                {
                    return;
                }
            }
            catch
            {
                Application.Exit();
            }
        }

        // Method where the game's logic is written
        protected virtual void Update(GameTimer GameTimer)
        {

        }

        // Draw all sprites on screen
        public virtual void Draw(DrawProcessor drawProcessor)
        {

        }

    }
}
