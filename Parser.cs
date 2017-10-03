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
        private SqlConnection conn;
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
            if (product.description == null || product.description =="")
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
                sr.Close();
                
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
            conn.Close();
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

        public string Get_Product_Reviews(string product_name, string url_img)
        {
            string revs = "";
            List<string> reviews = new List<string>();
            Connect_To_DB();
            var query = "select Id from Product_Info where Product_name='" + product_name + "' and Product_img_link='" + url_img + "'";
            SqlDataAdapter DataAdapter = new SqlDataAdapter(query, conn);
            DataTable DataTable = new DataTable();

            DataAdapter.Fill(DataTable);
            int product_id = Convert.ToInt32(DataTable.Rows[0].ItemArray[0].ToString());

            query = "select * From Product_Reviews where Product_ID='" + product_id + "'";
            DataAdapter = new SqlDataAdapter(query, conn);
            DataTable = new DataTable();

            DataAdapter.Fill(DataTable);

            if (DataTable.Rows.Count == 0)
            {
                var web = new HtmlWeb();
                var doc = new HtmlDocument();
                try
                {
                    doc = web.Load(product.url + "comments");
                }
                catch (Exception)
                {
                    MessageBox.Show("Проблеми з'єднання... Перевірте та спробуйте знову.");
                    return "";
                }
                StreamWriter sw = new StreamWriter(new FileStream("reviews.txt", FileMode.Create, FileAccess.Write));

                sw.Write(doc.DocumentNode.OuterHtml);

                sw.Close();

                var Node = doc.DocumentNode.Descendants("span")
                               .Where(d => d.Attributes.Contains("class") &&
                               d.Attributes["class"].Value.Equals("pp-review-heading-title-inner"));
                string column = "";
                foreach (var n in Node)
                {
                    column += n.InnerText;
                }

                if (column.Contains("&thinsp;"))
                {
                    var col = "";
                    int index = column.IndexOf("&thinsp;");
                    for (int i = 0; i < column.Length - 1; i++)
                    {
                        if (i < index || i > index + 7)
                        {
                            col += column[i];
                        }
                    }
                    column = col;
                }
                int count_review = Convert.ToInt32(column);
                MessageBox.Show("DONE!" + count_review);

                if (count_review > 10)
                {
                    int count = 0;
                    if (count_review % 10 > 0)
                    {
                        count = (count_review / 10) + 1;
                    }
                    else
                    {
                        count = count_review / 10;
                    }
                    for (int i = 0; i < count; i++)
                    {
                        if (i == 0)
                        {
                            Node = doc.DocumentNode.Descendants("div")
                                                   .Where(d => d.Attributes.Contains("class") &&
                                                   d.Attributes["class"].Value.Equals("pp-review-text"));
                            // sw = new StreamWriter(new FileStream("reviews_Page" + (i + 1) + ".txt", FileMode.Create, FileAccess.Write));
                            foreach (var n in Node)
                            {
                                string block = n.InnerText;
                                if (n.InnerText.Contains("'"))
                                {
                                    block = block.Replace("'","`");
                                }
                                query = "insert Product_Reviews(Product_ID, Review) VALUES('" + product_id + "', '" + block + "')";
                                DataAdapter = new SqlDataAdapter(query, conn);
                                DataTable = new DataTable();

                                DataAdapter.Fill(DataTable);
                                revs += n.InnerText;
                            }
                            // sw.Close();
                        }
                        else
                        {
                            doc = web.Load(product.url + "comments/page=" + (i + 1) + "/");
                            Node = doc.DocumentNode.Descendants("div")
                                                   .Where(d => d.Attributes.Contains("class") &&
                                                   d.Attributes["class"].Value.Equals("pp-review-text"));
                            // sw = new StreamWriter(new FileStream("reviews_Page" + (i + 1) + ".txt", FileMode.Create, FileAccess.Write));
                            foreach (var n in Node)
                            {
                                string block = n.InnerText;
                                if (n.InnerHtml.Contains("'"))
                                {
                                    block = block.Replace("'", "`");         
                                }
                                query = "insert Product_Reviews(Product_ID, Review) VALUES('" + product_id + "', '" + block + "')";
                                DataAdapter = new SqlDataAdapter(query, conn);
                                DataTable = new DataTable();

                                DataAdapter.Fill(DataTable);
                                revs += n.InnerText;
                            }
                             //sw.Close();
                        }
                    }
                }
            }
            else
            {
                for(int i =0; i< DataTable.Rows.Count; i++)
                {
                    revs += DataTable.Rows[i].ItemArray[2].ToString()+Environment.NewLine;
                }
            }
            conn.Close();
            MessageBox.Show("Success DONE!!!!");
            return revs;
        }
    }
}