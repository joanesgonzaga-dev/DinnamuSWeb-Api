using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DinnamuSWebApi.Models
{
    
    public class Cliente
    {
        private long idUnico;

        public long IdUnico
        { 
            //get => this.id; //body expressions notation unavailable on .Net framework 4.5
            get
            {
                return this.idUnico;
            }
            
            set
            {
                this.idUnico = value;
            }
        }

        [Required(ErrorMessage = "O campo {0} não pode ser vazio")]
        [StringLength(60)]
        public string Nome { get; set; }
        public string Apelido { get; set; }

        public long Codigo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string TipoCli { get; set; }

        [StringLength(60)]
        public string Endereco { get; set; }
        public string Numero { get; set; }
        [StringLength(60)]
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        [StringLength(8)]
        public string Cep { get; set; }
        public int CodigoCidade { get; set; }
        public int CodigoPais { get; set; }
        public int Loja { get; set; }
        
        [StringLength(2)]
        public string UF { get; set; }
    }
}