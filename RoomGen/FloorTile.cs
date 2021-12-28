using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    private RoomSprites roomSprites;
    [SerializeField]
    private int currentGameLevel;
    private SpriteRenderer sr;
    private BoxCollider2D col;
    private GameManager gameManager;
    public bool isTouchingOtherCollider = false;
    public Sprite defaultSprite;
    public Color defaultColor;
    public Sprite critSprite;
    public TextMeshPro text;



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
        // text.GetComponent<TextMeshPro>().text = transform.parent.parent.GetComponent<SimpleRoom>().WorldToArrayPOS(transform.localPosition).ToString();

        if (gameManager.playerHit)
        {
            CriticalFlash();
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col)
        {
            isTouchingOtherCollider = true; 
        }
    }

}
