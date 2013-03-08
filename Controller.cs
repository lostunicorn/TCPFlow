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

        public uint Time { get; private set; }

        public void Reset()
        {
            Time = 0;
        }

        public Controller(uint delay, uint rxBufferSize, uint ackTimeout)
        {
            log = new Log(this);
            sender = new Sender(this, ackTimeout);
            receiver = new Receiver(this, rxBufferSize, ackTimeout);
            network = new Network(this, delay);

            sender.PacketSent += log.OnPacketSent;
            sender.PacketSent += network.Send;

            receiver.AckSent += log.OnAckSent;
            receiver.AckSent += network.Send;
            receiver.PacketDelivered += log.OnPacketDelivered;

            network.PacketArrived += receiver.OnPacketReceived;
            network.PacketLost += log.OnPacketLost;
            network.AckArrived += sender.OnAckReceived;
            network.AckLost += log.OnAckLost;
        }

        public void Tick()
        {
            network.Tick();
            receiver.Tick();
            sender.Tick();

            ++Time;
        }
    }
}
