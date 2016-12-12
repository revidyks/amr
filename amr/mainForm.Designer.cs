namespace amr
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.buttonStart = new System.Windows.Forms.Button();
            this.numericUpDownQuantity = new System.Windows.Forms.NumericUpDown();
            this.pictureBoxMyLogo = new System.Windows.Forms.PictureBox();
            this.linkLabelMySite = new System.Windows.Forms.LinkLabel();
            this.checkedListBoxDomains = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxSex = new System.Windows.Forms.CheckedListBox();
            this.checkBoxMyWorld = new System.Windows.Forms.CheckBox();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.checkBoxUseProxy = new System.Windows.Forms.CheckBox();
            this.numericUpDownThreads = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMyLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreads)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.Black;
            this.buttonStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonStart.Location = new System.Drawing.Point(82, 11);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(79, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "start";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // numericUpDownQuantity
            // 
            this.numericUpDownQuantity.BackColor = System.Drawing.Color.Black;
            this.numericUpDownQuantity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.numericUpDownQuantity.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownQuantity.Location = new System.Drawing.Point(12, 12);
            this.numericUpDownQuantity.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownQuantity.Name = "numericUpDownQuantity";
            this.numericUpDownQuantity.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownQuantity.TabIndex = 2;
            this.numericUpDownQuantity.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // pictureBoxMyLogo
            // 
            this.pictureBoxMyLogo.BackColor = System.Drawing.Color.Black;
            this.pictureBoxMyLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxMyLogo.Image")));
            this.pictureBoxMyLogo.Location = new System.Drawing.Point(167, 12);
            this.pictureBoxMyLogo.Name = "pictureBoxMyLogo";
            this.pictureBoxMyLogo.Size = new System.Drawing.Size(100, 100);
            this.pictureBoxMyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMyLogo.TabIndex = 36;
            this.pictureBoxMyLogo.TabStop = false;
            // 
            // linkLabelMySite
            // 
            this.linkLabelMySite.AutoSize = true;
            this.linkLabelMySite.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.linkLabelMySite.Location = new System.Drawing.Point(165, 121);
            this.linkLabelMySite.Name = "linkLabelMySite";
            this.linkLabelMySite.Size = new System.Drawing.Size(100, 18);
            this.linkLabelMySite.TabIndex = 32;
            this.linkLabelMySite.TabStop = true;
            this.linkLabelMySite.Tag = "";
            this.linkLabelMySite.Text = "http://skyhighlab.ru";
            this.linkLabelMySite.UseCompatibleTextRendering = true;
            // 
            // checkedListBoxDomains
            // 
            this.checkedListBoxDomains.BackColor = System.Drawing.Color.Black;
            this.checkedListBoxDomains.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkedListBoxDomains.FormattingEnabled = true;
            this.checkedListBoxDomains.Items.AddRange(new object[] {
            "mail.ru",
            "bk.ru",
            "list.ru",
            "inbox.ru"});
            this.checkedListBoxDomains.Location = new System.Drawing.Point(12, 78);
            this.checkedListBoxDomains.Name = "checkedListBoxDomains";
            this.checkedListBoxDomains.Size = new System.Drawing.Size(64, 64);
            this.checkedListBoxDomains.TabIndex = 37;
            // 
            // checkedListBoxSex
            // 
            this.checkedListBoxSex.BackColor = System.Drawing.Color.Black;
            this.checkedListBoxSex.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkedListBoxSex.FormattingEnabled = true;
            this.checkedListBoxSex.Items.AddRange(new object[] {
            "male",
            "female"});
            this.checkedListBoxSex.Location = new System.Drawing.Point(12, 38);
            this.checkedListBoxSex.Name = "checkedListBoxSex";
            this.checkedListBoxSex.Size = new System.Drawing.Size(64, 34);
            this.checkedListBoxSex.TabIndex = 38;
            // 
            // checkBoxMyWorld
            // 
            this.checkBoxMyWorld.AutoSize = true;
            this.checkBoxMyWorld.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxMyWorld.Location = new System.Drawing.Point(82, 38);
            this.checkBoxMyWorld.Name = "checkBoxMyWorld";
            this.checkBoxMyWorld.Size = new System.Drawing.Size(69, 17);
            this.checkBoxMyWorld.TabIndex = 39;
            this.checkBoxMyWorld.Text = "my world";
            this.checkBoxMyWorld.UseVisualStyleBackColor = true;
            // 
            // labelQuantity
            // 
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.labelQuantity.Location = new System.Drawing.Point(82, 129);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(13, 13);
            this.labelQuantity.TabIndex = 40;
            this.labelQuantity.Text = "0";
            // 
            // checkBoxUseProxy
            // 
            this.checkBoxUseProxy.AutoSize = true;
            this.checkBoxUseProxy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxUseProxy.Location = new System.Drawing.Point(82, 55);
            this.checkBoxUseProxy.Name = "checkBoxUseProxy";
            this.checkBoxUseProxy.Size = new System.Drawing.Size(74, 17);
            this.checkBoxUseProxy.TabIndex = 41;
            this.checkBoxUseProxy.Text = "use proxy";
            this.checkBoxUseProxy.UseVisualStyleBackColor = true;
            // 
            // numericUpDownThreads
            // 
            this.numericUpDownThreads.BackColor = System.Drawing.Color.Black;
            this.numericUpDownThreads.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.numericUpDownThreads.Location = new System.Drawing.Point(82, 78);
            this.numericUpDownThreads.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownThreads.Name = "numericUpDownThreads";
            this.numericUpDownThreads.Size = new System.Drawing.Size(79, 20);
            this.numericUpDownThreads.TabIndex = 42;
            this.numericUpDownThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(279, 149);
            this.Controls.Add(this.numericUpDownThreads);
            this.Controls.Add(this.checkBoxUseProxy);
            this.Controls.Add(this.labelQuantity);
            this.Controls.Add(this.checkBoxMyWorld);
            this.Controls.Add(this.checkedListBoxSex);
            this.Controls.Add(this.checkedListBoxDomains);
            this.Controls.Add(this.pictureBoxMyLogo);
            this.Controls.Add(this.linkLabelMySite);
            this.Controls.Add(this.numericUpDownQuantity);
            this.Controls.Add(this.buttonStart);
            this.MaximumSize = new System.Drawing.Size(295, 185);
            this.MinimumSize = new System.Drawing.Size(295, 185);
            this.Name = "mainForm";
            this.Text = "automatic mail registration";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMyLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.NumericUpDown numericUpDownQuantity;
        private System.Windows.Forms.PictureBox pictureBoxMyLogo;
        private System.Windows.Forms.LinkLabel linkLabelMySite;
        private System.Windows.Forms.Label labelQuantity;
        public System.Windows.Forms.CheckedListBox checkedListBoxDomains;
        public System.Windows.Forms.CheckedListBox checkedListBoxSex;
        public System.Windows.Forms.CheckBox checkBoxMyWorld;
        public System.Windows.Forms.CheckBox checkBoxUseProxy;
        private System.Windows.Forms.NumericUpDown numericUpDownThreads;
    }
}

