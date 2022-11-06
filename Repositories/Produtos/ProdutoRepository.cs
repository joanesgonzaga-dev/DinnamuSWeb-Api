using DinnamuS_API.Repositories.Utils;
using DinnamuSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DinnamuSWebApi.Repositories.Produtos
{
    public class ProdutoRepository : IProdutoRepository
    {
        private IDbConnection _connection;
        private string _con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ProdutoRepository()
        {
            _connection = new SqlConnection(_con);
        }

        public List<Produto> Get()
        {
            SqlCommand command = new SqlCommand();

            string query = "SELECT cp.chaveunica,cp.codigo, cp.nome,cp.NOMEIMPRESSO, cp.codigonbn, cp.unidade, cp.ref, cp.codforn "
                + ",cp.datacad, cp.loja, cp.francionado, cp.bloqueado,"
                + "cp.foto,cp.icms,cp.codaliquota, cp.tributaçãoicms,cp.percentualdeicms, cp.classificaçãofiscal,"
                + "cp.origemdoproduto,cp.percentualipi, cp.tributaçãoipi, cp.margemdelucro, cp.feito, cp.fabricante,"
                + "cp.fiscal_convenio, cp.fiscal_stfaturamento, cp.fiscal_ReducaoBaseCalcIcms ,cp.ativado, cp.regimetributario,"
                + "cp.aliquota_icms_st, cp.perc_reducao_bc_st, cp.uf_icms_st, cp.modalidade_determina_bc_st, cp.perc_margem_valor_adic_st,"
                + "cp.vBCSTRet, cp.vICMSSTRet, cp.icms_sn_101_aliqaplic, cp.icms_sn_101_vlrcred, cp.situacaotributariaipi, cp.clEnq, "
                + "cp.cenq, cp.preco_decimais, cp.pis_cst,cp.cofins_cst, cp.cest, cp.tabelabaseatacado, cp.codgrade, "
                + "igp.chaveunica , igp.codigo, igp.tamanho, igp.codicorprod,igp.cor,igp.codbarraint,igp.codbarraforn,igp.precocompra,"
                + "igp.referencia,igp.precovenda,igp.estoqueinicial,igp.jafeito,igp.loja,igp.etiqueta,igp.qtetiqueta,igp.chavelojaorigem,"
                + "igp.ultimamodificacao "
                + ",ef.id, ef.codigoloja, ef.codigofilial, ef.codigoproduto, ef.estoque, ef.ultimamodificacao "
                + ",ipreco.chaveunica,ipreco.codigo,ipreco.codigoproduto,ipreco.precovenda "
                + ",t.chaveunica, t.loja,t.descricao,t.tabelabase "
                + "FROM cadproduto cp "
                + "LEFT OUTER JOIN itensgradeproduto igp "
                + "ON cp.chaveunica = igp.codigo "

                + "LEFT JOIN estoquefilial ef "
                + "ON igp.chaveunica = ef.codigoproduto "

                + "LEFT OUTER JOIN itenstabelapreco ipreco "
                + "ON igp.chaveunica = ipreco.codigoproduto "

                + "LEFT OUTER JOIN tabeladepreco t "
                + "ON ipreco.codigo = t.chaveunica "

                + "WHERE cp.nome IS NOT NULL";

            command.CommandText = query;
            command.Connection = (SqlConnection)_connection;
            _connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            Dictionary<long, Produto> produtos = new Dictionary<long, Produto>();
            Produto produto;
            //ItensGradeProduto item = null;

            while (reader.Read())
            {
                if (!produtos.ContainsKey(reader.GetInt64(0)))
                {
                    produto = new Produto();
                    produto.Chaveunica = (long)(Convert.IsDBNull(reader["chaveunica"]) ? 0 : reader["chaveunica"]);
                    produto.Codigo = (long)(Convert.IsDBNull(reader["codigo"]) ? 0 : reader["codigo"]);
                    produto.Nome = (string)((DBNull.Value == reader["nome"]) ? string.Empty : reader["nome"]);
                    produto.NomeImpresso = (string)((DBNull.Value == reader["nomeimpresso"]) ? string.Empty : reader["nomeimpresso"]);
                    produto.CodigoNbn = (string)((DBNull.Value == reader["codigonbn"]) ? string.Empty : reader["codigonbn"]);
                    produto.Unidade = (string)((DBNull.Value == reader["unidade"]) ? string.Empty : reader["unidade"]);
                    produto.Referencia = (string)((DBNull.Value == reader["ref"]) ? string.Empty : reader["ref"]);
                    produto.CodForn = (string)((DBNull.Value == reader["codforn"]) ? string.Empty : reader["codforn"]);
                    produto.DataCadastro = ((DBNull.Value == reader["datacad"]) ? string.Empty : reader.GetDateTime(8).ToShortDateString());
                    produto.Loja = (int)((DBNull.Value == reader["loja"]) ? 0 : reader["loja"]);
                    produto.Fracionado = (bool)((DBNull.Value == reader["francionado"]) ? false : reader["francionado"]);
                    produto.Bloqueado = (bool)((DBNull.Value == reader["bloqueado"]) ? false : reader["bloqueado"]);
                    //produto.Foto = (string)((DBNull.Value == reader[12]) ? string.Empty : reader.GetString(12));
                    produto.ICMS = (int)((DBNull.Value == reader["icms"]) ? 0 : reader["icms"]);
                    produto.CodAliquota = (string)((DBNull.Value == reader["codaliquota"]) ? string.Empty : reader["codaliquota"]);
                    produto.TributacaoICMS = (string)((DBNull.Value == reader["tributaçãoicms"]) ? string.Empty : reader["tributaçãoicms"]);
                    produto.PercentualDeICMS = (float)((DBNull.Value == reader["percentualdeicms"]) ? 0.0F : reader["percentualdeicms"]);
                    produto.ClassificacaoFiscal = (string)((DBNull.Value == reader["classificaçãofiscal"]) ? string.Empty : reader["classificaçãofiscal"]);
                    produto.OrigemDoProduto = (string)((DBNull.Value == reader["origemdoproduto"]) ? string.Empty : reader["origemdoproduto"]);
                    produto.PercentualIpi = (float)((DBNull.Value == reader["percentualipi"]) ? 0.0F : reader["percentualipi"]);
                    produto.TributacaoIpi = (int)((DBNull.Value == reader[20]) ? 0 : reader.GetInt16(20));
                    produto.MargemDelucro = (decimal)((DBNull.Value == reader["margemdelucro"]) ? 0.0M : reader["margemdelucro"]);
                    produto.Feito = (int)((DBNull.Value == reader["feito"]) ? -1 : reader["feito"]);
                    produto.FabricanteId = (int)((DBNull.Value == reader["fabricante"]) ? 0 : reader["fabricante"]);
                    produto.FiscalConvenio = (string)((DBNull.Value == reader["fiscal_convenio"]) ? string.Empty : reader["fiscal_convenio"]);
                    produto.FiscalStFaturamento = (string)((DBNull.Value == reader["fiscal_stfaturamento"]) ? string.Empty : reader["fiscal_stfaturamento"]);
                    produto.FiscalReducaoBaseCalcICMS = (decimal)((DBNull.Value == reader["fiscal_ReducaoBaseCalcIcms"]) ? 0.0M : reader["fiscal_ReducaoBaseCalcIcms"]);
                    produto.Ativado = (bool)((DBNull.Value == reader["ativado"]) ? false : reader["ativado"]);
                    produto.RegimeTributario = (int)((DBNull.Value == reader["regimetributario"]) ? -1 : reader["regimetributario"]);
                    produto.AliquotaIcmsSt = (double)((DBNull.Value == reader["aliquota_icms_st"]) ? 0.0 : reader["aliquota_icms_st"]);
                    produto.PercReducaoBcSt = (double)((DBNull.Value == reader["perc_reducao_bc_st"]) ? 0.0 : reader["perc_reducao_bc_st"]);
                    produto.UfIcmsSt = (int)((DBNull.Value == reader["uf_icms_st"]) ? -1 : reader["uf_icms_st"]);
                    produto.ModalidadeDeterminaBcSt = (int)((DBNull.Value == reader["modalidade_determina_bc_st"]) ? -1 : reader["modalidade_determina_bc_st"]);
                    produto.PercMargemValorAdicSt = (double)((DBNull.Value == reader["perc_margem_valor_adic_st"]) ? 0.0 : reader["perc_margem_valor_adic_st"]);
                    produto.vBcStRet = (double)((DBNull.Value == reader["vBCSTRet"]) ? 0.0 : reader["vBCSTRet"]);
                    produto.vIcmsStRet = (double)((DBNull.Value == reader["vIcmsStRet"]) ? 0.0 : reader["vIcmsStRet"]);
                    produto.IcmsSn101AliqAplic = (double)((DBNull.Value == reader["icms_sn_101_aliqaplic"]) ? 0.0 : reader["icms_sn_101_aliqaplic"]);
                    produto.IcmsSn101VlrCred = (double)((DBNull.Value == reader["icms_sn_101_vlrcred"]) ? 0.0 : reader["icms_sn_101_vlrcred"]);
                    produto.SituacaoTributariaIpi = (string)((DBNull.Value == reader["situacaotributariaipi"]) ? string.Empty : reader["situacaotributariaipi"]);
                    produto.ClEnq = (string)((DBNull.Value == reader["clEnq"]) ? string.Empty : reader["clEnq"]);
                    produto.CEnq = (string)((DBNull.Value == reader["cEnq"]) ? string.Empty : reader["cEnq"]);
                    produto.PrecoDecimais = (int)((DBNull.Value == reader["preco_decimais"]) ? 0 : reader["preco_decimais"]);

                    produto.PisCst = (int)((DBNull.Value == reader["pis_cst"]) ? -1 : reader["pis_cst"]);
                    produto.CofinsCst = (int)((DBNull.Value == reader["cofins_cst"]) ? -1 : reader["cofins_cst"]);
                    produto.Cest = (string)((DBNull.Value == reader["cest"]) ? string.Empty : reader["cest"]);
                    produto.TabelaBaseAtacado = (int)((DBNull.Value == reader["tabelabaseatacado"]) ? 0 : reader["tabelabaseatacado"]);
                    produto.CodGrade = (string)((DBNull.Value == reader["codgrade"]) ? string.Empty : reader["codgrade"]);

                    produto.ItensGrade = new List<ItensGradeProduto>();

                    produtos.Add(produto.Chaveunica, produto);
                }
                else
                {
                    produto = produtos[reader.GetInt64(0)];
                }

                ItensGradeProduto item = new ItensGradeProduto()
                {
                    ChaveUnica = (int)(Convert.IsDBNull(reader[47]) ? 0 : reader.GetInt32(47)),
                    CodigoProduto = (DBNull.Value == reader[48]) ? 0 : reader.GetInt64(48),
                    Tamanho = (string)((DBNull.Value == reader[49]) ? string.Empty : reader.GetString(49)),
                    CodiCorProd = (string)((DBNull.Value == reader[50]) ? string.Empty : reader.GetString(50)),
                    Cor = (string)((DBNull.Value == reader[51]) ? string.Empty : reader.GetString(51)),
                    CodBarraInt = (string)((DBNull.Value == reader[52]) ? string.Empty : reader.GetString(52)),
                    CodBarraForn = (string)((DBNull.Value == reader[53]) ? string.Empty : reader.GetString(53)),
                    PrecoCompra = (decimal)((DBNull.Value == reader[54]) ? 0.0M : reader.GetDecimal(54)),
                    Referencia = (string)((DBNull.Value == reader[55]) ? string.Empty : reader.GetString(55)),
                    PrecoVenda = (decimal)((DBNull.Value == reader[56]) ? 0.0M : reader.GetDecimal(56)),
                    EstoqueInicial = (decimal)((DBNull.Value == reader[57]) ? 0.0M : reader.GetDecimal(57)),
                    JaFeito = (string)((DBNull.Value == reader[58]) ? string.Empty : reader.GetString(58)),
                    Loja = (int)((DBNull.Value == reader[59]) ? -1 : reader.GetInt32(59)),
                    Etiqueta = (string)((DBNull.Value == reader[60]) ? string.Empty : reader.GetString(60)),
                    QtEtiqueta = (int)((DBNull.Value == reader[61]) ? -1 : reader.GetInt32(61)),
                    ChaveLojaOrigem = (int)((DBNull.Value == reader[62]) ? -1 : reader.GetInt32(62)),
                    UltimaModificacao = (DateTime)((DBNull.Value == reader[63]) ? DateTime.Now : reader.GetDateTime(63)),
                    PrecosDeVenda = new List<ItemTabelaDePreco>()

                };

                if (!produto.ItensGrade.Contains(item))
                {
                    produto.ItensGrade.Add(item);
                }


                EstoqueFilial estoqueFilial = new EstoqueFilial()
                {
                    Id = (long)((DBNull.Value == reader[64]) ? 0L : reader.GetInt64(64)),
                    CodigoLoja = (int)((DBNull.Value == reader[65]) ? -1 : reader.GetInt32(65)),
                    CodigoFilial = (int)((DBNull.Value == reader[66]) ? -1 : reader.GetInt32(66)),
                    CodigoProduto = (int)((DBNull.Value == reader[67]) ? -1 : reader.GetInt32(67)),
                    Estoque = (decimal)((DBNull.Value == reader[68]) ? 0.0M : reader.GetDecimal(68)),
                    UltimaModificacao = (DateTime)((DBNull.Value == reader[69]) ? DateTime.Now : reader.GetDateTime(69)),

                };

                item.EstoqueNasFiliais = (item.EstoqueNasFiliais == null) ? new List<EstoqueFilial>() : item.EstoqueNasFiliais;
                item.EstoqueNasFiliais.Add(estoqueFilial);


                ItemTabelaDePreco itemTabelaPreco = new ItemTabelaDePreco()
                {
                    ChaveUnica = (int)((DBNull.Value == reader[70]) ? 0 : reader.GetInt32(70)),
                    CodigoTabela = (int)((DBNull.Value == reader[71]) ? 0 : reader.GetInt32(71)),
                    CodigoProduto = (int)((DBNull.Value == reader[72]) ? 0 : reader.GetInt32(72)),
                    Preco = (decimal)((DBNull.Value == reader[73]) ? 0.0M : reader.GetDecimal(73)),
                    NomeTabela = ((DBNull.Value == reader[76]) ? string.Empty : reader.GetString(76)).ToString()
                };

                produto.ItensGrade.Find(i => i.ChaveUnica == item.ChaveUnica).PrecosDeVenda = (produto.ItensGrade.Find(i => i.ChaveUnica == i.ChaveUnica).PrecosDeVenda == null ? new List<ItemTabelaDePreco>() : produto.ItensGrade.Find(i => i.ChaveUnica == item.ChaveUnica).PrecosDeVenda);
                if (!produto.ItensGrade.Find(i => i.ChaveUnica == item.ChaveUnica).PrecosDeVenda.Contains(itemTabelaPreco))
                {
                    produto.ItensGrade.Find(i => i.ChaveUnica == item.ChaveUnica).PrecosDeVenda.Add(itemTabelaPreco);
                }

            }

            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }

            return produtos.Values.ToList();

        }

        public Produto Get(long codigo)
        {

            using (_connection = new SqlConnection(_con))
            {
                _connection.Open();
                SqlCommand command = (SqlCommand)_connection.CreateCommand();
                command.Connection = (SqlConnection)_connection;

                #region Monta a string de consulta: DadosOrc inner join ItensOrc
                string query = "SELECT cp.chaveunica,cp.codigo, cp.nome,cp.NOMEIMPRESSO, cp.codigonbn, cp.unidade, cp.ref, cp.codforn, cp.datacad, cp.loja, cp.francionado, cp.bloqueado,"
                    + "cp.foto,cp.icms,cp.codaliquota, cp.tributaçãoicms,cp.percentualdeicms, cp.classificaçãofiscal,"
                    + "cp.origemdoproduto,cp.percentualipi, cp.tributaçãoipi, cp.margemdelucro, cp.feito, cp.fabricante,"
                    + "cp.fiscal_convenio, cp.fiscal_stfaturamento, cp.fiscal_ReducaoBaseCalcIcms ,cp.ativado, cp.regimetributario,"
                    + "cp.aliquota_icms_st, cp.perc_reducao_bc_st, cp.uf_icms_st, cp.modalidade_determina_bc_st, cp.perc_margem_valor_adic_st,"
                    + "cp.vBCSTRet, cp.vICMSSTRet, cp.icms_sn_101_aliqaplic, cp.icms_sn_101_vlrcred, cp.situacaotributariaipi, cp.clEnq, "
                    + "cp.cenq, cp.preco_decimais, cp.pis_cst,cp.cofins_cst, cp.cest, cp.tabelabaseatacado, cp.codgrade, "
                    + "igp.chaveunica , igp.codigo, igp.tamanho, igp.codicorprod,igp.cor,igp.codbarraint,igp.codbarraforn,igp.precocompra,"
                    + "igp.referencia,igp.precovenda,igp.estoqueinicial,igp.jafeito,igp.loja,igp.etiqueta,igp.qtetiqueta,igp.chavelojaorigem,"
                    + "igp.ultimamodificacao "
                    + ",ef.id, ef.codigoloja, ef.codigofilial, ef.codigoproduto, ef.estoque, ef.ultimamodificacao "
                    + ",ipreco.chaveunica,ipreco.codigo,ipreco.codigoproduto,ipreco.precovenda "
                    + ",t.chaveunica, t.loja,t.descricao,t.tabelabase "
                    + "FROM cadproduto cp "
                    + "LEFT OUTER JOIN itensgradeproduto igp "
                    + "ON cp.chaveunica = igp.codigo "

                    + "LEFT JOIN estoquefilial ef "
                    + "ON igp.chaveunica = ef.codigoproduto "

                    + "LEFT OUTER JOIN itenstabelapreco ipreco "
                    + "ON igp.chaveunica = ipreco.codigoproduto "

                    + "LEFT OUTER JOIN tabeladepreco t "
                    + "ON ipreco.codigo = t.chaveunica "

                    + "WHERE cp.nome IS NOT NULL "
                    + " AND cp.codigo=@codigo ";
                #endregion

                command.CommandText = query;
                command.Parameters.AddWithValue("@codigo", codigo);
                SqlDataReader reader = command.ExecuteReader();

                #region Itera no retorno da consulta e monta a lista de Produtos (cadprod)
                Dictionary<long, Produto> produtos = new Dictionary<long, Produto>();
                while (reader.Read())
                {
                    Produto produto = new Produto();

                    if (!produtos.ContainsKey(reader.GetInt64(0)))
                    {
                        produto.Chaveunica = reader.GetInt64(0);
                        produto.Codigo = (long)((DBNull.Value == reader[1]) ? 0 : reader.GetInt64(1));
                        produto.Nome = (string)((DBNull.Value == reader[2]) ? string.Empty : reader.GetString(2));
                        produto.NomeImpresso = (string)((DBNull.Value == reader[3]) ? string.Empty : reader.GetString(3));
                        produto.CodigoNbn = (string)((DBNull.Value == reader[4]) ? string.Empty : reader.GetString(4));
                        produto.Unidade = (string)((DBNull.Value == reader[5]) ? string.Empty : reader.GetString(5));
                        produto.Referencia = (string)((DBNull.Value == reader[6]) ? string.Empty : reader.GetString(6));
                        produto.CodForn = (string)((DBNull.Value == reader[7]) ? string.Empty : reader.GetString(7));
                        produto.DataCadastro = ((DBNull.Value == reader[8]) ? "01/01/1900" : reader.GetDateTime(8).ToShortDateString());
                        produto.Loja = (int)((DBNull.Value == reader[9]) ? 0 : reader.GetInt32(9));
                        produto.Fracionado = (bool)((DBNull.Value == reader[10]) ? false : reader.GetBoolean(10));
                        produto.Bloqueado = (bool)((DBNull.Value == reader[11]) ? false : reader.GetBoolean(11));
                        //produto.Foto = (string)((DBNull.Value == reader[12]) ? string.Empty : reader.GetString(12));
                        produto.ICMS = (int)((DBNull.Value == reader[13]) ? 0 : reader.GetInt32(13));
                        produto.CodAliquota = (string)((DBNull.Value == reader[14]) ? string.Empty : reader.GetString(14));
                        produto.TributacaoICMS = (string)((DBNull.Value == reader[15]) ? string.Empty : reader.GetString(15));
                        produto.PercentualDeICMS = (float)((DBNull.Value == reader[16]) ? 0.0 : reader.GetFloat(16));
                        produto.ClassificacaoFiscal = (string)((DBNull.Value == reader[17]) ? string.Empty : reader.GetString(17));
                        produto.OrigemDoProduto = (string)((DBNull.Value == reader[18]) ? string.Empty : reader.GetString(18));
                        produto.PercentualIpi = (float)((DBNull.Value == reader[19]) ? 0.0F : reader.GetFloat(19));
                        produto.TributacaoIpi = (int)((DBNull.Value == reader[20]) ? 0 : reader.GetInt16(20));
                        produto.MargemDelucro = (decimal)((DBNull.Value == reader[21]) ? 0.0M : reader.GetDecimal(21));
                        produto.Feito = (int)((DBNull.Value == reader[22]) ? -1 : reader.GetInt32(22));
                        produto.FabricanteId = (int)((DBNull.Value == reader[23]) ? -1 : reader.GetInt32(23));
                        produto.FiscalConvenio = (string)((DBNull.Value == reader[24]) ? string.Empty : reader.GetString(24));
                        produto.FiscalStFaturamento = (string)((DBNull.Value == reader[25]) ? string.Empty : reader.GetString(25));
                        produto.FiscalReducaoBaseCalcICMS = (decimal)((DBNull.Value == reader[26]) ? 0.0M : reader.GetDecimal(26));
                        produto.Ativado = (bool)((DBNull.Value == reader[27]) ? false : reader.GetBoolean(27));
                        produto.RegimeTributario = (int)((DBNull.Value == reader[28]) ? -1 : reader.GetInt32(28));
                        produto.AliquotaIcmsSt = (double)((DBNull.Value == reader[29]) ? 0.0 : reader.GetDouble(29));
                        produto.PercReducaoBcSt = (double)((DBNull.Value == reader[30]) ? 0.0 : reader.GetDouble(30));
                        produto.UfIcmsSt = (int)((DBNull.Value == reader[31]) ? -1 : reader.GetInt32(31));
                        produto.ModalidadeDeterminaBcSt = (int)((DBNull.Value == reader[32]) ? -1 : reader.GetInt32(32));
                        produto.PercMargemValorAdicSt = (double)((DBNull.Value == reader[33]) ? 0.0 : reader.GetDouble(33));
                        produto.vBcStRet = (double)((DBNull.Value == reader[34]) ? 0.0 : reader.GetDouble(34));
                        produto.vIcmsStRet = (double)((DBNull.Value == reader[35]) ? 0.0 : reader.GetDouble(35));
                        produto.IcmsSn101AliqAplic = (double)((DBNull.Value == reader[36]) ? 0.0 : reader.GetDouble(36));
                        produto.IcmsSn101VlrCred = (double)((DBNull.Value == reader[37]) ? 0.0 : reader.GetDouble(37));
                        produto.SituacaoTributariaIpi = (string)((DBNull.Value == reader[38]) ? string.Empty : reader.GetString(38));
                        produto.ClEnq = (string)((DBNull.Value == reader[39]) ? string.Empty : reader.GetString(39));
                        produto.CEnq = (string)((DBNull.Value == reader[40]) ? string.Empty : reader.GetString(40));
                        produto.PrecoDecimais = (int)((DBNull.Value == reader[41]) ? -1 : reader.GetInt32(41));

                        produto.PisCst = (int)((DBNull.Value == reader[42]) ? -1 : reader.GetInt32(42));
                        produto.CofinsCst = (int)((DBNull.Value == reader[43]) ? -1 : reader.GetInt32(43));
                        produto.Cest = (string)((DBNull.Value == reader[44]) ? string.Empty : reader.GetString(44));
                        produto.TabelaBaseAtacado = (int)((DBNull.Value == reader[45]) ? -1 : reader.GetInt32(45));
                        produto.CodGrade = (string)((DBNull.Value == reader[46]) ? string.Empty : reader.GetString(46));
                        produto.ItensGrade = new List<ItensGradeProduto>();

                        produtos.Add(produto.Chaveunica, produto);
                    }
                    else
                    {
                        produto = produtos[reader.GetInt64(0)];
                    }

                    ItensGradeProduto item = new ItensGradeProduto()
                    {
                        ChaveUnica = (DBNull.Value == reader[47]) ? 0 : reader.GetInt32(47),
                        CodigoProduto = (DBNull.Value == reader[48]) ? 0 : reader.GetInt64(48),
                        Tamanho = (string)((DBNull.Value == reader[49]) ? string.Empty : reader.GetString(49)),
                        CodiCorProd = (string)((DBNull.Value == reader[50]) ? string.Empty : reader.GetString(50)),
                        Cor = (string)((DBNull.Value == reader[51]) ? string.Empty : reader.GetString(51)),
                        CodBarraInt = (string)((DBNull.Value == reader[52]) ? string.Empty : reader.GetString(52)),
                        CodBarraForn = (string)((DBNull.Value == reader[53]) ? string.Empty : reader.GetString(53)),
                        PrecoCompra = (decimal)((DBNull.Value == reader[54]) ? 0.0M : reader.GetDecimal(54)),
                        Referencia = (string)((DBNull.Value == reader[55]) ? string.Empty : reader.GetString(55)),
                        PrecoVenda = (decimal)((DBNull.Value == reader[56]) ? 0.0M : reader.GetDecimal(56)),
                        EstoqueInicial = (decimal)((DBNull.Value == reader[57]) ? 0.0M : reader.GetDecimal(57)),
                        JaFeito = (string)((DBNull.Value == reader[58]) ? string.Empty : reader.GetString(58)),
                        Loja = (int)((DBNull.Value == reader[59]) ? -1 : reader.GetInt32(59)),
                        Etiqueta = (string)((DBNull.Value == reader[60]) ? string.Empty : reader.GetString(60)),
                        QtEtiqueta = (int)((DBNull.Value == reader[61]) ? -1 : reader.GetInt32(61)),
                        ChaveLojaOrigem = (int)((DBNull.Value == reader[62]) ? -1 : reader.GetInt32(62)),
                        UltimaModificacao = (DateTime)((DBNull.Value == reader[63]) ? DateTime.Now : reader.GetDateTime(63)),
                        PrecosDeVenda = new List<ItemTabelaDePreco>()

                    };

                    if (!produto.ItensGrade.Contains(item))
                    {
                        produto.ItensGrade.Add(item);
                    }

                    EstoqueFilial estoqueFilial = new EstoqueFilial()
                    {
                        Id = (long)((DBNull.Value == reader[64]) ? 0L : reader.GetInt64(64)),
                        CodigoLoja = (int)((DBNull.Value == reader[65]) ? -1 : reader.GetInt32(65)),
                        CodigoFilial = (int)((DBNull.Value == reader[66]) ? -1 : reader.GetInt32(66)),
                        CodigoProduto = (int)((DBNull.Value == reader[67]) ? -1 : reader.GetInt32(67)),
                        Estoque = (decimal)((DBNull.Value == reader[68]) ? 0.0M : reader.GetDecimal(68))
                    };
                    item.EstoqueNasFiliais = (item.EstoqueNasFiliais == null) ? new List<EstoqueFilial>() : item.EstoqueNasFiliais;
                    item.EstoqueNasFiliais.Add(estoqueFilial);

                    ItemTabelaDePreco itemTabelaPreco = new ItemTabelaDePreco()
                    {
                        ChaveUnica = (int)((DBNull.Value == reader[70]) ? 0 : reader.GetInt32(70)),
                        CodigoTabela = (int)((DBNull.Value == reader[71]) ? 0 : reader.GetInt32(71)),
                        CodigoProduto = (int)((DBNull.Value == reader[72]) ? 0 : reader.GetInt32(72)),
                        Preco = (decimal)((DBNull.Value == reader[73]) ? 0.0M : reader.GetDecimal(73)),
                        NomeTabela = ((DBNull.Value == reader[76]) ? string.Empty : reader.GetString(76)).ToString()
                    };

                    produto.ItensGrade.Find(i => i.ChaveUnica == item.ChaveUnica).PrecosDeVenda = (produto.ItensGrade.Find(i => i.ChaveUnica == i.ChaveUnica).PrecosDeVenda == null ? new List<ItemTabelaDePreco>() : produto.ItensGrade.Find(i => i.ChaveUnica == item.ChaveUnica).PrecosDeVenda);
                    if (!produto.ItensGrade.Find(i => i.ChaveUnica == item.ChaveUnica).PrecosDeVenda.Contains(itemTabelaPreco))
                    {
                        produto.ItensGrade.Find(i => i.ChaveUnica == item.ChaveUnica).PrecosDeVenda.Add(itemTabelaPreco);
                    }
                }
                
                return produtos.Values.ToList().FirstOrDefault();
                #endregion
            }
        }

        public void Insert(Produto produto)
        {
            #region A Cerca do nao uso de transaction
            //NAO FOI POSSIVEL USAR TRANSACTION NESTE MÉTODO DEVIDO A NECESSIDADE DE RETORNAR A
            //PRIMARY KEY DO PRODUTO PARA INSERÇÃO DE SEUS REGISTROS FILHOS, O QUE NÃO É POSSIVEL
            //ANTES DO TRANSACTION.COMMIT()
            #endregion

            using (_connection = new SqlConnection(_con))
            {
                _connection.Open();
                SqlCommand cmd = (SqlCommand)_connection.CreateCommand();
                cmd.Connection = (SqlConnection)_connection;

                cmd.CommandText = "INSERT INTO cadproduto(codigo, nome,NOMEIMPRESSO,codigonbn, unidade, ref,codforn, datacad,loja, francionado, bloqueado,"
                            + "foto,icms,codaliquota, tributaçãoicms,percentualdeicms, classificaçãofiscal,"
                            + "origemdoproduto,percentualipi,tributaçãoipi,margemdelucro,feito,fabricante,"
                            + "fiscal_convenio, fiscal_stfaturamento, fiscal_ReducaoBaseCalcIcms,ativado,regimetributario,"
                            + "aliquota_icms_st,perc_reducao_bc_st, uf_icms_st,modalidade_determina_bc_st,perc_margem_valor_adic_st,"
                            + "vBCSTRet,vICMSSTRet,icms_sn_101_aliqaplic,icms_sn_101_vlrcred,situacaotributariaipi,clEnq,"
                            + "cenq,preco_decimais,pis_cst,cp.cofins_cst,cest, tabelabaseatacado) "
                            + "VALUES("
                            + "@codigo, @nome,@NOMEIMPRESSO,@codigonbn, @unidade, @ref,@codforn, @datacad,@loja, @francionado, @bloqueado,"
                            + "@foto,@icms,@codaliquota, @tributacaoicms,@percentualdeicms, @classificacaofiscal,"
                            + "@origemdoproduto,@percentualipi,@tributacaoipi,@margemdelucro,@feito,@fabricante,"
                            + "@fiscal_convenio, @fiscal_stfaturamento, @fiscal_ReducaoBaseCalcIcms,@ativado,@regimetributario,"
                            + "@aliquota_icms_st,@perc_reducao_bc_st, @uf_icms_st,@modalidade_determina_bc_st,@perc_margem_valor_adic_st,"
                            + "@vBCSTRet,@vICMSSTRet,@icms_sn_101_aliqaplic,@icms_sn_101_vlrcred,@situacaotributariaipi,@clEnq,"
                            + "@cenq,@preco_decimais,@pis_cst,@cofins_cst,@cest, @tabelabaseatacado)";

                long id = GerarId.GerarNovoCodigoTabela("cadproduto", false);

                produto.Codigo = id;
                cmd.Parameters.AddWithValue("@codigo", produto.Codigo);
                cmd.Parameters.AddWithValue("@nome", produto.Nome);
                cmd.Parameters.AddWithValue("@NOMEIMPRESSO", produto.NomeImpresso ?? string.Empty);
                cmd.Parameters.AddWithValue("@codigonbn", produto.CodigoNbn ?? string.Empty);
                cmd.Parameters.AddWithValue("@unidade", produto.Unidade ?? string.Empty);
                cmd.Parameters.AddWithValue("@ref", produto.Referencia ?? string.Empty);
                cmd.Parameters.AddWithValue("@codforn", produto.CodForn ?? string.Empty);
                cmd.Parameters.AddWithValue("@datacad", produto.DataCadastro ?? DateTime.Now.ToLongDateString());
                cmd.Parameters.AddWithValue("@loja", (produto.Loja.Equals(null)) ? 0 : produto.Loja);
                cmd.Parameters.AddWithValue("@francionado", (produto.Fracionado.Equals(null)) ? false : produto.Fracionado);
                cmd.Parameters.AddWithValue("@bloqueado", (produto.Bloqueado.Equals(null)) ? false : produto.Bloqueado);
                cmd.Parameters.AddWithValue("@foto", produto.Foto ?? new Byte[0]);
                cmd.Parameters.AddWithValue("@icms", (produto.ICMS.Equals(null)) ? 0 : produto.ICMS);
                cmd.Parameters.AddWithValue("@codaliquota", produto.CodAliquota ?? string.Empty);
                cmd.Parameters.AddWithValue("@tributacaoicms", produto.TributacaoICMS ?? string.Empty);
                cmd.Parameters.AddWithValue("@percentualdeicms", (produto.PercentualDeICMS.Equals(null)) ? 0.0F : produto.PercentualDeICMS);
                cmd.Parameters.AddWithValue("@classificacaofiscal", produto.ClassificacaoFiscal ?? string.Empty);
                cmd.Parameters.AddWithValue("@origemdoproduto", produto.OrigemDoProduto ?? string.Empty);
                cmd.Parameters.AddWithValue("@percentualipi", (produto.PercentualIpi.Equals(null)) ? 0.0F : produto.PercentualIpi);
                cmd.Parameters.AddWithValue("@tributacaoipi", (produto.TributacaoIpi.Equals(null)) ? 0 : produto.TributacaoIpi);
                cmd.Parameters.AddWithValue("@margemdelucro", (produto.MargemDelucro.Equals(null)) ? 0.0M : produto.MargemDelucro);
                cmd.Parameters.AddWithValue("@feito", (produto.Feito.Equals(null)) ? -1 : produto.Feito);
                cmd.Parameters.AddWithValue("@fabricante", (produto.FabricanteId.Equals(null)) ? 0 : produto.FabricanteId);
                cmd.Parameters.AddWithValue("@fiscal_convenio", produto.FiscalConvenio ?? string.Empty);
                cmd.Parameters.AddWithValue("@fiscal_stfaturamento", (produto.FiscalStFaturamento == null) ? string.Empty : produto.FiscalStFaturamento);
                cmd.Parameters.AddWithValue("@Fiscal_ReducaoBaseCalcIcms", produto.FiscalReducaoBaseCalcICMS.Equals(null) ? 0.0M : produto.FiscalReducaoBaseCalcICMS);
                cmd.Parameters.AddWithValue("@ativado", (produto.Ativado.Equals(null)) ? false : produto.Ativado);
                cmd.Parameters.AddWithValue("@regimetributario", (produto.RegimeTributario.Equals(null)) ? 0 : produto.RegimeTributario);
                cmd.Parameters.AddWithValue("@aliquota_icms_st", (produto.AliquotaIcmsSt.Equals(null)) ? 0.0D : produto.AliquotaIcmsSt);
                cmd.Parameters.AddWithValue("@perc_reducao_bc_st", (produto.PercReducaoBcSt.Equals(null)) ? 0.0D : produto.PercReducaoBcSt);
                cmd.Parameters.AddWithValue("@uf_icms_st", (produto.UfIcmsSt.Equals(null)) ? -1 : produto.UfIcmsSt);
                cmd.Parameters.AddWithValue("@modalidade_determina_bc_st", produto.ModalidadeDeterminaBcSt.Equals(null) ? 0 : produto.ModalidadeDeterminaBcSt);
                cmd.Parameters.AddWithValue("@perc_margem_valor_adic_st", produto.PercMargemValorAdicSt.Equals(null) ? 0.0D : produto.PercMargemValorAdicSt);
                cmd.Parameters.AddWithValue("@vBCSTRet", (produto.vBcStRet.Equals(null)) ? 0.0D : produto.vBcStRet);
                cmd.Parameters.AddWithValue("@vICMSSTRet", (produto.vIcmsStRet.Equals(null)) ? 0.0D : produto.vIcmsStRet);
                cmd.Parameters.AddWithValue("@icms_sn_101_aliqaplic", (produto.IcmsSn101AliqAplic.Equals(null)) ? 0.0D : produto.IcmsSn101AliqAplic);
                cmd.Parameters.AddWithValue("@icms_sn_101_vlrcred", (produto.IcmsSn101VlrCred.Equals(null)) ? 0.0D : produto.IcmsSn101VlrCred);
                cmd.Parameters.AddWithValue("@situacaotributariaipi", (produto.SituacaoTributariaIpi == null) ? string.Empty : produto.SituacaoTributariaIpi);
                cmd.Parameters.AddWithValue("@clEnq", produto.ClEnq == null ? string.Empty : produto.ClEnq);
                cmd.Parameters.AddWithValue("@cenq", produto.CEnq == null ? string.Empty : produto.CEnq);
                cmd.Parameters.AddWithValue("@preco_decimais", produto.PrecoDecimais.Equals(null) ? 0.0D : produto.PrecoDecimais);
                cmd.Parameters.AddWithValue("@pis_cst", produto.PisCst.Equals(null) ? 0 : produto.PisCst);
                cmd.Parameters.AddWithValue("@cofins_cst", produto.CofinsCst.Equals(null) ? 0 : produto.CofinsCst);
                cmd.Parameters.AddWithValue("@cest", produto.Cest == null ? string.Empty : produto.Cest);
                cmd.Parameters.AddWithValue("@tabelabaseatacado", produto.TabelaBaseAtacado.Equals(null) ? 0 : produto.TabelaBaseAtacado);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                Produto produtoCadastrado = Get(produto.Codigo);

                if (produto.ItensGrade.Count > 0)
                {

                    for (int i = 0; i < produto.ItensGrade.Count; i++)
                    {
                        cmd.CommandText = "INSERT INTO itensgradeproduto(codigo, codicorprod, cor, tamanho, codbarraint, codbarraforn, precocompra,"
                                            + "referencia,precovenda,estoqueinicial,jafeito,loja,etiqueta,qtetiqueta,chavelojaorigem,ultimamodificacao) "
                                            + "VALUES(@codigo,@codicorprod,@cor,@tamanho, @codbarraint, @codbarraforn, @precocompra,"
                                            + "@referencia, @precovenda, @estoqueinicial, @jafeito, @loja, @etiqueta, @qtetiqueta, @chavelojaorigem, "
                                            + "@ultimamodificacao); "
                                            + "SELECT CAST(scope_identity() AS int)";

                        cmd.Parameters.AddWithValue("@codigo", produtoCadastrado.Chaveunica);
                        cmd.Parameters.AddWithValue("@codicorprod", produto.ItensGrade.ElementAt(i).CodiCorProd == null ? string.Empty : produto.ItensGrade.ElementAt(i).CodiCorProd);
                        cmd.Parameters.AddWithValue("@cor", produto.ItensGrade.ElementAt(i).Cor == null ? string.Empty : produto.ItensGrade.ElementAt(i).Cor);
                        cmd.Parameters.AddWithValue("@tamanho", produto.ItensGrade.ElementAt(i).Tamanho == null ? string.Empty : produto.ItensGrade.ElementAt(i).Tamanho);
                        cmd.Parameters.AddWithValue("@codbarraint", produto.ItensGrade.ElementAt(i).CodBarraInt == null ? string.Empty : produto.ItensGrade.ElementAt(i).CodBarraInt);
                        cmd.Parameters.AddWithValue("@codbarraforn", produto.ItensGrade.ElementAt(i).CodBarraForn == null ? string.Empty : produto.ItensGrade.ElementAt(i).CodBarraForn);
                        cmd.Parameters.AddWithValue("@precocompra", produto.ItensGrade.ElementAt(i).PrecoCompra.Equals(null) ? 0.0M : produto.ItensGrade.ElementAt(i).PrecoCompra);
                        cmd.Parameters.AddWithValue("@referencia", produto.ItensGrade.ElementAt(i).Referencia == null ? string.Empty : produto.ItensGrade.ElementAt(i).Referencia);
                        cmd.Parameters.AddWithValue("@precovenda", produto.ItensGrade.ElementAt(i).PrecoVenda.Equals(null) ? 0.0M : produto.ItensGrade.ElementAt(i).PrecoVenda);
                        cmd.Parameters.AddWithValue("@estoqueinicial", produto.ItensGrade.ElementAt(i).EstoqueInicial.Equals(null) ? 0.0M : produto.ItensGrade.ElementAt(i).EstoqueInicial);
                        cmd.Parameters.AddWithValue("@jafeito", produto.ItensGrade.ElementAt(i).JaFeito == null ? "S" : produto.ItensGrade.ElementAt(i).JaFeito);
                        cmd.Parameters.AddWithValue("@loja", produto.ItensGrade.ElementAt(i).Loja.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).Loja);
                        cmd.Parameters.AddWithValue("@etiqueta", produto.ItensGrade.ElementAt(i).Etiqueta == null ? string.Empty : produto.ItensGrade.ElementAt(i).Etiqueta);
                        cmd.Parameters.AddWithValue("@qtetiqueta", produto.ItensGrade.ElementAt(i).QtEtiqueta.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).QtEtiqueta);
                        cmd.Parameters.AddWithValue("@chavelojaorigem", produto.ItensGrade.ElementAt(i).ChaveLojaOrigem.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).ChaveLojaOrigem);
                        cmd.Parameters.AddWithValue("@ultimamodificacao", produto.ItensGrade.ElementAt(i).UltimaModificacao = DateTime.Now);
                        
                        produto.ItensGrade.ElementAt(i).ChaveUnica = (int)cmd.ExecuteScalar();

                        if (produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.Count > 0)
                        {

                            for (int y = 0; y < produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.Count; y++)
                            {
                                cmd.CommandText = "INSERT INTO EstoqueFilial(CodigoLoja, CodigoFilial, CodigoProduto, Estoque) "
                                    + "VALUES(@CodigoLoja, @CodigoFilial, @CodigoProduto, @Estoque)";
                                cmd.Parameters.AddWithValue("CodigoLoja", produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).CodigoLoja.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).CodigoLoja);
                                cmd.Parameters.AddWithValue("CodigoFilial", produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).CodigoFilial.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).CodigoFilial);
                                cmd.Parameters.AddWithValue("CodigoProduto", produto.ItensGrade.ElementAt(i).ChaveUnica);
                                cmd.Parameters.AddWithValue("Estoque", produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).Estoque.Equals(null) ? 0.0M : produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).Estoque);
                                
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();

                            }
                        }

                        if (produto.ItensGrade.ElementAt(i).PrecosDeVenda.Count > 0)
                        {
                            for (int preco = 0; preco < produto.ItensGrade.ElementAt(i).PrecosDeVenda.Count; preco++)
                            {
                                cmd.CommandText = "INSERT INTO ItensTabelaPreco(Codigo, CodigoProduto, PrecoVenda) "
                                    + "VALUES(@Codigo, @CodigoProduto, @PrecoVenda)";
                                cmd.Parameters.AddWithValue("Codigo", produto.ItensGrade.ElementAt(i).PrecosDeVenda.ElementAt(preco).CodigoTabela.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).PrecosDeVenda.ElementAt(preco).CodigoTabela);
                                cmd.Parameters.AddWithValue("CodigoProduto", produto.ItensGrade.ElementAt(i).ChaveUnica);
                                cmd.Parameters.AddWithValue("PrecoVenda", produto.ItensGrade.ElementAt(i).PrecosDeVenda.ElementAt(preco).Preco.Equals(null) ? 0.0M : produto.ItensGrade.ElementAt(i).PrecosDeVenda.ElementAt(preco).Preco);
                                
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                        }
                    }
                }


            }
        }

        public void Update(Produto produto)
        {
            using (_connection = new SqlConnection(_con))
            {
                _connection.Open();

                SqlTransaction transaction = (SqlTransaction)_connection.BeginTransaction();
                SqlCommand cmd = (SqlCommand)_connection.CreateCommand();
                cmd.Transaction = transaction;

                try
                {
                    cmd.CommandText = "UPDATE cadproduto SET nome=@nome, nomeimpresso=@NOMEIMPRESSO, codigonbn=@codigonbn, unidade=@unidade"
                            + ", ref=@ref,codforn=@codforn, datacad=@datacad,loja=@loja, francionado=@francionado, bloqueado=@bloqueado"
                            + ",foto=@foto,icms=@icms,codaliquota=@codaliquota, tributaçãoicms=@tributacaoicms,percentualdeicms=@percentualdeicms"
                            + ", classificaçãofiscal=@classificacaofiscal, origemdoproduto=@origemdoproduto,percentualipi=@percentualipi,tributaçãoipi=@tributacaoipi"
                            + ",margemdelucro=@margemdelucro,feito=@feito,fabricante=@fabricante,fiscal_convenio=@fiscal_convenio, fiscal_stfaturamento=@fiscal_stfaturamento"
                            + ",fiscal_ReducaoBaseCalcIcms=@fiscal_ReducaoBaseCalcIcms,ativado=@ativado,regimetributario=@regimetributario,"
                            + "aliquota_icms_st=@aliquota_icms_st,perc_reducao_bc_st=@perc_reducao_bc_st, uf_icms_st=@uf_icms_st,modalidade_determina_bc_st=@modalidade_determina_bc_st"
                            + ",perc_margem_valor_adic_st=@perc_margem_valor_adic_st, vBCSTRet=@vBCSTRet,vICMSSTRet=@vICMSSTRet,icms_sn_101_aliqaplic=@icms_sn_101_aliqaplic"
                            + ",icms_sn_101_vlrcred=@icms_sn_101_vlrcred,situacaotributariaipi=@situacaotributariaipi,clEnq=@clEnq,cenq=@cenq,preco_decimais=@preco_decimais,pis_cst=@pis_cst"
                            + ",cofins_cst=@cofins_cst,cest=@cest, tabelabaseatacado=@tabelabaseatacado "
                            + "WHERE codigo=@codigo";


                    cmd.Parameters.AddWithValue("@codigo", produto.Codigo);
                    cmd.Parameters.AddWithValue("@nome", produto.Nome);
                    cmd.Parameters.AddWithValue("@NOMEIMPRESSO", produto.NomeImpresso ?? string.Empty);
                    cmd.Parameters.AddWithValue("@codigonbn", produto.CodigoNbn ?? string.Empty);
                    cmd.Parameters.AddWithValue("@unidade", produto.Unidade ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ref", produto.Referencia ?? string.Empty);
                    cmd.Parameters.AddWithValue("@codforn", produto.CodForn ?? string.Empty);
                    cmd.Parameters.AddWithValue("@datacad", produto.DataCadastro ?? DateTime.Now.ToLongDateString());
                    cmd.Parameters.AddWithValue("@loja", (produto.Loja.Equals(null)) ? 0 : produto.Loja);
                    cmd.Parameters.AddWithValue("@francionado", (produto.Fracionado.Equals(null)) ? false : produto.Fracionado);
                    cmd.Parameters.AddWithValue("@bloqueado", (produto.Bloqueado.Equals(null)) ? false : produto.Bloqueado);
                    cmd.Parameters.AddWithValue("@foto", produto.Foto ?? new Byte[0]);
                    cmd.Parameters.AddWithValue("@icms", (produto.ICMS.Equals(null)) ? 0 : produto.ICMS);
                    cmd.Parameters.AddWithValue("@codaliquota", produto.CodAliquota ?? string.Empty);
                    cmd.Parameters.AddWithValue("@tributacaoicms", produto.TributacaoICMS ?? string.Empty);
                    cmd.Parameters.AddWithValue("@percentualdeicms", (produto.PercentualDeICMS.Equals(null)) ? 0.0F : produto.PercentualDeICMS);
                    cmd.Parameters.AddWithValue("@classificacaofiscal", produto.ClassificacaoFiscal ?? string.Empty);
                    cmd.Parameters.AddWithValue("@origemdoproduto", produto.OrigemDoProduto ?? string.Empty);
                    cmd.Parameters.AddWithValue("@percentualipi", (produto.PercentualIpi.Equals(null)) ? 0.0F : produto.PercentualIpi);
                    cmd.Parameters.AddWithValue("@tributacaoipi", (produto.TributacaoIpi.Equals(null)) ? 0 : produto.TributacaoIpi);
                    cmd.Parameters.AddWithValue("@margemdelucro", (produto.MargemDelucro.Equals(null)) ? 0.0M : produto.MargemDelucro);
                    cmd.Parameters.AddWithValue("@feito", (produto.Feito.Equals(null)) ? -1 : produto.Feito);
                    cmd.Parameters.AddWithValue("@fabricante", (produto.FabricanteId.Equals(null)) ? 0 : produto.FabricanteId);
                    cmd.Parameters.AddWithValue("@fiscal_convenio", produto.FiscalConvenio ?? string.Empty);
                    cmd.Parameters.AddWithValue("@fiscal_stfaturamento", (produto.FiscalStFaturamento == null) ? string.Empty : produto.FiscalStFaturamento);
                    cmd.Parameters.AddWithValue("@Fiscal_ReducaoBaseCalcIcms", produto.FiscalReducaoBaseCalcICMS.Equals(null) ? 0.0M : produto.FiscalReducaoBaseCalcICMS);
                    cmd.Parameters.AddWithValue("@ativado", (produto.Ativado.Equals(null)) ? false : produto.Ativado);
                    cmd.Parameters.AddWithValue("@regimetributario", (produto.RegimeTributario.Equals(null)) ? 0 : produto.RegimeTributario);
                    cmd.Parameters.AddWithValue("@aliquota_icms_st", (produto.AliquotaIcmsSt.Equals(null)) ? 0.0D : produto.AliquotaIcmsSt);
                    cmd.Parameters.AddWithValue("@perc_reducao_bc_st", (produto.PercReducaoBcSt.Equals(null)) ? 0.0D : produto.PercReducaoBcSt);
                    cmd.Parameters.AddWithValue("@uf_icms_st", (produto.UfIcmsSt.Equals(null)) ? -1 : produto.UfIcmsSt);
                    cmd.Parameters.AddWithValue("@modalidade_determina_bc_st", produto.ModalidadeDeterminaBcSt.Equals(null) ? 0 : produto.ModalidadeDeterminaBcSt);
                    cmd.Parameters.AddWithValue("@perc_margem_valor_adic_st", produto.PercMargemValorAdicSt.Equals(null) ? 0.0D : produto.PercMargemValorAdicSt);
                    cmd.Parameters.AddWithValue("@vBCSTRet", (produto.vBcStRet.Equals(null)) ? 0.0D : produto.vBcStRet);
                    cmd.Parameters.AddWithValue("@vICMSSTRet", (produto.vIcmsStRet.Equals(null)) ? 0.0D : produto.vIcmsStRet);
                    cmd.Parameters.AddWithValue("@icms_sn_101_aliqaplic", (produto.IcmsSn101AliqAplic.Equals(null)) ? 0.0D : produto.IcmsSn101AliqAplic);
                    cmd.Parameters.AddWithValue("@icms_sn_101_vlrcred", (produto.IcmsSn101VlrCred.Equals(null)) ? 0.0D : produto.IcmsSn101VlrCred);
                    cmd.Parameters.AddWithValue("@situacaotributariaipi", (produto.SituacaoTributariaIpi == null) ? string.Empty : produto.SituacaoTributariaIpi);
                    cmd.Parameters.AddWithValue("@clEnq", produto.ClEnq == null ? string.Empty : produto.ClEnq);
                    cmd.Parameters.AddWithValue("@cenq", produto.CEnq == null ? string.Empty : produto.CEnq);
                    cmd.Parameters.AddWithValue("@preco_decimais", produto.PrecoDecimais.Equals(null) ? 0.0D : produto.PrecoDecimais);
                    cmd.Parameters.AddWithValue("@pis_cst", produto.PisCst.Equals(null) ? 0 : produto.PisCst);
                    cmd.Parameters.AddWithValue("@cofins_cst", produto.CofinsCst.Equals(null) ? 0 : produto.CofinsCst);
                    cmd.Parameters.AddWithValue("@cest", produto.Cest == null ? string.Empty : produto.Cest);
                    cmd.Parameters.AddWithValue("@tabelabaseatacado", produto.TabelaBaseAtacado.Equals(null) ? 0 : produto.TabelaBaseAtacado);

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    if (produto.ItensGrade.Count > 0)
                    {

                        for (int i = 0; i < produto.ItensGrade.Count; i++)
                        {

                            cmd.CommandText = "UPDATE itensgradeproduto SET codicorprod=@codicorprod,cor=@cor,tamanho=@tamanho, codbarraint=@codbarraint"
                                                + ",codbarraforn=@codbarraforn, precocompra=@precocompra,referencia=@referencia, precovenda=@precovenda"
                                                + ",estoqueinicial=@estoqueinicial, jafeito=@jafeito, loja=@loja, etiqueta=@etiqueta, qtetiqueta=@qtetiqueta"
                                                + ",chavelojaorigem=@chavelojaorigem "
                                                + " WHERE codigo=@codigo";

                            cmd.Parameters.AddWithValue("@codigo", produto.Chaveunica);
                            cmd.Parameters.AddWithValue("@codicorprod", produto.ItensGrade.ElementAt(i).CodiCorProd == null ? string.Empty : produto.ItensGrade.ElementAt(i).CodiCorProd);
                            cmd.Parameters.AddWithValue("@cor", produto.ItensGrade.ElementAt(i).Cor == null ? string.Empty : produto.ItensGrade.ElementAt(i).Cor);
                            cmd.Parameters.AddWithValue("@tamanho", produto.ItensGrade.ElementAt(i).Tamanho == null ? string.Empty : produto.ItensGrade.ElementAt(i).Tamanho);
                            cmd.Parameters.AddWithValue("@codbarraint", produto.ItensGrade.ElementAt(i).CodBarraInt == null ? string.Empty : produto.ItensGrade.ElementAt(i).CodBarraInt);
                            cmd.Parameters.AddWithValue("@codbarraforn", produto.ItensGrade.ElementAt(i).CodBarraForn == null ? string.Empty : produto.ItensGrade.ElementAt(i).CodBarraForn);
                            cmd.Parameters.AddWithValue("@precocompra", produto.ItensGrade.ElementAt(i).PrecoCompra.Equals(null) ? 0.0M : produto.ItensGrade.ElementAt(i).PrecoCompra);
                            cmd.Parameters.AddWithValue("@referencia", produto.ItensGrade.ElementAt(i).Referencia == null ? string.Empty : produto.ItensGrade.ElementAt(i).Referencia);
                            cmd.Parameters.AddWithValue("@precovenda", produto.ItensGrade.ElementAt(i).PrecoVenda.Equals(null) ? 0.0M : produto.ItensGrade.ElementAt(i).PrecoVenda);
                            cmd.Parameters.AddWithValue("@estoqueinicial", produto.ItensGrade.ElementAt(i).EstoqueInicial.Equals(null) ? 0.0M : produto.ItensGrade.ElementAt(i).EstoqueInicial);
                            cmd.Parameters.AddWithValue("@jafeito", produto.ItensGrade.ElementAt(i).JaFeito == null ? "S" : produto.ItensGrade.ElementAt(i).JaFeito);
                            cmd.Parameters.AddWithValue("@loja", produto.ItensGrade.ElementAt(i).Loja.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).Loja);
                            cmd.Parameters.AddWithValue("@etiqueta", produto.ItensGrade.ElementAt(i).Etiqueta == null ? string.Empty : produto.ItensGrade.ElementAt(i).Etiqueta);
                            cmd.Parameters.AddWithValue("@qtetiqueta", produto.ItensGrade.ElementAt(i).QtEtiqueta.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).QtEtiqueta);
                            cmd.Parameters.AddWithValue("@chavelojaorigem", produto.ItensGrade.ElementAt(i).ChaveLojaOrigem.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).ChaveLojaOrigem);

                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();


                            if (produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.Count > 0)
                            {

                                for (int y = 0; y < produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.Count; y++)
                                {
                                    cmd.CommandText = "UPDATE EstoqueFilial SET codigoloja=@CodigoLoja, codigofilial=@CodigoFilial"
                                                        + ",estoque=@Estoque WHERE codigoproduto=@codigoproduto";

                                    cmd.Parameters.AddWithValue("CodigoProduto", produto.ItensGrade.ElementAt(i).ChaveUnica);
                                    cmd.Parameters.AddWithValue("CodigoLoja", produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).CodigoLoja.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).CodigoLoja);
                                    cmd.Parameters.AddWithValue("CodigoFilial", produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).CodigoFilial.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).CodigoFilial);
                                    cmd.Parameters.AddWithValue("Estoque", produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).Estoque.Equals(null) ? 0.0M : produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).Estoque);
                                    //cmd3.Parameters.AddWithValue("UltimaModificacao", produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).UltimaModificacao == null ? DateTime.Now : produto.ItensGrade.ElementAt(i).EstoqueNasFiliais.ElementAt(y).UltimaModificacao);
                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();
                                }

                            }

                            if (produto.ItensGrade.ElementAt(i).PrecosDeVenda.Count > 0)
                            {

                                for (int preco = 0; preco < produto.ItensGrade.ElementAt(i).PrecosDeVenda.Count; preco++)
                                {
                                    cmd.CommandText = "UPDATE ItensTabelaPreco SET codigo=@Codigo, precovenda=@PrecoVenda WHERE codigoproduto=@codigoproduto AND chaveunica=@chaveunica";

                                    cmd.Parameters.AddWithValue("CodigoProduto", produto.ItensGrade.ElementAt(i).ChaveUnica);
                                    cmd.Parameters.AddWithValue("@chaveunica", produto.ItensGrade.ElementAt(i).PrecosDeVenda.ElementAt(preco).ChaveUnica);
                                    cmd.Parameters.AddWithValue("Codigo", produto.ItensGrade.ElementAt(i).PrecosDeVenda.ElementAt(preco).CodigoTabela.Equals(null) ? 0 : produto.ItensGrade.ElementAt(i).PrecosDeVenda.ElementAt(preco).CodigoTabela);
                                    cmd.Parameters.AddWithValue("PrecoVenda", produto.ItensGrade.ElementAt(i).PrecosDeVenda.ElementAt(preco).Preco.Equals(null) ? 0.0M : produto.ItensGrade.ElementAt(i).PrecosDeVenda.ElementAt(preco).Preco);
                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();
                                }
                            }
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

        public void Delete(long codigo)
        {
            using (_connection = new SqlConnection(_con))
            {
                _connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = (SqlConnection)_connection;
                SqlTransaction transaction = (SqlTransaction)_connection.BeginTransaction();
                cmd.Transaction = transaction;

                try
                {
                    cmd.CommandText = "DELETE FROM CADPRODUTO WHERE codigo = @codigo";
                    cmd.Parameters.AddWithValue("@codigo", codigo);

                    cmd.ExecuteScalar();
                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }

                    catch (InvalidOperationException exTransaction)
                    {
                        throw exTransaction;
                    }
                }

            }

        }
    }
}