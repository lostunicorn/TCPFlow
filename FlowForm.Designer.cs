namespace TCPFlow
{
    partial class FlowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbFlow = new System.Windows.Forms.PictureBox();
            this.ctxStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuLoseDataPacket = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLoseAck = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelayDelivery = new System.Windows.Forms.ToolStripMenuItem();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            this.lblDelay = new System.Windows.Forms.Label();
            this.grpRunSettings = new System.Windows.Forms.GroupBox();
            this.btnTick = new System.Windows.Forms.Button();
            this.rdRunManual = new System.Windows.Forms.RadioButton();
            this.rdRunAutomatic = new System.Windows.Forms.RadioButton();
            this.chkSkipHandshake = new System.Windows.Forms.CheckBox();
            this.pnlFlow = new System.Windows.Forms.Panel();
            this.grpNetworkSettings = new System.Windows.Forms.GroupBox();
            this.grpSenderSettings = new System.Windows.Forms.GroupBox();
            this.grpReceiverSettings = new System.Windows.Forms.GroupBox();
            this.numRXBufferSize = new System.Windows.Forms.NumericUpDown();
            this.m_lblRXBufferSize = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbFlow)).BeginInit();
            this.ctxStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            this.grpRunSettings.SuspendLayout();
            this.pnlFlow.SuspendLayout();
            this.grpNetworkSettings.SuspendLayout();
            this.grpReceiverSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRXBufferSize)).BeginInit();
            this.SuspendLayout();
            // 
            // pbFlow
            // 
            this.pbFlow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbFlow.ContextMenuStrip = this.ctxStrip;
            this.pbFlow.Location = new System.Drawing.Point(0, 0);
            this.pbFlow.Name = "pbFlow";
            this.pbFlow.Size = new System.Drawing.Size(322, 348);
            this.pbFlow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbFlow.TabIndex = 0;
            this.pbFlow.TabStop = false;
            this.pbFlow.Paint += new System.Windows.Forms.PaintEventHandler(this.pbFlow_Paint);
            this.pbFlow.MouseLeave += new System.EventHandler(this.pbFlow_MouseLeave);
            this.pbFlow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbFlow_MouseMove);
            // 
            // ctxStrip
            // 
            this.ctxStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLoseDataPacket,
            this.mnuLoseAck,
            this.mnuDelayDelivery});
            this.ctxStrip.Name = "ctxStrip";
            this.ctxStrip.ShowCheckMargin = true;
            this.ctxStrip.ShowImageMargin = false;
            this.ctxStrip.Size = new System.Drawing.Size(222, 76);
            this.ctxStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ctxStrip_Opening);
            // 
            // mnuLoseDataPacket
            // 
            this.mnuLoseDataPacket.Name = "mnuLoseDataPacket";
            this.mnuLoseDataPacket.Size = new System.Drawing.Size(221, 24);
            this.mnuLoseDataPacket.Text = "Lose Data Packet";
            this.mnuLoseDataPacket.Click += new System.EventHandler(this.mnuLoseDataPacket_Click);
            // 
            // mnuLoseAck
            // 
            this.mnuLoseAck.Name = "mnuLoseAck";
            this.mnuLoseAck.Size = new System.Drawing.Size(221, 24);
            this.mnuLoseAck.Text = "Lose Ack";
            this.mnuLoseAck.Click += new System.EventHandler(this.mnuLoseAck_Click);
            // 
            // mnuDelayDelivery
            // 
            this.mnuDelayDelivery.Name = "mnuDelayDelivery";
            this.mnuDelayDelivery.Size = new System.Drawing.Size(221, 24);
            this.mnuDelayDelivery.Text = "Delay Packet Delivery";
            this.mnuDelayDelivery.Click += new System.EventHandler(this.mnuDelayDelivery_Click);
            // 
            // numDelay
            // 
            this.numDelay.Location = new System.Drawing.Point(71, 21);
            this.numDelay.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(68, 22);
            this.numDelay.TabIndex = 0;
            this.numDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numDelay.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numDelay.ValueChanged += new System.EventHandler(this.numDelay_ValueChanged);
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(17, 23);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(48, 17);
            this.lblDelay.TabIndex = 2;
            this.lblDelay.Text = "Delay:";
            // 
            // grpRunSettings
            // 
            this.grpRunSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRunSettings.Controls.Add(this.btnTick);
            this.grpRunSettings.Controls.Add(this.rdRunManual);
            this.grpRunSettings.Controls.Add(this.rdRunAutomatic);
            this.grpRunSettings.Location = new System.Drawing.Point(787, 12);
            this.grpRunSettings.Name = "grpRunSettings";
            this.grpRunSettings.Size = new System.Drawing.Size(276, 58);
            this.grpRunSettings.TabIndex = 3;
            this.grpRunSettings.TabStop = false;
            this.grpRunSettings.Text = "Run";
            // 
            // btnTick
            // 
            this.btnTick.Location = new System.Drawing.Point(185, 19);
            this.btnTick.Name = "btnTick";
            this.btnTick.Size = new System.Drawing.Size(75, 23);
            this.btnTick.TabIndex = 2;
            this.btnTick.Text = "Tick";
            this.btnTick.UseVisualStyleBackColor = true;
            this.btnTick.Click += new System.EventHandler(this.btnTick_Click);
            // 
            // rdRunManual
            // 
            this.rdRunManual.AutoSize = true;
            this.rdRunManual.Checked = true;
            this.rdRunManual.Location = new System.Drawing.Point(104, 21);
            this.rdRunManual.Name = "rdRunManual";
            this.rdRunManual.Size = new System.Drawing.Size(75, 21);
            this.rdRunManual.TabIndex = 1;
            this.rdRunManual.TabStop = true;
            this.rdRunManual.Text = "Manual";
            this.rdRunManual.UseVisualStyleBackColor = true;
            // 
            // rdRunAutomatic
            // 
            this.rdRunAutomatic.AutoSize = true;
            this.rdRunAutomatic.Location = new System.Drawing.Point(7, 22);
            this.rdRunAutomatic.Name = "rdRunAutomatic";
            this.rdRunAutomatic.Size = new System.Drawing.Size(91, 21);
            this.rdRunAutomatic.TabIndex = 1;
            this.rdRunAutomatic.TabStop = true;
            this.rdRunAutomatic.Text = "Automatic";
            this.rdRunAutomatic.UseVisualStyleBackColor = true;
            this.rdRunAutomatic.CheckedChanged += new System.EventHandler(this.rdRunAutomatic_CheckedChanged);
            // 
            // chkSkipHandshake
            // 
            this.chkSkipHandshake.AutoSize = true;
            this.chkSkipHandshake.Checked = true;
            this.chkSkipHandshake.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSkipHandshake.Location = new System.Drawing.Point(188, 22);
            this.chkSkipHandshake.Name = "chkSkipHandshake";
            this.chkSkipHandshake.Size = new System.Drawing.Size(133, 21);
            this.chkSkipHandshake.TabIndex = 4;
            this.chkSkipHandshake.Text = "Skip Handshake";
            this.chkSkipHandshake.UseVisualStyleBackColor = true;
            this.chkSkipHandshake.CheckedChanged += new System.EventHandler(this.chkSkipHandshake_CheckedChanged);
            // 
            // pnlFlow
            // 
            this.pnlFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFlow.AutoScroll = true;
            this.pnlFlow.Controls.Add(this.pbFlow);
            this.pnlFlow.Location = new System.Drawing.Point(231, 82);
            this.pnlFlow.Name = "pnlFlow";
            this.pnlFlow.Size = new System.Drawing.Size(614, 422);
            this.pnlFlow.TabIndex = 5;
            this.pnlFlow.SizeChanged += new System.EventHandler(this.pnlFlow_SizeChanged);
            // 
            // grpNetworkSettings
            // 
            this.grpNetworkSettings.Controls.Add(this.lblDelay);
            this.grpNetworkSettings.Controls.Add(this.chkSkipHandshake);
            this.grpNetworkSettings.Controls.Add(this.numDelay);
            this.grpNetworkSettings.Location = new System.Drawing.Point(13, 13);
            this.grpNetworkSettings.Name = "grpNetworkSettings";
            this.grpNetworkSettings.Size = new System.Drawing.Size(328, 57);
            this.grpNetworkSettings.TabIndex = 6;
            this.grpNetworkSettings.TabStop = false;
            this.grpNetworkSettings.Text = "Network";
            // 
            // grpSenderSettings
            // 
            this.grpSenderSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpSenderSettings.Location = new System.Drawing.Point(13, 82);
            this.grpSenderSettings.Name = "grpSenderSettings";
            this.grpSenderSettings.Size = new System.Drawing.Size(212, 422);
            this.grpSenderSettings.TabIndex = 7;
            this.grpSenderSettings.TabStop = false;
            this.grpSenderSettings.Text = "Sender";
            // 
            // grpReceiverSettings
            // 
            this.grpReceiverSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpReceiverSettings.Controls.Add(this.numRXBufferSize);
            this.grpReceiverSettings.Controls.Add(this.m_lblRXBufferSize);
            this.grpReceiverSettings.Location = new System.Drawing.Point(851, 82);
            this.grpReceiverSettings.Name = "grpReceiverSettings";
            this.grpReceiverSettings.Size = new System.Drawing.Size(212, 422);
            this.grpReceiverSettings.TabIndex = 8;
            this.grpReceiverSettings.TabStop = false;
            this.grpReceiverSettings.Text = "Receiver";
            // 
            // numRXBufferSize
            // 
            this.numRXBufferSize.Location = new System.Drawing.Point(95, 22);
            this.numRXBufferSize.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numRXBufferSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRXBufferSize.Name = "numRXBufferSize";
            this.numRXBufferSize.Size = new System.Drawing.Size(101, 22);
            this.numRXBufferSize.TabIndex = 1;
            this.numRXBufferSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numRXBufferSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // m_lblRXBufferSize
            // 
            this.m_lblRXBufferSize.AutoSize = true;
            this.m_lblRXBufferSize.Location = new System.Drawing.Point(7, 22);
            this.m_lblRXBufferSize.Name = "m_lblRXBufferSize";
            this.m_lblRXBufferSize.Size = new System.Drawing.Size(81, 17);
            this.m_lblRXBufferSize.TabIndex = 0;
            this.m_lblRXBufferSize.Text = "Buffer Size:";
            // 
            // FlowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 516);
            this.Controls.Add(this.grpReceiverSettings);
            this.Controls.Add(this.grpSenderSettings);
            this.Controls.Add(this.grpNetworkSettings);
            this.Controls.Add(this.grpRunSettings);
            this.Controls.Add(this.pnlFlow);
            this.Name = "FlowForm";
            this.Text = "TCP Flow Visualizer";
            this.Shown += new System.EventHandler(this.FlowForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbFlow)).EndInit();
            this.ctxStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            this.grpRunSettings.ResumeLayout(false);
            this.grpRunSettings.PerformLayout();
            this.pnlFlow.ResumeLayout(false);
            this.pnlFlow.PerformLayout();
            this.grpNetworkSettings.ResumeLayout(false);
            this.grpNetworkSettings.PerformLayout();
            this.grpReceiverSettings.ResumeLayout(false);
            this.grpReceiverSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRXBufferSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbFlow;
        private System.Windows.Forms.NumericUpDown numDelay;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.GroupBox grpRunSettings;
        private System.Windows.Forms.Button btnTick;
        private System.Windows.Forms.RadioButton rdRunManual;
        private System.Windows.Forms.RadioButton rdRunAutomatic;
        private System.Windows.Forms.CheckBox chkSkipHandshake;
        private System.Windows.Forms.Panel pnlFlow;
        private System.Windows.Forms.GroupBox grpNetworkSettings;
        private System.Windows.Forms.GroupBox grpSenderSettings;
        private System.Windows.Forms.GroupBox grpReceiverSettings;
        private System.Windows.Forms.ContextMenuStrip ctxStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuLoseDataPacket;
        private System.Windows.Forms.ToolStripMenuItem mnuLoseAck;
        private System.Windows.Forms.ToolStripMenuItem mnuDelayDelivery;
        private System.Windows.Forms.NumericUpDown numRXBufferSize;
        private System.Windows.Forms.Label m_lblRXBufferSize;
    }
}

