using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    class Log
    {
        public List<DataPacket> packets = new List<DataPacket>();
        public List<Ack> acks = new List<Ack>();
        public List<DataPacket> delivered = new List<DataPacket>();

        private StringBuilder m_history;
        private Packet m_lastPacket;

        public Log()
        {
            m_history = new StringBuilder();
        }

        public void ClearHistory()
        {
            m_history.Clear();
            m_lastPacket = null;
        }

        public void Reset()
        {
            ClearHistory();

            packets.Clear();
            acks.Clear();
            delivered.Clear();
        }

        public void OnPacketSent(DataPacket packet)
        {
            packets.Add(packet);
        }

        public void OnPacketDropped(DataPacket packet)
        {
            ClearHistory();
        }

        public void OnAckSent(Ack ack)
        {
            acks.Add(ack);
        }

        public void OnAckDropped(Ack ack)
        {
            ClearHistory();
        }

        public void OnPacketDelivered(DataPacket packet)
        {
            delivered.Add(packet);
        }
    }
}
