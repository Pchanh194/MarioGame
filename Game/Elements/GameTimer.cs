using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Elements
{
    public class GameTimer
    {
        public GameTimer() 
        {
            StartTime =
            TimeFrame = DateTime.Now;
        }

        // Game start date and time
        public DateTime StartTime { get; set; }

        // Execution date and time of the current frame
        public DateTime TimeFrame { get; set; }

        // Total running time of the game
        public TimeSpan TotalTime { get { return TimeFrame - StartTime; } }

        // Milliseconds elapsed between current and previous frame
        public int FrameMilliseconds { get; set; }
    }
}
