using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPFlow
{
    public partial class FlowForm : Form
    {
        private Controller m_controller;

        private Point m_clickLocation;
        private uint m_selectedTime;

        private const uint PIXELS_PER_TICK = 20;
        private const int BORDER = 6;
        private const int DELIVERY_BORDER = 15;

        private float m_headerHeight;

        Pen m_blackPen = new Pen(Brushes.Black, 3),
            m_redPen = new Pen(Brushes.Red, 3),
            m_bluePen = new Pen(Brushes.Blue, 3);

        Font m_bigFont = new Font(FontFamily.GenericSerif, 12),
            m_smallFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

        SizeF m_txSize,
            m_rxSize;

        float m_txLine,
            m_rxLine;

        float m_txAngle,
            m_rxAngle;

        LinearGradientBrush m_txBrush,
            m_rxBrush;

        Pen m_txPen,
            m_rxPen;

        int m_flowWidth;

        public FlowForm(Controller controller)
        {
            InitializeComponent();

            pnlFlow.VerticalScroll.Visible = true;

            m_controller = controller;
            m_controller.Ticked += DrawFlow;

            InitStaticGraphics();
            InitDynamicGraphics();

            DrawFlow();
        }

        private void InitStaticGraphics()
        {
            Bitmap bitmap = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bitmap);

            m_headerHeight = 4 * BORDER + g.MeasureString("TX", m_bigFont).Height;

            m_txSize = g.MeasureString("TX", m_bigFont);
            m_rxSize = g.MeasureString("RX", m_bigFont);
        }

        private void InitDynamicGraphics()
        {
            m_flowWidth = pnlFlow.Width - SystemInformation.VerticalScrollBarWidth;

            m_txLine = 2 * BORDER + m_txSize.Width / 2;
            m_txAngle = (float)(180 / Math.PI * Math.Atan2(m_controller.network.Delay * PIXELS_PER_TICK, m_rxLine - m_txLine));
            m_txBrush = new LinearGradientBrush(new PointF(m_txLine, 0), new PointF(m_rxLine, 0), Color.Black, Color.White);
            m_txPen = new Pen(m_txBrush, 3);

            m_rxLine = m_flowWidth - (2 * BORDER + DELIVERY_BORDER + m_rxSize.Width / 2);
            m_rxAngle = -m_txAngle;
            m_rxBrush = new LinearGradientBrush(new PointF(m_txLine, 0), new PointF(m_rxLine, 0), Color.White, Color.Black);
            m_rxPen = new Pen(m_rxBrush, 3);
        }

        void DrawRotatedString(Graphics g, Font font, Brush brush, string s, float x, float y, float angle, bool fromTheRight)
        {
            System.Drawing.Drawing2D.GraphicsState state = g.Save();
            g.TranslateTransform(x, y);
            g.RotateTransform(angle);

            SizeF size = g.MeasureString(s, font);

            PointF pos = new PointF();
            pos.Y -= size.Height;

            if (fromTheRight)
                pos.X = -size.Width;

            g.DrawString(s, font, brush, pos);
            g.Restore(state);
        }

        static string GetFlagString(uint flags)
        {
            Type flagType = typeof(Model.Packet.FLAGS);

            StringBuilder builder = new StringBuilder();
            bool first = true;
            Array vals = Enum.GetValues(flagType);
            foreach (int val in vals)
            {
                if ((flags & val) == val)
                {
                    if (!first)
                        builder.Append("|");
                    builder.Append(Enum.GetName(flagType, val));
                }
            }

            return builder.ToString();
        }

        void DrawFlow()
        {
            uint endTime = m_controller.Time + 2 * m_controller.network.Delay;

            Bitmap bitmap = new Bitmap(m_flowWidth, Convert.ToInt32(endTime * PIXELS_PER_TICK + m_headerHeight));
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

            //Render the header
            g.DrawRectangle(m_blackPen, BORDER, BORDER, m_txSize.Width + 2 * BORDER, m_txSize.Height + 2 * BORDER);
            g.DrawString("TX", m_bigFont, Brushes.Black, 2*BORDER, 2 * BORDER);
            g.DrawLine(m_blackPen, m_txLine, 3 * BORDER + m_txSize.Height, m_txLine, bitmap.Height);

            g.DrawRectangle(m_blackPen, bitmap.Width - (3 * BORDER + DELIVERY_BORDER + m_rxSize.Width), BORDER, m_rxSize.Width + 2 * BORDER, m_rxSize.Height + 2 * BORDER);
            g.DrawString("RX", m_bigFont, Brushes.Black, bitmap.Width - (2 * BORDER + DELIVERY_BORDER + m_rxSize.Width), 2 * BORDER);
            g.DrawLine(m_blackPen, m_rxLine, 3 * BORDER + m_rxSize.Height, m_rxLine, bitmap.Height);

            //translate the graphics object so all future coördinates can be calculated in terms of simulated
            //time without taking the header into account
            g.TranslateTransform(0, m_headerHeight);

            for (uint time = 0; time < m_controller.Time; ++time)
            {
                //render packets, acks and such
                if (m_controller.log.packets.ContainsKey(time))
                {
                    Model.DataPacket packet = m_controller.log.packets[time];

                    PointF from, to;
                    from = new PointF(m_txLine, time * PIXELS_PER_TICK);
                    to = new PointF();

                    if (packet.Lost &&
                        m_controller.Time > packet.Time + m_controller.network.Delay / 3)
                    {
                        to.X = m_txLine + (m_rxLine - m_txLine) / 3;
                        to.Y = from.Y + (m_controller.network.Delay * PIXELS_PER_TICK) / 3;

                        //g.DrawLine(blackPen, from, to);
                        g.DrawLine(m_txPen, from, to);
                        g.DrawLine(m_redPen, new PointF(to.X - 10, to.Y - 10), new PointF(to.X + 10, to.Y + 10));
                        g.DrawLine(m_redPen, new PointF(to.X - 10, to.Y + 10), new PointF(to.X + 10, to.Y - 10));
                    }
                    else
                    {
                        float r = 1;

                        if (m_controller.Time < packet.Time + m_controller.network.Delay)
                            r = Convert.ToSingle((m_controller.Time - packet.Time) * 1.0 / m_controller.network.Delay);

                        to.Y = from.Y + m_controller.network.Delay * PIXELS_PER_TICK * r;
                        to.X = m_txLine + (m_rxLine - m_txLine) * r;

                        //g.DrawLine(blackPen, from, to);
                        g.DrawLine(m_txPen, from, to);
                    }

                    string desc;
                    if (packet.Flags == 0)
                        desc = string.Format("Seq: {0}", packet.ID);
                    else
                        desc = string.Format("Seq: {0} Flags: {1}", packet.ID, GetFlagString(packet.Flags));
                    DrawRotatedString(g, m_smallFont, Brushes.Black, desc, from.X, from.Y, m_txAngle, false);
                }

                if (m_controller.log.acks.ContainsKey(time))
                {
                    Model.Ack ack = m_controller.log.acks[time];

                    PointF from, to;
                    from = new PointF(m_rxLine, time * PIXELS_PER_TICK);
                    to = new PointF();

                    if (ack.Lost &&
                        m_controller.Time > ack.Time + m_controller.network.Delay / 3)
                    {
                        to.X = m_rxLine - (m_rxLine - m_txLine) / 3;
                        to.Y = from.Y + (m_controller.network.Delay * PIXELS_PER_TICK) / 3;

                        //g.DrawLine(blackPen, from, to);
                        g.DrawLine(m_rxPen, from, to);
                        g.DrawLine(m_redPen, new PointF(to.X - 10, to.Y - 10), new PointF(to.X + 10, to.Y + 10));
                        g.DrawLine(m_redPen, new PointF(to.X - 10, to.Y + 10), new PointF(to.X + 10, to.Y - 10));
                    }
                    else
                    {
                        float r = 1;

                        if (m_controller.Time < ack.Time + m_controller.network.Delay)
                            r = Convert.ToSingle((m_controller.Time - ack.Time) * 1.0 / m_controller.network.Delay);

                        to.X = m_rxLine - (m_rxLine - m_txLine) * r;
                        to.Y = from.Y + m_controller.network.Delay * PIXELS_PER_TICK * r;

                        //g.DrawLine(blackPen, from, to);
                        g.DrawLine(m_rxPen, from, to);
                    }

                    string desc;
                    if (ack.Flags == 0)
                        desc = string.Format("Ack: {0} Window: {1}", ack.NextID, ack.Window);
                    else
                        desc = string.Format("Ack: {0} Window: {1} Flags: {2}", ack.NextID, ack.Window, GetFlagString(ack.Flags));
                    DrawRotatedString(g, m_smallFont, Brushes.Black, desc, from.X, from.Y, m_rxAngle, true);
                }

                if (m_controller.log.delivered.ContainsKey(time))
                {
                    uint ID = m_controller.log.delivered[time];

                    PointF from = new PointF(m_rxLine, time * PIXELS_PER_TICK),
                        to = new PointF(from.X + DELIVERY_BORDER, from.Y - DELIVERY_BORDER);
                    g.DrawLine(m_bluePen, from, to);
                    g.DrawLine(m_bluePen, to, new PointF(to.X - 10, to.Y + 2));
                    g.DrawLine(m_bluePen, to, new PointF(to.X - 2, to.Y + 10));

                    from.X += 15;
                    DrawRotatedString(g, m_smallFont, Brushes.Blue, ID.ToString(), from.X, from.Y, 0, false);
                }
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

        private void pnlFlow_SizeChanged(object sender, EventArgs e)
        {
            InitDynamicGraphics();
            DrawFlow();
        }

        private void ctxStrip_Opened(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Print("ctxStrip_Opened");
        }

        private void pbFlow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //System.Diagnostics.Debug.Print("pbFlow_MouseDown");
                m_clickLocation = e.Location;
            }
        }

        private void pbFlow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //System.Diagnostics.Debug.Print("pbFlow_MouseMove");
                m_clickLocation = e.Location;
            }
        }

        private void pbFlow_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //System.Diagnostics.Debug.Print("pbFlow_MouseUp");
            }
        }

        private void ctxStrip_Opening(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Debug.Print("ctxStrip_Opening");
            //modify menu based on m_clickLocation
        }

        private void chkSkipHandshake_CheckedChanged(object sender, EventArgs e)
        {
            m_controller.sender.SkipHandshake = chkSkipHandshake.Checked;
        }

        private void FlowForm_Shown(object sender, EventArgs e)
        {
            InitDynamicGraphics();
        }

        private void pbFlow_MouseClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.Print("pbFlow_MouseClick");
        }

        private void pbFlow_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Print("pbFlow_Click");
        }
    }
}
