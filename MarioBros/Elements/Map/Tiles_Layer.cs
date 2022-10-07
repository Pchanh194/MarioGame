using Game.Elements;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MarioBros.Elements.Map
{
    public class Tiles_Layer : Game.Elements.Sprite
    {
        private Size _frameSize;

        // Properties
        public Size SizeOfTiles { get; private set; }   // Size of each map tile in pixels

        public Size Size { get; private set; }          // Map size in number of cells

        private Size KeyFrame { get; set; }             // Screen size in cells (map cells visible)

        private Dictionary<int, Rectangle> Tiles { get; set; }  // Dictionary with the tiles associated with their respective ID
        private int[,] Matrix { get; set; }             // Matrix with the graphic information of the map

        // Constructor
        public Tiles_Layer(Elements.Resources resources, Size frameSize)
        {
            this.Size = new Size(resources.MapData.width, resources.MapData.height);
            this.SizeOfTiles = new Size(resources.MapData.tilewidth, resources.MapData.tileheight);

            base.Position = Point.Empty;
            base.Image = resources.TextureAtlas;
            _frameSize = frameSize;

            // Load the map tiles

            Tiles = new Dictionary<int, Rectangle>();
            Size SizeOfTileSet = new Size(resources.TextureAtlas.Width / SizeOfTiles.Width, resources.TextureAtlas.Height / SizeOfTiles.Height);
           
            for (int y = 0; y < SizeOfTileSet.Height; y++)
                for (int x = 0; x < SizeOfTileSet.Width; x++)
                {
                    int _id = ((SizeOfTileSet.Width * y) + x) + 1;
                    Rectangle _rec = new Rectangle(x * SizeOfTiles.Width, y * SizeOfTiles.Height, SizeOfTiles.Width, SizeOfTiles.Height);
                    // Console.WriteLine(_id);
                    Tiles.Add(_id, _rec);
                }
           

            // Tile array loading

            var tilesLayer = resources.MapData.layers.FirstOrDefault(x => x.name == "Tiles");
            Matrix = new int[tilesLayer.width, tilesLayer.height];

            for (int row = 0; row < tilesLayer.height; row++)
            {
                var columns = tilesLayer.data.Skip(row * tilesLayer.width).Take(tilesLayer.width).ToList();
                for (int col = 0; col < tilesLayer.width; col++)
                {
                    Matrix[col, row] = Convert.ToInt32(columns[col]);
                    // Console.WriteLine(Matrix[col, row]);
                }
            }

            // Size in number of cells that are visible on the screen

            Size _KeyFrameSize = new Size((int)Math.Ceiling((float)frameSize.Width / (float)SizeOfTiles.Width), 
                                        (int)Math.Ceiling((float)frameSize.Height / (float)SizeOfTiles.Height));
            this.KeyFrame = new Size(Math.Min(_KeyFrameSize.Width + 1, Size.Width), 
                                    Math.Min(_KeyFrameSize.Height, Size.Height));
        }

        // Methods
        public override void Draw(DrawProcessor drawProcessor)
        {
            int _X = (int)Math.Floor((float)Position.X / (float)SizeOfTiles.Width); // x-coordinate of the first cell to draw
            int _Y = (int)Math.Floor((float)Position.Y / (float)SizeOfTiles.Height); // y-coordinate of the first cell to draw

            for (int x = _X; x < _X + KeyFrame.Width; x++)
                for (int y = _Y; y < _Y + KeyFrame.Height; y++)
                {
                    if (x >= 0 && x < this.Size.Width)
                    {
                        int _id = Matrix[x, y];
                        //Console.WriteLine(_id);
                        if (_id != 0)
                        {
                            var _rec = Tiles[_id];
                            
                            Point _position = new Point((int)(x * SizeOfTiles.Width - Position.X), 
                                                        (int)(y * SizeOfTiles.Height - Position.Y));
                            
                            drawProcessor.Draw(base.Image, _rec, _position);
                        }
                    }
                }
        }

    }
}
