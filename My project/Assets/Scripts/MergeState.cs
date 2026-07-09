using UnityEngine;

public class MergeState : IEnemyStates
{
    private EnemyController controller;
    public MergeState(EnemyController controller)
    {
        this.controller = controller;
    }
    public void Enter(EnemyController controller)
    {
        
    }
    public void Update()
    {
        
    }
    public void FixedUpdate()
    {
        controller.TryMarge();

        if(controller.mergeTarget != null && controller.mergeTarget.gameObject.activeInHierarchy)
        controller.GetMovent().MoveToward(controller.mergeTarget.transform.position);
    }
    public void Exit()
    {
        controller.GetMovent().ResetSpeed();
        controller.GetMovent().Stop();
    }
}
