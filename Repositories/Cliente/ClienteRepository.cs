using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DinnamuSWebApi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using DinnamuS_API.Repositories.Utils;
using System.Configuration;

namespace DinnamuSWebApi.Repositories
{
    public class ClienteRepository : IClienteRepository
    {

        private IDbConnection _conexao;

        public List<Cliente> GetList()
        {
            List<Cliente> clientes = new List<Cliente>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT idunico, codigo, nome, apelido, nasc, cpf, rg, tipocli, endereco, numero,complemento,"
                + "bairro, cidade, cep, codigocidade,codigopais, loja, uf FROM clientes WHERE nome IS NOT NULL ORDER BY nome ASC";
                cmd.Connection = (SqlConnection)_conexao;

                _conexao.Open();

                SqlDataReader _reader = cmd.ExecuteReader();

                while (_reader.Read())
                {
                    Cliente cliente = new Cliente()
                    {
                        IdUnico = (long)(Convert.IsDBNull(_reader["idunico"]) ? 0L : _reader["idunico"]),
                        Codigo = (long)(Convert.IsDBNull(_reader["codigo"]) ? 0L : _reader["codigo"]),
                        Nome = (string)(Convert.IsDBNull(_reader["nome"]) ? string.Empty : _reader["nome"]),
                        Apelido = (string)(Convert.IsDBNull(_reader["apelido"]) ? string.Empty : _reader["apelido"]),
                        DataNascimento = (DateTime)(Convert.IsDBNull(_reader["nasc"]) ? DateTime.Now : _reader["nasc"]),
                        CPF = (string)(Convert.IsDBNull(_reader["cpf"]) ? string.Empty : _reader["cpf"]),
                        RG = (string)(Convert.IsDBNull(_reader["rg"]) ? string.Empty : _reader["rg"]),
                        TipoCli = (string)(Convert.IsDBNull(_reader["tipocli"]) ? string.Empty : _reader["tipocli"]),
                        Endereco = (string)(Convert.IsDBNull(_reader["endereco"]) ? string.Empty : _reader["endereco"]),
                        Numero = (string)((DBNull.Value == _reader["numero"]) ? string.Empty : _reader["numero"]),
                        Complemento = (string)((Convert.IsDBNull(_reader["complemento"])) ? string.Empty : _reader["complemento"]),
                        Bairro = (string)(Convert.IsDBNull(_reader["bairro"]) ? string.Empty : _reader["bairro"]),
                        Cidade = (string)(Convert.IsDBNull(_reader["cidade"]) ? string.Empty : _reader["cidade"]),
                        Cep = (string)((Convert.IsDBNull(_reader["cep"])) ? string.Empty : _reader["cep"]),
                        CodigoCidade = (int)(Convert.IsDBNull(_reader["codigocidade"]) ? 0 : _reader["codigocidade"]),
                        CodigoPais = (int)(Convert.IsDBNull(_reader["codigopais"]) ? 0 : _reader["codigopais"]),
                        Loja = (int)(Convert.IsDBNull(_reader["loja"]) ? 0 : _reader["loja"]),
                        UF = (string)(Convert.IsDBNull(_reader["uf"]) ? string.Empty : _reader["uf"])
                    };

                    clientes.Add(cliente);
                }

                return clientes;
            }

            catch (SqlException ex)
            {
                Debug.Write(ex.Message);
                return null;
            }
            finally
            {
                _conexao.Close();
            }
        }

        public ClienteRepository()
        {
            string _con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _conexao = new SqlConnection(_con);
            
        }

        public Cliente GetById(long codigo)
        {
            Cliente cliente = null;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT idunico, codigo, nome, apelido, nasc, cpf, rg, tipocli, endereco, numero,complemento,"
                + "bairro, cidade, cep, codigocidade,codigopais, loja, uf FROM clientes WHERE codigo=@codigo";
                cmd.Connection = (SqlConnection)_conexao;
                cmd.Parameters.AddWithValue("@codigo", codigo);

                if (_conexao.State == ConnectionState.Closed)
                {

                    _conexao.Open();
                }

                SqlDataReader _reader = cmd.ExecuteReader();

                while (_reader.Read())
                {
                    cliente = new Cliente()
                {
                        IdUnico = codigo,
                        Codigo = codigo,
                        Nome = (string)(Convert.IsDBNull(_reader["nome"]) ? string.Empty : _reader["nome"]),
                        Apelido = (string)(Convert.IsDBNull(_reader["apelido"]) ? string.Empty : _reader["apelido"]),
                        DataNascimento = (DateTime)(Convert.IsDBNull(_reader["nasc"]) ? DateTime.Now : _reader["nasc"]),
                        CPF = (string)(Convert.IsDBNull(_reader["cpf"]) ? string.Empty : _reader["cpf"]),
                        RG = (string)(Convert.IsDBNull(_reader["rg"]) ? string.Empty : _reader["rg"]),
                        TipoCli = (string)(Convert.IsDBNull(_reader["tipocli"]) ? string.Empty : _reader["tipocli"]),
                        Endereco = (string)(Convert.IsDBNull(_reader["endereco"]) ? string.Empty : _reader["endereco"]),
                        Numero = (string)((DBNull.Value == _reader["numero"]) ? string.Empty : _reader["numero"]),
                        Complemento = (string)((Convert.IsDBNull(_reader["complemento"])) ? string.Empty : _reader["complemento"]),
                        Bairro = (string)(Convert.IsDBNull(_reader["bairro"]) ? string.Empty : _reader["bairro"]),
                        Cidade = (string)(Convert.IsDBNull(_reader["cidade"]) ? string.Empty : _reader["cidade"]),
                        Cep = (string)((Convert.IsDBNull(_reader["cep"])) ? string.Empty : _reader["cep"]),
                        CodigoCidade = (int)(Convert.IsDBNull(_reader["codigocidade"]) ? 0 : _reader["codigocidade"]),
                        CodigoPais = (int)(Convert.IsDBNull(_reader["codigopais"]) ? 0 : _reader["codigopais"]),
                        Loja = (int)(Convert.IsDBNull(_reader["loja"]) ? 0 : _reader["loja"]),
                        UF = (string)(Convert.IsDBNull(_reader["uf"]) ? string.Empty : _reader["uf"])
                    };

                   
                }

            if (_conexao.State == ConnectionState.Open)
            {
                _conexao.Close();
            }

            return cliente;
        }

        public List<Cliente> GetByName(string nome)
        {
            List<Cliente> clientes = new List<Cliente>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT idunico, codigo, nome, apelido, nasc, cpf, rg, tipocli, endereco, numero,complemento,"
            + "bairro, cidade, cep, codigocidade,codigopais, loja, uf FROM clientes WHERE nome LIKE " + "'%" + nome + "%'";
            cmd.Connection = (SqlConnection)_conexao;
            cmd.Parameters.Add(nome, DbType.String);
            
            _conexao.Open();

            SqlDataReader _reader = cmd.ExecuteReader();

            while (_reader.Read())
            {
                Cliente cliente = new Cliente()
                {
                    IdUnico = (long)(Convert.IsDBNull(_reader["idunico"]) ? 0L : _reader["idunico"]),
                    Codigo = (long)(Convert.IsDBNull(_reader["codigo"]) ? 0L : _reader["codigo"]),
                    Nome = (string)(Convert.IsDBNull(_reader["nome"]) ? string.Empty : _reader["nome"]),
                    Apelido = (string)(Convert.IsDBNull(_reader["apelido"]) ? string.Empty : _reader["apelido"]),
                    DataNascimento = (DateTime)(Convert.IsDBNull(_reader["nasc"]) ? DateTime.Now : _reader["nasc"]),
                    CPF = (string)(Convert.IsDBNull(_reader["cpf"]) ? string.Empty : _reader["cpf"]),
                    RG = (string)(Convert.IsDBNull(_reader["rg"]) ? string.Empty : _reader["rg"]),
                    TipoCli = (string)(Convert.IsDBNull(_reader["tipocli"]) ? string.Empty : _reader["tipocli"]),
                    Endereco = (string)(Convert.IsDBNull(_reader["endereco"]) ? string.Empty : _reader["endereco"]),
                    Numero = (string)((DBNull.Value == _reader["numero"]) ? string.Empty : _reader["numero"]),
                    Complemento = (string)((Convert.IsDBNull(_reader["complemento"])) ? string.Empty : _reader["complemento"]),
                    Bairro = (string)(Convert.IsDBNull(_reader["bairro"]) ? string.Empty : _reader["bairro"]),
                    Cidade = (string)(Convert.IsDBNull(_reader["cidade"]) ? string.Empty : _reader["cidade"]),
                    Cep = (string)((Convert.IsDBNull(_reader["cep"])) ? string.Empty : _reader["cep"]),
                    CodigoCidade = (int)(Convert.IsDBNull(_reader["codigocidade"]) ? 0 : _reader["codigocidade"]),
                    CodigoPais = (int)(Convert.IsDBNull(_reader["codigopais"]) ? 0 : _reader["codigopais"]),
                    Loja = (int)(Convert.IsDBNull(_reader["loja"]) ? 0 : _reader["loja"]),
                    UF = (string)(Convert.IsDBNull(_reader["uf"]) ? string.Empty : _reader["uf"])
                };

                clientes.Add(cliente);
            }

            if (_conexao.State == ConnectionState.Open)
            {
                _conexao.Close();
            }

            return clientes;
        }
        
        public Cliente InsertRetrieve(Cliente cliente)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = (SqlConnection)_conexao;
            cmd.CommandText = "INSERT INTO clientes(idunico, codigo, nome, apelido, nasc, cpf, rg, tipocli, endereco, numero,complemento,"
            + "bairro, cidade, cep, codigocidade,codigopais, loja, uf) VALUES(@idunico,@codigo,@nome,@apelido,@nasc,@cpf,@rg, @tipocli,@endereco,@numero,@complemento,@bairro,@cidade,@cep,@codigocidade,@codigopais,@loja,@uf)";
            long idUnico = GerarId.GerarNovoCodigoTabela("clientes", false);
            cmd.Parameters.AddWithValue("@idunico", idUnico);
            cmd.Parameters.AddWithValue("@codigo", idUnico);
            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@apelido", cliente.Apelido);
            cmd.Parameters.AddWithValue("@nasc", cliente.DataNascimento);
            cmd.Parameters.AddWithValue("@cpf", cliente.CPF);
            cmd.Parameters.AddWithValue("@rg", cliente.RG);
            cmd.Parameters.AddWithValue("@tipocli", cliente.TipoCli);
            cmd.Parameters.AddWithValue("@endereco", cliente.Endereco);
            cmd.Parameters.AddWithValue("@numero", cliente.Numero);
            cmd.Parameters.AddWithValue("@complemento", cliente.Complemento);
            cmd.Parameters.AddWithValue("@bairro", cliente.Bairro);
            cmd.Parameters.AddWithValue("@cidade", cliente.Cidade);
            cmd.Parameters.AddWithValue("@cep", cliente.Cep);
            cmd.Parameters.AddWithValue("@codigocidade", cliente.CodigoCidade);
            cmd.Parameters.AddWithValue("@codigopais", cliente.CodigoPais);
            cmd.Parameters.AddWithValue("@loja", cliente.Loja);
            cmd.Parameters.AddWithValue("@uf", cliente.UF);
            _conexao.Open();

            cmd.ExecuteNonQuery();

            if (_conexao.State == ConnectionState.Open)
            {
                _conexao.Close();
            }

            return GetById(idUnico);

        }

        public void Insert(Cliente cliente)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = (SqlConnection)_conexao;
            cmd.CommandText = "INSERT INTO clientes(idunico, codigo, nome, apelido, nasc, cpf, rg, tipocli, endereco, numero,complemento,"
            + "bairro, cidade, cep, codigocidade,codigopais, loja, uf) VALUES(@idunico,@codigo,@nome,@apelido,@nasc,@cpf,@rg, @tipocli,"
            + "@endereco,@numero,@complemento,@bairro,@cidade,@cep,@codigocidade,@codigopais,@loja,@uf)";

            long seq = GerarId.GerarNovoCodigoTabela("clientes", false);
            cmd.Parameters.AddWithValue("@idunico", seq);
            cmd.Parameters.AddWithValue("@codigo", seq);
            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@apelido", cliente.Apelido);
            cmd.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
            cmd.Parameters.AddWithValue("@Cpf", cliente.CPF);
            cmd.Parameters.AddWithValue("@Rg", cliente.RG);
            cmd.Parameters.AddWithValue("@TipoCli", cliente.TipoCli);
            cmd.Parameters.AddWithValue("@endereco", cliente.Endereco);
            cmd.Parameters.AddWithValue("@numero", cliente.Numero);
            cmd.Parameters.AddWithValue("@complemento", cliente.Complemento);
            cmd.Parameters.AddWithValue("@bairro", cliente.Bairro);
            cmd.Parameters.AddWithValue("@cidade", cliente.Cidade);
            cmd.Parameters.AddWithValue("@cep", cliente.Cep);
            cmd.Parameters.AddWithValue("@codigocidade", cliente.CodigoCidade);
            cmd.Parameters.AddWithValue("@codigopais", cliente.CodigoPais);
            cmd.Parameters.AddWithValue("@loja", cliente.Loja);
            cmd.Parameters.AddWithValue("@uf", cliente.UF);
            cmd.Parameters.AddWithValue("@idunico", cliente.IdUnico);


            _conexao.Open();

            cmd.ExecuteNonQuery();

            if (_conexao.State == ConnectionState.Open)
            {
                _conexao.Close();
            }


        }

        public void Update(Cliente cliente)
        {
            if (_conexao.State == ConnectionState.Closed)
            {
                _conexao.Open();
            }

            SqlTransaction transaction = (SqlTransaction)_conexao.BeginTransaction();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE clientes SET nome=@nome, apelido=@apelido, nasc=@DataNascimento, cpf=@Cpf,"
                + " rg=@Rg, tipocli=@TipoCli, endereco=@endereco, numero=@numero, complemento=@complemento, bairro=@bairro,"
                + " cidade=@cidade, cep=@cep, codigocidade=@codigocidade, codigopais=@codigopais, loja=@loja, uf=@uf"
                + " WHERE idunico=@idunico";
            cmd.Connection = (SqlConnection)_conexao;
            cmd.Transaction = transaction;

            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@apelido", cliente.Apelido);
            cmd.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
            cmd.Parameters.AddWithValue("@Cpf", cliente.CPF);
            cmd.Parameters.AddWithValue("@Rg", cliente.RG);
            cmd.Parameters.AddWithValue("@TipoCli", cliente.TipoCli);
            cmd.Parameters.AddWithValue("@endereco", cliente.Endereco);
            cmd.Parameters.AddWithValue("@numero", cliente.Numero);
            cmd.Parameters.AddWithValue("@complemento", cliente.Complemento);
            cmd.Parameters.AddWithValue("@bairro", cliente.Bairro);
            cmd.Parameters.AddWithValue("@cidade", cliente.Cidade);
            cmd.Parameters.AddWithValue("@cep", cliente.Cep);
            cmd.Parameters.AddWithValue("@codigocidade", cliente.CodigoCidade);
            cmd.Parameters.AddWithValue("@codigopais", cliente.CodigoPais);
            cmd.Parameters.AddWithValue("@loja", cliente.Loja);
            cmd.Parameters.AddWithValue("@uf", cliente.UF);
            cmd.Parameters.AddWithValue("@idunico", cliente.IdUnico);

            cmd.ExecuteNonQuery();

            transaction.Commit();


            if (_conexao.State == ConnectionState.Open)
            {
                _conexao.Close();
            }
        }

        public Cliente UpdateRetrieve(Cliente cliente)
        {
            if (_conexao.State == ConnectionState.Closed)
            {
                _conexao.Open();
            }

            SqlTransaction transaction = (SqlTransaction)_conexao.BeginTransaction();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE clientes SET nome=@nome, apelido=@apelido, nasc=@DataNascimento, cpf=@Cpf,"
                + " rg=@Rg, tipocli=@TipoCli, endereco=@endereco, numero=@numero, complemento=@complemento, bairro=@bairro,"
                + " cidade=@cidade, cep=@cep, codigocidade=@codigocidade, codigopais=@codigopais, loja=@loja, uf=@uf"
                + " WHERE idunico=@idunico";
            cmd.Connection = (SqlConnection)_conexao;
            cmd.Transaction = transaction;

            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@apelido", cliente.Apelido);
            cmd.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
            cmd.Parameters.AddWithValue("@Cpf", cliente.CPF);
            cmd.Parameters.AddWithValue("@Rg", cliente.RG);
            cmd.Parameters.AddWithValue("@TipoCli", cliente.TipoCli);
            cmd.Parameters.AddWithValue("@endereco", cliente.Endereco);
            cmd.Parameters.AddWithValue("@numero", cliente.Numero);
            cmd.Parameters.AddWithValue("@complemento", cliente.Complemento);
            cmd.Parameters.AddWithValue("@bairro", cliente.Bairro);
            cmd.Parameters.AddWithValue("@cidade", cliente.Cidade);
            cmd.Parameters.AddWithValue("@cep", cliente.Cep);
            cmd.Parameters.AddWithValue("@codigocidade", cliente.CodigoCidade);
            cmd.Parameters.AddWithValue("@codigopais", cliente.CodigoPais);
            cmd.Parameters.AddWithValue("@loja", cliente.Loja);
            cmd.Parameters.AddWithValue("@uf", cliente.UF);
            cmd.Parameters.AddWithValue("@idunico", cliente.IdUnico);

            cmd.ExecuteNonQuery();
            transaction.Commit();


            #region RetornaClienteAlterado
            SqlTransaction transaction2 = (SqlTransaction)_conexao.BeginTransaction();
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Transaction = transaction2;
            cmd2.CommandText = "SELECT idunico, codigo, nome, apelido, nasc, cpf, rg, tipocli, endereco, numero,complemento,"
                + "bairro, cidade, cep, codigocidade,codigopais, loja, uf FROM clientes WHERE codigo=@codigo";
            cmd2.Connection = (SqlConnection)_conexao;
            cmd2.Parameters.AddWithValue("@codigo", cliente.Codigo);
            SqlDataReader _reader2 = cmd2.ExecuteReader();
            transaction2.Commit();

            _reader2.Read();

            Cliente clienteRetrieved = new Cliente()
            {
                IdUnico = cliente.IdUnico,
                Codigo = cliente.Codigo,
                Nome = (string)((DBNull.Value == _reader2[2]) ? string.Empty : _reader2.GetString(2)),
                Apelido = (string)((DBNull.Value == _reader2[3]) ? string.Empty : _reader2.GetString(3)),
                DataNascimento = (DateTime)((DBNull.Value == _reader2[4]) ? DateTime.Now : _reader2.GetDateTime(4)),
                CPF = (string)((DBNull.Value == _reader2[5]) ? string.Empty : _reader2.GetString(5)),
                RG = (string)((DBNull.Value == _reader2[6]) ? string.Empty : _reader2.GetString(6)),
                TipoCli = (string)((DBNull.Value == _reader2[7]) ? string.Empty : _reader2.GetString(7)),
                Endereco = (string)((DBNull.Value == _reader2[8]) ? string.Empty : _reader2.GetString(8)),
                Numero = (string)((DBNull.Value == _reader2[9]) ? string.Empty : _reader2.GetString(9)),
                Complemento = (string)((DBNull.Value == _reader2[10]) ? string.Empty : _reader2.GetString(10)),
                Bairro = (string)((DBNull.Value == _reader2[11]) ? string.Empty : _reader2.GetString(11)),
                Cidade = (string)((DBNull.Value == _reader2[12]) ? string.Empty : _reader2.GetString(12)),
                Cep = (string)((DBNull.Value == _reader2[13]) ? string.Empty : _reader2.GetString(13)),
                CodigoCidade = (int)((DBNull.Value == _reader2[14]) ? -1 : _reader2.GetInt32(14)),
                CodigoPais = (int)((DBNull.Value == _reader2[15]) ? -1 : _reader2.GetInt32(15)),
                Loja = (int)((DBNull.Value == _reader2[16]) ? -1 : _reader2.GetInt32(16)),
                UF = (string)((DBNull.Value == _reader2[17]) ? string.Empty : _reader2.GetString(17))
            };

            if (_conexao.State == ConnectionState.Open)
            {
                _conexao.Close();
            }

            #endregion

            return clienteRetrieved;
        }

        public void Delete(int IdUnico)
        {
            if (_conexao.State == ConnectionState.Closed)
            {
                _conexao.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM clientes WHERE idunico=@IdUnico";
            cmd.Parameters.AddWithValue("@IdUnico", IdUnico);
            cmd.Connection = (SqlConnection)_conexao;
            cmd.ExecuteNonQuery();

            if (_conexao.State == ConnectionState.Open)
            {
                _conexao.Close();
            }

        }
    }
}