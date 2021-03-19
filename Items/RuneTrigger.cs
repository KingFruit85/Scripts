using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneTrigger : MonoBehaviour
{
    public GameObject[] Room1Traps;

    void Awake()
    {
        GetComponent<Animator>().Play("runeTile");
    }

    public void Activate()
    {

        GetComponent<Animator>().Play("runeTileGlow");

        foreach (var trap in Room1Traps)
        {
            trap.GetComponent<ArrowTrap>().Activate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Activate();
        }
    }

}
