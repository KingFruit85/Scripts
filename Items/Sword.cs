using UnityEngine;
using UnityEngine.Tilemaps;

public class Sword : MonoBehaviour
{

void Start()
{
    //Stops the sword colliding with wall tiles
    Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(),GameObject.Find("Walls").GetComponent<TilemapCollider2D>());
}

}
