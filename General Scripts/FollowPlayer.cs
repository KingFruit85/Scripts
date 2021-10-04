using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    [SerializeField]
    private float smoothSpeed = 25f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private Vector3 offset = new Vector3(0,0,-1);
    Scene scene;
    
    public bool followPlayer = false;
    void Start()
    {
        // the PCG scene uses the roomtemplates script to set the player transform 
        // in the camera as the player spawn is slightly delayed, this is just
        // a work around for the shop level where we don't want the roomtemplates
        // script active
        if (followPlayer)
        {
            scene = SceneManager.GetActiveScene();
        
            if(scene.name == "shop")
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
        
    }

    void Update()
    {
        if (followPlayer)
        {
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
        
    }

    void LateUpdate()
    {
        if (followPlayer)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position,desiredPosition,
                                                        ref velocity,
                                                        smoothSpeed*Time.deltaTime);
            transform.position = smoothedPosition;
        }

        
    }
}
