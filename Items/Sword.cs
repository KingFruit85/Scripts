using UnityEngine;
using UnityEngine.Tilemaps;

public class Sword : MonoBehaviour
{

void Start()
{
    //Stops the sword colliding with wall tiles
    Physics2D.IgnoreLayerCollision(13,17);
    Physics2D.IgnoreLayerCollision(13,18);
    Physics2D.IgnoreLayerCollision(13,8);
    Physics2D.IgnoreLayerCollision(13,20);
}


}
