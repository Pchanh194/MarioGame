using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Elements;
using MarioBros.Elements.Objects;

namespace MarioBros.Elements.Map
{
    public class Objects_Layer : Game.Elements.Sprite
    {
        private Size _frameSize;

        public Objects_Layer(Elements.Resources resources, Size frameSize)
        {
            this._frameSize = frameSize;
            base.Image = resources.TextureAtlas;
            var _objLayer = resources.MapData.layers.First(x => x.name == "Objects");

            MapObjNew = new List<Base>();
            MapObj = new List<Base>();
            _objLayer.objects.ForEach(x =>
            {
                switch (Convert.ToInt32(x.@class))
                {
                    case 1:
                        Mario = new Mario(resources, x);
                        // MapObj.Add(Mario);
                        break;
                    case 2:
                        Box box = new Box(resources, x);
                        ((Box)box).TossCoin += (sender, e) =>
                        {
                            Coin coin = new Coin(resources);
                            coin.PositionOnMap = box.PositionOnMap;
                            MapObjNew.Add(coin); 
                        };
                        MapObj.Add(box);
                        break;
                    case 3:
                        Brick brick = new Brick(resources, x);
                        MapObj.Add(brick);
                        break;
                    //case 4:
                    //    mushroom to grow up // toad
                    //    break;
                    case 5:
                        Pillar = new Rectangle(x.x, x.y - x.height, x.width, x.height);
                        break;
                    case 6:
                        Goomba goomba = new Goomba(resources, x);
                        MapObj.Add(goomba);
                        break;
                    case 7:
                        Koopa koopa = new Koopa(resources, x);
                        MapObj.Add(koopa);
                        break;
                }
            });

            MapObj.Add(Mario); 
        }
        
        // Playable character mario bros
        public Mario Mario { get; private set; }
       
        // Pillar Location
        public Rectangle Pillar { get; set; }

        // New objects to add to the map
        public List<Base> MapObjNew { get; set; }

        // map objects
        public List<Base> MapObj { get; set; }


        public void ValidateCollision(Elements.Objects.Base obj, PointF previousPosition)
        {
            var objTargets = this.MapObj.Where(x => !x.Equals(obj) && x.PositionOnMapRec.IntersectsWith(obj.PositionOnMapRec)).ToList();
            if (objTargets.Any())
            {
                foreach (var item in objTargets)
                    item.CheckCollision(obj, previousPosition);
            }
        }

        public void Update(Game.Elements.GameTimer GameTimer)
        {
            MapObj.ForEach(x =>
            {
                // Only updated the object if it is visible on screen
                if (_frameSize.Width >= x.Position.X && x.Position.X + x.AnimationRect.Width > 0) 
                    x.Update(GameTimer);
            });
        }

        public override void Draw(DrawProcessor drawProcessor)
        {
            MapObj.ForEach(x =>
            {
                // Only draw the object if it is visible on the screen
                if (_frameSize.Width >= x.Position.X && x.Position.X + x.AnimationRect.Width > 0) 
                    x.Draw(drawProcessor);
            });
        }
    }
}
