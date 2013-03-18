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

        private Tuple<uint, uint> m_steadyState;
        public Tuple<uint, uint> SteadyState
        {
            get
            {
                return m_steadyState;
            }
            set
            {
                if (m_steadyState != value)
                {
                    m_steadyState = value;
                    OnSteadyStateChanged();
                }
            }
        }

        public event Action<Tuple<uint, uint>> SteadyStateChanged;
        protected virtual void OnSteadyStateChanged()
        {
            if (SteadyStateChanged != null)
                SteadyStateChanged(SteadyState);
        }

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
            m_steadyState = new Tuple<uint, uint>(uint.MaxValue, uint.MaxValue);
        }

        private void AddToHistory(string element)
        {
            if (SteadyState.Item1 != uint.MaxValue) //steady state already detected?
                return;

            m_historyTiming.Add(m_history.Length, m_controller.Time);

            if (m_lastEventTime != uint.MaxValue &&
                m_controller.Time != m_lastEventTime)
            {
                m_history.Append(m_controller.Time - m_lastEventTime);

                string history = m_history.ToString();
                List<int> matchList = new List<int>();
                int pos = 0;
                while (pos != -1 && pos < history.Length - 1)
                {
                    pos = history.IndexOf(element, pos + 1);
                    if (pos != -1)
                        matchList.Add(pos);
                }

                int[] matchArr = matchList.ToArray();

                bool found = false;

                int j = matchArr.Length - 1;
                while (!found && j > 0)
                {
                    string secondSection = history.Substring(matchArr[j]);

                    int i = j - 1;
                    while (!found && i >= 0)
                    {
                        string firstSection = history.Substring(matchArr[i], matchArr[j] - matchArr[i]);

                        if (firstSection.Equals(secondSection) &&
                            firstSection.Contains('P') &&
                            firstSection.Contains('A') &&
                            firstSection.Contains('D'))
                        {
                            int firstPos = matchArr[i], secondPos = matchArr[j];
                            uint firstTime, secondTime;
                            while (!m_historyTiming.ContainsKey(firstPos))
                                --firstPos;
                            firstTime = m_historyTiming[firstPos];

                            while (!m_historyTiming.ContainsKey(secondPos))
                                --secondPos;
                            secondTime = m_historyTiming[secondPos];

                            //rejoice! steady state!
                            found = true;
                            SteadyState = new Tuple<uint, uint>(firstTime, secondTime);
                        }
                        --i;
                    }
                    --j;
                }
            }

            m_history.Append(element);
            m_lastEventTime = m_controller.Time;
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

        public void sender_PacketSent(DataPacket packet)
        {
            packets[m_controller.Time] = packet;

            AddToHistory("P");
        }

        public void network_PacketLost(DataPacket packet)
        {
            ClearHistory();
        }

        public void receiver_AckSent(Ack ack)
        {
            acks[m_controller.Time] = ack;

            AddToHistory("A");
        }

        public void network_AckLost(Ack ack)
        {
            ClearHistory();
        }

        public void receiver_PacketDelivered(Receiver.PacketDeliveryArgs args)
        {
            delivered[m_controller.Time] = args;

            AddToHistory("D");
        }

        public void sender_StateChanged(Sender.State state)
        {
            senderStates[m_controller.Time] = state;

            StringBuilder outstanding = new StringBuilder();
            foreach (uint id in state.Outstanding)
            {
                outstanding.Append(state.NextID - id);
                outstanding.Append('|');
            }

            //string str = string.Format(m_controller.sender.CongestionControlEnabled ? "CW{0}RW{1}O{2}{3}" : "RW{1}O{2}{3}", state.CongestionWindow, state.ReceiveWindow, outstanding.ToString(), state.Timedout);
            string str = string.Format("RW{0}O{1}{2}", state.ReceiveWindow, outstanding.ToString(), state.Timedout);
            AddToHistory(str);
        }

        public void receiver_StateChanged(Receiver.State state)
        {
            receiverStates[m_controller.Time] = state;

            StringBuilder buffer = new StringBuilder();
            foreach (uint id in state.Buffer)
            {
                buffer.Append(id - state.NextID);
                buffer.Append('|');
            }

            string str = String.Format("B{0}{1}", buffer.ToString(), state.Timedout);
            AddToHistory(str);
        }
    }
}
