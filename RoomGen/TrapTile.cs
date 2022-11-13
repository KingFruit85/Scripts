using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : MonoBehaviour
{
    public GameObject MyTrap;
    public Sprite defaultSprite;
    public Sprite triggeredSprite;
    public SpriteRenderer sr;
    public AudioClip triggered;
    public bool trapTriggered = false;
    

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = defaultSprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !trapTriggered)
        {
            // Fire arrow
            MyTrap.GetComponent<ArrowTrap>().ShootAtSpecificLocation(transform.position);
            // Play triggered sound
            GameObject.FindObjectOfType<AudioManager>().PlayAudioClip(triggered);
            // change sprite to tripped
            sr.sprite = triggeredSprite;
            trapTriggered = true;
        }
    }
}
