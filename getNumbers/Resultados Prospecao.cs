using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace getNumbers
{
    public partial class Resultados_Prospecao : Form
    {
        public Resultados_Prospecao()
        {
            InitializeComponent();
        }

        private void Resultados_Prospecao_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'prabitarDataSet.Potencial' table. You can move, or remove it, as needed.
            this.potencialTableAdapter.Fill(this.prabitarDataSet.Potencial);

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.B && dataGridView1.CurrentCell.ColumnIndex == 1)
            {
                e.Handled = true;
                DataGridViewCell cell = dataGridView1.Rows[0].Cells[0];
                dataGridView1.CurrentCell = cell;
                dataGridView1.BeginEdit(true);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentCell.ReadOnly = false;
            dataGridView1.BeginEdit(true);
        }

        private void dataGridView1_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentCell.ReadOnly = false;
            dataGridView1.BeginEdit(true);
        }

        private void potencialBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //((DataTable) dataGridView1.DataSource).DefaultView.RowFilter = string.Format("Telefone = '{0}'", "123");
            //prabitarDataSet.Potencial.DefaultView.RowFilter = "Telefone LIKE '9627%'";
            //prabitarDataSet.Tables["Potencial"].DefaultView.RowFilter = "Telefone LIKE '%96%'";
            //prabitarDataSet.Potencial.DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", "Telefone", textBox1.Text);
           // DataView dv = new DataView(dataGridView1)

            //var dt = (DataTable) potencialBindingSource.DataSource;
            var x = dataGridView1.DataSource as DataTable;
            var dv = new DataView(x);
            dv.RowFilter = string.Format("Telefone LIKE '%{0}%'", textBox1.Text);
            prabitarDataSet.Potencial.DefaultView.RowFilter = dv.RowFilter;
            dataGridView1.DataSource = dv.Table;
        }
    }
}
