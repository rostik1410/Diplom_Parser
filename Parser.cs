using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Diplom_Parser
{
   public class Parser
    {
        public SqlConnection conn;
        private string conn_string = @"Data Source=DESKTOP-KSC06U9;"
                           + "Initial Catalog=Product; integrated Security=true;";
        public Product product;
        public List<Product> product_list = new List<Product>();
        public Parser()
        {

        }    
        //1 PAGE WITH PRODUCTS
        public void Get_Catalog(string url , int page, string category)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url+Convert.ToString(page)+'/');
            Get_Product_Information(doc, category);
        }

        //THIS PART OF CODE CREATED FOR ONE TIME FOR CREATED DATABASE BECAUSE NOW WE ARE HAVE DATA WHAT WE NEED IN FILE 
        //THIS FILES WILL DROP AFTER DATABASE FILLING
        public void Get_Catalog_ofline(string filename, string category)
        {
            var doc = new HtmlDocument();
            doc.Load(filename, Encoding.GetEncoding(1251));
            Get_Product_Information(doc, category);
        }

        // GET BLOCK WITH NAME,URL,SRC AND DESCRIPTION
        public void Get_Product_Information(HtmlDocument doc,string category)
        {
            var Node = doc.DocumentNode
                          .Descendants("div")
                          .Where(d => d.Attributes.Contains("class") &&
                          d.Attributes["class"].Value.Contains("g-i-tile-i-image fix-height"));

            foreach (var n in Node)
            {
                var name = Get_Name_From_Node(n);
                var url = Get_Url_From_Node(n);
                var img = Get_Img_From_Node(n);
                product_list.Add(new Product(name, url, img, ""));
            }
          
            Add_To_DB(product_list, category);    
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

        public string Get_Description_From_Node(string product_name, string url_img)
        {
            string desc = "";
            List<string> descrip = new List<string>();
            Connect_To_DB();
            var query = "select * from Product_Info where Product_name='" + product_name + "' and Product_img_link='" + url_img + "'";
            SqlDataAdapter DataAdapter = new SqlDataAdapter(query, conn);
            DataTable DataTable = new DataTable();

            DataAdapter.Fill(DataTable);
            product = new Product(DataTable.Rows[0].ItemArray[2].ToString(), DataTable.Rows[0].ItemArray[3].ToString(), DataTable.Rows[0].ItemArray[4].ToString(), DataTable.Rows[0].ItemArray[5].ToString());
            if (product.description == "")
            {
                var web = new HtmlWeb();

                var doc = web.Load(product.url + "characteristics");

                /* StreamWriter sw = new StreamWriter(new FileStream("description.txt", FileMode.Append, FileAccess.Write));

                   sw.Write(doc.DocumentNode.OuterHtml);

                   sw.Close();*/
                var Node = doc.DocumentNode
                             .Descendants("div")
                             .Where(d => d.Attributes.Contains("class") &&
                             d.Attributes["class"].Value.Equals("clearfix pp-tab-with-aside-content pp-characteristics-tab"));
                StreamWriter sw = new StreamWriter(new FileStream("description_only.txt", FileMode.Create, FileAccess.Write),Encoding.GetEncoding(1251));

                foreach (var n in Node)
                {
                    //  sw.Write(n.OuterHtml);
                    sw.Write("\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");
                    sw.Write(n.InnerText);
                }
                sw.Close();

                StreamReader sr = new StreamReader(new FileStream("description_only.txt", FileMode.Open, FileAccess.Read), Encoding.GetEncoding(1251));
                {
                    string line = "";
                    while ((line=sr.ReadLine())!= null)
                    {
                        
                        if (line!= "" && !line.Contains("\\\\\\\\"))
                        {
                           if (line.Contains("&nbsp"))
                            {
                                line = line.Substring(0, line.IndexOf("&nbsp"));
                            }

                            if (line.Contains("&#039;"))
                            {
                                line = line.Replace("&#039;", "`");
                            }

                            if (line.Contains("'"))
                            {
                                line = line.Replace("'", "`");
                            }

                            descrip.Add(line);
                        }

                    }
                }
                
                foreach (var i in descrip)
                {
                    desc += i + Environment.NewLine;
                }

                query = "update Product_Info set Product_description='"+desc+"' where Product_name='" + product_name + "'";
                DataAdapter = new SqlDataAdapter(query, conn);
                DataTable = new DataTable();

                DataAdapter.Fill(DataTable);
                //MessageBox.Show("УЗЬО!");
            }
            else
            {
                desc = product.description;
            }
            return desc;
        }

        public SqlConnection Connection
        {
            get { return conn; }
            set { conn = value; }

        }

        public void Add_To_DB(List<Product> product_list , string category)
        {
            Connect_To_DB();

            foreach (var p in product_list)
            {
                var query = "insert into Product_Info(Category,Product_name,Product_link,Product_img_link,Product_description) values('"+category+"','"+p.name+"','"+p.url+"','"+p.image+"','"+p.description+"')";
                SqlDataAdapter DataAdapter = new SqlDataAdapter(query, conn);
                DataTable DataTable = new DataTable();
                DataAdapter.Fill(DataTable);
            }
            MessageBox.Show("Заповнення пройшло успішно!!!!");
            conn.Close();
        }

        public void Get_Product_Information_From_DB( string category)
        {
            product_list = new List<Product>();
            Connect_To_DB();

            var query = "select * from Product_Info where Category='"+category+"'";
            SqlDataAdapter DataAdapter = new SqlDataAdapter(query, conn);
            DataTable DataTable = new DataTable();

            DataAdapter.Fill(DataTable);
            for (int j = 0; j < DataTable.Rows.Count; j++)
            {
              
                product_list.Add(new Product(DataTable.Rows[j].ItemArray[2].ToString(), DataTable.Rows[j].ItemArray[3].ToString(),  DataTable.Rows[j].ItemArray[4].ToString(), DataTable.Rows[j].ItemArray[5].ToString()));
                
            }
            conn.Close();
        }

        public void Connect_To_DB()
        {
            conn = new SqlConnection(conn_string);

            try
            {
                conn.Open();
                string query = "set dateformat dmy";
                SqlCommand cmd3 = new SqlCommand(query, conn);
                cmd3.ExecuteNonQuery();
            }
            catch (SqlException xe)
            {
                if (xe.ToString().Contains("server was not found"))
                    MessageBox.Show("Не вдалось підключитись до бази даних.");
                else if (xe.ToString().Contains("Login failed for user"))
                    MessageBox.Show("                                      Не вірний логін або пароль." + Environment.NewLine + " Будь ласка переконайтесь у його правильності та спробуйте знову.");
            }
        }
        public void Get_Product_Reviews(HtmlDocument doc)
        {

        }
    }
}
