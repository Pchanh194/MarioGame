using Game.Elements;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioBros.Elements.Objects
{
    public class Base : Game.Elements.Sprite
    {
        private int index = 0; // number indicating the current frame of the animation to be drawn
        private int milisec = 0; // auxiliary variable that adds the elapsed milliseconds in each iteration of the update method
        private PointF positionOnMap;

        public event EventHandler<PositionEventArgs> PositionOnMapChanged;

        public Base()
        {
            FPS = 1;
        }
        // List of rectangles that make up the current animation
        protected Rectangle[] AnimationRects { get; set; }

        // Rectangle to draw in the current frame
        public Rectangle AnimationRect { get { return AnimationRects[index]; } }

        // Indicates the speed in "frames per second" that the character's animations will have
        public int FPS { get; set; }

        // Direction where the character is looking
        protected Direction DirectionState { get; set; }

        // Speed of the object when moving, adjusts its position on the stage
        public PointF Velocity { get; set; }

        // Position of the object on the map
        public virtual PointF PositionOnMap
        {
            get 
            { 
                return positionOnMap; 
            }
            set
            {
                PositionEventArgs arg = new PositionEventArgs(value, positionOnMap);
                positionOnMap = value;
                if (arg.CurrentPosition != arg.PreviousPosition)
                {
                    if (AnimationRects != null)
                        PositionOnMapRec = new RectangleF(value.X, value.Y, AnimationRect.Width, AnimationRect.Height);

                    if (PositionOnMapChanged != null)
                        PositionOnMapChanged(this, arg);
                }
                // Trigger the event indicating that the object moved on the map
            }
        }
        
        // Rectangle that the character occupies on the map
        public RectangleF PositionOnMapRec { get; private set; }
        
        // Indicates that the object will be removed from the map
        public bool Discarding { get; set; }


        // Make character animation
        public void Animation(GameTimer GameTimer)
        {
            milisec += GameTimer.FrameMilliseconds; // time counter in milliseconds (auxiliary variable)
            if (milisec >= 1000 / FPS) // delay will be calculated per frame of animation
            {
                milisec = 0;
                if (index < AnimationRects.Length - 1)
                    index++; // delay will be calculated through each frame of the animation
                else
                    index = 0; // if the current state does NOT have more frames, go back to the first one to do a loop animation
            }
        }
        
        // Adjust the position of the object on the map without triggering the event
        public void Fix_MapPosition(float x, float y)
        {
            positionOnMap = new PointF(positionOnMap.X + x, positionOnMap.Y + y);
        }

        // Validate the collision of the with another object
        public virtual void CheckCollision(Base obj, PointF previousPosition)
        {
        }

        // Create the collection of rectangles that make up an animation
        protected Rectangle[] Create_Rectangles(Size size, params Point[] regions)
        {
            Rectangle[] _rect = new Rectangle[regions.Length];
            for (int i = 0; i < regions.Length; i++)
                _rect[i] = new Rectangle(regions[i], size);

            return _rect;
        }
        
        // Restart the animation
        protected void ResetAnimation()
        {
            index = 0;
        }

        public virtual void Update(GameTimer GameTimer)
        {
            // this.PositionOnMap = new PointF(this.PositionOnMap.X + Velocity.X, this.PositionOnMap.Y + Velocity.Y);
            this.Animation(GameTimer);
        }

        public override void Draw(DrawProcessor drawProcessor)
        {
            drawProcessor.Draw(base.Image, this.AnimationRect, (int)base.Position.X, (int)base.Position.Y, DirectionState == Direction.Left);
        }
    }

    public class PositionEventArgs : EventArgs
    {
        public PositionEventArgs(PointF current, PointF previous)
        {
            this.CurrentPosition = current;
            this.PreviousPosition = previous;
        }

        public PointF CurrentPosition { get; set; }
        public PointF PreviousPosition { get; set; }
    }

    // Direction where the character is looking
    public enum Direction
    {
        Right,
        Left
    }
}
