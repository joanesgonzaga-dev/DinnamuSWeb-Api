using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Models
{
    public class Produto
    {
        public int Chaveunica { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 3)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Unidade { get; set; }
        public string Referencia { get; set; }
        public string CodForn { get; set; }
        public string CodigoNBN { get; set; }
        [DisplayName("Data Cadastro")]
        public string DataCadastro { get; set; }

        public List<ItensGradeProduto> ItensGrade { get; set; }
    }
}