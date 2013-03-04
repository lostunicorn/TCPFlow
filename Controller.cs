using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TCPFlow.Model;

namespace TCPFlow
{
    public class Controller
    {
        public Log log;
        public Sender sender;
        public Receiver receiver;
        public Network network;

        public Controller(uint delay)
        {
            log = new Log();
            sender = new Sender();
            receiver = new Receiver();
            network = new Network(delay);

            sender.PacketSent += log.OnPacketSent;
            sender.PacketSent += network.Send;

            receiver.AckSent += log.OnAckSent;
            receiver.AckSent += network.Send;
            receiver.PacketDelivered += log.OnPacketDelivered;

            network.PacketArrived += receiver.OnPacketReceived;
            network.PacketDropped += log.OnPacketDropped;
            network.AckArrived += sender.ReceiveAck;
            network.AckDropped += log.OnAckDropped;
        }

        public void Tick()
        {
            receiver.Tick();
            sender.Tick();
            network.Tick();
        }
    }
}
