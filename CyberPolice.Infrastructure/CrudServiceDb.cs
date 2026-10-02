using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CyberPolice.Infrastructure
{
    public class CrudServiceDb<T> : ICrudServiceAsyncDb<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public CrudServiceDb(IRepository<T> repository)
        {
            _repository = repository;
        }

        public Task<bool> CreateAsync(T element)
        {
            return _repository.AddAsync(element).ContinueWith(t => true);
        }

        public Task<T> ReadAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<IEnumerable<T>> ReadAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var all = await _repository.GetAllAsync();
            return all.Skip(page * amount).Take(amount);
        }

        public Task<bool> UpdateAsync(T element)
        {
            return _repository.Update(element).ContinueWith(t => true);
        }

        public Task<bool> RemoveAsync(T element)
        {
            return _repository.Delete(element).ContinueWith(t => true);
        }
    }
}