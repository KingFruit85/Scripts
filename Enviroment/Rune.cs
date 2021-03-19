using UnityEngine;

public class Rune : MonoBehaviour
{
    public Sprite deactivatedSprite;
    public Sprite activatedSprite;
    public GameObject myUnlock;
    private SpriteRenderer SR;
    public int MyCode;

    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = deactivatedSprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SR.sprite = activatedSprite;
            myUnlock.GetComponent<Barrier>()
                    .SubmitCode(MyCode);
        }
    }

    public void ResetRune()
    {
        SR.sprite = deactivatedSprite;
    }

   
}
