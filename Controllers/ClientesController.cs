using DinnamuSWebApi.Data.Clientes;
using DinnamuSWebApi.Models;
using DinnamuSWebApi.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DinnamuSWebApi.Controllers
{
    [RoutePrefix("api/clientes")]
    public class ClientesController : ApiController
    {
        private IClienteRepository _repository;

        public ClientesController(IClienteRepository repository)
        {
            _repository = repository;
        }
        // GET: api/clientes
        [HttpGet]
        public IHttpActionResult Clientes()
        {
          return Json(_repository.GetList());
        }

        // GET: api/clientes/5
        [HttpGet]
        [Route("{codigo:long}")]
        public IHttpActionResult Clientes([FromUri]long codigo)
        {
           return Json(_repository.GetById(codigo));    
        }

        [HttpGet]
        [Route("{nome}")]
        public IHttpActionResult Clientes(string nome)
        {
            return Json(_repository.GetByName(nome));
        }

        // POST: api/clientes
        [HttpPost]
        public HttpResponseMessage Inserir(InsertClienteDTO clienteDTO)
        {
            if(!ModelState.IsValid)
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

                Cliente cliente = new Cliente()
                {
                    Nome = clienteDTO.Nome,
                    Apelido = clienteDTO.Apelido,
                    TipoCli = clienteDTO.TipoCli,
                    DataNascimento = clienteDTO.DataNascimento,
                    CPF = clienteDTO.CPF,
                    RG = clienteDTO.RG,
                    Endereco = clienteDTO.Endereco,
                    Numero = clienteDTO.Numero,
                    Complemento = clienteDTO.Complemento,
                    Bairro = clienteDTO.Bairro,
                    Cidade = clienteDTO.Cidade,
                    Cep = clienteDTO.Cep,
                    CodigoCidade = clienteDTO.CodigoCidade,
                    CodigoPais = clienteDTO.CodigoPais,
                    UF = clienteDTO.UF,
                    Loja = clienteDTO.Loja,
                    
                };

                Cliente _cliente = _repository.InsertRetrieve(cliente);

                GetClienteDTO _clienteDTO = new GetClienteDTO()
                {
                    IdUnico = _cliente.IdUnico,
                    Codigo = _cliente.Codigo,
                    Nome = _cliente.Nome,
                    Apelido = _cliente.Apelido,
                    TipoCli = _cliente.TipoCli,
                    DataNascimento = _cliente.DataNascimento,
                    CPF = _cliente.CPF,
                    RG = _cliente.RG,
                    Endereco = _cliente.Endereco,
                    Numero = _cliente.Numero,
                    Complemento = _cliente.Complemento,
                    Bairro = _cliente.Bairro,
                    Cidade = _cliente.Cidade,
                    Cep = _cliente.Cep,
                    CodigoCidade = _cliente.CodigoCidade,
                    CodigoPais = _cliente.CodigoPais,
                    UF = _cliente.UF,
                    Loja = _cliente.Loja,

                };

                HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.Created, _clienteDTO);
                return resp;
            
        }

        // PUT: api/clientes/5
        [HttpPut]
        public HttpResponseMessage Update(UpdateClienteDTO clienteDto)
        {
            if(!ModelState.IsValid)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent("Não foi possível atualizar. Parâmetros ausêntes")
                    }
                );
            }

                Cliente cliente = new Cliente()
                {
                    IdUnico = clienteDto.IdUnico,
                    Codigo = clienteDto.Codigo,
                    Nome = clienteDto.Nome,
                    Apelido = clienteDto.Apelido,
                    DataNascimento = clienteDto.DataNascimento,
                    CPF = clienteDto.CPF,
                    RG = clienteDto.RG,
                    TipoCli = clienteDto.TipoCli,
                    Endereco = clienteDto.Endereco,
                    Numero = clienteDto.Numero,
                    Complemento = clienteDto.Complemento,
                    Bairro = clienteDto.Bairro,
                    Cidade = clienteDto.Cidade,
                    Cep = clienteDto.Cep,
                    CodigoCidade = clienteDto.CodigoCidade,
                    CodigoPais = clienteDto.CodigoPais,
                    Loja = clienteDto.Loja,
                    UF = clienteDto.UF
                };

                _repository.Update(cliente);

                HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.OK);
                return resp;
        }

        // DELETE: api/clientes/5
        [HttpDelete]
        [Route("{IdUnico}")]
        public HttpResponseMessage Excluir([FromUri]int IdUnico)
        {
            _repository.Delete(IdUnico);
            HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.OK);
            return resp;
        }
    }
}
