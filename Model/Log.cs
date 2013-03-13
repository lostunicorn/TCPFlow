﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    public class Log
    {
        /*
         * Lists of packets maintained for rendering
         */
        public Dictionary<uint, DataPacket> packets = new Dictionary<uint, DataPacket>();
        public Dictionary<uint, Ack> acks = new Dictionary<uint, Ack>();
        public Dictionary<uint, Receiver.PacketDeliveryArgs> delivered = new Dictionary<uint, Receiver.PacketDeliveryArgs>();

        public Dictionary<uint, Sender.State> senderStates = new Dictionary<uint, Sender.State>();
        public Dictionary<uint, Receiver.State> receiverStates = new Dictionary<uint, Receiver.State>();

        private StringBuilder m_history;
        private Dictionary<int, uint> m_historyTiming; //map index in the m_history string to a time
        private uint m_lastEventTime;

        private Controller m_controller;

        public uint SteadyStateStart { get; private set; }
        public uint SteadyStateStop { get; private set; }

        public Log(Controller controller)
        {
            m_controller = controller;

            m_history = new StringBuilder();
            m_historyTiming = new Dictionary<int, uint>();

            m_lastEventTime = uint.MaxValue;
        }

        public void ClearHistory()
        {
            m_historyTiming.Clear();
            m_history.Clear();
            m_lastEventTime = uint.MaxValue;
        }

        private void AddToHistory(string str)
        {
            m_historyTiming.Add(m_history.Length, m_controller.Time);

            if (m_lastEventTime != uint.MaxValue)
                m_history.Append(m_controller.Time - m_lastEventTime);

            m_history.Append(str);
            m_lastEventTime = m_controller.Time;

            //TODO: add steady state detection
            string history = m_history.ToString();
            int l = 1, len = history.Length, middle = len/2;
            bool found = false;
            while (!found && l < middle)
            {
                string substr = history.Substring(len - l);
                if (substr.Contains('P') &&
                    substr.Contains('A') &&
                    substr.Contains('D') &&
                    history.Substring(len-2*l, l).Equals(substr))
                {
                    SteadyStateStart = m_historyTiming[len - 2 * l];
                    SteadyStateStop = m_historyTiming[len - l];
                    //steady state detected!
                }
                ++l;
            }
        }

        public void Reset()
        {
            ClearHistory();

            packets.Clear();
            acks.Clear();
            delivered.Clear();

            senderStates.Clear();
            receiverStates.Clear();
        }

        public void OnPacketSent(DataPacket packet)
        {
            packets[m_controller.Time] = packet;

            AddToHistory("P");
        }

        public void OnPacketLost(DataPacket packet)
        {
            ClearHistory();
        }

        public void OnAckSent(Ack ack)
        {
            acks[m_controller.Time] = ack;

            AddToHistory("A");
        }

        public void OnAckLost(Ack ack)
        {
            ClearHistory();
        }

        public void OnPacketDelivered(Receiver.PacketDeliveryArgs args)
        {
            delivered[m_controller.Time] = args;

            AddToHistory("D");
        }

        public void OnSenderStateChanged(Sender.State state)
        {
            senderStates[m_controller.Time] = state;
        }

        public void OnReceiverStateChanged(Receiver.State state)
        {
            receiverStates[m_controller.Time] = state;
        }
    }
}
