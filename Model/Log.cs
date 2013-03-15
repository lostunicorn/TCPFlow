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

        private Tuple<uint, uint> m_steadyState;
        public Tuple<uint, uint> SteadyState
        {
            get
            {
                return m_steadyState;
            }
            set
            {
                if (m_steadyState != value)
                {
                    m_steadyState = value;
                    OnSteadyStateChanged();
                }
            }
        }

        public event Action<Tuple<uint, uint>> SteadyStateChanged;
        protected virtual void OnSteadyStateChanged()
        {
            if (SteadyStateChanged != null)
                SteadyStateChanged(SteadyState);
        }

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

        private void AddToHistory(string element)
        {
            m_historyTiming.Add(m_history.Length, m_controller.Time);

            if (m_lastEventTime != uint.MaxValue &&
                m_controller.Time != m_lastEventTime)
            {
                m_history.Append(m_controller.Time - m_lastEventTime);

                string history = m_history.ToString();
                List<int> matches = new List<int>();
                int pos = history.Length;
                while (pos > 0)
                {
                    pos = history.LastIndexOf(element, pos - 1);
                    if (pos != -1)
                        matches.Add(pos);
                }

                bool found = false;

                List<int>.Enumerator secondEn = matches.GetEnumerator();
                while (!found &&
                    secondEn.MoveNext())
                {
                    int secondPos = secondEn.Current;

                    //TODO: see if we can limit firstEn's range through Linq (filter matches)
                    List<int>.Enumerator firstEn = matches.GetEnumerator();
                    while (!found &&
                        firstEn.MoveNext())
                    {
                        int firstPos = firstEn.Current;
                        if (firstPos < secondPos)
                        {
                            string firstSection = history.Substring(firstPos, secondPos - firstPos);
                            string secondSection = history.Substring(secondPos);

                            if (firstSection.Equals(secondSection) &&
                                firstSection.Contains('P') &&
                                firstSection.Contains('A') &&
                                firstSection.Contains('D'))
                            {
                                uint firstTime, secondTime;
                                while (!m_historyTiming.ContainsKey(firstPos))
                                    --firstPos;
                                firstTime = m_historyTiming[firstPos];

                                while (!m_historyTiming.ContainsKey(secondPos))
                                    --secondPos;
                                secondTime = m_historyTiming[secondPos];

                                //rejoice! steady state!
                                found = true;
                                SteadyState = new Tuple<uint, uint>(firstTime, secondTime);
                            }
                        }
                    }
                }
            }

            m_history.Append(element);
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

            m_steadyState = new Tuple<uint, uint>(uint.MaxValue, uint.MaxValue);
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
            string str = string.Format(m_controller.sender.CongestionControlEnabled ? "CW{0}RW{1}O{2}{3}" : "RW{1}O{2}{3}", state.CongestionWindow, state.ReceiveWindow, outstanding.ToString(), state.Timedout);
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
