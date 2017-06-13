using System;
using System.Collections;
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

        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
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

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var db = new prabitarDataContext();

            var x = dataGridView1.CurrentCell?.Value;

            var cell = dataGridView1.CurrentCell;

            if(cell!=null && x!=null)
            {
                var col = cell.ColumnIndex;
                var lin = cell.RowIndex;

                int id = Convert.ToInt32(dataGridView1.Rows[lin].Cells[0].Value); // id coluna

                string headerColumn = cell.OwningColumn.HeaderText;

                if (headerColumn.Equals("Angariador"))
                {
                    var row = db.Potencials.FirstOrDefault(r => r.Candidato_ID == id);

                    if (row != null)
                        row.Angariador = x.ToString();
                }

                else if (headerColumn.Equals("Descricao"))
                {
                    var row = db.Potencials.FirstOrDefault(r => r.Candidato_ID == id);

                    if (row != null)
                        row.Descricao = x.ToString();
                }
            }

            db.SubmitChanges();
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
            /*var x = dataGridView1.DataSource as DataTable;
            var dv = new DataView(x);
            dv.RowFilter = string.Format("Telefone LIKE '%{0}%'", textBox1.Text);
            prabitarDataSet.Potencial.DefaultView.RowFilter = dv.RowFilter;
            dataGridView1.DataSource = dv.Table;*/



            FilterDataGrid();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            /*BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView1.DataSource;
            bs.Filter = "TituloAnuncio Like '%" + textBox2.Text + "%'";
            dataGridView1.DataSource = bs;*/

            FilterDataGrid();
        }

        private void FilterDataGrid()
        {
            string tele = "";
            string titu = "";

            if (!string.IsNullOrEmpty(textBox1.Text))
                tele = textBox1.Text;

            if (!string.IsNullOrEmpty(textBox2.Text))
                titu = textBox2.Text;

            int lower = 0;
            if (!string.IsNullOrEmpty(textBox3.Text))
                lower = Convert.ToInt32(textBox3.Text);

            int upper = 0;
            if (!string.IsNullOrEmpty(textBox4.Text))
                upper = Convert.ToInt32(textBox4.Text);

            string filterPreco;

            if (upper == 0)
                filterPreco = "Preco >=" + lower;
            else
                filterPreco = "Preco >= " + lower + " and Preco <= " + upper;

            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView1.DataSource;
            var filter = "Telefone Like '%" + tele + "%'" + " and TituloAnuncio Like '%" + titu + "%'" + " and " + filterPreco;
            bs.Filter = filter;
            dataGridView1.DataSource = bs;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            FilterDataGrid();
        }
    }
}
