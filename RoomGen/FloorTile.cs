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
    public bool isPulsing = false;
    private Color OgMaterialColour;
    private Material material;
    private Color startColour;
    private Color endColour;
    private float lastColorChangeTime;

    public float FadeDuration = 2f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        OgMaterialColour = GetComponent<Renderer>().material.color;

        currentGameLevel = GameObject.Find("GameManager").GetComponent<GameManager>().currentGameLevel;
        roomSprites = GameObject.Find("Room Sprites").GetComponent<RoomSprites>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        var r = Random.Range(0, roomSprites.floor.Length - 1);

        sr.sprite = roomSprites.floor[r];
        defaultSprite = sr.sprite;

        r = Random.Range(0, roomSprites.critFloor.Length - 1);

        critSprite = roomSprites.critFloor[r];

        SetFloorTileColor();

    }

    private IEnumerator CritFlash(float duration)
    {
        sr.sprite = critSprite;
        sr.color = Color.white;
        yield return new WaitForSeconds(duration);
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

        if (isPulsing)
        {
            var ratio = (Time.time - lastColorChangeTime) / FadeDuration;
            ratio = Mathf.Clamp01(ratio);

            // material.color = Color.Lerp( startColour, endColour, ratio );
            // material.color = Color.Lerp(startColour, endColour, Mathf.Sqrt(ratio)); // A cool effect
            material.color = Color.Lerp(startColour, endColour, ratio * ratio); // Another cool effect

            if (ratio == 1f)
            {
                lastColorChangeTime = Time.time;

                // Switch colors
                var temp = startColour;
                startColour = endColour;
                endColour = temp;
            }
        }
    }

    public void StartPulsingTiles()
    {
        isPulsing = true;
        material = GetComponent<Renderer>().material;
        startColour = defaultColor;
        // endColour = defaultColor += new Color(125.00f, 0.00f, 6.00f,1);
        endColour = defaultColor += new Color(124.00f, 2.00f, 2.00f, 1);
    }

    public void StopPulsingTiles()
    {
        isPulsing = false;
        GetComponent<Renderer>().material.color = OgMaterialColour;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col)
        {
            isTouchingOtherCollider = true;
        }
        if (col.tag == "Player" && isPulsing)
        {
            col.gameObject.GetComponent<Health>().TakeDamage(1, gameObject, "floorTile", true);
        }
    }

}
