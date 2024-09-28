using Domain.Interfaces.Generics;
using Domain.Interfaces.ICategoria;
using Domain.Interfaces.InterfaceServicos;
using Entities.Entidades;

namespace Domain.Servicos
{
    public class CategoriaServico : ICategoriaServico, IInjectable
    {
        private readonly InterfaceCategoria _interfaceCategoria;

        public CategoriaServico(InterfaceCategoria interfaceCategoria)
        {
            _interfaceCategoria = interfaceCategoria;
        }

        public async Task AdicionarCategoria(Categoria categoria)
            => await _interfaceCategoria.Add(categoria);

        public async Task AtualizarCategoria(Categoria categoria)
            =>  await _interfaceCategoria.Update(categoria);

        public async Task<bool> Existe(int idCategoria)
            => await _interfaceCategoria.Exists(idCategoria);
    }
}
