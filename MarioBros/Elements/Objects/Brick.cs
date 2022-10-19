using Game.Elements;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioBros.Elements.Objects
{
    public class Brick : Base, IGravity
    {
        private PointF? _originalPos;
        public Brick(Elements.Resources resources, Data.Object obj)
        {
            base.Image = resources.TextureAtlas;

            Size _unitSize = new Size(resources.MapData.tilewidth, resources.MapData.tileheight);
            AnimationRec_Normal = base.Create_Animation(_unitSize, new Point(224, 0));
            AnimationRects = AnimationRec_Normal;

            this._originalPos = new PointF(obj.x, (int)obj.y - resources.MapData.tileheight);
            this.PositionOnMap = _originalPos.Value;
        }
        public override PointF PositionOnMap
        {
            get => base.PositionOnMap;
            set => base.PositionOnMap = new PointF(value.X, Math.Min(value.Y, _originalPos.Value.Y));
        }

        public override void CheckCollision(Base obj, PointF previousPosition)
        {
            PointF difPosition = new PointF(obj.PositionOnMap.X - previousPosition.X, obj.PositionOnMap.Y - previousPosition.Y); // difference between current and previous position
            if (difPosition.Y < 0) // Cham duoi
            {
                obj.PositionOnMap = new PointF(obj.PositionOnMap.X, this.PositionOnMap.Y + obj.AnimationRect.Height);
                obj.Velocity = new PointF(obj.Velocity.X, 0);

                Velocity = new PointF(Velocity.X, -10);
            }
            else if (difPosition.Y > 0) // cham tren
            {
                obj.PositionOnMap = new PointF(obj.PositionOnMap.X, this.PositionOnMap.Y - obj.AnimationRect.Height);
                obj.Velocity = new PointF(obj.Velocity.X, 0);
            }
            else if (difPosition.X < 0)
            {
                obj.PositionOnMap = new PointF(this.PositionOnMap.X + obj.AnimationRect.Width, obj.PositionOnMap.Y);
                obj.Velocity = new PointF(0, obj.Velocity.Y);
            }
            else if (difPosition.X > 0)
            {
                obj.PositionOnMap = new PointF(this.PositionOnMap.X - obj.AnimationRect.Width, obj.PositionOnMap.Y);
                obj.Velocity = new PointF(0, obj.Velocity.Y);
            }
        }
        private Rectangle[] AnimationRec_Normal { get; set; }
    }
}
