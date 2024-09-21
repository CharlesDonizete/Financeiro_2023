using Domain.Interfaces.Generics;
using Entities.Entidades;
using Infra.Configuracao;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Infra.Repositorio.Generics
{
    public class RepositoryGenerics<T> : InterfaceGeneric<T>, IDisposable where T : Base
    {
        private readonly ContextBase _context;

        public RepositoryGenerics(ContextBase context)
        {
            _context = context;
        }

        public async Task Add(T objeto)
        {
            _context.Set<T>().Add(objeto);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T objeto)
        {
            _context.Set<T>().Remove(objeto);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> GetEntityById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> List()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Set<T>().AnyAsync(x => x.Id == id);
        }

        public async Task Update(T objeto)
        {
            _context.Set<T>().Update(objeto);
            await _context.SaveChangesAsync();
        }

        #region Disposed https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
        #endregion
    }
}
