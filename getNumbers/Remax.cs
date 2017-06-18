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
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace getNumbers
{
    class Remax
    {
        //https://casa.sapo.pt/Remax/Figueira-da-Foz/Buarcos/?sa=6&sys=5&or=10

        private const string SapoUrl = "http://casa.sapo.pt";

        private string titulo;
        private string localidade;
        private string preco;
        private detalhes info;
        private class detalhes
        {
            private int assoalhadas;
            private int quartos;
            private int casas_banho;
            private double area;
            private string ce;

            public detalhes(int a, int q, int c, double area, string ce)
            {
                this.assoalhadas = a;
                this.quartos = q;
                this.casas_banho = c;
                this.area = area;
                this.ce = ce;
            }

            public detalhes(string a, string q, string c, string area, string ce)
            {
                if (string.IsNullOrWhiteSpace(a))
                    a = "0";

                if (string.IsNullOrWhiteSpace(q))
                    q = "0";

                if (string.IsNullOrWhiteSpace(c))
                    c = "0";

                this.assoalhadas = Convert.ToInt32(a);
                this.quartos = Convert.ToInt32(q);
                this.casas_banho = Convert.ToInt32(c);
                this.area = Convert.ToDouble(area);
                this.ce = ce;
            }
        }

        private string lat;
        private string lon;

        public Remax()
        {

        }

        public void roubarRemaxSite()
        {
            var Webget = new HtmlWeb();

            var Markers = new List<Marker>();

            var db = new prabitarDataContext();
            var potencial = db.Potencials;

            var linqs = from a in db.Potencials select a.URL;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            string url = "http://www.remax.pt/PublicListingList.aspx?SearchKey=B8D9444A21C14D8DA4204297A435E0BA#mode=list&tt=261&cr=2&r=61&p=444&c=138233&cur=EUR&la=All&sb=PriceIncreasing&page=v&sc=12&sid=a81a1d1d-ee36-4236-a72e-31343349c574";

            var remaxImoveis = new List<Remax>();

            url = url.Replace("page=v", "page=" + 1);

            PhantomJSOptions options = new PhantomJSOptions();
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
            service.LoadImages = false;

            var driver = new PhantomJSDriver(service, options);

            driver.Navigate().GoToUrl(url);

            //var imoveis = new System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>(null);
            List<IWebElement> imoveis = new List<IWebElement>();

            for (int z = 0; z < 3000; z++)
            {
                imoveis = driver.FindElementsByClassName("listing-list").ToList();

                if (imoveis.Count != 0)
                    break;
            }

            if (imoveis.Count == 0)
                return;

            double xf = (double)Convert.ToInt32(driver.FindElementByClassName("num-matches").Text.Split(' ')[0]) / 24;
            int pages = (int)Math.Round(xf, MidpointRounding.AwayFromZero);

            try
            {
                for(int i = 0; i < pages; i++)
                {
                    if (i != 0)
                    {
                        url = url.Replace("page=v", "page=" + i + 1);

                        options = new PhantomJSOptions();
                        service = PhantomJSDriverService.CreateDefaultService();
                        service.LoadImages = false;

                        driver = new PhantomJSDriver(service, options);

                        driver.Navigate().GoToUrl(url);

                        //var imoveis = new System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>(null);
                        imoveis = new List<IWebElement>();

                        for (int z = 0; z < 3000; z++)
                        {
                            imoveis = driver.FindElementsByClassName("listing-list").ToList();

                            if (imoveis.Count != 0)
                                break;
                        }

                        if (imoveis == null)
                            break;
                    }                            

                    foreach (var imovel in imoveis)
                    {
                        try
                        {
                            var titulo = imovel.FindElement(By.ClassName("listinglist-proptype")).Text.Split('-')[0];
                            titulo = titulo.Remove(titulo.Length - 1, 1);
                            var morada = imovel.FindElement(By.ClassName("proplist-address")).Text;
                            var preco = imovel.FindElement(By.ClassName("proplist-price-container")).Text;
                            var info = imovel.FindElements(By.ClassName("gallery-attr-item-value"));

                            var x = new detalhes(info[0].Text, info[1].Text, info[2].Text, info[3].Text, info[4].Text);

                            var gps = imovel.FindElement(By.ClassName("ll-map-invoker"));

                            var lat = gps.GetAttribute("data-lat");
                            var lon = gps.GetAttribute("data-lng");

                            remaxImoveis.Add(new Remax
                            {
                                titulo = titulo,
                                localidade = morada,
                                preco = preco,
                                lat = lat,
                                lon = lon,
                                info = new detalhes(info[0].Text, info[1].Text, info[2].Text, info[3].Text, info[4].Text)
                            });
                        }
                        catch (Exception ex)
                        {

                            //throw;
                        }
                    }

                    driver.Quit();
                }

            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        public void roubar()
        {
            var Webget = new HtmlWeb();

            var Markers = new List<Marker>();

            var db = new prabitarDataContext();
            var potencial = db.Potencials;

            var linqs = from a in db.Potencials select a.URL;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            string url = "https://casa.sapo.pt/Remax/Figueira-da-Foz/Buarcos/?sa=6&sys=5&or=10&pn=#";
            string ur2 = "https://casa.sapo.pt/Remax/Figueira-da-Foz/Tavarede/?sa=6&sys=5&or=10&pn=#";
            string _url = "";

            try
            {
                for (var t = 0; t < 2; t++)
                {
                    HtmlDocument doc = new HtmlDocument();

                    if (t==1)
                    {
                        _url = ur2;
                        doc = Webget.Load(_url.Replace("#", "1"));
                    }

                    else if(t==0)
                    {
                        _url = url;
                        doc = Webget.Load(_url.Replace("#", "1"));
                    }

                    var nodes = doc.DocumentNode.SelectNodes("//*[@class=\"searchContent\"]/div");

                    var y = nodes[nodes.Count - 1].ChildNodes.FindFirst("p")?.InnerHtml;

                    if (y == null) continue;

                    var page = 1;
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

                            nodes = doc.DocumentNode.SelectNodes("//*[@class=\"searchResultProperty\"]/div");
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

                                    if (preco != null && titulo != null)
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
                                                            /*var marker =
                                                                new Marker(
                                                                    imobiliaria + "\nSapo: " + desc + "\nPreço: " +
                                                                    preco,
                                                                    Convert.ToDouble(longitude.Replace('.', ',')),
                                                                    Convert.ToDouble(latitude.Replace('.', ',')),
                                                                    link, data);

                                                            Markers.Add(marker);*/
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
                                                        DataAnuncio = data
                                                    }
                                                    : new Potencial
                                                    {
                                                        Preco = Convert.ToDecimal(preco.TrimEnd('€')),
                                                        TituloAnuncio = titulo,
                                                        URL = link,
                                                        Nome = imobiliaria,
                                                        Coordenadas = Convert.ToDouble(longitude.Replace('.', ',')) + ", " + Convert.ToDouble(latitude.Replace('.', ',')),
                                                        DataAnuncio = data
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

            MessageBox.Show("Concluído!", "REMAX STEAL!");

            /*var mapa = new gmap2();
            mapa.Show();
            mapa.addMarkers(Markers);*/
        }
    }

    
}
