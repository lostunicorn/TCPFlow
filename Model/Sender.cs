using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    public class Sender
    {
        public class State
        {
            public readonly uint Time;

            public readonly uint[] Outstanding;

            public readonly uint CongestionWindow;

            public readonly uint ReceiveWindow;

            public State(uint time, uint[] outstanding, uint receiveWindow, uint congestionWindow)
            {
                Time = time;
                Outstanding = outstanding;
                ReceiveWindow = receiveWindow;
                CongestionWindow = congestionWindow;
            }
        }

        public event Action<State> StateChanged;
        protected void ChangeState()
        {
            if (StateChanged != null)
            {
                StateChanged(new State(m_controller.Time, m_outstanding.Keys.ToArray(), ReceiveWindow, CongestionWindow));
            }
        }

        private Controller m_controller;

        private SortedList<uint, uint> m_outstanding;

        private Ack m_receivedAck;
        private List<Ack> m_previousAcks;

        private uint m_nextID;

        public uint AckTimeout { get; set; }

        public bool SkipHandshake { get; set; }

        public uint ReceiveWindow
        {
            get
            {
                return m_previousAcks.Count == 0 ? 1 : m_previousAcks[m_previousAcks.Count - 1].Window;
            }
        }
        public uint CongestionWindow { get; private set; }

        public const int START_ID = 0;

        public Sender(Controller controller, bool skipHandshake, uint ackTimeout)
        {
            m_controller = controller;

            m_outstanding = new SortedList<uint, uint>();

            m_previousAcks = new List<Ack>();

            AckTimeout = ackTimeout;
            SkipHandshake = skipHandshake;

            Reset();
        }

        public event Action<DataPacket> PacketSent;
        private void SendPacket(DataPacket packet)
        {
            m_outstanding[packet.ID] = m_controller.Time;

            if (PacketSent != null)
                PacketSent(packet);
        }

        public void OnAckReceived(Ack ack)
        {
            m_receivedAck = ack;
        }

        public void Tick()
        {
            bool packetSent = false;
            bool stateChanged = false;

            if (m_receivedAck != null)
            {
                stateChanged = true; //show state whenever we receive an ack (even when nothing actually changes)

                m_previousAcks.Add(m_receivedAck);
                if (m_previousAcks.Count > 3)
                    m_previousAcks.RemoveAt(0);

                //remove everything that was just acked from m_outstanding
                uint[] ids = m_outstanding.Keys.ToArray();
                foreach (uint id in ids)
                    if (id < m_receivedAck.NextID)
                        m_outstanding.Remove(id);

                //handle fast retransmit
                if (m_previousAcks.Count == 3 &&
                    m_previousAcks[0].NextID == m_previousAcks[1].NextID &&
                    m_previousAcks[1].NextID == m_previousAcks[2].NextID)
                {
                    SendPacket(new DataPacket(m_controller.Time, m_receivedAck.NextID));
                    packetSent = true;
                }
            }

            uint oldestTime = uint.MaxValue,
                oldestID = 0;

            if (!packetSent)
            {
                //resend old packet?

                //TODO: replace this foreach loop with linq
                foreach (KeyValuePair<uint, uint> pair in m_outstanding)
                {
                    if (pair.Value < oldestTime)
                    {
                        oldestID = pair.Key;
                        oldestTime = pair.Value;
                    }
                }

                //detect timeout
                if (m_controller.Time >= oldestTime + AckTimeout)
                {
                    SendPacket(new DataPacket(m_controller.Time, oldestID));
                    packetSent = true;

                    stateChanged = true;
                }
            }

            if (!packetSent)
            {
                //send next packet?
                if ((m_previousAcks.Count == 0 && m_outstanding.Count == 0) ||
                    (m_previousAcks.Count > 0 && m_previousAcks[m_previousAcks.Count - 1].Window > m_outstanding.Count))
                {
                    uint flags = 0;
                    //handle the handshake
                    if (!SkipHandshake && m_nextID == START_ID)
                        flags = (uint)Packet.FLAGS.SYN;
                    
                    SendPacket(new DataPacket(m_controller.Time, m_nextID++, flags));
                    packetSent = true;

                    stateChanged = true;
                }
            }

            m_receivedAck = null;

            if (stateChanged)
                ChangeState();
        }

        public void Reset()
        {
            m_receivedAck = null;
            m_nextID = START_ID;
            m_previousAcks.Clear();
            m_outstanding.Clear();
        }
    }
}
