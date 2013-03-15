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

            sender.PacketSent += log.OnPacketSent;
            sender.PacketSent += network.Send;
            sender.StateChanged += log.OnSenderStateChanged;

            receiver.AckSent += log.OnAckSent;
            receiver.AckSent += network.Send;
            receiver.PacketDelivered += log.OnPacketDelivered;
            receiver.StateChanged += log.OnReceiverStateChanged;

            network.PacketArrived += receiver.OnPacketReceived;
            network.PacketLost += log.OnPacketLost;
            network.AckArrived += sender.OnAckReceived;
            network.AckLost += log.OnAckLost;

            Reset();
        }

        public event Action Ticked;
        protected void OnTick()
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

            OnTick();
        }
    }
}
