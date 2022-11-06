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
        public long Chaveunica { get; set; }
        public long Codigo { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 3)]
        public string Nome { get; set; }
        public string NomeImpresso { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Unidade { get; set; }
        public string Referencia { get; set; }
        public string CodForn { get; set; }
        public string CodigoNbn { get; set; }
        [DisplayName("Data Cadastro")]
        public string DataCadastro { get; set; }
        public int Loja { get; set; }
        public bool Fracionado { get; set; }
        public bool Bloqueado { get; set; }
        public Byte[] Foto { get; set; }
        public int ICMS { get; set; }
        public string TributacaoICMS { get; set; }
        public string CodAliquota { get; set; }
        public float PercentualDeICMS { get; set; }
        public string ClassificacaoFiscal { get; set; }
        public string OrigemDoProduto { get; set; }
        public float PercentualIpi { get; set; }
        public int TributacaoIpi { get; set; }
        public decimal MargemDelucro { get; set; }
        public int Feito { get; set; }
        public int FabricanteId { get; set; }
        public string FiscalConvenio { get; set; }
        public string FiscalStFaturamento { get; set; }
        public decimal FiscalReducaoBaseCalcICMS { get; set; }
        public bool Ativado { get; set; }
        public int RegimeTributario { get; set; }
        public double AliquotaIcmsSt { get; set; }
        public double PercReducaoBcSt { get; set; }
        public int UfIcmsSt { get; set; }
        public int ModalidadeDeterminaBcSt { get; set; }
        public double PercMargemValorAdicSt { get; set; }
        public double vBcStRet { get; set; }
        public double vIcmsStRet { get; set; }
        public double IcmsSn101AliqAplic { get; set; }
        public double IcmsSn101VlrCred { get; set; }
        public string SituacaoTributariaIpi { get; set; }
        public string ClEnq { get; set; }
        public string CEnq { get; set; }
        public int PrecoDecimais { get; set; }
        public int PisCst { get; set; }
        public int CofinsCst { get; set; }
        public string Cest { get; set; }
        public int TabelaBaseAtacado { get; set; }
        public string CodGrade { get; set; }
        public List<ItensGradeProduto> ItensGrade { get; set; }
    }
}