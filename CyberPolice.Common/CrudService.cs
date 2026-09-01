using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CyberPolice.Common
{
    public class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly Dictionary<Guid, T> _storage = new Dictionary<Guid, T>();

        public void Create(T element)
        {
            Guid id = (Guid)element.GetType().GetProperty("Id").GetValue(element);
            _storage[id] = element;
        }

        public T Read(Guid id)
        {
            return _storage[id];
        }

        public IEnumerable<T> ReadAll()
        {
            return _storage.Values;
        }

        public void Update(T element)
        {
            Guid id = (Guid)element.GetType().GetProperty("Id").GetValue(element);
            _storage[id] = element;
        }

        public void Remove(T element)
        {
            Guid id = (Guid)element.GetType().GetProperty("Id").GetValue(element);
            _storage.Remove(id);
        }

        public void Save(string filePath)
        {
            string json = JsonConvert.SerializeObject(_storage.Values.ToList());
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<T> items = JsonConvert.DeserializeObject<List<T>>(json);
            _storage.Clear();
            if (items != null)
            {
                foreach (T item in items)
                    Create(item);
            }
        }
    }
}