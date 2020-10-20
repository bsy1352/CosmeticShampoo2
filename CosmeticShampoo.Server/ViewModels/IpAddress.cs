using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShampoo.Server.ViewModels
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
    }
}
