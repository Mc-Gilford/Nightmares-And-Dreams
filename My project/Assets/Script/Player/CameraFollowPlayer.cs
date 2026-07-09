using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject player;
    private Vector3 offset = new Vector3(0,1,1);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void LateUpdate() {
        transform.position = player.transform.position+ offset;
        transform.rotation  = player.transform.rotation;

    }
}
