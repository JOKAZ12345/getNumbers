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
    }
}
