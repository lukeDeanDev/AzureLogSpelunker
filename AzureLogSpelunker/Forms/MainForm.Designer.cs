namespace AzureLogSpelunker.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ConnectionNickname = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ConnectionString = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RememberConnection = new System.Windows.Forms.Button();
            this.ForgetConnection = new System.Windows.Forms.Button();
            this.btnFetchFromAzure = new System.Windows.Forms.Button();
            this.BeginDateTime = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.EndDateTime = new System.Windows.Forms.DateTimePicker();
            this.UTC = new System.Windows.Forms.CheckBox();
            this.BeginPartitionKey = new System.Windows.Forms.TextBox();
            this.EndPartitionKey = new System.Windows.Forms.TextBox();
            this.cbFilters = new System.Windows.Forms.CheckedListBox();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.btAdd = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.btUp = new System.Windows.Forms.Button();
            this.btDown = new System.Windows.Forms.Button();
            this.btnApplySqlFilters = new System.Windows.Forms.Button();
            this.azureBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.grpAzure = new System.Windows.Forms.GroupBox();
            this.grpTimeframe = new System.Windows.Forms.GroupBox();
            this.grpFetchTo = new System.Windows.Forms.GroupBox();
            this.FetchToDisk = new System.Windows.Forms.RadioButton();
            this.FetchToMemory = new System.Windows.Forms.RadioButton();
            this.RecordsCached = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.grpLocalSqlFilters = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.sqlComputed = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.sqliteHelp = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.sqlBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.grpAzure.SuspendLayout();
            this.grpTimeframe.SuspendLayout();
            this.grpFetchTo.SuspendLayout();
            this.grpLocalSqlFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConnectionNickname
            // 
            this.ConnectionNickname.FormattingEnabled = true;
            this.ConnectionNickname.Location = new System.Drawing.Point(124, 13);
            this.ConnectionNickname.Name = "ConnectionNickname";
            this.ConnectionNickname.Size = new System.Drawing.Size(209, 21);
            this.ConnectionNickname.TabIndex = 0;
            this.toolTip1.SetToolTip(this.ConnectionNickname, "Your personal nickname that you\'d like to remember this Connection String by");
            this.ConnectionNickname.SelectedIndexChanged += new System.EventHandler(this.ConnectionNickname_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Connection Nickname";
            // 
            // ConnectionString
            // 
            this.ConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectionString.Location = new System.Drawing.Point(124, 40);
            this.ConnectionString.Name = "ConnectionString";
            this.ConnectionString.Size = new System.Drawing.Size(678, 20);
            this.ConnectionString.TabIndex = 3;
            this.toolTip1.SetToolTip(this.ConnectionString, "The full connection string for the Azure Table Storage you want to connect to.  S" +
        "elect the \"Example\" Connection Nickname for an example.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Connection String";
            // 
            // RememberConnection
            // 
            this.RememberConnection.Image = ((System.Drawing.Image)(resources.GetObject("RememberConnection.Image")));
            this.RememberConnection.Location = new System.Drawing.Point(339, 11);
            this.RememberConnection.Name = "RememberConnection";
            this.RememberConnection.Size = new System.Drawing.Size(26, 23);
            this.RememberConnection.TabIndex = 1;
            this.toolTip1.SetToolTip(this.RememberConnection, "Remember this connection string by this connection nickname.");
            this.RememberConnection.UseVisualStyleBackColor = true;
            this.RememberConnection.Click += new System.EventHandler(this.RememberConnection_Click);
            // 
            // ForgetConnection
            // 
            this.ForgetConnection.Image = ((System.Drawing.Image)(resources.GetObject("ForgetConnection.Image")));
            this.ForgetConnection.Location = new System.Drawing.Point(371, 11);
            this.ForgetConnection.Name = "ForgetConnection";
            this.ForgetConnection.Size = new System.Drawing.Size(26, 23);
            this.ForgetConnection.TabIndex = 2;
            this.toolTip1.SetToolTip(this.ForgetConnection, "Forget this connection");
            this.ForgetConnection.UseVisualStyleBackColor = true;
            this.ForgetConnection.Click += new System.EventHandler(this.ForgetConnection_Click);
            // 
            // btnFetchFromAzure
            // 
            this.btnFetchFromAzure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFetchFromAzure.Image = ((System.Drawing.Image)(resources.GetObject("btnFetchFromAzure.Image")));
            this.btnFetchFromAzure.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFetchFromAzure.Location = new System.Drawing.Point(665, 81);
            this.btnFetchFromAzure.Name = "btnFetchFromAzure";
            this.btnFetchFromAzure.Size = new System.Drawing.Size(124, 32);
            this.btnFetchFromAzure.TabIndex = 6;
            this.btnFetchFromAzure.Text = "Fetch from Azure";
            this.btnFetchFromAzure.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFetchFromAzure.UseVisualStyleBackColor = true;
            this.btnFetchFromAzure.Click += new System.EventHandler(this.btnFetchFromAzure_Click);
            // 
            // BeginDateTime
            // 
            this.BeginDateTime.CustomFormat = "MM\'/\'dd\'/\'yyyy hh\':\'mm\':\'ss tt ";
            this.BeginDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.BeginDateTime.Location = new System.Drawing.Point(107, 18);
            this.BeginDateTime.Name = "BeginDateTime";
            this.BeginDateTime.Size = new System.Drawing.Size(174, 20);
            this.BeginDateTime.TabIndex = 1;
            this.BeginDateTime.ValueChanged += new System.EventHandler(this.BeginDateTime_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Begin";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "End";
            // 
            // EndDateTime
            // 
            this.EndDateTime.CustomFormat = "MM\'/\'dd\'/\'yyyy hh\':\'mm\':\'ss tt ";
            this.EndDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDateTime.Location = new System.Drawing.Point(107, 44);
            this.EndDateTime.Name = "EndDateTime";
            this.EndDateTime.Size = new System.Drawing.Size(174, 20);
            this.EndDateTime.TabIndex = 2;
            this.EndDateTime.ValueChanged += new System.EventHandler(this.EndDateTime_ValueChanged);
            // 
            // UTC
            // 
            this.UTC.AutoSize = true;
            this.UTC.Location = new System.Drawing.Point(6, 30);
            this.UTC.Name = "UTC";
            this.UTC.Size = new System.Drawing.Size(48, 17);
            this.UTC.TabIndex = 0;
            this.UTC.Text = "UTC";
            this.toolTip1.SetToolTip(this.UTC, "Show the begin and end times in Coordinated Universal Time.");
            this.UTC.UseVisualStyleBackColor = true;
            this.UTC.CheckedChanged += new System.EventHandler(this.UTC_CheckedChanged);
            // 
            // BeginPartitionKey
            // 
            this.BeginPartitionKey.Enabled = false;
            this.BeginPartitionKey.Location = new System.Drawing.Point(287, 18);
            this.BeginPartitionKey.Name = "BeginPartitionKey";
            this.BeginPartitionKey.Size = new System.Drawing.Size(186, 20);
            this.BeginPartitionKey.TabIndex = 10;
            // 
            // EndPartitionKey
            // 
            this.EndPartitionKey.Enabled = false;
            this.EndPartitionKey.Location = new System.Drawing.Point(287, 44);
            this.EndPartitionKey.Name = "EndPartitionKey";
            this.EndPartitionKey.Size = new System.Drawing.Size(186, 20);
            this.EndPartitionKey.TabIndex = 11;
            // 
            // cbFilters
            // 
            this.cbFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFilters.FormattingEnabled = true;
            this.cbFilters.Location = new System.Drawing.Point(6, 45);
            this.cbFilters.Name = "cbFilters";
            this.cbFilters.Size = new System.Drawing.Size(764, 139);
            this.cbFilters.TabIndex = 3;
            this.toolTip1.SetToolTip(this.cbFilters, "Checked filters will be used in a WHERE clause in the order they appear.");
            this.cbFilters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbFilters_ItemCheck);
            this.cbFilters.SelectedIndexChanged += new System.EventHandler(this.cbFilters_SelectedIndexChanged);
            // 
            // tbFilter
            // 
            this.tbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilter.Location = new System.Drawing.Point(6, 19);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(732, 20);
            this.tbFilter.TabIndex = 0;
            this.toolTip1.SetToolTip(this.tbFilter, "Enter a SQL Expression you\'d like to use as a filter in a WHERE clause");
            // 
            // btAdd
            // 
            this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAdd.Image = ((System.Drawing.Image)(resources.GetObject("btAdd.Image")));
            this.btAdd.Location = new System.Drawing.Point(744, 16);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(26, 23);
            this.btAdd.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btAdd, "Remember this filter");
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // btDelete
            // 
            this.btDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDelete.Image = ((System.Drawing.Image)(resources.GetObject("btDelete.Image")));
            this.btDelete.Location = new System.Drawing.Point(776, 16);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(26, 23);
            this.btDelete.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btDelete, "Forget this filter");
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btUp
            // 
            this.btUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btUp.Image = ((System.Drawing.Image)(resources.GetObject("btUp.Image")));
            this.btUp.Location = new System.Drawing.Point(776, 126);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(26, 26);
            this.btUp.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btUp, "Move the selected filter up.");
            this.btUp.UseVisualStyleBackColor = true;
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            // 
            // btDown
            // 
            this.btDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDown.Image = ((System.Drawing.Image)(resources.GetObject("btDown.Image")));
            this.btDown.Location = new System.Drawing.Point(776, 158);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(26, 26);
            this.btDown.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btDown, "Move the selected filter down.");
            this.btDown.UseVisualStyleBackColor = true;
            this.btDown.Click += new System.EventHandler(this.btDown_Click);
            // 
            // btnApplySqlFilters
            // 
            this.btnApplySqlFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplySqlFilters.Image = ((System.Drawing.Image)(resources.GetObject("btnApplySqlFilters.Image")));
            this.btnApplySqlFilters.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnApplySqlFilters.Location = new System.Drawing.Point(661, 260);
            this.btnApplySqlFilters.Name = "btnApplySqlFilters";
            this.btnApplySqlFilters.Size = new System.Drawing.Size(124, 32);
            this.btnApplySqlFilters.TabIndex = 6;
            this.btnApplySqlFilters.Text = "Apply SQL Filters";
            this.btnApplySqlFilters.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApplySqlFilters.UseVisualStyleBackColor = true;
            this.btnApplySqlFilters.Click += new System.EventHandler(this.btnApplySqlFilters_Click);
            // 
            // azureBackgroundWorker
            // 
            this.azureBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.azureBackgroundWorker_DoWork);
            this.azureBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.azureBackgroundWorker_RunWorkerCompleted);
            // 
            // grpAzure
            // 
            this.grpAzure.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAzure.Controls.Add(this.grpTimeframe);
            this.grpAzure.Controls.Add(this.grpFetchTo);
            this.grpAzure.Controls.Add(this.RecordsCached);
            this.grpAzure.Controls.Add(this.label7);
            this.grpAzure.Controls.Add(this.ConnectionNickname);
            this.grpAzure.Controls.Add(this.label1);
            this.grpAzure.Controls.Add(this.ConnectionString);
            this.grpAzure.Controls.Add(this.label2);
            this.grpAzure.Controls.Add(this.RememberConnection);
            this.grpAzure.Controls.Add(this.ForgetConnection);
            this.grpAzure.Controls.Add(this.btnFetchFromAzure);
            this.grpAzure.Location = new System.Drawing.Point(12, 12);
            this.grpAzure.Name = "grpAzure";
            this.grpAzure.Size = new System.Drawing.Size(808, 154);
            this.grpAzure.TabIndex = 0;
            this.grpAzure.TabStop = false;
            this.grpAzure.Text = "Step 1: Fetch from Azure";
            // 
            // grpTimeframe
            // 
            this.grpTimeframe.Controls.Add(this.UTC);
            this.grpTimeframe.Controls.Add(this.BeginDateTime);
            this.grpTimeframe.Controls.Add(this.BeginPartitionKey);
            this.grpTimeframe.Controls.Add(this.label3);
            this.grpTimeframe.Controls.Add(this.label4);
            this.grpTimeframe.Controls.Add(this.EndDateTime);
            this.grpTimeframe.Controls.Add(this.EndPartitionKey);
            this.grpTimeframe.Location = new System.Drawing.Point(9, 66);
            this.grpTimeframe.Name = "grpTimeframe";
            this.grpTimeframe.Size = new System.Drawing.Size(480, 77);
            this.grpTimeframe.TabIndex = 4;
            this.grpTimeframe.TabStop = false;
            this.grpTimeframe.Text = "Timeframe";
            this.toolTip1.SetToolTip(this.grpTimeframe, "The timeframe you\'d like to fetch logs for");
            // 
            // grpFetchTo
            // 
            this.grpFetchTo.Controls.Add(this.FetchToDisk);
            this.grpFetchTo.Controls.Add(this.FetchToMemory);
            this.grpFetchTo.Location = new System.Drawing.Point(495, 67);
            this.grpFetchTo.Name = "grpFetchTo";
            this.grpFetchTo.Size = new System.Drawing.Size(74, 76);
            this.grpFetchTo.TabIndex = 5;
            this.grpFetchTo.TabStop = false;
            this.grpFetchTo.Text = "Fetch to";
            this.toolTip1.SetToolTip(this.grpFetchTo, "Where do you want to temporarily cache the logs you fetch?");
            // 
            // FetchToDisk
            // 
            this.FetchToDisk.AutoSize = true;
            this.FetchToDisk.Location = new System.Drawing.Point(6, 42);
            this.FetchToDisk.Name = "FetchToDisk";
            this.FetchToDisk.Size = new System.Drawing.Size(46, 17);
            this.FetchToDisk.TabIndex = 1;
            this.FetchToDisk.TabStop = true;
            this.FetchToDisk.Text = "Disk";
            this.toolTip1.SetToolTip(this.FetchToDisk, "Caching on disk is slower than caching in memory, but space is usually plentiful." +
        "  Recommended for large timeframes.");
            this.FetchToDisk.UseVisualStyleBackColor = true;
            // 
            // FetchToMemory
            // 
            this.FetchToMemory.AutoSize = true;
            this.FetchToMemory.Location = new System.Drawing.Point(6, 19);
            this.FetchToMemory.Name = "FetchToMemory";
            this.FetchToMemory.Size = new System.Drawing.Size(62, 17);
            this.FetchToMemory.TabIndex = 0;
            this.FetchToMemory.TabStop = true;
            this.FetchToMemory.Text = "Memory";
            this.toolTip1.SetToolTip(this.FetchToMemory, "Caching in memory is fast, but space is limited.  Recommended for small timeframe" +
        "s.");
            this.FetchToMemory.UseVisualStyleBackColor = true;
            // 
            // RecordsCached
            // 
            this.RecordsCached.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RecordsCached.Location = new System.Drawing.Point(657, 125);
            this.RecordsCached.Name = "RecordsCached";
            this.RecordsCached.ReadOnly = true;
            this.RecordsCached.Size = new System.Drawing.Size(132, 20);
            this.RecordsCached.TabIndex = 2;
            this.RecordsCached.TabStop = false;
            this.RecordsCached.Text = "0";
            this.RecordsCached.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.RecordsCached, "Number of records currently in the SQLite cache");
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(579, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Rows cached";
            // 
            // grpLocalSqlFilters
            // 
            this.grpLocalSqlFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLocalSqlFilters.Controls.Add(this.label6);
            this.grpLocalSqlFilters.Controls.Add(this.sqlComputed);
            this.grpLocalSqlFilters.Controls.Add(this.label5);
            this.grpLocalSqlFilters.Controls.Add(this.sqliteHelp);
            this.grpLocalSqlFilters.Controls.Add(this.tbFilter);
            this.grpLocalSqlFilters.Controls.Add(this.cbFilters);
            this.grpLocalSqlFilters.Controls.Add(this.btnApplySqlFilters);
            this.grpLocalSqlFilters.Controls.Add(this.btAdd);
            this.grpLocalSqlFilters.Controls.Add(this.btDelete);
            this.grpLocalSqlFilters.Controls.Add(this.btDown);
            this.grpLocalSqlFilters.Controls.Add(this.btUp);
            this.grpLocalSqlFilters.Location = new System.Drawing.Point(12, 184);
            this.grpLocalSqlFilters.Name = "grpLocalSqlFilters";
            this.grpLocalSqlFilters.Size = new System.Drawing.Size(808, 334);
            this.grpLocalSqlFilters.TabIndex = 1;
            this.grpLocalSqlFilters.TabStop = false;
            this.grpLocalSqlFilters.Text = "Step 2: Apply SQL Filters";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 212);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "SQL that will be applied";
            // 
            // sqlComputed
            // 
            this.sqlComputed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sqlComputed.Location = new System.Drawing.Point(6, 228);
            this.sqlComputed.Multiline = true;
            this.sqlComputed.Name = "sqlComputed";
            this.sqlComputed.ReadOnly = true;
            this.sqlComputed.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.sqlComputed.Size = new System.Drawing.Size(646, 97);
            this.sqlComputed.TabIndex = 25;
            this.sqlComputed.TabStop = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(373, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "The checked expression filters will be ANDed together in the WHERE clause.";
            // 
            // sqliteHelp
            // 
            this.sqliteHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sqliteHelp.AutoSize = true;
            this.sqliteHelp.Location = new System.Drawing.Point(645, 189);
            this.sqliteHelp.Name = "sqliteHelp";
            this.sqliteHelp.Size = new System.Drawing.Size(125, 13);
            this.sqliteHelp.TabIndex = 23;
            this.sqliteHelp.TabStop = true;
            this.sqliteHelp.Text = "SQLite expression syntax";
            this.sqliteHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.sqliteHelp_LinkClicked);
            // 
            // sqlBackgroundWorker
            // 
            this.sqlBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.sqlBackgroundWorker_DoWork);
            this.sqlBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.sqlBackgroundWorker_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 528);
            this.Controls.Add(this.grpLocalSqlFilters);
            this.Controls.Add(this.grpAzure);
            this.MinimumSize = new System.Drawing.Size(765, 484);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpAzure.ResumeLayout(false);
            this.grpAzure.PerformLayout();
            this.grpTimeframe.ResumeLayout(false);
            this.grpTimeframe.PerformLayout();
            this.grpFetchTo.ResumeLayout(false);
            this.grpFetchTo.PerformLayout();
            this.grpLocalSqlFilters.ResumeLayout(false);
            this.grpLocalSqlFilters.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ConnectionNickname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ConnectionString;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button RememberConnection;
        private System.Windows.Forms.Button ForgetConnection;
        private System.Windows.Forms.Button btnFetchFromAzure;
        private System.Windows.Forms.DateTimePicker BeginDateTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker EndDateTime;
        private System.Windows.Forms.CheckBox UTC;
        private System.Windows.Forms.TextBox BeginPartitionKey;
        private System.Windows.Forms.TextBox EndPartitionKey;
        private System.Windows.Forms.CheckedListBox cbFilters;
        private System.Windows.Forms.TextBox tbFilter;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.Button btnApplySqlFilters;
        private System.ComponentModel.BackgroundWorker azureBackgroundWorker;
        private System.Windows.Forms.GroupBox grpAzure;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox grpLocalSqlFilters;
        private System.Windows.Forms.LinkLabel sqliteHelp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox sqlComputed;
        private System.ComponentModel.BackgroundWorker sqlBackgroundWorker;
        private System.Windows.Forms.TextBox RecordsCached;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox grpFetchTo;
        private System.Windows.Forms.RadioButton FetchToDisk;
        private System.Windows.Forms.RadioButton FetchToMemory;
        private System.Windows.Forms.GroupBox grpTimeframe;
    }
}

