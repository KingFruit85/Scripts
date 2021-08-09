using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{   
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;


    //Create a Damage Popup
    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCrit)
    {
        Vector3 newPOS = new Vector3(position.x + .4f, position.y + .5f);

        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup,
                                                     newPOS,
                                                     Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCrit);
        return damagePopup;
    }

    void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCrit)
    {
        if (isCrit)
        {
            textMesh.color = Color.red; 
        textMesh.SetText(damageAmount.ToString() + " (critical!)");

            isCrit = false;
        }
        else
        {
            textColor = textMesh.color;
            textMesh.SetText(damageAmount.ToString());

        }

        disappearTimer = 1f;
    }

    private void Update()
    {
        float moveYSpeed = 2.0f;
        transform.position += new Vector3(0,moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            // Start disappearing
            float disappearSpeed = 2f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
