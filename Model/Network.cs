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

        private Queue<DataPacket> m_packetsUnderway = new Queue<DataPacket>();
        private Queue<Ack> m_acksUnderway = new Queue<Ack>();

        private SortedSet<uint> m_packetsToDrop;
        private SortedSet<uint> m_acksToDrop;

        private Controller m_controller;

        public Network(Controller controller, uint delay)
        {
            m_controller = controller;

            Delay = delay;
            m_packetsToDrop = new SortedSet<uint>();
            m_acksToDrop = new SortedSet<uint>();
        }

        public void AddLostPacket(uint number)
        {
            m_packetsToDrop.Add(number);
        }

        public void RemoveLostPacket(uint number)
        {
            m_packetsToDrop.Remove(number);
        }

        public void AddLostAck(uint number)
        {
            m_acksToDrop.Add(number);
        }

        public void RemoveLostAck(uint number)
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

        public event Action<DataPacket> PacketLost;
        private void DropPacket(DataPacket packet)
        {
            packet.Lost = true;
            if (PacketLost != null)
                PacketLost(packet);
        }

        public event Action<Ack> AckLost;
        private void DropAck(Ack ack)
        {
            ack.Lost = true;
            if (AckLost != null)
                AckLost(ack);
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
                m_packetsUnderway.Peek().Time + Delay == m_controller.Time)
                PacketArrives(m_packetsUnderway.Dequeue());

            if (m_acksUnderway.Count > 0 &&
                m_acksUnderway.Peek().Time + Delay == m_controller.Time)
                AckArrives(m_acksUnderway.Dequeue());
        }
    }
}
