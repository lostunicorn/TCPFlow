using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    public class Sender
    {
        private Controller m_controller;

        private SortedList<uint, uint> m_outstanding;

        private Ack m_receivedAck;
        private List<Ack> m_previousAcks;

        private uint m_nextID;

        public uint AckTimeout { get; set; }

        public bool SkipHandshake { get; set; }

        public const int START_ID = 1;

        public Sender(Controller controller, bool skipHandshake, uint ackTimeout)
        {
            m_controller = controller;

            m_outstanding = new SortedList<uint, uint>();

            m_previousAcks = new List<Ack>();

            m_nextID = START_ID;

            AckTimeout = ackTimeout;
            SkipHandshake = skipHandshake;
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

            if (m_receivedAck != null)
            {
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
                }
            }

            m_receivedAck = null;
        }

        public void Reset()
        {
            m_receivedAck = null;
            m_outstanding.Clear();
        }
    }
}
