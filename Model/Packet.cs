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

        public bool Lost { get; set; }

        public enum FLAGS
        {
            SYN = 1
        };

        public uint Flags { get; set; }

        public Packet(uint time, uint flags = 0)
        {
            Time = time;
            Flags = flags;
        }
    }
}
