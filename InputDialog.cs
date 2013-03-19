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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPFlow
{
    public partial class InputDialog : Form
    {
        public uint Value
        {
            get
            {
                return (uint)numValue.Value;
            }
            set
            {
                numValue.Value = value;
            }
        }

        public string ValueText
        {
            get
            {
                return lblValue.Text;
            }
            set
            {
                lblValue.Text = value;
            }
        }

        public uint Minimum
        {
            get
            {
                return (uint)numValue.Minimum;
            }
            set
            {
                numValue.Minimum = value;
            }
        }

        public uint Maximum
        {
            get
            {
                return (uint)numValue.Maximum;
            }
            set
            {
                numValue.Maximum = value;
            }
        }

        public InputDialog()
        {
            InitializeComponent();
        }
    }
}
