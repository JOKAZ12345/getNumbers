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
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace getNumbers
{
    class SapoCasa
    {
        private const string SapoUrl = "http://casa.sapo.pt";

        public void test()
        {
            var Webget = new HtmlWeb();

            var Markers = new List<Marker>();

            var db = new prabitarDataContext();
            var potencial = db.Potencials;

            var linqs = from a in db.Potencials select a.URL;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            string url = "https://casa.sapo.pt/Venda/Apartamentos/Figueira-da-Foz/?sa=6&or=10&pn=#";
            string ur2 = "https://casa.sapo.pt/Venda/Moradias/Figueira-da-Foz/?sa=6&or=10&pn=#";
            string gar = "https://casa.sapo.pt/Venda/Garagens/Figueira-da-Foz/?sa=6&pn=#";
            string _url = "";
            try
            {
                //var doc = Webget.Load("https://casa.sapo.pt/Venda/Moradias/Figueira-da-Foz/?sa=6&nd=60&or=10&pn=1");

                for (var t = 0; t < 3; t++)
                {
                    HtmlDocument doc;
                    switch (t)
                    {
                        case 0:
                            _url = url;
                            doc = Webget.Load(_url.Replace("#", "1"));
                            break;
                        case 1:
                            _url = ur2;
                            doc = Webget.Load(_url.Replace("#", "1"));
                            break;
                        default:
                            _url = gar;
                            doc = Webget.Load(_url.Replace("#", "1"));
                            break;
                    }

                    var nodes = doc.DocumentNode.SelectNodes("//*[@id=\"divSearchPageResults\"]/div");

                    var y = nodes[nodes.Count - 1].ChildNodes.FindFirst("p")?.InnerHtml;

                    if (y == null) continue;

                    var page=1;
                    try
                    {
                        page = Convert.ToInt32(y.Substring(y.IndexOf(">", y.IndexOf("</a>\r\n\t", y.Length - 40) - 5) + 1,
                            y.IndexOf("</a>\r\n\t", y.Length - 40) -
                            y.IndexOf(">", y.IndexOf("</a>\r\n\t", y.Length - 40) - 5) - 1));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    for (var i = 1; i <= page; i++)
                    {
                        if (i != 1)
                        {
                            doc = Webget.Load(_url.Replace("#", i.ToString()));

                            nodes = doc.DocumentNode.SelectNodes("//*[@id=\"divSearchPageResults\"]/div");
                        }

                        foreach (var node in nodes)
                        {
                            var getNode = node.ChildNodes.FirstOrDefault(x => x.Name == "a");

                            if (getNode != null && getNode.Attributes.Any(x => x.Name.Equals("href")))
                            {
                                var link = SapoUrl + getNode.Attributes["href"].Value;

                                if (!string.IsNullOrEmpty(link))
                                {
                                    var imovel = Webget.Load(link);

                                    var preco = imovel.DocumentNode.SelectSingleNode("//*[@class=\"detailPropertyPrice\"]")?.InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", "");

                                    var titulo =
                                        imovel.DocumentNode.SelectSingleNode("//*[@class=\"detailPropertyTitle\"]")?
                                            .ChildNodes[0].InnerText.Replace("\n", "")
                                            .Replace("\t", "")
                                            .Replace("\r", "");

                                    string data = null;

                                    if (preco != null && titulo!=null)
                                    {
                                        var features = imovel.DocumentNode.SelectSingleNode("//*[@class=\"detailFeaturesList\"]");

                                        var imobiliaria = imovel.DocumentNode.SelectSingleNode("//*[@class=\"ownerName\"]")?.InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", "");

                                        foreach (var feature in features.ChildNodes)
                                        {
                                            if (feature.Name.Contains("div"))
                                            {
                                                var text =
                                                    feature.InnerHtml.Replace("<span>", "")
                                                        .Replace("</span>", ":")
                                                        .Replace("\n", "")
                                                        .Replace("\t", "")
                                                        .Replace("\r", "").Split(':');

                                                if (text[0].Contains("Publicado"))
                                                {
                                                    data = text[1].Replace(" ", "");
                                                }
                                            }
                                        }

                                        try
                                        {
                                            foreach (
                                                var script in imovel.DocumentNode.Descendants("script").ToArray())
                                            {
                                                var s =
                                                    script.InnerText.Replace("\n", "")
                                                        .Replace("\t", "")
                                                        .Replace("\r", "");

                                                if (!s.Contains("GoogleMap")) continue;
                                                var re = new Regex("var CenterMarkerLat = (.*?);\\s*$");
                                                var m = re.Match(s);
                                                var d = m.Value.Split(';');
                                                string latitude = null;
                                                string longitude = null;
                                                string desc = null;

                                                foreach (var var in d)
                                                {
                                                    if (var.Contains("HouseLat"))
                                                    {
                                                        latitude = var.Split('\'')[1];
                                                    }

                                                    else if (var.Contains("HouseLon"))
                                                    {
                                                        longitude = var.Split('\'')[1];

                                                        if (latitude != null && longitude != null && desc != null)
                                                        {
                                                            var marker =
                                                                new Marker(
                                                                    imobiliaria + "\nSapo: " + desc + "\nPreço: " +
                                                                    preco,
                                                                    Convert.ToDouble(longitude.Replace('.', ',')),
                                                                    Convert.ToDouble(latitude.Replace('.', ',')),
                                                                    link, data);

                                                            Markers.Add(marker);
                                                        }

                                                        break;
                                                    }

                                                    else if (var.Contains("HouseDescription"))
                                                    {
                                                        desc = var.Split('\'')[1];
                                                    }
                                                }

                                                if (preco.Contains("/"))
                                                    preco = preco.Split('/')[0];

                                                var x = string.IsNullOrEmpty(preco)

                                                    ? new Potencial
                                                    {
                                                        Preco = Convert.ToDecimal("0"),
                                                        TituloAnuncio = titulo,
                                                        URL = link,
                                                        Nome = imobiliaria,
                                                        Coordenadas = Convert.ToDouble(longitude.Replace('.', ',')) + ", " + Convert.ToDouble(latitude.Replace('.', ',')),
                                                    }
                                                    : new Potencial
                                                    {
                                                        Preco = Convert.ToDecimal(preco.TrimEnd('€')),
                                                        TituloAnuncio = titulo,
                                                        URL = link,
                                                        Nome = imobiliaria,
                                                        Coordenadas = Convert.ToDouble(longitude.Replace('.', ',')) + ", " + Convert.ToDouble(latitude.Replace('.', ',')),
                                                    };

                                                // Used local list to reduce sql overhead
                                                if (!linqs.Contains(x.URL))
                                                    potencial.InsertOnSubmit(x);
                                            }
                                        }
                                        catch
                                            (Exception e)
                                        {
                                            MessageBox.Show(e.ToString(), preco.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                db.SubmitChanges();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("Concluído!", "Sapo!");

            /*var mapa = new gmap2();
            mapa.Show();
            mapa.addMarkers(Markers);*/
        }
    }
}
