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
    public class Ack : Packet
    {
        public uint NextID { get; set; }

        public uint Window { get; set; }

        public uint Number { get; private set; }
        private static uint m_nextNumber = 0;

        public static void Reset()
        {
            m_nextNumber = 0;
        }

        public Ack(uint time, uint nextID, uint window, uint flags = 0) : base(time, flags)
        {
            NextID = nextID;
            Window = window;
            Number = m_nextNumber++;
        }
    }
}
