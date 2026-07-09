using System.Collections;
using UnityEngine;
 using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int initialPoolSize;
    [SerializeField] private Transform[] fleePoints;
    private Queue<GameObject> pool = new Queue<GameObject>();
    public Transform[] GetFleePoints() => fleePoints;

    void Awake()
    {
        for(int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    public GameObject GetFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if(pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            EnemyController ctrl = obj.GetComponentInChildren<EnemyController>();
            if(ctrl != null)
            {
               ctrl.SetPool(this);
               if(fleePoints != null && fleePoints.Length > 0)
               ctrl.SetFleePoints(fleePoints);
            }
        }
        else
        {
            if(enemyPrefab == null)
            {
                Debug.LogError("EnemyPrefab no asignado en EnemyPool", this);
            return null;

            }
            obj = Instantiate(enemyPrefab, position, rotation);
        }
        return obj;
    }
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
