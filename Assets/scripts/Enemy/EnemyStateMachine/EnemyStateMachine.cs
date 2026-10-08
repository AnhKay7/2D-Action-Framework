using UnityEngine;

public class EnemyStateMachine
{

    public EnemyState CurrentState { get; private set; }

    public void Initialize(EnemyState initialState)
    {
        CurrentState = initialState;
        CurrentState.EnterState();
    }

    public void ChangeState(EnemyState nextState)
    {
        if (CurrentState == nextState)
            return;

        CurrentState?.ExitState();

        CurrentState = nextState;
        CurrentState.EnterState();
    }
}
