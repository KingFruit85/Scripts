using System.Collections;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField]
    private float Intensity = 1.0f;
    private bool shakeIsActive;
    private Transform target;
    private Vector3 initialPOS;
    private float pendingShakeDuration = 0f;
    private bool isShaking = false;

    public bool manualShake { get; private set; }

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
        manualShake = true;
        if (duration > 0)
        {
            Intensity = intensity;
            pendingShakeDuration += duration;
        }
    }

    public void ActivateShake()
    {
        Intensity = 0.5f;
        shakeIsActive = true;
    }

    public void DeactivateShake()
    {
        shakeIsActive = false;
        manualShake = false;
    }

    void Update()
    {
        initialPOS = target.position;

        if (shakeIsActive)
        {
            pendingShakeDuration += 0.5f;
        }

        if (!shakeIsActive && !manualShake)
        {
            pendingShakeDuration = 0;
        }

        if (pendingShakeDuration > 0 && !isShaking)
        {
            StartCoroutine("DoShake");
        }
    }

    public void CombatShaker(string direction)
    {
        // StartCoroutine(MeleeShake(direction));
    }



    IEnumerator DoShake()
    {
        isShaking = true;

        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + pendingShakeDuration)
        {
            var randomPoint = new Vector3(Random.Range(-0.1f, 0.1f) * Intensity, Random.Range(-0.1f, 0.1f) * Intensity, 0
            );
            target.position += randomPoint;
            yield return null;
        }

        pendingShakeDuration = 0f;
        isShaking = false;
        manualShake = false;


    }

    IEnumerator MeleeShake(string direction)
    {
        isShaking = true;
        Vector3 point = new Vector3(0, 0);

        if (direction == "Left")
        {
            point = new Vector3(-0.01f * Intensity, +0.0f * Intensity);
        }
        else if (direction == "Right")
        {
            point = new Vector3(+0.01f * Intensity, +0.0f * Intensity);
        }
        else if (direction == "Up")
        {
            point = new Vector3(0.01f * Intensity, +-0.01f * Intensity);
        }
        else if (direction == "Down")
        {
            point = new Vector3(0.01f * Intensity, + +0.01f * Intensity);
        }

        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + 0.2f)
        {
            target.position += point;
            yield return null;
        }

        pendingShakeDuration = 0f;
        isShaking = false;
    }

}
