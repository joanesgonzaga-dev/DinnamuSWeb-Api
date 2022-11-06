using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Models
{
    public class ItemOrc
    {
        public long IdUnico { get; set; }
        public long Codigo { get; set; } //codigo na DadosOrc
        public int CodProd { get; set; } //ChaveUnica ItensGradeProduto
        public string DescricaoProd { get; set; }
        public string CodMov { get; set; }
        public string NomeMov { get; set; }
        public string TipoMov { get; set; }
        public DateTime Data { get; set; }
     
        public decimal Quantidade { get; set; }
        public decimal Preco { get; set; }
        public decimal Total { get; set; }
        public string CodCor { get; set; }
        public string CodTam { get; set; }
        public string REF { get; set; }
        public decimal DescP { get; set; }
        public decimal DescV { get; set; }
        public string TipoDesc { get; set; }
        public decimal PrecoOriginal { get; set; }
        public string NomeCor { get; set; }
        public string Icms { get; set; }
        public string UN { get; set; }
        public int LojaId { get; set; }
        public int VendedorId { get; set; }
        public string NomeVendedor { get; set; }
        public int DocGerado { get; set; }
        public string st { get; set; }
        public decimal Aliquota { get; set; }
        public string CodAliquota { get; set; }
        public string DescRateio { get; set; }
        public string IMPRESSO { get; set; }
        public decimal Liquido { get; set; }
        public decimal Custo { get; set; }
        public string NomeImpresso { get; set; }
        public string Tabela { get; set; } //nome da tabela de preço usada para esse item na cotação/orçamento
        public string PermitidoDesconto { get; set; }
        public decimal CustoAV { get; set; }
        public decimal CustoAP { get; set; }
        public decimal LucroAV { get; set; }
        public decimal LucroAP { get; set; }
        public decimal PercDescAutorizado { get; set; }
        public decimal Comissao { get; set; }
        public bool CondicionalMarcadoExclusao { get; set; }
        public bool EstoqueOK { get; set; }
        public decimal ComissaoVendedor { get; set; }
        public DateTime DataComissao { get; set; }
        public string IdLote { get; set; }
        public decimal DescontoLiberado { get; set; }
        public int Seq { get; set; }
        public decimal PercIpi { get; set; }
        public decimal ValorIpi { get; set; }
        public string SituacaoTributariaDesc { get; set; }
        public decimal ValorIcms { get; set; }
        public decimal BaseCalcValorIcms { get; set; }
        public decimal BaseCalcValorIcmsSubst { get; set; }
        public decimal ValorIcmsSubst { get; set; }
        public int ValorTotalNotaFiscal { get; set; }
        public string DadosEncomenda { get; set; }
        public DateTime PrevisaoDeEntrega { get; set; }
        public DateTime DataConclusaoServico { get; set; }
        public DateTime DataEntrega { get; set; }
        public string ObsEntrega { get; set; }
        public string SituacaoEncomenda { get; set; }
        public string ObsConclusao { get; set; }
        public bool EncomendaEncerrada { get; set; }
        public DateTime EncomendaDataEncerrada { get; set; }
        public bool EmExecucaoEncomenda { get; set; }
        public DateTime DataInicioExecucao { get; set; }
        public bool Fracionado { get; set; }
        public string SituacaoDefConsig { get; set; }
        public float CodVendedor { get; set; }
        public int CodigoRecebimentoRevenda { get; set; }
        public int CodigoRevenda { get; set; }
        public int CodigoProcessamento { get; set; }
        public string NCM { get; set; }
        public string SequenciaOriginal { get; set; }
        public long CodigoOriginal { get; set; }
        public float QtConf { get; set; }
        public bool NaoSinc { get; set; }
        public decimal PrecoAtacadoItem { get; set; }
        public decimal DescAtacadoItem { get; set; }
        

    }
}