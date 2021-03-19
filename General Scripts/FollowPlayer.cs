using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    [SerializeField]
    private float smoothSpeed = 25f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private Vector3 offset = new Vector3(0,0,-1);

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position,desiredPosition,
                                                      ref velocity,
                                                      smoothSpeed*Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
