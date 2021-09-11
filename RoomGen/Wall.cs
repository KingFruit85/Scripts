using System.Collections;
using System.Collections.Generic;
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
    public bool miscWall;
    public bool hasToggleSprite;
    public GameObject[] attachedTraps;

    private RoomSprites roomSprites;
    private SpriteRenderer sr;
    private int currentGameLevel;
    private GameManager gameManager;
    public Camera cam;

    public Sprite alternativeSprite;
    public Sprite defaultSprite;
    public Color defaultColor;
    public Sprite critSprite;


    void Awake()
    {
        roomSprites = GameObject.Find("Room Sprites").GetComponent<RoomSprites>();
        sr = GetComponent<SpriteRenderer>();
        currentGameLevel = GameObject.Find("GameManager").GetComponent<GameManager>().currentGameLevel;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

        UpdateTile();

        // if (miscWall)
        // {
        //     var r = Random.Range(0, roomSprites.miscWalls.Length-1);
        //     sr.sprite = roomSprites.corners[r];
        // }

        

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
            sr.color = gameManager.LevelWallBaseColor;
            defaultColor = sr.color;
            cam.backgroundColor = gameManager.LevelWallBaseColor;
    }

}