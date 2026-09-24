using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CyberPolice.Common
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        // thread-safe колекція
        private readonly ConcurrentDictionary<Guid, T> _storage = new ConcurrentDictionary<Guid, T>();

        // примітив синхронізації для безпечного запису у файл
        private readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);

        public string FilePath { get; set; }

        public CrudServiceAsync(string filePath)
        {
            FilePath = filePath;
        }

        private Guid GetId(T element)
        {
            return (Guid)element.GetType().GetProperty("Id").GetValue(element);
        }

        public Task<bool> CreateAsync(T element)
        {
            var id = GetId(element);
            bool result = _storage.TryAdd(id, element);
            return Task.FromResult(result);
        }

        public Task<T> ReadAsync(Guid id)
        {
            _storage.TryGetValue(id, out var value);
            return Task.FromResult(value);
        }

        public Task<IEnumerable<T>> ReadAllAsync()
        {
            return Task.FromResult<IEnumerable<T>>(_storage.Values.ToList());
        }

        // пагінація
        public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var result = _storage.Values
                .Skip(page * amount)
                .Take(amount)
                .ToList();
            return Task.FromResult<IEnumerable<T>>(result);
        }

        public Task<bool> UpdateAsync(T element)
        {
            var id = GetId(element);
            _storage[id] = element;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveAsync(T element)
        {
            var id = GetId(element);
            bool result = _storage.TryRemove(id, out _);
            return Task.FromResult(result);
        }

        // асинхронне безпечне збереження у файл
        public async Task<bool> SaveAsync()
        {
            await _fileLock.WaitAsync();
            try
            {
                string json = JsonConvert.SerializeObject(_storage.Values.ToList());
                using (var writer = new StreamWriter(FilePath, false))
                {
                    await writer.WriteAsync(json);
                }
                return true;
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task<bool> LoadAsync()
        {
            await _fileLock.WaitAsync();
            try
            {
                if (!File.Exists(FilePath)) return false;
                string json;
                using (var reader = new StreamReader(FilePath))
                {
                    json = await reader.ReadToEndAsync();
                }
                var items = JsonConvert.DeserializeObject<List<T>>(json);
                _storage.Clear();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        var id = GetId(item);
                        _storage.TryAdd(id, item);
                    }
                }
                return true;
            }
            finally
            {
                _fileLock.Release();
            }
        }

        // реалізація IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            return _storage.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}