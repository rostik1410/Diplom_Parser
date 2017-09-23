using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_Parser
{
    public class Product
    {
        public string name;
        public string url;
        public string image;
        public string description;

        public Product(string _name, string _url, string _image, string _description)
        {
            name = _name;
            url = _url;
            image = _image;
            description = _description;
        }
    }
}
