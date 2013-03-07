using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    public class Receiver
    {
        private SortedList<uint, uint> m_sequenceNumbersToHold;
        private SortedList<uint, uint> m_sequenceNumbersHeld;

        private SortedSet<uint> m_buffer;

        private Controller m_controller;

        private DataPacket m_receivedPacket;

        private uint m_nextID;
        private uint m_ackSendTime;

        public uint BufferSize { get; set; }

        public uint AckTimeout { get; set; }

        public Receiver(Controller controller, uint bufferSize, uint ackTimeout)
        {
            m_sequenceNumbersToHold = new SortedList<uint, uint>();
            m_sequenceNumbersHeld = new SortedList<uint, uint>();

            m_buffer = new SortedSet<uint>();

            m_controller = controller;

            m_nextID = 0;
            m_ackSendTime = uint.MaxValue;

            BufferSize = bufferSize;
            AckTimeout = ackTimeout;
        }

        public void AddSequenceNumberToHold(uint number)
        {
            if (m_sequenceNumbersToHold.ContainsKey(number))
                ++m_sequenceNumbersToHold[number];
            else
                m_sequenceNumbersToHold.Add(number, 1);
        }

        public void RemoveSequenceNumberToHold(uint number)
        {
            if (m_sequenceNumbersToHold.ContainsKey(number))
            {
                if (m_sequenceNumbersToHold[number] > 1)
                    --m_sequenceNumbersToHold[number];
                else
                    m_sequenceNumbersToHold.Remove(number);
            }
        }

        public void OnPacketReceived(DataPacket packet)
        {
            m_receivedPacket = packet;
        }

        public event Action<Ack> AckSent;
        private void SendAck(Ack ack)
        {
            if (AckSent != null)
                AckSent(ack);
        }

        public event Action<uint> PacketDelivered;
        private void DeliverPacket(uint ID)
        {
            if (PacketDelivered != null)
                PacketDelivered(ID);
        }

        public void Reset()
        {
            m_sequenceNumbersHeld.Clear();
            m_buffer.Clear();
        }

        public bool HoldPacket(uint id)
        {
            if (m_sequenceNumbersToHold.ContainsKey(id))
            {
                if (m_sequenceNumbersHeld.ContainsKey(id))
                {
                    if (m_sequenceNumbersHeld[id] < m_sequenceNumbersToHold[id])
                    {
                        ++m_sequenceNumbersHeld[id];
                        return true;
                    }
                }
                else
                {
                    m_sequenceNumbersHeld[id] = 1;
                    return true;
                }
            }

            return false;
        }

        public void Tick()
        {
            if (m_receivedPacket != null)
            {
                m_buffer.Add(m_receivedPacket.ID);
            }

            if (m_buffer.Contains(m_nextID))
            {
                if (!HoldPacket(m_nextID))
                {
                    DeliverPacket(m_nextID);
                    m_buffer.Remove(m_nextID);
                    ++m_nextID;
                }
            }
            else if (m_buffer.Count >= BufferSize)//make sure there's room for the next ID
            {
                m_buffer.Remove(m_receivedPacket.ID);
                m_receivedPacket.Dropped = true;
            }

            /*
             * if a packet was added to the buffer while the buffer was already full
             * (and the packet to be delivered was not delivered), delete the
             * received packet from the buffer
             */
            if (m_buffer.Count > BufferSize)
            {
                m_buffer.Remove(m_receivedPacket.ID);
                m_receivedPacket.Dropped = true;
            }

            //send ack?
            if (m_receivedPacket != null || m_controller.Time >= m_ackSendTime + AckTimeout)
            {
                uint id = m_nextID;
                while (m_buffer.Contains(id))
                    ++id;
                SendAck(new Ack(m_controller.Time, id, (uint)(BufferSize - m_buffer.Count)));
                m_ackSendTime = m_controller.Time;
            }

            m_receivedPacket = null;
        }
    }
}
