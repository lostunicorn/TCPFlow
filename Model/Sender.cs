﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    class Sender
    {
        public event Action<DataPacket> PacketSent;
        private void SendPacket(DataPacket packet)
        {
            if (PacketSent != null)
                PacketSent(null);
        }

        public void ReceiveAck(Ack ack)
        {
        }

        public void Tick()
        {
        }
    }
}