using Game.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioBros.Elements.Objects
{
    public class Koopa : Goomba, IGravity
    {

        #region Constructor
        public Koopa(Elements.Resources resources, Data.Object obj) : base(resources, obj)
        {
            base.Image = resources.TextureAtlas;

            Size _unitSize = new Size(resources.MapData.tilewidth, resources.MapData.tileheight * 2);
            base.AnimationRec_Normal = base.Create_Rectangles(_unitSize,
                new Point(224, 384),
                new Point(256, 384)
            );
            base.AnimationRec_Dying = base.Create_Rectangles(new Size(resources.MapData.tilewidth, resources.MapData.tileheight), new Point(192, 384));

            this.PositionOnMap = new PointF(obj.x, (int)obj.y - resources.MapData.tileheight);
            this.Velocity = new PointF(-2, 0);
            this.FPS = 6;
            this.State = MonsterState.Normal;
        }
        #endregion
    }
}

