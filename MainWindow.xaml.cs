using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using HtmlAgilityPack;

namespace Diplom_Parser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string product_name;
        string url_img;
        HtmlDocument doc = new HtmlDocument();
        Parser parser;
        public MainWindow()
        {
            InitializeComponent();
            parser = new Parser();

            /*doc = parser.Get_Catalog();
             parser.Get_Product_Description(doc); 
            Take_info_in_form(parser.product_list);*/

        }

        public void Take_info_in_form(List<Product> product_list)
        {
            int i = 0, j = 0;
            foreach (var p in product_list)
            {
                Product_block pb = new Product_block();
                pb.img.Source = new BitmapImage(new Uri(p.image));
                pb.Prod_name.Text = p.name;

                pb.Height = 200;
                pb.Width = 150;
                pb.Margin = new Thickness(0, 10, 0, 30);
                if (i == 4)
                {
                    i = 0;
                    j++;

                    grid2.RowDefinitions.Add(new RowDefinition());
                }
                grid2.Children.Add(pb);
                Grid.SetColumn(pb, i);
                Grid.SetRow(pb, j);
                i++;
            }
        }

        private void notebook_btn_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://rozetka.com.ua/ua/notebooks/c80004/filter/page=";
            parser = new Parser();
            /*for (int i = 1; i <= 60; i++)
            {
                doc = new HtmlDocument();
                doc=parser.Get_Catalog(url, i,"notebook");
                StreamWriter sw = new StreamWriter(new FileStream("NoteBookName.txt", FileMode.Append, FileAccess.Write));
                sw.Write(doc.DocumentNode.OuterHtml);
                if (i % 10 == 0)
                {
                    MessageBox.Show(i + " DONE!!!!");
                }
                sw.Close();
            }*/

            //parser.Get_Catalog_ofline("NoteBookName.txt", "notebook");

            parser.Get_Product_Information_From_DB("notebook");
            Take_info_in_form(parser.product_list);
        }

        private void phone_btn_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://rozetka.com.ua/ua/mobile-phones/c80003/preset=smartfon/";
            parser = new Parser();
            /*for (int i = 1; i <= 13; i++)
            {
                doc = new HtmlDocument();
                doc=parser.Get_Catalog(url, i, "phone");
                StreamWriter sw = new StreamWriter(new FileStream("FileName.txt", FileMode.Append, FileAccess.Write));
                sw.Write(doc.DocumentNode.OuterHtml);
                sw.Close();
            }*/

            //parser.Get_Catalog_ofline("FileName.txt", "phone");

            parser.Get_Product_Information_From_DB("phone");
            Take_info_in_form(parser.product_list);
        }

        private void grid2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string descrip ="";
            string revs = "";
            Description_TB.Text = "";
            if (grid2.Children.Count > 0)
            {
                for (int i = 0; i < grid2.Children.Count; i++)
                {
                    if (grid2.Children[i].IsMouseOver == true)
                    {
                        var product = (Product_block)grid2.Children[i];
                        product_name = product.Prod_name.Text;
                        url_img = product.img.Source.ToString();
                        Product_grid.Visibility = Visibility.Visible;
                        scrollview_product.Visibility = Visibility.Visible;
                        Prod_name_LB.Content = product_name;
                        Description_IMG.Source = new BitmapImage(new Uri(url_img));
                        break;
                    }

                }
                descrip = parser.Get_Description_From_Node(product_name, url_img);
                Description_TB.Text = descrip;

                revs = parser.Get_Product_Reviews(product_name, url_img);
                Reviews_TB.Text = revs;
            }

            //FILL DESCRIPTION FIELD IN DB
            /*for (int i =0; i< grid2.Children.Count; i++)
            {
                var product = (Product_block)grid2.Children[i];
                product_name = product.Prod_name.Text;
                url_img = product.img.Source.ToString();

                descrip = parser.Get_Description_From_Node(product_name, url_img);
            }*/
        }

        private void back_to_menu_icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Product_grid.Visibility == Visibility.Visible)
            {
                Product_grid.Visibility = Visibility.Hidden;
                scrollview_product.Visibility = Visibility.Hidden;
            }
        }
    }
}