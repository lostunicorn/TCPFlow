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

        public Sender(Controller controller)
        {
            m_controller = controller;
        }

        public event Action<DataPacket> PacketSent;
        private void SendPacket(DataPacket packet)
        {
            if (PacketSent != null)
                PacketSent(null);
        }

        public void ReceiveAck(Ack ack)
        {
        }

        public void Tick()
        {
        }
    }
}
