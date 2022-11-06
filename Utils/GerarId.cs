using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DinnamuS_API.Repositories.Utils
{
    public class GerarId
    {
        private static IDbConnection _connection;
        private static SqlCommand cmd;
        private static int codLoja = 0;
        private static int codPdv = 0;
        static string _con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        static GerarId()
        {    
            //_connection = new SqlConnection("Server=MAFIA;DATABASE=Principal;User ID=sa;Password=sa");
        }

        /// <summary>
        /// Gera um novo Id (long) para a tabela
        /// </summary>
        /// <param name="nomeTabela">Nome da tabela para a qual gerar Sequencial</param>
        /// <param name="usarPDV">Deve usar o campo codigo_pdv_online do cadastro de lojas?</param>
        /// <returns>retorna o sequencial para o Id, do tipo long (Int64)</returns>
        public static long GerarNovoCodigoTabela(string nomeTabela, bool usarPDV = true)
        {
            using (_connection = new SqlConnection(_con))
            {
                _connection.Open();

                using (cmd = (SqlCommand)_connection.CreateCommand())
                {
                    cmd.CommandText = "NovoCodigoTabela";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter nValor = new SqlParameter();
                    nValor.ParameterName = "@nValor";
                    nValor.SqlDbType = SqlDbType.BigInt;
                    nValor.Value = 0;
                    nValor.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(nValor);

                    cmd.Parameters.AddWithValue("@cNomeTabela", nomeTabela);
                    cmd.Connection = (SqlConnection)_connection;

                    cmd.ExecuteNonQuery();

                    long novaSequencia = (long)nValor.Value;

                    RetornaCodigoLojaPDV();

                    string IdConcatenado = string.Empty;

                    if (usarPDV)
                    {
                        IdConcatenado = codLoja.ToString() + "0" + codPdv.ToString() + "0" + novaSequencia.ToString();
                    }

                    else
                    {
                        IdConcatenado = codLoja.ToString() + "0" + novaSequencia.ToString();
                    }

                    return Int64.Parse(IdConcatenado);
                }
                
            }

                
        }

        /// <summary>
        /// Gera um novo código sequencial para os campos da tabela
        /// </summary>
        /// <param name="nomeTabela">Tabela a ter o código gerado</param>
        /// <returns></returns>
        public static void GerarNovoSequencialTabela(string nomeTabela, DateTime data, out long valor, out long indiceSequenciamento)
        {
            using (_connection = new SqlConnection(_con))
            {
                using (cmd = (SqlCommand)_connection.CreateCommand())
                {
                    cmd.Connection = (SqlConnection)_connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GerarNovoSequencialTabela";
                    _connection.Open();

                    cmd.Parameters.AddWithValue("cNomeTabela", nomeTabela);
                    cmd.Parameters.AddWithValue("Data", data);

                    SqlParameter nValor = new SqlParameter();
                    nValor.ParameterName = "@nValor";
                    nValor.SqlDbType = SqlDbType.BigInt;
                    nValor.Value = 0;
                    nValor.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(nValor);

                    SqlParameter nIndiceSequenciamento = new SqlParameter();
                    nIndiceSequenciamento.ParameterName = "@nIndiceSequenciamento";
                    nIndiceSequenciamento.SqlDbType = SqlDbType.BigInt;
                    nIndiceSequenciamento.Value = 0;
                    nIndiceSequenciamento.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(nIndiceSequenciamento);

                    cmd.ExecuteNonQuery();

                    valor = (long)nValor.Value;
                    indiceSequenciamento = (long)nIndiceSequenciamento.Value;
                }
            }
                
        }
        private static void RetornaCodigoLojaPDV()
        {
            using (_connection = new SqlConnection(_con))
            {
                _connection.Open();

                using (cmd = (SqlCommand)_connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT codigo, codigo_pdv_online FROM lojas WHERE atual='S' ";
                    cmd.Connection = (SqlConnection)_connection;

                    SqlDataReader _reader = cmd.ExecuteReader();

                    _reader.Read();
                    codLoja = (int)_reader.GetInt32(0);
                    codPdv = (int)_reader.GetInt32(1);
                }
                
            }

            
        }
    }
}
