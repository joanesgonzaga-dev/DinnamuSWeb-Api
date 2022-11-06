using DinnamuS_API.Repositories.Utils;
using DinnamuSWebApi.Data.Vendas;
using DinnamuSWebApi.Models;
using DinnamuSWebApi.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Repositories.Vendas
{
    public class CotacaoRepository : ICotacaoRepository
    {
        IDbConnection _connection;
        private string _con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public CotacaoRepository()
        {
            //_connection = new SqlConnection(_con);
        }
        public DadosOrc cotacaoByCodigo(long codigo)
        {
            using (_connection = new SqlConnection(_con))
            {
                _connection.Open();
                SqlCommand cmd = (SqlCommand)_connection.CreateCommand();
                cmd.Connection = (SqlConnection)_connection;

            #region monta a string de consulta: dadosOrc inner join itensorc
                cmd.CommandText = @"SELECT"
            + " do.codigo, do.codigocotacao, do.codigoorcamento, do.codigovenda, do.codmov, do.nomemov"
            + ",do.tipomov,do.data, do.valor, do.feito, do.codvendedor, do.vendedor "
            + ",do.codoperador, do.operador,do.cliente,do.hora,do.codCliente,do.desconto "
            + ",do.percdesc, do.totalbruto,do.innomeorc, do.recebido, do.cupomfiscal,do.ENTREGA "
            + ",do.loja, do.INICIOCUPOM, do.liberado, do.codcaixa, do.objetocaixa, do.controlecx "
            + ",do.filial,do.obs,do.gaveta, do.COO,do.CAUCAO, do.CodigoGerenteManteveReserva "
            + ",do.SetorEntrega_Resp, do.SetorEntrega_Conf,do.SetorEntrega_Entregador, do.SetorEntrega_DataHoraEntrega,do.ImpressoCotacao, do.ImpressoVenda"
            + ",do.faturada, do.MarcarFaturada,do.CodigoDocFaturado, do.TotalProdutos,do.TotalNota,do.ValorIcms"
            + ",do.BaseCalcValorIcms,do.ValorIcmsSubst ,do.BaseCalcValorIcmsSubst, do.frete,do.seguro,do.outrasdesp "
            + ",do.valoripi,do.nota_nome,do.nota_endereco ,do.nota_bairro,do.nota_cidade,do.nota_uf"
            + ",do.nota_cep,do.nota_dadosadicionais,do.nota_dataentrega, do.nota_dataemissao,do.nota_numero,do.nota_CFOP "
            + ",do.FRACAO,do.TotaNotal,do.entrega_nome, do.entrega_cpf,do.entrega_rg,do.entrega_endereco "
            + ",do.entrega_bairro,do.entrega_cep, do.entrega_fone, do.entrega_cidade,do.entrega_uf, do.valortotalnotafiscal "
            + ",do.dinheiro, do.troco, do.pdv, do.EstoqueOK, do.codigoprocessamento, do.acrescimo "
            + ",do.acrescimopercentual, do.naosinc, do.mesclagem_nota, do.mesclagem_seq, do.xml_recebimento, do.descatacado "
            + " "

            + ",iorc.idunico, iorc.codigo, iorc.codprod, iorc.descricao, iorc.codmov, iorc.nomemov "
            + ",iorc.tipomov, iorc.data, iorc.quantidade, iorc.preco, iorc.total, iorc.codcor "
            + ",iorc.codtam, iorc.ref, iorc.descp, iorc.descv, iorc.tipodesc, iorc.precooriginal "
            + ",iorc.nomecor, iorc.icms, iorc.unidade, iorc.loja, iorc.vendedor, iorc.nomevendedor "
            + ",iorc.docgerado, iorc.st, iorc.aliquota, iorc.codaliquota, iorc.descrateio, iorc.IMPRESSO "
            + ",iorc.liquido, iorc.custo, iorc.nome_impresso, iorc.tabela, iorc.permitidodesconto, iorc.custo_av "
            + ",iorc.custo_ap, iorc.lucro_av, iorc.lucro_ap, iorc.perc_desc_autorizado, iorc.comissao, iorc.Condicional_Marcado_Exclusao "
            + ",iorc.estoqueOK, iorc.ComissaoVendedor, iorc.DataComissao, iorc.idLote, iorc.DescontoLiberado, iorc.SEQ "
            + ",iorc.percIpi, iorc.ValorIpi, iorc.SituacaoTributariaDesc, iorc.ValorIcms, iorc.BaseCalcValorIcms, iorc.ValorIcmsSubst "
            + ",iorc.BaseCalcValorIcmsSubst, iorc.valortotalnotafiscal, iorc.dadosencomenda, iorc.previsaoentrega, iorc.dataconclusaoservico, iorc.dataentrega "
            + ",iorc.obsentrega, iorc.situacaoencomenda, iorc.obsconclusao, iorc.encomenda_encerrada, iorc.encomenda_dataencerrada, iorc.em_execucao_encomenda "
            + ",iorc.data_inicio_execucao, iorc.FRACIONADO, iorc.situacao_def_consig, iorc.codigo_recebimento_revenda, iorc.codigorevenda "
            + ",iorc.codigoprocessamento, iorc.ncm, iorc.codvendedor, iorc.sequenciaoriginal, iorc.codigooriginal, iorc.qtconf "
            + ",iorc.naosinc, iorc.precoatacadoitem, iorc.descatacadoitem "
            + " FROM dadosorc do "
            + " inner join itensorc iorc "
            + " on do.codigo = iorc.codigo "

            + "WHERE do.codigo=@codigo "

            + " AND do.recebido='N' AND do.feito='S' ";
            #endregion

                cmd.Parameters.AddWithValue("@codigo", codigo);
               
                SqlDataReader reader = cmd.ExecuteReader();

                #region Iterage no retorno da consulta e monta a lista de Cotacoes
                Dictionary<long, DadosOrc> cotacoes = new Dictionary<long, DadosOrc>();
                while (reader.Read())
                {
                    DadosOrc cotacao = new DadosOrc();

                    if (!cotacoes.ContainsKey(reader.GetInt64(0)))
                    {
                        //do.codigo, do.codigocotacao, do.codigoorcamento, do.codigovenda, do.codmov, do.nomemov
                        cotacao.Codigo = (long)reader.GetInt64(0);
                        cotacao.CodigoCotacao = (int)((DBNull.Value == reader[1]) ? default(int) : reader.GetInt32(1));
                        cotacao.CodigoOrcamento = (int)((DBNull.Value == reader[2]) ? default(int) : reader.GetInt32(2));
                        cotacao.CodigoVenda = (int)((DBNull.Value == reader[3]) ? default(int) : reader.GetInt32(3));
                        cotacao.CodMov = ((DBNull.Value == reader[4]) ? string.Empty : reader.GetString(4)).ToString();
                        cotacao.NomeMov = ((DBNull.Value == reader[5]) ? string.Empty : reader.GetString(5)).ToString();

                        //do.tipomov,do.data, do.valor, do.feito, do.codvendedor, do.vendedor
                        cotacao.TipoMov = ((DBNull.Value == reader[6]) ? string.Empty : reader.GetString(6)).ToString();
                        cotacao.Data = ((DBNull.Value == reader[7]) ? new DateTime(1901, 01, 01) : reader.GetDateTime(7));
                        cotacao.Valor = (decimal)((DBNull.Value == reader[8]) ? default(decimal) : reader.GetDecimal(8));
                        cotacao.Feito = ((DBNull.Value == reader[9]) ? string.Empty : reader.GetString(9)).ToString();
                        cotacao.CodVendedor = (string)((DBNull.Value == reader[10]) ? string.Empty : reader.GetString(10));
                        cotacao.NomeVendedor = (string)((DBNull.Value == reader[11]) ? string.Empty : reader.GetString(11));

                        //do.codoperador, do.operador,do.cliente,do.hora,do.codCliente,do.desconto
                        cotacao.CodOperador = (string)((DBNull.Value == reader[12]) ? string.Empty : reader.GetString(12));
                        cotacao.NomeOperador = (string)((DBNull.Value == reader[13]) ? string.Empty : reader.GetString(13));
                        cotacao.NomeCliente = (string)((DBNull.Value == reader[14]) ? string.Empty : reader.GetString(14));
                        cotacao.Hora = (DateTime)((DBNull.Value == reader[15]) ? new DateTime() : reader.GetDateTime(15));
                        cotacao.CodCliente = (string)((DBNull.Value == reader[16]) ? string.Empty : reader.GetString(16));
                        cotacao.Desconto = (decimal)((DBNull.Value == reader[17]) ? default(decimal) : reader.GetDecimal(17));

                        //do.percdesc, do.totalbruto,do.innomeorc, do.recebido, do.cupomfiscal,do.ENTREGA
                        cotacao.PercDesc = (decimal)((DBNull.Value == reader[18]) ? default(decimal) : reader.GetDecimal(18));
                        cotacao.TotalBruto = (decimal)((DBNull.Value == reader[19]) ? default(decimal) : reader.GetDecimal(19));
                        cotacao.InNomeOrc = (string)((DBNull.Value == reader[20]) ? string.Empty : reader.GetString(20));
                        cotacao.Recebido = (string)((DBNull.Value == reader[21]) ? string.Empty : reader.GetString(21));
                        cotacao.CupomFiscal = (string)((DBNull.Value == reader[22]) ? string.Empty : reader.GetString(22));
                        cotacao.Entrega = (string)((DBNull.Value == reader[23]) ? string.Empty : reader.GetString(23));

                        cotacao.Loja = (int)((DBNull.Value == reader[24]) ? default(int) : reader.GetInt32(24));
                        cotacao.InicioCupom = (string)((DBNull.Value == reader[25]) ? string.Empty : reader.GetString(25));
                        cotacao.Liberado = (string)((DBNull.Value == reader[26]) ? string.Empty : reader.GetString(26));
                        cotacao.CodCaixa = (int)((DBNull.Value == reader[27]) ? default(int) : reader.GetInt32(27));
                        cotacao.ObjetoCaixa = (int)((DBNull.Value == reader[28]) ? default(int) : reader.GetInt32(28));
                        cotacao.ControleCx = (string)((DBNull.Value == reader[29]) ? string.Empty : reader.GetString(29));

                        cotacao.Filial = (int)((DBNull.Value == reader[30]) ? default(int) : reader.GetInt32(30));
                        cotacao.Obs = (string)((DBNull.Value == reader[31]) ? string.Empty : reader.GetString(31));
                        cotacao.Gaveta = (bool)((DBNull.Value == reader[32]) ? false : reader.GetBoolean(32));
                        cotacao.COO = (string)((DBNull.Value == reader[33]) ? string.Empty : reader.GetString(33));
                        cotacao.Caucao = (bool)((DBNull.Value == reader[34]) ? false : reader.GetBoolean(34));
                        cotacao.CodigoGerenteManteveReserva = (int)((DBNull.Value == reader[35]) ? 0 : reader.GetInt32(35));

                        cotacao.SetorEntregaResp = (int)((DBNull.Value == reader[36]) ? default(int) : reader.GetInt32(36));
                        cotacao.SetorEntregaConf = (int)((DBNull.Value == reader[37]) ? default(int) : reader.GetInt32(37));
                        cotacao.SetorEntregaEntregador = (int)((DBNull.Value == reader[38]) ? default(int) : reader.GetInt32(38));
                        cotacao.SetorEntregaDataHoraEntrega = (DateTime)((DBNull.Value == reader[39]) ? new DateTime(1901, 01, 01) : reader.GetDateTime(39));
                        cotacao.ImpressoCotacao = (int)((DBNull.Value == reader[40]) ? default(int) : reader.GetInt32(40));
                        cotacao.ImpressoVenda = (int)((DBNull.Value == reader[41]) ? default(int) : reader.GetInt32(41));

                        cotacao.Faturada = (bool)((DBNull.Value == reader[42]) ? false : reader.GetBoolean(42));
                        cotacao.MarcarFaturada = (bool)((DBNull.Value == reader[43]) ? false : reader.GetBoolean(43));
                        cotacao.CodigoDocFaturado = (int)((DBNull.Value == reader[44]) ? default(int) : reader.GetInt32(44));
                        cotacao.TotalProdutos = (decimal)((DBNull.Value == reader[45]) ? default(decimal) : reader.GetDecimal(45));
                        cotacao.TotalNota = (decimal)((DBNull.Value == reader[46]) ? default(decimal) : reader.GetDecimal(46));
                        cotacao.ValorIcms = (decimal)((DBNull.Value == reader[47]) ? default(decimal) : reader.GetDecimal(47));

                        cotacao.BaseCalcValorIcms = (decimal)((DBNull.Value == reader[48]) ? default(decimal) : reader.GetDecimal(48));
                        cotacao.ValorIcmsSubst = (decimal)((DBNull.Value == reader[49]) ? default(decimal) : reader.GetDecimal(49));
                        cotacao.BaseCalcValorIcmsSubst = (decimal)((DBNull.Value == reader[50]) ? default(decimal) : reader.GetDecimal(50));
                        cotacao.Frete = (decimal)((DBNull.Value == reader[51]) ? default(decimal) : reader.GetDecimal(51));
                        cotacao.Seguro = (decimal)((DBNull.Value == reader[52]) ? default(decimal) : reader.GetDecimal(52));
                        cotacao.OutrasDesp = (decimal)((DBNull.Value == reader[53]) ? default(decimal) : reader.GetDecimal(53));

                        cotacao.ValorIpi = (decimal)((DBNull.Value == reader[54]) ? default(decimal) : reader.GetDecimal(54));
                        cotacao.NotaNome = (string)((DBNull.Value == reader[55]) ? string.Empty : reader.GetString(55));
                        cotacao.NotaEndereco = (string)((DBNull.Value == reader[56]) ? string.Empty : reader.GetString(56));
                        cotacao.NotaBairro = (string)((DBNull.Value == reader[57]) ? string.Empty : reader.GetString(57));
                        cotacao.NotaCidade = (string)((DBNull.Value == reader[58]) ? string.Empty : reader.GetString(58));
                        cotacao.NotaUf = (string)((DBNull.Value == reader[59]) ? string.Empty : reader.GetString(59));

                        cotacao.NotaCep = (string)((DBNull.Value == reader[60]) ? string.Empty : reader.GetString(60));
                        cotacao.NotaDadosAdicionais = (string)((DBNull.Value == reader[61]) ? string.Empty : reader.GetString(61));
                        cotacao.NotaDataEntrega = ((DBNull.Value == reader[62]) ? new DateTime(1901, 01, 01) : reader.GetDateTime(62));
                        cotacao.NotaDataEmissao = ((DBNull.Value == reader[63]) ? new DateTime(1901, 01, 01) : reader.GetDateTime(63));
                        cotacao.NotaNumero = (int)((DBNull.Value == reader[64]) ? default(int) : reader.GetInt32(64));
                        cotacao.NotaCfop = (string)((DBNull.Value == reader[65]) ? string.Empty : reader.GetString(65));

                        cotacao.Fracao = (decimal)((DBNull.Value == reader[66]) ? default(decimal) : reader.GetDecimal(66));
                        cotacao.TotalNota = (decimal)((DBNull.Value == reader[67]) ? default(decimal) : reader.GetDecimal(67));
                        cotacao.EntregaNome = (string)((DBNull.Value == reader[68]) ? string.Empty : reader.GetString(68));
                        cotacao.EntregaCpf = (string)((DBNull.Value == reader[69]) ? string.Empty : reader.GetString(69));
                        cotacao.EntregaRg = (string)((DBNull.Value == reader[70]) ? string.Empty : reader.GetString(70));
                        cotacao.EntregaEndereco = (string)((DBNull.Value == reader[71]) ? string.Empty : reader.GetString(71));

                        cotacao.EntregaBairro = (string)((DBNull.Value == reader[72]) ? string.Empty : reader.GetString(72));
                        cotacao.EntregaCep = (string)((DBNull.Value == reader[73]) ? string.Empty : reader.GetString(73));
                        cotacao.EntregaFone = (string)((DBNull.Value == reader[74]) ? string.Empty : reader.GetString(74));
                        cotacao.EntregaCidade = (string)((DBNull.Value == reader[75]) ? string.Empty : reader.GetString(75));
                        cotacao.EntregaUf = (string)((DBNull.Value == reader[76]) ? string.Empty : reader.GetString(76));
                        cotacao.ValorTotalNotaFiscal = (int)((DBNull.Value == reader[77]) ? 0 : reader.GetInt32(77));

                        cotacao.Dinheiro = (decimal)((DBNull.Value == reader[78]) ? default(decimal) : reader.GetDecimal(78));
                        cotacao.Troco = (decimal)((DBNull.Value == reader[79]) ? default(decimal) : reader.GetDecimal(79));
                        cotacao.PDV = (int)((DBNull.Value == reader[80]) ? 0 : reader.GetInt32(80));
                        cotacao.EstoqueOK = (int)((DBNull.Value == reader[81]) ? 0 : reader.GetInt32(81));
                        cotacao.CodigoProcessamento = (int)((DBNull.Value == reader[82]) ? default(int) : reader.GetInt32(82));

                        //cotacao.Acrescimo = (float)(Convert.IsDBNull(reader[83]) ? default(float) : reader.GetFloat(83));
                        //cotacao.AcrescimoPercentual = (float)((DBNull.Value == reader[84]) ? default(float) : reader.GetFloat(84));
                        cotacao.NaoSinc = (bool)((DBNull.Value == reader[85]) ? false : reader.GetBoolean(85));
                        cotacao.MesclagemNota = (long)((DBNull.Value == reader[86]) ? default(long) : reader.GetInt64(86));
                        cotacao.MesclagemSeq = (string)((DBNull.Value == reader[87]) ? string.Empty : reader.GetString(87));
                        cotacao.XmlRecebimento = (string)((DBNull.Value == reader[88]) ? string.Empty : reader.GetString(88));
                        cotacao.DescAtacado = (decimal)((DBNull.Value == reader[89]) ? default(decimal) : reader.GetDecimal(89));

                        cotacoes.Add(cotacao.Codigo, cotacao);
                    }

                    else
                    {
                        cotacao = cotacoes[reader.GetInt64(0)];
                    }

                    #region Monta a lista de itens da cotação DadosOrc.ItensOrc
                    ItemOrc item = new ItemOrc()
                    {
                        //iorc.idunico, iorc.codigo, iorc.codprod, iorc.descricao, iorc.codmov, iorc.nomemov
                        IdUnico = (long)((DBNull.Value == reader[90]) ? default(long) : reader.GetInt64(90)),
                        Codigo = (long)((DBNull.Value == reader[91]) ? default(long) : reader.GetInt64(91)),
                        CodProd = (int)((DBNull.Value == reader[92]) ? default(int) : reader.GetInt32(92)),
                        DescricaoProd = (string)((DBNull.Value == reader[93]) ? string.Empty : reader.GetString(93)),
                        CodMov = (string)((DBNull.Value == reader[94]) ? string.Empty : reader.GetString(94)),
                        NomeMov = (string)((DBNull.Value == reader[95]) ? string.Empty : reader.GetString(95)),

                        //"iorc.tipomov, iorc.data, iorc.quantidade, iorc.preco, iorc.total, iorc.codcor "
                        TipoMov = (string)((DBNull.Value == reader[96]) ? string.Empty : reader.GetString(96)),
                        Data = (DateTime)((DBNull.Value == reader[97]) ? new DateTime(1900, 01, 01) : reader.GetDateTime(97)),
                        Quantidade = (decimal)((DBNull.Value == reader[98]) ? default(decimal) : reader.GetDecimal(98)),
                        Preco = (decimal)((DBNull.Value == reader[99]) ? default(decimal) : reader.GetDecimal(99)),
                        Total = (decimal)((DBNull.Value == reader[100]) ? default(decimal) : reader.GetDecimal(100)),
                        CodCor = (string)((DBNull.Value == reader[101]) ? string.Empty : reader.GetString(101)),

                        //iorc.codtam,iorc.ref, iorc.descp, iorc.descv, iorc.tipodesc, iorc.precooriginal
                        CodTam = (string)((DBNull.Value == reader[102]) ? string.Empty : reader.GetString(102)),
                        REF = (string)((DBNull.Value == reader[103]) ? string.Empty : reader.GetString(103)),
                        DescP = (decimal)((DBNull.Value == reader[104]) ? default(decimal) : reader.GetDecimal(104)),
                        DescV = (decimal)((DBNull.Value == reader[105]) ? default(decimal) : reader.GetDecimal(105)),
                        TipoDesc = (string)((DBNull.Value == reader[106]) ? string.Empty : reader.GetString(106)),
                        PrecoOriginal = (decimal)((DBNull.Value == reader[107]) ? default(decimal) : reader.GetDecimal(107)),

                        //iorc.nomecor,iorc.icms,iorc.unidade,iorc.loja,iorc.vendedor,iorc.nomevendedor
                        NomeCor = (string)((DBNull.Value == reader[108]) ? string.Empty : reader.GetString(108)),
                        Icms = (string)((DBNull.Value == reader[109]) ? string.Empty : reader.GetString(109)),
                        UN = (string)((DBNull.Value == reader[110]) ? string.Empty : reader.GetString(110)),
                        LojaId = (int)((DBNull.Value == reader[111]) ? default(int) : reader.GetInt32(111)),
                        VendedorId = (int)((DBNull.Value == reader[112]) ? default(int) : reader.GetInt32(112)),
                        NomeVendedor = (string)((DBNull.Value == reader[113]) ? string.Empty : reader.GetString(113)),

                        //iorc.docgerado, iorc.st, iorc.aliquota, iorc.codaliquota, iorc.descrateio, iorc.IMPRESSO
                        DocGerado = (int)((DBNull.Value == reader[114]) ? default(int) : reader.GetInt32(114)),
                        st = (string)((DBNull.Value == reader[115]) ? "" : reader.GetString(115)),
                        Aliquota = (decimal)((DBNull.Value == reader[116]) ? default(decimal) : reader.GetDecimal(116)),
                        CodAliquota = (string)((DBNull.Value == reader[117]) ? "" : reader.GetString(117)),
                        DescRateio = (string)((DBNull.Value == reader[118]) ? "" : reader.GetString(118)),
                        IMPRESSO = (string)((DBNull.Value == reader[119]) ? "" : reader.GetString(119)),

                        //iorc.liquido, iorc.custo, iorc.nome_impresso, iorc.tabela, iorc.permitidodesconto, iorc.custo_av
                        Liquido = (decimal)((DBNull.Value == reader[120]) ? default(decimal) : reader.GetDecimal(120)),
                        Custo = (decimal)((DBNull.Value == reader[121]) ? default(decimal) : reader.GetDecimal(121)),
                        NomeImpresso = (string)((DBNull.Value == reader[122]) ? string.Empty : reader.GetString(122)),
                        Tabela = (string)((DBNull.Value == reader[123]) ? string.Empty : reader.GetString(123)),
                        PermitidoDesconto = (string)((DBNull.Value == reader[124]) ? "" : reader.GetString(124)),
                        CustoAV = (decimal)((DBNull.Value == reader[125]) ? default(decimal) : reader.GetDecimal(125)),

                        //iorc.custo_ap, iorc.lucro_av, iorc.lucro_ap, iorc.perc_desc_autorizado, iorc.comissao, iorc.Condicional_Marcado_Exclusao
                        CustoAP = (decimal)((DBNull.Value == reader[126]) ? default(decimal) : reader.GetDecimal(126)),
                        LucroAV = (decimal)((DBNull.Value == reader[127]) ? default(decimal) : reader.GetDecimal(127)),
                        LucroAP = (decimal)((DBNull.Value == reader[128]) ? default(decimal) : reader.GetDecimal(128)),
                        PercDescAutorizado = (decimal)((DBNull.Value == reader[129]) ? default(decimal) : reader.GetDecimal(129)),
                        Comissao = (decimal)((DBNull.Value == reader[130]) ? default(decimal) : reader.GetDecimal(130)),
                        CondicionalMarcadoExclusao = (bool)((DBNull.Value == reader[131]) ? false : reader.GetBoolean(131)),

                        //iorc.estoqueOK, iorc.ComissaoVendedor, iorc.DataComissao, iorc.idLote, iorc.DescontoLiberado, iorc.SEQ
                        EstoqueOK = (bool)((DBNull.Value == reader[132]) ? false : reader.GetBoolean(132)),
                        ComissaoVendedor = (decimal)((DBNull.Value == reader[133]) ? default(decimal) : reader.GetDecimal(133)),
                        DataComissao = (DateTime)((DBNull.Value == reader[134]) ? new DateTime(1900, 01, 01) : reader.GetDateTime(134)),
                        IdLote = (string)((DBNull.Value == reader[135]) ? string.Empty : reader.GetString(135)),
                        DescontoLiberado = (decimal)((DBNull.Value == reader[136]) ? default(decimal) : reader.GetDecimal(136)),
                        Seq = (int)((DBNull.Value == reader[137]) ? default(int) : reader.GetInt32(137)),

                        //iorc.percIpi, iorc.ValorIpi, iorc.SituacaoTributariaDesc, iorc.ValorIcms, iorc.BaseCalcValorIcms, iorc.ValorIcmsSubst
                        PercIpi = (decimal)((DBNull.Value == reader[138]) ? default(decimal) : reader.GetDecimal(138)),
                        ValorIpi = (decimal)((DBNull.Value == reader[139]) ? default(decimal) : reader.GetDecimal(139)),
                        SituacaoTributariaDesc = (string)((DBNull.Value == reader[140]) ? string.Empty : reader.GetString(140)),
                        ValorIcms = (decimal)((DBNull.Value == reader[141]) ? default(decimal) : reader.GetDecimal(141)),
                        BaseCalcValorIcms = (decimal)((DBNull.Value == reader[142]) ? default(decimal) : reader.GetDecimal(142)),
                        ValorIcmsSubst = (decimal)((DBNull.Value == reader[143]) ? default(decimal) : reader.GetDecimal(143)),

                        //iorc.BaseCalcValorIcmsSubst, iorc.valortotalnotafiscal, iorc.dadosencomenda, iorc.previsaoentrega, iorc.dataconclusaoservico, iorc.dataentrega
                        BaseCalcValorIcmsSubst = (decimal)((DBNull.Value == reader[144]) ? default(decimal) : reader.GetDecimal(144)),
                        ValorTotalNotaFiscal = (int)((DBNull.Value == reader[145]) ? default(int) : reader.GetInt32(145)),
                        DadosEncomenda = (string)((DBNull.Value == reader[146]) ? string.Empty : reader.GetString(146)),
                        PrevisaoDeEntrega = (DateTime)((Convert.IsDBNull(reader[147])) ? new DateTime(1900, 01, 01) : reader.GetDateTime(147)),
                        DataConclusaoServico = (DateTime)((Convert.IsDBNull(reader[148])) ? new DateTime(1900, 01, 01) : reader.GetDateTime(148)),
                        DataEntrega = (DateTime)((Convert.IsDBNull(reader[149])) ? new DateTime(1900, 01, 01) : reader.GetDateTime(149)),

                        //iorc.obsentrega, iorc.situacaoencomenda, iorc.obsconclusao, iorc.encomenda_encerrada, iorc.encomenda_dataencerrada, iorc.em_execucao_encomenda
                        ObsEntrega = (string)((DBNull.Value == reader[150]) ? string.Empty : reader.GetString(150)),
                        SituacaoEncomenda = (string)((DBNull.Value == reader[151]) ? string.Empty : reader.GetString(151)),
                        ObsConclusao = (string)((DBNull.Value == reader[152]) ? string.Empty : reader.GetString(152)),
                        EncomendaEncerrada = (bool)((DBNull.Value == reader[153]) ? false : reader.GetBoolean(153)),
                        EncomendaDataEncerrada = (DateTime)((Convert.IsDBNull(reader[154])) ? new DateTime(1900, 01, 01) : reader.GetDateTime(154)),
                        EmExecucaoEncomenda = (bool)((DBNull.Value == reader[155]) ? false : reader.GetBoolean(155)),

                        ////iorc.data_inicio_execucao, iorc.FRACIONADO, iorc.situacao_def_consig, iorc.codigo_recebimento_revenda, iorc.codigorevenda
                        DataInicioExecucao = (DateTime)((Convert.IsDBNull(reader[156])) ? new DateTime(1900, 01, 01) : reader.GetDateTime(156)),
                        Fracionado = (bool)((DBNull.Value == reader[157]) ? false : reader.GetBoolean(157)),
                        SituacaoDefConsig = (string)((DBNull.Value == reader[158]) ? string.Empty : reader.GetString(158)),
                        CodigoRecebimentoRevenda = (int)((DBNull.Value == reader[159]) ? default(int) : reader.GetInt32(159)),
                        CodigoRevenda = (int)((DBNull.Value == reader[160]) ? default(int) : reader.GetInt32(160)),

                        ////iorc.codigoprocessamento, iorc.ncm, iorc.codvendedor, iorc.sequenciaoriginal, iorc.codigooriginal, iorc.qtconf
                        CodigoProcessamento = (int)((DBNull.Value == reader[161]) ? default(int) : reader.GetInt32(161)),
                        NCM = (string)((DBNull.Value == reader[162]) ? string.Empty : reader.GetString(162)),
                        //CodVendedor = default(float),
                        SequenciaOriginal = (string)((DBNull.Value == reader[164]) ? string.Empty : reader.GetString(164)),
                        CodigoOriginal = (long)((DBNull.Value == reader[165]) ? default(long) : reader.GetInt64(165)),
                        //QtConf = (float)((DBNull.Value == reader[166]) ? default(float) : reader.GetFloat(166)),

                        ////iorc.naosinc, iorc.precoatacadoitem, iorc.descatacadoitem
                        NaoSinc = (bool)((DBNull.Value == reader[167]) ? false : reader.GetBoolean(167)),
                        PrecoAtacadoItem = (decimal)((DBNull.Value == reader[168]) ? default(decimal) : reader.GetDecimal(168)),
                        DescAtacadoItem = (decimal)((DBNull.Value == reader[169]) ? default(decimal) : reader.GetDecimal(169)),
                    };

                    cotacao.itens = (cotacao.itens == null) ? new List<ItemOrc>() : cotacao.itens;
                    cotacao.itens.Add(item);
                    #endregion
                }

                return cotacoes.Values.ToList().FirstOrDefault();
                #endregion
            }
        }
        public List<DadosOrc> cotacoes()
        {
            using (_connection = new SqlConnection(_con))
            {
                SqlCommand cmd = (SqlCommand)_connection.CreateCommand();
                cmd.Connection = (SqlConnection)_connection;
                _connection.Open();

                #region monta a string de consulta: dadosOrc inner join itensorc
                cmd.CommandText = @"SELECT"
                + " do.codigo, do.codigocotacao, do.codigoorcamento, do.codigovenda, do.codmov, do.nomemov"
                + ",do.tipomov,do.data, do.valor, do.feito, do.codvendedor, do.vendedor "
                + ",do.codoperador, do.operador,do.cliente,do.hora,do.codCliente,do.desconto "
                + ",do.percdesc, do.totalbruto,do.innomeorc, do.recebido, do.cupomfiscal,do.ENTREGA "
                + ",do.loja, do.INICIOCUPOM, do.liberado, do.codcaixa, do.objetocaixa, do.controlecx "
                + ",do.filial,do.obs,do.gaveta, do.COO,do.CAUCAO, do.CodigoGerenteManteveReserva "
                + ",do.SetorEntrega_Resp, do.SetorEntrega_Conf,do.SetorEntrega_Entregador, do.SetorEntrega_DataHoraEntrega,do.ImpressoCotacao, do.ImpressoVenda"
                + ",do.faturada, do.MarcarFaturada,do.CodigoDocFaturado, do.TotalProdutos,do.TotalNota,do.ValorIcms"
                + ",do.BaseCalcValorIcms,do.ValorIcmsSubst ,do.BaseCalcValorIcmsSubst, do.frete,do.seguro,do.outrasdesp "
                + ",do.valoripi,do.nota_nome,do.nota_endereco ,do.nota_bairro,do.nota_cidade,do.nota_uf"
                + ",do.nota_cep,do.nota_dadosadicionais,do.nota_dataentrega, do.nota_dataemissao,do.nota_numero,do.nota_CFOP "
                + ",do.FRACAO,do.TotaNotal,do.entrega_nome, do.entrega_cpf,do.entrega_rg,do.entrega_endereco "
                + ",do.entrega_bairro,do.entrega_cep, do.entrega_fone, do.entrega_cidade,do.entrega_uf, do.valortotalnotafiscal "
                + ",do.dinheiro, do.troco, do.pdv, do.EstoqueOK, do.codigoprocessamento, do.acrescimo "
                + ",do.acrescimopercentual, do.naosinc, do.mesclagem_nota, do.mesclagem_seq, do.xml_recebimento, do.descatacado "
                + " "

                + ",iorc.idunico, iorc.codigo, iorc.codprod, iorc.descricao, iorc.codmov, iorc.nomemov "
                + ",iorc.tipomov, iorc.data, iorc.quantidade, iorc.preco, iorc.total, iorc.codcor "
                + ",iorc.codtam, iorc.ref, iorc.descp, iorc.descv, iorc.tipodesc, iorc.precooriginal "
                + ",iorc.nomecor, iorc.icms, iorc.unidade, iorc.loja, iorc.vendedor, iorc.nomevendedor "
                + ",iorc.docgerado, iorc.st, iorc.aliquota, iorc.codaliquota, iorc.descrateio, iorc.IMPRESSO "
                + ",iorc.liquido, iorc.custo, iorc.nome_impresso, iorc.tabela, iorc.permitidodesconto, iorc.custo_av "
                + ",iorc.custo_ap, iorc.lucro_av, iorc.lucro_ap, iorc.perc_desc_autorizado, iorc.comissao, iorc.Condicional_Marcado_Exclusao "
                + ",iorc.estoqueOK, iorc.ComissaoVendedor, iorc.DataComissao, iorc.idLote, iorc.DescontoLiberado, iorc.SEQ "
                + ",iorc.percIpi, iorc.ValorIpi, iorc.SituacaoTributariaDesc, iorc.ValorIcms, iorc.BaseCalcValorIcms, iorc.ValorIcmsSubst "
                + ",iorc.BaseCalcValorIcmsSubst, iorc.valortotalnotafiscal, iorc.dadosencomenda, iorc.previsaoentrega, iorc.dataconclusaoservico, iorc.dataentrega "
                + ",iorc.obsentrega, iorc.situacaoencomenda, iorc.obsconclusao, iorc.encomenda_encerrada, iorc.encomenda_dataencerrada, iorc.em_execucao_encomenda "
                + ",iorc.data_inicio_execucao, iorc.FRACIONADO, iorc.situacao_def_consig, iorc.codigo_recebimento_revenda, iorc.codigorevenda "
                + ",iorc.codigoprocessamento, iorc.ncm, iorc.codvendedor, iorc.sequenciaoriginal, iorc.codigooriginal, iorc.qtconf "
                + ",iorc.naosinc, iorc.precoatacadoitem, iorc.descatacadoitem "
                + " FROM dadosorc do "
                + " inner join itensorc iorc "
                + " on do.codigo = iorc.codigo "

                + " AND do.recebido='N' AND do.feito='S' ";
                #endregion

                SqlDataReader reader = cmd.ExecuteReader();

                #region Itera no retorno da consulta e monta a lista de Cotacoes
                Dictionary<long, DadosOrc> cotacoes = new Dictionary<long, DadosOrc>();
                while (reader.Read())
                {
                    DadosOrc cotacao = new DadosOrc();

                    if (!cotacoes.ContainsKey(reader.GetInt64(0)))
                    {
                        cotacao.Codigo = (long)reader.GetInt64(0);
                        cotacao.CodigoCotacao = (int)((DBNull.Value == reader[1]) ? default(int) : reader.GetInt32(1));
                        cotacao.CodigoOrcamento = (int)((DBNull.Value == reader[2]) ? default(int) : reader.GetInt32(2));
                        cotacao.CodigoVenda = (int)((DBNull.Value == reader[3]) ? default(int) : reader.GetInt32(3));
                        cotacao.CodMov = ((DBNull.Value == reader[4]) ? string.Empty : reader.GetString(4)).ToString();
                        cotacao.NomeMov = ((DBNull.Value == reader[5]) ? string.Empty : reader.GetString(5)).ToString();

                        cotacao.TipoMov = ((DBNull.Value == reader[6]) ? string.Empty : reader.GetString(6)).ToString();
                        cotacao.Data = ((DBNull.Value == reader[7]) ? new DateTime(1901, 01, 01) : reader.GetDateTime(7));
                        cotacao.Valor = (decimal)((DBNull.Value == reader[8]) ? default(decimal) : reader.GetDecimal(8));
                        cotacao.Feito = ((DBNull.Value == reader[9]) ? string.Empty : reader.GetString(9)).ToString();
                        cotacao.CodVendedor = (string)((DBNull.Value == reader[10]) ? string.Empty : reader.GetString(10));
                        cotacao.NomeVendedor = (string)((DBNull.Value == reader[11]) ? string.Empty : reader.GetString(11));



                        cotacao.PercDesc = (decimal)((DBNull.Value == reader[18]) ? default(decimal) : reader.GetDecimal(18));
                        cotacao.TotalBruto = (decimal)((DBNull.Value == reader[19]) ? default(decimal) : reader.GetDecimal(19));
                        cotacao.InNomeOrc = (string)((DBNull.Value == reader[20]) ? string.Empty : reader.GetString(20));
                        cotacao.Recebido = (string)((DBNull.Value == reader[21]) ? string.Empty : reader.GetString(21));
                        cotacao.CupomFiscal = (string)((DBNull.Value == reader[22]) ? string.Empty : reader.GetString(22));
                        cotacao.Entrega = (string)((DBNull.Value == reader[23]) ? string.Empty : reader.GetString(23));

                        cotacao.Loja = (int)((DBNull.Value == reader[24]) ? default(int) : reader.GetInt32(24));
                        cotacao.InicioCupom = (string)((DBNull.Value == reader[25]) ? string.Empty : reader.GetString(25));
                        cotacao.Liberado = (string)((DBNull.Value == reader[26]) ? string.Empty : reader.GetString(26));
                        cotacao.CodCaixa = (int)((DBNull.Value == reader[27]) ? default(int) : reader.GetInt32(27));
                        cotacao.ObjetoCaixa = (int)((DBNull.Value == reader[28]) ? default(int) : reader.GetInt32(28));
                        cotacao.ControleCx = (string)((DBNull.Value == reader[29]) ? string.Empty : reader.GetString(29));

                        cotacao.Filial = (int)((DBNull.Value == reader[30]) ? default(int) : reader.GetInt32(30));
                        cotacao.Obs = (string)((DBNull.Value == reader[31]) ? string.Empty : reader.GetString(31));
                        cotacao.Gaveta = (bool)((DBNull.Value == reader[32]) ? false : reader.GetBoolean(32));
                        cotacao.COO = (string)((DBNull.Value == reader[33]) ? string.Empty : reader.GetString(33));
                        cotacao.Caucao = (bool)((DBNull.Value == reader[34]) ? false : reader.GetBoolean(34));
                        cotacao.CodigoGerenteManteveReserva = (int)((DBNull.Value == reader[35]) ? 0 : reader.GetInt32(35));

                        cotacao.SetorEntregaResp = (int)((DBNull.Value == reader[36]) ? default(int) : reader.GetInt32(36));
                        cotacao.SetorEntregaConf = (int)((DBNull.Value == reader[37]) ? default(int) : reader.GetInt32(37));
                        cotacao.SetorEntregaEntregador = (int)((DBNull.Value == reader[38]) ? default(int) : reader.GetInt32(38));
                        cotacao.SetorEntregaDataHoraEntrega = (DateTime)((DBNull.Value == reader[39]) ? new DateTime(1901, 01, 01) : reader.GetDateTime(39));
                        cotacao.ImpressoCotacao = (int)((DBNull.Value == reader[40]) ? default(int) : reader.GetInt32(40));
                        cotacao.ImpressoVenda = (int)((DBNull.Value == reader[41]) ? default(int) : reader.GetInt32(41));

                        cotacao.Faturada = (bool)((DBNull.Value == reader[42]) ? false : reader.GetBoolean(42));
                        cotacao.MarcarFaturada = (bool)((DBNull.Value == reader[43]) ? false : reader.GetBoolean(43));
                        cotacao.CodigoDocFaturado = (int)((DBNull.Value == reader[44]) ? default(int) : reader.GetInt32(44));
                        cotacao.TotalProdutos = (decimal)((DBNull.Value == reader[45]) ? default(decimal) : reader.GetDecimal(45));
                        cotacao.TotalNota = (decimal)((DBNull.Value == reader[46]) ? default(decimal) : reader.GetDecimal(46));
                        cotacao.ValorIcms = (decimal)((DBNull.Value == reader[47]) ? default(decimal) : reader.GetDecimal(47));

                        cotacao.BaseCalcValorIcms = (decimal)((DBNull.Value == reader[48]) ? default(decimal) : reader.GetDecimal(48));
                        cotacao.ValorIcmsSubst = (decimal)((DBNull.Value == reader[49]) ? default(decimal) : reader.GetDecimal(49));
                        cotacao.BaseCalcValorIcmsSubst = (decimal)((DBNull.Value == reader[50]) ? default(decimal) : reader.GetDecimal(50));
                        cotacao.Frete = (decimal)((DBNull.Value == reader[51]) ? default(decimal) : reader.GetDecimal(51));
                        cotacao.Seguro = (decimal)((DBNull.Value == reader[52]) ? default(decimal) : reader.GetDecimal(52));
                        cotacao.OutrasDesp = (decimal)((DBNull.Value == reader[53]) ? default(decimal) : reader.GetDecimal(53));

                        cotacao.ValorIpi = (decimal)((DBNull.Value == reader[54]) ? default(decimal) : reader.GetDecimal(54));
                        cotacao.NotaNome = (string)((DBNull.Value == reader[55]) ? string.Empty : reader.GetString(55));
                        cotacao.NotaEndereco = (string)((DBNull.Value == reader[56]) ? string.Empty : reader.GetString(56));
                        cotacao.NotaBairro = (string)((DBNull.Value == reader[57]) ? string.Empty : reader.GetString(57));
                        cotacao.NotaCidade = (string)((DBNull.Value == reader[58]) ? string.Empty : reader.GetString(58));
                        cotacao.NotaUf = (string)((DBNull.Value == reader[59]) ? string.Empty : reader.GetString(59));

                        cotacao.NotaCep = (string)((DBNull.Value == reader[60]) ? string.Empty : reader.GetString(60));
                        cotacao.NotaDadosAdicionais = (string)((DBNull.Value == reader[61]) ? string.Empty : reader.GetString(61));
                        cotacao.NotaDataEntrega = ((DBNull.Value == reader[62]) ? new DateTime(1901, 01, 01) : reader.GetDateTime(62));
                        cotacao.NotaDataEmissao = ((DBNull.Value == reader[63]) ? new DateTime(1901, 01, 01) : reader.GetDateTime(63));
                        cotacao.NotaNumero = (int)((DBNull.Value == reader[64]) ? default(int) : reader.GetInt32(64));
                        cotacao.NotaCfop = (string)((DBNull.Value == reader[65]) ? string.Empty : reader.GetString(65));

                        cotacao.Fracao = (decimal)((DBNull.Value == reader[66]) ? default(decimal) : reader.GetDecimal(66));
                        cotacao.TotalNota = (decimal)((DBNull.Value == reader[67]) ? default(decimal) : reader.GetDecimal(67));
                        cotacao.EntregaNome = (string)((DBNull.Value == reader[68]) ? string.Empty : reader.GetString(68));
                        cotacao.EntregaCpf = (string)((DBNull.Value == reader[69]) ? string.Empty : reader.GetString(69));
                        cotacao.EntregaRg = (string)((DBNull.Value == reader[70]) ? string.Empty : reader.GetString(70));
                        cotacao.EntregaEndereco = (string)((DBNull.Value == reader[71]) ? string.Empty : reader.GetString(71));

                        cotacao.EntregaBairro = (string)((DBNull.Value == reader[72]) ? string.Empty : reader.GetString(72));
                        cotacao.EntregaCep = (string)((DBNull.Value == reader[73]) ? string.Empty : reader.GetString(73));
                        cotacao.EntregaFone = (string)((DBNull.Value == reader[74]) ? string.Empty : reader.GetString(74));
                        cotacao.EntregaCidade = (string)((DBNull.Value == reader[75]) ? string.Empty : reader.GetString(75));
                        cotacao.EntregaUf = (string)((DBNull.Value == reader[76]) ? string.Empty : reader.GetString(76));
                        cotacao.ValorTotalNotaFiscal = (int)((DBNull.Value == reader[77]) ? 0 : reader.GetInt32(77));

                        cotacao.Dinheiro = (decimal)((DBNull.Value == reader[78]) ? default(decimal) : reader.GetDecimal(78));
                        cotacao.Troco = (decimal)((DBNull.Value == reader[79]) ? default(decimal) : reader.GetDecimal(79));
                        cotacao.PDV = (int)((DBNull.Value == reader[80]) ? 0 : reader.GetInt32(80));
                        cotacao.EstoqueOK = (int)((DBNull.Value == reader[81]) ? 0 : reader.GetInt32(81));
                        cotacao.CodigoProcessamento = (int)((DBNull.Value == reader[82]) ? default(int) : reader.GetInt32(82));

                        //cotacao.Acrescimo = (float)(Convert.IsDBNull(reader[83]) ? default(float) : reader.GetFloat(83));
                        //cotacao.AcrescimoPercentual = (float)((DBNull.Value == reader[84]) ? default(float) : reader.GetFloat(84));
                        cotacao.NaoSinc = (bool)((DBNull.Value == reader[85]) ? false : reader.GetBoolean(85));
                        cotacao.MesclagemNota = (long)((DBNull.Value == reader[86]) ? default(long) : reader.GetInt64(86));
                        cotacao.MesclagemSeq = (string)((DBNull.Value == reader[87]) ? string.Empty : reader.GetString(87));
                        cotacao.XmlRecebimento = (string)((DBNull.Value == reader[88]) ? string.Empty : reader.GetString(88));
                        cotacao.DescAtacado = (decimal)((DBNull.Value == reader[89]) ? default(decimal) : reader.GetDecimal(89));


                        cotacoes.Add(cotacao.Codigo, cotacao);
                    }

                    else
                    {
                        cotacao = cotacoes[reader.GetInt64(0)];
                    }

                    #region Monta a lista de itens da cotação DadosOrc.ItensOrc
                    ItemOrc item = new ItemOrc()
                    {
                        //iorc.idunico, iorc.codigo, iorc.codprod, iorc.descricao, iorc.codmov, iorc.nomemov
                        IdUnico = (long)((DBNull.Value == reader[90]) ? default(long) : reader.GetInt64(90)),
                        Codigo = (long)((DBNull.Value == reader[91]) ? default(long) : reader.GetInt64(91)),
                        CodProd = (int)((DBNull.Value == reader[92]) ? default(int) : reader.GetInt32(92)),
                        DescricaoProd = (string)((DBNull.Value == reader[93]) ? string.Empty : reader.GetString(93)),
                        CodMov = (string)((DBNull.Value == reader[94]) ? string.Empty : reader.GetString(94)),
                        NomeMov = (string)((DBNull.Value == reader[95]) ? string.Empty : reader.GetString(95)),

                        //"iorc.tipomov, iorc.data, iorc.quantidade, iorc.preco, iorc.total, iorc.codcor "
                        TipoMov = (string)((DBNull.Value == reader[96]) ? string.Empty : reader.GetString(96)),
                        Data = (DateTime)((DBNull.Value == reader[97]) ? new DateTime(1900, 01, 01) : reader.GetDateTime(97)),
                        Quantidade = (decimal)((DBNull.Value == reader[98]) ? default(decimal) : reader.GetDecimal(98)),
                        Preco = (decimal)((DBNull.Value == reader[99]) ? default(decimal) : reader.GetDecimal(99)),
                        Total = (decimal)((DBNull.Value == reader[100]) ? default(decimal) : reader.GetDecimal(100)),
                        CodCor = (string)((DBNull.Value == reader[101]) ? string.Empty : reader.GetString(101)),

                        //iorc.codtam,iorc.ref, iorc.descp, iorc.descv, iorc.tipodesc, iorc.precooriginal
                        CodTam = (string)((DBNull.Value == reader[102]) ? string.Empty : reader.GetString(102)),
                        REF = (string)((DBNull.Value == reader[103]) ? string.Empty : reader.GetString(103)),
                        DescP = (decimal)((DBNull.Value == reader[104]) ? default(decimal) : reader.GetDecimal(104)),
                        DescV = (decimal)((DBNull.Value == reader[105]) ? default(decimal) : reader.GetDecimal(105)),
                        TipoDesc = (string)((DBNull.Value == reader[106]) ? string.Empty : reader.GetString(106)),
                        PrecoOriginal = (decimal)((DBNull.Value == reader[107]) ? default(decimal) : reader.GetDecimal(107)),

                        //iorc.nomecor,iorc.icms,iorc.unidade,iorc.loja,iorc.vendedor,iorc.nomevendedor
                        NomeCor = (string)((DBNull.Value == reader[108]) ? string.Empty : reader.GetString(108)),
                        Icms = (string)((DBNull.Value == reader[109]) ? string.Empty : reader.GetString(109)),
                        UN = (string)((DBNull.Value == reader[110]) ? string.Empty : reader.GetString(110)),
                        LojaId = (int)((DBNull.Value == reader[111]) ? default(int) : reader.GetInt32(111)),
                        VendedorId = (int)((DBNull.Value == reader[112]) ? default(int) : reader.GetInt32(112)),
                        NomeVendedor = (string)((DBNull.Value == reader[113]) ? string.Empty : reader.GetString(113)),

                        //iorc.docgerado, iorc.st, iorc.aliquota, iorc.codaliquota, iorc.descrateio, iorc.IMPRESSO
                        DocGerado = (int)((DBNull.Value == reader[114]) ? default(int) : reader.GetInt32(114)),
                        st = (string)((DBNull.Value == reader[115]) ? "" : reader.GetString(115)),
                        Aliquota = (decimal)((DBNull.Value == reader[116]) ? default(decimal) : reader.GetDecimal(116)),
                        CodAliquota = (string)((DBNull.Value == reader[117]) ? "" : reader.GetString(117)),
                        DescRateio = (string)((DBNull.Value == reader[118]) ? "" : reader.GetString(118)),
                        IMPRESSO = (string)((DBNull.Value == reader[119]) ? "" : reader.GetString(119)),

                        //iorc.liquido, iorc.custo, iorc.nome_impresso, iorc.tabela, iorc.permitidodesconto, iorc.custo_av
                        Liquido = (decimal)((DBNull.Value == reader[120]) ? default(decimal) : reader.GetDecimal(120)),
                        Custo = (decimal)((DBNull.Value == reader[121]) ? default(decimal) : reader.GetDecimal(121)),
                        NomeImpresso = (string)((DBNull.Value == reader[122]) ? string.Empty : reader.GetString(122)),
                        Tabela = (string)((DBNull.Value == reader[123]) ? string.Empty : reader.GetString(123)),
                        PermitidoDesconto = (string)((DBNull.Value == reader[124]) ? "" : reader.GetString(124)),
                        CustoAV = (decimal)((DBNull.Value == reader[125]) ? default(decimal) : reader.GetDecimal(125)),

                        //iorc.custo_ap, iorc.lucro_av, iorc.lucro_ap, iorc.perc_desc_autorizado, iorc.comissao, iorc.Condicional_Marcado_Exclusao
                        CustoAP = (decimal)((DBNull.Value == reader[126]) ? default(decimal) : reader.GetDecimal(126)),
                        LucroAV = (decimal)((DBNull.Value == reader[127]) ? default(decimal) : reader.GetDecimal(127)),
                        LucroAP = (decimal)((DBNull.Value == reader[128]) ? default(decimal) : reader.GetDecimal(128)),
                        PercDescAutorizado = (decimal)((DBNull.Value == reader[129]) ? default(decimal) : reader.GetDecimal(129)),
                        Comissao = (decimal)((DBNull.Value == reader[130]) ? default(decimal) : reader.GetDecimal(130)),
                        CondicionalMarcadoExclusao = (bool)((DBNull.Value == reader[131]) ? false : reader.GetBoolean(131)),

                        //iorc.estoqueOK, iorc.ComissaoVendedor, iorc.DataComissao, iorc.idLote, iorc.DescontoLiberado, iorc.SEQ
                        EstoqueOK = (bool)((DBNull.Value == reader[132]) ? false : reader.GetBoolean(132)),
                        ComissaoVendedor = (decimal)((DBNull.Value == reader[133]) ? default(decimal) : reader.GetDecimal(133)),
                        DataComissao = (DateTime)((DBNull.Value == reader[134]) ? new DateTime(1900, 01, 01) : reader.GetDateTime(134)),
                        IdLote = (string)((DBNull.Value == reader[135]) ? string.Empty : reader.GetString(135)),
                        DescontoLiberado = (decimal)((DBNull.Value == reader[136]) ? default(decimal) : reader.GetDecimal(136)),
                        Seq = (int)((DBNull.Value == reader[137]) ? default(int) : reader.GetInt32(137)),

                        //iorc.percIpi, iorc.ValorIpi, iorc.SituacaoTributariaDesc, iorc.ValorIcms, iorc.BaseCalcValorIcms, iorc.ValorIcmsSubst
                        PercIpi = (decimal)((DBNull.Value == reader[138]) ? default(decimal) : reader.GetDecimal(138)),
                        ValorIpi = (decimal)((DBNull.Value == reader[139]) ? default(decimal) : reader.GetDecimal(139)),
                        SituacaoTributariaDesc = (string)((DBNull.Value == reader[140]) ? string.Empty : reader.GetString(140)),
                        ValorIcms = (decimal)((DBNull.Value == reader[141]) ? default(decimal) : reader.GetDecimal(141)),
                        BaseCalcValorIcms = (decimal)((DBNull.Value == reader[142]) ? default(decimal) : reader.GetDecimal(142)),
                        ValorIcmsSubst = (decimal)((DBNull.Value == reader[143]) ? default(decimal) : reader.GetDecimal(143)),

                        //iorc.BaseCalcValorIcmsSubst, iorc.valortotalnotafiscal, iorc.dadosencomenda, iorc.previsaoentrega, iorc.dataconclusaoservico, iorc.dataentrega
                        BaseCalcValorIcmsSubst = (decimal)((DBNull.Value == reader[144]) ? default(decimal) : reader.GetDecimal(144)),
                        ValorTotalNotaFiscal = (int)((DBNull.Value == reader[145]) ? default(int) : reader.GetInt32(145)),
                        DadosEncomenda = (string)((DBNull.Value == reader[146]) ? string.Empty : reader.GetString(146)),
                        PrevisaoDeEntrega = (DateTime)((Convert.IsDBNull(reader[147])) ? new DateTime(1900, 01, 01) : reader.GetDateTime(147)),
                        DataConclusaoServico = (DateTime)((Convert.IsDBNull(reader[148])) ? new DateTime(1900, 01, 01) : reader.GetDateTime(148)),
                        DataEntrega = (DateTime)((Convert.IsDBNull(reader[149])) ? new DateTime(1900, 01, 01) : reader.GetDateTime(149)),

                        //iorc.obsentrega, iorc.situacaoencomenda, iorc.obsconclusao, iorc.encomenda_encerrada, iorc.encomenda_dataencerrada, iorc.em_execucao_encomenda
                        ObsEntrega = (string)((DBNull.Value == reader[150]) ? string.Empty : reader.GetString(150)),
                        SituacaoEncomenda = (string)((DBNull.Value == reader[151]) ? string.Empty : reader.GetString(151)),
                        ObsConclusao = (string)((DBNull.Value == reader[152]) ? string.Empty : reader.GetString(152)),
                        EncomendaEncerrada = (bool)((DBNull.Value == reader[153]) ? false : reader.GetBoolean(153)),
                        EncomendaDataEncerrada = (DateTime)((Convert.IsDBNull(reader[154])) ? new DateTime(1900, 01, 01) : reader.GetDateTime(154)),
                        EmExecucaoEncomenda = (bool)((DBNull.Value == reader[155]) ? false : reader.GetBoolean(155)),

                        ////iorc.data_inicio_execucao, iorc.FRACIONADO, iorc.situacao_def_consig, iorc.codigo_recebimento_revenda, iorc.codigorevenda
                        DataInicioExecucao = (DateTime)((Convert.IsDBNull(reader[156])) ? new DateTime(1900, 01, 01) : reader.GetDateTime(156)),
                        Fracionado = (bool)((DBNull.Value == reader[157]) ? false : reader.GetBoolean(157)),
                        SituacaoDefConsig = (string)((DBNull.Value == reader[158]) ? string.Empty : reader.GetString(158)),
                        CodigoRecebimentoRevenda = (int)((DBNull.Value == reader[159]) ? default(int) : reader.GetInt32(159)),
                        CodigoRevenda = (int)((DBNull.Value == reader[160]) ? default(int) : reader.GetInt32(160)),

                        ////iorc.codigoprocessamento, iorc.ncm, iorc.codvendedor, iorc.sequenciaoriginal, iorc.codigooriginal, iorc.qtconf
                        CodigoProcessamento = (int)((DBNull.Value == reader[161]) ? default(int) : reader.GetInt32(161)),
                        NCM = (string)((DBNull.Value == reader[162]) ? string.Empty : reader.GetString(162)),
                        //CodVendedor = default(float),
                        SequenciaOriginal = (string)((DBNull.Value == reader[164]) ? string.Empty : reader.GetString(164)),
                        CodigoOriginal = (long)((DBNull.Value == reader[165]) ? default(long) : reader.GetInt64(165)),
                        //QtConf = (float)((DBNull.Value == reader[166]) ? default(float) : reader.GetFloat(166)),

                        ////iorc.naosinc, iorc.precoatacadoitem, iorc.descatacadoitem
                        NaoSinc = (bool)((DBNull.Value == reader[167]) ? false : reader.GetBoolean(167)),
                        PrecoAtacadoItem = (decimal)((DBNull.Value == reader[168]) ? default(decimal) : reader.GetDecimal(168)),
                        DescAtacadoItem = (decimal)((DBNull.Value == reader[169]) ? default(decimal) : reader.GetDecimal(169)),
                    };


                    cotacao.itens = (cotacao.itens == null) ? new List<ItemOrc>() : cotacao.itens;
                    cotacao.itens.Add(item);
                    #endregion
                }

                return cotacoes.Values.ToList();
                #endregion
            }

        }
        public void Inserir(DadosOrc cotacao)
        {
            using (_connection = new SqlConnection(_con))
            {
                _connection.Open();
                SqlCommand cmd = (SqlCommand)_connection.CreateCommand();
                cmd.Connection = (SqlConnection)_connection;
                SqlTransaction transaction = (SqlTransaction)_connection.BeginTransaction();
                cmd.Transaction = transaction;
                
                try
                {
                    #region Monta a string DML: Insert Into DadosOrc...
                    string strInsert = "INSERT INTO dadosorc("
                        + " codigo, codigocotacao, codigoorcamento, codigovenda, codmov, nomemov"
                    + ",tipomov,data, valor, feito, codvendedor, vendedor "
                    + ",codoperador, operador,cliente,hora,codCliente,desconto "
                    + ",percdesc, totalbruto,innomeorc, recebido, cupomfiscal, ENTREGA "
                    + ",loja, INICIOCUPOM, liberado, codcaixa, objetocaixa, controlecx "
                    + ",filial, obs, gaveta, COO, CAUCAO, CodigoGerenteManteveReserva "
                    + ",SetorEntrega_Resp, SetorEntrega_Conf,SetorEntrega_Entregador, SetorEntrega_DataHoraEntrega,ImpressoCotacao, ImpressoVenda"
                    + ",faturada, MarcarFaturada, CodigoDocFaturado, TotalProdutos, TotalNota, ValorIcms"
                    + ",BaseCalcValorIcms, ValorIcmsSubst , BaseCalcValorIcmsSubst, frete, seguro, outrasdesp "
                    + ",valoripi, nota_nome, nota_endereco , nota_bairro, nota_cidade, nota_uf"
                    + ",nota_cep, nota_dadosadicionais, nota_dataentrega, nota_dataemissao, nota_numero, nota_CFOP "
                    + ",FRACAO, TotaNotal, entrega_nome, entrega_cpf, entrega_rg, entrega_endereco "
                    + ",entrega_bairro, entrega_cep, entrega_fone, entrega_cidade, entrega_uf, valortotalnotafiscal "
                    + ",dinheiro, troco, pdv, EstoqueOK, codigoprocessamento, acrescimo "
                    + ",acrescimopercentual, naosinc, mesclagem_nota, mesclagem_seq, xml_recebimento, descatacado "
                    + ") VALUES("

                    + " @codigo, @codigocotacao, @codigoorcamento, @codigovenda, @codmov, @nomemov"
                    + ",@tipomov, @data, @valor, @feito, @codvendedor, @vendedor "
                    + ",@codoperador, @operador, @cliente, @hora, @codCliente, @desconto "
                    + ",@percdesc, @totalbruto, @innomeorc, @recebido, @cupomfiscal, @ENTREGA "
                    + ",@loja, @INICIOCUPOM, @liberado, @codcaixa, @objetocaixa, @controlecx "
                    + ",@filial, @obs, @gaveta, @COO, @CAUCAO, @CodigoGerenteManteveReserva "
                    + ",@SetorEntrega_Resp, @SetorEntrega_Conf, @SetorEntrega_Entregador, @SetorEntrega_DataHoraEntrega, @ImpressoCotacao, @ImpressoVenda"
                    + ",@faturada, @MarcarFaturada, @CodigoDocFaturado, @TotalProdutos, @TotalNota, @ValorIcms"
                    + ",@BaseCalcValorIcms, @ValorIcmsSubst , @BaseCalcValorIcmsSubst, @frete, @seguro, @outrasdesp "
                    + ",@valoripi, @nota_nome, @nota_endereco, @nota_bairro, @nota_cidade, @nota_uf"
                    + ",@nota_cep, @nota_dadosadicionais, @nota_dataentrega, @nota_dataemissao, @nota_numero, @nota_CFOP "
                    + ",@FRACAO, @TotaNotal, @entrega_nome, @entrega_cpf, @entrega_rg, @entrega_endereco "
                    + ",@entrega_bairro, @entrega_cep, @entrega_fone, @entrega_cidade, @entrega_uf, @valortotalnotafiscal "
                    + ",@dinheiro, @troco, @pdv, @EstoqueOK, @codigoprocessamento, @acrescimo "
                    + ",@acrescimopercentual, @naosinc, @mesclagem_nota, @mesclagem_seq, @xml_recebimento, @descatacado) "
                    + " SELECT CAST(scope_identity() AS int)";
                    cmd.CommandText = strInsert;
                    #endregion

                    #region Definição dos parametros do Inserto Into DadosOrc
                    //@codigo, @codigocotacao, @codigoorcamento, @codigovenda, @codmov, @nomemov
                    long codigo = GerarId.GerarNovoCodigoTabela("dadosorc", true);
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    long codigoCotacao = default(long);
                    long codigoOrcamento = default(long);
                    GerarId.GerarNovoSequencialTabela("dadosorc", DateTime.Now, out codigoCotacao, out codigoOrcamento);
                    cotacao.CodigoCotacao = (int)codigoCotacao;
                    cotacao.CodigoOrcamento = (int)codigoOrcamento;
                    cmd.Parameters.AddWithValue("@codigocotacao", cotacao.CodigoCotacao);
                    cmd.Parameters.AddWithValue("@codigoorcamento", cotacao.CodigoOrcamento);
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codigovenda", DBNull.Value, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codmov", cotacao.CodMov, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nomemov", cotacao.NomeMov, null));

                    //@tipomov, @data, @valor, @feito, @codvendedor, @vendedor
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@tipomov", cotacao.TipoMov, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@data", cotacao.Data.ToShortDateString(), null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@valor", cotacao.Valor, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@feito", cotacao.Feito, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codvendedor", cotacao.CodVendedor, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@vendedor", cotacao.NomeVendedor, null));
                    
                    //@codoperador, @operador, @cliente, @hora, @codCliente, @desconto
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codoperador", null, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@operador", null, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@cliente", cotacao.NomeCliente, null));
                    //cmd.Parameters.AddWithValue("@hora",string.Format("{0}", cotacao.Hora.TimeOfDay));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@hora", cotacao.Hora, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codcliente", cotacao.CodCliente, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@desconto", cotacao.Desconto, null));

                    //@percdesc, @totalbruto, @innomeorc, @recebido, @cupomfiscal, @ENTREGA
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@percdesc", cotacao.PercDesc, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@totalbruto", cotacao.TotalBruto, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@innomeorc", cotacao.InNomeOrc, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@recebido", cotacao.Recebido, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@cupomfiscal", cotacao.CupomFiscal, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@ENTREGA", cotacao.Entrega, null));

                    //@loja, @INICIOCUPOM, @liberado, @codcaixa, @objetocaixa, @controlecx
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@loja", cotacao.Loja, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@INICIOCUPOM", cotacao.InicioCupom, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@liberado", cotacao.Liberado, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codcaixa", null, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@objetocaixa", null, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@controlecx", null, null));

                    //@filial, @obs, @gaveta, @COO, @CAUCAO, @CodigoGerenteManteveReserva
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@filial", null, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@obs", cotacao.Obs, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@gaveta", cotacao.Gaveta, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@COO", cotacao.COO, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@CAUCAO", cotacao.Caucao, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@CodigoGerenteManteveReserva", cotacao.CodigoGerenteManteveReserva, null));

                    //@SetorEntrega_Resp, @SetorEntrega_Conf, @SetorEntrega_Entregador, @SetorEntrega_DataHoraEntrega, @ImpressoCotacao, @ImpressoVenda
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@SetorEntrega_Resp", cotacao.SetorEntregaResp, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@SetorEntrega_Conf", cotacao.SetorEntregaConf, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@SetorEntrega_Entregador", cotacao.SetorEntregaEntregador, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@SetorEntrega_DataHoraEntrega", cotacao.SetorEntregaDataHoraEntrega, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@ImpressoCotacao", cotacao.ImpressoCotacao, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@ImpressoVenda", cotacao.ImpressoVenda, null));

                    //@faturada, @MarcarFaturada, @CodigoDocFaturado, @TotalProdutos, @TotalNota, @ValorIcms
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@faturada", cotacao.Faturada, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@MarcarFaturada", cotacao.MarcarFaturada, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@CodigoDocFaturado", cotacao.CodigoDocFaturado, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@TotalProdutos", cotacao.TotalProdutos, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@TotalNota", cotacao.TotalNota, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@ValorIcms", cotacao.ValorIcms, null));

                    //@BaseCalcValorIcms, @ValorIcmsSubst , @BaseCalcValorIcmsSubst, @frete, @seguro, @outrasdesp
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@BaseCalcValorIcms", cotacao.BaseCalcValorIcms, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@ValorIcmsSubst", cotacao.ValorIcmsSubst, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@BaseCalcValorIcmsSubst", cotacao.BaseCalcValorIcmsSubst, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@frete", cotacao.Frete, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@seguro", cotacao.Seguro, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@outrasdesp", cotacao.OutrasDesp, null));

                    //@valoripi, @nota_nome, @nota_endereco, @nota_bairro, @nota_cidade, @nota_uf
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@valoripi", cotacao.ValorIpi, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nota_nome", cotacao.NotaNome, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nota_endereco", cotacao.NotaEndereco, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nota_bairro", cotacao.NotaBairro, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nota_cidade", cotacao.NotaCidade, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nota_uf", cotacao.NotaUf, null));

                    //@nota_cep, @nota_dadosadicionais, @nota_dataentrega, @nota_dataemissao, @nota_numero, @nota_CFOP
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nota_cep", cotacao.NotaCep, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nota_dadosadicionais", cotacao.NotaDadosAdicionais, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nota_dataentrega", cotacao.NotaDataEntrega, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nota_dataemissao", cotacao.NotaDataEmissao, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nota_numero", cotacao.NotaNumero, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nota_CFOP", cotacao.NotaCfop, null));

                    //@FRACAO, @TotaNotal, @entrega_nome, @entrega_cpf, @entrega_rg, @entrega_endereco
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@FRACAO", cotacao.Fracao, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@TotaNotal", cotacao.TotalNota, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@entrega_nome", cotacao.EntregaNome, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@entrega_cpf", cotacao.EntregaCpf, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@entrega_rg", cotacao.EntregaRg, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@entrega_endereco", cotacao.EntregaEndereco, null));

                    //@entrega_bairro, @entrega_cep, @entrega_fone, @entrega_cidade, @entrega_uf, @valortotalnotafiscal
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@entrega_bairro", cotacao.EntregaBairro, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@entrega_cep", cotacao.EntregaCep, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@entrega_fone", cotacao.EntregaFone, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@entrega_cidade", cotacao.EntregaCidade, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@entrega_uf", cotacao.EntregaUf, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@valortotalnotafiscal", cotacao.ValorTotalNotaFiscal, null));

                    //@dinheiro, @troco, @pdv, @EstoqueOK, @codigoprocessamento, @acrescimo
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@dinheiro", null, DbType.Decimal));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@troco", null, DbType.Decimal));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@pdv", null, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@EstoqueOK", null, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codigoprocessamento", cotacao.CodigoProcessamento, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@acrescimo", cotacao.Acrescimo, null));

                    //@acrescimopercentual, @naosinc, @mesclagem_nota, @mesclagem_seq, @xml_recebimento, @descatacado
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@acrescimopercentual", cotacao.AcrescimoPercentual, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@naosinc", cotacao.NaoSinc, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@mesclagem_nota", cotacao.MesclagemNota, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@mesclagem_seq", cotacao.MesclagemSeq, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@xml_recebimento", cotacao.XmlRecebimento, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@descatacado", cotacao.DescAtacado, null));
                    #endregion

                    cmd.ExecuteScalar();
                    cmd.Parameters.Clear();

                    cotacao.Codigo = codigo;

                    if (cotacao.itens.Count > 0) //Esta validação pode ficar no FrontEnd
                    {
                        #region Monta a comando DML Insert na ItensOrc

                        cmd.CommandText = "INSERT INTO ItensOrc("
                            + "idunico, codigo, codprod, descricao, codmov, nomemov "
                    + ",tipomov, data, quantidade, preco, total, codcor "
                    + ",codtam, ref, descp, descv, tipodesc, precooriginal "
                    + ",nomecor, icms, unidade, loja, vendedor, nomevendedor "
                    + ",docgerado, st, aliquota, codaliquota, descrateio, IMPRESSO "
                    + ",liquido, custo, nome_impresso, tabela, permitidodesconto, custo_av "
                    + ",custo_ap, lucro_av, lucro_ap, perc_desc_autorizado, comissao, Condicional_Marcado_Exclusao "
                    + ",estoqueOK, ComissaoVendedor, DataComissao, idLote, DescontoLiberado, SEQ "
                    + ",percIpi, ValorIpi, SituacaoTributariaDesc, ValorIcms, BaseCalcValorIcms, ValorIcmsSubst "
                    + ",BaseCalcValorIcmsSubst, valortotalnotafiscal, dadosencomenda, previsaoentrega, dataconclusaoservico, dataentrega "
                    + ",obsentrega, situacaoencomenda, obsconclusao, encomenda_encerrada, encomenda_dataencerrada, em_execucao_encomenda "
                    + ",data_inicio_execucao, FRACIONADO, situacao_def_consig, codigo_recebimento_revenda, codigorevenda "
                    + ",codigoprocessamento, ncm, codvendedor, sequenciaoriginal, codigooriginal, qtconf "
                    + ",naosinc, precoatacadoitem, descatacadoitem) VALUES("

                    + " @idunico, @codigo, @codprod, @descricao, @codmov, @nomemov "
                    + ",@tipomov, @data, @quantidade, @preco, @total, @codcor "
                    + ",@codtam, @ref, @descp, @descv, @tipodesc, @precooriginal "
                    + ",@nomecor, @icms, @unidade, @loja, @vendedor, @nomevendedor "
                    + ",@docgerado, @st, @aliquota, @codaliquota, @descrateio, @IMPRESSO "
                    + ",@liquido, @custo, @nome_impresso, @tabela, @permitidodesconto, @custo_av "
                    + ",@custo_ap, @lucro_av, @lucro_ap, @perc_desc_autorizado, @comissao, @Condicional_Marcado_Exclusao "
                    + ",@estoqueOK, @ComissaoVendedor, @DataComissao, @idLote, @DescontoLiberado, @SEQ "
                    + ",@percIpi, @ValorIpi, @SituacaoTributariaDesc, @ValorIcms, @BaseCalcValorIcms, @ValorIcmsSubst "
                    + ",@BaseCalcValorIcmsSubst, @valortotalnotafiscal, @dadosencomenda, @previsaoentrega, @dataconclusaoservico, @dataentrega "
                    + ",@obsentrega, @situacaoencomenda, @obsconclusao, @encomenda_encerrada, @encomenda_dataencerrada, @em_execucao_encomenda "
                    + ",@data_inicio_execucao, @FRACIONADO, @situacao_def_consig, @codigo_recebimento_revenda, @codigorevenda "
                    + ",@codigoprocessamento, @ncm, @codvendedor, @sequenciaoriginal, @codigooriginal, @qtconf "
                    + ",@naosinc, @precoatacadoitem, @descatacadoitem)";
                        #endregion

                        //Itera sobre os itens da Cotação e Insere na ItensOrc
                        for (int i = 0; i < cotacao.itens.Count; i++)
                        {
                            #region Define os parametros do comando Insert na ItensOrc
                            //@idunico, @codigo, @codprod, @descricao, @codmov, @nomemov
                            long idUnico = GerarId.GerarNovoCodigoTabela("itensorc", true);
                            cmd.Parameters.AddWithValue("idunico", idUnico);
                            cmd.Parameters.AddWithValue("@codigo", cotacao.Codigo);
                            cmd.Parameters.AddWithValue("@codprod", cotacao.itens[i].CodProd);
                            cmd.Parameters.AddWithValue("@descricao", cotacao.itens[i].DescricaoProd);
                            cmd.Parameters.Add(ValidParameter.CreateParameter( "@codmov", cotacao.itens[i].CodMov, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@nomemov", cotacao.itens[i].NomeMov, null));

                            //@tipomov, @data, @quantidade, @preco, @total, @codcor
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@tipomov", cotacao.itens[i].TipoMov, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@data", DateTime.Now, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@quantidade", cotacao.itens[i].Quantidade, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@preco", cotacao.itens[i].Preco, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@total", cotacao.itens[i].Total, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@codcor", cotacao.itens[i].CodCor, null));

                            //@codtam, @ref, @descp, @descv, @tipodesc, @precooriginal 
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@codtam", cotacao.itens[i].CodTam, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@ref", cotacao.itens[i].REF, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@descp", cotacao.itens[i].DescP, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@descv", cotacao.itens[i].DescV, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@tipodesc", cotacao.itens[i].TipoDesc, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@precooriginal", cotacao.itens[i].PrecoOriginal, null));

                            //@nomecor, @icms, @unidade, @loja, @vendedor, @nomevendedor
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@nomecor", cotacao.itens[i].NomeCor, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@icms", cotacao.itens[i].Icms, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@unidade", cotacao.itens[i].UN, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@loja", cotacao.itens[i].LojaId, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@vendedor", cotacao.itens[i].VendedorId, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@nomevendedor", cotacao.itens[i].NomeVendedor, null));

                            //@docgerado, @st, @aliquota, @codaliquota, @descrateio, @IMPRESSO
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@docgerado", cotacao.itens[i].DocGerado, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@st", cotacao.itens[i].st, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@aliquota", cotacao.itens[i].Aliquota, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@codaliquota", cotacao.itens[i].CodAliquota, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@descrateio", cotacao.itens[i].DescRateio, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@IMPRESSO", cotacao.itens[i].IMPRESSO, null));

                            //@liquido, @custo, @nome_impresso, @tabela, @permitidodesconto, @custo_av
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@liquido", cotacao.itens[i].Liquido, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@custo", cotacao.itens[i].Custo, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@nome_impresso", cotacao.itens[i].NomeImpresso, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@tabela", cotacao.itens[i].Tabela, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@permitidodesconto", cotacao.itens[i].PermitidoDesconto, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@custo_av", cotacao.itens[i].CustoAV, null));

                            //@custo_ap, @lucro_av, @lucro_ap, @perc_desc_autorizado, @comissao, @Condicional_Marcado_Exclusao
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@custo_ap", cotacao.itens[i].CustoAP, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@lucro_av", cotacao.itens[i].LucroAV, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@lucro_ap", cotacao.itens[i].LucroAP, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@perc_desc_autorizado", cotacao.itens[i].PercDescAutorizado, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@comissao", cotacao.itens[i].Comissao, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@Condicional_Marcado_Exclusao", cotacao.itens[i].CondicionalMarcadoExclusao, null));

                            //@estoqueOK, @ComissaoVendedor, @DataComissao, @idLote, @DescontoLiberado, @SEQ
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@estoqueOK", cotacao.itens[i].EstoqueOK, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@ComissaoVendedor", cotacao.itens[i].ComissaoVendedor, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@DataComissao", cotacao.itens[i].DataComissao, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@idLote", cotacao.itens[i].IdLote, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@DescontoLiberado", cotacao.itens[i].DescontoLiberado, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@SEQ", cotacao.itens[i].Seq, null));

                            //@percIpi, @ValorIpi, @SituacaoTributariaDesc, @ValorIcms, @BaseCalcValorIcms, @ValorIcmsSubst
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@percIpi", cotacao.itens[i].PercIpi, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@ValorIpi", cotacao.itens[i].ValorIpi, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@SituacaoTributariaDesc", cotacao.itens[i].SituacaoTributariaDesc, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@ValorIcms", cotacao.itens[i].ValorIcms, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@BaseCalcValorIcms", cotacao.itens[i].BaseCalcValorIcms, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@ValorIcmsSubst", cotacao.itens[i].ValorIcmsSubst, null));

                            //@BaseCalcValorIcmsSubst, @valortotalnotafiscal, @dadosencomenda, @previsaoentrega, @dataconclusaoservico, @dataentrega
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@BaseCalcValorIcmsSubst", cotacao.itens[i].BaseCalcValorIcmsSubst, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@valortotalnotafiscal", cotacao.itens[i].ValorTotalNotaFiscal, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@dadosencomenda", cotacao.itens[i].DadosEncomenda, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@previsaoentrega", cotacao.itens[i].PrevisaoDeEntrega, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@dataconclusaoservico", cotacao.itens[i].DataConclusaoServico, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@dataentrega", cotacao.itens[i].DataEntrega, null));

                            //@obsentrega, @situacaoencomenda, @obsconclusao, @encomenda_encerrada, @encomenda_dataencerrada, @em_execucao_encomenda
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@obsentrega", cotacao.itens[i].ObsEntrega, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@situacaoencomenda", cotacao.itens[i].SituacaoEncomenda, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@obsconclusao", cotacao.itens[i].ObsConclusao, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@encomenda_encerrada", cotacao.itens[i].EncomendaEncerrada, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@encomenda_dataencerrada", cotacao.itens[i].EncomendaDataEncerrada, null));
                            cmd.Parameters.AddWithValue("@em_execucao_encomenda", cotacao.itens[i].EmExecucaoEncomenda);

                            //@data_inicio_execucao, @FRACIONADO, @situacao_def_consig, @codigo_recebimento_revenda, @codigorevenda "
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@data_inicio_execucao", cotacao.itens[i].DataInicioExecucao, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@FRACIONADO", cotacao.itens[i].Fracionado, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@situacao_def_consig", cotacao.itens[i].SituacaoDefConsig, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@codigo_recebimento_revenda", cotacao.itens[i].CodigoRecebimentoRevenda, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@codigorevenda", cotacao.itens[i].CodigoRevenda, null));


                            //@codigoprocessamento, @ncm, @codvendedor, @sequenciaoriginal, @codigooriginal, @qtconf "
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@codigoprocessamento", cotacao.itens[i].CodigoProcessamento,null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@ncm", cotacao.itens[i].NCM,null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@codvendedor", cotacao.itens[i].CodVendedor, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@sequenciaoriginal", cotacao.itens[i].SequenciaOriginal, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@codigooriginal", cotacao.itens[i].CodigoOriginal, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@qtconf", cotacao.itens[i].QtConf, null));

                            //@naosinc, @precoatacadoitem, @descatacadoitem
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@naosinc", cotacao.itens[i].NaoSinc, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@precoatacadoitem", cotacao.itens[i].PrecoAtacadoItem, null));
                            cmd.Parameters.Add(ValidParameter.CreateParameter("@descatacadoitem", cotacao.itens[i].DescAtacadoItem, null));

                            cmd.ExecuteNonQuery();

                            cmd.Parameters.Clear();
                            #endregion
                        }
                    }

                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (InvalidOperationException exRollback)
                    {
                        throw exRollback;
                    }

                    catch (Exception generalException)
                    {
                        throw generalException;
                    }
                }
            }
            
        }

        public void InserirItemNaCotacao(ItemOrc item)
        {
            using (_connection = new SqlConnection(_con))
            {
                _connection.Open();
                SqlCommand cmd = (SqlCommand)_connection.CreateCommand();
                cmd.Connection = (SqlConnection)_connection;
                SqlTransaction transaction = (SqlTransaction)_connection.BeginTransaction();
                cmd.Transaction = transaction;

                try
                {
                    #region Monta a DML INSERT INTO itensOrc
                    cmd.CommandText = "INSERT INTO ItensOrc("
                        + "idunico, codigo, codprod, descricao, codmov, nomemov "
                + ",tipomov, data, quantidade, preco, total, codcor "
                + ",codtam, ref, descp, descv, tipodesc, precooriginal "
                + ",nomecor, icms, unidade, loja, vendedor, nomevendedor "
                + ",docgerado, st, aliquota, codaliquota, descrateio, IMPRESSO "
                + ",liquido, custo, nome_impresso, tabela, permitidodesconto, custo_av "
                + ",custo_ap, lucro_av, lucro_ap, perc_desc_autorizado, comissao, Condicional_Marcado_Exclusao "
                + ",estoqueOK, ComissaoVendedor, DataComissao, idLote, DescontoLiberado, SEQ "
                + ",percIpi, ValorIpi, SituacaoTributariaDesc, ValorIcms, BaseCalcValorIcms, ValorIcmsSubst "
                + ",BaseCalcValorIcmsSubst, valortotalnotafiscal, dadosencomenda, previsaoentrega, dataconclusaoservico, dataentrega "
                + ",obsentrega, situacaoencomenda, obsconclusao, encomenda_encerrada, encomenda_dataencerrada, em_execucao_encomenda "
                + ",data_inicio_execucao, FRACIONADO, situacao_def_consig, codigo_recebimento_revenda, codigorevenda "
                + ",codigoprocessamento, ncm, codvendedor, sequenciaoriginal, codigooriginal, qtconf "
                + ",naosinc, precoatacadoitem, descatacadoitem) VALUES("

                + " @idunico, @codigo, @codprod, @descricao, @codmov, @nomemov "
                + ",@tipomov, @data, @quantidade, @preco, @total, @codcor "
                + ",@codtam, @ref, @descp, @descv, @tipodesc, @precooriginal "
                + ",@nomecor, @icms, @unidade, @loja, @vendedor, @nomevendedor "
                + ",@docgerado, @st, @aliquota, @codaliquota, @descrateio, @IMPRESSO "
                + ",@liquido, @custo, @nome_impresso, @tabela, @permitidodesconto, @custo_av "
                + ",@custo_ap, @lucro_av, @lucro_ap, @perc_desc_autorizado, @comissao, @Condicional_Marcado_Exclusao "
                + ",@estoqueOK, @ComissaoVendedor, @DataComissao, @idLote, @DescontoLiberado, @SEQ "
                + ",@percIpi, @ValorIpi, @SituacaoTributariaDesc, @ValorIcms, @BaseCalcValorIcms, @ValorIcmsSubst "
                + ",@BaseCalcValorIcmsSubst, @valortotalnotafiscal, @dadosencomenda, @previsaoentrega, @dataconclusaoservico, @dataentrega "
                + ",@obsentrega, @situacaoencomenda, @obsconclusao, @encomenda_encerrada, @encomenda_dataencerrada, @em_execucao_encomenda "
                + ",@data_inicio_execucao, @FRACIONADO, @situacao_def_consig, @codigo_recebimento_revenda, @codigorevenda "
                + ",@codigoprocessamento, @ncm, @codvendedor, @sequenciaoriginal, @codigooriginal, @qtconf "
                + ",@naosinc, @precoatacadoitem, @descatacadoitem) ";
                    #endregion

                    #region Define os parametros do comando Insert na ItensOrc
                    //@idunico, @codigo, @codprod, @descricao, @codmov, @nomemov
                    long idUnico = GerarId.GerarNovoCodigoTabela("itensorc", true);
                    cmd.Parameters.AddWithValue("idunico", idUnico);
                    cmd.Parameters.AddWithValue("@codigo", item.Codigo);
                    cmd.Parameters.AddWithValue("@codprod", item.CodProd);
                    cmd.Parameters.AddWithValue("@descricao", item.DescricaoProd);
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codmov", item.CodMov, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nomemov", item.NomeMov, null));

                    //@tipomov, @data, @quantidade, @preco, @total, @codcor
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@tipomov", item.TipoMov, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@data", item.Data, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@quantidade", item.Quantidade, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@preco", item.Preco, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@total", item.Total, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codcor", item.CodCor, null));

                    //@codtam, @ref, @descp, @descv, @tipodesc, @precooriginal 
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codtam", item.CodTam, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@ref", item.REF, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@descp", item.DescP, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@descv", item.DescV, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@tipodesc", item.TipoDesc, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@precooriginal", item.PrecoOriginal, null));

                    //@nomecor, @icms, @unidade, @loja, @vendedor, @nomevendedor
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nomecor", item.NomeCor, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@icms", item.Icms, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@unidade", item.UN, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@loja", item.LojaId, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@vendedor", item.VendedorId, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nomevendedor", item.NomeVendedor, null));

                    //@docgerado, @st, @aliquota, @codaliquota, @descrateio, @IMPRESSO
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@docgerado", item.DocGerado, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@st", item.st, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@aliquota", item.Aliquota, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codaliquota", item.CodAliquota, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@descrateio", item.DescRateio, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@IMPRESSO", item.IMPRESSO, null));

                    //@liquido, @custo, @nome_impresso, @tabela, @permitidodesconto, @custo_av
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@liquido", item.Liquido, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@custo", item.Custo, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@nome_impresso", item.NomeImpresso, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@tabela", item.Tabela, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@permitidodesconto", item.PermitidoDesconto, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@custo_av", item.CustoAV, null));

                    //@custo_ap, @lucro_av, @lucro_ap, @perc_desc_autorizado, @comissao, @Condicional_Marcado_Exclusao
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@custo_ap", item.CustoAP, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@lucro_av", item.LucroAV, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@lucro_ap", item.LucroAP, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@perc_desc_autorizado", item.PercDescAutorizado, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@comissao", item.Comissao, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@Condicional_Marcado_Exclusao", item.CondicionalMarcadoExclusao, null));

                    //@estoqueOK, @ComissaoVendedor, @DataComissao, @idLote, @DescontoLiberado, @SEQ
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@estoqueOK", item.EstoqueOK, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@ComissaoVendedor", item.ComissaoVendedor, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@DataComissao", item.DataComissao, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@idLote", item.IdLote, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@DescontoLiberado", item.DescontoLiberado, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@SEQ", item.Seq, null));

                    //@percIpi, @ValorIpi, @SituacaoTributariaDesc, @ValorIcms, @BaseCalcValorIcms, @ValorIcmsSubst
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@percIpi", item.PercIpi, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@ValorIpi", item.ValorIpi, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@SituacaoTributariaDesc", item.SituacaoTributariaDesc, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@ValorIcms", item.ValorIcms, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@BaseCalcValorIcms", item.BaseCalcValorIcms, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@ValorIcmsSubst", item.ValorIcmsSubst, null));

                    //@BaseCalcValorIcmsSubst, @valortotalnotafiscal, @dadosencomenda, @previsaoentrega, @dataconclusaoservico, @dataentrega
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@BaseCalcValorIcmsSubst", item.BaseCalcValorIcmsSubst, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@valortotalnotafiscal", item.ValorTotalNotaFiscal, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@dadosencomenda", item.DadosEncomenda, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@previsaoentrega", item.PrevisaoDeEntrega, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@dataconclusaoservico", item.DataConclusaoServico, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@dataentrega", item.DataEntrega, null));

                    //@obsentrega, @situacaoencomenda, @obsconclusao, @encomenda_encerrada, @encomenda_dataencerrada, @em_execucao_encomenda
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@obsentrega", item.ObsEntrega, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@situacaoencomenda", item.SituacaoEncomenda, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@obsconclusao", item.ObsConclusao, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@encomenda_encerrada", item.EncomendaEncerrada, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@encomenda_dataencerrada", item.EncomendaDataEncerrada, null));
                    cmd.Parameters.AddWithValue("@em_execucao_encomenda", item.EmExecucaoEncomenda);

                    //@data_inicio_execucao, @FRACIONADO, @situacao_def_consig, @codigo_recebimento_revenda, @codigorevenda "
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@data_inicio_execucao", item.DataInicioExecucao, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@FRACIONADO", item.Fracionado, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@situacao_def_consig", item.SituacaoDefConsig, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codigo_recebimento_revenda", item.CodigoRecebimentoRevenda, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codigorevenda", item.CodigoRevenda, null));


                    //@codigoprocessamento, @ncm, @codvendedor, @sequenciaoriginal, @codigooriginal, @qtconf "
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codigoprocessamento", item.CodigoProcessamento, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@ncm", item.NCM, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codvendedor", item.CodVendedor, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@sequenciaoriginal", item.SequenciaOriginal, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@codigooriginal", item.CodigoOriginal, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@qtconf", item.QtConf, null));

                    //@naosinc, @precoatacadoitem, @descatacadoitem
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@naosinc", item.NaoSinc, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@precoatacadoitem", item.PrecoAtacadoItem, null));
                    cmd.Parameters.Add(ValidParameter.CreateParameter("@descatacadoitem", item.DescAtacadoItem, null));
                    #endregion

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (InvalidOperationException exRollback)
                    {
                        throw exRollback;
                    }

                    catch (Exception generalException)
                    {
                        throw generalException;
                    }
                }
            }
        }
    }
}