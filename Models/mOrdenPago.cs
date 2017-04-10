using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class mOrdenPago
    {
        public int idBanco { get; set; }
        public int idSucursal { get; set; }
        public int idOrdenPago { get; set; }
        public string nomBanco { get; set; }
        public string nomSucursal { get; set; }
        public int nroOrdenPago { get; set; }
        public decimal monto { get; set; }
        public string estado { get; set; }
        public string moneda { get; set; }
        public string fecPago { get; set; }
    }
}