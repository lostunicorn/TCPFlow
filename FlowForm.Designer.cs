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
            this.pbFlow = new System.Windows.Forms.PictureBox();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            this.lblDelay = new System.Windows.Forms.Label();
            this.grpRunSettings = new System.Windows.Forms.GroupBox();
            this.btnTick = new System.Windows.Forms.Button();
            this.rdRunManual = new System.Windows.Forms.RadioButton();
            this.rdRunAutomatic = new System.Windows.Forms.RadioButton();
            this.chkSkipHandshake = new System.Windows.Forms.CheckBox();
            this.pnlFlow = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbFlow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            this.grpRunSettings.SuspendLayout();
            this.pnlFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbFlow
            // 
            this.pbFlow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbFlow.Location = new System.Drawing.Point(3, 3);
            this.pbFlow.Name = "pbFlow";
            this.pbFlow.Size = new System.Drawing.Size(541, 348);
            this.pbFlow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbFlow.TabIndex = 0;
            this.pbFlow.TabStop = false;
            // 
            // numDelay
            // 
            this.numDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numDelay.Location = new System.Drawing.Point(734, 12);
            this.numDelay.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(68, 22);
            this.numDelay.TabIndex = 1;
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
            this.lblDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(625, 14);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(103, 17);
            this.lblDelay.TabIndex = 2;
            this.lblDelay.Text = "Network Delay:";
            // 
            // grpRunSettings
            // 
            this.grpRunSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRunSettings.Controls.Add(this.btnTick);
            this.grpRunSettings.Controls.Add(this.rdRunManual);
            this.grpRunSettings.Controls.Add(this.rdRunAutomatic);
            this.grpRunSettings.Location = new System.Drawing.Point(628, 126);
            this.grpRunSettings.Name = "grpRunSettings";
            this.grpRunSettings.Size = new System.Drawing.Size(174, 115);
            this.grpRunSettings.TabIndex = 3;
            this.grpRunSettings.TabStop = false;
            this.grpRunSettings.Text = "Run:";
            // 
            // btnTick
            // 
            this.btnTick.Location = new System.Drawing.Point(41, 77);
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
            this.rdRunManual.Location = new System.Drawing.Point(7, 50);
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
            this.rdRunAutomatic.TabIndex = 0;
            this.rdRunAutomatic.TabStop = true;
            this.rdRunAutomatic.Text = "Automatic";
            this.rdRunAutomatic.UseVisualStyleBackColor = true;
            // 
            // chkSkipHandshake
            // 
            this.chkSkipHandshake.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSkipHandshake.AutoSize = true;
            this.chkSkipHandshake.Location = new System.Drawing.Point(628, 67);
            this.chkSkipHandshake.Name = "chkSkipHandshake";
            this.chkSkipHandshake.Size = new System.Drawing.Size(133, 21);
            this.chkSkipHandshake.TabIndex = 4;
            this.chkSkipHandshake.Text = "Skip Handshake";
            this.chkSkipHandshake.UseVisualStyleBackColor = true;
            // 
            // pnlFlow
            // 
            this.pnlFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFlow.AutoScroll = true;
            this.pnlFlow.Controls.Add(this.pbFlow);
            this.pnlFlow.Location = new System.Drawing.Point(13, 13);
            this.pnlFlow.Name = "pnlFlow";
            this.pnlFlow.Size = new System.Drawing.Size(606, 448);
            this.pnlFlow.TabIndex = 5;
            this.pnlFlow.SizeChanged += new System.EventHandler(this.pnlFlow_SizeChanged);
            // 
            // FlowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 473);
            this.Controls.Add(this.pnlFlow);
            this.Controls.Add(this.chkSkipHandshake);
            this.Controls.Add(this.grpRunSettings);
            this.Controls.Add(this.lblDelay);
            this.Controls.Add(this.numDelay);
            this.Name = "FlowForm";
            this.Text = "TCP Flow Visualizer";
            ((System.ComponentModel.ISupportInitialize)(this.pbFlow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            this.grpRunSettings.ResumeLayout(false);
            this.grpRunSettings.PerformLayout();
            this.pnlFlow.ResumeLayout(false);
            this.pnlFlow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

