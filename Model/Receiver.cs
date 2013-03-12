using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    public class Receiver
    {
        public class State
        {
            public readonly uint Time;

            public readonly uint[] Buffer;

            public State(uint time, uint[] buffer)
            {
                Time = time;
                Buffer = buffer;
            }
        }
        public event Action<State> StateChanged;
        protected void ChangeState()
        {
            if (StateChanged != null)
                StateChanged(new State(m_controller.Time, m_buffer.ToArray()));
        }

        private SortedList<uint, uint> m_sequenceNumbersToHold;
        private SortedList<uint, uint> m_sequenceNumbersHeld;

        private SortedSet<uint> m_buffer;

        private Controller m_controller;

        private DataPacket m_receivedPacket;

        private uint m_nextID;
        private uint m_ackSendTime;

        public uint BufferSize { get; set; }

        public uint AckTimeout { get; set; }

        public const int START_ID = 1;

        public Receiver(Controller controller, uint bufferSize, uint ackTimeout)
        {
            m_sequenceNumbersToHold = new SortedList<uint, uint>();
            m_sequenceNumbersHeld = new SortedList<uint, uint>();

            m_buffer = new SortedSet<uint>();

            m_controller = controller;

            BufferSize = bufferSize;
            AckTimeout = ackTimeout;

            Reset();
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

        public class PacketDeliveryArgs : EventArgs
        {
            public readonly uint ID;
            public readonly bool Delivered;

            public PacketDeliveryArgs(uint id, bool delivered)
            {
                ID = id;
                Delivered = delivered;
            }
        }
        public event Action<PacketDeliveryArgs> PacketDelivered;
        private void DeliverPacket(uint ID, bool delivered)
        {
            if (PacketDelivered != null)
                PacketDelivered(new PacketDeliveryArgs(ID, delivered));
        }

        public void Reset()
        {
            m_sequenceNumbersHeld.Clear();
            m_buffer.Clear();
            m_ackSendTime = uint.MaxValue;
            m_receivedPacket = null;
            m_nextID = START_ID;
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
            if (m_receivedPacket != null && (m_receivedPacket.Flags & (uint)Packet.FLAGS.SYN) == (uint)Packet.FLAGS.SYN)
            {
                SendAck(new Ack(m_controller.Time, m_receivedPacket.ID + 1, BufferSize, (uint)Packet.FLAGS.SYN));
                ++m_nextID;
                m_receivedPacket = null;

                ChangeState();

                return;
            }

            bool stateChanged = false;

            if (m_receivedPacket != null)
            {
                m_buffer.Add(m_receivedPacket.ID);

                stateChanged = true;
            }

            if (m_buffer.Contains(m_nextID))
            {
                if (!HoldPacket(m_nextID))
                {
                    DeliverPacket(m_nextID, true);
                    m_buffer.Remove(m_nextID++);
                }
                else
                    DeliverPacket(m_nextID, false);
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

            if (stateChanged)
                ChangeState();
        }
    }
}
