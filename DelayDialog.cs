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
    public partial class DelayDialog : Form
    {
        public uint Delay
        {
            get
            {
                return (uint)numDelay.Value;
            }
            set
            {
                numDelay.Value = value;
            }
        }

        public DelayDialog()
        {
            InitializeComponent();
        }
    }
}
