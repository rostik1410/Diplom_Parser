using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace Diplom_Parser
{
    class Parser
    {
        public Parser()
        {

        }    
        //32 товара
        public HtmlDocument Get_Catalog()
        {
            var url = "https://rozetka.com.ua/ua/mobile-phones/c80003/preset=smartfon/";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            return doc;
        }
        //кожен товар з 32
        public void Get_Product_Description(HtmlDocument doc)
        {
            var Node = doc.DocumentNode
                          .Descendants("div")
                          .Where(d => d.Attributes.Contains("class") &&
                          d.Attributes["class"].Value=="g-i-tile g-i-tile-catalog");

            StreamWriter sw = new StreamWriter(new FileStream("NodeInfoText.txt", FileMode.Create, FileAccess.Write));
            foreach (var n in Node)
            {
                sw.Write("\n //////////// \n");
                sw.Write(n.InnerText);
                sw.Write("\n //////////// \n");
                
            }
            sw.Close();
        }

        public void Get_Product_Reviews(string Hrml_code)
        {

        }
    }
}
