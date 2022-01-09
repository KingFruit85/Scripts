using UnityEngine;

public class Rune : MonoBehaviour
{
    public Sprite deactivatedSprite;
    public Sprite activatedSprite;
    public GameObject myUnlock;
    public FlameBowl flameBowl;
    public Barrier[] barriers;
    private SpriteRenderer SR;
    public string MyCode;
    public GameObject myTrap;

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

        if (flameBowl && other.tag == "Player")
        {
            flameBowl.Light();
        }
    }

    void UnlockBarriers()
    {
        if (barriers != null || barriers.Length > 0)
        {
            foreach (var barrier in barriers)
            {
                Destroy(barrier);
            }
        }
    }

    public void ResetRune()
    {
        SR.sprite = deactivatedSprite;
    }

   
}
