using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShampoo.Viewer2.ViewModels
{
    public class IpAddress
    {
        private static IpAddress instance = null;

        public static IpAddress Get()
        {
            if (instance == null)
                instance = new IpAddress();

            return instance;
        }
        public string ipAddress { get; set; }
        public string port { get; set; }
    }
}
