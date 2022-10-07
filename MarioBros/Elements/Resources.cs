using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioBros.Elements
{
    /// Class that loads game resources
    public class Resources
    { 
        /// Empty grid block
        public Image TextureAtlas { get; set; }

        public Data.Map MapData { get; set; }
    }
}
