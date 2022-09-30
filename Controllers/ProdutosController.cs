using DinnamuSWebApi.Repositories.Produtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace DinnamuSWebApi.Controllers
{
    public class ProdutosController : ApiController
    {
        private IProdutoRepository _repository;

        public ProdutosController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/produtos")]
        public IHttpActionResult Produtos()
        {
            try
            {
                return Json(_repository.Get());
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            
        }
    }
}