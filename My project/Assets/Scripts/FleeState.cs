using UnityEngine;

public class FleeState : IEnemyStates
{
    private EnemyController controller;
    private Vector3 fleetarget;
    private float destroyDistance = 30f;

    public FleeState(EnemyController controller)
    {
        this.controller = controller ;
    }
    public void Enter(EnemyController controller)
    {
        controller.GetMovent().SetFleeSpeed();
    }
    public void Update()
    {
        
    }
    public void FixedUpdate()
    {
        if(Vector3.Distance(controller.transform.position, controller.FleeDestination) > controller.fleeStopDistance)
        controller.GetMovent().MoveToward(controller.FleeDestination);
        else 
        controller.GetMovent().Stop();
    }
    public void Exit()
    {
        controller.GetMovent().ResetSpeed();
    }
}
