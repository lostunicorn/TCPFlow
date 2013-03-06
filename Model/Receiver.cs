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

        private Controller m_controller;

        public Receiver(Controller controller)
        {
            m_controller = controller;

            m_sequenceNumbersToHold = new SortedList<uint, uint>();
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
            //TODO: add business logic
        }
    }
}
