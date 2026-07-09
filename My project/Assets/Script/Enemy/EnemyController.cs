using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyStateType
{
    Patrol,
    Chase,
    Flee,
    Merge
}

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyMovement followPlayer;
    private Transform player;
    
    [Header("Settings")]
    [SerializeField] private float chaseRange;
    [SerializeField] private float loseRange;
    [SerializeField] private float PatrolRadius;

    [Header("Merge Settings")]
    [SerializeField] private float mergeDelay;
    [SerializeField] private float mergeRange;

    [Header("Gizmos")]
    [SerializeField] private Color chaseGizmoColor = new Color(1, 1,0,0.3f);
    [SerializeField] private Color loseGizmoColor = new Color(1, 0,0,0.3f);

    public EnemyController mergeTarget;
    public EnemyController originalController;
    private Coroutine mergeCoroutine;

    private Transform[] cachedFleePoints;
    public float fleeStopDistance = 0.01f;

    public Vector3 FleeDestination{ get; set;}

    public bool HasSplit { get; set;}
    [SerializeField] private EnemyPool enemyPool;
    

    private Dictionary<EnemyStateType, IEnemyStates> states;
    public IEnemyStates currentState {get; private set;}


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        states = new Dictionary<EnemyStateType, IEnemyStates>
       {
            { EnemyStateType.Patrol, new PatrolState(this) },
            { EnemyStateType.Chase, new ChaseState(this) },
            { EnemyStateType.Flee, new FleeState(this) },
            { EnemyStateType.Merge, new MergeState(this) }
        };
        ChangeState(EnemyStateType.Chase);
    }
    void Update()
    {
        currentState?.Update();
    }
    void FixedUpdate() => currentState?.FixedUpdate();
    
    public void ChangeState(EnemyStateType newState)
    {
        currentState?.Exit();
        currentState = states[newState];
        currentState.Enter(this);
    }
    public Transform GetPlayer() => player;
    public EnemyMovement GetMovent() => followPlayer;
    public void SetPool(EnemyPool pool) => enemyPool = pool;
    public void SetFleePoints(Transform[] points) => cachedFleePoints = points;
    public Vector3 GetRandomPatrolPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * PatrolRadius;
        randomDir.y = 0;
        return transform.position + randomDir;
    }
    /*private void EvaluateTransitions()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if(currentState is PatrolState && dist <= chaseRange)
        ChangeState(EnemyStateType.Chase);
        else if(currentState is ChaseState && dist > loseRange)
        ChangeState(EnemyStateType.Patrol);
    }*/

    void OnDrawGizmos()
    {
        Gizmos.color = chaseGizmoColor;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = loseGizmoColor;
        Gizmos.DrawSphere(transform.position, loseRange);
    }
    public void SplitAndFlee()
    {
        
    HasSplit = true;
    Vector3 offset = transform.right * 2f;

    if(cachedFleePoints == null || cachedFleePoints.Length == 0)
    {
        EnemyPool pool = FindFirstObjectByType<EnemyPool>();
        if(pool != null)
           cachedFleePoints = pool.GetFleePoints();
    }

        int idx1 = Random.Range(0, cachedFleePoints.Length);
        int idx2;
        do { idx2 = Random.Range(0, cachedFleePoints.Length); }
        while (idx2 == idx1);

        EnemyController ctrl1 = null, ctrl2 = null;

        GameObject copy1 = enemyPool.GetFromPool(transform.position + offset, transform.rotation);
        if(copy1 != null)
        {
           ctrl1 = copy1.GetComponentInChildren<EnemyController>();
           ctrl1.HasSplit = true;
           ctrl1.FleeDestination = cachedFleePoints[idx1].position;
           ctrl1.ForceFlee();
        }
        
        GameObject copy2 = enemyPool.GetFromPool(transform.position - offset, transform.rotation);
        if(copy2 != null)
        {
            ctrl2 = copy2.GetComponentInChildren<EnemyController>();
            ctrl2.HasSplit = true;
            ctrl2.FleeDestination = cachedFleePoints[idx2].position;
            ctrl2.ForceFlee();
        }

        if(ctrl1 != null && ctrl2 != null)
        {
            ctrl1.OnSplit(ctrl2, this);
            ctrl2.OnSplit(ctrl1, this);
        }

        gameObject.SetActive(false);
    }

    public void OnSplit(EnemyController otherCopy, EnemyController original)
    {
        mergeTarget = otherCopy;
        originalController = original;
        mergeCoroutine = StartCoroutine(WaitAndMarge());
    }
    private IEnumerator WaitAndMarge()
    {
        yield return new WaitForSeconds(mergeDelay);
        if(mergeTarget != null && mergeTarget.gameObject.activeInHierarchy)
        ChangeState(EnemyStateType.Merge);
        else
        {
            HasSplit = false;
            originalController = null;
            mergeTarget = null;
            ChangeState(EnemyStateType.Patrol);
        }
    }
    public void TryMarge()
    {
        if(mergeTarget == null || !mergeTarget.gameObject.activeInHierarchy)
        {
            ReturnToPool();
            return;
        }
        
        float dist = Vector3.Distance(transform.position, mergeTarget.transform.position);
        if(dist > mergeRange) return;

        if(originalController != null && !originalController.gameObject.activeInHierarchy)
        {
            originalController.transform.position = (transform.position + mergeTarget.transform.position) * 0.5f;
            originalController.HasSplit = false;
            originalController.gameObject.SetActive(true);
            originalController.ChangeState(EnemyStateType.Chase);
        }
        ReturnToPool();
        
    }
    public void ForceFlee()
    {
        ChangeState(EnemyStateType.Flee);
    }
    public void ReturnToPool()
    {
        followPlayer.ResetSpeed();
        if(mergeCoroutine != null)
        StopCoroutine(mergeCoroutine);

        HasSplit = false;
        mergeTarget = null;
        originalController = null;
        mergeCoroutine = null;
        gameObject.SetActive(false);
        enemyPool.ReturnToPool(gameObject);
    }

}
