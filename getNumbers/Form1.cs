using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace getNumbers
{
    public partial class Form1 : Form
    {
        public Form1()
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

            string localizacao = comboBox1.SelectedItem.ToString().ToLower().Replace(" ", "-").Replace("ã", "a"); // TODO: Check for more

            var doc = Webget.Load(idealistaURL + tipo + coimbra + figueira + localizacao + "/pagina-" + "1");
            //var doc = Webget.Load("https://www.idealista.pt/comprar-casas/gois/alvares/");

            if (doc == null)
                return;

            var numPages = NumPagesComprarCasa(doc);

            // TODO: Meter link genérico
            for (var j = 1; j < numPages; j++, doc = Webget.Load(idealistaURL + tipo + coimbra + figueira + localizacao + "/pagina-" + j))
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
                                                            //tele = tele.Replace("+", "0").Replace(" ", "");
                                                            //var telefone = Convert.ToInt64(tele);

                                                            if (!_agencias.Any(x => x == tele) && potencial.All(x => x.Telefone != tele))
                                                            {
                                                                imoveis.Add(new Imoveis(titulo, preco, url_imovel, tele));
                                                                db.Potencials.InsertOnSubmit(new Potencial()
                                                                {
                                                                    Telefone = tele,
                                                                    TituloAnuncio = titulo,
                                                                    URL = idealistaURL.Replace("/", "") + url_imovel,
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
                                        url = url + "-" + (i + 1).ToString();

                                    else
                                    {
                                        url = url.Replace("-" + i.ToString(), "-" + (i + 1).ToString());
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

            var agencias = new List<Agencia>();

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

                        string telefone="";
                        var link = "";

                        if(res!=null)
                            telefone = res.InnerText;

                        res = doc.DocumentNode.SelectSingleNode("//*[@id=\"online\"]/a");

                        if (res != null)
                            link = res.Attributes["href"].Value;

                        var a = new Agencia{ Nome = imobiliaria, Telefone = telefone, URL = link, url_idealista = url };

                        agencias.Add(a);

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
            if (!string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox1.Text))
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

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Isto vai limpar a prospeção que esteja na base de dados ao cruzar dados com Imobiliárias",
                "Limpeza");

            var db = new prabitarDataContext();

            var contactosAgencias = db.Agencias;
            var contactosPotenciais = db.Potencials;

            foreach (var t in contactosPotenciais)
            {
                foreach (var tt in contactosAgencias)
                {
                    if (t.Telefone == tt.Telefone)
                        db.Potencials.DeleteOnSubmit(t);
                }
            }

            db.SubmitChanges();

            MessageBox.Show("Limpeza Feita!");
        }
    }
}
