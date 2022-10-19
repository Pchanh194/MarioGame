using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Elements;

namespace MarioBros.Elements.Objects
{
    public class Mario : Base, IGravity
    {
        private MarioAction _marioAction;
        private Direction _direction;

        public event EventHandler Dead;

        public Mario(Elements.Resources resources, Data.Object obj)
        {
            base.Image = resources.TextureAtlas;

            Size _unitSize = new Size(resources.MapData.tilewidth, resources.MapData.tileheight);
            AnimationRec_Small_Stand = base.Create_Animation(_unitSize, new Point(320, 640));
            AnimationRec_Small_Walk = base.Create_Animation(_unitSize, new Point(416, 640), new Point(320, 672), new Point(352, 672));
            AnimationRec_Small_Jump = base.Create_Animation(_unitSize, new Point(384, 672));
            AnimationRec_Small_Dead = base.Create_Animation(_unitSize, new Point(352, 640));
            AnimationRec_Small_Stop = base.Create_Animation(_unitSize, new Point(384, 640));
            AnimationRec_Small_Pillar = base.Create_Animation(_unitSize, new Point(320, 704));

            ActionStatus = MarioAction.Idle;
            DirectionState = Direction.Right;

            this.FPS = 12;
            this.Velocity = PointF.Empty;
            this.PositionOnMap = new PointF(obj.x, (int)obj.y - resources.MapData.tileheight);
        }

        public MarioAction ActionStatus
        {
            get 
            { 
                return _marioAction; 
            }
            set
            {
                _marioAction = value;
                SetSourceRectangle();
            }
        }

        // Direction where the character is looking
        new public Direction DirectionState
        {
            get 
            { 
                return _direction; 
            }
            set
            {
                _direction = value;
                SetSourceRectangle();
            }
        }
        private Rectangle[] AnimationRec_Small_Stand { get; set; }
        private Rectangle[] AnimationRec_Small_Walk { get; set; }
        private Rectangle[] AnimationRec_Small_Jump { get; set; }
        private Rectangle[] AnimationRec_Small_Dead { get; set; }
        private Rectangle[] AnimationRec_Small_Stop { get; set; }
        private Rectangle[] AnimationRec_Small_Run { get; set; }
        private Rectangle[] AnimationRec_Small_Pillar { get; set; }
        
        // Sets the value of the array of rectangles to be drawn
        private void SetSourceRectangle()
        {
            // Console.WriteLine(ActionStatus.ToString());
            // depending on the new state of the character assigns the array of rectangle that corresponds to AnimationRect so that it is drawn correctly
            if (ActionStatus == MarioAction.Idle)
                AnimationRects = AnimationRec_Small_Stand;
            else if (ActionStatus == MarioAction.Walk)
                AnimationRects = AnimationRec_Small_Walk;
            else if (ActionStatus == MarioAction.Jump || ActionStatus == MarioAction.Falling)
                AnimationRects = AnimationRec_Small_Jump;
            else if (ActionStatus == MarioAction.Die)
                AnimationRects = AnimationRec_Small_Dead;
            else if (ActionStatus == MarioAction.Stop)
                AnimationRects = AnimationRec_Small_Stop;
            else if (ActionStatus == MarioAction.Pillar)
                AnimationRects = AnimationRec_Small_Pillar;

            ResetAnimation();
        }

        // Interaction with the character
        public void MoveCharacter()
        {
            if (this.ActionStatus == MarioAction.Die)
                return;

            float _aceleration = 0.2f;
            float _maxAceleration = 6f;

            // TURBO
            if (Elements.KeyCode.Turbo) // double the speed when the character runs
            {
                _aceleration *= 2;
                _maxAceleration *= 2;
            }
            else if (Math.Abs(Velocity.X) > _maxAceleration)
            {
                // if I stop using turbo, I slow down
                Velocity = (Velocity.X < 0 ? new PointF(Velocity.X + _aceleration, Velocity.Y) : new PointF(Velocity.X - _aceleration, Velocity.Y));
                Velocity = new PointF((float)Math.Round(Velocity.X, 1), (float)Math.Round(Velocity.Y, 1)); // correction
            }
            FPS = Math.Abs(Velocity.X) <= _maxAceleration / 2 ? 6 : 12; // speed up the animation depending on the speed
            if (Elements.KeyCode.Turbo)
                FPS *= 2;

            // RIGHT
            if (Elements.KeyCode.Right) // shift right
            {
                if (Velocity.X < _maxAceleration)
                    Velocity = new PointF(Velocity.X + _aceleration, Velocity.Y);

                if ((ActionStatus != MarioAction.Jump && ActionStatus != MarioAction.Falling))
                {
                    if (DirectionState != Direction.Right)
                        DirectionState = Direction.Right;

                    if (Velocity.X <= 0)
                    {
                        if (ActionStatus != MarioAction.Stop)
                            ActionStatus = MarioAction.Stop;
                    }
                    else if (ActionStatus != MarioAction.Walk)
                        ActionStatus = MarioAction.Walk;
                }
            }
            
            // LEFT
            if (Elements.KeyCode.Left) // shift left
            {
                if (Velocity.X > -_maxAceleration)
                    Velocity = new PointF(Velocity.X - _aceleration, Velocity.Y);

                if ((ActionStatus != MarioAction.Jump && ActionStatus != MarioAction.Falling))
                {
                    if (DirectionState != Direction.Left)
                        DirectionState = Direction.Left;

                    if (Velocity.X > 0)
                    {
                        if (ActionStatus != MarioAction.Stop)
                            ActionStatus = MarioAction.Stop;
                    }
                    else if (ActionStatus != MarioAction.Walk)
                        ActionStatus = MarioAction.Walk;
                }
            }
            
            // JUMP
            if (Elements.KeyCode.Jump && (ActionStatus != MarioAction.Jump && ActionStatus != MarioAction.Falling))
            {
                ActionStatus = MarioAction.Jump;
                float _jAaceleration = Elements.KeyCode.Turbo ? 24 : 20;
                Velocity = new PointF(Velocity.X, -_jAaceleration);
            }

            if (ActionStatus == MarioAction.Falling && Velocity.Y == 0)
                ActionStatus = Velocity.X != 0 ? MarioAction.Walk : MarioAction.Stop;

            if (ActionStatus == MarioAction.Jump && Velocity.Y >= 0)
                ActionStatus = MarioAction.Falling;
            
            // STOP WALK
            if (ActionStatus != MarioAction.Jump && !Elements.KeyCode.Right && !Elements.KeyCode.Left) // stop walking
            {
                float _velX = (Velocity.X > 0 ? -(_aceleration * 2) : Velocity.X < 0 ? (_aceleration * 2) : 0) + Velocity.X;
                if (Math.Abs(Velocity.X) < (_aceleration * 2)) _velX = 0;

                Velocity = new PointF((float)Math.Round(_velX, 2), Velocity.Y);
            }
            
            // IDLE
            if (ActionStatus != MarioAction.Jump && ActionStatus != MarioAction.Falling && ActionStatus != MarioAction.Idle && Velocity.X == 0) // retorna a estado de espera
                ActionStatus = MarioAction.Idle;
        }

        // kill MarioBros bros
        public void Kill()
        {
            // this.ActionStatus = MarioAction.Die; // change the state to display the corresponding sprite
            this.Velocity = new PointF(Velocity.X, -20); // change speed to show death jump
            // this.Dead(this, EventArgs.Empty); // notify the game controller that mario died to change the state of the game
        }

        public override void Update(GameTimer GameTimer)
        {
            this.MoveCharacter();

            base.Update(GameTimer);
        }
        public override void Draw(DrawProcessor drawProcessor)
        {
            drawProcessor.Draw(base.Image, base.AnimationRect, (int)base.Position.X, (int)base.Position.Y, DirectionState == Direction.Left);
        }
    }
    public enum MarioAction // Different types of actions that the character can perform
    {   
        Walk,
        Jump,
        Idle,
        Falling,
        Stop,
        Die,    
        Pillar,     
    }
}
