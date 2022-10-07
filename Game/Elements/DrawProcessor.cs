using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Elements
{
    // Class with drawing logic
    public class DrawProcessor : IDisposable
    {
        public DrawProcessor(int width, int height)
        {
            ImageBase = new Bitmap(width, height);
            Graphics = System.Drawing.Graphics.FromImage(ImageBase);
            Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        }

        /// ImageBase on which the other images are drawn
        public Image ImageBase { get; private set; }
        
        // Class with drawing functions
        private System.Drawing.Graphics Graphics { get; set; }

        public void Dispose()
        {
            Graphics.Dispose();
            ImageBase = null;
        }

        // Draw an image on the screen
        
        public void Draw(Image image, Rectangle rectangle, int x, int y, bool flip = false)
        {
            if (!flip)
                Graphics.DrawImage(image, x, y, rectangle, GraphicsUnit.Pixel);
            else
            {
                var _image = new Bitmap(rectangle.Width, rectangle.Height); // Get the image/dimension of the rectangle
                using (var _graphics = System.Drawing.Graphics.FromImage(_image))
                {
                    _graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    _graphics.DrawImage(image, 0, 0, rectangle, GraphicsUnit.Pixel);
                }
                _image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                Graphics.DrawImage(_image, x, y);
            }
        }
        public void Draw(Image image, Rectangle rectangle, Point position)
        {
            Graphics.DrawImage(image, position.X, position.Y, rectangle, GraphicsUnit.Pixel);
        }
        public void Draw(Image image, Point position)
        {
            Graphics.DrawImage(image, position.X, position.Y, image.Width, image.Height);
        }
    }
}
