using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DinnamuS_API.Repositories.Utils
{
    public class GerarId
    {
        private static IDbConnection _connection;
        private static int codLoja = 0;
        private static int codPdv = 0;

        static GerarId()
        {
            _connection = new SqlConnection("Server=MAFIA;DATABASE=Principal;User ID=sa;Password=sa");
        }

        /// <summary>
        /// Recebe o nome da tabela a ter o Id gerado
        /// </summary>
        /// <param name="nomeTabela">Nome da tabela para a qual gerar Sequencial</param>
        /// <param name="usarPDV">Deve usar o campo codigo_pdv_online do cadastro de lojas?</param>
        /// <returns>retorna o sequencial para o Id, do tipo long (Int64)</returns>
        public static long Gerar(string nomeTabela, bool usarPDV = true)
        {
            try
            {
                SqlCommand cmd = new SqlCommand() ;
                cmd.CommandText = "NovoCodigoTabela";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cNomeTabela", nomeTabela);

                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@nValor";
                param1.SqlDbType = SqlDbType.BigInt;
                param1.Value = 0;

                param1.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param1);

                cmd.Connection = (SqlConnection)_connection;

                _connection.Open();

                cmd.ExecuteNonQuery();

                long novaSequencia = (long)param1.Value;

                RetornaCodigoLojaPDV();

                string loja = codLoja.ToString();
                string pdv  = codPdv.ToString();


                string IdConcatenado = string.Empty;

                if(usarPDV)
                {
                    IdConcatenado = loja + "0" + pdv + "0" + novaSequencia.ToString();
                }

                else
                {
                    IdConcatenado = loja + "0" + novaSequencia.ToString();
                }
                

                return Int64.Parse(IdConcatenado);
            }

            finally
            {
                _connection.Close();
            }
        }

        private static void RetornaCodigoLojaPDV()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT codigo, codigo_pdv_online FROM lojas WHERE atual='S' ";
                cmd.Connection = (SqlConnection)_connection;
                
                if(_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }

                SqlDataReader _reader = cmd.ExecuteReader();

                _reader.Read();
                codLoja = (int)_reader.GetInt32(0);
                codPdv = (int)_reader.GetInt32(1);
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
