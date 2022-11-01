using System;

namespace Project.Pool
{
    /// <summary>
    /// Conteneur pour le type de classe � cloner
    /// </summary>
    /// <typeparam name="T">Un type � cloner</typeparam>
    public class Pool<T> : IPool<T> where T : class
    {
        public string Key { get; }
        public int DefaultCpapcity { get; }
        public Func<T> CreateFunc { get; }

        public Pool(string key, int defaultCapacity, Func<T> createFunc)
        {
            Key = key;
            DefaultCpapcity = defaultCapacity;
            CreateFunc = createFunc;
        }
    }
}