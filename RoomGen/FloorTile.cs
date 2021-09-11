using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    private RoomSprites roomSprites;
    [SerializeField]
    private int currentGameLevel;
    private SpriteRenderer sr;
    private GameManager gameManager;

    public Sprite defaultSprite;
    public Color defaultColor;

    public Sprite critSprite;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        currentGameLevel = GameObject.Find("GameManager").GetComponent<GameManager>().currentGameLevel;
        roomSprites = GameObject.Find("Room Sprites").GetComponent<RoomSprites>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        var r = Random.Range(0,roomSprites.floor.Length - 1);
        
        sr.sprite = roomSprites.floor[r];
        defaultSprite = sr.sprite;
        
        r = Random.Range(0, roomSprites.critFloor.Length -1);

        // critSprite = roomSprites.critFloor[r];

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
        sr.color = gameManager.LevelFloorBaseColor;
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
    }
}
