using Game.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioBros.Elements.Objects
{
    public class Coin : Base, IGravity
    {
        private PointF? _originalPos;

        public Coin(Elements.Resources resources)
        {
            base.Image = resources.TextureAtlas;

            Size _unitSize = new Size(resources.MapData.tilewidth, resources.MapData.tileheight);
            AnimationRects = base.Create_Rectangles(_unitSize, new Point(256, 192),
                                                               new Point(288, 192),
                                                               new Point(320, 192),
                                                               new Point(352, 192));

            this.Velocity = new PointF(0, -20);
            this.FPS = 6;
        }

        public override void Update(GameTimer GameTimer)
        {
            if (_originalPos == null)
                _originalPos = this.PositionOnMap;

            if (this.PositionOnMap.Y > _originalPos.Value.Y)
                this.Discarding = true;

            base.Update(GameTimer);
        }
    }
}
