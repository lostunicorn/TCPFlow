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
    public partial class FlowForm : Form
    {
        private Controller m_controller;

        public FlowForm(Controller controller)
        {
            InitializeComponent();

            m_controller = controller;
        }
    }
}
