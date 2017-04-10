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
    public class OrdenPagoController : ApiController
    {
        [HttpGet]
        public List<mOrdenPago> ObtenerOrdenes(string filtro = "")
        {
            rOrdenPago dao = new rOrdenPago();
            List<mOrdenPago> lst = dao.ListaOrdenes(filtro);
            return lst;
        }

        [HttpPost]
        public IHttpActionResult MantOrden(string sAccion, [FromBody] mOrdenPago modelo)
        {
            bool bResultado = false;
            string sMensaje = string.Empty;

            rOrdenPago dao = new rOrdenPago();
            bResultado = dao.Mantenimiento(sAccion, modelo, ref sMensaje);

            return Ok(new { resultado = bResultado, mensaje = sMensaje });
        }
    }
}
