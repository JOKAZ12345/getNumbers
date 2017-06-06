using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using RestSharp;

namespace getNumbers
{
    class olx
    {
        private string replaceHTMLChars(string s)
        {
            return s.Replace("\n", "").Replace("\t", "").Replace("\r", "");
        }

        public void test()
        {
            var Webget = new HtmlWeb();

            var db = new prabitarDataContext();
            var potencial = db.Potencials;

            string url1 =
                "https://www.olx.pt/imoveis/casas-moradias-para-arrendar-vender/buarcos/?search%5Bdescription%5D=1&search%5Bprivate_business%5D=private";
            var doc = Webget.Load(url1);


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
                    var imovel = Webget.Load(linq);

                    var preco = imovel.DocumentNode.SelectSingleNode("//*[@class=\"xxxx-large not-arranged\"]")?.InnerText;
                    var contactos = imovel.DocumentNode.SelectNodes("//*[@class=\"offer-sidebar__buttons contact_methods\"]");

                    string token = "";
                    string olx_id = "";

                    foreach (var script in imovel.DocumentNode.Descendants("script"))
                    {
                        if (script.InnerText.Contains("phoneToken"))
                        {
                            var reg = new Regex("'(.*?)'");
                            var res = reg.Match(script.InnerText);
                            token = res.Groups[1].Value;

                            reg = new Regex("-(\\w*?).html");
                            res = reg.Match(linq);
                            olx_id = res.Groups[1].Value;
                        }
                    }

                    var client = new RestClient("https://www.olx.pt/ajax/misc/contact/phone/AzyQb");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader()


                    // OLX PROCURAR POR
                    // var phonetoken = 'xxxxxxxx';
                    // Ir buscar o ID do anúncio 'id':'AsC3l'
                    // URL: 
                }
            }

            /*foreach (var preco in preco_nodes)
            {
                var x = replaceHTMLChars(preco.InnerText.Replace(".", "")); // we need to remove dots from price
            }*/
        }
    }
}
