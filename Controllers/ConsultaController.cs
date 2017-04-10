using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using WebApi.Repositorio;

namespace WebApi.Controllers
{
    public class ConsultaController : ApiController
    {
        [HttpGet]
        public List<mOrdenPago> ObtenerOrdenes(int idBanco, int idSucursal, string moneda)
        {
            rOrdenPago dao = new rOrdenPago();
            List<mOrdenPago> lst = dao.ListaOrdenesSucursal(idBanco, idSucursal, moneda);
            return lst;
        }
    }
}
