using DinnamuS_API.Repositories.Utils;
using DinnamuSWebApi.Data.Vendas;
using DinnamuSWebApi.Models;
using System;
using System.Collections.Generic;
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

        public CotacaoRepository()
        {
            _connection = new SqlConnection("Server=MAFIA;DATABASE=Principal;User ID=sa;Password=sa");
        }

        public DadosOrc cotacaoByCodigo(long codigo)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT do.codigo, do.data, do.valor, do.hora, do.desconto, do.percdesc, do.totalbruto,
                do.dinheiro, do.troco, do.codigocotacao, do.codvendedor, do.vendedor, do.codCliente, do.cliente, do.recebido,
                do.feito,
                iorc.idunico, iorc.codigo, iorc.codprod, iorc.descricao, iorc.quantidade,iorc.preco, iorc.total, iorc.codtam,
                iorc.ref, iorc.tabela, iorc.descp, iorc.descv, iorc.precooriginal, iorc.unidade, iorc.custo, iorc.seq, iorc.ncm
                 
                FROM dadosorc do 
                inner join itensorc iorc
                on do.codigo = iorc.codigo

                WHERE do.codigo=@codigo
        
                AND do.recebido='N' AND do.feito='S' ";

                cmd.Connection = (SqlConnection)_connection;
                cmd.Parameters.AddWithValue("@codigo", codigo);
                _connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();


                Dictionary<long, DadosOrc> cotacoes = new Dictionary<long, DadosOrc>();

                while (reader.Read())
                {
                    DadosOrc cotacao = new DadosOrc();

                    if (!cotacoes.ContainsKey(reader.GetInt64(0)))
                    {
                        cotacao.Codigo = (long)reader.GetInt64(0);
                        cotacao.Data = ((DBNull.Value == reader[1]) ? "01/01/1900" : reader.GetDateTime(1).ToShortDateString());
                        cotacao.Valor = (decimal)((DBNull.Value == reader[2]) ? 0.0M : reader.GetDecimal(2));
                        cotacao.Hora = ((DBNull.Value == reader[3]) ? "00:00:00" : reader.GetDateTime(3).ToShortTimeString());
                        cotacao.Desconto = (decimal)((DBNull.Value == reader[4]) ? 0.00M : reader.GetDecimal(4));
                        cotacao.PercDesc = (decimal)((DBNull.Value == reader[5]) ? 0.00M : reader.GetDecimal(5));
                        cotacao.TotalBruto = (decimal)((DBNull.Value == reader[6]) ? 0.00M : reader.GetDecimal(6));
                        cotacao.Dinheiro = (decimal)((DBNull.Value == reader[7]) ? 0.00M : reader.GetDecimal(7));
                        cotacao.Troco = (decimal)((DBNull.Value == reader[8]) ? 0.00M : reader.GetDecimal(8));
                        cotacao.CodigoCotacao = (int)((DBNull.Value == reader[9]) ? 0 : reader.GetInt32(9));
                        cotacao.CodVendedor = (string)((DBNull.Value == reader[10]) ? string.Empty : reader.GetString(10));
                        cotacao.NomeVendedor = (string)((DBNull.Value == reader[11]) ? string.Empty : reader.GetString(11));
                        cotacao.CodCliente = (string)((DBNull.Value == reader[12]) ? string.Empty : reader.GetString(12));
                        cotacao.NomeCliente = (string)((DBNull.Value == reader[13]) ? string.Empty : reader.GetString(13));

                        cotacoes.Add(cotacao.Codigo, cotacao);
                    }

                    else
                    {
                        cotacao = cotacoes[reader.GetInt64(0)];
                    }

                    ItemOrc item = new ItemOrc()
                    {
                        IdUnico = (long)((DBNull.Value == reader[16]) ? 0 : reader.GetInt64(16)),
                        Codigo = (long)((DBNull.Value == reader[17]) ? 0 : reader.GetInt64(17)),
                        CodProd = (int)((DBNull.Value == reader[18]) ? 0 : reader.GetInt32(18)),
                        DescricaoProd = (string)((DBNull.Value == reader[19]) ? string.Empty : reader.GetString(19)),
                        Quantidade = (decimal)((DBNull.Value == reader[20]) ? 0.0M : reader.GetDecimal(20)),
                        Preco = (decimal)((DBNull.Value == reader[21]) ? 0.0M : reader.GetDecimal(21)),
                        Total = (decimal)((DBNull.Value == reader[22]) ? 0.0M : reader.GetDecimal(22)),
                        CodTam = (string)((DBNull.Value == reader[23]) ? string.Empty : reader.GetString(23)),
                        REF = (string)((DBNull.Value == reader[24]) ? string.Empty : reader.GetString(24)),
                        Tabela = (string)((DBNull.Value == reader[25]) ? string.Empty : reader.GetString(25)),
                        DescP = (decimal)((DBNull.Value == reader[26]) ? 0.0M : reader.GetDecimal(26)),
                        DescV = (decimal)((DBNull.Value == reader[27]) ? 0.0M : reader.GetDecimal(27)),
                        PrecoOriginal = (decimal)((DBNull.Value == reader[28]) ? 0.0M : reader.GetDecimal(28)),
                        UN = (string)((DBNull.Value == reader[29]) ? string.Empty : reader.GetString(29)),
                        Custo = (decimal)((DBNull.Value == reader[30]) ? 0.0M : reader.GetDecimal(30)),
                        Seq = (int)((DBNull.Value == reader[31]) ? 0 : reader.GetInt32(31)),
                        NCM = (string)((DBNull.Value == reader[32]) ? string.Empty : reader.GetString(32))
                    };

                    cotacao.itens = (cotacao.itens == null) ? new List<ItemOrc>() : cotacao.itens;
                    cotacao.itens.Add(item);
                }

                return cotacoes.Values.ToList().FirstOrDefault();
            }

            catch (SqlException ex)
            {
                throw ex;
            }

            catch (IOException ex)
            {
                throw ex;
            }

            finally
            {
                _connection.Close();
            }

            
        }

        public List<DadosOrc> cotacoes()
        {
            //List<DadosOrc_ConsultaCotacaoDTO> cotacoes = new List<DadosOrc_ConsultaCotacaoDTO>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT do.codigo, do.data, do.valor, do.hora, do.desconto, do.percdesc, do.totalbruto,
                do.dinheiro, do.troco, do.codigocotacao, do.codvendedor, do.vendedor, do.codCliente, do.cliente, do.recebido,
                do.feito,
                iorc.idunico, iorc.codigo, iorc.codprod, iorc.descricao, iorc.quantidade,iorc.preco, iorc.total, iorc.codtam,
                iorc.ref, iorc.tabela, iorc.descp, iorc.descv, iorc.precooriginal, iorc.unidade, iorc.custo, iorc.seq, iorc.ncm
                 
                FROM dadosorc do 
                inner join itensorc iorc
                on do.codigo = iorc.codigo
        
                AND do.recebido='N' AND do.feito='S' ";
                
                cmd.Connection = (SqlConnection)_connection;
                _connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                Dictionary<long, DadosOrc> cotacoes = new Dictionary<long, DadosOrc>();

                while(reader.Read())
                {
                    DadosOrc cotacao = new DadosOrc();

                    if (!cotacoes.ContainsKey(reader.GetInt64(0)))
                    {
                        cotacao.Codigo = (long)reader.GetInt64(0);
                        cotacao.Data = ((DBNull.Value == reader[1]) ? "01/01/1900" : reader.GetDateTime(1).ToShortDateString());
                        cotacao.Valor = (decimal)((DBNull.Value == reader[2]) ? default(decimal) : reader.GetDecimal(2));
                        cotacao.Hora = ((DBNull.Value == reader[3]) ? "00:00:00" : reader.GetDateTime(3).ToShortTimeString());
                        cotacao.Desconto = (decimal)((DBNull.Value == reader[4]) ? default(decimal) : reader.GetDecimal(4));
                        cotacao.PercDesc = (decimal)((DBNull.Value == reader[5]) ? default(decimal) : reader.GetDecimal(5));
                        cotacao.TotalBruto = (decimal)((DBNull.Value == reader[6]) ? default(decimal) : reader.GetDecimal(6));
                        cotacao.Dinheiro = (decimal)((DBNull.Value == reader[7]) ? default(decimal) : reader.GetDecimal(7));
                        cotacao.Troco = (decimal)((DBNull.Value == reader[8]) ? default(decimal) : reader.GetDecimal(8));
                        cotacao.CodigoCotacao = (int)((DBNull.Value == reader[9]) ? default(int) : reader.GetInt32(9));
                        cotacao.CodVendedor = (string)((DBNull.Value == reader[10]) ? string.Empty : reader.GetString(10));
                        cotacao.NomeVendedor = (string)((DBNull.Value == reader[11]) ? string.Empty : reader.GetString(11));
                        cotacao.CodCliente = (string)((DBNull.Value == reader[12]) ? string.Empty : reader.GetString(12));
                        cotacao.NomeCliente = (string)((DBNull.Value == reader[13]) ? string.Empty : reader.GetString(13));
                        
                        cotacoes.Add(cotacao.Codigo, cotacao);
                    }

                    else
                    {
                        cotacao = cotacoes[reader.GetInt64(0)];
                    }

                    ItemOrc item = new ItemOrc()
                    {
                        IdUnico = (long)((DBNull.Value == reader[16]) ? default(long) : reader.GetInt64(16)),
                        Codigo = (long)((DBNull.Value == reader[17]) ? default(long) : reader.GetInt64(17)),
                        CodProd = (int)((DBNull.Value == reader[18]) ? default(int) : reader.GetInt32(18)),
                        DescricaoProd = (string)((DBNull.Value == reader[19]) ? string.Empty : reader.GetString(19)),
                        Quantidade = (decimal)((DBNull.Value == reader[20]) ? default(decimal) : reader.GetDecimal(20)),
                        Preco = (decimal)((DBNull.Value == reader[21]) ? default(decimal) : reader.GetDecimal(21)),
                        Total = (decimal)((DBNull.Value == reader[22]) ? default(decimal) : reader.GetDecimal(22)),
                        CodTam = (string)((DBNull.Value == reader[23]) ? string.Empty : reader.GetString(23)),
                        REF = (string)((DBNull.Value == reader[24]) ? string.Empty : reader.GetString(24)),
                        Tabela = (string)((DBNull.Value == reader[25]) ? string.Empty : reader.GetString(25)),
                        DescP = (decimal)((DBNull.Value == reader[26]) ? default(decimal) : reader.GetDecimal(26)),
                        DescV = (decimal)((DBNull.Value == reader[27]) ? default(decimal) : reader.GetDecimal(27)),
                        PrecoOriginal = (decimal)((DBNull.Value == reader[28]) ? default(decimal) : reader.GetDecimal(28)),
                        UN = (string)((DBNull.Value == reader[29]) ? string.Empty : reader.GetString(29)),
                        Custo = (decimal)((DBNull.Value == reader[30]) ? default(decimal) : reader.GetDecimal(30)),
                        Seq = (int)((DBNull.Value == reader[31]) ? default(int) : reader.GetInt32(31)),
                        NCM = (string)((DBNull.Value == reader[32]) ? string.Empty : reader.GetString(32))
                    };

                    cotacao.itens = (cotacao.itens == null) ? new List<ItemOrc>() : cotacao.itens;
                    cotacao.itens.Add(item);
                }

                return cotacoes.Values.ToList();
            }

            catch(SqlException ex)
            {
                throw ex;
            }

            catch(IOException ex)
            {
                throw ex;
            }

            finally
            {
                _connection.Close();
            }

            //return null;
        }

        public void Inserir(DadosOrc cotacao)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO dadosorc(codigo,codigocotacao,data,valor,hora,desconto,percdesc,totalbruto,"
                + "dinheiro,troco,codvendedor,nomevendedor,codcliente,nomecliente) VALUES(@codigo,@codigocotacao,@data,@valor,"+
                "@hora,@desconto,@percdesc,@totalbruto,"
                + "@dinheiro,@troco,@codvendedor,@nomevendedor,@codcliente,@nomecliente)";

                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@codigo", GerarId.Gerar("dadosorc"));
                cmd.Parameters.AddWithValue("@codigocotacao", cotacao.CodigoCotacao);
                cmd.Parameters.AddWithValue("@data", cotacao.Data);
                cmd.Parameters.AddWithValue("@valor", cotacao.Valor);
                cmd.Parameters.AddWithValue("@hora", cotacao.Hora);
                cmd.Parameters.AddWithValue("@desconto", cotacao.Desconto);
                cmd.Parameters.AddWithValue("@percdesc", cotacao.PercDesc);
                cmd.Parameters.AddWithValue("@totalbruto", cotacao.TotalBruto);
                cmd.Parameters.AddWithValue("@dinheiro", cotacao.Dinheiro);
                cmd.Parameters.AddWithValue("@troco", cotacao.Troco);
                cmd.Parameters.AddWithValue("@codvendedor", cotacao.CodVendedor);
                cmd.Parameters.AddWithValue("@nomevendedor", cotacao.NomeVendedor);
                cmd.Parameters.AddWithValue("@codcliente", cotacao.CodCliente);
                cmd.Parameters.AddWithValue("@nomecliente", cotacao.NomeCliente);
                _connection.Open();

                cmd.ExecuteScalar();

            }
            catch(Exception ex)
            {
                throw ex;
            }

            finally
            {
                _connection.Close();
            }
        }
    }
}