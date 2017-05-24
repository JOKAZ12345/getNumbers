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
    public partial class Pessoas : Form
    {
        public Pessoas()
        {
            InitializeComponent();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var db = new prabitarDataContext();

            var pessoas = db.Pessoas;

            var nome = "";
            if (!string.IsNullOrEmpty(textBox5.Text))
                nome = textBox5.Text.ToLower();

            var BI = 0;
            if (!string.IsNullOrEmpty(textBox6.Text))
                BI = Convert.ToInt32(textBox6.Text);

            var telefone = 0;
            if (!string.IsNullOrEmpty(textBox7.Text))
                telefone = Convert.ToInt32(textBox7.Text);

            var email = "";
            if (!string.IsNullOrEmpty(textBox8.Text))
                email = textBox8.Text.ToLower();

            var morada = "";
            if (!string.IsNullOrEmpty(textBox9.Text))
                morada = textBox9.Text.ToLower();

            //TODO: Remover do nome os "do", "de", "da"
            var pessoa = pessoas.FirstOrDefault(x => x.Telefone == telefone ||
                                                           x.BI == BI || x.Nome.ToLower().Equals(nome) ||
                                                           x.Email.ToLower().Equals(email) ||
                                                           x.Morada.ToLower().Equals(morada));

            if (pessoa != null)
            {
                textBox5.Text = pessoa.Nome;
                textBox6.Text = pessoa.BI.ToString();
                textBox7.Text = pessoa.Telefone.ToString();
                textBox8.Text = pessoa.Email;
                textBox9.Text = pessoa.Morada;
                textBox10.Text = pessoa.DataNascimento.ToString();
                    // TODO: Atenção não tou a pesquisar pela data nascimento
                textBox11.Text = pessoa.NIF.ToString(); // TODO: Atenção não tou a pesquisar pelo NIF
            }

            else
            {
                var Pessoa = new Pessoa();

                Pessoa.Nome = textBox5.Text;
                Pessoa.BI = Convert.ToInt32(textBox6.Text);
                Pessoa.Telefone = Convert.ToInt32(textBox7.Text);

            }
                MessageBox.Show("Não existe!");
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click_1(object sender, EventArgs e)
        {

        }
    }
}
