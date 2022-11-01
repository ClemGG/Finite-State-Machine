using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Project.Pool
{
    /// <summary>
    /// R�serve clonant l'objet et appelant ses interfaces
    /// � son activation et d�sactivation.
    /// Peut instancier tout type de class, y compris celles n'h�ritant pas
    /// de MonoBehaviour ou ScriptableObject.
    /// </summary>
    /// <typeparam name="TBase">Le type de base dont les pools pourront contenir des types h�rit�s.</typeparam>
    [System.Serializable]
    public class ClassPooler<TBase> where TBase : class
    {
        #region Variables d'instance

        /// <summary>
        /// Le dictionnaire contenant toutes les ObjectPools
        /// </summary>
        private Dictionary<string, ObjectPool<TBase>> _poolDictionary;

        #endregion

        #region Constructeurs

        public ClassPooler(params IPool<TBase>[] newPools)
        {
            _poolDictionary = new Dictionary<string, ObjectPool<TBase>>(newPools.Length);
            AddPools(newPools);
        }

        #endregion

        #region Fonctions publiques

        /// <summary>
        /// Ajoute une nouvelle ObjectPool pour chaque IPool dans la liste.
        /// </summary>
        /// <param name="newPools">Les pools � convertir en ObjectPool</param>
        public void AddPools(params IPool<TBase>[] newPools)
        {
            for (int i = 0; i < newPools.Length; i++)
            {
                _poolDictionary.Add(newPools[i].Key, CreatePool(newPools[i].DefaultCpapcity, newPools[i].CreateFunc));
            }

        }

        /// <summary>
        /// Supprime les ObjectPools aux adresses renseign�es
        /// </summary>
        /// <param name="keys">Les cl�s correspondant aux entr�es des ObjectPools � supprimer</param>
        public void RemovePools(params string[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (_poolDictionary.ContainsKey(keys[i]))
                {
                    _poolDictionary[keys[i]].Dispose();
                    _poolDictionary.Remove(keys[i]);
                }
            }
        }

        /// <summary>
        /// Utilise un clone dans une instance "using" qui appelle IDisposable imm�diatement
        /// apr�s la fin du "using"
        /// </summary>
        /// <typeparam name="TChild">Le type du clone � r�cup�rer</typeparam>
        /// <param name="key">L'adresse de l'ObjectPool contenant le <typeparamref name="TChild"/></param>
        /// <returns>Une version IDisposable du clone</returns>
        public PooledObject<TChild> UsingFromPool<TChild>(string key = null) where TChild : class, TBase
        {

            if (key is null) key = typeof(TChild).Name;

            if (!_poolDictionary.ContainsKey(key))
            {
                //Debug.LogError($"Pooler Error : The key '{key}' does not exist.");
                return default;
            }

            ObjectPool<TChild> pool = _poolDictionary[key] as ObjectPool<TChild>;

            return new PooledObject<TChild>(pool.Get(), pool);
        }

        /// <summary>
        /// Retourne un clone du type en param�tre via son adresse "<paramref name="key"/>" dans le dictionnaire
        /// </summary>
        /// <typeparam name="TChild">Le type du clone � r�cup�rer</typeparam>
        /// <param name="key">L'adresse de l'ObjectPool contenant le <typeparamref name="TChild"/></param>
        /// <returns>Une instance du clone</returns>
        public TChild GetFromPool<TChild>(string key = null) where TChild : TBase
        {

            if (key is null) key = typeof(TChild).Name;

            if (!_poolDictionary.ContainsKey(key))
            {
                //Debug.LogError($"Pooler Error : The key '{key}' does not exist.");
                return default;
            }

            return (TChild)_poolDictionary[key].Get();
        }

        /// <summary>
        /// Renvoie un clone dans son ObjectPool correspondante une fois utilis�
        /// </summary>
        /// <param name="obj">Le clone � renvoyer</param>
        /// <param name="key">L'adresse de son ObjectPool dans le dictionnaire</param>
        public void ReturnToPool(TBase obj, string key = null)
        {
            if (key is null) key = obj.GetType().Name;

            if (!_poolDictionary.ContainsKey(key))
            {
                //Debug.LogError($"Pooler Error : The key '{key}' does not exist.");
                return;
            }

            _poolDictionary[key].Release(obj);
        }

        #endregion

        #region Fonctions priv�es

        /// <summary>
        /// Cr�e un ObjectPool � partir de <typeparamref name="TBase"/>.
        /// </summary>
        /// <param name="defaultCapacity">La taille de base de la liste contenant les clones.</param>
        /// <param name="createFunc">La fonction retournant un clone de <typeparamref name="TBase"/>.</param>
        /// <returns>Un ObjectPool ajout� au dictionnaire du ClassPooler.</returns>
        private ObjectPool<TBase> CreatePool(int defaultCapacity, Func<TBase> createFunc)
        {
            ObjectPool<TBase> newPool = new
                (
                    createFunc: () => createFunc.Invoke(),
                    actionOnGet: (obj) => Dequeue(obj),
                    actionOnRelease: (obj) => Enqueue(obj),
                    collectionCheck: false,
                    defaultCapacity: defaultCapacity
                );

            return newPool;
        }

        /// <summary>
        /// Cr�e un clone (ou en sort un existant de l'ObjectPool) et appele sa fonction d'activation
        /// </summary>
        /// <param name="obj">L'objet � activer</param>
        private static void Dequeue(TBase obj)
        {
            // (IDequeued)obj causes an InvalidCastException if obj does not derive from the interface
            switch (obj)
            {
                case IDequeued pooledObj:
                    pooledObj.OnDequeued();
                    break;

                case GameObject go:
                    IDequeued[] components = go.GetComponents<IDequeued>();
                    for (int i = 0; i < components.Length; i++)
                    {
                        components[i].OnDequeued();
                    }
                    break;
            }
        }

        /// <summary>
        /// Cr�e un clone (ou en sort un existant de l'ObjectPool) et appele sa fonction de d�sactivation
        /// </summary>
        /// <param name="obj">L'objet � activer</param>
        private static void Enqueue(TBase obj)
        {
            // (IEnqueued)obj causes an InvalidCastException if obj does not derive from the interface
            switch (obj)
            {
                case IEnqueued pooledObj:
                    pooledObj.OnEnqueued();
                    break;

                case GameObject go:
                    IEnqueued[] components = go.GetComponents<IEnqueued>();
                    for (int i = 0; i < components.Length; i++)
                    {
                        components[i].OnEnqueued();
                    }
                    break;
            }
        }

        #endregion
    }
}