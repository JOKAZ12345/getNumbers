using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace getNumbers
{
    class olx
    {
        enum Imovel
        {
            Apartamento,
            Moradia,
            Garagem,
            Loja,
            Terreno,
            Comercial
        }

        private string urlBuilder(Imovel v, string distancia, string publico)
        {
            string host = "https://www.olx.pt/";            

            string apartamentos = "imoveis/apartamento-casa-a-venda/";
            string garagens = "imoveis/garagens-estacionamento/";
            string lojas = "imoveis/escritorios-lojas/";
            string moradias = "imoveis/casas-moradias-para-arrendar-vender/";
            string terrenos = "imoveis/terrenos-quintas/";
            string estabelecimentos = "imoveis/estabelecimentos-comerciais-para-alugar-vender/";

            string figueira = "figueiradafoz/?";

            string particulares = "&search[private_business]=private";
            string profissionais = "&search[private_business]=business";

            string dist = "&search[dist]=10"; // Distância de raio da cidade meti por defeito 10 KM 

            string type = apartamentos;
            string p = particulares;

            if (v == Imovel.Moradia)
                type = moradias;

            else if (v == Imovel.Garagem)
                type = garagens;

            else if (v == Imovel.Loja)
                type = lojas;

            else if (v == Imovel.Terreno)
                type = terrenos;

            else if (v == Imovel.Comercial)
                type = estabelecimentos;

            if (publico == "Profissional")
                p = profissionais;

            if (distancia != "")
                dist = dist.Replace("10", distancia);


            return host + type + figueira + p + dist;
        }
        private string replaceHTMLChars(string s)
        {
            return s.Replace("\n", "").Replace("\t", "").Replace("\r", "");
        }

        public static string XmlHttpRequest(string urlString, string xmlContent)
        {

            string response = null;
            HttpWebRequest httpWebRequest = null;//Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebResponse httpWebResponse = null;//Declare an HTTP-specific implementation of the WebResponse class

            //Creates an HttpWebRequest for the specified URL.
            httpWebRequest = (HttpWebRequest)WebRequest.Create(urlString);

            try
            {
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlContent);
                //Set HttpWebRequest properties
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = bytes.Length;
                //httpWebRequest.ContentType = "text/xml; encoding='utf-8'";

                httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                httpWebRequest.Headers.Add("Accept-Language", "pt-PT,pt;q=0.8,en-US;q=0.6,en;q=0.4,it-IT;q=0.2,it;q=0.2");
                httpWebRequest.Accept = "*/*";
                httpWebRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");

                string cookieStr = "PHPSESSID=6ctl0qjf6eguemeiggvv9g48t5; mobile_default=desktop; xtvrn=$522886$; dfp_segment_test=64; dfp_segment_test_v2=75; pt=01e5964f389c915aa6fed58776632f9d5a22e9b3de4e15920f1f9cfeabb7384d7ffecb74bd158e115b7154348d3ed204888f01e26524d7338aae0c1362fb705d; dfp_user_id=f513c45e-11b4-ff47-b4e4-ee51bf293023-ver2; optimizelyEndUserId=oeu1496624983639r0.998747084689942; from_detail=1";
                CookieContainer cookiecontainer = new CookieContainer();
                string[] cookies = cookieStr.Split(';');
                foreach(var cookie in cookies)
                {
                    cookiecontainer.SetCookies(new Uri(urlString), cookie);
                }
                httpWebRequest.CookieContainer = cookiecontainer;
                //httpWebRequest.Headers.Add("Cookie", "PHPSESSID=fo4dso6rq94ba7pgbprfnjobd1; mobile_default=desktop; xtvrn=$522886$; dfp_segment_test=34; dfp_segment_test_v2=64; pt=553ddaf2794dfc1a7c8f12860c8ddb2282f9a4b80a60538b5d27bbdd3fa7018c1b4f5a2433c5c6d016516424c57371e3a68ec14d0407e62f310f9914b561fb26; from_detail=1");
                httpWebRequest.Host = "www.olx.pt";
                httpWebRequest.Referer = urlString;
                httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    //Writes a sequence of bytes to the current stream 
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();//Close stream
                }

                //Sends the HttpWebRequest, and waits for a response.
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    //Get response stream into StreamReader
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                            response = reader.ReadToEnd();
                    }
                }
                httpWebResponse.Close();//Close HttpWebResponse
            }
            catch (Exception ex)
            {
                return null;
            }
            httpWebResponse.Close();
            //Release objects
            httpWebResponse = null;
            httpWebRequest = null;

            return response;
        }

        private string getDataFromString(string data)
        {
            string dia = DateTime.Today.Day.ToString();
            string mes = DateTime.Today.Month.ToString();
            string ano = DateTime.Today.Year.ToString();

            var dt = data.Split(' ');

            for(int i = 1; i <= 3; i++)
            {
                if (i == 1)
                {
                    if (dt[i].Length == 1)
                        dia = "0" + dt[i];
                }

                else if (i == 2)
                {
                    string _mes = dt[2];

                    if (_mes == "Janeiro")
                        mes = "01";
                    else if (_mes == "Fevereiro")
                        mes = "02";
                    else if (_mes == "Março")
                        mes = "03";
                    else if (_mes == "Abril")
                        mes = "04";
                    else if (_mes == "Maio")
                        mes = "05";
                    else if (_mes == "Junho")
                        mes = "06";
                    else if (_mes == "Julho")
                        mes = "07";
                    else if (_mes == "Agosto")
                        mes = "08";
                    else if (_mes == "Setembro")
                        mes = "09";
                    else if (_mes == "Outubro")
                        mes = "10";
                    else if (_mes == "Novembro")
                        mes = "11";
                    else if (_mes == "Dezembro")
                        mes = "12";
                    
                }
                else if (i == 3)
                    ano = dt[i];
            }

            return dia + "/" + mes + "/" + ano;
        }

        public void test()
        {
            var Webget = new HtmlWeb();

            var db = new prabitarDataContext();
            var potencial = db.Potencials;
            var agencias = db.Agencias;
            var ignorar = db.Ignorars;

            var lista = new List<Potencial>();

            var en = Enum.GetValues(typeof(Imovel));

            foreach(Imovel imovel in Enum.GetValues(typeof(Imovel)))
            {
                PhantomJSOptions options = new PhantomJSOptions();
                PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
                service.LoadImages = false;

                var driver = new PhantomJSDriver(service, options);

                var r = urlBuilder(imovel, "10", "Publico");

                /*string url1 =
                    "https://www.olx.pt/imoveis/casas-moradias-para-arrendar-vender/buarcos/?search%5Bdescription%5D=1&search%5Bprivate_business%5D=private";

                string url2 =
                    "https://www.olx.pt/imoveis/apartamento-casa-a-venda/buarcos/?search%5Bdescription%5D=1&search%5Bprivate_business%5D=private&search%5Bdist%5D=10";*/
                var doc = Webget.Load(r);

                var casas_nodes = doc.DocumentNode.SelectNodes("//*[@class=\"marginright5 link linkWithHash detailsLink\"]");
                //var preco_nodes = doc.DocumentNode.SelectNodes("//*[@class=\"wwnormal tright td-price\"]");

                //var n = casas_nodes.Zip(preco_nodes, (node, htmlNode) => "[" + node.Attributes["href"].Value + "] [" + replaceHTMLChars(htmlNode.InnerText.Replace(".", "")) + "]");

                foreach (var casas in casas_nodes)
                {
                    var t = casas.SelectNodes("//*[@class=\"line-content\"]");

                    var regex = new Regex("<strong(.*?)>(.*?)</strong>");
                    var m = regex.Match(casas.InnerHtml);
                    var titulo = replaceHTMLChars(m.Groups[2].Value);

                    var linq = casas.Attributes["href"]?.Value;

                    if (!string.IsNullOrEmpty(linq))
                    {
                        if(!potencial.Any(x => x.URL == linq)) // URL not yet added to the DB
                        {
                            try
                            {
                                driver.Navigate().GoToUrl(linq);
                                var num = driver.FindElementByXPath("//*[@id=\"contact_methods\"]/li[2]/div");
                                var preco = driver.FindElementByClassName("xxxx-large");
                                var user = driver.FindElementByClassName("offer-user__details");
                                var data = driver.FindElementByXPath("//*[@id=\"offerdescription\"]/div[2]/div[1]/em");

                                string numInit = num.Text;

                                if (num != null)
                                {
                                    num.Click();

                                    for (int i = 0; i < 100; i++)
                                    {
                                        if (num.Text != numInit)
                                        {
                                            numInit = num.Text.Replace(" ", "");
                                            break;
                                        }
                                    }

                                    string _data = "";

                                    if (data != null)
                                        _data = getDataFromString(data.Text.Split(',')[1]);

                                    if (preco != null && user != null)
                                    {
                                        string _preco = preco.Text;
                                        string userNome = user.Text.Split('\r')[0];

                                        var tele = num.Text.Replace(" ", "");

                                        if (!potencial.Any(x => x.TituloAnuncio == titulo && (x.Telefone == numInit || x.Nome == userNome)))
                                        {
                                            var pot = new Potencial
                                            {
                                                Preco = Convert.ToDecimal(_preco.Replace(".", "").Replace(",", "").TrimEnd('€')),
                                                TituloAnuncio = titulo,
                                                URL = linq,
                                                Nome = userNome,
                                                Telefone = numInit,
                                                DataAnuncio = _data
                                            };

                                            //lista.Add(pot);
                                            potencial.InsertOnSubmit(pot);
                                        }
                                    }
                                }
                            }

                            catch (Exception ex)
                            {
                                //Debug.WriteLine(ex.Message.ToString());
                            }
                        }                        
                    }                                    
                }
                //Debug.WriteLine(lista.Count);
                driver.Quit();
            }

            MessageBox.Show("Importado do OLX!");
            db.SubmitChanges();
            db.Dispose();            
        }
    }
}
