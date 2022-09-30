using DinnamuSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Data.Vendas
{
    public class DadosOrc_ConsultaCotacaoDTO
    {
        public long Codigo { get; set; }
        public int CodigoCotacao { get; set; }

        public string Data { get; set; }
        public decimal Valor { get; set; }
        public string Hora { get; set; }
        public decimal Desconto { get; set; }
        public decimal PercDesc { get; set; }
        public decimal TotalBruto { get; set; }
        public decimal Dinheiro { get; set; }
        public decimal Troco { get; set; }
        public string CodVendedor { get; set; }
        public string NomeVendedor { get; set; }
        public string CodCliente { get; set; }
        public string NomeCliente { get; set; }

        public List<ItemOrc> itens { get; set; }
    }
}