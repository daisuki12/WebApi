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
    public class BancoController : ApiController
    {
        [HttpGet]
        public List<mBanco> ObtenerBancos(string filtro = "")
        {
            rBanco dao = new rBanco();
            List<mBanco> lst = dao.ListaBancos(filtro);
            return lst;
        }

        [HttpPost]
        public IHttpActionResult MantBanco(string sAccion, [FromBody] mBanco modelo)
        {
            bool bResultado = false;
            string sMensaje = string.Empty;

            rBanco dao = new rBanco();
            bResultado = dao.Mantenimiento(sAccion, modelo, ref sMensaje);

            return Ok(new { resultado = bResultado, mensaje = sMensaje });
        }
    }
}
