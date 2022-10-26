using Game.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioBros.Elements.Objects
{
    public class Goomba : Monster
    {
        public Goomba(Elements.Resources resources, Data.Object obj) : base(resources, obj)
        {
            base.Image = resources.TextureAtlas;

            Size _unitSize = new Size(resources.MapData.tilewidth, resources.MapData.tileheight);
            AnimationRec_Normal = base.Create_Animation(_unitSize,
                new Point(0, 480),
                new Point(32, 480)
            );
            AnimationRec_Dying = base.Create_Animation(_unitSize, new Point(64, 480));

            this.PositionOnMap = new PointF(obj.x, (int)obj.y - resources.MapData.tileheight);
            this.Velocity = new PointF(-2, 0);
            this.FPS = 6;
            this.State = MonsterState.Normal;
        }
    }
}
