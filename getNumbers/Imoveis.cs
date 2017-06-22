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
    public partial class Imoveis : Form
    {
        public Imoveis()
        {
            InitializeComponent();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var db = new prabitarDataSet();
            var pendentes = db.Pendentes;
            var pessoas = db.Pessoa;

            var morada = textBox1.Text;
            var concelho = textBox2.Text;
            var freguesia = textBox3.Text;
            var cp = textBox4.Text;

            var tipo = textBox5.Text;
            var caracterisitcas = textBox6.Text;
            var ce = textBox7.Text;
            var dataConstrucao = textBox8.Text;
            var preco = textBox17.Text;

            var notas = textBox16.Text;

            var proprietario = pessoas.FirstOrDefault(x => x.BI == Convert.ToInt32(textBox12.Text));

            if (proprietario != null)
            {
                textBox9.Text = proprietario.Nome;
                textBox13.Text = proprietario.Morada;
                //textBox10.Text = proprietario.ci
                textBox12.Text = proprietario.BI.ToString();
                textBox11.Text = proprietario.NIF.ToString();
                textBox14.Text = proprietario.Telefone.ToString();
                textBox15.Text = proprietario.Email;
            }

            else
            {
                var nome = textBox9.Text;
                var moradaProprietario = textBox13.Text;
                var estadoCivil = textBox10.Text;
                var BI = textBox12.Text;
                var NIF = textBox11.Text;
                var telefone = textBox14.Text;
                var email = textBox15.Text;
            }

            var imovel = new Pendente()
            {
                Localizacao = morada,
                Concelho = concelho,
                Freguesia = freguesia,

            };
        }
    }
}
