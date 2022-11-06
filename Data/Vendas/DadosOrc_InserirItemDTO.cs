using DinnamuSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Data.Vendas
{
    public class DadosOrc_InserirItemDTO
    {
        public long Codigo { get; set; }
        public List<ItemOrc> itens { get; set; }
    }
}