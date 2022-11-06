using DinnamuSWebApi.Data.Vendas;
using DinnamuSWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinnamuSWebApi.Repositories.Vendas
{
    public interface ICotacaoRepository
    {
        List<DadosOrc> cotacoes();

        DadosOrc cotacaoByCodigo(long codigo);

        void Inserir(DadosOrc cotacao);

        void InserirItemNaCotacao(ItemOrc item);

    }
}
