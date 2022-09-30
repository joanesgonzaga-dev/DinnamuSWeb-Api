using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Data.Clientes
{
    public class InsertClienteDTO
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [MaxLength(60)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [MaxLength(60)]
        public string Apelido { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Tipo de Cliente: F para Física e J para Jurídica")]
        [MinLength(1)]
        [MaxLength(1)]
        [StringLength(1)]
        public string TipoCli { get; set; }

        [StringLength(60)]
        public string Endereco { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Numero { get; set; }
        [StringLength(60)]
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        [StringLength(8)]
        public string Cep { get; set; }
        public int CodigoCidade { get; set; }
        public int CodigoPais { get; set; }
        [Required]
        public int Loja { get; set; }

        [StringLength(2)]
        public string UF { get; set; }
    }
}