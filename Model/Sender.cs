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
    public class Sender
    {
        public class State
        {
            public readonly uint Time;

            public readonly uint[] Outstanding;

            public readonly float CongestionWindow;

            public readonly uint ReceiveWindow;

            public readonly uint NextID;

            public readonly bool Timedout;

            public State(uint time, uint nextID, uint[] outstanding, uint receiveWindow, float congestionWindow, bool timedout)
            {
                Time = time;
                NextID = nextID;
                Outstanding = outstanding;
                ReceiveWindow = receiveWindow;
                CongestionWindow = congestionWindow;
                Timedout = timedout;
            }
        }

        public event Action<State> StateChanged;
        protected void OnStateChanged(bool timedout)
        {
            if (StateChanged != null)
            {
                StateChanged(new State(m_controller.Time, m_nextID, m_outstanding.Keys.ToArray(), ReceiveWindow, CongestionWindow, timedout));
            }
        }

        private Controller m_controller;

        private SortedList<uint, uint> m_outstanding;

        private Ack m_receivedAck;
        private List<Ack> m_previousAcks;

        private uint m_nextID;

        public uint Timeout { get; set; }

        public bool CongestionControlEnabled { get; set; }

        public uint ReceiveWindow { get; private set; }

        public float CongestionWindow { get; private set; }

        public float SlowStartThreshold { get; private set; }

        public const uint INITIAL_SLOW_START_THRESHOLD = 64;

        public Sender(Controller controller, bool congestionControlEnabled, uint timeout)
        {
            m_controller = controller;

            m_outstanding = new SortedList<uint, uint>();

            m_previousAcks = new List<Ack>();

            Timeout = timeout;
            CongestionControlEnabled = congestionControlEnabled;

            Reset();
        }

        public event Action<DataPacket> PacketSent;
        private void SendPacket(DataPacket packet)
        {
            m_outstanding[packet.ID] = m_controller.Time;

            if (PacketSent != null)
                PacketSent(packet);
        }

        public void receiver_AckArrived(Ack ack)
        {
            m_receivedAck = ack;
        }

        public void Tick()
        {
            bool packetSent = false;
            bool stateChanged = false;

            if (m_receivedAck != null)
            {
                stateChanged = true; //show state whenever we receive an ack (even when nothing actually changes)

                m_previousAcks.Add(m_receivedAck);
                if (m_previousAcks.Count > 3)
                    m_previousAcks.RemoveAt(0);

                ReceiveWindow = m_receivedAck.Window;

                //remove everything that was just acked from m_outstanding
                bool progress = false;
                uint[] ids = m_outstanding.Keys.ToArray();
                foreach (uint id in ids)
                {
                    if (id < m_receivedAck.NextID)
                    {
                        progress = true;
                        m_outstanding.Remove(id);
                    }
                }

                if (progress)
                {
                    if (CongestionWindow < SlowStartThreshold)
                        ++CongestionWindow;
                    else
                        CongestionWindow += 1 / CongestionWindow;
                }

                //handle fast retransmit
                if (m_previousAcks.Count == 3 &&
                    m_previousAcks[0].NextID == m_previousAcks[1].NextID &&
                    m_previousAcks[1].NextID == m_previousAcks[2].NextID)
                {
                    m_previousAcks.Clear();

                    SendPacket(new DataPacket(m_controller.Time, m_receivedAck.NextID));
                    packetSent = true;
                    CongestionWindow = SlowStartThreshold = CongestionWindow / 2;
                }
            }

            uint oldestTime = uint.MaxValue,
                oldestID = 0;
            bool timedout = false;

            if (!packetSent && m_outstanding.Count > 0)
            {
                //resend old packet?

                //TODO: replace this foreach loop with linq
                foreach (KeyValuePair<uint, uint> pair in m_outstanding)
                {
                    if (pair.Value < oldestTime)
                    {
                        oldestID = pair.Key;
                        oldestTime = pair.Value;
                    }
                }

                //detect timeout
                if (m_controller.Time >= oldestTime + Timeout)
                {
                    SendPacket(new DataPacket(m_controller.Time, oldestID));
                    packetSent = true;

                    timedout = true;
                    stateChanged = true;

                    SlowStartThreshold = CongestionWindow / 2;
                    CongestionWindow = 1;
                }
            }

            if (!packetSent)
            {
                //send next packet?

                uint sendWindow = ReceiveWindow;
                if (CongestionControlEnabled)
                    sendWindow = (uint)Math.Min(ReceiveWindow, CongestionWindow);
                if (m_outstanding.Count < sendWindow)
                {
                    uint flags = 0;
                    //handle the handshake
                    if (!m_controller.SkipHandshake && m_nextID == 0)
                        flags = (uint)Packet.FLAGS.SYN;
                    
                    SendPacket(new DataPacket(m_controller.Time, m_nextID++, flags));
                    packetSent = true;

                    stateChanged = true;
                }
            }

            m_receivedAck = null;

            if (stateChanged)
                OnStateChanged(timedout);
        }

        public void Reset()
        {
            m_receivedAck = null;
            m_nextID = m_controller.SkipHandshake ? (uint)1 : 0;
            m_previousAcks.Clear();
            m_outstanding.Clear();
            ReceiveWindow = m_controller.SkipHandshake ? m_controller.receiver.BufferSize : 1;
            CongestionWindow = 1;
            SlowStartThreshold = INITIAL_SLOW_START_THRESHOLD;
        }
    }
}
