using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Data.Vendas
{
    public class ClienteParaConsultaCotacaoDTO
    {
        public int IdUnico { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }

        public int Codigo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public char TipoCli { get; set; }

        [StringLength(60)]
        public string Endereco { get; set; }
        public string Numero { get; set; }
        [StringLength(60)]
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        [StringLength(8)]
        public string Cep { get; set; }
        public int CodigoCidade { get; set; }
        public int CodigoPais { get; set; }


        [StringLength(2)]
        public string UF { get; set; }
    }
}