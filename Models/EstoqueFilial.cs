using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Models
{
    public class EstoqueFilial
    {
        public long Id { get; set; }
        public int CodigoLoja { get; set; }
        public int CodigoFilial { get; set; }
        public int CodigoProduto { get; set; }
        public decimal Estoque { get; set; }
        public DateTime UltimaModificacao { get; set; }

    }
}