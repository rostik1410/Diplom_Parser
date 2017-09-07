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
            /*parser = new Parser();
            doc = parser.Get_Catalog();
            textBlock1.Text = doc.DocumentNode.OuterHtml;
            /* StreamWriter sw = new StreamWriter(new FileStream("FileName.txt", FileMode.Create, FileAccess.Write));
             sw.Write(textBlock1.Text);
             sw.Close();*/
           // parser.Get_Product_Description(doc);
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
    }
}
