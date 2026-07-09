using JetBrains.Annotations;
using UnityEngine;

public class ChaseState : IEnemyStates
{
   private EnemyController controller;

   public ChaseState(EnemyController controller)
    {
        this.controller = controller;
    }
    public void Enter(EnemyController controller)
    {

    }
    public void Enter()
    {
        
    }
    public void Update()
    {
        
    }
    public void FixedUpdate()
    {
        controller.GetMovent().MoveToward(controller.GetPlayer().position);
    }
    public void Exit()
    {
        
    }
}
