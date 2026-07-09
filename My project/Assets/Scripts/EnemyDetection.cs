using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyDetection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light flashlight;
    private EnemyController controller;
    private Transform player;

    [Header("Settings")]
    [SerializeField] private float detectionInterval = 0.2f;
    [SerializeField] private LayerMask obstacleMask;

    private WaitForSeconds wait;

    void Awake()
    {
        controller = GetComponent<EnemyController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if(flashlight == null)
        flashlight = player.GetComponentInChildren<Light>();
        wait = new WaitForSeconds(detectionInterval);
    }
    void OnEnable()
    {
        StartCoroutine(DetectionLoop());
    }
    IEnumerator DetectionLoop()
    {
        yield return wait;
        while (enabled)
        {
            if(flashlight != null && flashlight.enabled && !controller.HasSplit)
            CheckFlashlight();
            yield return wait;
        }
    }
    void CheckFlashlight()
    {
        Vector3 origin = flashlight.transform.position;
        Vector3 dir = (transform.position - origin).normalized;
        float dis = Vector3.Distance(origin, transform.position);
        if(dis > flashlight.range) return;

        float angle = Vector3.Angle(flashlight.transform.forward, dir);
        if(angle > flashlight.spotAngle * 0.5f) return;

         if (!Physics.SphereCast(origin, 0.5f, dir, out RaycastHit hit, dis, obstacleMask))
            controller.SplitAndFlee();
    }
}
