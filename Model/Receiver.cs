using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    public class Receiver
    {
        private List<int> m_sequenceNumbersToHold;

        public Receiver()
        {
            m_sequenceNumbersToHold = new List<int>();
        }

        public void AddSequenceNumberToHold(int number)
        {
            m_sequenceNumbersToHold.Add(number);
        }

        public void RemoveSequenceNumberToHold(int number)
        {
            m_sequenceNumbersToHold.Remove(number);
        }

        public void OnPacketReceived(DataPacket packet)
        {
        }

        public event Action<Ack> AckSent;
        private void SendAck(Ack ack)
        {
            if (AckSent != null)
                AckSent(ack);
        }

        public event Action<DataPacket> PacketDelivered;
        private void DeliverPacket(DataPacket packet)
        {
            if (PacketDelivered != null)
                PacketDelivered(packet);
        }

        public void Tick()
        {

        }
    }
}
