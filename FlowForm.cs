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

        private const uint PIXELS_PER_TICK = 2;
        private const int BORDER = 6;
        private const int DELIVERY_BORDER = 15;
        private readonly float HEADER_HEIGHT;

        public FlowForm(Controller controller)
        {
            InitializeComponent();

            HEADER_HEIGHT = CalculateHeaderHeight();

            m_controller = controller;

            m_controller.log.Changed += DrawFlow;

            DrawFlow();
        }

        private float CalculateHeaderHeight()
        {
            Bitmap bitmap = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bitmap);

            Font font = new Font(FontFamily.GenericSerif, 12);
            return 4*BORDER + g.MeasureString("TX", font).Height;
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

        void DrawFlow()
        {
            uint endTime = m_controller.Time + 2 * m_controller.network.Delay;

            Bitmap bitmap = new Bitmap(pnlFlow.Width, Convert.ToInt32(endTime * PIXELS_PER_TICK + HEADER_HEIGHT));
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

            Pen blackPen = new Pen(Brushes.Black, 3),
                redPen = new Pen(Brushes.Red, 3),
                bluePen = new Pen(Brushes.Blue, 3);
            Font bigFont = new Font(FontFamily.GenericSerif, 12),
                smallFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);

            //Render the header
            SizeF txSize = g.MeasureString("TX", bigFont),
                rxSize = g.MeasureString("RX", bigFont);

            float txLine = 2 * BORDER + txSize.Width / 2,
                rxLine = bitmap.Width - (2 * BORDER + DELIVERY_BORDER + rxSize.Width / 2),
                txAngle = (float)(180 / Math.PI * Math.Atan2(m_controller.network.Delay * PIXELS_PER_TICK, rxLine - txLine)),
                rxAngle = -txAngle;

            g.DrawRectangle(blackPen, BORDER, BORDER, txSize.Width + 2 * BORDER, txSize.Height + 2 * BORDER);
            g.DrawString("TX", bigFont, Brushes.Black, 2*BORDER, 2 * BORDER);
            g.DrawLine(blackPen, txLine, 3 * BORDER + txSize.Height, txLine, bitmap.Height);

            g.DrawRectangle(blackPen, bitmap.Width - (3 * BORDER + DELIVERY_BORDER + rxSize.Width), BORDER, rxSize.Width + 2 * BORDER, rxSize.Height + 2 * BORDER);
            g.DrawString("RX", bigFont, Brushes.Black, bitmap.Width - (2 * BORDER + DELIVERY_BORDER + rxSize.Width), 2 * BORDER);
            g.DrawLine(blackPen, rxLine, 3 * BORDER + rxSize.Height, rxLine, bitmap.Height);

            //translate the graphics object so all future coördinates can be calculated in terms of simulated
            //time without taking the header into account
            g.TranslateTransform(0, HEADER_HEIGHT);

            //render packets, acks and such
            foreach (KeyValuePair<uint, Model.DataPacket> pair in m_controller.log.packets)
            {
                PointF from, to;
                from = new PointF(txLine, pair.Key*PIXELS_PER_TICK);
                to = new PointF();

                Model.DataPacket packet = pair.Value;

                if (packet.Lost)
                {
                    to.X = txLine + (rxLine - txLine)/3;
                    to.Y = from.Y + (m_controller.network.Delay * PIXELS_PER_TICK) / 3;
                }
                else {
                    to.X = rxLine;
                    to.Y = from.Y + m_controller.network.Delay * PIXELS_PER_TICK;
                }
                g.DrawLine(blackPen, from, to);

                if (packet.Lost)
                {
                    g.DrawLine(redPen, new PointF(to.X - 10, to.Y - 10), new PointF(to.X + 10, to.Y + 10));
                    g.DrawLine(redPen, new PointF(to.X - 10, to.Y + 10), new PointF(to.X + 10, to.Y - 10));
                }

                DrawRotatedString(g, smallFont, Brushes.Black, "Seq: " + packet.ID, from.X, from.Y, txAngle, false);
            }

            foreach (KeyValuePair<uint, Model.Ack> pair in m_controller.log.acks)
            {
                PointF from, to;
                from = new PointF(rxLine, pair.Key * PIXELS_PER_TICK);
                to = new PointF();

                Model.Ack ack = pair.Value;

                if (ack.Lost)
                {
                    to.X = txLine + (rxLine - txLine) * 2 / 3;
                    to.Y = from.Y + (m_controller.network.Delay * PIXELS_PER_TICK) / 3;
                }
                else
                {
                    to.X = txLine;
                    to.Y = from.Y + m_controller.network.Delay * PIXELS_PER_TICK;
                }
                g.DrawLine(blackPen, from, to);

                if (ack.Lost)
                {
                    g.DrawLine(redPen, new PointF(to.X - 10, to.Y - 10), new PointF(to.X + 10, to.Y + 10));
                    g.DrawLine(redPen, new PointF(to.X - 10, to.Y + 10), new PointF(to.X + 10, to.Y - 10));
                }
                DrawRotatedString(g, smallFont, Brushes.Black, "Ack: " + ack.NextID, from.X, from.Y, rxAngle, true);
            }

            foreach (KeyValuePair<uint, Model.DataPacket> pair in m_controller.log.delivered)
            {
                Model.DataPacket packet = pair.Value;

                PointF from = new PointF(rxLine, pair.Key * PIXELS_PER_TICK),
                    to = new PointF(from.X + DELIVERY_BORDER, from.Y - DELIVERY_BORDER);
                g.DrawLine(bluePen, from, to);
                g.DrawLine(bluePen, to, new PointF(to.X - 10, to.Y + 2));
                g.DrawLine(bluePen, to, new PointF(to.X - 2, to.Y + 10));

                from.X += 15;
                DrawRotatedString(g, smallFont, Brushes.Blue, packet.ID.ToString(), from.X, from.Y, 0, false);
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
            DrawFlow();
        }
    }
}
