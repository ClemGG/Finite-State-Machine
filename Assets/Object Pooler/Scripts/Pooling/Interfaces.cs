using System;

namespace Project.Pool
{
    /// <summary>
    /// Interface covariante pour permettre d'assigner n'importe quel type h�ritant de T
    /// </summary>
    /// <typeparam name="T">Le type de base � cloner</typeparam>
    public interface IPool<out T>
    {
        public string Key { get; }
        public int DefaultCpapcity { get; }
        public Func<T> CreateFunc { get; }
    }

    /// <summary>
    /// Appel�e quand un �l�ment retourne dans la r�serve
    /// </summary>
    public interface IEnqueued
    {
        void OnEnqueued();
    }

    /// <summary>
    /// Appel�e quand un �l�ment quitte la r�serve
    /// </summary>
    public interface IDequeued
    {
        void OnDequeued();
    }
}