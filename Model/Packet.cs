using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    public class Packet
    {
        public uint Time { get; set; }

        public bool Dropped { get; set; }

        public Packet(uint time)
        {
            Time = time;
        }
    }
}
