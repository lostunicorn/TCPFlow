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

        public bool SkipHandshake { get; set; }

        public void Reset()
        {
            Time = uint.MaxValue;

            sender.Reset();
            receiver.Reset();
            network.Reset();
            log.Reset();
            DataPacket.Reset();
            Ack.Reset();
        }

        public Controller(uint delay, uint rxBufferSize, uint timeout)
        {
            SkipHandshake = true;

            receiver = new Receiver(this, rxBufferSize, 3, timeout);
            log = new Log(this);
            sender = new Sender(this, true, timeout); //sender relies on receiver, so needs to be created after receiver!
            network = new Network(this, delay);

            sender.PacketSent += log.sender_PacketSent;
            sender.PacketSent += network.Send;
            sender.StateChanged += log.sender_StateChanged;

            receiver.AckSent += log.receiver_AckSent;
            receiver.AckSent += network.Send;
            receiver.PacketDelivered += log.receiver_PacketDelivered;
            receiver.StateChanged += log.receiver_StateChanged;

            network.PacketArrived += receiver.network_PacketArrived;
            network.PacketLost += log.network_PacketLost;
            network.AckArrived += sender.receiver_AckArrived;
            network.AckLost += log.network_AckLost;

            Reset();
        }

        public event Action Ticked;
        protected void OnTicked()
        {
            if (Ticked != null)
                Ticked();
        }

        public void Replay()
        {
            uint until = Time;
            Reset();
            TickUntil(until);
        }

        public void TickUntil(uint time)
        {
            if (time == uint.MaxValue)
                return;

            while (Time < time || Time == uint.MaxValue)
                Tick();
        }

        public void Tick()
        {
            ++Time;

            network.Tick();
            receiver.Tick();
            sender.Tick();

            OnTicked();
        }
    }
}
