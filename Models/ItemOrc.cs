using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Models
{
    public class ItemOrc
    {
        public long IdUnico { get; set; }
        public long Codigo { get; set; } //codigo na DadosOrc
        public int CodProd { get; set; } //ChaveUnica ItensGradeProduto
        public string DescricaoProd { get; set; }
        public string NomeMov { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Preco { get; set; }
        public decimal Total { get; set; }
        public string Tabela { get; set; } //nome da tabela de preço usada para esse item na cotação/orçamento
        public string CodTam { get; set; }
        public string REF { get; set; }
        public decimal DescP { get; set; }
        public decimal DescV { get; set; }
        public decimal PrecoOriginal { get; set; }
        public string UN { get; set; }
        public decimal Custo { get; set; }
        public int Seq { get; set; }
        public string NCM { get; set; }

    }
}