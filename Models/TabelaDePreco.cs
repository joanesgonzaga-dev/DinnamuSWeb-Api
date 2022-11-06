using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Models
{
    public class TabelaDePreco
    {
        public int ChaveUnica { get; set; }
        public int Codigo { get; set; }
        public int Loja { get; set; }
        public string Descricao { get; set; }
        public int TabelaBase { get; set; }
        public decimal MargemDeLucro { get; set; }

    }
}