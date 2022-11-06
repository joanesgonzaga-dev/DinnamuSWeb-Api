using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinnamuSWebApi.Models
{
    public class ItensGradeProduto
    {
        public int ChaveUnica { get; set; }
        public long CodigoProduto { get; set; }
        public string CodiCorProd { get; set; }
        public string Cor { get; set; }
        public string Tamanho { get; set; }
        public string CodBarraInt { get; set; }
        public string CodBarraForn { get; set; }
        public decimal PrecoCompra { get; set; }
        public string Referencia { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal EstoqueInicial { get; set; }
        public string JaFeito { get; set; }
        public int Loja { get; set; }
        public string Etiqueta { get; set; }
        public int QtEtiqueta { get; set; }
        public int ChaveLojaOrigem { get; set; }
        public DateTime UltimaModificacao { get; set; }

        public List<EstoqueFilial> EstoqueNasFiliais { get; set; }

        public List<ItemTabelaDePreco> PrecosDeVenda { get; set; }

        public override bool Equals(object obj)
        {
            var produto = obj as ItensGradeProduto;
            return produto != null &&
                   ChaveUnica == produto.ChaveUnica;
        }

        public override int GetHashCode()
        {
            return -1623384538 + ChaveUnica.GetHashCode();
        }
    }
}
