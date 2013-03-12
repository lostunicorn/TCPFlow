﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    public class Ack : Packet
    {
        public uint NextID { get; set; }

        public uint Window { get; set; }

        public uint Number { get; private set; }
        private static uint m_nextNumber = 0;

        public static void Reset()
        {
            m_nextNumber = 0;
        }

        public Ack(uint time, uint nextID, uint window, uint flags = 0) : base(time, flags)
        {
            NextID = nextID;
            Window = window;
            Number = m_nextNumber++;
        }
    }
}
