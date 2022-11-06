using DinnamuSWebApi.Models;
using DinnamuSWebApi.Repositories.Produtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DinnamuSWebApi.Controllers
{
    [RoutePrefix("api/produtos")]
    public class ProdutosController : ApiController
    {
        private IProdutoRepository _repository;

        public ProdutosController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IHttpActionResult Produtos()
        {
            return Json(_repository.Get());
        }

        [HttpGet]
        [Route("{codigo:int}")]
        public IHttpActionResult Produtos([FromUri] int codigo)
        {
            return Json(_repository.Get(codigo));
        }

        [HttpPost]
        public HttpResponseMessage Insert([FromBody] Produto produto)
        {
            _repository.Insert(produto);
            produto = _repository.Get(produto.Codigo);
            HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.Created, produto);
            return resp;
        }

        [HttpPut]
        public HttpResponseMessage Update([FromBody]Produto produto)
        {
            _repository.Update(produto);
            produto = _repository.Get(produto.Codigo);
            HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.Created, produto);
            return resp;
        }

        [HttpDelete]
        [Route("{codigo}")]
        public HttpResponseMessage Delete([FromUri]long codigo)
        {
            _repository.Delete(codigo);
            HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.NoContent);
            return resp;
        }
    }
}