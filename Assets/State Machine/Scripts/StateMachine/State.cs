
/// <summary>
/// Classe de convenance pour ajouter des �tats � la machine plus facilement
/// </summary>
public abstract class State
{
    #region Variables d'instance

    /// <summary>
    /// Cet �tat est-il le premier de la hi�rarchie ?
    /// </summary>
    protected bool IsRootState { get; set; } = false;

    #endregion

    #region Fonctions prot�g�es

    /// <summary>
    /// Callback appel� quand on entre dans l'�tat
    /// </summary>
    protected virtual void Enter() { }

    /// <summary>
    /// Callback appel� quand on quitte l'�tat
    /// </summary>
    protected virtual void Exit() { }

    /// <summary>
    /// Callback appel� quand on m�j l'�tat dans une m�thode Update()
    /// </summary>
    protected virtual void Update() { }

    /// <summary>
    /// Callback appel� quand on m�j l'�tat dans une m�thode FixedUpdate()
    /// </summary>
    protected virtual void FixedUpdate() { }

    #endregion
}
