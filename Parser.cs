using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace Diplom_Parser
{
   public class Parser
    {
        public Product product;
        public List<Product> product_list = new List<Product>();
        public Parser()
        {

        }    
        //32 товара
        public HtmlDocument Get_Catalog(string url , int page)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url+Convert.ToString(page)+'/');
            return doc;
        }
        //кожен товар з 32
        public void Get_Product_Description(/*HtmlDocument doc*/ string file_for_read)
        {
            // ONLINE PARSING FOR GET DATA
            // GET BLOCK WITH NAME,URL,SRC AND DESCRIPTION
            /*  var Node = doc.DocumentNode
                            .Descendants("div")
                            .Where(d => d.Attributes.Contains("class") &&
                            d.Attributes["class"].Value== "g-i-tile g-i-tile-catalog");

              StreamWriter sw = new StreamWriter(new FileStream("NoteInfoHtml.txt", FileMode.Create, FileAccess.Write), Encoding.GetEncoding(1251));
              foreach (var n in Node)
              {
                  sw.Write("\n //////////// \n");
                  sw.Write(n.InnerHtml);
                  sw.Write("\n //////////// \n");

              }
              sw.Close();*/


            // GET BLOCK WITH NAME,URL AND SRC WITHOUT DESCRIPTION
            /*
            var node_doc = new HtmlDocument();
            node_doc.Load("NoteInfoHtml.txt", Encoding.GetEncoding(1251));
            var info_node = node_doc.DocumentNode
                                .Descendants("div")
                                .Where(d=>d.Attributes.Contains("class")&&
                                d.Attributes["class"].Value.Contains("g-i-tile-i-image fix-height"));

            StreamWriter siw = new StreamWriter(new FileStream("NoteInfo.txt", FileMode.Create, FileAccess.Write ),Encoding.GetEncoding(1251));
            foreach (var n in info_node)
            {
                var name = Get_Name_From_Node(n);
                var url = Get_Url_From_Node(n);
                var img = Get_Img_From_Node(n);
                siw.WriteLine(name);
                siw.WriteLine(url);
                siw.WriteLine(img);
                siw.Write(n.InnerHtml);
                siw.Write(Environment.NewLine);
                product_list.Add(new Product(name, url, "", img));
            }
            siw.Close();*/

            // ONLY FOR OFLINE QUIKE WORK
            var name = "";
            var url = "";
            var img ="";
            StreamReader sr = new StreamReader(new FileStream(file_for_read, FileMode.Open, FileAccess.Read), Encoding.GetEncoding(1251));
            {
                int i = 0;
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    if (i == 0)
                    {
                        name = line;
                    }
                    else if (i == 1)
                    {
                        url = line;
                    }
                    else if (i == 2)
                    {
                        img = line;
                    }
                    else if (i > 5)
                    {
                        if (line == "") i = -1;
                    }
                    i++;
                    if (name != "" && url != "" && img != "")
                    {
                        product_list.Add(new Product(name, url, "", img));
                        name = url = img = "";
                    }
                }

            }
        }

        private string Get_Name_From_Node(HtmlNode n)
        {
            var text=n.InnerHtml;
            var index = text.IndexOf("title")+7;
            var name = "";
            for(int i = index; i< text.Length; i++)
            {
                if (text[i] == '"') break;
                name += text[i];
            }
            return name;
        }
       
        private string Get_Url_From_Node(HtmlNode n)
        {
            var text = n.InnerHtml;
            var index = text.IndexOf("a href")+8;
            var url = "";
            for (int i = index; i < text.Length; i++)
            {
                if (text[i] == '"') break;
                url += text[i];
            }
            return url;
        }

        private string Get_Img_From_Node(HtmlNode n)
        {
            var text = n.InnerHtml;
            var index = text.IndexOf("img src") + 9;
            var img = "";
            for (int i = index; i < text.Length; i++)
            {
                if (text[i] == '"') break;
                img += text[i];
            }
            return img;
        }

        public void Get_Product_Reviews(string Hrml_code)
        {

        }
    }
}
