using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//Used to instantiate any type of class, including those not deriving from MonoBehaviour or ScriptableObject

namespace Project.Pool {



    [System.Serializable]
    public class ClassPooler<TBase> where TBase : class
    {

        [SerializeField] private Dictionary<string, ObjectPool<TBase>> poolDictionary;


        #region Constructors

        public ClassPooler(params (string key, int defaultCapacity, Func<TBase> createFunc)[] newPools)
        {
            poolDictionary = new Dictionary<string, ObjectPool<TBase>>(newPools.Length);
            AddPools(newPools);
        }


        private ObjectPool<TBase> CreatePool(int defaultCapacity, Func<TBase> createFunc)
        {

            ObjectPool<TBase> newPool = new ObjectPool<TBase>(
                createFunc: () => createFunc.Invoke(),
                actionOnGet: (obj) => Dequeue(obj),
                actionOnRelease: (obj) => Enqueue(obj),
                collectionCheck: true,
                defaultCapacity: defaultCapacity
                );

            return newPool;
        }


        public void AddPools(params (string key, int defaultCapacity, Func<TBase> createFunc)[] newPools)
        {
            for (int i = 0; i < newPools.Length; i++)
            {
                poolDictionary.Add(newPools[i].key, CreatePool(newPools[i].defaultCapacity, newPools[i].createFunc));
            }

        }


        public void RemovePools(params string[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (poolDictionary.ContainsKey(keys[i]))
                {
                    poolDictionary[keys[i]].Dispose();
                    poolDictionary.Remove(keys[i]);
                }
            }
        }

        #endregion





        #region Pooling

        private static void Dequeue(TBase obj)
        {
            //(IDequeued)obj causes an InvalidCastException if obj does not derive from the interface
            if (obj is IDequeued pooledObj)
            {
                pooledObj.OnDequeued();
            }
        }
        private static void Enqueue(TBase obj)
        {
            //(IEnqueued)obj causes an InvalidCastException if obj does not derive from the interface
            if (obj is IEnqueued pooledObj)
            {
                pooledObj.OnEnqueued();
            }
        }




        public TChild GetFromPool<TChild>(string key = null) where TChild : TBase, new()
        {

            if (key is null) key = typeof(TChild).Name;

            if (!poolDictionary.ContainsKey(key))
            {
                Debug.LogError($"Pooler Error : The key {key} does not exist.");
                return default;
            }

            TChild obj = (TChild)poolDictionary[key].Get();


            return obj;
        }

        public void ReturnToPool(TBase obj, string key = null)
        {
            if(key is null) key = obj.GetType().Name;

            if (!poolDictionary.ContainsKey(key))
            {
                Debug.LogError($"Pooler Error : The key {key} does not exist.");
                return;
            }

            poolDictionary[key].Release(obj);
        }

        #endregion

    }
}