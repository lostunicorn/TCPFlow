/*
 * Copyright 2013 Jeroen De Wachter, Sofie Van Hoecke
 * 
 * This file is part of TCPFlow.
 * 
 * TCPFlow is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * TCPFlow is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with TCPFlow.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPFlow.Model
{
    public class Receiver
    {
        public class State
        {
            public readonly uint Time;

            public readonly uint[] Buffer;

            public readonly uint NextID;

            public readonly bool Timedout;

            public State(uint time, uint nextID, uint[] buffer, bool timedout)
            {
                Time = time;
                NextID = nextID;
                Buffer = buffer;
                Timedout = timedout;
            }
        }

        public event Action<State> StateChanged;
        protected void OnStateChanged(bool timedout)
        {
            if (StateChanged != null)
                StateChanged(new State(m_controller.Time, m_nextID, m_buffer.ToArray(), timedout));
        }

        private SortedList<uint, uint> m_sequenceNumbersToHold;
        private SortedList<uint, uint> m_sequenceNumbersHeld;

        private SortedSet<uint> m_buffer;

        private Controller m_controller;

        private DataPacket m_receivedPacket;

        private uint m_nextID;
        private uint m_lastPacketReceiveTime;
        private uint m_lastAckSendTime;

        private uint m_lastDeliveryTime;

        public uint BufferSize { get; set; }

        public uint Timeout { get; set; }

        public uint DeliveryInterval { get; set; }

        public uint MaxAckDelay { get; set; }

        public Receiver(Controller controller, uint bufferSize, uint deliveryInterval, uint maxAckDelay, uint timeout)
        {
            m_sequenceNumbersToHold = new SortedList<uint, uint>();
            m_sequenceNumbersHeld = new SortedList<uint, uint>();

            m_buffer = new SortedSet<uint>();

            m_controller = controller;

            BufferSize = bufferSize;
            Timeout = timeout;
            DeliveryInterval = deliveryInterval;
            MaxAckDelay = maxAckDelay;

            Reset();
        }

        public uint GetTicksToHold(uint id)
        {
            if (m_sequenceNumbersToHold.ContainsKey(id))
                return m_sequenceNumbersToHold[id];
            else
                return 0;
        }

        public void SetTicksToHold(uint id, uint ticks)
        {
            if (ticks == 0)
                m_sequenceNumbersToHold.Remove(id);
            else
                m_sequenceNumbersToHold[id] = ticks;
        }

        public void network_PacketArrived(DataPacket packet)
        {
            m_receivedPacket = packet;
        }

        public event Action<Ack> AckSent;
        private void SendAck(Ack ack)
        {
            m_lastAckSendTime = m_controller.Time;
            if (AckSent != null)
                AckSent(ack);
        }

        public class PacketDeliveryArgs : EventArgs
        {
            public readonly uint ID;
            public readonly bool Delivered;

            public PacketDeliveryArgs(uint id, bool delivered)
            {
                ID = id;
                Delivered = delivered;
            }
        }

        public event Action<PacketDeliveryArgs> PacketDelivered;
        private void DeliverPacket(uint ID, bool delivered)
        {
            if (delivered)
                m_lastDeliveryTime = m_controller.Time;

            if (PacketDelivered != null)
                PacketDelivered(new PacketDeliveryArgs(ID, delivered));
        }

        public void Reset()
        {
            m_sequenceNumbersHeld.Clear();
            m_buffer.Clear();
            m_lastAckSendTime = m_lastPacketReceiveTime = m_lastDeliveryTime = uint.MaxValue;
            m_receivedPacket = null;
            m_nextID = m_controller.SkipHandshake ? (uint)1 : 0;
        }

        public bool HoldPacket(uint id)
        {
            if (m_sequenceNumbersToHold.ContainsKey(id))
            {
                if (m_sequenceNumbersHeld.ContainsKey(id))
                {
                    if (m_sequenceNumbersHeld[id] < m_sequenceNumbersToHold[id])
                    {
                        ++m_sequenceNumbersHeld[id];
                        return true;
                    }
                }
                else
                {
                    m_sequenceNumbersHeld[id] = 1;
                    return true;
                }
            }

            return false;
        }

        public void Tick()
        {
            if (m_receivedPacket != null && (m_receivedPacket.Flags & (uint)Packet.FLAGS.SYN) == (uint)Packet.FLAGS.SYN)
            {
                SendAck(new Ack(m_controller.Time, m_receivedPacket.ID + 1, BufferSize, (uint)Packet.FLAGS.SYN));
                ++m_nextID;
                m_receivedPacket = null;

                OnStateChanged(false);

                return;
            }

            bool outoforder = m_receivedPacket != null &&
                m_receivedPacket.ID != (m_buffer.Count > 0 ? m_buffer.Max + 1 : m_nextID);

            bool stateChanged = false;

            if (m_receivedPacket != null)
            {
                if (m_receivedPacket.ID >= m_nextID) //avoid re-adding something to the buffer that was already delivered
                    m_buffer.Add(m_receivedPacket.ID);

                stateChanged = true;
            }

            if (m_buffer.Contains(m_nextID) &&
                (m_lastDeliveryTime == uint.MaxValue || m_controller.Time >= m_lastDeliveryTime + DeliveryInterval))
            {
                if (!HoldPacket(m_nextID))
                {
                    DeliverPacket(m_nextID, true);
                    m_buffer.Remove(m_nextID++);
                }
                else
                    DeliverPacket(m_nextID, false);
            }
            else if (m_buffer.Count >= BufferSize)//make sure there's room for the next ID
            {
                m_buffer.Remove(m_receivedPacket.ID);
                m_receivedPacket.Dropped = true;
            }

            /*
             * if a packet was added to the buffer while the buffer was already full
             * (and the packet to be delivered was not delivered), delete the
             * received packet from the buffer
             */
            if (m_buffer.Count > BufferSize)
            {
                m_buffer.Remove(m_receivedPacket.ID);
                m_receivedPacket.Dropped = true;
            }

            bool timeout = m_receivedPacket == null &&
                m_lastAckSendTime != uint.MaxValue &&
                m_controller.Time == m_lastAckSendTime + Timeout;

            bool delayedAckTimeout = m_lastPacketReceiveTime != uint.MaxValue &&
                m_controller.Time == m_lastPacketReceiveTime + MaxAckDelay &&
                (m_lastPacketReceiveTime > m_lastAckSendTime || m_lastAckSendTime == uint.MaxValue);

            bool ackWaiting = m_receivedPacket != null &&                                                   //received a packet
                m_lastPacketReceiveTime != uint.MaxValue &&                                                 //that was not the first packet (ever)
                (m_lastPacketReceiveTime > m_lastAckSendTime || m_lastAckSendTime == uint.MaxValue);        //we did not send an ack for the previous packet

            bool immediateAck = m_receivedPacket != null &&
                MaxAckDelay == 0;

            //send ack?
            if ( ackWaiting || outoforder || delayedAckTimeout || timeout || immediateAck)
            {
                stateChanged = true;

                uint id = m_nextID;
                uint windowSize = BufferSize;
                while (m_buffer.Contains(id))
                {
                    ++id;
                    --windowSize;
                }

                SendAck(new Ack(m_controller.Time, id, windowSize));
            }

            if (m_receivedPacket != null)
            {
                m_lastPacketReceiveTime = m_controller.Time;
                m_receivedPacket = null;
            }

            if (stateChanged)
                OnStateChanged(timeout);
        }
    }
}
