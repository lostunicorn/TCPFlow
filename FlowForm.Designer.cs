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
            this.mnuPacketDelay = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuLoseAck = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAckDelay = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelayDelivery = new System.Windows.Forms.ToolStripMenuItem();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            this.lblDelay = new System.Windows.Forms.Label();
            this.grpRunSettings = new System.Windows.Forms.GroupBox();
            this.rdRunAutomaticUntilSteady = new System.Windows.Forms.RadioButton();
            this.btnTick = new System.Windows.Forms.Button();
            this.rdRunManual = new System.Windows.Forms.RadioButton();
            this.rdRunAutomatic = new System.Windows.Forms.RadioButton();
            this.chkSkipHandshake = new System.Windows.Forms.CheckBox();
            this.pnlFlow = new System.Windows.Forms.Panel();
            this.grpNetworkSettings = new System.Windows.Forms.GroupBox();
            this.grpSenderSettings = new System.Windows.Forms.GroupBox();
            this.numTXTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblTXTimeout = new System.Windows.Forms.Label();
            this.chkCongestionControl = new System.Windows.Forms.CheckBox();
            this.grpReceiverSettings = new System.Windows.Forms.GroupBox();
            this.numDeliveryInterval = new System.Windows.Forms.NumericUpDown();
            this.lblDeliveryInterval = new System.Windows.Forms.Label();
            this.numRXTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblRXTimeout = new System.Windows.Forms.Label();
            this.numRXBufferSize = new System.Windows.Forms.NumericUpDown();
            this.m_lblRXBufferSize = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbFlow)).BeginInit();
            this.ctxStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            this.grpRunSettings.SuspendLayout();
            this.pnlFlow.SuspendLayout();
            this.grpNetworkSettings.SuspendLayout();
            this.grpSenderSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTXTimeout)).BeginInit();
            this.grpReceiverSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeliveryInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRXTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRXBufferSize)).BeginInit();
            this.SuspendLayout();
            // 
            // pbFlow
            // 
            this.pbFlow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbFlow.ContextMenuStrip = this.ctxStrip;
            this.pbFlow.Location = new System.Drawing.Point(0, 0);
            this.pbFlow.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbFlow.Name = "pbFlow";
            this.pbFlow.Size = new System.Drawing.Size(540, 348);
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
            this.mnuPacketDelay,
            this.toolStripSeparator1,
            this.mnuLoseAck,
            this.mnuAckDelay,
            this.mnuDelayDelivery});
            this.ctxStrip.Name = "ctxStrip";
            this.ctxStrip.ShowCheckMargin = true;
            this.ctxStrip.ShowImageMargin = false;
            this.ctxStrip.Size = new System.Drawing.Size(196, 120);
            this.ctxStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ctxStrip_Opening);
            // 
            // mnuLoseDataPacket
            // 
            this.mnuLoseDataPacket.Name = "mnuLoseDataPacket";
            this.mnuLoseDataPacket.Size = new System.Drawing.Size(195, 22);
            this.mnuLoseDataPacket.Text = "Lose Data Packet";
            this.mnuLoseDataPacket.Click += new System.EventHandler(this.mnuLoseDataPacket_Click);
            // 
            // mnuPacketDelay
            // 
            this.mnuPacketDelay.Name = "mnuPacketDelay";
            this.mnuPacketDelay.Size = new System.Drawing.Size(195, 22);
            this.mnuPacketDelay.Text = "Set Packet Delay...";
            this.mnuPacketDelay.Click += new System.EventHandler(this.mnuPacketDelay_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // mnuLoseAck
            // 
            this.mnuLoseAck.Name = "mnuLoseAck";
            this.mnuLoseAck.Size = new System.Drawing.Size(195, 22);
            this.mnuLoseAck.Text = "Lose Ack";
            this.mnuLoseAck.Click += new System.EventHandler(this.mnuLoseAck_Click);
            // 
            // mnuAckDelay
            // 
            this.mnuAckDelay.Name = "mnuAckDelay";
            this.mnuAckDelay.Size = new System.Drawing.Size(195, 22);
            this.mnuAckDelay.Text = "Set Ack Delay...";
            this.mnuAckDelay.Click += new System.EventHandler(this.mnuAckDelay_Click);
            // 
            // mnuDelayDelivery
            // 
            this.mnuDelayDelivery.Name = "mnuDelayDelivery";
            this.mnuDelayDelivery.Size = new System.Drawing.Size(195, 22);
            this.mnuDelayDelivery.Text = "Delay Packet Delivery...";
            this.mnuDelayDelivery.Click += new System.EventHandler(this.mnuDelayDelivery_Click);
            // 
            // numDelay
            // 
            this.numDelay.Location = new System.Drawing.Point(98, 17);
            this.numDelay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numDelay.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(56, 20);
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
            this.lblDelay.Location = new System.Drawing.Point(5, 19);
            this.lblDelay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(37, 13);
            this.lblDelay.TabIndex = 2;
            this.lblDelay.Text = "Delay:";
            // 
            // grpRunSettings
            // 
            this.grpRunSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRunSettings.Controls.Add(this.rdRunAutomaticUntilSteady);
            this.grpRunSettings.Controls.Add(this.btnTick);
            this.grpRunSettings.Controls.Add(this.rdRunManual);
            this.grpRunSettings.Controls.Add(this.rdRunAutomatic);
            this.grpRunSettings.Location = new System.Drawing.Point(638, 10);
            this.grpRunSettings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpRunSettings.Name = "grpRunSettings";
            this.grpRunSettings.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpRunSettings.Size = new System.Drawing.Size(159, 89);
            this.grpRunSettings.TabIndex = 3;
            this.grpRunSettings.TabStop = false;
            this.grpRunSettings.Text = "Simulation";
            // 
            // rdRunAutomaticUntilSteady
            // 
            this.rdRunAutomaticUntilSteady.AutoSize = true;
            this.rdRunAutomaticUntilSteady.Location = new System.Drawing.Point(5, 39);
            this.rdRunAutomaticUntilSteady.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdRunAutomaticUntilSteady.Name = "rdRunAutomaticUntilSteady";
            this.rdRunAutomaticUntilSteady.Size = new System.Drawing.Size(154, 17);
            this.rdRunAutomaticUntilSteady.TabIndex = 3;
            this.rdRunAutomaticUntilSteady.TabStop = true;
            this.rdRunAutomaticUntilSteady.Text = "Automatic until steady state";
            this.rdRunAutomaticUntilSteady.UseVisualStyleBackColor = true;
            this.rdRunAutomaticUntilSteady.CheckedChanged += new System.EventHandler(this.rdRunAutomaticUntilSteady_CheckedChanged);
            // 
            // btnTick
            // 
            this.btnTick.Location = new System.Drawing.Point(75, 62);
            this.btnTick.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnTick.Name = "btnTick";
            this.btnTick.Size = new System.Drawing.Size(56, 19);
            this.btnTick.TabIndex = 2;
            this.btnTick.Text = "Tick";
            this.btnTick.UseVisualStyleBackColor = true;
            this.btnTick.Click += new System.EventHandler(this.btnTick_Click);
            // 
            // rdRunManual
            // 
            this.rdRunManual.AutoSize = true;
            this.rdRunManual.Checked = true;
            this.rdRunManual.Location = new System.Drawing.Point(5, 61);
            this.rdRunManual.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdRunManual.Name = "rdRunManual";
            this.rdRunManual.Size = new System.Drawing.Size(60, 17);
            this.rdRunManual.TabIndex = 1;
            this.rdRunManual.TabStop = true;
            this.rdRunManual.Text = "Manual";
            this.rdRunManual.UseVisualStyleBackColor = true;
            // 
            // rdRunAutomatic
            // 
            this.rdRunAutomatic.AutoSize = true;
            this.rdRunAutomatic.Location = new System.Drawing.Point(4, 17);
            this.rdRunAutomatic.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdRunAutomatic.Name = "rdRunAutomatic";
            this.rdRunAutomatic.Size = new System.Drawing.Size(72, 17);
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
            this.chkSkipHandshake.Location = new System.Drawing.Point(4, 40);
            this.chkSkipHandshake.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkSkipHandshake.Name = "chkSkipHandshake";
            this.chkSkipHandshake.Size = new System.Drawing.Size(105, 17);
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
            this.pnlFlow.Location = new System.Drawing.Point(10, 10);
            this.pnlFlow.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlFlow.Name = "pnlFlow";
            this.pnlFlow.Size = new System.Drawing.Size(624, 409);
            this.pnlFlow.TabIndex = 5;
            // 
            // grpNetworkSettings
            // 
            this.grpNetworkSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpNetworkSettings.Controls.Add(this.lblDelay);
            this.grpNetworkSettings.Controls.Add(this.numDelay);
            this.grpNetworkSettings.Location = new System.Drawing.Point(638, 197);
            this.grpNetworkSettings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpNetworkSettings.Name = "grpNetworkSettings";
            this.grpNetworkSettings.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpNetworkSettings.Size = new System.Drawing.Size(159, 50);
            this.grpNetworkSettings.TabIndex = 6;
            this.grpNetworkSettings.TabStop = false;
            this.grpNetworkSettings.Text = "Network Options";
            // 
            // grpSenderSettings
            // 
            this.grpSenderSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSenderSettings.Controls.Add(this.numTXTimeout);
            this.grpSenderSettings.Controls.Add(this.chkSkipHandshake);
            this.grpSenderSettings.Controls.Add(this.lblTXTimeout);
            this.grpSenderSettings.Controls.Add(this.chkCongestionControl);
            this.grpSenderSettings.Location = new System.Drawing.Point(638, 103);
            this.grpSenderSettings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpSenderSettings.Name = "grpSenderSettings";
            this.grpSenderSettings.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpSenderSettings.Size = new System.Drawing.Size(159, 89);
            this.grpSenderSettings.TabIndex = 7;
            this.grpSenderSettings.TabStop = false;
            this.grpSenderSettings.Text = "Sender Options";
            // 
            // numTXTimeout
            // 
            this.numTXTimeout.Location = new System.Drawing.Point(97, 62);
            this.numTXTimeout.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numTXTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTXTimeout.Name = "numTXTimeout";
            this.numTXTimeout.Size = new System.Drawing.Size(57, 20);
            this.numTXTimeout.TabIndex = 2;
            this.numTXTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numTXTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblTXTimeout
            // 
            this.lblTXTimeout.AutoSize = true;
            this.lblTXTimeout.Location = new System.Drawing.Point(4, 63);
            this.lblTXTimeout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTXTimeout.Name = "lblTXTimeout";
            this.lblTXTimeout.Size = new System.Drawing.Size(48, 13);
            this.lblTXTimeout.TabIndex = 1;
            this.lblTXTimeout.Text = "Timeout:";
            // 
            // chkCongestionControl
            // 
            this.chkCongestionControl.AutoSize = true;
            this.chkCongestionControl.Location = new System.Drawing.Point(5, 18);
            this.chkCongestionControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkCongestionControl.Name = "chkCongestionControl";
            this.chkCongestionControl.Size = new System.Drawing.Size(151, 17);
            this.chkCongestionControl.TabIndex = 0;
            this.chkCongestionControl.Text = "Enable Congestion Control";
            this.chkCongestionControl.UseVisualStyleBackColor = true;
            // 
            // grpReceiverSettings
            // 
            this.grpReceiverSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpReceiverSettings.Controls.Add(this.numDeliveryInterval);
            this.grpReceiverSettings.Controls.Add(this.lblDeliveryInterval);
            this.grpReceiverSettings.Controls.Add(this.numRXTimeout);
            this.grpReceiverSettings.Controls.Add(this.lblRXTimeout);
            this.grpReceiverSettings.Controls.Add(this.numRXBufferSize);
            this.grpReceiverSettings.Controls.Add(this.m_lblRXBufferSize);
            this.grpReceiverSettings.Location = new System.Drawing.Point(638, 251);
            this.grpReceiverSettings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpReceiverSettings.Name = "grpReceiverSettings";
            this.grpReceiverSettings.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpReceiverSettings.Size = new System.Drawing.Size(159, 123);
            this.grpReceiverSettings.TabIndex = 8;
            this.grpReceiverSettings.TabStop = false;
            this.grpReceiverSettings.Text = "Receiver Options";
            // 
            // numDeliveryInterval
            // 
            this.numDeliveryInterval.Location = new System.Drawing.Point(98, 84);
            this.numDeliveryInterval.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numDeliveryInterval.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numDeliveryInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDeliveryInterval.Name = "numDeliveryInterval";
            this.numDeliveryInterval.Size = new System.Drawing.Size(57, 20);
            this.numDeliveryInterval.TabIndex = 5;
            this.numDeliveryInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numDeliveryInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblDeliveryInterval
            // 
            this.lblDeliveryInterval.AutoSize = true;
            this.lblDeliveryInterval.Location = new System.Drawing.Point(8, 84);
            this.lblDeliveryInterval.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDeliveryInterval.Name = "lblDeliveryInterval";
            this.lblDeliveryInterval.Size = new System.Drawing.Size(86, 13);
            this.lblDeliveryInterval.TabIndex = 4;
            this.lblDeliveryInterval.Text = "Delivery Interval:";
            // 
            // numRXTimeout
            // 
            this.numRXTimeout.Location = new System.Drawing.Point(98, 52);
            this.numRXTimeout.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numRXTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRXTimeout.Name = "numRXTimeout";
            this.numRXTimeout.Size = new System.Drawing.Size(57, 20);
            this.numRXTimeout.TabIndex = 3;
            this.numRXTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numRXTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblRXTimeout
            // 
            this.lblRXTimeout.AutoSize = true;
            this.lblRXTimeout.Location = new System.Drawing.Point(5, 54);
            this.lblRXTimeout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRXTimeout.Name = "lblRXTimeout";
            this.lblRXTimeout.Size = new System.Drawing.Size(48, 13);
            this.lblRXTimeout.TabIndex = 2;
            this.lblRXTimeout.Text = "Timeout:";
            // 
            // numRXBufferSize
            // 
            this.numRXBufferSize.Location = new System.Drawing.Point(98, 18);
            this.numRXBufferSize.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.numRXBufferSize.Size = new System.Drawing.Size(57, 20);
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
            this.m_lblRXBufferSize.Location = new System.Drawing.Point(5, 18);
            this.m_lblRXBufferSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_lblRXBufferSize.Name = "m_lblRXBufferSize";
            this.m_lblRXBufferSize.Size = new System.Drawing.Size(61, 13);
            this.m_lblRXBufferSize.TabIndex = 0;
            this.m_lblRXBufferSize.Text = "Buffer Size:";
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(638, 386);
            this.btnReset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(56, 19);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(741, 386);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(56, 19);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save...";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FlowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 428);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpReceiverSettings);
            this.Controls.Add(this.grpSenderSettings);
            this.Controls.Add(this.grpNetworkSettings);
            this.Controls.Add(this.grpRunSettings);
            this.Controls.Add(this.pnlFlow);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.grpSenderSettings.ResumeLayout(false);
            this.grpSenderSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTXTimeout)).EndInit();
            this.grpReceiverSettings.ResumeLayout(false);
            this.grpReceiverSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeliveryInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRXTimeout)).EndInit();
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
        private System.Windows.Forms.Label lblTXTimeout;
        private System.Windows.Forms.CheckBox chkCongestionControl;
        private System.Windows.Forms.NumericUpDown numRXTimeout;
        private System.Windows.Forms.Label lblRXTimeout;
        private System.Windows.Forms.NumericUpDown numTXTimeout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuPacketDelay;
        private System.Windows.Forms.ToolStripMenuItem mnuAckDelay;
        private System.Windows.Forms.NumericUpDown numDeliveryInterval;
        private System.Windows.Forms.Label lblDeliveryInterval;
        private System.Windows.Forms.RadioButton rdRunAutomaticUntilSteady;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSave;
    }
}

