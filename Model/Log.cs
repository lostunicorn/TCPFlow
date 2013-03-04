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
        private Dictionary<int, uint> m_historyTiming; //map index in the m_history string to a time
        private uint m_lastEventTime;

        public uint SteadyStateStart { get; private set; }
        public uint SteadyStateStop { get; private set; }

        public Log()
        {
            m_history = new StringBuilder();
            m_historyTiming = new Dictionary<int, uint>();

            m_lastEventTime = uint.MaxValue;
        }

        public void ClearHistory()
        {
            m_historyTiming.Clear();
            m_history.Clear();
            m_lastEventTime = uint.MaxValue;
        }

        private void AddToHistory(string str)
        {
            m_historyTiming.Add(m_history.Length, Timing.Time);

            if (m_lastEventTime != uint.MaxValue)
                m_history.Append(Timing.Time - m_lastEventTime);

            m_history.Append(str);
            m_lastEventTime = Timing.Time;

            //TODO: add steady state detection
            string history = m_history.ToString();
            int l = 1, len = history.Length, middle = len/2;
            bool found = false;
            while (!found && l < middle)
            {
                string substr = history.Substring(len - l);
                if (substr.Contains('P') &&
                    substr.Contains('A') &&
                    substr.Contains('D') &&
                    history.Substring(len-2*l, l).Equals(substr))
                {
                    SteadyStateStart = m_historyTiming[len - 2 * l];
                    SteadyStateStop = m_historyTiming[len - l];
                    //steady state detected!
                }
                ++l;
            }
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
