using Game.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioBros.Elements.Objects
{
    public class Monster : Base, IGravity
    {
        private MonsterState _monsterState;
        private int miliseconsDying;

        public Monster(Elements.Resources resources, Data.Object obj)
        {

        }

        public Rectangle[] AnimationRec_Normal { get; set; }
        public Rectangle[] AnimationRec_Dying { get; set; }

        public MonsterState State
        {
            get { return _monsterState; }
            set
            {
                _monsterState = value;
                base.ResetAnimation();
                AnimationRects = value == MonsterState.Normal ? AnimationRec_Normal : AnimationRec_Dying;
            }
        }

        public override void CheckCollision(Base obj, PointF previousPosition)
        {
            if (this.State == MonsterState.Dead)
                return;

            PointF difPosition = new PointF(obj.PositionOnMap.X - previousPosition.X, obj.PositionOnMap.Y - previousPosition.Y); // difference between current and previous position

            if (obj is Mario)
            {
                Mario mario = (Mario)obj;
                if (difPosition.Y != 0) // if there is a vertical collision
                {
                    this.State = MonsterState.Dead;
                    this.Velocity = PointF.Empty;

                    mario.ActionStatus = MarioAction.Jump;
                    mario.Velocity = new PointF(obj.Velocity.X, -15); // bounce mario bros
                }
                else
                    mario.Kill(); // kill MarioBros
            }
            else if (obj is Box || obj is Brick)
                this.PositionOnMap = new PointF(this.PositionOnMap.X, obj.PositionOnMap.Y - obj.AnimationRect.Height);
            else
                obj.Velocity = new PointF(-obj.Velocity.X, obj.Velocity.Y); // switch to walking in the opposite direction
        }

        public override void Update(GameTimer GameTimer)
        {
            if (this.State == MonsterState.Dead)
            {
                // counter before disappearing
                miliseconsDying += GameTimer.FrameMilliseconds;
                if (miliseconsDying >= 1000)
                    Discarding = true;
            }

            base.Update(GameTimer);
        }

        public enum MonsterState
        {
            Normal,
            Dead
        }

    }
}
