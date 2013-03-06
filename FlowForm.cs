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

        private const uint PIXELS_PER_TICK = 5;
        private const int BORDER = 6;

        public FlowForm(Controller controller)
        {
            InitializeComponent();

            m_controller = controller;

            m_controller.log.Changed += log_Changed;

            log_Changed();
        }

        void log_Changed()
        {
            int headerHeight = 4*BORDER+12;

            Bitmap bitmap = new Bitmap(pbFlow.Width, Convert.ToInt32(m_controller.Time * PIXELS_PER_TICK + headerHeight));
            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

            Pen thinPen = new Pen(Brushes.Black);
            Font font = new Font(FontFamily.GenericSerif, 12);

            SizeF txSize = g.MeasureString("TX", font),
                rxSize = g.MeasureString("RX", font);

            float txLine = 2 * BORDER + txSize.Width / 2,
                rxLine = bitmap.Width - (2 * BORDER + rxSize.Width / 2);

            g.DrawRectangle(thinPen, BORDER, BORDER, txSize.Width + 2 * BORDER, txSize.Height + 2 * BORDER);
            g.DrawString("TX", font, Brushes.Black, 2*BORDER, 2 * BORDER);
            g.DrawLine(thinPen, txLine, 3 * BORDER + txSize.Height, txLine, bitmap.Height);

            g.DrawRectangle(thinPen, bitmap.Width - (3 * BORDER + rxSize.Width), BORDER, rxSize.Width + 2 * BORDER, rxSize.Height + 2 * BORDER);
            g.DrawString("RX", font, Brushes.Black, bitmap.Width - (2 * BORDER + rxSize.Width), 2 * BORDER);
            g.DrawLine(thinPen, rxLine, 3 * BORDER + rxSize.Height, rxLine, bitmap.Height);

            g.TranslateTransform(0, headerHeight);

            foreach (KeyValuePair<uint, Model.DataPacket> pair in m_controller.log.packets)
            {
                g.DrawLine(thinPen, txLine, pair.Key*PIXELS_PER_TICK, rxLine, (pair.Key + m_controller.network.Delay) * PIXELS_PER_TICK);
            }

            foreach (KeyValuePair<uint, Model.Ack> pair in m_controller.log.acks)
            {
                g.DrawLine(thinPen, rxLine, pair.Key * PIXELS_PER_TICK, txLine, (pair.Key + m_controller.network.Delay) * PIXELS_PER_TICK);
            }

            foreach (KeyValuePair<uint, Model.DataPacket> pair in m_controller.log.delivered)
            {
            }

            g.Dispose();
            pbFlow.Image = bitmap;
        }

        private void numDelay_ValueChanged(object sender, EventArgs e)
        {
            m_controller.network.Delay = Convert.ToUInt32(numDelay.Value);
        }

        private void btnTick_Click(object sender, EventArgs e)
        {
            m_controller.Tick();
        }
    }
}
