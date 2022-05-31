using System;

namespace Project.Pool
{
    public interface IPool<out T>
    {
        public string Key { get; }
        public int DefaultCpapcity { get; }
        public Func<T> CreateFunc { get; }
    }


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