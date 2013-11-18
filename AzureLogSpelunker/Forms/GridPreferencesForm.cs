using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AzureLogSpelunker.Models;

namespace AzureLogSpelunker.Forms
{
    public partial class GridPreferencesForm : Form
    {
        private readonly ISettings _settings = new Settings();

        public GridPreferencesForm()
        {
            InitializeComponent();
        }

        private void GridPreferencesForm_Load(object sender, EventArgs e)
        {
            LoadColumns();
            cbWordwrap.Checked = _settings.GetWordwrap();
        }

        private void GridPreferencesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateColumns();
            _settings.SetWordwrap(cbWordwrap.Checked);
        }

        #region Columns
        private void LoadColumns()
        {
            var columns = _settings.GetColumns();
            columnLayout.Items.Clear();
            foreach (var column in columns)
            {
                columnLayout.Items.Add(column.ColumnName, column.Active);
            }
            UpdateColumns();
        }

        private void btUp_Click(object sender, EventArgs e)
        {
            var thisIndex = columnLayout.SelectedIndex;
            if (thisIndex > 0)
            {
                var thisItem = columnLayout.SelectedItem;
                var thisCheckedState = columnLayout.GetItemCheckState(thisIndex);
                columnLayout.Items[thisIndex] = columnLayout.Items[thisIndex - 1];
                columnLayout.SetItemCheckState(thisIndex, columnLayout.GetItemCheckState(thisIndex - 1));
                columnLayout.Items[thisIndex - 1] = thisItem;
                columnLayout.SetItemCheckState(thisIndex - 1, thisCheckedState);
                columnLayout.SelectedIndex -= 1;
                UpdateColumns();
            }
        }

        private void btDown_Click(object sender, EventArgs e)
        {
            var thisIndex = columnLayout.SelectedIndex;
            if (thisIndex != -1 && thisIndex < columnLayout.Items.Count - 1)
            {
                var thisItem = columnLayout.SelectedItem;
                var thisCheckedState = columnLayout.GetItemCheckState(thisIndex);
                columnLayout.Items[thisIndex] = columnLayout.Items[thisIndex + 1];
                columnLayout.SetItemCheckState(thisIndex, columnLayout.GetItemCheckState(thisIndex + 1));
                columnLayout.Items[thisIndex + 1] = thisItem;
                columnLayout.SetItemCheckState(thisIndex + 1, thisCheckedState);
                columnLayout.SelectedIndex += 1;
                UpdateColumns();
            }
        }

        private void UpdateColumns(int changingIndex = -1, CheckState changingCheckState = CheckState.Indeterminate)
        {
            var columnList = new List<Column>();
            columnLayout.Enabled = false;
            for (var i = 0; i < columnLayout.Items.Count; i++)
            {
                columnList.Add(new Column
                {
                    ColumnName = columnLayout.Items[i].ToString(),
                    Active = i != changingIndex ? columnLayout.GetItemCheckState(i) == CheckState.Checked : changingCheckState == CheckState.Checked,
                    Position = i
                });
            }
            columnLayout.Enabled = true;
            _settings.UpdateColumns(columnList);
        }
        #endregion Columns


    }
}
