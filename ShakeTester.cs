using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTester : MonoBehaviour
{
    public Shaker Shaker;
    public float duration = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        Shaker.Shake(duration);

        }
    }
}
