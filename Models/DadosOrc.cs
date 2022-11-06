using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Models
{
    public class DadosOrc
    {
        public long Codigo { get; set; }
        public int CodigoCotacao { get; set; }
        public int CodigoOrcamento { get; set; }
        public int CodigoVenda { get; set; }
        public string CodMov { get; set; }
        public string TipoMov { get; set; }
        public string NomeMov { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public string Feito { get; set; }
        public string CodVendedor { get; set; }
        public string NomeVendedor { get; set; }
        public string CodOperador {get;set; }
        public string NomeOperador { get; set; }
        public string NomeCliente { get; set; }
        public DateTime Hora { get; set; }
        public string CodCliente { get; set; }
        public decimal Desconto { get; set; }
        public decimal PercDesc { get; set; }
        public decimal TotalBruto { get; set; }
        public string InNomeOrc { get; set; }
        public string Recebido { get; set; }
        public string CupomFiscal { get; set; }
        public string Entrega { get; set; }
        public int Loja { get; set; }
        public string InicioCupom { get; set; }
        public string Liberado { get; set; }
        public int CodCaixa { get; set; }
        public int ObjetoCaixa { get; set; }
        public string ControleCx { get; set; }
        public int Filial { get; set; }
        public bool VendaCondicional { get; set; }
        public string Obs { get; set; }
        public bool Gaveta { get; set; }
        public string COO { get; set; }
        public bool Caucao { get; set; }
        public int CodigoGerenteManteveReserva { get; set; }
        public int SetorEntregaResp { get; set; }
        public int SetorEntregaConf { get; set; }
        public int SetorEntregaEntregador { get; set; }
        public DateTime SetorEntregaDataHoraEntrega { get; set; }

        public int ImpressoCotacao { get; set; }
        public int ImpressoVenda { get; set; }
        public bool Faturada { get; set; }
        public bool MarcarFaturada { get; set; }
        public int CodigoDocFaturado { get; set; }
        public decimal TotalProdutos { get; set; }
        public decimal TotalNota { get; set; }
        public decimal ValorIcms { get; set; }
        public decimal BaseCalcValorIcms { get; set; }
        public decimal ValorIcmsSubst { get; set; }
        public decimal BaseCalcValorIcmsSubst { get; set; }
        public decimal Frete { get; set; }
        public decimal Seguro { get; set; }
        public decimal OutrasDesp { get; set; }
        public decimal ValorIpi { get; set; }
        public string NotaNome { get; set; }
        public string NotaEndereco { get; set; }
        public string NotaBairro { get; set; }
        public string NotaCidade { get; set; }
        public string NotaUf { get; set; }
        public string NotaCep { get; set; }
        public string NotaDadosAdicionais { get; set; }
        public DateTime NotaDataEntrega { get; set; }
        public DateTime NotaDataEmissao { get; set; }
        public int NotaNumero { get; set; }
        public string NotaCfop { get; set; }
        public decimal Fracao { get; set; }
        public string EntregaNome { get; set; }
        public string EntregaCpf { get; set; }
        public string EntregaRg { get; set; }
        public string EntregaEndereco { get; set; }
        public string EntregaBairro { get; set; }
        public string EntregaCep { get; set; }
        public string EntregaFone { get; set; }
        public string EntregaCidade { get; set; }
        public string EntregaUf { get; set; }
        public int ValorTotalNotaFiscal { get; set; }
        public decimal Dinheiro { get; set; }
        public decimal Troco { get; set; }
        public int PDV { get; set; }
        public int EstoqueOK { get; set; }
        public int CodigoProcessamento { get; set; }
        public float Acrescimo { get; set; }
        public float AcrescimoPercentual { get; set; }
        public bool NaoSinc { get; set; }
        public long MesclagemNota { get; set; }
        public string MesclagemSeq { get; set; }
        public string XmlRecebimento { get; set; }
        public decimal DescAtacado { get; set; }

        public List<ItemOrc> itens { get; set; }
    }
}

