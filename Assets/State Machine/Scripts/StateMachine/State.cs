
/// <summary>
/// Convenience class to add states to the factory more easily
/// </summary>
public abstract class State
{
    protected bool _isRootState { get; set; } = false;

    protected virtual void Enter() { }
    protected virtual void Exit() { }
    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }


}
