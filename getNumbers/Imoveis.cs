using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using Geocoding;
using Geocoding.Microsoft;
using Geocoding.MapQuest;
using Geocoding.Google;

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

        private int? getIntegerFromString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;

            return Convert.ToInt32(str);
        }

        private string getValueFromString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "";

            return str;
        }

        private double getFloatFromString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return 0.0;

            return Convert.ToDouble(str);
        }

        private DateTime getDateFromString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return DateTime.Now.Date;

            return DateTime.Parse(str, new System.Globalization.CultureInfo("pt-PT", true), System.Globalization.DateTimeStyles.AssumeLocal).Date;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var db = new prabitarDataContext();
            var pendentes = db.Pendentes;
            var pessoas = db.Pessoas;
            var precoDB = db.Precos;

            var morada = getValueFromString(textBox1.Text);
            var concelho = getValueFromString(textBox2.Text);
            var freguesia = getValueFromString(textBox3.Text);
            var cp = getIntegerFromString(textBox4.Text);

            var tipo = getValueFromString(textBox5.Text);
            var caracterisitcas = getValueFromString(textBox6.Text);
            var ce = getValueFromString(textBox7.Text);
            var dataAngariacao = getDateFromString(textBox8.Text);
            var preco = getIntegerFromString(textBox17.Text);
            var comissao = getIntegerFromString(textBox18.Text);
            var area = getFloatFromString(textBox19.Text);

            var notas = textBox16.Text;

            Pessoa proprietario = null;/* = pessoas.First(x => x.BI == getIntegerFromString(textBox12.Text));*/

            foreach (var p in pessoas)
            {
                var bi = getIntegerFromString(textBox12.Text);

                if (bi == null)
                    break;

                if (p.BI == bi)
                {
                    proprietario = p;
                    break;
                }
            }

            if (proprietario != null)
            {
                textBox9.Text = proprietario.Nome;
                //textBox13.Text = proprietario.Morada;
                //textBox10.Text = proprietario.ci
                textBox12.Text = proprietario.BI.ToString();
                textBox11.Text = proprietario.NIF.ToString();
                textBox14.Text = proprietario.Telefone.ToString();
                textBox15.Text = proprietario.Email;
            }

            else
            {
                var nome = getValueFromString(textBox9.Text);
                var moradaProprietario = getValueFromString(textBox13.Text);
                var estadoCivil = getValueFromString(textBox10.Text);
                var BI = getIntegerFromString(textBox12.Text);
                var NIF = getIntegerFromString(textBox11.Text);
                var telefone = getValueFromString(textBox14.Text);
                var email = getValueFromString(textBox15.Text);

                proprietario = new Pessoa()
                {
                    Nome = nome,
                    BI = BI,
                    NIF = NIF,
                    Telefone = telefone,
                    Email = email
                };

                pessoas.InsertOnSubmit(proprietario);

                db.SubmitChanges();
            }

            /*var coordenadas = await BingGeo(morada);

            string rua = morada;

            try
            {
                rua = morada.Substring(0, morada.IndexOf(" ", morada.IndexOf(" ", morada.IndexOf("nº")) + 1));
            }
            catch (Exception ex)
            {

            }*/

            Pendente imovel = null;

            if (radioButton2.Checked)
            {
                imovel = new Pendente()
                {
                    Localizacao = morada,
                    Concelho = concelho,
                    Freguesia = freguesia,
                    CodigoPostal = cp,
                    Tipo = tipo,
                    Caracteristicas = caracterisitcas,
                    CE = ce,
                    DataAngariacao = dataAngariacao,
                    Vendedor_ID = proprietario.Pessoa_ID,
                    Angariador = comboBox1.Text,
                    Nota = notas,
                    Coordenadas = await BingGeo(morada + " " + concelho),
                    Area = area
                };
            }

            else
            {
                imovel = new Pendente()
                {
                    Localizacao = morada,
                    Concelho = concelho,
                    Freguesia = freguesia,
                    CodigoPostal = cp,
                    Tipo = tipo,
                    Caracteristicas = caracterisitcas,
                    CE = ce,
                    DataAngariacao = dataAngariacao,
                    Arrendatario_ID = proprietario.Pessoa_ID,
                    Angariador = comboBox1.Text,
                    Nota = notas,
                    Coordenadas = await BingGeo(morada + " " + concelho),
                    Area = area
                };
            }

            pendentes.InsertOnSubmit(imovel);
            db.SubmitChanges();

            var precoImovel = new Preco()
            {
                Preco1 = Convert.ToDecimal(preco),
                Data = DateTime.Now.Date,
                Imovel_ID = imovel.Imovel_ID,
                Comissao = comissao
            };

            precoDB.InsertOnSubmit(precoImovel);
            db.SubmitChanges();

            imovel.Preco_ID = precoImovel.Preco_ID;

            db.SubmitChanges();
            db.Dispose();

            MessageBox.Show("Inserido!");
        }

        private async Task<string> BingGeo(string rua)
        {
            /*try
            {
                var BingGeoCoder = new BingMapsGeocoder("Al1qXg0aEDZ-c6XmCkhND5UWiO3OVMQanAXqGQemST8R3rMU2KZnYJoYVUZvGgL6");
                var address = await BingGeoCoder.GeocodeAsync(rua);
            }

            catch(Exception ex)
            {

            }

            try
            {
                var MapQuestCoder = new MapQuestGeocoder("fGPdAAuMcGCBHouFfXlPDpwkEisbdAoG");
                var address = await MapQuestCoder.GeocodeAsync(rua);
            }

            catch (Exception ex)
            {

            }*/

            try
            {
                var GoogleGeoCoder = new GoogleGeocoder();
                var address = (await GoogleGeoCoder.GeocodeAsync(rua)).ToList();

                foreach(var gps in address)
                {
                    var res = MessageBox.Show(gps.FormattedAddress, gps.Coordinates.ToString(), MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                        return gps.Coordinates.ToString();
                }

                return null;
            }

            catch (Exception ex)
            {
                return null;
            }/*

            try
            {
                var BingGeoCoder = new BingMapsGeocoder("Al1qXg0aEDZ-c6XmCkhND5UWiO3OVMQanAXqGQemST8R3rMU2KZnYJoYVUZvGgL6");
                var address = await BingGeoCoder.GeocodeAsync(rua);
            }

            catch (Exception ex)
            {

            }*/

        }

        private void button4_Click(object sender, EventArgs e)
        {
            BingGeo("Rua 1º de Maio nº 3 Figueira da Foz");
        }
    }
}
