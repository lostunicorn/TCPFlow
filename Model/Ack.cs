using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    class Ack : Packet
    {
        public uint NextID { get; set; }

        public Ack(uint time, uint nextID) : base(time)
        {
            NextID = nextID;
        }
    }
}
