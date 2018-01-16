using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HtmlAgilityPack;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

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
        string current_category;
        List<Product> product_list;
        bool catalog_viewed;
        List<Product> list;
        public MainWindow()
        {
            InitializeComponent();
            parser = new Parser();
            product_list = new List<Product>();
            /*doc = parser.Get_Catalog();
             parser.Get_Product_Description(doc); 
            Take_info_in_form(parser.product_list);*/

        }

        public void Take_info_in_form(List<Product> product_list)
        {
            grid2.Children.Clear();
            int i = 0, j = 0, top=0;
            foreach (var p in product_list)
            {
                top = (j==0) ? 50 : 0;
                Product_block pb = new Product_block();
                pb.img.Source = new BitmapImage(new Uri(p.image));
                pb.Prod_name.Text = p.name;

                pb.Height = 200;
                pb.Width = 150;
                             
                switch (i)
                {
                    case 0:
                        pb.Margin = new Thickness(36, top + 20, 0, 0);
                        break;
                    case 1:
                        pb.Margin = new Thickness(10, top + 20, 0, 0);
                        break;

                    case 2:
                        pb.Margin = new Thickness(0, top + 20, 10, 0);
                        break;

                    case 3:
                        pb.Margin = new Thickness(0, top + 20, 36 , 0);
                        break;

                    case 4:
                        i = 0;
                        j++;
                        grid2.RowDefinitions.Add(new RowDefinition());
                        pb.Margin = new Thickness(36, 20, 0, 0);
                        break;
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
            Notebook_Filter.Visibility = Visibility.Visible;
            title.Visibility = Visibility.Visible;

            //Stopwatch stw = new Stopwatch();
            //stw.Start();
            /*for (int i = 1; i <= 64; i++)
            {
                doc = new HtmlDocument();
                parser.Get_Catalog(url, i,"notebook");
            }*/
            //stw.Stop();
            //TimeSpan ts = stw.Elapsed;
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //ts.Hours, ts.Minutes, ts.Seconds,
            //ts.Milliseconds / 10);
            //MessageBox.Show("Час виконання парсингу ноутбуків " + elapsedTime);
            //parser.Get_Catalog_ofline("NoteBookName.txt", "notebook");
            current_category = "notebook";
            catalog_viewed = true;
            parser.Get_Product_Information_From_DB("notebook");
            product_list = parser.product_list;
            Take_info_in_form(product_list);
        }

        private void phone_btn_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://rozetka.com.ua/ua/mobile-phones/c80003/page=";
            parser = new Parser();
            Phone_Filter.Visibility = Visibility.Visible;
            title.Visibility = Visibility.Visible;
            /*for (int i = 1; i <= 13; i++)
            {
                doc = new HtmlDocument();
                parser.Get_Catalog(url, i, "phone");
            }*/

            //parser.Get_Catalog_ofline("FileName.txt", "phone");
            current_category = "phone";
            catalog_viewed = true;
            parser.Get_Product_Information_From_DB("phone");
            product_list = parser.product_list;
            Take_info_in_form(product_list);
        }

        private void tv_btn_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://rozetka.com.ua/ua/all-tv/c80037/page=";
            parser = new Parser();
            TV_Filter.Visibility = Visibility.Visible;
            title.Visibility = Visibility.Visible;

            /* for (int i = 1; i <= 13; i++)
             {
                 doc = new HtmlDocument();
                 parser.Get_Catalog(url, i, "tv");
             }*/
            current_category = "tv";
            catalog_viewed = true;
            parser.Get_Product_Information_From_DB("tv");
            product_list = parser.product_list;
            Take_info_in_form(product_list);
        }

        private void phototech_btn_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://rozetka.com.ua/ua/photo/c80001/filter/page=";
            parser = new Parser();
            title.Visibility = Visibility.Visible;

            //Stopwatch stw = new Stopwatch();
            //stw.Start();
            /* for (int i = 1; i <= 10; i++)
             {
                 doc = new HtmlDocument();
               
                 parser.Get_Catalog(url, i, "photo");
             }*/
            //stw.Stop();
            //TimeSpan ts = stw.Elapsed;
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //ts.Hours, ts.Minutes, ts.Seconds,
            //ts.Milliseconds / 10);
            //MessageBox.Show("Час виконання парсингу фотоапаратів " + elapsedTime);
            current_category = "photo";
            catalog_viewed = true;
            parser.Get_Product_Information_From_DB("photo");
            product_list = parser.product_list;
            Take_info_in_form(product_list);
        }

        private void grid2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string descrip ="";
            List<string> reviews = new List<string>();
            Description_TB.Text = "";
            if (grid2.Children.Count > 0)
            {
                for (int i = 0; i < grid2.Children.Count; i++)
                {
                    if (grid2.Children[i].IsMouseOver == true)
                    {
                        Phone_Filter.Visibility = Visibility.Hidden;
                        Notebook_Filter.Visibility = Visibility.Hidden;
                        title.Visibility = Visibility.Hidden;
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

                //parser.updateDataBase();
                descrip = parser.Get_Description_From_Node(product_name, url_img);
                Description_TB.Text = descrip;
                Reviews_TB.Text = "";
                reviews = parser.Get_Product_Reviews(product_name, url_img);
                foreach (var rev in reviews)
                {
                    Reviews_TB.Text += rev;
                }
                IList<SentimentBatchResultItem> result = new List<SentimentBatchResultItem>();

                SentimentAnalysis sa = new SentimentAnalysis();
                result = sa.getSentimentalClass(reviews);


                int count_positive = 0;
                foreach (var item in result)
                {
                    if (item.Score >= 0.5)
                    {
                        count_positive++;
                    }
                }

                rews_diff.Text = "Кількість позитивних відгуків " + count_positive + "/" + result.Count;

                List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
                valueList.Add(new KeyValuePair<string, int>("Позитивні", count_positive));
                valueList.Add(new KeyValuePair<string, int>("Негативні", result.Count - count_positive));

                // Setting data for pie chart
                pieChart.DataContext = valueList;

                if (result.Count < 10)
                {
                    decision.Text = "Відгуків занадто мало для коректного прийняття рішення.";
                }
                else
                {
                    double one = Convert.ToDouble(count_positive), two = Convert.ToDouble(result.Count);
                    double resul = one / two;
                    if (resul > 0.55)
                    {
                        decision.Text = "Товар рекомендовано до покупки.";
                    }
                    else
                    {
                        decision.Text = "Товар не може бути рекомендований до покупки.";
                    }
                }


            }

            //FILL DESCRIPTION FIELD IN DB
            //Stopwatch stw = new Stopwatch();
            //stw.Start();
            /*for (int i =0; i< grid2.Children.Count; i++)
            {
                var product = (Product_block)grid2.Children[i];
                product_name = product.Prod_name.Text;
                url_img = product.img.Source.ToString();

                descrip = parser.Get_Description_From_Node(product_name, url_img);
            }*/
            //stw.Stop();
            //TimeSpan ts = stw.Elapsed;
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //ts.Hours, ts.Minutes, ts.Seconds,
            //ts.Milliseconds / 10);
            //MessageBox.Show("Час виконання парсингу опису фотоапаратів " + elapsedTime);
        }

        private void back_to_menu_icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Product_grid.Visibility == Visibility.Visible)
            {
                Product_grid.Visibility = Visibility.Hidden;
                scrollview_product.Visibility = Visibility.Hidden;
                Phone_Filter.Visibility = Visibility.Hidden;
                Notebook_Filter.Visibility = Visibility.Hidden;
                TV_Filter.Visibility = Visibility.Hidden;
                title.Visibility = Visibility.Visible;
                switch (current_category)
                {
                    case "notebook":
                        Notebook_Filter.Visibility = Visibility.Visible;
                        break;

                    case "phone":
                        Phone_Filter.Visibility = Visibility.Visible;
                        break;

                    case "tv":
                        TV_Filter.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void back_to_catalog_icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Phone_Filter.Visibility = Visibility.Hidden;
            Notebook_Filter.Visibility = Visibility.Hidden;
            TV_Filter.Visibility = Visibility.Hidden;
            title.Visibility = Visibility.Hidden;

            if (grid2.Children.Count < product_list.Count)
            {
                catalog_viewed = true;
                Take_info_in_form(product_list);
                title.Visibility = Visibility.Visible;
                switch (current_category)
                {
                    case "notebook":
                        Notebook_Filter.Visibility = Visibility.Visible;
                        break;

                    case "phone":
                        Phone_Filter.Visibility = Visibility.Visible;
                        break;

                    case "tv":
                        TV_Filter.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void search_icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            list = new List<Product>(product_list);
            for(int i=0;i<list.Count;i++)
            {
                if(!list[i].name.ToLower().Contains(searchTB.Text.ToLower()))
                {
                    list.Remove(list[i]);
                    i--;
                }
            }
            catalog_viewed = false;
            Take_info_in_form(list);
        }

        private void PhoneShow_Click(object sender, RoutedEventArgs e)
        {
            if(catalog_viewed == true)
            {
                list = new List<Product>(product_list);
            }
            
            List<double> display = new List<double>();
            List<double> ozu = new List<double>();
            List<string> os_version = new List<string>();
            List<int> pzu = new List<int>();

            //DISPLAY------------------------------------
            if (l4.IsChecked == true)
            {
                for (double i = 3; i <= 4; i+=0.1)
                {
                    display.Add(Math.Round(i, 1));
                }
                
            }
            if(m4to5.IsChecked == true)
            {
                for(double i = 4.1; i<=5; i+=0.1)
                {
                    display.Add(Math.Round(i, 1));
                }
            }
            if(m5to5and5.IsChecked == true)
            {
                for(double i=5.05; i<=5.5; i+=0.05)
                {
                    display.Add(Math.Round(i,2));
                }
            }
            if(m5and5.IsChecked == true)
            {
                for(double i=5.6; i<7; i+=0.1)
                {
                    display.Add(Math.Round(i,1));
                }
            }
            if (display.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    bool check = false;
                    for (int j = 0; j < display.Count; j++)
                    {
                        if (!list[i].description.ToLower().Contains("діагональ екрана\r\n" + display[j].ToString().Replace(',','.') + " \""))
                        {
                            check = false;
                        }
                        else
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        list.Remove(list[i]);
                        i--;
                    }
                }
            }

            //OZU----------------------------------------
            if(to1.IsChecked == true)
            {
                for (double i = 0; i <= 1024; i += 256)
                {
                    ozu.Add(i);
                }
                ozu.Add(1);
            }
            if (m1to2.IsChecked == true)
            {
                for (double i = 1; i <= 2; i += 0.5)
                {
                    ozu.Add(i);
                }
            }
            if (m2to3.IsChecked == true)
            {
                for (double i = 2.5; i <= 3; i += 0.5)
                {
                    ozu.Add(i);
                }
            }
            if (m3.IsChecked == true)
            {
                for (double i = 3.5; i <= 8; i += 0.5)
                {
                    ozu.Add(i);
                }
            }
            if (ozu.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    bool check = false;
                    for (int j = 0; j < ozu.Count; j++)
                    {
                        if (!list[i].description.Contains("Оперативна пам`ять\r\n" + ozu[j].ToString().Replace(',','.') + " Г"))
                        {
                            check = false;
                        }
                        else
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        list.Remove(list[i]);
                        i--;
                    }
                }
            }

            //OS-----------------------------------------
            if(IOS.IsChecked==true)
            {
                os_version.Add("iOS");
            }
            if(Android.IsChecked==true)
            {
                os_version.Add("Android");
            }
            
            if(os_version.Count>0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    bool check = false;
                    for (int j = 0; j < os_version.Count; j++)
                    {
                        if (!list[i].description.Contains("Операційна система\r\n" + os_version[j]))
                        {
                            check = false;
                        }
                        else
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        list.Remove(list[i]);
                        i--;
                    }
                }
            }

            //PZU----------------------------------------
            if (l32.IsChecked == true)
            {
                for (int i = 0; i < 32; i += 2)
                {
                    pzu.Add(i);
                }
            }
            if (m32l64.IsChecked == true)
            {
                for (int i = 32; i <= 64; i += 4)
                {
                    pzu.Add(i);
                }
            }
            if (m65l128.IsChecked == true)
            {
                for (int i = 68; i <= 128; i += 4)
                {
                    pzu.Add(i);
                }
            }
            if (m128.IsChecked == true)
            {
                for (int i = 128; i <= 256; i += 32)
                {
                    pzu.Add(i);
                }
            }
            if (pzu.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    bool check = false;
                    for (int j = 0; j < pzu.Count; j++)
                    {
                        if (!list[i].description.Contains("Вбудована пам`ять\r\n" + pzu[j].ToString().Replace(',', '.') + " Г"))
                        {
                            check = false;
                        }
                        else
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        list.Remove(list[i]);
                        i--;
                    }
                }
            }

            catalog_viewed = false;
            Take_info_in_form(list);
        }

        private void NotebookShow_Click(object sender, RoutedEventArgs e)
        {
            if (catalog_viewed == true)
            {
                list = new List<Product>(product_list);
            }

            List<double> display = new List<double>();
            List<int> ozu = new List<int>();
            List<string> cpu = new List<string>();
            List<string> gpu = new List<string>();

            //DISPLAY------------------------------------
            if (nb_m9l12and5.IsChecked == true)
            {
                for (double i = 9; i <= 12.5; i+=0.2)
                {
                    display.Add(i);
                }

            }
            if (nb_13.IsChecked == true)
            {
                display.Add(13);
            }
            if (nb_14.IsChecked == true)
            {
                display.Add(14);
            }
            if (nb_m15l15and6.IsChecked == true)
            {
                for (double i = 15; i <=15.6; i+=0.6)
                {
                    display.Add(i);
                }
            }
            if(nb_m16l17.IsChecked == true)
            {
                for(double i=16;i<=17;i+=0.5)
                {
                    display.Add(i);
                }
            }
            if (display.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    bool check = false;
                    for (int j = 0; j < display.Count; j++)
                    {
                        if (!list[i].description.ToLower().Contains("діагональ екрана\r\n" + display[j].ToString().Replace(',', '.') + "\""))
                        {
                            check = false;
                        }
                        else
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        list.Remove(list[i]);
                        i--;
                    }
                }
            }


            //OZU
            if (nb_2_4.IsChecked == true)
            {
                for (int i = 2; i <= 4; i ++)
                {
                    ozu.Add(i);
                }
            }
            if (nb_6.IsChecked == true)
            {
                ozu.Add(6);
            }
            if (nb_8.IsChecked == true)
            {
                ozu.Add(8);
            }
            if (nb_12_16.IsChecked == true)
            {
                ozu.Add(12);
                ozu.Add(16);
            }
            if(nb_m16.IsChecked == true)
            {
                for(int i=20;i<=64;i+=4)
                {
                    ozu.Add(i);
                }
            }
            if (ozu.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    bool check = false;
                    for (int j = 0; j < ozu.Count; j++)
                    {
                        if (!list[i].description.Contains("оперативної пам`яті\r\n" + ozu[j].ToString().Replace(',', '.') + " Г"))
                        {
                            check = false;
                        }
                        else
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        list.Remove(list[i]);
                        i--;
                    }
                }
            }
        
            //CPU

            if(nb_IntelCeleron.IsChecked==true)
            {
                cpu.Add("Intel Celeron");
            }
            if(nb_IntelPentium.IsChecked==true)
            {
                cpu.Add("Intel Pentium");
            }
            if(nb_IntelCore.IsChecked==true)
            {
                cpu.Add("Intel Core");
            }
            if(nb_AMDA.IsChecked==true)
            {
                cpu.Add("AMD A");
            }
            if (nb_AMDFX.IsChecked == true)
            {
                cpu.Add("AMD FX");
            }
            if (nb_AMDRyzen.IsChecked == true)
            {
                cpu.Add("AMD Ryzen");
            }
            if(cpu.Count>0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    bool check = false;
                    for (int j = 0; j < cpu.Count; j++)
                    {
                        if (!list[i].description.Contains(cpu[j]))
                        {
                            check = false;
                        }
                        else
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        list.Remove(list[i]);
                        i--;
                    }
                }
            }

            //GPU

            if(nb_HDgraphics.IsChecked==true)
            {
                gpu.Add("HD Graphics");
            }
            if (nb_Irisgraphics.IsChecked == true)
            {
                gpu.Add("Iris Graphics");
            }
            if (nb_GeForce.IsChecked == true)
            {
                gpu.Add("GeForce");
            }
            if(nb_AMDRadeon.IsChecked==true)
            {
                gpu.Add("Radeon");
            }
            if(gpu.Count>0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    bool check = false;
                    for (int j = 0; j < gpu.Count; j++)
                    {
                        if (!list[i].description.Contains(gpu[j]))
                        {
                            check = false;
                        }
                        else
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        list.Remove(list[i]);
                        i--;
                    }
                }
            }

            Take_info_in_form(list);

        }

        private void TVShow_Click(object sender, RoutedEventArgs e)
        {
            if (catalog_viewed == true)
            {
                list = new List<Product>(product_list);
            }

            List<int> display = new List<int>();
            List<string> smart = new List<string>();

            //DISPLAY------------------------------------
            if (m20l30.IsChecked == true)
            {
                for (int i = 20; i <= 30; i ++)
                {
                    display.Add(i);
                }

            }
            if (m30l40.IsChecked == true)
            {
                for (int i = 31; i <= 40; i ++)
                {
                    display.Add(i);
                }
            }
            if (m40l60.IsChecked == true)
            {
                for (int i = 40; i <= 60; i ++)
                {
                    display.Add(i);
                }
            }
            if (m60.IsChecked == true)
            {
                for (int i = 60; i < 120; i ++)
                {
                    display.Add(i);
                }
            }
            if (display.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    bool check = false;
                    for (int j = 0; j < display.Count; j++)
                    {
                        if (!list[i].description.ToLower().Contains("діагональ екрана\r\n" + display[j].ToString().Replace(',', '.') + "\""))
                        {
                            check = false;
                        }
                        else
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        list.Remove(list[i]);
                        i--;
                    }
                }
            }

            if(smartyes.IsChecked==true)
            {
                smart.Add("Є");
            }
            if(smartno.IsChecked ==true)
            {
                smart.Add("Немає");
            }
            if (smart.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    bool check = false;
                    for (int j = 0; j < smart.Count; j++)
                    {
                        if (!list[i].description.Contains("Підтримка Smart TV\r\n" + smart[j] + ""))
                        {
                            check = false;
                        }
                        else
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        list.Remove(list[i]);
                        i--;
                    }
                }
            }

            Take_info_in_form(list);
        }
    }
}