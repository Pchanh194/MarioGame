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

namespace MarioBros
{
    public partial class Demo : Game.Game
    {
        #region Constructor
        public Demo()
        {
            InitializeComponent();
            Initialize();
        }
        #endregion

        #region Properties
        // Graphic resources of the game
        public Elements.Resources Resources { get; set; }
        
        public Elements.Map.MapHandler MapHandler { get; set; }
        #endregion

        #region Methods

        // Load the graphic resources of the game
        private void Initialize()
        {
            string directory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
            this.Resources = new Elements.Resources()
            {
                TextureAtlas = Load_Image($"{directory}/TileSet.png"),
                MapData = Newtonsoft.Json.JsonConvert.DeserializeObject<Data.Map>(Load_Text($"{directory}/level2.json"))
            };
            Canvas.BackColor = System.Drawing.ColorTranslator.FromHtml(this.Resources.MapData.backgroundcolor);
            InitializeMap();
        }

        // Load the map
        private void InitializeMap()
        {
            MapHandler = new Elements.Map.MapHandler(this.Resources, this.Canvas.Size);
            MapHandler.Restart += (obj, e) => InitializeMap(); // reset the map
        }
        #endregion

        #region Events

        private void Demo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                Elements.KeyCode.Left = true;

            if (e.KeyCode == Keys.Right)
                Elements.KeyCode.Right = true;

            if (e.KeyCode == Keys.Z)
                Elements.KeyCode.Turbo = true;

            if (e.KeyCode == Keys.Up)
                Elements.KeyCode.Jump = true;
        }
        private void Demo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                Elements.KeyCode.Left = false;

            if (e.KeyCode == Keys.Right)
                Elements.KeyCode.Right = false;

            if (e.KeyCode == Keys.Z)
                Elements.KeyCode.Turbo = false;

            if (e.KeyCode == Keys.Up)
                Elements.KeyCode.Jump = false;
        }
        #endregion

        #region Update
        protected override void Update(GameTimer GameTimer)
        {
            this.MapHandler.Update(GameTimer);
        }
        #endregion

        #region Draw

        // Draw the grid
        public override void Draw(DrawProcessor drawProcessor)
        {
            this.MapHandler.Draw(drawProcessor);
        }
        #endregion

        private void Demo_Load(object sender, EventArgs e)
        {

        }
    }
}
