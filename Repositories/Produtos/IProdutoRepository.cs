using DinnamuSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinnamuSWebApi.Repositories.Produtos
{
    public interface IProdutoRepository
    {
        List<Produto> Get();
        Produto Get(int chaveunica);
        void Insert(Produto produto);
        void Update(Produto produto);
        void Delete(int chaveunica);
    }
}
