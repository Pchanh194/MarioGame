using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioBros.Elements.Map
{
    public class Obstacles_Layer
    {
        // Properties

        // Map size in number of cells
        public Size Size { get; private set; }

        // Size of each map tile in pixels
        private Size SizeOfTiles { get; set; }

        // Matrix with information on obstacles
        private bool[,] Matrix { get; set; }

        // Constructor
        public Obstacles_Layer(Elements.Resources resources)
        {
            this.Size = new Size(resources.MapData.width, resources.MapData.height);
            this.SizeOfTiles = new Size(resources.MapData.tilewidth, resources.MapData.tileheight);

            var tilesLayer = resources.MapData.layers.FirstOrDefault(x => x.name == "Obstacles");
            Matrix = new bool[tilesLayer.width, tilesLayer.height];
            for (int row = 0; row < tilesLayer.height; row++)
            {
                var columns = tilesLayer.data.Skip(row * tilesLayer.width).Take(tilesLayer.width).ToList();
                for (int col = 0; col < tilesLayer.width; col++)
                    Matrix[col, row] = Convert.ToInt32(columns[col]) != 0;
            }
        }
        

        // Methods
        public void ValidateCollision(Elements.Objects.Base obj, PointF previousPosition)
        {
            if (obj == null)
                return;

            // element rectangle inside the map
            var objRectangle = obj.PositionOnMapRec; //new RectangleF(obj.PositionOnMap.X, obj.PositionOnMap.Y, obj.AnimationRect.Width, obj.AnimationRect.Height);

            int _tileX_Max = (int)Math.Ceiling((objRectangle.X + obj.AnimationRect.Width) / SizeOfTiles.Width) - 1;
            int _tileX_Min = (int)Math.Floor(objRectangle.X / SizeOfTiles.Width);
            int _tileY_Max = (int)Math.Ceiling((objRectangle.Y + obj.AnimationRect.Height) / SizeOfTiles.Height) - 1;
            int _tileY_Min = (int)Math.Floor(objRectangle.Y / SizeOfTiles.Height);

            // Console.WriteLine(_tileX_Min.ToString() + " " + _tileX_Max.ToString() + " " + _tileY_Min.ToString() + " " + _tileY_Max.ToString());

            for (int row = _tileY_Min; row <= _tileY_Max; row++)
                for (int col = _tileX_Min; col <= _tileX_Max; col++)
                    if (Has_TileCollision(row, col)) // Xem xem con mario có còn trong canvas hem
                    {
                        RectangleF recTile = new RectangleF((float)col * (float)SizeOfTiles.Width, (float)row * (float)SizeOfTiles.Height, SizeOfTiles.Width, SizeOfTiles.Height);
                        RectangleF area = RectangleF.Intersect(objRectangle, recTile);
                        if (area.Width == 0 && area.Height == 0)
                            continue;

                        PointF difPosition = new PointF(obj.PositionOnMap.X - previousPosition.X, obj.PositionOnMap.Y - previousPosition.Y); // difference between current and previous position
                        var adjPosition = Get_PositionAdjust(area, difPosition); // nếu mấy con quái vật nó chạm vào cái ống hay cái mẹ j đó thì dời nó lùi lại để ko bị overlapped, hàm này trả về cái lượng mà nó cần lùi

                        if (difPosition.Y != 0) // if there is a vertical collision
                        {
                            obj.Fix_MapPosition(0, adjPosition.Y); // đổi lại chỗ đứng theo lượng get_positionAdjust
                            obj.Velocity = new PointF(obj.Velocity.X, 0);
                            break;
                        }

                        if (difPosition.X != 0) // if there is a horizontal collision
                        {
                            obj.Fix_MapPosition(adjPosition.X, 0);

                            if (obj is Objects.Mario)
                                obj.Velocity = new PointF(0, obj.Velocity.Y); // khi va chạm thì mario ko đi đc theo hướng ngang, bởi vận tốc mario phụ thuộc vào ng chơi.
                            else
                                obj.Velocity = new PointF(-obj.Velocity.X, obj.Velocity.Y); // va chạm với hướng ngang thì con nấm đổi hướng và đi ngc lại

                            break;
                        }
                    }
        }
        private PointF Get_PositionAdjust(RectangleF colArea, PointF difPosition) 
        {
            float _x =
                difPosition.X > 0 ? -colArea.Width :
                difPosition.X < 0 ? colArea.Width :
                0;

            float _y =
                difPosition.Y > 0 ? -colArea.Height :
                difPosition.Y < 0 ? colArea.Height :
                0;

            return new PointF(_x, _y);
        }
        private bool Has_TileCollision(int row, int col)
        {
            if (col < 0 || col >= Size.Width)
                return true;

            if (row < 0 || row >= Size.Height)
                return false;

            return Matrix[col, row];
        }
        
    }
}
