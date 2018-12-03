using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;

namespace DigitalFenceMonitor
{
    public partial class FormAlertSearch : Form
    {
        public FormAlertSearch()
        {
            InitializeComponent();
        }
        OleDbConnection conn = FormMain.conn;
        private void AlertSearch_Load(object sender, EventArgs e)
        {
            if (FormMain.NowUser=="admin")
                but_clear.Enabled = true;
            ReLoad();
        }

        private void ReLoad()
        {
            

            OleDbDataAdapter alertada = null;
            DataTable datatable = new DataTable();
            BindingSource bindingsrc = new BindingSource();
            OleDbCommandBuilder commandbld = null;

            string sql = "select * from tb_Content";

            alertada = new OleDbDataAdapter(sql, conn);
            dataGridView.DataSource = datatable;
            bindingsrc.DataSource = datatable;
            commandbld = new OleDbCommandBuilder(alertada);
            alertada.Fill(datatable);

            dataGridView.Columns["NowDate"].HeaderText = "Date";
            dataGridView.Columns["NowTime"].HeaderText = "Time";
            dataGridView.Columns["StationIP"].HeaderText = "BaseSta IP";
            dataGridView.Columns["StationName"].HeaderText = "BaseSta Name";
            dataGridView.Columns["AreaNum"].HeaderText = "Zone Number";

            dataGridView.Columns["AlertType"].HeaderText = "Info Type";
            dataGridView.Columns["Descripe"].HeaderText = "Location";
            dataGridView.Columns["OperationInfo"].HeaderText = "Operation information";
            dataGridView.Columns["Operator"].HeaderText = "Operator";
            dataGridView.Columns["OperationResult"].HeaderText = "Process result";

            dataGridView.Columns[0].Visible = false;

            dataGridView.Columns[1].Width = 80;
            dataGridView.Columns[2].Width = 80;
            dataGridView.Columns[5].Width = 75;
        }

        private void but_clear_Click(object sender, EventArgs e)
        {
            string sql = "delete from tb_Content";
            OleDbCommand comm = new OleDbCommand(sql, conn);
            comm.ExecuteNonQuery();

            DataTable datatable = null;
            dataGridView.DataSource = datatable;
        }

        private void btn_output_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Start generating exported data", "Generation HInt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Excel.Application excel = new Excel.Application();
            excel.Application.Workbooks.Add(true);
            excel.Visible = false;
            for (int i = 0; i < dataGridView.ColumnCount; i++)
                excel.Cells[1, i + 1] = dataGridView.Columns[i].HeaderText;

            for (int i = 0; i < dataGridView.RowCount; i++)
            {
                for (int j = 0; j < dataGridView.ColumnCount; j++)
                {
                    if (dataGridView[j, i].ValueType == typeof(string))
                    {
                        excel.Cells[i + 2, j + 1] = "'" + dataGridView[j, i].Value.ToString();
                    }
                    else
                    {
                        excel.Cells[i + 2, j + 1] = dataGridView[j, i].Value.ToString();
                    }
                }

            }

            MessageBox.Show("Generation success, please save the file.", "Generation HInt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            excel.Visible = true;
        }
    }
}
