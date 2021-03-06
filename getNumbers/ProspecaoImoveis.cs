﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

// TODO: Adicionar só potenciais Activos

namespace getNumbers
{
    public partial class ProspecaoImoveis : Form
    {
        public ProspecaoImoveis()
        {
            InitializeComponent();
            comboBox1.Text = "Buarcos e São Julião";
            comboBox1.SelectedItem = comboBox1.Text;
        }

        private class Imoveis
        {
            internal Imoveis(string a, string b, string c, string d)
            {
                Titulo = a;
                Preco = b;
                UrlImovel = c;
                Telefone = d;
            }

            private string Titulo { get; }
            private string Preco { get; }
            private string UrlImovel { get; }
            private string Telefone { get; }
        }

        private const string idealistaURL = "http://www.idealista.pt/";
        private const string comprar = "comprar-casas/";
        private const string arrendar = "arrendar-casas/";
        private const string coimbra = "coimbra/";
        private const string figueira = "figueira-da-foz/";

        private void button1_Click(object sender, EventArgs e)
        {
            string tipo;
            if (venderRBTN.Checked)
                tipo = comprar;
            else if (arrendarRBTN.Checked)
                tipo = arrendar;

            else
            {
                MessageBox.Show("Selecionar o tipo de negócio VENDA/ARRENDAMENTO", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var Webget = new HtmlWeb();

            var db = new prabitarDataContext();

            var imoveis = new List<Imoveis>();
            var potencial = new List<Potencial>();

            var _agencias = from a in db.Agencias select a.Telefone;
            potencial = new List<Potencial>(db.Potencials.Select(a => a));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var localizacao = comboBox1.SelectedItem.ToString().ToLower().Replace(" ", "-").Replace("ã", "a"); // TODO: Check for more

            var doc = Webget.Load(idealistaURL + tipo + coimbra + figueira + localizacao + "/pagina-" + "1" + "?ordem=atualizado-desc");

            if (doc == null)
                return;

            var numPages = NumPagesComprarCasa(doc);

            // TODO: Meter link genérico
            for (var j = 1; j < numPages; j++, doc = Webget.Load(idealistaURL + tipo + coimbra + figueira + localizacao + "/pagina-" + j + "?ordem=atualizado-desc"))
            {
                var ourNode = doc.DocumentNode.SelectNodes("//*[@id=\"main-content\"]/div[2]/article");

                if (ourNode == null) continue;

                int i = 1;

                string titulo = null;
                string url_imovel = null;
                string preco = null;

                foreach (var node in ourNode)
                {
                    try
                    {
                        foreach (var child in node.ChildNodes)
                        {
                            if (child.Name == "div")
                            {
                                foreach (var _child in child.ChildNodes)
                                {
                                    if (_child.Name == "div")
                                    {
                                        if (_child.Attributes.Any(x => x.Name == "class" && x.Value == "clearfix"))
                                        {
                                            var myNode = _child.ChildNodes[_child.ChildNodes.Count - 2];

                                            int divcont = 0;
                                            foreach (var childNode in myNode.ChildNodes)
                                            {
                                                if (childNode.Name == "a") // TODO: Assumo que o node <a href> existe
                                                {
                                                    titulo = childNode.InnerText;
                                                    url_imovel = childNode.Attributes["href"].Value;
                                                }

                                                if (childNode.Name == "div" && !childNode.InnerHtml.Contains("href"))
                                                {
                                                    if (divcont == 0)
                                                    {
                                                        preco = childNode.InnerText.Substring(1, childNode.InnerText.IndexOf(" ", 1) - 2).Replace(".", "");
                                                        divcont++;
                                                    }
                                                }

                                                else if (divcont != 0 && childNode.Name == "div" && childNode.InnerText.ToLower().Contains("contactar"))
                                                {
                                                    if (childNode.Attributes["class"].Value == "item-toolbar clearfix")
                                                    {
                                                        var tele = childNode.ChildNodes[1].ChildNodes[1].InnerText;

                                                        if (tele != null)
                                                        {
                                                            // Falta guardar aqui o nome do proprietário

                                                            // TODO: Verificar se o anúncio já está na BD
                                                            if (!_agencias.Any(x => x == tele) && potencial.All(x => x.Telefone != tele) && !potencial.All(x => url_imovel != null && x.URL.Contains(url_imovel)))
                                                            {
                                                                imoveis.Add(new Imoveis(titulo, preco, url_imovel, tele));
                                                                db.Potencials.InsertOnSubmit(new Potencial
                                                                {
                                                                    Telefone = tele,
                                                                    TituloAnuncio = titulo,
                                                                    URL = idealistaURL.Replace("/", "") + url_imovel, // TODO: ACHO QUE ESTÁ MAL
                                                                    Preco = Convert.ToDecimal(preco)
                                                                });
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    i++;
                }
            }

            db.SubmitChanges();
            db.Dispose();
        }

        /*
                 *  Vai buscar as imobiliárias ao site
                 *  e guarda os links no ficheiro de txt
                 *  urls.txt
                 */
        private void button2_Click(object sender, EventArgs e)
        {
            var urls = new List<string>();

            var Webget = new HtmlWeb();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var doc = Webget.Load("https://www.idealista.pt/pro/comprar-casas/aveiro-distrito/imobiliarias");

            var nodes = doc.DocumentNode.SelectSingleNode("//*[@id=\"header\"]/div/ul/li[2]/div/ul"); // Vai buscar os districtos disponíveis no site

            try
            {
                int j = 1;
                foreach (var node in nodes.ChildNodes)
                {
                    if (node.Name.Contains("li"))
                    {
                        if (j == 1) // 1º distrito
                        {
                            var imobiliarias = doc.DocumentNode.SelectNodes("//*[@id=\"experts-list\"]/dl");

                            if (imobiliarias != null) // Podem não haver imobiliárias
                                urls.AddRange(
                                    imobiliarias.Select(imobiliaria => imobiliaria.SelectSingleNode("dd/a").InnerHtml));
                            j++;
                        }

                        else // Os outros
                        {
                            var ed = node.SelectSingleNode("//*[@id=\"header\"]/div/ul/li[2]/div/ul/li[" + j++ + "]/a");
                            // Vai buscar o nome do districto

                            var _url = ed.Attributes["href"].Value;
                            var districto = ed.Attributes["title"].Value;

                            var url = "https://www.idealista.pt" + _url;
                            // Temos aqui o URL

                            var dist = Webget.Load(url);

                            var _node = dist.DocumentNode.SelectSingleNode("//*[@id=\"pager_list\"]/font");

                            if (_node == null) // É só uma página
                            {
                                var imobiliarias = dist.DocumentNode.SelectNodes("//*[@id=\"experts-list\"]/dl");

                                if (imobiliarias != null) // Podem não haver imobiliárias
                                    urls.AddRange(imobiliarias.Select(imobiliaria => imobiliaria.SelectSingleNode("dd/a").InnerHtml));
                            }

                            else
                            {
                                int numPages = Convert.ToInt32(_node.ChildNodes[_node.ChildNodes.Count - 2 - 1].InnerText);

                                for (var i = 1; i <= numPages; i++)
                                {
                                    var imobiliarias = dist.DocumentNode.SelectNodes("//*[@id=\"experts-list\"]/dl");

                                    if (imobiliarias != null)
                                        urls.AddRange(
                                            imobiliarias.Select(
                                                imobiliaria => imobiliaria.SelectSingleNode("dd/a").InnerHtml));

                                    if (i == 1) // 1ª página
                                        url = url + "-" + (i + 1);

                                    else
                                    {
                                        url = url.Replace("-" + i, "-" + (i + 1));
                                    }

                                    dist = Webget.Load(url);
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            File.WriteAllLines("urls.txt", urls);
        }

        public static bool AreAnyDuplicates<T>(IEnumerable<T> list)
        {
            var hashset = new HashSet<T>();
            return list.Any(e => !hashset.Add(e));
        }


        // Todo: Atenção que até ao momento só funciona com o idealista.pt
        private void button3_Click(object sender, EventArgs e)
        {
            var Webget = new HtmlWeb();

            var db = new prabitarDataContext();

            var _agencias = from a in db.Agencias select a;


            try
            {
                var d = File.ReadAllLines("urls.txt").ToList();

                if (AreAnyDuplicates(d))
                {
                    var duplicates = d.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key);
                    foreach (var s in duplicates)
                        d.Remove(s);
                }

                foreach (var agencia in _agencias) // Remove as agencias que já existirem e vai procurar aquelas que faltam
                {
                    if (d.Contains(agencia.url_idealista))
                        d.Remove(agencia.url_idealista);
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                foreach (var url in d)
                {
                    var doc = Webget.Load("https://" + url);

                    var res = doc.DocumentNode.SelectSingleNode("//*[@id=\"commercial-name\"]");

                    if (res != null)
                    {
                        var imobiliaria = doc.DocumentNode.SelectSingleNode("//*[@id=\"commercial-name\"]").InnerText;

                        res = doc.DocumentNode.SelectSingleNode("//*[@id=\"online\"]/span/span");

                        var telefone = "";
                        var link = "";

                        if (res != null)
                            telefone = res.InnerText;

                        res = doc.DocumentNode.SelectSingleNode("//*[@id=\"online\"]/a");

                        if (res != null)
                            link = res.Attributes["href"].Value;

                        var a = new Agencia { Nome = imobiliaria, Telefone = telefone, URL = link, url_idealista = url };
                        new List<Agencia>().Add(a);

                        db.Agencias.InsertOnSubmit(a);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }

            db.SubmitChanges();
        }

        private static int NumPagesComprarCasa(HtmlDocument doc)
        {
            var node = doc.DocumentNode.SelectNodes("//*[@id=\"main-content\"]/div[2]/div/ul/li");

            if (node == null)
                return 1;

            for (var i = 0; i < node.Count; i++)
            {
                if (node[i].InnerText.ToLower().Contains("seguinte")) // Se tiver seguinte eu sei que o i anterior tem o nº de páginas
                    return Convert.ToInt32(node[i - 1].InnerText);
            }

            return 1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Isto vai limpar a prospeção que esteja na base de dados ao cruzar dados com Imobiliárias",
                @"Limpeza");

            var db = new prabitarDataContext();

            var contactosAgencias = db.Agencias;
            var contactosPotenciais = db.Potencials;

            foreach (var t in contactosPotenciais)
            {
                foreach (var tt in contactosAgencias)
                {
                    if (t.Telefone != null && tt.Telefone!=null && t.Telefone == tt.Telefone)
                        db.Potencials.DeleteOnSubmit(t);

                    else if(tt.Nome!=null && t.Nome!=null && tt.Nome.ToLower().Contains(t.Nome.ToLower()))
                        db.Potencials.DeleteOnSubmit(t);
                }
            }

            db.SubmitChanges();

            MessageBox.Show(@"Limpeza Feita!");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // TODO: Depois inserir o nº na BD
            // Tenta ir a cada URL e limpar as agências (pode ter passado algum número que não esteja na BD)

            var Webget = new HtmlWeb();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var db = new prabitarDataContext();
            var potenciais = db.Potencials;
            var agencias = db.Agencias;

            foreach (var potencial in potenciais)
            {
                if (potencial.URL.Contains("idealista"))
                {
                    var doc = Webget.Load(potencial.URL.Replace("http:w", "http://w"));

                    var node = doc?.DocumentNode.SelectNodes("//*[@id=\"side-content\"]/section/div"); // Vai buscar o tipo de anunciante

                    if (node != null)
                    {
                        var anunciante = node[node.Count - 1].InnerText.ToLower();

                        if (!anunciante.Contains("particular"))
                        {
                            var d = MessageBox.Show(potencial.TituloAnuncio + "\n\n" + anunciante + "\n\n" + potencial.URL,
                                "Deseja eliminar?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                            if (d == DialogResult.Yes || d == DialogResult.OK)
                                db.Potencials.DeleteOnSubmit(potencial);
                        }
                    }
                }

                else if (potencial.URL.Contains("sapo"))
                {
                    if(agencias.Any(x => x.Nome.ToLower().Equals(potencial.Nome.ToLower())))
                        db.Potencials.DeleteOnSubmit(potencial);
                }
                
            }

            db.SubmitChanges();

            MessageBox.Show("Limpeza Feita!");
        }

        // Exportar dados em CSV ou .pdf
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var db = new prabitarDataContext();

                var potencial = db.Potencials;

                var folderBrowserDialog1 = new FolderBrowserDialog();

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    CreateCSV(potencial.ToList(), folderBrowserDialog1.SelectedPath);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

            private static void CreateHeader<T>(List<T> list, TextWriter sw, string delimitador)
            {
                var properties = typeof(T).GetProperties();
                for (var i = 0; i < properties.Length - 1; i++)
                {
                    sw.Write(properties[i].Name + delimitador);
                }
                var lastProp = properties[properties.Length - 1].Name;
                sw.Write(lastProp + sw.NewLine);
            }

            private static void CreateRows<T>(IEnumerable<T> list, TextWriter sw, string delimitador)
            {
                foreach (var item in list)
                {
                    var properties = typeof(T).GetProperties();
                    for (var i = 0; i < properties.Length - 1; i++)
                    {
                        var prop = properties[i];
                        sw.Write(prop.GetValue(item) + delimitador);
                    }
                    var lastProp = properties[properties.Length - 1];
                    sw.Write(lastProp.GetValue(item) + sw.NewLine);
                }
            }

            public static void CreateCSV<T>(List<T> list, string filePath)
            {
                using (var sw = new StreamWriter(filePath))
                {
                    CreateHeader(list, sw, ";");
                    CreateRows(list, sw, ";");
                }
            }

        private void button5_Click(object sender, EventArgs e)
        {
            var sapo = new SapoCasa();

            sapo.test();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var mapa = new Resultados_Prospecao();
            mapa.Show(this);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var olx = new olx();
            olx.test();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var mapa = new gmap2();
            mapa.addMarkers(new List<Marker>());
            mapa.ShowDialog(this);
            //mapa.addMarkers(new List<Marker>());
            //mapa.Refresh();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var remax = new Remax();
            remax.roubar();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var mapa = new gmap2();
            mapa.ShowDialog(this);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var remax = new Remax();
            remax.roubarRemaxSite();

        }

        private void ProspecaoImoveis_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'prabitarDataSet1.Potencial' table. You can move, or remove it, as needed.
            //dataGridView1.BorderStyle = BorderStyle.None; dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249); dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise; dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke; dataGridView1.BackgroundColor = Color.White; dataGridView1.EnableHeadersVisualStyles = false; dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None; dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72); dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.potencialTableAdapter.Fill(this.prabitarDataSet1.Potencial);

        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {

                var db = new prabitarDataContext();

                var potencial = db.Potencials;

                foreach (var p in potencial)
                {
                    TextBox textBox = new TextBox();
                    textBox.Text = p.Telefone;
                    textBox.Location = new Point(10, panel1.Controls.Count * 25);

                    TextBox textBox2 = new TextBox();
                    textBox2.Text = p.TituloAnuncio;
                    textBox2.Location = new Point(170, panel1.Controls.Count * 25);
                    textBox2.Size = new Size(textBox2.Text.Length * 6, 20);

                    panel1.Controls.Add(textBox);
                    panel1.Controls.Add(textBox2);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
