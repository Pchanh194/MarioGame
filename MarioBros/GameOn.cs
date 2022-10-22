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
    public partial class GameOn : Game.Game
    {
        // LevelProcessor level = new LevelProcessor(0);
        public GameOn(int level)
        {
            InitializeComponent();
            Initialize(level);
        }
        // Graphic resources of the game
        public Elements.Resources Resources { get; set; }

        public Elements.Map.MapProcessor MapProcessor { get; set; }

        // Load the graphic resources of the game
        private void Initialize(int level)
        {
            string directory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
            this.Resources = new Elements.Resources();
            this.Resources.TextureAtlas = Load_Image($"{directory}/TileSet.png");
            if (Program.NextLevel)
            {
                Program.Level++;
                Program.NextLevel = false;
            }
            Check_File($"{directory}/level{Program.Level}.json");

            this.Resources.MapData = Newtonsoft.Json.JsonConvert.DeserializeObject<Data.Map>(Load_Text($"{directory}/level{Program.Level}.json"));
            Canvas.BackColor = System.Drawing.ColorTranslator.FromHtml(this.Resources.MapData.backgroundcolor);
            InitializeMap();
        }

        // Load the map
        public void InitializeMap()
        {
            MapProcessor = new Elements.Map.MapProcessor(this.Resources, this.Canvas.Size);
            MapProcessor.Restart += (obj, e) => InitializeMap(); // reset the map
        }

        private void GameOn_KeyDown(object sender, KeyEventArgs e)
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
        private void GameOn_KeyUp(object sender, KeyEventArgs e)
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

        protected override void Update(GameTimer GameTimer)
        {
            this.MapProcessor.Update(GameTimer);
        }

        // Draw the grid
        public override void Draw(DrawProcessor drawProcessor)
        {
            this.MapProcessor.Draw(drawProcessor);
        }

        private void GameOn_Load(object sender, EventArgs e)
        {

        }
    }
}
