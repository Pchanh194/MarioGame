using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game.Elements
{
    public class KeyCode
    {
        public static KeyCode keyCode = new KeyCode();
        private KeyCode()
        {

        }
        public bool Space { get; private set; }
        public bool Right { get; private set; }
        public bool Left { get; private set; }
        public bool Up { get; private set; }
        public bool Down { get; private set; }

        public void SetKey(Keys key)
        {
            if (key == Keys.Space) this.Space = true;
            if (key == Keys.Right) this.Right = true;
            if (key == Keys.Left) this.Left = true;
            if (key == Keys.Up) this.Up = true;
            if (key == Keys.Down) this.Down = true;
        }

        // Clear selected keys
        public void Clear()
        {
            Space = false;
            Right = false;
            Left = false;
            Up = false;
            Down = false;
        }
        public static KeyCode getKeyCode()
        {
            return keyCode;
        }
    }
}
