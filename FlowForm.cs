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

        private Timer m_tickTimer;

        private uint m_selectedTime = uint.MaxValue;
        private uint m_contextTime;

        private const uint PIXELS_PER_TICK = 20;
        private const int BORDER = 6;
        private const int DELIVERY_BORDER = 15;

        private const int TICK_INTERVAL = 500;

        private float m_headerHeight;

        Pen m_blackPen = new Pen(Brushes.Black, 3),
            m_redPen = new Pen(Brushes.Red, 3),
            m_bluePen = new Pen(Brushes.Blue, 3),
            m_txPen,
            m_rxPen,
            m_thinPen = new Pen(Brushes.Black, 1),
            m_lightGreenPen = new Pen(Brushes.LightGreen, 3);

        Font m_bigFont = new Font(FontFamily.GenericSerif, 12),
            m_smallFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold),
            m_timelineFont = new Font(FontFamily.GenericSerif, 8);

        SizeF m_txSize,
            m_rxSize,
            m_numberSize,
            m_windowStateSize;

        float m_txLine,
            m_rxLine;

        LinearGradientBrush m_txBrush,
            m_rxBrush;

        SolidBrush m_selectionBrush;

        DelayDialog m_delayDialog = new DelayDialog();

        int m_flowWidth;

        public FlowForm(Controller controller)
        {
            InitializeComponent();

            m_controller = controller;
            m_controller.Ticked += DrawFlow;

            m_tickTimer = new Timer();
            m_tickTimer.Interval = TICK_INTERVAL;
            m_tickTimer.Tick += tickTimer_Tick;

            pnlFlow.VerticalScroll.Visible = true;
            numRXBufferSize.Value = m_controller.receiver.BufferSize;
            numRXBufferSize.ValueChanged += numRXBufferSize_ValueChanged;

            numRXTimeout.Value = m_controller.receiver.Timeout;
            numRXTimeout.ValueChanged += numRXTimeout_ValueChanged;

            numDeliveryInterval.Value = m_controller.receiver.DeliveryInterval;
            numDeliveryInterval.ValueChanged += numDeliveryInterval_ValueChanged;

            chkCongestionControl.Checked = m_controller.sender.CongestionControlEnabled;
            chkCongestionControl.CheckedChanged += chkCongestionControl_CheckedChanged;

            numTXTimeout.Value = m_controller.sender.Timeout;
            numTXTimeout.ValueChanged += numTXTimeout_ValueChanged;

            InitStaticGraphics();
            InitDynamicGraphics();

            DrawFlow();
        }

        void numDeliveryInterval_ValueChanged(object sender, EventArgs e)
        {
            m_controller.receiver.DeliveryInterval = (uint)numDeliveryInterval.Value;
            Replay();
        }

        void chkCongestionControl_CheckedChanged(object sender, EventArgs e)
        {
            m_controller.sender.CongestionControlEnabled = chkCongestionControl.Checked;
            InitDynamicGraphics();
            Replay();
        }

        void numTXTimeout_ValueChanged(object sender, EventArgs e)
        {
            m_controller.sender.Timeout = (uint)numTXTimeout.Value;
            Replay();
        }

        void numRXTimeout_ValueChanged(object sender, EventArgs e)
        {
            m_controller.receiver.Timeout = (uint)numRXTimeout.Value;
            Replay();
        }

        void tickTimer_Tick(object sender, EventArgs e)
        {
            m_controller.Tick();
        }

        private void InitStaticGraphics()
        {
            Bitmap bitmap = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bitmap);

            m_headerHeight = 6 * BORDER + g.MeasureString("TX", m_bigFont).Height;

            m_txSize = g.MeasureString("TX", m_bigFont);
            m_rxSize = g.MeasureString("RX", m_bigFont);

            m_numberSize = g.MeasureString("00", m_smallFont);

            m_selectionBrush = new SolidBrush(Color.FromArgb(128, Color.DarkGreen));

            g.Dispose();
        }

        private void InitDynamicGraphics()
        {
            Bitmap bitmap = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bitmap);

            m_flowWidth = pnlFlow.Width - SystemInformation.VerticalScrollBarWidth;

            float txBufferWidth = Convert.ToSingle(m_numberSize.Width * m_controller.receiver.BufferSize);
            float rxBufferWidth = Convert.ToSingle(m_numberSize.Width * m_controller.receiver.BufferSize);

            if (m_controller.sender.CongestionControlEnabled)
                m_windowStateSize = g.MeasureString(string.Format("RW: {0} CW: {1:f2}", 99, 99), m_smallFont);
            else
                m_windowStateSize = g.MeasureString(string.Format("RW: {0}", 99), m_smallFont);

            m_txLine = m_windowStateSize.Width + txBufferWidth + 2 * BORDER + m_txSize.Width / 2;
            m_txBrush = new LinearGradientBrush(new PointF(m_txLine, 0), new PointF(m_rxLine, 0), Color.Black, Color.White);
            m_txPen = new Pen(m_txBrush, 3);

            m_rxLine = m_flowWidth - (rxBufferWidth + 2 * BORDER + DELIVERY_BORDER + m_rxSize.Width / 2);
            m_rxBrush = new LinearGradientBrush(new PointF(m_txLine, 0), new PointF(m_rxLine, 0), Color.White, Color.Black);
            m_rxPen = new Pen(m_rxBrush, 3);

            g.Dispose();
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
            g.DrawRectangle(m_blackPen, m_txLine - (m_txSize.Width / 2 + BORDER), BORDER, m_txSize.Width + 2 * BORDER, m_txSize.Height + 2 * BORDER);
            g.DrawString("TX", m_bigFont, Brushes.Black, m_txLine - m_txSize.Width / 2, 2 * BORDER);
            g.DrawLine(m_blackPen, m_txLine, 3 * BORDER + m_txSize.Height, m_txLine, bitmap.Height);

            g.DrawRectangle(m_blackPen, m_rxLine - (m_rxSize.Width / 2 + BORDER), BORDER, m_rxSize.Width + 2 * BORDER, m_rxSize.Height + 2 * BORDER);
            g.DrawString("RX", m_bigFont, Brushes.Black, m_rxLine - m_rxSize.Width / 2, 2 * BORDER);
            g.DrawLine(m_blackPen, m_rxLine, 3 * BORDER + m_rxSize.Height, m_rxLine, bitmap.Height);

            //translate the graphics object so all future coördinates can be calculated in terms of simulated
            //time without taking the header into account
            g.TranslateTransform(0, m_headerHeight);

            //render the timelines
            uint time = 0;
            while (time * PIXELS_PER_TICK + m_headerHeight < bitmap.Height)
            {
                int length = time % 10 == 0 ? 10 : 5;

                SizeF size = g.MeasureString(time.ToString(), m_timelineFont);

                PointF from = new PointF(m_txLine, time * PIXELS_PER_TICK),
                    to = new PointF(m_txLine - length, time * PIXELS_PER_TICK);
                g.DrawLine(m_thinPen, from, to);
                g.DrawString(time.ToString(), m_timelineFont, Brushes.Black, new PointF(from.X - size.Width - 10, from.Y - size.Height / 2));

                from = new PointF(m_rxLine, time * PIXELS_PER_TICK);
                to = new PointF(m_rxLine + length, time * PIXELS_PER_TICK);
                g.DrawLine(m_thinPen, from, to);
                g.DrawString(time.ToString(), m_timelineFont, Brushes.Black, new PointF(from.X + 10, from.Y - size.Height / 2));

                time += 5;
            }

            if (m_controller.Time != uint.MaxValue)
            {
                for (time = 0; time <= m_controller.Time; ++time)
                {
                    //render packet
                    if (m_controller.log.packets.ContainsKey(time))
                    {
                        Model.DataPacket packet = m_controller.log.packets[time];
                        uint delay = m_controller.network.GetPacketDelay(packet.Number);
                        float txAngle = (float)(180 / Math.PI * Math.Atan2(delay * PIXELS_PER_TICK, m_rxLine - m_txLine));

                        PointF from, to;
                        from = new PointF(m_txLine, time * PIXELS_PER_TICK);
                        to = new PointF();

                        if (packet.Lost &&
                            m_controller.Time > packet.Time + delay / 3)
                        {
                            to.X = m_txLine + (m_rxLine - m_txLine) / 3;
                            to.Y = from.Y + (delay * PIXELS_PER_TICK) / 3;

                            //g.DrawLine(blackPen, from, to);
                            g.DrawLine(m_txPen, from, to);
                            g.DrawLine(m_redPen, new PointF(to.X - 10, to.Y - 10), new PointF(to.X + 10, to.Y + 10));
                            g.DrawLine(m_redPen, new PointF(to.X - 10, to.Y + 10), new PointF(to.X + 10, to.Y - 10));
                        }
                        else
                        {
                            float r = 1;

                            if (m_controller.Time < packet.Time + delay)
                                r = Convert.ToSingle((m_controller.Time - packet.Time) * 1.0 / delay);

                            to.Y = from.Y + delay * PIXELS_PER_TICK * r;
                            to.X = m_txLine + (m_rxLine - m_txLine) * r;

                            //g.DrawLine(blackPen, from, to);
                            g.DrawLine(m_txPen, from, to);
                        }

                        string desc;
                        if (packet.Flags == 0)
                            desc = string.Format("Seq: {0}", packet.ID);
                        else
                            desc = string.Format("Seq: {0} Flags: {1}", packet.ID, GetFlagString(packet.Flags));
                        DrawRotatedString(g, m_smallFont, Brushes.Black, desc, from.X, from.Y, txAngle, false);
                    }

                    //render ack
                    if (m_controller.log.acks.ContainsKey(time))
                    {
                        Model.Ack ack = m_controller.log.acks[time];
                        uint delay = m_controller.network.GetAckDelay(ack.Number);
                        float rxAngle = -(float)(180 / Math.PI * Math.Atan2(delay * PIXELS_PER_TICK, m_rxLine - m_txLine));

                        PointF from, to;
                        from = new PointF(m_rxLine, time * PIXELS_PER_TICK);
                        to = new PointF();

                        if (ack.Lost &&
                            m_controller.Time > ack.Time + delay / 3)
                        {
                            to.X = m_rxLine - (m_rxLine - m_txLine) / 3;
                            to.Y = from.Y + (delay * PIXELS_PER_TICK) / 3;

                            //g.DrawLine(blackPen, from, to);
                            g.DrawLine(m_rxPen, from, to);
                            g.DrawLine(m_redPen, new PointF(to.X - 10, to.Y - 10), new PointF(to.X + 10, to.Y + 10));
                            g.DrawLine(m_redPen, new PointF(to.X - 10, to.Y + 10), new PointF(to.X + 10, to.Y - 10));
                        }
                        else
                        {
                            float r = 1;

                            if (m_controller.Time < ack.Time + delay)
                                r = Convert.ToSingle((m_controller.Time - ack.Time) * 1.0 / delay);

                            to.X = m_rxLine - (m_rxLine - m_txLine) * r;
                            to.Y = from.Y + delay * PIXELS_PER_TICK * r;

                            //g.DrawLine(blackPen, from, to);
                            g.DrawLine(m_rxPen, from, to);
                        }

                        string desc;
                        if (ack.Flags == 0)
                            desc = string.Format("Ack: {0} Window: {1}", ack.NextID, ack.Window);
                        else
                            desc = string.Format("Ack: {0} Window: {1} Flags: {2}", ack.NextID, ack.Window, GetFlagString(ack.Flags));
                        DrawRotatedString(g, m_smallFont, Brushes.Black, desc, from.X, from.Y, rxAngle, true);
                    }

                    //render delivery
                    if (m_controller.log.delivered.ContainsKey(time))
                    {
                        Model.Receiver.PacketDeliveryArgs args = m_controller.log.delivered[time];

                        PointF to = new PointF(bitmap.Width - m_numberSize.Width, time * PIXELS_PER_TICK),
                            from = new PointF(to.X - DELIVERY_BORDER, to.Y);
                        g.DrawLine(m_bluePen, from, to);

                        if (args.Delivered)
                        {
                            g.DrawLine(m_bluePen, to, new PointF(to.X - 7, to.Y - 5));
                            g.DrawLine(m_bluePen, to, new PointF(to.X - 7, to.Y + 5));
                        }
                        else
                        {
                            g.DrawLine(m_redPen, new PointF(to.X - 10, to.Y - 10), new PointF(to.X + 10, to.Y + 10));
                            g.DrawLine(m_redPen, new PointF(to.X - 10, to.Y + 10), new PointF(to.X + 10, to.Y - 10));
                        }

                        DrawRotatedString(g, m_smallFont, Brushes.Blue, args.ID.ToString(), to.X, to.Y + m_numberSize.Height/2, 0, false);
                    }

                    //render sender state
                    if (m_controller.log.senderStates.ContainsKey(time))
                    {
                        Model.Sender.State state = m_controller.log.senderStates[time];

                        Point p = new Point(0, (int)(time * PIXELS_PER_TICK - m_numberSize.Height / 2.0));

                        string str;
                        if (m_controller.sender.CongestionControlEnabled)
                            str = String.Format("RW: {0} CW: {1:f2}", state.ReceiveWindow, state.CongestionWindow);
                        else
                            str = String.Format("RW: {0}", state.ReceiveWindow);

                        g.DrawString(str, m_smallFont, Brushes.Black, p);
                        p.X += (int)m_windowStateSize.Width;

                        int i = 0;
                        while (i < state.Outstanding.Length)
                        {
                            Rectangle rect = new Rectangle(p, new Size((int)m_numberSize.Width, (int)m_numberSize.Height));
                            g.FillRectangle(Brushes.Red, rect);
                            g.DrawRectangle(m_thinPen, rect);
                            g.DrawString(state.Outstanding[i].ToString(), m_smallFont, Brushes.White, p);
                            ++i;
                            p.X += (int)m_numberSize.Width;
                        }

                        uint nextID = state.NextID;
                        /*
                        if (state.Outstanding.Length > 0)
                            nextID = state.Outstanding[state.Outstanding.Length - 1] + 1;
                        */

                        while (i < m_controller.receiver.BufferSize)
                        {
                            Rectangle rect = new Rectangle(p, new Size((int)m_numberSize.Width, (int)m_numberSize.Height));
                            g.FillRectangle(Brushes.White, rect);
                            g.DrawRectangle(m_thinPen, rect);
                            g.DrawString(nextID.ToString(), m_smallFont, Brushes.Black, p);
                            ++nextID;
                            ++i;
                            p.X += (int)m_numberSize.Width;
                        }

                        if (state.Timedout)
                        {
                            PointF to = new PointF(m_txLine - m_lightGreenPen.Width, time * PIXELS_PER_TICK),
                                from = to;
                            from.Y -= m_controller.sender.Timeout * PIXELS_PER_TICK;

                            g.DrawLine(m_lightGreenPen, from, to);
                            g.DrawLine(m_lightGreenPen, to, new PointF(to.X - 5, to.Y - 7));
                            g.DrawLine(m_lightGreenPen, to, new PointF(to.X + 5, to.Y - 7));
                        }
                    }

                    //render receiver state
                    if (m_controller.log.receiverStates.ContainsKey(time))
                    {
                        Model.Receiver.State state = m_controller.log.receiverStates[time];

                        int i = 0, bufIndex = 0;
                        uint nextID = state.NextID;

                        while (i < m_controller.receiver.BufferSize)
                        {
                            Point p = new Point((int)(m_rxLine + BORDER + i * m_numberSize.Width), (int)(time * PIXELS_PER_TICK - m_numberSize.Height / 2.0));
                            Rectangle rect = new Rectangle(p, new Size((int)m_numberSize.Width, (int)m_numberSize.Height));
                            Brush brush;
                            string str = nextID.ToString();

                            if (state.Buffer.Length == 0 || //buffer is empty
                                nextID > state.Buffer[state.Buffer.Length - 1]) //nextID is larger than the largest element in buffer
                            {
                                brush = Brushes.White;
                                str = "";
                            }
                            else if (state.Buffer.Length > bufIndex && state.Buffer[bufIndex] == nextID) //is nextID in buffer?
                            {
                                brush = Brushes.Green;
                                ++bufIndex;
                            }
                            else
                            {
                                brush = Brushes.Red;
                            }

                            g.FillRectangle(brush, rect);
                            g.DrawRectangle(m_thinPen, rect);
                            g.DrawString(nextID.ToString(), m_smallFont, Brushes.White, p);

                            ++nextID;
                            ++i;
                        }

                        if (state.Timedout)
                        {
                            PointF to = new PointF(m_rxLine + m_lightGreenPen.Width, time * PIXELS_PER_TICK),
                                from = to;
                            from.Y -= m_controller.receiver.Timeout * PIXELS_PER_TICK;

                            g.DrawLine(m_lightGreenPen, from, to);
                            g.DrawLine(m_lightGreenPen, to, new PointF(to.X - 5, to.Y - 7));
                            g.DrawLine(m_lightGreenPen, to, new PointF(to.X + 5, to.Y - 7));
                        }
                    }
                }
            }

            g.Dispose();
            pbFlow.Image = bitmap;
        }

        private void numDelay_ValueChanged(object sender, EventArgs e)
        {
            m_controller.network.Delay = Convert.ToUInt32(numDelay.Value);

            Replay();
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

        private void pbFlow_MouseMove(object sender, MouseEventArgs e)
        {
            uint m_oldSelectedTime = m_selectedTime;

            int time = Convert.ToInt32((e.Location.Y - m_headerHeight) / PIXELS_PER_TICK);
            if (time < 0 || time > m_controller.Time)
                m_selectedTime = uint.MaxValue;
            else
                m_selectedTime = (uint)time;

            if (m_selectedTime != m_oldSelectedTime)
                pbFlow.Invalidate(); //redraw if necessary
        }

        private void ctxStrip_Opening(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Debug.Print("ctxStrip_Opening");
            //modify menu based on m_selectedTime

            m_contextTime = m_selectedTime;

            if (m_controller.log.packets.ContainsKey(m_contextTime))
            {
                mnuLoseDataPacket.Enabled = mnuPacketDelay.Enabled = true;
                mnuLoseDataPacket.Checked = m_controller.log.packets[m_contextTime].Lost;
            }
            else
            {
                mnuLoseDataPacket.Enabled = mnuPacketDelay.Enabled = false;
                mnuLoseDataPacket.Checked = false;
            }

            if (m_controller.log.acks.ContainsKey(m_contextTime))
            {
                mnuLoseAck.Enabled = mnuAckDelay.Enabled = true;
                mnuLoseAck.Checked = m_controller.log.acks[m_contextTime].Lost;
            }
            else
            {
                mnuLoseAck.Enabled = mnuAckDelay.Enabled = false;
                mnuLoseAck.Checked = false;
            }

            if (m_controller.log.delivered.ContainsKey(m_contextTime))
            {
                mnuDelayDelivery.Enabled = true;
                mnuDelayDelivery.Checked = !m_controller.log.delivered[m_contextTime].Delivered;
            }
            else
            {
                mnuDelayDelivery.Enabled = false;
                mnuDelayDelivery.Checked = false;
            }
        }

        private void chkSkipHandshake_CheckedChanged(object sender, EventArgs e)
        {
            m_controller.SkipHandshake = chkSkipHandshake.Checked;
            Replay();
        }

        private void FlowForm_Shown(object sender, EventArgs e)
        {
            InitDynamicGraphics();
        }

        private void rdRunAutomatic_CheckedChanged(object sender, EventArgs e)
        {
            btnTick.Enabled = rdRunManual.Checked;

            if (rdRunAutomatic.Checked)
                m_tickTimer.Start();
            else
                m_tickTimer.Stop();
        }

        private void pbFlow_MouseLeave(object sender, EventArgs e)
        {
            if (m_selectedTime != uint.MaxValue)
            {
                m_selectedTime = uint.MaxValue;
                pbFlow.Invalidate();
            }
        }

        private void pbFlow_Paint(object sender, PaintEventArgs e)
        {
            if (m_selectedTime != uint.MaxValue)
                e.Graphics.FillRectangle(m_selectionBrush, 0, Convert.ToSingle(m_headerHeight + (m_selectedTime -0.5) * PIXELS_PER_TICK), pbFlow.Width, PIXELS_PER_TICK);
        }

        private void mnuLoseDataPacket_Click(object sender, EventArgs e)
        {
            if (mnuLoseDataPacket.Checked) //--> going to unchecked
                m_controller.network.RemoveLostPacket(m_controller.log.packets[m_contextTime].Number);
            else
                m_controller.network.AddLostPacket(m_controller.log.packets[m_contextTime].Number);

            Replay();
        }

        private void mnuLoseAck_Click(object sender, EventArgs e)
        {
            if (mnuLoseAck.Checked) //--> going to unchecked
                m_controller.network.RemoveLostAck(m_controller.log.acks[m_contextTime].Number);
            else
                m_controller.network.AddLostAck(m_controller.log.acks[m_contextTime].Number);

            Replay();
        }

        private void mnuDelayDelivery_Click(object sender, EventArgs e)
        {
            if (mnuDelayDelivery.Checked) //--> going to unchecked
                m_controller.receiver.RemoveSequenceNumberToHold(m_controller.log.delivered[m_contextTime].ID);
            else
                m_controller.receiver.AddSequenceNumberToHold(m_controller.log.delivered[m_contextTime].ID);

            Replay();
        }

        private void Replay()
        {
            m_controller.Ticked -= DrawFlow; //make sure we don't redraw intermediate states
            m_controller.Replay();
            m_controller.Ticked += DrawFlow; //make sure we do redraw following states
            DrawFlow();
        }

        private void numRXBufferSize_ValueChanged(object sender, EventArgs e)
        {
            m_controller.receiver.BufferSize = Convert.ToUInt32(numRXBufferSize.Value);
            InitDynamicGraphics();
            Replay();
        }

        private void mnuPacketDelay_Click(object sender, EventArgs e)
        {
            Model.DataPacket packet = m_controller.log.packets[m_contextTime];
            uint oldDelay = m_controller.network.GetPacketDelay(packet.Number);
            m_delayDialog.Delay = oldDelay;
            if (m_delayDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK &&
                m_delayDialog.Delay != oldDelay)
            {
                m_controller.network.SetCustomPacketDelay(packet.Number, m_delayDialog.Delay);
                Replay();
            }
        }

        private void mnuAckDelay_Click(object sender, EventArgs e)
        {
            Model.Ack ack = m_controller.log.acks[m_contextTime];
            uint oldDelay = m_controller.network.GetAckDelay(ack.Number);
            m_delayDialog.Delay = oldDelay;
            if (m_delayDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK &&
                m_delayDialog.Delay != oldDelay)
            {
                m_controller.network.SetCustomAckDelay(ack.Number, m_delayDialog.Delay);
                Replay();
            }
        }
    }
}
