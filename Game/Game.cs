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
        #region Objects
        private GameTimer _GameTimer;
        
        // Timer that refreshes the game image
        private Timer _timer;
        
        // Event that is triggered when the mouse button is released on the canvas
        public event EventHandler<MouseEventArgs> Canvas_MouseUp;
        #endregion

        #region Constructor
        public Game()
        {
            InitializeComponent();
            _GameTimer = new GameTimer();
            KeyCode = new KeyCode();

            _timer = new Timer();
            _timer.Interval = 1000 / 30; // 60 FPS (the interval is not always respected in winforms)
            _timer.Tick += (sender, e) =>
            {
                var _now = DateTime.Now;
                _GameTimer.FrameMilliseconds = (int)(_now - _GameTimer.TimeFrame).TotalMilliseconds;
                _GameTimer.TimeFrame = _now;

                Application.DoEvents();
                this.Update(_GameTimer);  // run game logic
                this.KeyCode.Clear();

                using (DrawProcessor drawProcessor = new DrawProcessor(this.Canvas.Width, this.Canvas.Height))
                {
                    this.Draw(drawProcessor);    // Update the image every frame
                    Canvas.Image = drawProcessor.ImageBase; // assign the image of the new box to the picturebox
                }
            };
        }
        #endregion

        #region Properties
        /// Information of what keys are pressed
        protected KeyCode KeyCode { get; set; } 
        #endregion

        #region Events
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
            this.KeyCode.SetKey(e.KeyData);
        }
        #endregion

        #region Methods
        // Upload an image
        // <param name="path">path of the image to load</param>
        // <returns></returns>
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
        // <param name="path">path of the file to upload</param>
        // <returns></returns>
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

        // Method where the game's logic is written
        // <param name="GameTimer">Elapsed game time information</param>
        protected virtual void Update(GameTimer GameTimer)
        {

        }

        // Draw all sprites on screen
        // <param name="drawProcessor">draw controller</param>
        public virtual void Draw(DrawProcessor drawProcessor)
        {

        }

        #endregion
    }
}
