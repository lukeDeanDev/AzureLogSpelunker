using System;
using System.Data;
using System.Windows.Forms;

namespace AzureLogSpelunker.Forms
{
    public partial class DisplayForm : Form
    {
        private readonly BindingSource _bindingSource = new BindingSource();
        private readonly ISettings _settings = new Settings();
        private DataTable _currentDataTable;

        public DisplayForm()
        {
            InitializeComponent();
        }

        public void PopulateGrid(DataTable dataTable)
        {
            _currentDataTable = dataTable;
            UseWaitCursor = true;
            var grid = dataGridView1;

            grid.AutoGenerateColumns = false;

            _bindingSource.DataSource = dataTable;
            grid.DataSource = _bindingSource;
            grid.EditMode = DataGridViewEditMode.EditProgrammatically;

            if (grid.Columns.Count == 0)
                ResetGridColumns();

            UseWaitCursor = false;
        }

        public void ResetGridColumns()
        {
            var grid = dataGridView1;
            grid.Columns.Clear();
            var columns = _settings.GetColumns();
            foreach (var column in columns)
            {
                if (column.Active)
                {
                    var dataGridViewColumn = new DataGridViewTextBoxColumn
                        {
                            DataPropertyName = column.ColumnName,
                            HeaderText = column.ColumnName,
                            Resizable = DataGridViewTriState.True,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                            DefaultCellStyle =
                                {
                                    WrapMode = _settings.GetWordwrap() ? DataGridViewTriState.True : DataGridViewTriState.False
                                }
                        };
                    grid.Columns.Add(dataGridViewColumn);
                }
            }
            grid.AutoResizeColumns();
        }

        private void DisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void btnPreferences_Click(object sender, EventArgs e)
        {
            new GridPreferencesForm().ShowDialog();
            ResetGridColumns();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
                {
                    Filter = ("xml files|*.xml"),
                    FilterIndex = 0,
                    InitialDirectory = _settings.GetLastExportPath()
                };
            if (saveFileDialog.InitialDirectory == "")
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _currentDataTable.WriteXml(saveFileDialog.FileName);
                    _settings.SetLastExportPath(System.IO.Path.GetDirectoryName(saveFileDialog.FileName));
                }
            }
            catch (Exception ex)
            {
                MainForm.MyMessageBox(ex.ToString());
            }
        }

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //        MyMessageBox(dataGridView1["Message", e.RowIndex].FormattedValue as string);
        //}

    }
}
