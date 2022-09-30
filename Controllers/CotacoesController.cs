using DinnamuSWebApi.Data.Vendas;
using DinnamuSWebApi.Repositories.Vendas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DinnamuSWebApi.Controllers
{
    [RoutePrefix("api/cotacoes")]
    public class CotacoesController : ApiController
    {
        private ICotacaoRepository _repository;

        public CotacoesController(ICotacaoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IHttpActionResult cotacoes()
        {
            try
            {
                return Json(_repository.cotacoes());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("{codigo:long}")]
        public IHttpActionResult cotacoes([FromUri]long codigo)
        {
            try
            {
                return Json(_repository.cotacaoByCodigo(codigo));
            }

            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpPost]
        public IHttpActionResult Inserir([FromBody]CotacaoDTOInserir cotacao)
        {
            try
            {
                return Json("ok");
            }

            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
