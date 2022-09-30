using DinnamuSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Data.Vendas
{
    public class CotacaoDTOConsulta
    {
        public DadosOrc_ConsultaCotacaoDTO DadosOrc { get; set; }
    
        public List<ItemOrc> ItensOrc { get; set; }

    }
}