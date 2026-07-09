using UnityEngine;

public class PatrolState : IEnemyStates
{
   private EnemyController controllerEnemy;
   private Vector3 targetPoint;

   public PatrolState(EnemyController controller)
    {
        this.controllerEnemy = controller;
    }
    public void Enter(EnemyController controller)
    {
        targetPoint = controllerEnemy.GetRandomPatrolPoint();
    }
    public void Update()
    {
        
    }
    public void FixedUpdate()
    {
        if(Vector3.Distance(controllerEnemy.transform.position, targetPoint) < 1f) targetPoint = controllerEnemy.GetRandomPatrolPoint();
        controllerEnemy.GetMovent().MoveToward(targetPoint);
    }
    public void Exit()
    {
        
    }

}
