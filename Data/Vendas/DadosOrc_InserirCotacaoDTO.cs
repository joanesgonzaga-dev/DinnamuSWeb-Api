using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Data.Vendas
{
    public class DadosOrc_InserirCotacaoDTO
    {
        public string CodMov { get; set; }
        public string NomeMov { get; set; }
        public string TipoMov { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public DateTime Hora { get; set; }
        public decimal Desconto { get; set; }
        public decimal PercDesc { get; set; }
        public decimal TotalBruto { get; set; }
        public decimal Dinheiro { get; set; }
        public decimal Troco { get; set; }

    }
}