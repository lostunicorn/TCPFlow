using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    class DataPacket : Packet
    {
        public uint ID { get; set; }

        public uint Number { get; private set; }
        private static uint m_nextNumber = 0;

        public DataPacket(uint time, uint id) : base(time)
        {
            ID = id;
            Number = m_nextNumber++;
        }
    }
}
