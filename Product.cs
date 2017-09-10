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
        public string description;
        public string image;

        public Product(string _name, string _url, string _description, string _image)
        {
            name = _name;
            url = _url;
            description = _description;
            image = _image;
        }
    }
}
