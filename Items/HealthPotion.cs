using System;
using UnityEngine;
public class HealthPotion : MonoBehaviour
{
    [SerializeField]
    private int HealthAmount = 50;

    public void SetHealthAmount(int value)
    {
        HealthAmount = value;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var player = other.gameObject.GetComponent<Health>();
            player.AddHealth(HealthAmount); 
            Destroy(gameObject);
        }
    }
}
