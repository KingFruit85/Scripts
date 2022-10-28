using UnityEngine;
using UnityEngine.Tilemaps;

public class Sword : MonoBehaviour
{

void Start()
{
    //Stops the sword colliding with wall tiles
    Physics2D.IgnoreLayerCollision(13,17); // Walls
    Physics2D.IgnoreLayerCollision(13,18); // Bars
    Physics2D.IgnoreLayerCollision(13,8);  // Enemies
    Physics2D.IgnoreLayerCollision(13,20); // Table
    Physics2D.IgnoreLayerCollision(13,24); // Doors
    Physics2D.IgnoreLayerCollision(13,0); // Default

    
}

}
