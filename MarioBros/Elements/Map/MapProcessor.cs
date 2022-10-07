using Game.Elements;
using MarioBros.Elements.Objects;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MarioBros.Elements.Map
{
    public class MapHandler
    {
        // Objects
        public event EventHandler Restart;
        private float _gravity = 2f;
        private Size _frameSize;
        private RectangleF _canvasRec;

        public MapHandler(Elements.Resources resources, Size frameSize)
        {
            _frameSize = frameSize;
            State = GameState.Play;

            Objects_Layer = new Objects_Layer(resources, frameSize);
            Obstacles_Layer = new Obstacles_Layer(resources);
            Tiles_Layer = new Tiles_Layer(resources, frameSize);

            // Binds the position change event of objects on the map
            Objects_Layer.MapObj.ForEach(x => x.PositionOnMapChanged += ChangeObjPosition); 
            Objects_Layer.Mario.Dead += Mario_Dead;

            UpdateObjPosition(); // update the initial position of the objects
        }


        // State of the running game
        public GameState State { get; set; }

        // Map objects
        public Objects_Layer Objects_Layer { get; set; }

        // Map obstacles information
        public Obstacles_Layer Obstacles_Layer { get; set; }

        // Graphic map information
        public Tiles_Layer Tiles_Layer { get; set; }

        public void Update(GameTimer gameTimer)   
        {
            if (State == GameState.Play)
                UpdatePlaying(gameTimer);
            else if (State == GameState.Win)
                UpdateWin(gameTimer);
            else if (State == GameState.Die)
                UpdateDie(gameTimer);

            if (Objects_Layer.Mario.Position.Y > (this._frameSize.Height * 2))
                this.Restart(null, EventArgs.Empty);
            // if mario falls into a well or completes the map, it resets
        }
        private void UpdatePlaying(GameTimer gameTimer)
        {
            _canvasRec = new RectangleF(this.Tiles_Layer.Position.X, this.Tiles_Layer.Position.Y, 
                                        _frameSize.Width, _frameSize.Height);

            List<Base> lstDiscard = new List<Base>();
            this.Objects_Layer.MapObj.ForEach(obj =>
            {
                if (obj is IGravity)
                {
                    obj.Velocity = new PointF(obj.Velocity.X, obj.Velocity.Y + _gravity);   // v = gt
                    obj.PositionOnMap = new PointF(obj.PositionOnMap.X, obj.PositionOnMap.Y + obj.Velocity.Y);    // s' = s + v
                    // when updating the position of the map in Y, the collisions are validated and the position is adjusted if necessary
                }

                // obj.Update(GameTimer);

                if (obj.Velocity.X != 0)
                {
                    if (obj.PositionOnMapRec.IntersectsWith(_canvasRec)) // objects move only if they are in view 
                        obj.PositionOnMap = new PointF(obj.PositionOnMap.X + obj.Velocity.X, obj.PositionOnMap.Y);
                }
                obj.Update(gameTimer);

                if (obj.Discarding)
                    lstDiscard.Add(obj);
            });

            lstDiscard.ForEach(x => this.Objects_Layer.MapObj.Remove(x)); // remove discarded objects from the list
            this.Objects_Layer.MapObjNew.ForEach(x => this.Objects_Layer.MapObj.Add(x)); // new objects created in the course of the game are added, in this example coins
            this.Objects_Layer.MapObjNew.Clear();

            CheckCharacterMove();
            UpdateObjPosition();

            if (this.Objects_Layer.Mario.PositionOnMap.X + (this.Objects_Layer.Mario.AnimationRect.Width / 2) >= this.Objects_Layer.Pillar.X)
            {
                Mario mario = this.Objects_Layer.Mario;
                mario.ActionStatus = MarioAction.Pillar;
                mario.PositionOnMap = new PointF(this.Objects_Layer.Pillar.X - 10, mario.PositionOnMap.Y);
                mario.Velocity = new PointF(0, 0);
                this.State = GameState.Win;
                // if mario reaches the pillar change the state of the game
            }
        }
        private void UpdateWin(GameTimer gameTimer)
        {
            Mario mario = Objects_Layer.Mario;
            mario.Velocity = new PointF(mario.Velocity.X, mario.Velocity.Y + _gravity);
            mario.PositionOnMap = new PointF(mario.PositionOnMap.X + mario.Velocity.X, mario.PositionOnMap.Y);
            mario.PositionOnMap = new PointF(mario.PositionOnMap.X, mario.PositionOnMap.Y + mario.Velocity.Y);
            mario.Animation(gameTimer);

            if (mario.ActionStatus == MarioAction.Pillar)
            {
                var pillar = Objects_Layer.Pillar;
                if (mario.PositionOnMap.Y + mario.AnimationRect.Height == pillar.Y + pillar.Height)
                {
                    mario.ActionStatus = MarioAction.Walk;
                    mario.Velocity = new PointF(6, 0);
                }
            }

            UpdateObjPosition(mario); // update only Mario's position on the map

            if (mario.PositionOnMap.X >= ((Tiles_Layer.Size.Width - 1) * Tiles_Layer.SizeOfTiles.Width))
            {
                this.Restart(null, EventArgs.Empty); // reset the map (or load the winning screen)
            }
        }

        private void UpdateDie(GameTimer gameTimer)
        {
            // show the animation of mario dying
            Mario mario = Objects_Layer.Mario;
            mario.Velocity = new PointF(mario.Velocity.X, mario.Velocity.Y + _gravity);
            mario.PositionOnMap = new PointF(mario.PositionOnMap.X, mario.PositionOnMap.Y + mario.Velocity.Y);
            // when updating the position of the map in Y, the collisions are validated and the position is adjusted if necessary

            UpdateObjPosition(mario); // update only Mario's position on the map 
        }

        // Draw the grid
        public void Draw(DrawProcessor drawProcessor)
        {
            this.Tiles_Layer.Draw(drawProcessor);
            this.Objects_Layer.Draw(drawProcessor);
        }

        private void ChangeObjPosition(object sender, PositionEventArgs e)
        {
            if (State == GameState.Play || State == GameState.Win)
            {
                // validate the collision of the object with the locked cells of the map
                this.Obstacles_Layer.ValidateCollision((Elements.Objects.Base)sender, e.PreviousPosition);

                // validate object collision with other objects
                this.Objects_Layer.ValidateCollision((Elements.Objects.Base)sender, e.PreviousPosition);
            }
        }

        // Updates the on-screen position of map objects
        private void UpdateObjPosition(Base mapObject = null)   // Note
        {
            List<Base> lstObjects;

            if (mapObject != null)
                lstObjects = new List<Base>() { mapObject };
            else
                lstObjects = this.Objects_Layer.MapObj;

            lstObjects.ForEach(obj =>
            {
                obj.Position = new PointF(obj.PositionOnMap.X - this.Tiles_Layer.Position.X, obj.PositionOnMap.Y - this.Tiles_Layer.Position.Y);
            });
        }

        // Movement of the character on screen
        private void CheckCharacterMove()
        {
            if (this.Objects_Layer.Mario.Position.X > _frameSize.Width / 2)
            {
                var Xdif = this.Objects_Layer.Mario.Position.X - (float)_frameSize.Width / 2f;
                this.Tiles_Layer.Position = new PointF(this.Tiles_Layer.Position.X + (float)Xdif, this.Tiles_Layer.Position.Y);

                UpdateObjPosition();
            }
            else if (this.Objects_Layer.Mario.Position.X < 0)
                this.Objects_Layer.Mario.PositionOnMap = new PointF(this.Tiles_Layer.Position.X, this.Objects_Layer.Mario.PositionOnMap.Y); // prevents the character from going off the left edge of the screen
        }

        private void Mario_Dead(object sender, EventArgs e)
        {
            this.State = GameState.Die; // when detecting that mario died, the state of the game changes
        }

        public enum GameState
        {
            Play, // game running
            Die, // show the animation of mario dying
            Win, // show the animation of mario winning
        }
    }
}
