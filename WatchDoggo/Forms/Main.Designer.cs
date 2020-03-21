namespace WatchDoggo.Forms
{
    partial class Main
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel4 = new System.Windows.Forms.Panel();
            this.buttonReset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSwitch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelAccount = new System.Windows.Forms.Label();
            this.labelBalance = new System.Windows.Forms.Label();
            this.dataGridViewMarket = new System.Windows.Forms.DataGridView();
            this.MarketColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MarketColumn2 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridViewProfitTable = new System.Windows.Forms.DataGridView();
            this.StatementColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatementColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatementColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelStatemant = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarket)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProfitTable)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.buttonReset);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.buttonSwitch);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.labelAccount);
            this.panel4.Controls.Add(this.labelBalance);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(10, 10);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(280, 135);
            this.panel4.TabIndex = 36;
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(14, 96);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(249, 23);
            this.buttonReset.TabIndex = 20;
            this.buttonReset.Text = "Refresh";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 12);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.label1.Size = new System.Drawing.Size(59, 26);
            this.label1.TabIndex = 10;
            this.label1.Text = "Account:";
            // 
            // buttonSwitch
            // 
            this.buttonSwitch.Location = new System.Drawing.Point(14, 67);
            this.buttonSwitch.Name = "buttonSwitch";
            this.buttonSwitch.Size = new System.Drawing.Size(249, 23);
            this.buttonSwitch.TabIndex = 0;
            this.buttonSwitch.Text = "Switch Account";
            this.buttonSwitch.UseVisualStyleBackColor = true;
            this.buttonSwitch.Click += new System.EventHandler(this.ButtonSwitch_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 38);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.label3.Size = new System.Drawing.Size(61, 26);
            this.label3.TabIndex = 11;
            this.label3.Text = "Balance:";
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccount.Location = new System.Drawing.Point(73, 12);
            this.labelAccount.Margin = new System.Windows.Forms.Padding(0);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(34, 16);
            this.labelAccount.TabIndex = 2;
            this.labelAccount.Text = "N/A";
            // 
            // labelBalance
            // 
            this.labelBalance.AutoSize = true;
            this.labelBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBalance.Location = new System.Drawing.Point(73, 38);
            this.labelBalance.Margin = new System.Windows.Forms.Padding(0);
            this.labelBalance.Name = "labelBalance";
            this.labelBalance.Size = new System.Drawing.Size(34, 16);
            this.labelBalance.TabIndex = 3;
            this.labelBalance.Text = "N/A";
            // 
            // dataGridViewMarket
            // 
            this.dataGridViewMarket.AllowUserToAddRows = false;
            this.dataGridViewMarket.AllowUserToDeleteRows = false;
            this.dataGridViewMarket.AllowUserToResizeColumns = false;
            this.dataGridViewMarket.AllowUserToResizeRows = false;
            this.dataGridViewMarket.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewMarket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMarket.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MarketColumn1,
            this.MarketColumn2});
            this.dataGridViewMarket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMarket.Location = new System.Drawing.Point(0, 32);
            this.dataGridViewMarket.MultiSelect = false;
            this.dataGridViewMarket.Name = "dataGridViewMarket";
            this.dataGridViewMarket.ReadOnly = true;
            this.dataGridViewMarket.RowHeadersVisible = false;
            this.dataGridViewMarket.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewMarket.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMarket.Size = new System.Drawing.Size(280, 224);
            this.dataGridViewMarket.TabIndex = 2;
            this.dataGridViewMarket.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewMarket_CellContentClick);
            // 
            // MarketColumn1
            // 
            this.MarketColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MarketColumn1.HeaderText = "Name";
            this.MarketColumn1.Name = "MarketColumn1";
            this.MarketColumn1.ReadOnly = true;
            this.MarketColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // MarketColumn2
            // 
            this.MarketColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.MarketColumn2.HeaderText = "Action";
            this.MarketColumn2.MinimumWidth = 60;
            this.MarketColumn2.Name = "MarketColumn2";
            this.MarketColumn2.ReadOnly = true;
            this.MarketColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MarketColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.MarketColumn2.Width = 60;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5, 5, 10, 10);
            this.panel2.Size = new System.Drawing.Size(280, 32);
            this.panel2.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 25);
            this.label5.TabIndex = 6;
            this.label5.Text = "Available Market";
            // 
            // dataGridViewProfitTable
            // 
            this.dataGridViewProfitTable.AllowUserToAddRows = false;
            this.dataGridViewProfitTable.AllowUserToDeleteRows = false;
            this.dataGridViewProfitTable.AllowUserToResizeColumns = false;
            this.dataGridViewProfitTable.AllowUserToResizeRows = false;
            this.dataGridViewProfitTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewProfitTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProfitTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StatementColumn1,
            this.StatementColumn2,
            this.StatementColumn4});
            this.dataGridViewProfitTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProfitTable.Location = new System.Drawing.Point(5, 44);
            this.dataGridViewProfitTable.MultiSelect = false;
            this.dataGridViewProfitTable.Name = "dataGridViewProfitTable";
            this.dataGridViewProfitTable.ReadOnly = true;
            this.dataGridViewProfitTable.RowHeadersVisible = false;
            this.dataGridViewProfitTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewProfitTable.RowTemplate.Height = 150;
            this.dataGridViewProfitTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewProfitTable.Size = new System.Drawing.Size(423, 357);
            this.dataGridViewProfitTable.TabIndex = 1;
            // 
            // StatementColumn1
            // 
            this.StatementColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.StatementColumn1.HeaderText = "Date";
            this.StatementColumn1.MinimumWidth = 100;
            this.StatementColumn1.Name = "StatementColumn1";
            this.StatementColumn1.ReadOnly = true;
            this.StatementColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // StatementColumn2
            // 
            this.StatementColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.StatementColumn2.DefaultCellStyle = dataGridViewCellStyle1;
            this.StatementColumn2.HeaderText = "Contract";
            this.StatementColumn2.Name = "StatementColumn2";
            this.StatementColumn2.ReadOnly = true;
            this.StatementColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // StatementColumn4
            // 
            this.StatementColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatementColumn4.DefaultCellStyle = dataGridViewCellStyle2;
            this.StatementColumn4.HeaderText = "Profit/Loss";
            this.StatementColumn4.MinimumWidth = 100;
            this.StatementColumn4.Name = "StatementColumn4";
            this.StatementColumn4.ReadOnly = true;
            this.StatementColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Window;
            this.panel3.Controls.Add(this.labelStatemant);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(423, 32);
            this.panel3.TabIndex = 2;
            // 
            // labelStatemant
            // 
            this.labelStatemant.AutoSize = true;
            this.labelStatemant.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatemant.Location = new System.Drawing.Point(3, 3);
            this.labelStatemant.Name = "labelStatemant";
            this.labelStatemant.Size = new System.Drawing.Size(111, 25);
            this.labelStatemant.TabIndex = 7;
            this.labelStatemant.Text = "Profit Table";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel6);
            this.splitContainer1.Panel1.Controls.Add(this.panel4);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 5, 10);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewProfitTable);
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(5, 12, 10, 10);
            this.splitContainer1.Size = new System.Drawing.Size(734, 411);
            this.splitContainer1.SplitterDistance = 295;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 6;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dataGridViewMarket);
            this.panel6.Controls.Add(this.panel2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(10, 145);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(280, 256);
            this.panel6.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 411);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(750, 450);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarket)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProfitTable)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonSwitch;
        private System.Windows.Forms.Label labelBalance;
        private System.Windows.Forms.Label labelAccount;
        private System.Windows.Forms.DataGridView dataGridViewProfitTable;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelStatemant;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewMarket;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatementColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatementColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatementColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn MarketColumn1;
        private System.Windows.Forms.DataGridViewLinkColumn MarketColumn2;
    }
}