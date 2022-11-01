using System;

namespace Project.Pool
{
    /// <summary>
    /// Interface covariante pour permettre d'assigner n'importe quel type héritant de T
    /// </summary>
    /// <typeparam name="T">Le type de base à cloner</typeparam>
    public interface IPool<out T>
    {
        public string Key { get; }
        public int DefaultCpapcity { get; }
        public Func<T> CreateFunc { get; }
    }

    /// <summary>
    /// Appelée quand un élément retourne dans la réserve
    /// </summary>
    public interface IEnqueued
    {
        void OnEnqueued();
    }

    /// <summary>
    /// Appelée quand un élément quitte la réserve
    /// </summary>
    public interface IDequeued
    {
        void OnDequeued();
    }
}