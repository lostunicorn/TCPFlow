/*
 * Copyright 2013 Jeroen De Wachter
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
    public class Network
    {
        public uint Delay { get; set; }

        private SortedList<uint, DataPacket> m_packetsUnderway = new SortedList<uint, DataPacket>();
        private SortedList<uint, Ack> m_acksUnderway = new SortedList<uint, Ack>();

        private SortedSet<uint> m_packetsToDrop = new SortedSet<uint>();
        private SortedSet<uint> m_acksToDrop = new SortedSet<uint>();

        private SortedList<uint, uint> m_customDelayPackets = new SortedList<uint,uint>();
        private SortedList<uint, uint> m_customDelayAcks = new SortedList<uint,uint>();

        private Controller m_controller;

        public Network(Controller controller, uint delay)
        {
            m_controller = controller;

            Delay = delay;
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

        public uint GetPacketDelay(uint number)
        {
            if (m_customDelayPackets.ContainsKey(number))
                return m_customDelayPackets[number];
            else
                return Delay;
        }

        public void SetCustomPacketDelay(uint number, uint delay)
        {
            if (delay == Delay)
                m_customDelayPackets.Remove(number);
            else
                m_customDelayPackets[number] = delay;
        }

        public void RemoveCustomPacketDelay(uint number)
        {
            m_customDelayPackets.Remove(number);
        }

        public uint GetAckDelay(uint number)
        {
            if (m_customDelayAcks.ContainsKey(number))
                return m_customDelayAcks[number];
            else
                return Delay;
        }

        public void SetCustomAckDelay(uint number, uint delay)
        {
            if (delay == Delay)
                m_customDelayAcks.Remove(number);
            else
                m_customDelayAcks[number] = delay;
        }

        public void RemoveCustomAckDelay(uint number)
        {
            m_customDelayAcks.Remove(number);
        }

        public event Action<DataPacket> PacketArrived;
        private void PacketArrives(DataPacket packet)
        {
            if (PacketArrived != null)
                PacketArrived(packet);
        }

        public void Send(DataPacket packet)
        {
            uint delay = Delay;
            if (m_customDelayPackets.ContainsKey(packet.Number))
                delay = m_customDelayPackets[packet.Number];

            if (m_packetsToDrop.Contains(packet.Number))
                DropPacket(packet);
            else
                m_packetsUnderway[m_controller.Time + delay] = packet;
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
            uint delay = Delay;
            if (m_customDelayAcks.ContainsKey(ack.Number))
                delay = m_customDelayAcks[ack.Number];

            if (m_acksToDrop.Contains(ack.Number))
                DropAck(ack);
            else
                m_acksUnderway[m_controller.Time + delay] = ack;
        }

        public void Reset()
        {
            m_packetsUnderway.Clear();
            m_acksUnderway.Clear();
        }

        public void Tick()
        {
            if (m_packetsUnderway.ContainsKey(m_controller.Time))
                PacketArrives(m_packetsUnderway[m_controller.Time]);

            if (m_acksUnderway.ContainsKey(m_controller.Time))
                AckArrives(m_acksUnderway[m_controller.Time]);
        }
    }
}
