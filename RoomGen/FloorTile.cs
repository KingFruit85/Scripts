using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    private RoomSprites roomSprites;
    [SerializeField]
    private int currentGameLevel;
    private SpriteRenderer sr;
    private BoxCollider2D col;

    private GameManager gameManager;

    // public bool leftDetector = false;
    // public bool rightDetector = false;
    // public bool upDetector = false;
    // public bool downDetector = false;

    public bool isTouchingOtherCollider = false;

    public Sprite defaultSprite;
    public Color defaultColor;

    public Sprite critSprite;

    public List<GameObject> objectsAroundMe = new List<GameObject>();

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        currentGameLevel = GameObject.Find("GameManager").GetComponent<GameManager>().currentGameLevel;
        roomSprites = GameObject.Find("Room Sprites").GetComponent<RoomSprites>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        var r = Random.Range(0,roomSprites.floor.Length - 1);
        
        sr.sprite = roomSprites.floor[r];
        defaultSprite = sr.sprite;
        
        r = Random.Range(0, roomSprites.critFloor.Length -1);

        critSprite = roomSprites.critFloor[r];

        SetFloorTileColor();
              
    }

    private IEnumerator CritFlash(float duration)
    {
        sr.sprite = critSprite;
        sr.color = Color.white;
        yield return new WaitForSeconds( duration );
        sr.color = defaultColor;
        sr.sprite = defaultSprite;
    }

    public void SetFloorTileColor()
    {
        // sr.color = gameManager.LevelFloorBaseColor;
        sr.color = Color.white;
        defaultColor = sr.color;
    }

    public void CriticalFlash()
    {
        StartCoroutine(CritFlash(.1f));
    }

    void Update()
    {
        if (gameManager.playerHit)
        {
            CriticalFlash();
        }

        // Debug.DrawRay(new Vector2(transform.position.x,transform.position.y + .6f), new Vector2(0,.2f), Color.green,.2f);
        // Debug.DrawRay(new Vector2(transform.position.x,transform.position.y - .6f), new Vector2(0, - .2f), Color.red,.2f);
        // Debug.DrawRay(new Vector2(transform.position.x - .6f,transform.position.y), new Vector2(-.2f,0), Color.blue,.2f);
        // Debug.DrawRay(new Vector2(transform.position.x  + .6f,transform.position.y), new Vector2(.2f,0), Color.yellow,.2f);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col)
        {
            isTouchingOtherCollider = true; 
        }
    }

    // objectsAroundMe isn't working properly
    // void FixedUpdate()
    // {
        
    //     if (!upDetector)
    //     {
    //         RaycastHit2D upHit = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y + .6f), new Vector2(0,.2f));
    //         if (upHit.collider != null)
    //         {
    //             upDetector = true;
    //             objectsAroundMe.Add(upHit.collider.gameObject);
    //         }

    //     }
    //     if (!downDetector)
    //     {
    //         RaycastHit2D downHit = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y - .6f), new Vector2(0, - .2f));
    //         if (downHit.collider != null)
    //         {
    //             downDetector = true;
    //             objectsAroundMe.Add(downHit.collider.gameObject);
    //         }
    //     }

    //     if (!leftDetector)
    //     {
    //         RaycastHit2D leftHit = Physics2D.Raycast(new Vector2(transform.position.x - .6f,transform.position.y), new Vector2(-.2f,0));
    //         if (leftHit.collider != null)
    //         {
    //             leftDetector = true;
    //             objectsAroundMe.Add(leftHit.collider.gameObject);
    //         }
    //     }

    //     if (!rightDetector)
    //     {
    //         RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(transform.position.x  + .6f,transform.position.y), new Vector2(.2f,0));
    //         if (rightHit.collider != null)
    //         {
    //             rightDetector = true;
    //             objectsAroundMe.Add(rightHit.collider.gameObject);
    //         }
    //     }
    // }
}
