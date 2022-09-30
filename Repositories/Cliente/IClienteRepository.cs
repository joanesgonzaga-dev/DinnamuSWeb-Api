using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinnamuSWebApi.Models;

namespace DinnamuSWebApi.Repositories
{
    public interface IClienteRepository
    {
        List<Cliente> GetList();
        Cliente GetById(long IdUnico);
        List<Cliente> GetByName(string nome);
        void Insert(Cliente cliente);
        Cliente InsertRetrieve(Cliente cliente);
        void Update(Cliente cliente);
        Cliente UpdateRetrieve(Cliente cliente);
        void Delete(int IdUnico);
    }
}
