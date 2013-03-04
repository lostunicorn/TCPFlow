using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    class Timing
    {
        public static uint Time { get; set; }

        public static void Reset() {
            Time = 0;
        }

        event Action Tick;
        public void DoTick()
        {
            if (Tick != null)
                Tick();
        }
    }
}
