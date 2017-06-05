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
    public partial class Imobiliarias : Form
    {
        public Imobiliarias()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var db = new prabitarDataContext();

            var imobiliarias = db.Agencias;

            foreach (var imobiliaria in imobiliarias)
            {
                if (textBox2.Text != imobiliaria.Telefone) continue;
                textBox1.Text = imobiliaria.Nome;
                textBox3.Text = imobiliaria.URL;
                textBox4.Text = imobiliaria.url_idealista;
                return;
            }

            MessageBox.Show("Essa agência não existe na base de dados!", "Contacto errado?!", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var db = new prabitarDataContext();

            // Todo: Eu por enquanto deixo adicionar agências repetidas
            if (!string.IsNullOrEmpty(textBox2.Text) 
                | !string.IsNullOrEmpty(textBox1.Text))
            {
                var a = new Agencia
                {
                    Nome = textBox1.Text,
                    Telefone = textBox2.Text,
                    URL = textBox3.Text,
                    url_idealista = textBox4.Text
                };

                db.Agencias.InsertOnSubmit(a);
                db.SubmitChanges();

                MessageBox.Show("Adicionado!!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
