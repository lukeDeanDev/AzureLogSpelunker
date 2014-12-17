using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using AzureLogSpelunker.Models;

namespace AzureLogSpelunker.Forms
{
    public partial class MainForm : Form
    {
        private readonly ISettings _settings = new Settings();
        private readonly ISqlCache _sqlCache = new SqlCache();
        private readonly DisplayForm _displayForm = new DisplayForm();
        private string _quickFilter = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = Application.ProductName;
            RefreshConnection();
            var now = DateTime.Now;
            BeginDateTime.Value = now.AddHours(-1);
            EndDateTime.Value = now;
            LoadFilters();
            UTC.Checked = _settings.GetUtc();
            ConnectionNickname.Text = _settings.GetLastConnectionNickname();
            InitializeFetchTo();
            sqliteHelp.Links.Add(0, sqliteHelp.Text.Length, "http://sqlite.org/lang_expr.html");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _sqlCache.Close();
            UpdateFilters();
            _settings.SetUtc(UTC.Checked);
            _settings.SetLastConnectionNickname(ConnectionNickname.Text);
            SaveFetchTo();
        }

        #region AzureControls

        private void ResetTableNameCombo()
        {
            cmbTableNames.Items.Clear();
            cmbTableNames.Items.Add("WADLogsTable");
            cmbTableNames.SelectedItem = cmbTableNames.Items[0];
        }

        private void ConnectionNickname_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectionString.Text = _settings.GetConnectionString(ConnectionNickname.Text);
        }

        private void RememberConnection_Click(object sender, EventArgs e)
        {
            _settings.SetConnectionString(ConnectionNickname.Text, ConnectionString.Text);
            RefreshConnection(ConnectionNickname.Text);
        }

        private void ForgetConnection_Click(object sender, EventArgs e)
        {
            _settings.DeleteConnectionString(ConnectionNickname.Text);
            ConnectionNickname.ResetText();
            RefreshConnection();
        }

        private void RefreshConnection(string currentText = null)
        {
            ConnectionNickname.DataSource = _settings.GetConnectionStrings();
            ConnectionNickname.SelectedIndex = ConnectionNickname.FindStringExact(currentText);
            ConnectionString.Text = _settings.GetConnectionString(ConnectionNickname.Text);
        }

        private void UTC_CheckedChanged(object sender, EventArgs e)
        {
            if (UTC.Checked)
            {
                BeginDateTime.Value = BeginDateTime.Value.ToUniversalTime();
                EndDateTime.Value = EndDateTime.Value.ToUniversalTime();
            }
            else
            {
                BeginDateTime.Value = BeginDateTime.Value.ToLocalTime();
                EndDateTime.Value = EndDateTime.Value.ToLocalTime();
            }
        }

        private void BeginDateTime_ValueChanged(object sender, EventArgs e)
        {
            BeginPartitionKey.Text = TableStorage.UtcDateTimeToPartitionKey(UTC.Checked ? BeginDateTime.Value : BeginDateTime.Value.ToUniversalTime());
        }

        private void EndDateTime_ValueChanged(object sender, EventArgs e)
        {
            EndPartitionKey.Text = TableStorage.UtcDateTimeToPartitionKey(UTC.Checked ? EndDateTime.Value : EndDateTime.Value.ToUniversalTime());
        }

        private void InitializeFetchTo()
        {
            var fetchTo = _settings.GetFetchTo();
            switch (fetchTo)
            {
                case "disk":
                    FetchToDisk.Checked = true;
                    break;
                default:
                    FetchToMemory.Checked = true;
                    break;
            }
        }

        private void SaveFetchTo()
        {
            foreach (var control in grpFetchTo.Controls)
            {
                var radioButton = control as RadioButton;
                if (radioButton != null && radioButton.Checked)
                {
                    _settings.SetFetchTo(radioButton.Text.ToLowerInvariant());
                    break;
                }
            }
        }
        #endregion AzureControls

        #region AzureFetch
        private void btnFetchFromAzure_Click(object sender, EventArgs e)
        {
            DisableGroups();
            var model = new AzureQueryModel
            {
                ConnectionString = ConnectionString.Text,
                TableName = cmbTableNames.Text,
                BeginPartitionKey = BeginPartitionKey.Text,
                EndPartitionKey = EndPartitionKey.Text
            };
            azureBackgroundWorker.RunWorkerAsync(model);
        }

        private void azureBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var model = (AzureQueryModel)e.Argument;
                model.ResultSet = TableStorage.Go(model.ConnectionString, model.TableName, model.BeginPartitionKey, model.EndPartitionKey);
                e.Result = model;
            }
            catch (Exception ex)
            {
                MyMessageBox(ex.ToString());
            }
        }

        private void azureBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var model = e.Result as AzureQueryModel;
            if (model != null)
            {
                var tableResultSet = model.ResultSet;
                SaveFetchTo();
                RecordsCached.Text = _sqlCache.PopulateCache(tableResultSet, _settings).ToString(CultureInfo.InvariantCulture);
                var dataTable = TableStorage.MakeDataTable<LogEntity>();
                TableStorage.FillDataTable(dataTable, tableResultSet);
                //PopulateGrid(dataTable);
            }
            EnableGroups();
        }
        #endregion AzureFetch

        #region ApplySql

        private class WorkArgument
        {
            public DataTable DataTable { get; set; }
            public string QuickFilter { get; set; }
        }

        private void btnApplySqlFilters_Click(object sender, EventArgs e)
        {
            UpdateFilters();
            DisableGroups();
            var workArgument = new WorkArgument
            {
                DataTable = TableStorage.MakeDataTable<LogEntity>(),
                QuickFilter = _quickFilter
            };
            sqlBackgroundWorker.RunWorkerAsync(workArgument);
        }

        private void sqlBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var workArgument = e.Argument as WorkArgument;
            try
            {
                _sqlCache.ApplyFilters(workArgument.DataTable, _settings, workArgument.QuickFilter);
            }
            catch (SQLiteException ex)
            {
                MyMessageBox(ex.ToString());
            }
            finally
            {
                e.Result = workArgument;
            }
        }

        private void sqlBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableGroups();
            var dataTable = (e.Result as WorkArgument).DataTable;
            PopulateGrid(dataTable);
        }

        private void PopulateGrid(DataTable dataTable)
        {
            _displayForm.Show();
            _displayForm.BringToFront();
            _displayForm.PopulateGrid(dataTable);
        }
        #endregion ApplySql

        #region Filters
        private void LoadFilters()
        {
            var filters = _settings.GetFilters();
            cbFilters.Items.Clear();
            foreach (var filter in filters)
            {
                cbFilters.Items.Add(filter.FilterText, filter.Active);
            }
            UpdateFilters();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            var thisIndex = cbFilters.SelectedIndex;
            if (thisIndex != -1)
            {
                var rowsAffected = _settings.DeleteFilter(cbFilters.Items[thisIndex].ToString());
                if (rowsAffected > 0)
                    cbFilters.Items.RemoveAt(thisIndex);
                UpdateFilters();
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            var filterText = tbFilter.Text;
            filterText = filterText.Trim();
            if (!String.IsNullOrWhiteSpace(filterText))
            {
                var rowsAffected = _settings.AddFilter(filterText);
                if (rowsAffected == 1)
                {
                    cbFilters.Items.Insert(0, filterText);
                    cbFilters.SetItemChecked(0, true);
                    cbFilters.SelectedIndex = 0;
                }
                if (rowsAffected == 0)
                {
                    cbFilters.SelectedIndex = cbFilters.FindStringExact(filterText);
                }
                UpdateFilters();
            }
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            var thisIndex = cbFilters.SelectedIndex;
            if (thisIndex > 0)
            {
                var thisItem = cbFilters.SelectedItem;
                var thisCheckedState = cbFilters.GetItemCheckState(thisIndex);

                cbFilters.Items.RemoveAt(thisIndex);
                cbFilters.Items.Insert(0, thisItem);

                cbFilters.SetItemCheckState(0, thisCheckedState);
                cbFilters.SelectedIndex = 0;
                UpdateFilters();
            }
        }

        private void btUp_Click(object sender, EventArgs e)
        {
            var thisIndex = cbFilters.SelectedIndex;
            if (thisIndex > 0)
            {
                var thisItem = cbFilters.SelectedItem;
                var thisCheckedState = cbFilters.GetItemCheckState(thisIndex);
                cbFilters.Items[thisIndex] = cbFilters.Items[thisIndex - 1];
                cbFilters.SetItemCheckState(thisIndex, cbFilters.GetItemCheckState(thisIndex - 1));
                cbFilters.Items[thisIndex - 1] = thisItem;
                cbFilters.SetItemCheckState(thisIndex - 1, thisCheckedState);
                cbFilters.SelectedIndex -= 1;
                UpdateFilters();
            }
        }

        private void btDown_Click(object sender, EventArgs e)
        {
            var thisIndex = cbFilters.SelectedIndex;
            if (thisIndex != -1 && thisIndex < cbFilters.Items.Count - 1)
            {
                var thisItem = cbFilters.SelectedItem;
                var thisCheckedState = cbFilters.GetItemCheckState(thisIndex);
                cbFilters.Items[thisIndex] = cbFilters.Items[thisIndex + 1];
                cbFilters.SetItemCheckState(thisIndex, cbFilters.GetItemCheckState(thisIndex + 1));
                cbFilters.Items[thisIndex + 1] = thisItem;
                cbFilters.SetItemCheckState(thisIndex + 1, thisCheckedState);
                cbFilters.SelectedIndex += 1;
                UpdateFilters();
            }
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            var thisIndex = cbFilters.SelectedIndex;
            if (thisIndex != -1 && thisIndex < cbFilters.Items.Count - 1)
            {
                var thisItem = cbFilters.SelectedItem;
                var thisCheckedState = cbFilters.GetItemCheckState(thisIndex);

                cbFilters.Items.RemoveAt(thisIndex);
                cbFilters.Items.Add(thisItem, thisCheckedState);

                cbFilters.SelectedIndex = cbFilters.Items.Count - 1;

                UpdateFilters();
            }
        }

        private void cbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilters.SelectedIndex == -1)
            {
                tbFilter.Clear();
            }
            else
            {
                tbFilter.Text = cbFilters.SelectedItem.ToString();
            }
        }

        private void cbFilters_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var i = e.Index;
            if (i > -1)
            {
                UpdateFilters(e.Index, e.NewValue);
            }
        }

        private void UpdateFilters(int changingIndex = -1, CheckState changingCheckState = CheckState.Indeterminate)
        {
            var filterList = new List<Filter>();
            cbFilters.Enabled = false;
            for (var i = 0; i < cbFilters.Items.Count; i++)
            {
                filterList.Add(new Filter
                {
                    FilterText = cbFilters.Items[i].ToString(),
                    Active = i != changingIndex ? cbFilters.GetItemCheckState(i) == CheckState.Checked : changingCheckState == CheckState.Checked,
                    Position = i
                });
            }
            cbFilters.Enabled = true;
            _settings.UpdateFilters(filterList);
            sqlComputed.Text = _sqlCache.ComputeSql(_settings, _quickFilter);
        }

        private void btnQuickFilter_Click(object sender, EventArgs e)
        {
            var filterText = tbFilter.Text;
            // Set the quickFilter
            _quickFilter = filterText.Trim();
            // Run the ApplySql
            btnApplySqlFilters_Click(sender, e);
            // Clear the quickFilter
            _quickFilter = "";
            // Reset the text on the filter edit since it got cleared
            tbFilter.Text = filterText;
        }

        #endregion Filters

        public static void MyMessageBox(string text)
        {
            MessageBox.Show(text, "Remember Ctrl+C copies to the clipboard");
        }

        private void sqliteHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start((string)e.Link.LinkData);
        }

        private void DisableGroups()
        {
            UseWaitCursor = true;
            grpAzure.Enabled = false;
            grpLocalSqlFilters.Enabled = false;
        }

        private void EnableGroups()
        {
            grpAzure.Enabled = true;
            grpLocalSqlFilters.Enabled = true;
            UseWaitCursor = false;
        }

        private void cmbTableNames_DropDown(object sender, EventArgs e)
        {
            var oldCursor = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string oldText = cmbTableNames.Text;
                cmbTableNames.Items.Clear();
                cmbTableNames.Items.AddRange(TableStorage.GetTableNames(ConnectionString.Text).ToArray());
                // Find the index of the previously set name
                int newIndex = cmbTableNames.Items.IndexOf(oldText);
                cmbTableNames.SelectedItem = newIndex >= 0 ? cmbTableNames.Items[newIndex] : cmbTableNames.Items[0];

            }
            catch (Exception ex)
            {
                cmbTableNames.Items.Clear();
                cmbTableNames.Items.Add(ex.Message);
                cmbTableNames.SelectedItem = cmbTableNames.Items[0];
            }
            finally
            {
                Cursor.Current = oldCursor;
            }
        }

        private void ConnectionString_TextChanged(object sender, EventArgs e)
        {
            ResetTableNameCombo();
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnQuickFilter.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
    }
}

