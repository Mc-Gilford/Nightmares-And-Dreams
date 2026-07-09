using UnityEngine;

public interface IEnemyStates
{
    void Enter(EnemyController controller);
    void Update();
    void FixedUpdate();
    void Exit();
}
