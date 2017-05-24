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
    class SapoCasa
    {
        private const string SapoUrl = "http://casa.sapo.pt";

        public void test()
        {
            var Webget = new HtmlWeb();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            try
            {
                var doc = Webget.Load("https://casa.sapo.pt/Venda/Moradias/Figueira-da-Foz/?sa=6&nd=60&or=10");

                var nodes = doc.DocumentNode.SelectNodes("//*[@id=\"divSearchPageResults\"]/div");

                foreach (var node in nodes)
                {
                    var link = SapoUrl + node.ChildNodes.FirstOrDefault(x => x.Name == "a")?.Attributes["href"].Value;
                    /*var str = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "#");
                        // Mais fácil depois para procurar com "#"*/ // NAO VALE A PENA.. MAIS VALE ENTRAR DENTRO DO URL E SACAR CENAS


                    var imovel = Webget.Load(link);

                    var preco = imovel.DocumentNode.SelectSingleNode("//*[@class=\"detailPropertyPrice\"]").InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", "");


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
