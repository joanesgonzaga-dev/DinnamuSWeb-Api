using DinnamuSWebApi.Models;
using DinnamuSWebApi.Repositories.Produtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DinnamuS_API.Repositories.Produtos
{
    public class ProdutoRepository : IProdutoRepository
    {
        private IDbConnection _connection;

        public ProdutoRepository()
        {
            _connection = new SqlConnection("Server=MAFIA;DATABASE=Principal;User ID=sa;Password=sa");
        }

        public void Delete(int chaveunica)
        {
            throw new NotImplementedException();
        }

        public List<Produto> Get()
        {
            try
            {
                SqlCommand command = new SqlCommand();

                string query = "SELECT cp.chaveunica, cp.nome, cp.codigonbn, igp.chaveunica , igp.codigo, igp.tamanho, ef.codigoproduto, ef.estoque FROM cadproduto cp LEFT OUTER JOIN itensgradeproduto igp ON cp.chaveunica = igp.codigo LEFT JOIN estoquefilial ef ON igp.chaveunica = ef.codigoproduto WHERE cp.nome IS NOT NULL";

                command.CommandText = query;
                command.Connection = (SqlConnection)_connection;
                _connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                Dictionary<int, Produto> produtos = new Dictionary<int, Produto>();
          
                while (reader.Read())
                {
                    Produto produto = new Produto();

                    if (!produtos.ContainsKey(reader.GetInt32(0)))
                    {
                        produto.Chaveunica = reader.GetInt32(0);
                        produto.Nome = reader.GetString(1);
                        produto.CodigoNBN = (string)((DBNull.Value == reader[2]) ? string.Empty : reader.GetString(2));
                        produtos.Add(produto.Chaveunica, produto);
                    }
                    else
                    {
                        produto = produtos[reader.GetInt32(0)]; //Atribuição necessária devido ao new Produto() realizado no inicio do While
                    }

                    ItensGradeProduto item = new ItensGradeProduto()
                    {
                        ChaveUnica = (DBNull.Value == reader[3]) ? 0 : reader.GetInt32(3),
                        CodigoProduto = (DBNull.Value == reader[4]) ? 0 : reader.GetInt32(4),
                        Tamanho = (string)((DBNull.Value == reader[5]) ? string.Empty : reader.GetString(5))
                    };

                    produto.ItensGrade = (produto.ItensGrade == null) ? new List<ItensGradeProduto>() : produto.ItensGrade;
                    produto.ItensGrade.Add(item);
                }

                return produtos.Values.ToList();
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
    }
}
