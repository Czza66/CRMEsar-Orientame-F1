using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.MenuVM
{
    public class menuVM
    {
        public string Nombre {get; set;}
        public string Area { get; set;}
        public string Controller {get; set;}
        public string Action {get; set;}
        public int Orden {get; set;}
    }
}
