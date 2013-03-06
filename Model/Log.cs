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
        public Dictionary<uint, DataPacket> packets = new Dictionary<uint, DataPacket>();
        public Dictionary<uint, Ack> acks = new Dictionary<uint, Ack>();
        public Dictionary<uint, DataPacket> delivered = new Dictionary<uint, DataPacket>();

        private StringBuilder m_history;
        private Dictionary<int, uint> m_historyTiming; //map index in the m_history string to a time
        private uint m_lastEventTime;

        private Controller m_controller;

        public uint SteadyStateStart { get; private set; }
        public uint SteadyStateStop { get; private set; }

        public Log(Controller controller)
        {
            m_controller = controller;

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
            m_historyTiming.Add(m_history.Length, m_controller.Time);

            if (m_lastEventTime != uint.MaxValue)
                m_history.Append(m_controller.Time - m_lastEventTime);

            m_history.Append(str);
            m_lastEventTime = m_controller.Time;

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
            packets.Add(m_controller.Time, packet);

            OnChanged();

            AddToHistory("P");
        }

        public void OnPacketLost(DataPacket packet)
        {
            ClearHistory();
        }

        public void OnAckSent(Ack ack)
        {
            acks.Add(m_controller.Time, ack);

            OnChanged();

            AddToHistory("A");
        }

        public void OnAckLost(Ack ack)
        {
            ClearHistory();
        }

        public void OnPacketDelivered(DataPacket packet)
        {
            delivered.Add(m_controller.Time, packet);

            OnChanged();

            AddToHistory("D");
        }

        public event Action Changed;
        protected void OnChanged()
        {
            if (Changed != null)
                Changed();
        }
    }
}
