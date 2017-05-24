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

                    if (!string.IsNullOrEmpty(link))
                    {
                        var imovel = Webget.Load(link);

                        var preco = imovel.DocumentNode.SelectSingleNode("//*[@class=\"detailPropertyPrice\"]").InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", "");

                        //var gps = imovel.DocumentNode.SelectNodes("//*[@id=\"MapaGis\"]/script[2]");

                        foreach (var script in imovel.DocumentNode.Descendants("script").ToArray())
                        {
                            string s = script.InnerText;

                            if (s.Contains("GoogleMap"))
                            {
                                var d = s;
                            }

                            HtmlTextNode text =
                                (HtmlTextNode) script.ChildNodes.Single(x => x.NodeType == HtmlNodeType.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}