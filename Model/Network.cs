using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    public class Network
    {
        public uint Delay { get; set; }

        public Network(uint delay)
        {
            Delay = delay;
            m_packetsToDrop = new SortedSet<uint>();
            m_acksToDrop = new SortedSet<uint>();
        }

        private Queue<DataPacket> m_packetsUnderway = new Queue<DataPacket>();
        private Queue<Ack> m_acksUnderway = new Queue<Ack>();

        private SortedSet<uint> m_packetsToDrop;
        private SortedSet<uint> m_acksToDrop;

        public void AddDroppedPacket(uint number)
        {
            m_packetsToDrop.Add(number);
        }

        public void RemoveDroppedPacket(uint number)
        {
            m_packetsToDrop.Remove(number);
        }

        public void AddDroppedAck(uint number)
        {
            m_acksToDrop.Add(number);
        }

        public void RemoveDroppedAck(uint number)
        {
            m_acksToDrop.Remove(number);
        }

        public event Action<DataPacket> PacketArrived;
        private void PacketArrives(DataPacket packet)
        {
            if (PacketArrived != null)
                PacketArrived(packet);
        }

        public void Send(DataPacket packet)
        {
            if (m_packetsToDrop.Contains(packet.Number))
                DropPacket(packet);
            else
                m_packetsUnderway.Enqueue(packet);
        }

        public event Action<DataPacket> PacketDropped;
        private void DropPacket(DataPacket packet)
        {
            packet.Dropped = true;
            if (PacketDropped != null)
                PacketDropped(packet);
        }

        public event Action<Ack> AckDropped;
        private void DropAck(Ack ack)
        {
            ack.Dropped = true;
            if (AckDropped != null)
                AckDropped(ack);
        }

        public event Action<Ack> AckArrived;
        private void AckArrives(Ack ack)
        {
            if (AckArrived != null)
                AckArrived(ack);
        }
        
        public void Send(Ack ack)
        {
            if (m_acksToDrop.Contains(ack.Number))
                DropAck(ack);
            else
                m_acksUnderway.Enqueue(ack);
        }

        public void Reset()
        {
            m_packetsUnderway.Clear();
            m_acksUnderway.Clear();
        }

        public void Tick()
        {
            if (m_packetsUnderway.Count > 0 &&
                m_packetsUnderway.Peek().Time + Delay == Timing.Time)
                PacketArrives(m_packetsUnderway.Dequeue());

            if (m_acksUnderway.Count > 0 &&
                m_packetsUnderway.Peek().Time + Delay == Timing.Time)
                AckArrives(m_acksUnderway.Dequeue());
        }
    }
}
