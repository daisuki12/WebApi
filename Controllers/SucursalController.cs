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
    public class SucursalController : ApiController
    {
        [HttpGet]
        public List<mSucursal> ObtenerSucursales(string filtro = "", int idBanco = 0, bool tipoFiltro = false)
        {
            rSucursal dao = new rSucursal();
            List<mSucursal> lst = null;

            if (tipoFiltro)
                lst = dao.ListaSucursales(filtro);
            else
                lst = dao.ListaSucursalesBanco(idBanco);

            return lst;
        }    

        [HttpPost]
        public IHttpActionResult MantSucursal(string sAccion, [FromBody] mSucursal modelo)
        {
            bool bResultado = false;
            string sMensaje = string.Empty;

            rSucursal dao = new rSucursal();
            bResultado = dao.Mantenimiento(sAccion, modelo, ref sMensaje);

            return Ok(new { resultado = bResultado, mensaje = sMensaje });
        }
    }
}
