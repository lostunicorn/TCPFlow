using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    public class Log
    {
        /*
         * Lists of packets maintained for rendering
         */
        public List<DataPacket> packets = new List<DataPacket>();
        public List<Ack> acks = new List<Ack>();
        public List<DataPacket> delivered = new List<DataPacket>();

        private StringBuilder m_history;
        private uint m_lastEventTime;

        public Log()
        {
            m_history = new StringBuilder();

            m_lastEventTime = uint.MaxValue;
        }

        public void ClearHistory()
        {
            m_history.Clear();
            m_lastEventTime = uint.MaxValue;
        }

        private void AddToHistory(string str)
        {
            if (m_lastEventTime != uint.MaxValue)
                m_history.Append(Timing.Time - m_lastEventTime);

            m_history.Append(str);
            m_lastEventTime = Timing.Time;

            //TODO: add steady state detection
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

            AddToHistory("P");
        }

        public void OnPacketDropped(DataPacket packet)
        {
            ClearHistory();
        }

        public void OnAckSent(Ack ack)
        {
            acks.Add(ack);

            AddToHistory("A");
        }

        public void OnAckDropped(Ack ack)
        {
            ClearHistory();
        }

        public void OnPacketDelivered(DataPacket packet)
        {
            delivered.Add(packet);

            AddToHistory("D");
        }
    }
}
