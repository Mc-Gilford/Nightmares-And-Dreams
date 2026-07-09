using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
  private NavMeshAgent agent;
  [SerializeField] private float SpeedMove;
  [SerializeField] private float fleeSpeed;

  private float baseSpeed;
  private float baseAcceleration;
  private float baseAngularSpeed;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = SpeedMove;
        baseSpeed = agent.speed;
        baseAcceleration = agent.acceleration;
        baseAngularSpeed = agent.angularSpeed;
    }

    public void MoveToward(Vector3 targetPosition)
    {
        agent.isStopped = false;
        agent.SetDestination(targetPosition);
    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }
    public void SetFleeSpeed()
    {
        float ratio = fleeSpeed / Mathf.Max(baseSpeed, 0.01f);
        agent.speed = fleeSpeed;
        agent.acceleration = baseAcceleration * ratio;
        agent.angularSpeed = baseAngularSpeed * ratio;   
    }
    public void ResetSpeed()
    {
        agent.speed = baseSpeed;
        agent.acceleration = baseAcceleration;
        agent.angularSpeed = baseAngularSpeed;
    }

}
