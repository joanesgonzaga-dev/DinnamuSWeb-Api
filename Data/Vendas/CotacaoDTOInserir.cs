using DinnamuSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Data.Vendas
{
    public class CotacaoDTOInserir
    {
        public DadosOrc_InserirCotacaoDTO DadosOrc { get; set; }
        public VendedorParaInserirCotacaoDTO Vendedor { get; set; }

        public ClienteParaConsultaCotacaoDTO Cliente { get; set; }

        public List<ItemOrc> ItensOrc { get; set; }

    }
}