using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool topWall;
    public bool bottomWall;
    public bool leftWall;
    public bool rightWall;
    public bool topLeftCorner;
    public bool topRightCorner;
    public bool bottomLeftCorner;
    public bool bottomRightCorner;
    private RoomSprites roomSprites;
    private SpriteRenderer sr;
    private int currentGameLevel;
    private GameManager gameManager;
    public Camera cam;
    public TextMeshPro text;

    public Sprite TopLeftWall;
    public Sprite TopRightWall;
    public Sprite BottomLeftWall;
    public Sprite BottomRightWall;
    public Sprite RightEndWall;
    public Sprite LeftEndWall;
    public Sprite TopEndWall;
    public Sprite BottomEndWall;



    public Sprite alternativeSprite;
    public Sprite defaultSprite;
    public Color defaultColor;
    public Sprite critSprite;

    public bool wallToLeft = false;
    public bool wallToRight = false;
    public bool wallToTop = false;
    public bool wallToBottom = false;

    public Vector2 Pos; 

    void Awake()
    {
        roomSprites = GameObject.Find("Room Sprites").GetComponent<RoomSprites>();
        sr = GetComponent<SpriteRenderer>();
        currentGameLevel = GameObject.Find("GameManager").GetComponent<GameManager>().currentGameLevel;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();


        UpdateTile();     
        SetWallTileColor();
    }

    public void UpdateTile()
    {
        if (topWall)
        {
			var r = Random.Range(0, (roomSprites.topWalls.Length-1));
            sr.sprite = roomSprites.topWalls[r];

            r = Random.Range(0, roomSprites.critTopWalls.Length-1);
            critSprite = roomSprites.critTopWalls[r];
        }

        if (bottomWall)
        {
			var r = Random.Range(0, roomSprites.bottomWalls.Length-1);
            sr.sprite = roomSprites.bottomWalls[r];

            r = Random.Range(0, roomSprites.critBottomWalls.Length-1);
            critSprite = roomSprites.critBottomWalls[r];
        }

        if (leftWall)
        {
			var r = Random.Range(0, roomSprites.leftWalls.Length-1);
            sr.sprite = roomSprites.leftWalls[r];

            r = Random.Range(0, roomSprites.critLeftWalls.Length-1);
            critSprite = roomSprites.critLeftWalls[r];
        }

        if (rightWall)
        {
			var r = Random.Range(0, roomSprites.rightWalls.Length-1);
            sr.sprite = roomSprites.rightWalls[r];

            r = Random.Range(0, roomSprites.critRightWalls.Length-1);
            critSprite = roomSprites.critRightWalls[r];
        }

        if (topLeftCorner)
        {
			var r = Random.Range(0, roomSprites.corners.Length-1);
            sr.sprite = roomSprites.corners[r];

            r = Random.Range(0, roomSprites.critTopLeftCorners.Length-1);
            critSprite = roomSprites.critTopLeftCorners[r];
        }

        if (topRightCorner)
        {
            var r = Random.Range(0, roomSprites.corners.Length-1);
            sr.sprite = roomSprites.corners[r];

			r = Random.Range(0, roomSprites.critTopRightCorners.Length-1);
            critSprite = roomSprites.critTopRightCorners[r];
        }

        if (bottomLeftCorner)
        {
            var r = Random.Range(0, roomSprites.corners.Length-1);
            sr.sprite = roomSprites.corners[r];

			r = Random.Range(0, roomSprites.critBottomLeftCorners.Length-1);
            critSprite = roomSprites.critBottomLeftCorners[r];
        }

        if (bottomRightCorner)
        {
            var r = Random.Range(0, roomSprites.corners.Length-1);
            sr.sprite = roomSprites.corners[r];

			r = Random.Range(0, roomSprites.critBottomRightCorners.Length-1);
            critSprite = roomSprites.critBottomRightCorners[r];
        }

        defaultSprite = sr.sprite;
    }

    //Perhaps just limit the sprite change to the current room?
    private IEnumerator CritFlash(float duration)
    {
        sr.sprite = critSprite;
        sr.color = Color.white;
        yield return new WaitForSeconds( duration );
        sr.color = defaultColor;
        sr.sprite = defaultSprite;
    }


    void Update()
    {
        // text.GetComponent<TextMeshPro>().text = transform.parent.parent.GetComponent<SimpleRoom>().WorldToArrayPOS(transform.localPosition).ToString();

        if (gameManager.playerHit)
        {
            StartCoroutine(CritFlash(.1f));
            
        }
    }

    public void setAltSprite()
    {
        sr.sprite = alternativeSprite;
    }

    public void setAltDefaultSprite()
    {
        sr.sprite = defaultSprite;
    }
    public void SetWallTileColor()
    {
            // sr.color = gameManager.LevelWallBaseColor;
            sr.color = Color.white;

            defaultColor = sr.color;
            cam.backgroundColor = gameManager.LevelWallBaseColor;
    }

    public void SetTileSprite(Vector3 tilePosition)
    {
        var room = transform.parent.parent.GetComponent<SimpleRoom>();

        Pos = room.WorldToArrayPOS(tilePosition);

        // Check if there is a wall above
        if (room.GetTileContents(Pos.x,(Pos.y - 1)) == '#')
        {
            wallToTop = true;
        }

        // Check if there is a wall below
        if (room.GetTileContents(Pos.x,(Pos.y + 1)) == '#')
        {
            wallToBottom = true;
        }
        
        // Check if there is a wall left
        if (room.GetTileContents((Pos.x - 1),Pos.y) == '#')
        {
            wallToLeft = true;
        }

        // Check if there is a wall right
        if (room.GetTileContents((Pos.x + 1),Pos.y) == '#')
        {
            wallToRight = true;
        }

        if(wallToRight && wallToBottom) sr.sprite = TopLeftWall;
        if(wallToLeft && wallToBottom) sr.sprite = TopRightWall;
        if(wallToTop && wallToRight) sr.sprite = BottomLeftWall;
        if(wallToTop && wallToLeft) sr.sprite = BottomRightWall;
        if(wallToLeft && !wallToRight && !wallToBottom & !wallToTop) sr.sprite = RightEndWall;
        if(wallToRight && !wallToLeft && !wallToBottom & !wallToTop) sr.sprite = LeftEndWall;
        if(wallToTop && !wallToLeft && !wallToBottom & !wallToRight) sr.sprite = BottomEndWall;
        if(wallToBottom && !wallToLeft && !wallToTop & !wallToRight ) sr.sprite = TopEndWall;




    }

}