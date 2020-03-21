namespace WatchDoggo.Forms
{
    partial class SwitchAccount
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelVrtl = new System.Windows.Forms.Label();
            this.labelCurr = new System.Windows.Forms.Label();
            this.labelAcct = new System.Windows.Forms.Label();
            this.comboBoxAccounts = new System.Windows.Forms.ComboBox();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.buttonSelect);
            this.panel3.Controls.Add(this.buttonLogin);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(10, 105);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(264, 46);
            this.panel3.TabIndex = 3;
            // 
            // buttonSelect
            // 
            this.buttonSelect.AutoSize = true;
            this.buttonSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSelect.Location = new System.Drawing.Point(0, 0);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(264, 23);
            this.buttonSelect.TabIndex = 0;
            this.buttonSelect.Text = "Select Account";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.ButtonSelect_Click);
            // 
            // buttonLogin
            // 
            this.buttonLogin.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonLogin.Location = new System.Drawing.Point(0, 23);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(264, 23);
            this.buttonLogin.TabIndex = 1;
            this.buttonLogin.Text = "Login / Logout";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.ButtonLogout_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelVrtl);
            this.panel2.Controls.Add(this.labelCurr);
            this.panel2.Controls.Add(this.labelAcct);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(10, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(264, 74);
            this.panel2.TabIndex = 2;
            // 
            // labelVrtl
            // 
            this.labelVrtl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelVrtl.Location = new System.Drawing.Point(0, 46);
            this.labelVrtl.Name = "labelVrtl";
            this.labelVrtl.Size = new System.Drawing.Size(264, 17);
            this.labelVrtl.TabIndex = 2;
            this.labelVrtl.Text = "Virtual: N/A";
            this.labelVrtl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCurr
            // 
            this.labelCurr.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelCurr.Location = new System.Drawing.Point(0, 29);
            this.labelCurr.Name = "labelCurr";
            this.labelCurr.Size = new System.Drawing.Size(264, 17);
            this.labelCurr.TabIndex = 1;
            this.labelCurr.Text = "Currency: N/A";
            this.labelCurr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAcct
            // 
            this.labelAcct.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAcct.Location = new System.Drawing.Point(0, 12);
            this.labelAcct.Name = "labelAcct";
            this.labelAcct.Size = new System.Drawing.Size(264, 17);
            this.labelAcct.TabIndex = 0;
            this.labelAcct.Text = "Name: N/A";
            this.labelAcct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxAccounts
            // 
            this.comboBoxAccounts.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAccounts.FormattingEnabled = true;
            this.comboBoxAccounts.Location = new System.Drawing.Point(10, 10);
            this.comboBoxAccounts.Name = "comboBoxAccounts";
            this.comboBoxAccounts.Size = new System.Drawing.Size(264, 21);
            this.comboBoxAccounts.TabIndex = 0;
            this.comboBoxAccounts.SelectedIndexChanged += new System.EventHandler(this.ComboBoxAccounts_SelectedIndexChanged);
            // 
            // SwitchAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.comboBoxAccounts);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SwitchAccount";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose Account";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxAccounts;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelCurr;
        private System.Windows.Forms.Label labelAcct;
        private System.Windows.Forms.Label labelVrtl;
    }
}