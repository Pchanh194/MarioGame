using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Elements
{
    /// Objects that was drawn on the screen
    public class Sprite
    {
        public Sprite()
        {
            Visible = true;
        }
        // Instance to the Sprite to draw
        public Sprite(Image image, Point position)
        {
            this.Image = image;
            this.Position = position;

            Visible = true;
        }
        
        // Image to draw
        public Image Image { get; set; }

        // Determines whether or not to draw the image
        public bool Visible { get; set; }

        // Position on the screen where the image will be drawn
        public PointF Position { get; set; }
        
        // Draw all sprites on screen
        // ImageBase: Base image to where it will be drawn
        // Class with drawing methods
        public virtual void Draw(DrawProcessor drawProcessor)
        {
            if (this.Visible)
                drawProcessor.Draw(this.Image, new Point((int)this.Position.X, (int)this.Position.Y));
        }
    }
}
