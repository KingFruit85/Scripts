using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public GameObject healthPotion;
    public GameObject arrows;
    public GameObject coins;
    private Vector2 epos;

    void Update()
    {
        epos = GetComponent<Transform>().position;
    }

    public void SpawnLoot()
    {
        var R = Random.Range(1,4);
        
        switch (R)
        {
            default: throw new System.Exception("not a valid number");
            
            case 1:
            Instantiate(healthPotion, epos, Quaternion.identity);
            break;

            case 2:
            Instantiate(arrows,epos, Quaternion.identity);
            break;

            case 3:
            Instantiate(coins,epos, Quaternion.identity);
            break;

        }
        
    }

}
