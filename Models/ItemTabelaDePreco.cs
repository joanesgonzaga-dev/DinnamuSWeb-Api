using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Models
{
    public class ItemTabelaDePreco
    {
        public int ChaveUnica { get; set; }
        public int CodigoTabela { get; set; }
        public int CodigoProduto { get; set; }
        public string NomeTabela { get; set; }
        public decimal Preco { get; set; }

        public override bool Equals(object obj)
        {
            var preco = obj as ItemTabelaDePreco;
            return preco != null &&
                   ChaveUnica == preco.ChaveUnica;
        }

        public override int GetHashCode()
        {
            return -1623384538 + ChaveUnica.GetHashCode();
        }
    }
}