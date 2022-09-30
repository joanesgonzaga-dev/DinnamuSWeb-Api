using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Models
{
    public class Grade
    {
        public int Chaveunica { get; set; }
        public string Descricao { get; set; }
        List<ItensTabelaGrade> ItensTabelaGrade { get; set; }
    }
}