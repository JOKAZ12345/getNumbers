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

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            try
            {
                //var doc = Webget.Load("https://casa.sapo.pt/Venda/Moradias/Figueira-da-Foz/?sa=6&nd=60&or=10&pn=1");
                var doc = Webget.Load("https://casa.sapo.pt/Venda/Apartamentos/Figueira-da-Foz/?sa=6&or=10&pn=1");

                var nodes = doc.DocumentNode.SelectNodes("//*[@id=\"divSearchPageResults\"]/div");

                var y = nodes[nodes.Count - 1].ChildNodes.FindFirst("p")?.InnerHtml;

                if (y != null)
                {
                    var page = Convert.ToInt32(y.Substring(y.IndexOf('>', y.Length - "</a>\r\n\t".Length - 5)).Split('<')[0].Replace(">", ""));

                    for (var i = 1; i <= page; i++)
                    {
                        if (i != 1)
                        {
                            doc = Webget.Load("https://casa.sapo.pt/Venda/Apartamentos/Figueira-da-Foz/?sa=6&or=10&pn=#".Replace("#", i.ToString()));

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

                                    string data = null;

                                    if (preco != null)
                                    {
                                        var features = imovel.DocumentNode.SelectSingleNode("//*[@class=\"detailFeaturesList\"]");

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
                                        foreach (var script in imovel.DocumentNode.Descendants("script").ToArray())
                                        {
                                            string s = script.InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", "");

                                            if (s.Contains("GoogleMap"))
                                            {
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
                                                            var marker = new Marker("Sapo: " + desc + "\nPreço: " + preco, Convert.ToDouble(longitude.Replace('.', ',')),
                                                                Convert.ToDouble(latitude.Replace('.', ',')), link, data);

                                                            Markers.Add(marker);
                                                        }

                                                        break;
                                                    }

                                                    else if (var.Contains("HouseDescription"))
                                                    {
                                                        desc = var.Split('\'')[1];
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

            var mapa = new gmap2();
            mapa.Show();
            mapa.addMarkers(Markers);
        }
    }
}
