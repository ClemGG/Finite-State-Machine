
/// <summary>
/// Classe de convenance pour ajouter des états à la machine plus facilement
/// </summary>
public abstract class State
{
    #region Variables d'instance

    /// <summary>
    /// Cet état est-il le premier de la hiérarchie ?
    /// </summary>
    protected bool IsRootState { get; set; } = false;

    #endregion

    #region Fonctions protégées

    /// <summary>
    /// Callback appelé quand on entre dans l'état
    /// </summary>
    protected virtual void Enter() { }

    /// <summary>
    /// Callback appelé quand on quitte l'état
    /// </summary>
    protected virtual void Exit() { }

    /// <summary>
    /// Callback appelé quand on màj l'état dans une méthode Update()
    /// </summary>
    protected virtual void Update() { }

    /// <summary>
    /// Callback appelé quand on màj l'état dans une méthode FixedUpdate()
    /// </summary>
    protected virtual void FixedUpdate() { }

    #endregion
}
