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
        public Dictionary<uint, Receiver.PacketDeliveryArgs> delivered = new Dictionary<uint, Receiver.PacketDeliveryArgs>();

        public Dictionary<uint, Sender.State> senderStates = new Dictionary<uint, Sender.State>();
        public Dictionary<uint, Receiver.State> receiverStates = new Dictionary<uint, Receiver.State>();

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

            //TODO: add steady state detection
            string history = m_history.ToString();
            int posFirst, posSecond;
            posSecond = history.LastIndexOf(str);
            if (posSecond != -1)
            {
                posFirst = history.LastIndexOf(str, posSecond);

                if (posFirst != -1)
                {
                    string firstSection = history.Substring(posFirst, posSecond - posFirst);
                    string secondSection = history.Substring(posSecond);

                    if (firstSection.Equals(secondSection))
                    {
                        uint firstTime, secondTime;
                        while (!m_historyTiming.ContainsKey(posFirst))
                            --posFirst;
                        firstTime = m_historyTiming[posFirst];

                        while (!m_historyTiming.ContainsKey(posSecond))
                            --posSecond;
                        secondTime = m_historyTiming[posSecond];

                        //rejoice! steady state!
                        System.Diagnostics.Debug.Print("Steady state detected from {0} to {1}", firstTime, secondTime);
                    }
                }
            }

            m_history.Append(str);
            m_lastEventTime = m_controller.Time;
        }

        public void Reset()
        {
            ClearHistory();

            packets.Clear();
            acks.Clear();
            delivered.Clear();

            senderStates.Clear();
            receiverStates.Clear();
        }

        public void OnPacketSent(DataPacket packet)
        {
            packets[m_controller.Time] = packet;

            AddToHistory("P");
        }

        public void OnPacketLost(DataPacket packet)
        {
            ClearHistory();
        }

        public void OnAckSent(Ack ack)
        {
            acks[m_controller.Time] = ack;

            AddToHistory("A");
        }

        public void OnAckLost(Ack ack)
        {
            ClearHistory();
        }

        public void OnPacketDelivered(Receiver.PacketDeliveryArgs args)
        {
            delivered[m_controller.Time] = args;

            AddToHistory("D");
        }

        public void OnSenderStateChanged(Sender.State state)
        {
            senderStates[m_controller.Time] = state;

            StringBuilder outstanding = new StringBuilder();
            foreach (uint id in state.Outstanding)
            {
                outstanding.Append(state.NextID - id);
                outstanding.Append('|');
            }
            string str = string.Format("CW{0}RW{1}O{2}{3}", state.CongestionWindow, state.ReceiveWindow, outstanding.ToString(), state.Timedout);
            AddToHistory(str);
        }

        public void OnReceiverStateChanged(Receiver.State state)
        {
            receiverStates[m_controller.Time] = state;

            StringBuilder buffer = new StringBuilder();
            foreach (uint id in state.Buffer)
            {
                buffer.Append(id - state.NextID);
                buffer.Append('|');
            }

            string str = String.Format("B{0}{1}", buffer.ToString(), state.Timedout);
            AddToHistory(str);
        }
    }
}
