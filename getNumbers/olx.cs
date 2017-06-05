﻿using System;
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
                var regex = new Regex("<strong(.*?)>(.*?)</strong>");
                var m = regex.Match(casas.InnerHtml);
                var titulo = replaceHTMLChars(m.Groups[2].Value);

                var linq = casas.Attributes["href"]?.Value;

                var imovel = Webget.Load(linq);

                var preco = imovel.DocumentNode.SelectSingleNode("//*[@class=\"xxxx-large not-arranged\"]").InnerText;
                var contactos = imovel.DocumentNode.SelectNodes("//*[@class=\"offer-sidebar__buttons contact_methods\"]");
            }

            /*foreach (var preco in preco_nodes)
            {
                var x = replaceHTMLChars(preco.InnerText.Replace(".", "")); // we need to remove dots from price
            }*/
        }
    }
}
