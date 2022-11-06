using DinnamuSWebApi.Data.Vendas;
using DinnamuSWebApi.Models;
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
        public HttpResponseMessage Inserir([FromBody]DadosOrc cotacao)
        {
            _repository.Inserir(cotacao);
                //cotacao = _repository.cotacoes(produto.Codigo);
            HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.Created);
            return resp;            
        }

        [HttpPost]
        [Route("{inseriritem}")]
        public HttpResponseMessage InserirItemNaCotacao([FromBody]DadosOrc_InserirItemDTO cotacao)
        {
            if (!ModelState.IsValid)
            {
                #region HttpResponseException
                //Not a really true exception, but a way to send a specific HttpResponse
                throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent("Não foi possível criar. Parâmetros ausêntes")
                    }
                );
                #endregion
            }

            //Objeto DadosOrc para necessidades futuras
            DadosOrc _cotacao = new DadosOrc()
            {
                Codigo = cotacao.Codigo,
            };

            _cotacao.itens = new List<ItemOrc>();
            _cotacao.itens.Add(cotacao.itens[0]);

            _repository.InserirItemNaCotacao(_cotacao.itens[0]);

            return null;
        }
    }
}
