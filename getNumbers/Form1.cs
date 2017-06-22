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

                        string telefone = "";
                        var link = "";

                        if (res != null)
                            telefone = res.InnerText;

                        res = doc.DocumentNode.SelectSingleNode("//*[@id=\"online\"]/a");

                        if (res != null)
                            link = res.Attributes["href"].Value;

                        var a = new Agencia { Nome = imobiliaria, Telefone = telefone, URL = link, url_idealista = url };

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

        private void procurarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void opçõesAvançadasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var pessoasForm = new Pessoas();

            pessoasForm.Show(ParentForm);
        }

        private void pesquisarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var imobiliariasForm = new Imobiliarias();

            imobiliariasForm.Show(ParentForm);
        }

        private void procurarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var prospecaoForm = new ProspecaoImoveis();

            prospecaoForm.Show(ParentForm);
        }

        private void opçõesAvançadasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var imoveisForm = new Imoveis();
            imoveisForm.ShowDialog();
        }
    }
}
