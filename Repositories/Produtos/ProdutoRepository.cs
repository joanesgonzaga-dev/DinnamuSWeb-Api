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

        public ProdutoRepository()
        {
            string _con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _connection = new SqlConnection(_con);
            
        }

        public List<Produto> Get()
        {
            
            try
            {
                SqlCommand command = new SqlCommand();

                string query = "SELECT cp.chaveunica, cp.nome, cp.codigonbn, cp.unidade, cp.ref, cp.codforn, datacad, igp.chaveunica , igp.codigo, igp.tamanho, ef.codigoproduto, ef.estoque FROM cadproduto cp LEFT OUTER JOIN itensgradeproduto igp ON cp.chaveunica = igp.codigo LEFT JOIN estoquefilial ef ON igp.chaveunica = ef.codigoproduto WHERE cp.nome IS NOT NULL";

                command.CommandText = query;
                command.Connection = (SqlConnection)_connection;
                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                Dictionary<int, Produto> produtos = new Dictionary<int, Produto>();
          

                while (reader.Read())
                {
                    Produto produto = new Produto();
                    
                        if(!produtos.ContainsKey(reader.GetInt32(0)))
                        {
                            produto.Chaveunica = reader.GetInt32(0);
                            produto.Nome = (string)((DBNull.Value == reader[1]) ? string.Empty : reader.GetString(1));
                            produto.CodigoNBN = (string)((DBNull.Value == reader[2]) ? string.Empty : reader.GetString(2));
                            produto.Unidade = (string)((DBNull.Value == reader[3]) ? string.Empty : reader.GetString(3));
                            produto.Referencia = (string)((DBNull.Value == reader[4]) ? string.Empty : reader.GetString(4));
                            produto.CodForn = (string)((DBNull.Value == reader[5]) ? string.Empty : reader.GetString(5));
                            produto.DataCadastro = ((DBNull.Value == reader[6]) ? "01/01/1900" : reader.GetDateTime(6).ToShortDateString());

                            produtos.Add(produto.Chaveunica, produto);
                        }
                        else
                        {
                            produto = produtos[reader.GetInt32(0)];
                        }

                        ItensGradeProduto item = new ItensGradeProduto()
                        {
                            ChaveUnica = (DBNull.Value == reader[7]) ? 0 : reader.GetInt32(7),
                            CodigoProduto = (DBNull.Value == reader[8]) ? 0 : reader.GetInt32(8),
                            Tamanho = (string)((DBNull.Value == reader[9]) ? string.Empty : reader.GetString(9))
                        };

                        produto.ItensGrade = (produto.ItensGrade == null) ? new List<ItensGradeProduto>() : produto.ItensGrade;
                        produto.ItensGrade.Add(item);
                }

                return produtos.Values.ToList();
            }

            catch (SqlException ex)
            {
                Debug.Write(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return null;
            }
            finally
            {
                _connection.Close();
            }
        }

        public Produto Get(int chaveunica)
        {
            throw new NotImplementedException();
        }

        public void Insert(Produto produto)
        {
            throw new NotImplementedException();
        }

        public void Update(Produto produto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int chaveunica)
        {
            throw new NotImplementedException();
        }
    }
}