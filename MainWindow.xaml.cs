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
            foreach(var p in product_list)
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
        private void menu_icon_png_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (HamburgerMenu.Margin.Left < 0)
            {
                HamburgerMenu.Margin = new Thickness(0, 0, 0, 0);
                menu_icon_png.Source = new BitmapImage(new Uri(@"images\arrow_left_icon.png", UriKind.Relative));
            }
            else
            {
                HamburgerMenu.Margin = new Thickness(-200, 0, 0, 0);
                menu_icon_png.Source = new BitmapImage(new Uri(@"images\menu_icon.png", UriKind.Relative));
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
            
             parser.Get_Product_Description_From_DB("notebook");
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

            parser.Get_Product_Description_From_DB("phone");
            Take_info_in_form(parser.product_list);
        }
    }
}
