using Game.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioBros.Elements.Objects
{
    public class Box : Base, IGravity
    {
        // Objects
        private BoxState _boxState;
        private PointF? _originalPos;
        public event EventHandler TossCoin; // Try to drop grown up mushroom

        // Constructor
        public Box(Elements.Resources resources, Data.Object obj)
        {
            base.Image = resources.TextureAtlas;

            Size _unitSize = new Size(resources.MapData.tilewidth, resources.MapData.tileheight);
            AnimationRec_Normal = base.Create_Rectangles(_unitSize, new Point(320, 0),
                                                                    new Point(320, 64),
                                                                    new Point(320, 128),
                                                                    new Point(320, 64),
                                                                    new Point(320, 0));
            AnimationRec_Empty = base.Create_Rectangles(_unitSize, new Point(224, 64));

            State = BoxState.Normal;
            this.FPS = 8;

            this._originalPos = new PointF(obj.x, (int)obj.y - resources.MapData.tileheight);
            this.PositionOnMap = _originalPos.Value;
        }

        // Properties
        public BoxState State
        {
            get 
            { 
                return _boxState; 
            }
            set
            {
                _boxState = value;
                AnimationRects = value == BoxState.Normal ? AnimationRec_Normal : AnimationRec_Empty;
                ResetAnimation();
            }
        }
        private Rectangle[] AnimationRec_Normal { get; set; }
        private Rectangle[] AnimationRec_Empty { get; set; }
        
        // private Rectangle[] AnimationRec_Coin { get; set; }
        public override PointF PositionOnMap      // Limit where boxes can drop down to after collision
        {
            get => base.PositionOnMap;
            set => base.PositionOnMap = new PointF(value.X, Math.Min(value.Y, _originalPos.Value.Y));
        }

        // Methods
        public override void CheckCollision(Base obj, PointF previousPosition)
        {
            PointF difPosition = new PointF(obj.PositionOnMap.X - previousPosition.X, obj.PositionOnMap.Y - previousPosition.Y); // difference between current and previous position
            if (difPosition.Y < 0)
            {
                obj.PositionOnMap = new PointF(obj.PositionOnMap.X, this.PositionOnMap.Y + obj.AnimationRect.Height); 
                obj.Velocity = new PointF(obj.Velocity.X, 0);

                if (State == BoxState.Normal)
                {
                    State = BoxState.Empty;
                    TossCoin(this, EventArgs.Empty);
                    Velocity = new PointF(Velocity.X, -10);
                }
            }
            else if (difPosition.Y > 0)
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
        
        // Structures
        public enum BoxState
        {
            Normal,
            Empty
        }
    }
}
