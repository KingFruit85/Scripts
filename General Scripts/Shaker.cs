using System.Collections;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [Range(0f,2f)][SerializeField]
    private float Intensity = 1.0f;
    private Transform target;
    private Vector3 initialPOS;
    private float pendingShakeDuration = 0f;
    private bool isShaking = false;

    void Start()
    {
        target = GetComponent<Transform>();
        initialPOS = target.localPosition;
    }

    public void Shake(float duration)
    {
        if (duration > 0)
        {
            pendingShakeDuration += duration;
        }
    }

    public void Shake(float duration, float intensity)
    {
        if (duration > 0)
        {
            Intensity = intensity;
            pendingShakeDuration += duration;
        }
    }

    void Update()
    {
        initialPOS = target.position;

        if (pendingShakeDuration > 0 && !isShaking)
        {
            StartCoroutine("DoShake");
        }
    }

    public void CombatShaker(string direction)
    {
        StartCoroutine(MeleeShake(direction));
    }



    IEnumerator DoShake()
    {
        isShaking = true;

        var startTime = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup < startTime + pendingShakeDuration)
        {
            var randomPoint = new Vector3(Random.Range(-0.1f,0.1f) * Intensity, Random.Range(-0.1f,0.1f)* Intensity, 0
            );
            target.position += randomPoint;
            yield return null;
        }

        pendingShakeDuration = 0f;
        isShaking = false;

    }

    IEnumerator MeleeShake(string direction)
    {
        isShaking = true;
        Vector3 point = new Vector3(0,0);

        if (direction == "Left")
        {
            point = new Vector3(-0.01f * Intensity, + 0.0f * Intensity);
        }
        else if (direction == "Right")
        {
            point = new Vector3(+0.01f * Intensity, + 0.0f * Intensity);
        }
        else if (direction == "Up")
        {
            point = new Vector3(0.01f * Intensity, + -0.01f * Intensity);
        }
        else if (direction == "Down")
        {
            point = new Vector3(0.01f * Intensity, + +0.01f * Intensity);
        }

        var startTime = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup < startTime + 0.2f)
        {
            target.position += point;
            yield return null;
        }

        pendingShakeDuration = 0f;
        isShaking = false;
    }

}
